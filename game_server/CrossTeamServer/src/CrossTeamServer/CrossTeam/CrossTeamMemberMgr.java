package CrossTeamServer.CrossTeam;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import CTSDB.Bo.CrossTeamMemberBO;
import Common.CrossTeamEnum.ENPCrossTeamMemberPos;
import Common.CrossTeamObj.CrossTeamMember_Info;
import Common.ServerObj.ServerObj_GLSTeamMember;
import CrossTeamServer.CrossTeamServer;
import GS2GC.p012_ActivityTeamOp.GS2GC_012_054_OnActivityTeamDissolve;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CommonFunc;
import RPC._ARPCData;
import RPC._ARpcCallBack;

import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * 队伍成员管理器
 */
public class CrossTeamMemberMgr
{
    //所属队伍数据
    private CrossTeamInfo _m_team;

    //队伍成员数据
    private ArrayList<CrossTeamMemberInfo> _m_alMemberList;

    //锁对象
    private MutexAtom _m_mutex;

    public CrossTeamMemberMgr(CrossTeamInfo _team)
    {
        _m_team = _team;

        _m_alMemberList = new ArrayList<>();

        _m_mutex = new MutexAtom();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    protected void _initMemberFromDB(CrossTeamMemberBO _bo)
    {
        CrossTeamMemberInfo member = new CrossTeamMemberInfo(_m_team, _bo);
        _m_alMemberList.add(member);
    }

    /**
     * 构造成员协议列表
     * @param _list
     */
    public void makeProto(ArrayList<CrossTeamMember_Info> _list)
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alMemberList.size(); i++)
            {
                CrossTeamMemberInfo member = _m_alMemberList.get(i);
                if(null == member)
                    continue;

                _list.add(member.toProto());
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 构造 GLS GroupMgr 所需的轻量成员列表，每个成员只包含 cid
     * @param _list 目标列表
     */
    public void makeGLSMemberList(ArrayList<ServerObj_GLSTeamMember> _list)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_alMemberList.size(); i++)
            {
                CrossTeamMemberInfo member = _m_alMemberList.get(i);
                if (null == member)
                    continue;

                ServerObj_GLSTeamMember memberData = new ServerObj_GLSTeamMember();
                memberData.setCid(member.getMemberCid());
                _list.add(memberData);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 检查队伍是否已满员
     * @return
     */
    public boolean isFull()
    {
        _lock();

        try
        {
            return _m_team.getMemberLimit() > 0 && _m_alMemberList.size() >= _m_team.getMemberLimit();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取队长Cid
     * @return
     */
    public long getLeaderCid()
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alMemberList.size(); i++)
            {
                CrossTeamMemberInfo member = _m_alMemberList.get(i);
                if(null == member)
                    continue;

                if(member.getMemberPos() == ENPCrossTeamMemberPos.LEADER)
                    return member.getMemberCid();
            }

            return 0L;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 查找成员
     * @param _memberId
     * @return
     */
    public CrossTeamMemberInfo lookup(long _memberId)
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alMemberList.size(); i++)
            {
                CrossTeamMemberInfo member = _m_alMemberList.get(i);
                if(null == member)
                    continue;

                if(member.getMemberCid() == _memberId)
                    return member;
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 查找队长成员
     * @return
     */
    public CrossTeamMemberInfo lookupLeader()
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alMemberList.size(); i++)
            {
                CrossTeamMemberInfo member = _m_alMemberList.get(i);
                if(null == member)
                    continue;

                if(member.getMemberPos() == ENPCrossTeamMemberPos.LEADER)
                    return member;
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 判断玩家是否是队长
     * @param _cid
     * @return
     */
    public boolean isLeader(long _cid)
    {
        CrossTeamMemberInfo member = lookup(_cid);
        if(null == member)
            return false;

        return member.getMemberPos() == ENPCrossTeamMemberPos.LEADER;
    }

    /**
     * 添加普通成员
     * @param _memberCid
     * @return
     */
    public CrossTeamMemberInfo addMember(long _memberCid)
    {
        CrossTeamMemberInfo member = null;

        _lock();
        try
        {
            // 检查玩家是否已在队伍中（调用无锁版本，此时未持有 CrossTeamMgr 锁，无死锁风险）
            if (_m_team.getGroup().getTeamMgr()._isInTeamNoLock(_memberCid))
                return null;

            // 内联满员检查，避免调用 isFull() 在 memberMgr 锁内再次加锁
            if (_m_team.getMemberLimit() > 0 && _m_alMemberList.size() >= _m_team.getMemberLimit())
                return null;

            member = _addMember(_memberCid, ENPCrossTeamMemberPos.NONE);
        }
        finally
        {
            _unlock();
        }

        // memberMgr 锁外执行：申请清理、映射关系、变更通知
        if (null != member)
        {
            //删除该玩家所有的队伍申请
            _m_team.getGroup().getPlayerApplyMgr().removePlayerAllApply(member.getMemberCid());
            //增加玩家-队伍映射
            _m_team.getGroup().getTeamMgr().addPlayerTeamMap(_memberCid, _m_team);
            _onMemberChg();
        }

        return member;
    }

    /**
     * 添加指定职位成员
     * @param _memberCid
     * @param _memberPos
     * @return
     */
    protected CrossTeamMemberInfo _addMember(long _memberCid, ENPCrossTeamMemberPos _memberPos)
    {
        CrossTeamMemberBO bo = new CrossTeamMemberBO();
        bo.setTeamId(_m_team.getBM(), _m_team.getTeamId());
        bo.setMemberCid(_m_team.getBM(), _memberCid);
        bo.setMemberPos(_m_team.getBM(), _memberPos.ordinal());
        bo.setJoinMs(_m_team.getBM(), CommonFunc.getNowTimeMS());
        bo.insert(_m_team.getBM());

        CrossTeamMemberInfo member = new CrossTeamMemberInfo(_m_team, bo);
        _m_alMemberList.add(member);

        return member;
    }

    /**
     * 向队长US发送RPC请求，只返回请求结果，不返回RPC响应数据
     * @param _rpc
     * @param _callback
     * @param <T>
     */
    public <T extends _ARPCData> void rpc2LeaderUs(T _rpc, _ICallBackResult _callback)
    {
        int leaderUs = CommonFunc.parseServerTypeIdFromCid(getLeaderCid());
        CrossTeamServer.getInstance().rpc2us().requestToRepeat(leaderUs, _rpc, new _ARpcCallBack<T>()
        {
            @Override
            public void call_back(int _errCode, T _rpc)
            {
                if(_errCode > 0)
                {
                    if(null != _callback)
                        _callback.onRunOver(Result.failed(_errCode));
                    return;
                }

                if(null != _callback)
                    _callback.onRunOver(Result.SUCC);
            }
        }, 10, () ->
        {
            CommLog.error("CrossTeamMemberMgr rpc2LeaderUs fail, rpc:{} leaderUs:{}", _rpc.getClass().getName(), leaderUs);
        });
    }

    /**
     * 移除成员
     * @param _memberCid
     */
    public CrossTeamMemberInfo removeMember(long _memberCid)
    {
        CrossTeamMemberInfo member = null;

        _lock();

        try
        {
            // 内联查找，避免调用 lookup() 重复加锁导致死锁
            for (int i = 0; i < _m_alMemberList.size(); i++)
            {
                CrossTeamMemberInfo m = _m_alMemberList.get(i);
                if (null != m && m.getMemberCid() == _memberCid)
                {
                    member = m;
                    break;
                }
            }
            if (null == member)
                return null;

            _m_alMemberList.remove(member);
            member._discard();
        }
        finally
        {
            _unlock();
        }

        //移除玩家-队伍映射
        _m_team.getGroup().getTeamMgr().removePlayerTeamMap(_memberCid, _m_team);

        // 通知成员变更
        _onMemberChg();

        return member;
    }

    /**
     * 广播消息给队伍成员
     * @param _proto
     */
    public void broadcastMsg(_IALProtocolStructure _proto)
    {
        broadcastMsg(_proto.makeFullPackage(), 0L);
    }

    /**
     *
     * @param _proto
     * @param _excludeCid
     */
    public void broadcastMsg(_IALProtocolStructure _proto, long _excludeCid)
    {
        broadcastMsg(_proto.makeFullPackage(), _excludeCid);
    }

    /**
     * 广播消息给队伍成员，可排除指定玩家
     * @param _proto       已序列化的消息
     * @param _excludeCid  排除的玩家CID，0表示不排除
     */
    public void broadcastMsg(ByteBuffer _proto, long _excludeCid)
    {
        ArrayList<Long> cidList = new ArrayList<>();

        _lock();
        try
        {
            for (int i = 0; i < _m_alMemberList.size(); i++)
            {
                CrossTeamMemberInfo member = _m_alMemberList.get(i);
                if (null == member)
                    continue;
                if (_excludeCid != 0 && member.getMemberCid() == _excludeCid)
                    continue;

                cidList.add(member.getMemberCid());
            }
        }
        finally
        {
            _unlock();
        }

        CrossTeamServer.getInstance().broadMsg2GC(cidList, _proto);
    }

    /**
     * 队伍解散时调用
     */
    protected void _onDissolve()
    {
        _lock();

        try
        {
            if (_m_alMemberList.isEmpty())
                return;

            // 删除成员数据库记录
            _m_team.getBM().getBM(CrossTeamMemberBO.class).delAll("team_id", _m_team.getTeamId());

            // 收集全员 CID，整批广播解散消息
            ArrayList<Long> cidList = new ArrayList<>();
            for (int i = 0; i < _m_alMemberList.size(); i++)
            {
                CrossTeamMemberInfo member = _m_alMemberList.get(i);
                if (null != member)
                    cidList.add(member.getMemberCid());
            }

            _m_alMemberList.clear();

            GS2GC_012_054_OnActivityTeamDissolve proto = new GS2GC_012_054_OnActivityTeamDissolve();
            proto.setTeamId(_m_team.getTeamId());
            CrossTeamServer.getInstance().broadMsg2GC(cidList, proto);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 成员数据变更时调用，通知队长所在 US 同步 Group 数据
     */
    private void _onMemberChg()
    {
        _m_team.notifyGroupSync();
    }
}
