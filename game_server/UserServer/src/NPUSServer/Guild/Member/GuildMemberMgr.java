
package NPUSServer.Guild.Member;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.US_Service.Guild._AGuildBroadCastRPC;
import Common.GuildEnum.EGuildBoxType;
import Common.GuildEnum.EGuildPositionType;
import Common.GuildObj.Guild_MemberBaseInfo;
import Common.GuildObj.Guild_MemberContributeInfo;
import Common.GuildObj.Guild_MemberEntrustInfo;
import Common.MailObj.Mail_Data;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import GS2GC.p032_GuildOp.GS2GC_032_050_OnGuildShowInfoChg;
import GS2GC.p032_GuildOp.GS2GC_032_051_OnMemberBaseInfoChg;
import GS2GC.p032_GuildOp.GS2GC_032_054_OnGuildMemberAdd;
import GS2GC.p032_GuildOp.GS2GC_032_055_OnGuildMemberRemove;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBack;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Guild.RefGuildLevel;
import NPGameRes.Refs.Guild.RefGuildPosition;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Msg.GuildLogFunc;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.GuildMemberBO;
import USLOGDB.Bo.LogGuildPositionChgBO;

import java.util.ArrayList;
import java.util.List;
import java.util.function.Predicate;

public class GuildMemberMgr
{
    private GuildInfo _m_guildInfo;
    private List<GuildMemberInfo> _m_memberList;
    //盟主
    private GuildMemberInfo _m_leader;
    //职位人数统计
    private int[] _m_positionCount;
    //数据容器锁
    private MutexAtom _m_listMutex;
    //检索继任者标记
    private boolean _m_findSuccessor;

    public GuildMemberMgr(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
        _m_memberList = new ArrayList<>();
        _m_positionCount = new int[EGuildPositionType.values().length];
        _m_listMutex = new MutexAtom();
        _m_findSuccessor = true;
    }

    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    public int getMemberCount()
    {
        return _m_memberList.size();
    }

    private void _lock()
    {
        _m_listMutex.lock();
    }

    private void _unlock()
    {
        _m_listMutex.unlock();
    }

    public long getLeaderId()
    {
        GuildMemberInfo leader = _m_leader;
        return leader == null ? 0 : leader.getCid();
    }

    public GuildMemberInfo getLeader()
    {
        return _m_leader;
    }

    /**
     * 获取还可以加入的玩家数量
     * @return
     */
    public int getCanJoinNum()
    {
        return _m_guildInfo.getMaxMemberCount() - getMemberCount();
    }

    /**
     * 初始化成员对象
     */
    public void initMember(GuildMemberBO _bo)
    {
        GuildMemberInfo memberInfo = new GuildMemberInfo(this, _bo);
        _m_memberList.add(memberInfo);

        //统计职位人数
        _m_positionCount[memberInfo.getPosition().ordinal()]++;

        //设置盟主
        if (memberInfo.getPosition() == EGuildPositionType.LEADER)
            _m_leader = memberInfo;
    }

