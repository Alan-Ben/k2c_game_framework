package CrossTeamServer.CrossTeam;

import ALBasicServer.ALBasicMutex.MutexObject;
import CTSDB.Bo.CrossTeamBO;
import Common.CrossTeamEnum.ENPCrossTeamJoinType;
import Common.CrossTeamEnum.ENPCrossTeamMemberPos;
import Common.CrossTeamObj.CrossTeam_BaseInfo;
import Common.CrossTeamObj.CrossTeam_SetInfo_Join;
import Common.ServerObj.ServerObj_ActivityTeamGroup;
import CrossTeamServer.CrossTeamServer;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.HashMap;

/**
 * 队伍数据管理器
 */
public class CrossTeamMgr
{
    //分组实例对象
    private CrossGroup _m_group;

    //队伍列表
    private ArrayList<CrossTeamInfo> _m_alTeamList;

    //已经加入队伍的玩家集合
    private HashMap<Long, CrossTeamInfo> _m_hmPlayerTeamMap;

    //锁对象
    private MutexObject _m_mutex;

    public CrossTeamMgr(CrossGroup _group)
    {
        _m_group = _group;

        _m_alTeamList = new ArrayList<>();

        _m_hmPlayerTeamMap = new HashMap<>();

        _m_mutex = new MutexObject();
    }

    protected void _lock() {_m_mutex.lock();}
    protected void _unlock() {_m_mutex.unlock();}

    public CrossGroup getGroup() {return _m_group;}

    protected void _initFromDB(CrossTeamBO _bo)
    {
        CrossTeamInfo info = new CrossTeamInfo(_m_group, _bo);
        _m_alTeamList.add(info);

        //启动队伍聊天房间
        info._startRegChatRoom();
    }

    /**
     * 队伍数据初始化完成后调用，对申请列表进行排序
     */
    protected void _onInited()
    {
        for(int i = 0; i < _m_alTeamList.size(); i++)
        {
            CrossTeamInfo info = _m_alTeamList.get(i);
            if(null == info)
                continue;

            //队伍数据初始化完成后调用，对申请列表进行排序
            info._onInited();
        }
    }