    /**
     * 查询成员数据
     * @param _cid 角色ID
     * @return
     */
    public GuildMemberInfo lookup(long _cid)
    {
        _lock();
        try
        {
            for (GuildMemberInfo member : _m_memberList)
            {
                if (member.getCid() == _cid)
                {
                    return member;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查询成员数据
     * @param _predicate 过滤条件
     * @return
     */
    public List<GuildMemberInfo> lookup(Predicate<GuildMemberInfo> _predicate)
    {
        _lock();
        try
        {
            List<GuildMemberInfo> memberList = new ArrayList<>();
            for (GuildMemberInfo member : _m_memberList)
            {
                if (_predicate.test(member))
                {
                    memberList.add(member);
                }
            }
            return memberList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取成员列表
     * @return
     */
    public List<Long> getMemberCidList(Predicate<GuildMemberInfo> _predicate)
    {
        _lock();
        try
        {
            List<Long> cidList = new ArrayList<>();
            for (GuildMemberInfo _member : _m_memberList)
            {
                //过滤条件
                if (_predicate != null && !_predicate.test(_member))
                    continue;

                cidList.add(_member.getCid());
            }
            return cidList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 添加成员
     * @param _cid
     */
    public Result addMember(long _cid, EGuildPositionType _positionType)
    {
        GuildMemberInfo guildMemberInfo;
        _lock();
        try
        {
            //检查是否人数达到上限
            if (getMemberCount() >= _m_guildInfo.getMaxMemberCount())
                return GuildErr.GUILD_MEMBER_NUM_REACH_LIMIT;

            //不允许任命多个盟主
            if (_positionType == EGuildPositionType.LEADER && _m_leader != null)
                return GuildErr.DONT_HAVE_PERMISSION;

            BM bmObj = _m_guildInfo.getGuildMgr().getServer().getBM();

            GuildMemberBO bo = new GuildMemberBO();
            bo.setGuildId(bmObj, _m_guildInfo.getGuildId());
            bo.setCid(bmObj, _cid);
            bo.setPosition(bmObj, _positionType.ordinal());
            bo.setLastReportOnlineTimestamp(bmObj, CommonFunc.getNowTimeMS());
            bo.insert(bmObj);

            guildMemberInfo = new GuildMemberInfo(this, bo);
            _m_memberList.add(guildMemberInfo);

            _m_positionCount[EGuildPositionType.MEMBER.ordinal()]++;

            //设置盟主
            if (_positionType == EGuildPositionType.LEADER)
                _m_leader = guildMemberInfo;

            _m_findSuccessor = true;
        } finally
        {
            _unlock();
        }
        
        //标记联盟宝箱
        _m_guildInfo.getGuildBoxMgr()._updateMemberMaxInstanceId(guildMemberInfo);

        //广播消息
        broadcastMsg(new GS2GC_032_054_OnGuildMemberAdd(guildMemberInfo.makeProto()),
                _memberInfo -> _memberInfo.getCid() != _cid);

        return Result.SUCC;
    }

    /**
     * 广播邮件
     * @param _mailData
     * @param _context
     */
    public void broadcastMail(Mail_Data _mailData, NPPlayerContext _context)
    {
        //获取成员列表
        List<Long> memberCidList = getMemberCidList(null);
        if (memberCidList.isEmpty())
            return;

        ALSynTaskManager.getInstance().regTask(() ->
        {
            for (Long cid : memberCidList)
            {
                MailSystem.addMail(_m_guildInfo.getGuildMgr().getServer(), cid, _mailData, _context);
            }
        });
    }

    /**
     * 广播协议
     * @param _proto
     */
    public void broadcastMsg(_IALProtocolStructure _proto)
    {
        broadcastMsg(_proto, null);
    }

    /**
     * 广播消息
     * @param _proto     协议
     * @param _predicate 过滤条件
     */
    public void broadcastMsg(_IALProtocolStructure _proto, Predicate<GuildMemberInfo> _predicate)
    {
        //获取成员列表
        List<Long> memberCidList = getMemberCidList(_predicate);
        if (memberCidList.isEmpty())
            return;

        ALSynTaskManager.getInstance().regTask(() ->
        {
            for (Long cid : memberCidList)
            {
                getGuildInfo().getGuildMgr().getServer().sendMsgToGC(cid, _proto);
            }
        });
    }

    /********
     * 向所有成员广播RPC消息
     * @param _rpc
     * @param <T>
     */
    public <T extends _AGuildBroadCastRPC> void broadcastRPC(T _rpc)
    {
        broadcastRPC(_rpc, null, null);
    }
    public <T extends _AGuildBroadCastRPC> void broadcastRPC(T _rpc, Predicate<GuildMemberInfo> _predicate, final _ICallBack _failCallback)
    {
        broadcastRPC(_rpc, null, null, 3, _failCallback);
    }
    public <T extends _AGuildBroadCastRPC> void broadcastRPC(T _rpc, Predicate<GuildMemberInfo> _predicate, final _ARpcCallBack<T> _callback, int _retryNum, final _ICallBack _failCallback)
    {
        //获取成员列表
        List<Long> memberCidList = getMemberCidList(_predicate);
        if (memberCidList.isEmpty())
            return;

        ALSynTaskManager.getInstance().regTask(() ->
        {
            for (Long cid : memberCidList)
            {
                int memberUSId = CommonFunc.parseServerTypeIdFromCid(cid);

                //构造发送协议对象
                T sendRPC = (T)_rpc.cloneRPCForCid(cid);

                //发送RPC处理
                getGuildInfo().getGuildMgr().getServer().rpc2us().requestToRepeat(memberUSId, sendRPC
                    , _callback
                    , _retryNum
                    , _failCallback);
            }
        });
    }

    /**
     * 构造成员信息
     * @return
     */
    public List<Guild_MemberBaseInfo> makeProtoMemberList()
    {
        _lock();
        try
        {
            List<Guild_MemberBaseInfo> memberList = new ArrayList<>();
            for (GuildMemberInfo memberInfo : _m_memberList)
            {
                memberList.add(memberInfo.makeProto());
            }
            return memberList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 转让会长
     * @param _newLeaderCid
     * @return
     */
    public Result transLeader(long _newLeaderCid,NPPlayerContext _context)
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();
        //判断上次转让的时间是否超过要求时间
        long lastProactiveTransLeaderTimeMs = _m_guildInfo.getLastProactiveTransLeaderTimeMs();
        if (nowTimeMS - lastProactiveTransLeaderTimeMs < (long) RefGeneral.Ref().guild_leader_proactive_transfer_cd_hours * 3600 * 1000)
            return GuildErr.GUILD_LEADER_PROACTIVE_TRANSFER_CD;

        GuildMemberInfo oriLeader;
        _lock();
        try
        {
            //查询新盟主信息
            GuildMemberInfo newLeaderInfo = lookup(_newLeaderCid);
            if (newLeaderInfo == null)
                return GuildErr.MEMBER_NOT_FOUND;

            //判断是否已经是副盟主
            if (newLeaderInfo.getPosition() != EGuildPositionType.DEPUTY_LEADER)
                return GuildErr.MEMBER_NOT_DEPUTY_LEADER;

            //判断是否有盟主
            if (_m_leader == null)
                return GuildErr.LEADER_NOT_FOUND;

            //查询配置
            RefGuildPosition refPosition = RefGuildPosition.getMgr().get(EGuildPositionType.LEADER.ordinal());
            if (refPosition == null)
                return CommErr.REF_NOT_FOUND;

            //判断贡献是否足够
            if (newLeaderInfo.getTotalContribution() < refPosition.trans_need_historical_contributions)
                return GuildErr.GUILD_CONTRIBUTION_NOT_ENOUGH;

            //查询配置
            RefGuildPosition refMemberPosition = RefGuildPosition.getMgr().get(newLeaderInfo.getPosition().ordinal());
            if (refMemberPosition == null)
                return CommErr.REF_NOT_FOUND;

            //把新盟主修改成盟主（操作者为原盟主）
            _chgPosition(newLeaderInfo, refPosition, _m_leader.getCid(), _context);
            //原盟主职位修改（操作者为原盟主自己）
            _chgPosition(_m_leader, refMemberPosition, _m_leader.getCid(), _context);

            oriLeader = _m_leader;
            _m_leader = newLeaderInfo;

            //盟主变动所以需要把联盟信息也推一遍
            broadcastMsg(new GS2GC_032_050_OnGuildShowInfoChg(_m_guildInfo.makeShowInfo()));
        } finally
        {
            _unlock();
        }

        GuildLogFunc.sendLeaderTransferLog(oriLeader.getCid(), _newLeaderCid, getGuildInfo());

        //记录转让时间
        _m_guildInfo.setLastProactiveTransLeaderTimeMs(nowTimeMS);

        return Result.SUCC;
    }

    /**
     * GM命令强制转让盟主
     *
     * 执行流程：
     * 1. 校验新盟主是否存在
     * 2. 直接修改职位，绕过所有业务规则检查（CD、副盟主、贡献度等）
     *
     * @param _newLeaderCid 新盟主CID
     * @param _context 操作上下文
     * @return 转让结果
     *
     * 线程安全：需要在调用方的锁保护下执行
     */
    public Result forceTransLeader(long _newLeaderCid, NPPlayerContext _context)
    {
        long oriLeaderCid = 0;
        _lock();
        try
        {
            // 查询新盟主信息
            GuildMemberInfo newLeaderInfo = lookup(_newLeaderCid);
            if (newLeaderInfo == null)
                return GuildErr.MEMBER_NOT_FOUND;

            // 判断是否有盟主
            if (_m_leader == null)
                return GuildErr.LEADER_NOT_FOUND;

            // 如果已经是盟主，直接返回成功
            if (_m_leader.getCid() == _newLeaderCid)
                return Result.SUCC;

            // 查询盟主职位配置
            RefGuildPosition refLeaderPosition = RefGuildPosition.getMgr().get(EGuildPositionType.LEADER.ordinal());
            if (refLeaderPosition == null)
                return CommErr.REF_NOT_FOUND;

            // 查询新盟主当前职位配置（用于将原盟主降为该职位）
            RefGuildPosition refMemberPosition = RefGuildPosition.getMgr().get(newLeaderInfo.getPosition().ordinal());
            if (refMemberPosition == null)
                return CommErr.REF_NOT_FOUND;

            // 把新盟主修改成盟主（系统操作，operatorCid为0）
            _chgPosition(newLeaderInfo, refLeaderPosition, 0, _context);
            // 把原盟主修改成新盟主原来的职位（系统操作，operatorCid为0）
            _chgPosition(_m_leader, refMemberPosition, 0, _context);

            oriLeaderCid = _m_leader.getCid();
            _m_leader = newLeaderInfo;

            // 盟主变动所以需要把联盟信息也推一遍
            broadcastMsg(new GS2GC_032_050_OnGuildShowInfoChg(_m_guildInfo.makeShowInfo()));
        } finally
        {
            _unlock();
        }

        // 发送盟主转让日志
        GuildLogFunc.sendLeaderTransferLog(oriLeaderCid, _newLeaderCid, getGuildInfo());

        return Result.SUCC;
    }

    /**
     * 修改玩家职位
     * <p>
     * 执行流程：
     * 1. 锁定成员列表，修改职位并更新职位人数统计
     * 2. 广播职位变更消息给所有公会成员
     * 3. 发送职位变更邮件通知给被变更职位的玩家
     * 4. 记录职位变更数据库日志
     * @param _memberInfo  被变更职位的成员信息
     * @param _refPosition 新职位配置
     * @param _operatorCid 操作者CID，系统操作为0
     */
    private void _chgPosition(GuildMemberInfo _memberInfo, RefGuildPosition _refPosition, long _operatorCid, NPPlayerContext _context)
    {
        // 保存原职位用于日志记录
        int oldPosition = _memberInfo.getPosition().ordinal();

        _lock();
        try
        {
            //减去原职位人数
            _m_positionCount[_memberInfo.getPosition().ordinal()]--;
            //修改职位
            _memberInfo.setPosition(_refPosition);
            //增加新职位人数
            _m_positionCount[_memberInfo.getPosition().ordinal()]++;
        } finally
        {
            _unlock();
        }

        //广播推送变更
        broadcastMsg(new GS2GC_032_051_OnMemberBaseInfoChg(_memberInfo.makeProto()));

        ALSynTaskManager.getInstance().regTask(() ->
        {
            //邮件通知职位变更
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().guild_position_change_mail_id);
            mailData.getContentReplace().add(getGuildInfo().getName());
            mailData.getContentReplace().add(_refPosition.name);
            MailSystem.addMail(_m_guildInfo.getGuildMgr().getServer(), _memberInfo.getCid(), mailData, _context);
        });

        //记录数据库日志
        int newPosition = _refPosition.type.ordinal();

        try
        {
            BM bmObj = _m_guildInfo.getGuildMgr().getServer().getBM();

            LogGuildPositionChgBO logBo = new LogGuildPositionChgBO();
            logBo.setGuildId(bmObj, _m_guildInfo.getGuildId());
            logBo.setCid(bmObj, _memberInfo.getCid());
            logBo.setOldPosition(bmObj, oldPosition);
            logBo.setNewPosition(bmObj, newPosition);
            logBo.setOperatorCid(bmObj, _operatorCid);
            CommLogDB.log(bmObj, logBo, null);
        } catch (Exception e)
        {
            USLog.error(_m_guildInfo.getGuildMgr().getServer(),
                    "GuildMemberMgr._chgPosition - log failed: exception occurred, guildId={}, cid={}, oldPosition={}, newPosition={}, operatorCid={}, error={}",
                    _m_guildInfo.getGuildId(), _memberInfo.getCid(), oldPosition, newPosition, _operatorCid, e.getMessage());
        }
    }

    /**
     * 踢出成员
     * @param _cid
     * @return
     */
    public Result removeMember(long _cid)
    {
    	GuildMemberInfo memberInfo = null;
        _lock();
        try
        {
            //查找成员
            memberInfo = lookup(_cid);
            if (memberInfo == null)
                return GuildErr.MEMBER_NOT_FOUND;

            //删除成员
            _m_memberList.remove(memberInfo);
            memberInfo.discard();
        } finally
        {
            _unlock();
        }

        //广播消息
        broadcastMsg(new GS2GC_032_055_OnGuildMemberRemove(_cid));
        
        //结算该玩家的联盟宝箱
        getGuildInfo().getGuildBoxMgr().settlePlayerAllBox(memberInfo);

        //移除联盟聊天房间
        getGuildInfo().getChatRoom().QuitRoom(_cid);

        return Result.SUCC;
    }

    /**
     * 任命职位
     * @param _addInfo
     * @param _appointeeInfo 任命者
     * @param _memberId      成员ID
     * @param _positionType  职位ID
     * @return
     */
    public Result appointPosition(GuildOp_PlayerCname _addInfo, GuildMemberInfo _appointeeInfo,
                                  long _memberId, EGuildPositionType _positionType, NPPlayerContext _context)
    {
        _lock();
        try
        {
            GuildMemberInfo memberInfo = lookup(_memberId);
            if (memberInfo == null)
                return GuildErr.MEMBER_NOT_FOUND;

            //检查权限是否可以任命该职位
            boolean canAppoint = _appointeeInfo.checkCanAppoint(_positionType);
            if (!canAppoint)
                return GuildErr.DONT_HAVE_PERMISSION;

            //不允许任命多个盟主
            if (_positionType == EGuildPositionType.LEADER)
                return GuildErr.DONT_HAVE_PERMISSION;

            //查询配置
            RefGuildPosition refPosition = RefGuildPosition.getMgr().get(_positionType.ordinal());
            if (refPosition == null)
                return CommErr.REF_NOT_FOUND;

            //判断贡献是否足够
            if (memberInfo.getTotalContribution() < refPosition.trans_need_historical_contributions)
                return GuildErr.GUILD_CONTRIBUTION_NOT_ENOUGH;

            RefGuildLevel levelRef = _m_guildInfo.getLevelRef();
            if (levelRef == null)
                return CommErr.REF_NOT_FOUND;

            //判断是否达到上限
            if (_positionType != EGuildPositionType.MEMBER && levelRef.getPositionLimit(_positionType) <= _m_positionCount[_positionType.ordinal()])
                return GuildErr.GUILD_POSITION_NUM_REACH_LIMIT;

            //修改职位（操作者为任命者）
            _chgPosition(memberInfo, refPosition, _appointeeInfo.getCid(), _context);
        } finally
        {
            _unlock();
        }

        GuildLogFunc.sendPositionChangeLog(_addInfo.getCname(), _appointeeInfo.getPosition(),
                _memberId, _positionType, getGuildInfo());

        return Result.SUCC;
    }

    /**
     * 获取成员贡献信息
     * @param _cid
     * @return
     */
    public Guild_MemberContributeInfo getMemberContributeInfo(long _cid)
    {
        GuildMemberInfo memberInfo = lookup(_cid);
        if (memberInfo == null)
            return null;

        return memberInfo.makeContributeProto();
    }

    /**
     * 弹劾盟主
     * @param _oriLeaderCid
     */
    public void passiveTransLeader(long _oriLeaderCid, NPPlayerContext _context)
    {
        if (!_m_findSuccessor)
            return;

        long newLeaderCid;

        _lock();
        try
        {
            //判断盟主是否已经变更
            if (_m_leader.getCid() != _oriLeaderCid)
                return;

            //寻找合适的接任者
            //且满足近72小时内有登录的联盟成员
            //副盟主>精英>成员
            //相同职位的按个人历史贡献
            //如果没有满足条件的则不变更盟主
            GuildMemberInfo newLeader = null;
            for (GuildMemberInfo memberInfo : _m_memberList)
            {
                //过滤条件
                if (memberInfo.getPosition() == EGuildPositionType.LEADER)
                    continue;

                //判断是否满足条件
                if (memberInfo.getOfflineTimeMs() > (long) RefGeneral.Ref().guild_leader_transfer_target_online_within_hours * 3600 * 1000)
                    continue;

                if (newLeader == null)
                {
                    newLeader = memberInfo;
                    continue;
                }

                // 比较职位
                if (newLeader.getPosition() == EGuildPositionType.DEPUTY_LEADER && memberInfo.getPosition() != EGuildPositionType.DEPUTY_LEADER)
                {
                    newLeader = memberInfo;
                    continue;
                } else if (newLeader.getPosition() == EGuildPositionType.ELITE && memberInfo.getPosition() == EGuildPositionType.MEMBER)
                {
                    newLeader = memberInfo;
                    continue;
                }

                //比较贡献
                if (newLeader.getTotalContribution() < memberInfo.getTotalContribution())
                {
                    newLeader = memberInfo;
                }
            }

            //没有找到合适的接任者
            if (newLeader == null)
                return;

            //如果找不到继任者, 则标记false
            _m_findSuccessor = false;

            //查询配置
            RefGuildPosition refLeaderPosition = RefGuildPosition.getMgr().get(EGuildPositionType.LEADER.ordinal());
            if (refLeaderPosition == null)
            {
                USLog.error(_m_guildInfo.getGuildMgr().getServer(), "GuildMemberMgr passiveTransLeader refLeaderPosition is null");
                return;
            }

            //查询配置
            RefGuildPosition refMemberPosition = RefGuildPosition.getMgr().get(EGuildPositionType.MEMBER.ordinal());
            if (refMemberPosition == null)
            {
                USLog.error(_m_guildInfo.getGuildMgr().getServer(), "GuildMemberMgr passiveTransLeader refMemberPosition is null");
                return;
            }

            //把新盟主修改成盟主（系统操作，operatorCid为0）
            _chgPosition(newLeader, refLeaderPosition, 0, _context);
            //把原盟主修改成成员（系统操作，operatorCid为0）
            _chgPosition(_m_leader, refMemberPosition, 0, _context);

            _m_leader = newLeader;
            newLeaderCid = newLeader.getCid();
        } finally
        {
            _unlock();
        }

        GuildLogFunc.sendLeaderTransferLog(_oriLeaderCid, newLeaderCid, getGuildInfo());
    }

    /**
     * 玩家上线处理
     * @param _cid
     */
    public void onMemberOnline(long _cid, long _timeMS)
    {
        _lock();
        try
        {
            GuildMemberInfo memberInfo = lookup(_cid);
            if (memberInfo == null)
                return;

            memberInfo.onOnline(_timeMS);

            _m_findSuccessor = true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 玩家下线处理
     * @param _cid
     */
    public void onMemberOffline(long _cid, long _timeMS)
    {
        _lock();
        try
        {
            GuildMemberInfo memberInfo = lookup(_cid);
            if (memberInfo == null)
                return;

            memberInfo.onOffline(_timeMS);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造处理委托记录
     * @return
     */
    public List<Guild_MemberEntrustInfo> makeEntrustList()
    {
        _lock();
        try
        {
            List<Guild_MemberEntrustInfo> sendBoxList = new ArrayList<>();
            for (GuildMemberInfo memberInfo : _m_memberList)
            {
                if (memberInfo.getTotalDealEntrustNum() == 0)
                    continue;

                sendBoxList.add(memberInfo.makeEntrustProto());
            }
            return sendBoxList;
        } finally
        {
            _unlock();
        }
    }
    
    /***************************************************
     * 联盟宝箱对联盟玩家的修改处理
     * @param _boxType
     * @param _addValue
     */
    public void incrClientAddCount(EGuildBoxType _boxType, int _addValue)
    {
        _lock();
        try
        {
            for (int i = 0; i < _m_memberList.size(); i++)
            {
            	GuildMemberInfo member = _m_memberList.get(i);
            	if(null == member)
            		continue;

            	member.getMemberGuildBoxInfo().incrClientAddCount(_boxType, _addValue);
            }
        } 
        finally
        {
            _unlock();
        }
    }
    
    public void onGuildDissolve()
    {
    	ArrayList<GuildMemberInfo> memberList = new ArrayList<>();
    
    	_lock();
    	try
    	{
    		memberList.addAll(_m_memberList);
    	}
    	finally
    	{
    		_unlock();
    	}
    	
    	for(int i = 0; i < memberList.size(); i++)
    	{
    		GuildMemberInfo member = memberList.get(i);
    		if(null == member)
    			continue;
    		
    		getGuildInfo().getGuildBoxMgr().settlePlayerAllBox(member);
    	}
    }
}