    /**
     * 获取所有队伍列表
     * @return
     */
    public ArrayList<CrossTeamInfo> getAllTeamList()
    {
        _lock();

        try
        {
            return new ArrayList<>(_m_alTeamList);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取指定US是队长的队伍列表
     * @param _usId
     * @param _teamList
     */
    public void makeUsLeaderTeamList(int _usId, ArrayList<ServerObj_ActivityTeamGroup> _teamList)
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alTeamList.size(); i++)
            {
                CrossTeamInfo team = _m_alTeamList.get(i);
                if(null == team)
                    continue;

                long leaderCid = team.getMemberMgr().getLeaderCid();
                int leaderUsId = CommonFunc.parseServerTypeIdFromCid(leaderCid);
                if(leaderUsId != _usId)
                    continue;

                ServerObj_ActivityTeamGroup obj = new ServerObj_ActivityTeamGroup();
                obj.setTeamId(team.getTeamId());
                obj.setLeaderCid(leaderCid);

                _teamList.add(obj);
            }
        }
        finally
        {
            _unlock();
        }
    }

    // 每页最多返回数量，服务端固定，不对外开放
    private static final int PAGE_SIZE = 50;

    /**
     * 获取队伍总数量
     * @return 队伍总数
     */
    public int getTotalCount()
    {
        _lock();
        try
        {
            return _m_alTeamList.size();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取指定页的队伍基础信息列表
     * @param _page     页码（从1开始）
     * @param _teamList 填充目标列表
     */
    public void makeTeamListByPage(int _page, ArrayList<CrossTeam_BaseInfo> _teamList)
    {
        if (_page < 1)
            _page = 1;

        int startIdx = (_page - 1) * PAGE_SIZE;

        _lock();
        try
        {
            for (int i = startIdx; i < _m_alTeamList.size() && i < startIdx + PAGE_SIZE; i++)
            {
                CrossTeamInfo team = _m_alTeamList.get(i);
                if (null == team)
                    continue;
                _teamList.add(team.toBaseInfo());
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 查找队伍
     * @param _teamId
     * @return
     */
    public CrossTeamInfo lookup(long _teamId)
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alTeamList.size(); i++)
            {
                CrossTeamInfo team = _m_alTeamList.get(i);
                if(null == team)
                    continue;

                if(team.getTeamId() == _teamId)
                    return team;
            }

            return null;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 创建队伍
     */
    public CrossTeamInfo create(int _teamIdx, long _cid, int _memberLimit,
                                ENPCrossTeamJoinType _joinType, CrossTeam_SetInfo_Join _joinCond,
                                String _teamName, String _teamDec)
    {
        _lock();

        try
        {
            if(isInTeam(_cid))
                return null;

            //创建队伍数据，加入方式和申请条件分别存储到独立字段
            CrossTeamBO bo = new CrossTeamBO();
            bo.setTeamId(CrossTeamServer.getInstance().getBM(), CommonFunc.makeActivityTeamId(_m_group.getGroupId(), _teamIdx));
            bo.setMemberLimit(CrossTeamServer.getInstance().getBM(), _memberLimit);
            bo.setJoinType(CrossTeamServer.getInstance().getBM(), _joinType.ordinal());
            bo.setJoinCondType(CrossTeamServer.getInstance().getBM(), _joinCond.getCond().ordinal());
            bo.setJoinCondValue(CrossTeamServer.getInstance().getBM(), _joinCond.getValue());
            bo.setTeamName(CrossTeamServer.getInstance().getBM(), _teamName);
            bo.setTeamDec(CrossTeamServer.getInstance().getBM(), _teamDec);
            bo.insert(CrossTeamServer.getInstance().getBM());

            CrossTeamInfo team = new CrossTeamInfo(_m_group, bo);
            _m_alTeamList.add(team);

            //创建队长数据
            team.getMemberMgr()._addMember(_cid, ENPCrossTeamMemberPos.LEADER);

            //删除该玩家所有的队伍申请
            getGroup().getPlayerApplyMgr().removePlayerAllApply(_cid);
            //增加玩家-队伍映射
            getGroup().getTeamMgr().addPlayerTeamMap(_cid, team);

            return team;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     *
     * @param _cid
     * @return
     */
    public CrossTeamInfo getPlayerTeam(long _cid)
    {
        _lock();

        try
        {
            return _m_hmPlayerTeamMap.get(_cid);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 检查玩家是否已经在队伍中（不加锁，仅供已持锁状态下调用）
     * @param _cid
     * @return
     */
    protected boolean _isInTeamNoLock(long _cid)
    {
        return _m_hmPlayerTeamMap.containsKey(_cid);
    }

    /**
     * 检查玩家是否已经在队伍中
     * @param _cid
     * @return
     */
    public boolean isInTeam(long _cid)
    {
        _lock();

        try
        {
            return _m_hmPlayerTeamMap.containsKey(_cid);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 增加玩家和队伍的映射关系
     * @param _cid
     * @param _team
     */
    public void addPlayerTeamMap(long _cid, CrossTeamInfo _team)
    {
        _lock();

        try
        {
            if(!_m_alTeamList.contains(_team))
                return;

            _m_hmPlayerTeamMap.put(_cid, _team);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除玩家和队伍的映射关系
     * @param _cid
     * @param _team
     */
    public void removePlayerTeamMap(long _cid, CrossTeamInfo _team)
    {
        _lock();

        try
        {
            if(!_m_alTeamList.contains(_team))
                return;

            _m_hmPlayerTeamMap.remove(_cid);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 解散队伍
     * @param _teamId
     */
    public void dissolve(long _teamId)
    {
        _lock();

        try
        {
            for(int i = 0; i < _m_alTeamList.size(); i++)
            {
                CrossTeamInfo team = _m_alTeamList.get(i);
                if(null == team)
                    continue;

                if(team.getTeamId() == _teamId)
                {
                    _m_alTeamList.remove(i);
                    team._onDissolve();
                    break;
                }
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 销毁数据
     */
    protected void _onDiscard()
    {
        _lock();

        try
        {
            //销毁所有队伍数据
            for(int i = 0; i < _m_alTeamList.size(); i++)
            {
                CrossTeamInfo teamInfo = _m_alTeamList.get(i);
                if(null == teamInfo)
                    continue;

                //销毁队伍数据
                teamInfo._onDiscard();
            }
            //清空队伍列表
            _m_alTeamList.clear();
        }
        finally
        {
            _unlock();
        }
    }
}
