package NPUSServer.Guild;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexManager;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.US_Service.Guild.*;
import Common.CachedObj.CachedObj_CachedGuildInfo;
import Common.GuildEnum.*;
import Common.GuildObj.*;
import Common.MailObj.Mail_Data;
import Common.NpChatObj.Common_ChatContent_GuildLog;
import Common.NpChatObj.Common_ChatContent_GuildRecruit;
import Common.NpChatObj.NPCommon_ChatSystemPlayerContent;
import Common.RankObj.Rank_BaseItem;
import CommonEnum.ECurrency;
import CommonEnum.ESpecAttrType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_002_UserJoinInfo;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetGainItemInfo;
import GS2GC.p032_GuildOp.*;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPChatRoomType;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Guild.RefGuildConstruct;
import NPGameRes.Refs.Guild.RefGuildEntrustQuality;
import NPGameRes.Refs.Guild.RefGuildFlag;
import NPGameRes.Refs.Guild.RefGuildLevel;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RefUnicodeLengthCheck;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GUILD_EXP_GAIN;
import NPUSServer.Common.Event.Events.Event_P_MAX_EARNINGS_CHG;
import NPUSServer.Guild.Cooperate.GuildCooperateInfo;
import NPUSServer.Guild.Dispatch.GuildHeroDispatchMgr;
import NPUSServer.Guild.EntrustWeight.GuildEntrustWeightList;
import NPUSServer.Guild.Event.GuildEventMgr;
import NPUSServer.Guild.GuildBox.GuildBoxMgr;
import NPUSServer.Guild.GuildDungeon.GuildDungeonMgr;
import NPUSServer.Guild.Log.GuildLogMgr;
import NPUSServer.Guild.MarsHelp.GuildMarsHelpMgr;
import NPUSServer.Guild.MarsMineBattleReport.GuildMarsMineBattleReportMgr;
import NPUSServer.Guild.MarsMineShare.GuildMarsMineShareMgr;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.Guild.Member.GuildMemberMgr;
import NPUSServer.Guild.Msg.GuildLogFunc;
import NPUSServer.Guild.Msg.GuildMemberCommitter;
import NPUSServer.Guild.Msg.GuildMsgDispatcher;
import NPUSServer.Guild.Rally.GuildRallyMgr;
import NPUSServer.Guild.Task.GuildCheckTaskMgr;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.NPEvent.EventMgr.EventObj.NPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.RankFixedMgr.RankFixedInfo;
import NPUSServer.USLog;
import NPUSServer.UsChatRoomInfo;
import RPC._ARpcCallBack;
import USDB.Bo.GuildBO;
import USLOGDB.Bo.LogGuildMemberChgBO;
import USLOGDB.Bo.LogGuildWealthChgBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class GuildInfo
{
    private GuildMgr _m_mgr;

    private GuildMemberMgr _m_memberMgr;
    private GuildEventMgr _m_eventMgr;
    private GuildCheckTaskMgr _m_checkTaskMgr;
    private GuildHeroDispatchMgr _m_heroDispatchMgr;
    private GuildLogMgr _m_GuildLogMgr;
    
    //公会副本管理数据
    private GuildDungeonMgr _m_dungeonMgr;
    
    //火星互助数据管理
    private GuildMarsHelpMgr _m_marsHelpMgr;
    //火星分享矿管理数据
    private GuildMarsMineShareMgr _m_marsMineShareMgr;
    //联盟火星矿战报管理数据
    private GuildMarsMineBattleReportMgr _m_marsMineBattleReportMgr;

    //联盟宝箱数据管理
    private GuildBoxMgr _m_mgrGuildBoxMgr;
    
    //联盟协作管理数据
    private GuildCooperateInfo _m_cooperateInfo;
    // 联盟集结管理数据（每个联盟独立一份）
    private GuildRallyMgr _m_rallyMgr;
    
    private GuildEntrustWeightList _m_entrustWeightList;
    private int _m_entrustSerialId;
    //数据锁
    private MutexManager _m_mutex;

    private GuildBO _m_bo;
    private Guild_JoinLimitInfo[] _m_joinLimitList;
    //建造次数信息
    private Guild_ConstructList _m_constructList;

    private long _m_preTotalEarnings;
    private long _m_newTotalEarnings;

    private RefGuildLevel _m_guildLevelRef;

    //Lazy处理类对象
    private LazyTaskDealer _m_lazyLoader;

    //聊天房间对象
    private UsChatRoomInfo _m_crChatRoom;

    public GuildInfo(GuildBO _bo, GuildMgr _mgr)
    {
        _m_bo = _bo;
        _m_mgr = _mgr;
        _m_memberMgr = new GuildMemberMgr(this);
        _m_eventMgr = new GuildEventMgr(this);
        _m_checkTaskMgr = new GuildCheckTaskMgr(this);
        _m_heroDispatchMgr = new GuildHeroDispatchMgr(this);
        _m_GuildLogMgr = new GuildLogMgr(this);
        
        _m_dungeonMgr = new GuildDungeonMgr(this);
        
        _m_marsHelpMgr = new GuildMarsHelpMgr(this);
        _m_marsMineShareMgr = new GuildMarsMineShareMgr(this);
        _m_marsMineBattleReportMgr = new GuildMarsMineBattleReportMgr(this);

        _m_mgrGuildBoxMgr = new GuildBoxMgr(this);
        
        _m_cooperateInfo = new GuildCooperateInfo(this);
        _m_rallyMgr = new GuildRallyMgr(this);
        
        _m_entrustWeightList = new GuildEntrustWeightList(this);
        _m_entrustSerialId = 0;
        _m_mutex = new MutexManager();

        _m_crChatRoom = new UsChatRoomInfo(getGuildMgr().getServer(), ENPChatRoomType.GUILD, getGuildId());

        _m_guildLevelRef = RefGuildLevel.getMgr().get(_bo.getLevel());
        if (_m_guildLevelRef == null)
            USLog.error(_m_mgr.getServer(), "GuildInfo init failed, levelRef not find, guildId:{} level:{}", _bo.getGuildId(), _bo.getLevel());

        _m_lazyLoader = new LazyTaskDealer(this::loadRankData, 3000);

        //加载加入限制信息
        _m_joinLimitList = new Guild_JoinLimitInfo[EGuildJoinLimitType.EGuildJoinLimitType_Length];
        Guild_JoinLimitInfoList joinLimitInfoList = new Guild_JoinLimitInfoList();
        if (_bo.getJoinLimitInfo() != null)
        {
            joinLimitInfoList.readPackage(ByteBuffer.wrap(_bo.getJoinLimitInfo()));
            for (Guild_JoinLimitInfo joinLimitInfo : joinLimitInfoList.getJoinLimitInfo())
            {
                _m_joinLimitList[joinLimitInfo.getType().ordinal()] = joinLimitInfo;
            }
        }

        //加载建造信息
        _m_constructList = new Guild_ConstructList();
        if (_bo.getConstructList() != null)
        {
            _m_constructList.readPackage(ByteBuffer.wrap(_bo.getConstructList()));
        }

        _m_entrustWeightList.initFromDb(_bo.getEntrustWeightBaseList());

        if (!_bo.getIsDissovle())
        {
            //创建固定聊天房间
            _m_mgr.getServer().getChatRoomMgr().regRoom(_m_crChatRoom);
        }
        
        //兼容更新联盟当前活跃点目标等级，如果未设置，则更新成当前公会等级
        if(_m_bo.getActivePointTargetLvl() == 0)
        {
        	_m_bo.saveActivePointTargetLvl(_m_mgr.getServer().getBM(), _m_bo.getLevel());
        }
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }
    
    public GuildBO getBo()
    {
    	return _m_bo;
    }

    public GuildMgr getGuildMgr()
    {
        return _m_mgr;
    }

    public GuildMemberMgr getMemberMgr()
    {
        return _m_memberMgr;
    }

    public GuildHeroDispatchMgr getHeroDispatchMgr()
    {
        return _m_heroDispatchMgr;
    }

    public GuildLogMgr getGuildLogMgr()
    {
        return _m_GuildLogMgr;
    }
    
    public GuildDungeonMgr getDungeonMgr()
    {
    	return _m_dungeonMgr;
    }
    
    public GuildMarsHelpMgr getMarsHelpMgr()
    {
    	return _m_marsHelpMgr;
    }

    public GuildBoxMgr getGuildBoxMgr()
    {
        return _m_mgrGuildBoxMgr;
    }

    public GuildMarsMineShareMgr getMarsMineShareMgr()
    {
        return _m_marsMineShareMgr;
    }

    public GuildMarsMineBattleReportMgr getMarsMineBattleReportMgr()
    {
        return _m_marsMineBattleReportMgr;
    }

    public long getGuildId()
    {
        return _m_bo.getGuildId();
    }

    public EGuildJoinType getJoinType()
    {
        return EGuildJoinType.EGuildJoinType_FromInt(_m_bo.getJoinType());
    }

    public long getLeaderId()
    {
        return _m_memberMgr.getLeaderId();
    }

    public int getMemberCount()
    {
        return _m_memberMgr.getMemberCount();
    }

    public long getTotalEarnings()
    {
        return _m_preTotalEarnings;
    }

    public String getName()
    {
        return _m_bo.getName();
    }

    public String getSimpleName()
    {
        return _m_bo.getSimpleName();
    }

    public int getMaxMemberCount()
    {
        RefGuildLevel levelRef = getLevelRef();
        return levelRef == null ? 0 : levelRef.member_limit;
    }

    public RefGuildLevel getLevelRef()
    {
        return _m_guildLevelRef;
    }
    public int getLevel()
    {
        return _m_bo.getLevel();
    }

    public long getWealth()
    {
        return _m_bo.getWealth();
    }

    public boolean isDissolve()
    {
        return _m_bo.getIsDissovle();
    }
    
    public long getActivePoint()
    {
        return _m_bo.getActivePoint();
    }
    public int getActivePointTargetLvl()
    {
        return _m_bo.getActivePointTargetLvl();
    }

    public GuildEventMgr getEventMgr()
    {
        return _m_eventMgr;
    }
    
    public GuildCooperateInfo getCooperateInfo()
    {
        return _m_cooperateInfo;
    }

    /**
     * 获取联盟集结管理器
     * 说明：集结相关逻辑统一从该管理器入口处理
     */
    public GuildRallyMgr getRallyMgr()
    {
        return _m_rallyMgr;
    }

    public long getLastProactiveTransLeaderTimeMs()
    {
        return _m_bo.getLastProactiveTransLeaderTimeMs();
    }

    public UsChatRoomInfo getChatRoom() {return _m_crChatRoom;}

    public void onInited()
    {
    	//副本数据处理
    	_m_dungeonMgr._onInited();
        //联盟协助排行榜处理
        _m_cooperateInfo.onInited();
    	
        _initCheckRandomEntrust();
        
        //联盟宝箱数据处理
        _m_mgrGuildBoxMgr._onInited();
    }

    /**
     * 初始化检查随机委托
     */
    private void _initCheckRandomEntrust()
    {
        if (_m_bo.getEntrustRefId() != 0)
            return;

        _refreshEntrustData();
    }

    /**
     * 修改公会名称和简称
     * @param _name
     * @param _simpleName
     */
    public Result chgName(String _name, String _simpleName)
    {
        _lock();
        try
        {
            String oriName = _m_bo.getName();
            String oriSimpleName = _m_bo.getSimpleName();

            Result result = _m_mgr.getNameChecker().chgName(oriName, _name, oriSimpleName, _simpleName);
            if (!result.isSucc())
                return result;

            BM bmObj = _m_mgr.getServer().getBM();
            _m_bo.setName(bmObj, _name);
            _m_bo.setSimpleName(bmObj, _simpleName);
            _m_bo.saveAllMarked(bmObj);

            //向所有成员广播simpleName变更
            GuildSimpleNameChg_2C rpc = new GuildSimpleNameChg_2C();
            rpc.req().setGuildId(getGuildId());
            rpc.req().setGuildName(getName());
            rpc.req().setSimpleName(_simpleName);

            //广播RPC
            getMemberMgr().broadcastRPC(rpc);
        } finally
        {
            _unlock();
        }

        //推送变更
        _m_memberMgr.broadcastMsg(new GS2GC_032_050_OnGuildShowInfoChg(makeShowInfo()));

        return Result.SUCC;
    }

    /**
     * 检查是否可以满足加入条件
     * @param _userJoinInfo
     * @return
     */
    public boolean checkCanFitJoinLimit(GuildOp_002_UserJoinInfo _userJoinInfo)
    {
        for (Guild_JoinLimitInfo joinLimitInfo : _m_joinLimitList)
        {
            if (joinLimitInfo == null)
                continue;

            if (!checkCanFit(_userJoinInfo, joinLimitInfo))
                return false;
        }
        return true;
    }

    /**
     * 检查是否满足条件
     * @param _userJoinInfo
     * @param _joinLimitInfo
     * @return
     */
    private boolean checkCanFit(GuildOp_002_UserJoinInfo _userJoinInfo, Guild_JoinLimitInfo _joinLimitInfo)
    {
        if (_joinLimitInfo == null)
            return true;

        switch (_joinLimitInfo.getType())
        {
            case NATION_POWER:
                return _userJoinInfo.getEarnings() >= _joinLimitInfo.getValue();
            case LEVEL:
                return _userJoinInfo.getLevel() >= _joinLimitInfo.getValue();
            default:
                return true;
        }
    }

    /**
     * 加入联盟
     * @param _isRequestJoin 如果是通过同意申请加入的情况，需要设置为true，通过申请和主动操作在校对上有所不同
     */
    public Result joinGuild(long _cid, EGuildPositionType _position, boolean _isCreateJoin, boolean _isRequestJoin)
    {
        _lock();
        try
        {
            //检查公会是否已经解散
            if (_m_bo.getIsDissovle())
                return GuildErr.GUILD_HAD_DISSOLVE;

            //加入成员列表
            Result result = _m_memberMgr.addMember(_cid, _position);
            if (!result.isSucc())
                return result;
        } finally
        {
            _unlock();
        }

        //发送RPC修改用户数据中的联盟Id,修改成功后才可以进行后续处理
        //如用户加入失败则会从成员中再次剔除
        GuildJoin_2C rpc = new GuildJoin_2C();
        rpc.req().setCid(_cid);
        rpc.req().setGuildId(getGuildId());
        rpc.req().setIsRequestJoin(_isRequestJoin);
        rpc.req().setSimpleName(getSimpleName());
        for(int i = 0; i < ESpecAttrType.values().length; i++) {
            rpc.req().getBuildingAddPerArr().add(getHeroDispatchMgr().getAddValue(ESpecAttrType.ESpecAttrType_FromInt(i)));
        }

        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);
        final GuildInfo thisGuild = this;

        getGuildMgr().getServer().rpc2us().requestToRepeat(usId, rpc
                , new _ARpcCallBack<GuildJoin_2C>() {
                    @Override
                    public void call_back(int _errCode, GuildJoin_2C _rpc)
                    {
                        //移除联盟内的对应玩家的申请
                        _m_mgr.getJoinRequestMgr().rmvLocalGuildCidRequest(thisGuild, _cid);

                        if(_errCode != 0)
                        {
                            //失败则剔除成员，直接结束
                            _lock();
                            try
                            {
                                //加入成员列表
                                _m_memberMgr.removeMember(_cid);
                            } finally
                            {
                                _unlock();
                            }
                            return ;
                        }

                        //后续继续做相关处理
                        //新增联盟协作的数据
                        _m_cooperateInfo.getDamageRankList().onNewMemberAdded(_cid);

                        //判断是否有火星自动互助
                        if(_rpc.retObj().getMarsAutoHelpEndTimeMS() > 0)
                        {
                            getMarsHelpMgr().getAutoHelpPlayerMgr().ensure(_cid, _rpc.retObj().getMarsAutoHelpEndTimeMS());
                        }

                        //触发国力事件
                        Event_P_MAX_EARNINGS_CHG event = new Event_P_MAX_EARNINGS_CHG(
                                NPPlayerContext.createNew(ENPGameEvent.JOIN_GUILD), _rpc.retObj().getMaxEarning(), 0);
                        _m_mgr.getServer().getGlobalEventHandlerMgr().handle(event, new NPGlobalUserEventObj(_cid));

                        //推送相关消息
                        //公会数据
                        getGuildMgr().getServer().sendMsgToGC(_cid, new GS2GC_032_065_OnJoinGuild(_isCreateJoin, makeDetailInfo(_cid)));
                        //公会副本数据
                        getGuildMgr().getServer().sendMsgToGC(_cid, US2GCWriter_037_GuildDungeonOp.make_056_OnDungeonPush(_cid, thisGuild));
                    }
                }
                , 3
                , () -> {
                    USLog.error(getGuildMgr().getServer(), "player:{} guild:{} send rpc memberRmv fail.", _cid, _m_bo.getGuildId());
                });


        return Result.SUCC;
    }

    /**
     * 踢出玩家
     * @param _cid
     * @param _isKick
     * @return
     */
    public Result removeMember(long _cid, boolean _isKick)
    {
        //移除成员
        Result result = _m_memberMgr.removeMember(_cid);
        if (!result.isSucc())
            return result;

        //移除玩家的大臣派遣数据
        _m_heroDispatchMgr.removeDispatchByCid(_cid);

        //移除联盟协作的数据
        _m_cooperateInfo.getDamageRankList().removePlayerData(_cid);
        
        //移除火星求助数据
        _m_marsHelpMgr.removePlayerHelp(_cid);

        //异步处理玩家的数据
        ALSynTaskManager.getInstance().regTask(() ->
        {
            //发送RPC处理
            memberRmv(_cid, _isKick);

            //移除玩家排行榜国力
            RankFixedInfo rankFixedInfo = _m_mgr.getServer().getRankFixedMgr().lookupRank(RefGeneral.Ref().guild_rank_fixed_id);
            if (rankFixedInfo != null)
                rankFixedInfo.removeSubObj(getGuildId(), _cid);

            //修改玩家Cache中数据
            PlayerCacheFunc.updateGuildInfo(getGuildMgr().getServer(), _cid, new CachedObj_CachedGuildInfo());
        });

        return Result.SUCC;
    }

    /**
     * 1s tick
     */
    public void tick(long _nowTimeMs)
    {
        _lock();
        try
        {
            //国力变更推送
            if (_m_preTotalEarnings != _m_newTotalEarnings)
            {
                _m_preTotalEarnings = _m_newTotalEarnings;
                _m_memberMgr.broadcastMsg(new GS2GC_032_062_OnGuildTotalEarningsChg(_m_preTotalEarnings));
            }

            //事件管理器tick
            _m_eventMgr.tick();
            
            //检查任务
            _m_checkTaskMgr.tick(_nowTimeMs);
            
            //公会副本检查
            _m_dungeonMgr.tick(_nowTimeMs);

            //联盟集结到期处理
            _m_rallyMgr.tick(_nowTimeMs);
            
        } finally
        {
            _unlock();
        }
    }

    /**
     * 国力变更处理
     * @param _score
     */
    public void onTotalEarningsChg(long _score)
    {
        _lock();
        try
        {
            _m_newTotalEarnings = _score;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 处理成员信息
     * @param _committer 成员提交者
     * @param _proto     协议
     */
    public void dealMemberMsg(_ANPUSUserBasicMsgItem _committer, _IALProtocolStructure _proto)
    {
        //查询成员信息
        GuildMemberInfo memberInfo = _m_memberMgr.lookup(_committer.getUserData().getCid());
        if (memberInfo == null)
        {
            _committer.commitFailRes(GuildErr.NOT_MEMBER_OF_GUILD.getCode());
            return;
        }

        GuildMsgDispatcher.getInstance().dealMsg(new GuildMemberCommitter(this, memberInfo, _committer), _proto);
    }

    /**
     * 解散公会
     * @return
     */
    public Result dissolve(long _cid)
    {
        _lock();
        try
        {
            //检查是否满足解散条件
            if (!_m_memberMgr.lookup(_member -> _member.getCid() != _cid).isEmpty())
                return GuildErr.STILL_HAVE_OTHER_MEMBER;

            //标识解散
            _m_bo.saveIsDissovle(_m_mgr.getServer().getBM(), true);
        } finally
        {
            _unlock();
        }

        //踢出当前操作玩家
        removeMember(_cid, false);

        //解散公会
        _m_mgr.onGuildDissolve(this);

        //销毁固定聊天房间
        _m_mgr.getServer().getChatRoomMgr().unRegRoom(_m_crChatRoom);

        //销毁相关数据
        //派遣数据、成员数据在移除成员时已处理，故这里不需要再处理
        _m_cooperateInfo.discard();
        _m_dungeonMgr.discard();
        _m_eventMgr.discard();
        _m_GuildLogMgr.discard();
        _m_marsHelpMgr.discard();
        _m_mgrGuildBoxMgr.discard();
        _m_marsMineShareMgr.discard();
        _m_marsMineBattleReportMgr.discard();

        //销毁公会数据
        _m_bo.del(getGuildMgr().getServer().getBM());

        BM bmObj = getGuildMgr().getServer().getBM();
        LogGuildMemberChgBO logBo = new LogGuildMemberChgBO();
        logBo.setGuildId(bmObj, getGuildId());
        logBo.setType(bmObj, 4);
        logBo.setCid(bmObj, _cid);
        CommLogDB.log(bmObj, logBo, null);

        return Result.SUCC;
    }

    /**
     * 修改公会旗帜
     * @param _flagId
     * @return
     */
    public Result chgFlag(long _flagId)
    {
        RefGuildFlag refGuildFlag = RefGuildFlag.getMgr().get(_flagId);
        if (refGuildFlag == null)
            return CommErr.REF_NOT_FOUND;

        _lock();
        try
        {
            _m_bo.saveFlagId(_m_mgr.getServer().getBM(), _flagId);
        } finally
        {
            _unlock();
        }

        //推送变更
        getMemberMgr().broadcastMsg(new GS2GC_032_050_OnGuildShowInfoChg(makeShowInfo()));

        return Result.SUCC;
    }

    /**
     * 修改公会宣言
     * @param _declaration
     * @return
     */
    public Result chgDeclaration(String _declaration)
    {
        //检查长度
        if (!RefGeneral.Ref().guild_declaration_length_limit.inRange(RefUnicodeLengthCheck.getMgr().getCharLength(_declaration)))
            return GuildErr.STRING_LENGTH_OVER_LIMIT;

        _lock();
        try
        {
            _m_bo.saveDeclaration(_m_mgr.getServer().getBM(), _declaration);
        } finally
        {
            _unlock();
        }

        //推送变更
        getMemberMgr().broadcastMsg(new GS2GC_032_050_OnGuildShowInfoChg(makeShowInfo()));

        return Result.SUCC;
    }

    /**
     * 修改公会公告
     * @param _announcement
     * @return
     */
    public Result chgAnnouncement(String _announcement)
    {
        //检查长度
        if (!RefGeneral.Ref().guild_announcement_length_limit.inRange(RefUnicodeLengthCheck.getMgr().getCharLength(_announcement)))
            return GuildErr.STRING_LENGTH_OVER_LIMIT;

        _lock();
        try
        {
            _m_bo.saveAnnouncement(_m_mgr.getServer().getBM(), _announcement);
        } finally
        {
            _unlock();
        }

        //推送变更
        getMemberMgr().broadcastMsg(new GS2GC_032_052_OnGuildAnnouncementChg(_announcement));

        return Result.SUCC;
    }

    /**
     * 修改加入方式
     * @param _type
     * @param _joinLimitInfo
     * @return
     */
    public Result chgJoinType(EGuildJoinType _type, ArrayList<Guild_JoinLimitInfo> _joinLimitInfo)
    {
        _lock();
        try
        {
            //保存加入方式
            _m_joinLimitList = new Guild_JoinLimitInfo[EGuildJoinLimitType.EGuildJoinLimitType_Length];
            for (Guild_JoinLimitInfo joinLimitInfo : _joinLimitInfo)
            {
                _m_joinLimitList[joinLimitInfo.getType().ordinal()] = joinLimitInfo;
            }

            _m_bo.setJoinType(_m_mgr.getServer().getBM(), _type.ordinal());
            _m_bo.setJoinLimitInfo(_m_mgr.getServer().getBM(), new Guild_JoinLimitInfoList(_joinLimitInfo).makePackage().array());
            _m_bo.saveAllMarked(_m_mgr.getServer().getBM());
        } finally
        {
            _unlock();
        }

        //推送变更
        getMemberMgr().broadcastMsg(new GS2GC_032_050_OnGuildShowInfoChg(makeShowInfo()));

        return Result.SUCC;
    }

    /**
     * 建造
     * @param _addInfo
     * @param _memberInfo
     * @param _refId
     * @param _context
     * @return
     */
    public Result construct(GuildOp_PlayerCname _addInfo, GuildMemberInfo _memberInfo, long _refId, NPPlayerContext _context)
    {
        //查找配置
        RefGuildConstruct refGuildConstruct = RefGuildConstruct.getMgr().get(_refId);
        if (refGuildConstruct == null)
        {
            USLog.error(_m_mgr.getServer(), "GuildInfo.construct - refGuildConstruct not found, guildId={} refId={} cid:{}",
                    _m_bo.getGuildId(), _refId, _memberInfo.getCid());
            return CommErr.REF_NOT_FOUND;
        }

        //查找联盟等级配置
        RefGuildLevel levelRef = getLevelRef();
        if (levelRef == null)
        {
            USLog.error(_m_mgr.getServer(), "GuildInfo.construct - levelRef not found, guildId={} level={} cid:{}",
                    _m_bo.getGuildId(), _m_bo.getLevel(), _memberInfo.getCid());
            return CommErr.REF_NOT_FOUND;
        }

        //获取个人贡献
        _memberInfo.gainDevote(refGuildConstruct.add_devote, _context);

        //记录建造信息
        onConstruct(_memberInfo.getCid(), refGuildConstruct, levelRef, _context);

        GuildLogFunc.sendGuildConstructionLog(_addInfo.getCname(), _refId, refGuildConstruct.add_guild_exp,
                refGuildConstruct.add_guild_wealth, refGuildConstruct.add_personal_guild_coin, this);

        return Result.SUCC;
    }

    /**
     * 记录建造信息
     * @param _ref
     * @param _context
     */
    private void onConstruct(long _cid, RefGuildConstruct _ref, RefGuildLevel _levelRef, NPPlayerContext _context)
    {
        _lock();
        try
        {
            //检查是否需要重置记录
            int timeTag = CommonFunc.getNowTagYYYYMMDD();
            if (timeTag != _m_constructList.getDate())
            {
                _m_constructList = new Guild_ConstructList();
                _m_constructList.setDate(timeTag);
            }

            //查询对应类型的记录
            Guild_ConstructInfo constructInfo = null;
            for (Guild_ConstructInfo tempItem : _m_constructList.getConstructList())
            {
                if (tempItem.getType() == _ref.type)
                {
                    constructInfo = tempItem;
                    break;
                }
            }
            if (constructInfo == null)
            {
                constructInfo = new Guild_ConstructInfo();
                constructInfo.setType(_ref.type);
                _m_constructList.addConstructList(constructInfo);
            }

            constructInfo.setNum(constructInfo.getNum() + 1);

            long gainExp = _ref.add_guild_exp;
            long gainWealth = _ref.add_guild_wealth;

            //如果是金币建造，需要考虑上限情况
            if (_ref.type == EGuildConstructType.GOLD)
            {
                WCGPairLong alreadyGainExpAndWealth = getAlreadyGainExpAndWealth();

                //计算还可以获得的经验和财富
                long canGainExp = Math.max(0, _levelRef.construct_gain_guild_exp_limit - alreadyGainExpAndWealth.first());
                long canGainWealth = Math.max(0, _levelRef.construct_gain_guild_wealth_limit - alreadyGainExpAndWealth.second());

                //计算实际获得的经验和财富
                gainExp = Math.min(canGainExp, gainExp);
                gainWealth = Math.min(canGainWealth, gainWealth);
            }

            //记录获得的经验和财富
            constructInfo.setGainGuildExpCount(constructInfo.getGainGuildExpCount() + gainExp);
            constructInfo.setGainGuildWealthCount(constructInfo.getGainGuildWealthCount() + gainWealth);
            //增加累计奖励进度
            _m_constructList.setRewardPoint(_m_constructList.getRewardPoint() + _ref.add_reward_point);

            //获取经验
            gainExp(_cid, gainExp, _context);
            //获取财富
            gainWealth(gainWealth, _context);

            _m_bo.saveConstructList(_m_mgr.getServer().getBM(), _m_constructList.makePackage().array());

            //推送变更
            getMemberMgr().broadcastMsg(new GS2GC_032_067_OnGuildConstructListChg(makeProtoConstructList()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加建造进度
     * @param _num
     */
    public void addConstructPoint(int _num)
    {
        _lock();
        try
        {
            //检查是否需要重置记录
            int timeTag = CommonFunc.getNowTagYYYYMMDD();
            if (timeTag != _m_constructList.getDate())
            {
                _m_constructList = new Guild_ConstructList();
                _m_constructList.setDate(timeTag);
            }

            _m_constructList.setRewardPoint(_m_constructList.getRewardPoint() + _num);
            _m_bo.saveConstructList(_m_mgr.getServer().getBM(), _m_constructList.makePackage().array());

            //推送变更
            getMemberMgr().broadcastMsg(new GS2GC_032_067_OnGuildConstructListChg(makeProtoConstructList()));
        } finally
        {
            _unlock();
        }
    }


    /**
     * 获取已经获得的经验和财富
     * @return 前者经验，后者财富
     */
    public WCGPairLong getAlreadyGainExpAndWealth()
    {
        _lock();
        try
        {
            WCGPairLong pair = new WCGPairLong();
            for (Guild_ConstructInfo constructInfo : _m_constructList.getConstructList())
            {
                pair.setFirst(pair.first() + constructInfo.getGainGuildExpCount());
                pair.setSecond(pair.second() + constructInfo.getGainGuildWealthCount());
            }
            return pair;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获得经验
     * @param _guildExp
     * @param _context
     */
    public void gainExp(long _cid, long _guildExp, NPPlayerContext _context)
    {
        if (_guildExp <= 0)
            return;

        int oriLevel = _m_bo.getLevel();

        _lock();
        try
        {
            long newExp = _m_bo.getExp() + _guildExp;

            //检查升级
            if (_m_guildLevelRef != null)
            {
                RefGuildLevel refCurLevel = _m_guildLevelRef;
                while (true)
                {
                    //查找下一等级配置
                    RefGuildLevel refNextLevel = RefGuildLevel.getMgr().get(refCurLevel.level + 1);
                    if (refNextLevel == null)
                        break;

                    //如果经验不足升级到下一等级，跳出循环
                    int needExp = refCurLevel.need_exp;
                    if (needExp <= 0 || newExp < needExp)
                        break;

                    //消耗升级经验，升级到下一等级
                    newExp -= needExp;
                    refCurLevel = refNextLevel;
                }

                //更新等级配置缓存
                if (_m_guildLevelRef != refCurLevel)
                {
                    _m_bo.setLevel(_m_mgr.getServer().getBM(), refCurLevel.level);
                    _m_guildLevelRef = refCurLevel;

                    //向所有成员广播simpleName变更
                    GuildLevelChg_2C rpc = new GuildLevelChg_2C();
                    rpc.req().setGuildId(getGuildId());
                    rpc.req().setGuildLvl(refCurLevel.level);

                    //广播RPC
                    getMemberMgr().broadcastRPC(rpc);
                }
            } else
            {
                USLog.error(_m_mgr.getServer(), "GuildInfo gainExp failed, levelRef not find, guildId:{} level:{}", _m_bo.getGuildId(), _m_bo.getLevel());
            }

            //保存经验
            _m_bo.setExp(_m_mgr.getServer().getBM(), newExp);
            _m_bo.saveAllMarked(_m_mgr.getServer().getBM());

            _context.getCollector().addItem(RefGeneral.Ref().guild_exp_common_item, _guildExp);
        } finally
        {
            _unlock();
        }

        //推送变更
        getMemberMgr().broadcastMsg(new GS2GC_032_050_OnGuildShowInfoChg(makeShowInfo()));

        //如果等级发生提升
        int newLevel = _m_bo.getLevel();
        if (oriLevel != newLevel)
        {
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().guild_upgrade_mail_id);
            mailData.getContentReplace().add("[" + getSimpleName() + "]");
            mailData.getContentReplace().add(getName());
            mailData.getContentReplace().add(String.valueOf(newLevel));

            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_UPGRADE);
            List<Long> memberCidList = getMemberMgr().getMemberCidList(null);
            for (Long memberCid : memberCidList)
            {
                MailSystem.addMail(_m_mgr.getServer(), memberCid, mailData, context);
            }

            GuildLogFunc.sendGuildUpgradeLog(newLevel, this);
        }
        
        //触发事件
        ALSynTaskManager.getInstance().regTask(()->
        {
        	Event_P_GUILD_EXP_GAIN evt = new Event_P_GUILD_EXP_GAIN(_context, _guildExp);
        	_m_mgr.getServer().getGlobalEventHandlerMgr().handle(evt, new NPGlobalUserEventObj(_cid));
        });
    }

    /**
     * 获得财富
     * @param _guildWealth
     * @param _context
     */
    public void gainWealth(long _guildWealth, NPPlayerContext _context)
    {
        if (_guildWealth <= 0)
            return;

        long oriValue;
        long newValue;
        _lock();
        try
        {
            oriValue = getWealth();
            _m_bo.saveWealth(_m_mgr.getServer().getBM(), getWealth() + _guildWealth);
            newValue = getWealth();
        } finally
        {
            _unlock();
        }

        //推送变更
        getMemberMgr().broadcastMsg(new GS2GC_032_053_OnGuildWealthChg(getWealth()));

        _context.getCollector().addItem(RefGeneral.Ref().guild_wealth_common_item, _guildWealth);

        BM bmObj = getGuildMgr().getServer().getBM();
        LogGuildWealthChgBO logBo = new LogGuildWealthChgBO();
        logBo.setGuildId(bmObj, getGuildId());
        logBo.setEvent(bmObj, _context.getContextId());
        logBo.setOldNumber(bmObj, oriValue);
        logBo.setChgValue(bmObj, _guildWealth);
        logBo.setFinalNumber(bmObj, newValue);
        CommLogDB.log(bmObj, logBo, _context);
    }
    
    /**
     * 消耗财富
     * @param _guildWealth
     * @param _context
     * @return
     */
    public boolean spendWealth(long _guildWealth, NPPlayerContext _context)
    {
        _lock();
        
        try
        {
        	long newWealth = getWealth() - _guildWealth;
        	if(newWealth < 0)
        		return false;

            long oriValue = getWealth();
        	
            _m_bo.saveWealth(_m_mgr.getServer().getBM(), newWealth);
            
            ALSynTaskManager.getInstance().regTask(()->
            {
                //推送变更
                getMemberMgr().broadcastMsg(new GS2GC_032_053_OnGuildWealthChg(getWealth()));
            });

            BM bmObj = getGuildMgr().getServer().getBM();
            LogGuildWealthChgBO logBo = new LogGuildWealthChgBO();
            logBo.setGuildId(bmObj, getGuildId());
            logBo.setEvent(bmObj, _context.getContextId());
            logBo.setOldNumber(bmObj, oriValue);
            logBo.setChgValue(bmObj, _guildWealth);
            logBo.setFinalNumber(bmObj, getWealth());
            CommLogDB.log(bmObj, logBo, _context);
            
            return true;
        } 
        finally
        {
            _unlock();
        }
    }

    /**
     * 发送联盟日志
     */
    public void sendGuildLog(EGuildLogType _msgType, _IALProtocolStructure _proto, List<EGuildLogShowType> _showTypeList)
    {
        //发送联盟日志到聊天频道
        if (_showTypeList.contains(EGuildLogShowType.CHAT))
            sendGuildLogToChat(_msgType, _proto);

        //发送联盟日志到弹窗
        if (_showTypeList.contains(EGuildLogShowType.POPUP_WINDOW))
            getGuildLogMgr().addLog(_msgType, _proto);
    }

    /**
     * 发送联盟日志到聊天频道
     * @param _msgType
     * @param _proto
     */
    public void sendGuildLogToChat(EGuildLogType _msgType, _IALProtocolStructure _proto)
    {
        NPCommon_ChatSystemPlayerContent userProto = new NPCommon_ChatSystemPlayerContent();
        userProto.setSystemPlayerId(RefGeneral.Ref().guild_log_chat_npc_id);

        Common_ChatContent_GuildLog contentProto = new Common_ChatContent_GuildLog();
        contentProto.setLogType(_msgType);
        contentProto.setData(_proto.makePackage());

        //对全服聊天频道发送消息
        getChatRoom().SendRoomMsg(0, ENPChatMsgType.GUILD_LOG, userProto.makePackage(), contentProto.makePackage(), null);
    }

    /**
     * 保存最后一次主动转让盟主时间
     * @param _nowTimeMS
     */
    public void setLastProactiveTransLeaderTimeMs(long _nowTimeMS)
    {
        _m_bo.saveLastProactiveTransLeaderTimeMs(_m_mgr.getServer().getBM(), _nowTimeMS);
    }

    /**
     * 转让盟主
     * @param _memberId
     * @return
     */
    public Result transLeader(long _memberId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            return _m_memberMgr.transLeader(_memberId, _context);
        } finally
        {
            _unlock();
        }
    }

    /**
     * GM命令强制转让盟主
     *
     * 执行流程：
     * 1. 校验当前盟主ID是否匹配
     * 2. 调用强制转让方法，绕过所有业务规则检查（CD、副盟主、贡献度等）
     *
     * @param _currentLeaderCid 当前盟主CID（用于校验）
     * @param _newLeaderCid 新盟主CID
     * @param _context 操作上下文
     * @return 转让结果
     *
     * 线程安全：在数据锁保护下执行
     */
    public Result transferLeaderByGM(long _currentLeaderCid, long _newLeaderCid, NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 校验当前盟主ID是否正确
            long actualLeaderId = getLeaderId();
            if (actualLeaderId != _currentLeaderCid)
                return CommErr.PARAM_ERROR;

            // 调用强制转让方法（绕过业务规则限制）
            return _m_memberMgr.forceTransLeader(_newLeaderCid, _context);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造缓存信息
     * @return
     */
    public CachedObj_CachedGuildInfo makeCacheInfo()
    {
        CachedObj_CachedGuildInfo info = new CachedObj_CachedGuildInfo();
        info.setGuildId(getGuildId());
        info.setGuildName(getName());
        info.setGuildSimpleName(getSimpleName());
        return info;
    }

    /**
     * 尝试公开招募
     * @return
     */
    public void tryOpenRecruit(GuildMsgCommiter _commiter)
    {
        _lock();
        try
        {
            long nowTimeMS = CommonFunc.getNowTimeMS();
            //检查是否在招募CD中
            if (_m_bo.getNextOpenRecruitTimeMs() > nowTimeMS)
                _commiter.commitFailRes(GuildErr.GUILD_RECRUIT_CD.getCode());

            _m_bo.saveNextOpenRecruitTimeMs(_m_mgr.getServer().getBM(), nowTimeMS + RefGeneral.Ref().guild_open_recruit_gap_sec * 1000L);

            getMemberMgr().broadcastMsg(new GS2GC_032_068_OnOpenRecruitCdChg(_m_bo.getNextOpenRecruitTimeMs()));
        } finally
        {
            _unlock();
        }

        Common_ChatContent_GuildRecruit proto = new Common_ChatContent_GuildRecruit();
        proto.setGuildId(getGuildId());
        proto.setGuildName(getName());

        _commiter.commitSucRes(proto);
    }

    /**
     * 清空公开招募CD
     */
    public void clearOpenRecruitCd()
    {
        _lock();
        try
        {
            _m_bo.saveNextOpenRecruitTimeMs(_m_mgr.getServer().getBM(), 0);
            getMemberMgr().broadcastMsg(new GS2GC_032_068_OnOpenRecruitCdChg(0));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 领取建造奖励
     * @param _num
     * @return
     */
    public Result checkConstructReward(int _num)
    {
        //检查是否是本周计数
        int timeTag = CommonFunc.getNowTagYYYYMMDD();
        if (timeTag != _m_constructList.getDate())
            return GuildErr.GUILD_CONSTRUCT_REWARD_POINT_NOT_ENOUGH;

        //检查是否有足够的点数
        if (_m_constructList.getRewardPoint() < _num)
            return GuildErr.GUILD_CONSTRUCT_REWARD_POINT_NOT_ENOUGH;

        return Result.SUCC;
    }

    /**
     * 处理委托
     * @param _committer
     * @return
     */
    public void dealEntrust(GuildMsgCommiter _committer)
    {
        //查询成员对象
        GuildMemberInfo memberInfo = getMemberMgr().lookup(_committer.getCid());
        if(null == memberInfo)
        {
            _committer.commitFailRes(GuildErr.NOT_MEMBER_OF_GUILD.getCode());
            return;
        }

        RefGuildEntrustQuality oriRef = RefGuildEntrustQuality.getMgr().get(_m_bo.getEntrustRefId());
        if (oriRef == null)
        {
            USLog.error(_m_mgr.getServer(), "GuildInfo dealEntrust error, guildId = {} refId = {}",
                    getGuildId(), _m_bo.getEntrustRefId());
            _committer.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        }

        long gainCurrency;

        _lock();
        try
        {
            //增加委托处理点数
            _m_bo.saveEntrustPoint(_m_mgr.getServer().getBM(), _m_bo.getEntrustPoint() + 1);

            gainCurrency = (long) Math.ceil(getTotalEarnings() * oriRef.gain_currency_per / 10000d);

            //检查是否达到要求
            _checkEntrustReach();

            //推送变更
            getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_071_OnGuildEntrustChg(makeEntrustInfo()));
        } finally
        {
            _unlock();
        }

        //暴击判断：按配置概率随机触发，触发后按权重随机倍数（critMul>1表示暴击）
        int critMul = 1;
        if (!RefGeneral.Ref().guild_entrust_crit_mul_weight.isEmpty())
        {
            critMul = RefGeneral.Ref().guild_entrust_crit_mul_weight.random();
            gainCurrency *= critMul;
        }

        //累加成员委托次数
        memberInfo.recordDealEntrustNum(1);

        //构造奖励
        GuildOp_RetGainItemInfo retInfo = new GuildOp_RetGainItemInfo();
        retInfo.getItemList().add(new NPCommon_ItemInfo(ENPItemType.CURRENCY.ordinal(), ECurrency.SILVER.ordinal(), gainCurrency, null));
        retInfo.setCritMul(critMul);

        _committer.commitSucRes(retInfo);
    }

    /**
     * 增加委托进度
     * @param _num
     */
    public void incEntrustPoint(int _num)
    {
        _lock();
        try
        {
            //增加委托处理点数
            _m_bo.saveEntrustPoint(_m_mgr.getServer().getBM(), _m_bo.getEntrustPoint() + _num);

            //检查是否达到要求
            _checkEntrustReach();

            //推送变更
            getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_071_OnGuildEntrustChg(makeEntrustInfo()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 刷新委托数据（GM命令用）
     */
    public boolean refreshEntrustData()
    {
        _lock();
        try
        {
            boolean result = _refreshEntrustData();
            if (result)
            {
                //推送变更
                getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_071_OnGuildEntrustChg(makeEntrustInfo()));
            }
            return result;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 刷新委托数据
     */
    private boolean _refreshEntrustData()
    {
        RefGuildEntrustQuality newRef = _m_entrustWeightList.randomRef();
        if (newRef == null)
        {
            USLog.error(_m_mgr.getServer(), "GuildInfo checkEntrustReach error, guildId = {} refId = {}",
                    getGuildId(), _m_bo.getEntrustRefId());
            return false;
        }

        //记录随机结果
        _m_entrustWeightList.dealRollResult(newRef.Id());

        _m_entrustSerialId++;

        _m_bo.setEntrustRefId(_m_mgr.getServer().getBM(), newRef.Id());
        Long newEventId = CommonFunc.randSelect(newRef.guild_random_requests_id_list);
        _m_bo.setEntrustEventId(_m_mgr.getServer().getBM(), newEventId == null ? 0 : newEventId);
        _m_bo.setEntrustWeightBaseList(_m_mgr.getServer().getBM(), _m_entrustWeightList.toString());
        _m_bo.setEntrustPoint(_m_mgr.getServer().getBM(), 0);
        _m_bo.saveAllMarked(_m_mgr.getServer().getBM());

        return true;
    }

    /**
     * 检查委托是否达到要求
     */
    private void _checkEntrustReach()
    {
        RefGuildEntrustQuality oriRef = RefGuildEntrustQuality.getMgr().get(_m_bo.getEntrustRefId());
        if (oriRef == null)
        {
            USLog.error(_m_mgr.getServer(), "GuildInfo checkEntrustReach error, guildId = {} refId = {}",
                    getGuildId(), _m_bo.getEntrustRefId());
            return;
        }

        //检查是否达到要求
        if (_m_bo.getEntrustPoint() >= oriRef.count)
        {
            //刷新委托数据
            if (!_refreshEntrustData())
                return;

            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DEAL_ANECDOTE_CHOICE_EVENT);
            //发送委托奖励邮件
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().guild_entrust_reward_mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(oriRef.reawrd_item_list));
            getMemberMgr().broadcastMail(mailData, context);
        }
    }

    /**
     * 构造建造信息
     * @return
     */
    private Guild_ConstructList makeProtoConstructList()
    {
        _lock();
        try
        {
            Guild_ConstructList constructList = new Guild_ConstructList();
            constructList.setDate(_m_constructList.getDate());
            for (Guild_ConstructInfo guildConstructInfo : _m_constructList.getConstructList())
            {
                if (guildConstructInfo == null)
                    continue;

                constructList.getConstructList().add(guildConstructInfo);
            }
            constructList.setRewardPoint(_m_constructList.getRewardPoint());
            return constructList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造委托信息
     * @return
     */
    private Guild_EntrustInfo makeEntrustInfo()
    {
        Guild_EntrustInfo entrustInfo = new Guild_EntrustInfo();
        entrustInfo.setSerial(_m_entrustSerialId);
        entrustInfo.setRefId(_m_bo.getEntrustRefId());
        entrustInfo.setEventId(_m_bo.getEntrustEventId());
        entrustInfo.setPoint(_m_bo.getEntrustPoint());
        return entrustInfo;
    }

    /**
     * 公会最简展示数据
     * @return
     */
    public Guild_IconShow makeIconShowInfo()
    {
    	Guild_IconShow guildShowInfo = new Guild_IconShow();
        guildShowInfo.setGuidId(_m_bo.getGuildId());
        guildShowInfo.setFlagId(_m_bo.getFlagId());
        guildShowInfo.setName(_m_bo.getName());
        
        return guildShowInfo;
    }
    /**
     * 构造展示信息
     * @return Guild_ShowInfo
     */
    public Guild_ShowInfo makeShowInfo()
    {
        Guild_ShowInfo guildShowInfo = new Guild_ShowInfo();
        guildShowInfo.setGuidId(_m_bo.getGuildId());
        guildShowInfo.setFlagId(_m_bo.getFlagId());
        guildShowInfo.setName(_m_bo.getName());
        guildShowInfo.setSimpleName(_m_bo.getSimpleName());
        guildShowInfo.setDeclaration(_m_bo.getDeclaration());
        guildShowInfo.setLeaderId(getLeaderId());
        guildShowInfo.setLevel(_m_bo.getLevel());
        guildShowInfo.setExp(_m_bo.getExp());
        guildShowInfo.setTotalEarnings(getTotalEarnings());
        guildShowInfo.setJoinType(EGuildJoinType.EGuildJoinType_FromInt(_m_bo.getJoinType()));
        _lock();
        try
        {
            for (Guild_JoinLimitInfo joinLimitInfo : _m_joinLimitList)
            {
                if (joinLimitInfo == null)
                    continue;

                guildShowInfo.getJoinLimitInfo().add(joinLimitInfo);
            }
        } finally
        {
            _unlock();
        }
        return guildShowInfo;
    }

    /**
     * 构造详细信息
     * @param _cid 请求者信息
     * @return
     */
    public Guild_DetailInfo makeDetailInfo(long _cid)
    {
        Guild_DetailInfo detailInfo = new Guild_DetailInfo();
        detailInfo.setGuildInfo(makeShowInfo());
        detailInfo.setGuildWealth(getWealth());
        detailInfo.setAnnouncement(_m_bo.getAnnouncement());
        //成员列表
        detailInfo.getMemberList().addAll(getMemberMgr().makeProtoMemberList());
        //加入请求列表
        detailInfo.getJoinRequestList().addAll(_m_mgr.getJoinRequestMgr().makeProtoGuildJoinRequestList(getGuildId()));
        //建造信息
        detailInfo.setConstructList(makeProtoConstructList());
        //事件列表
        detailInfo.getEventList().addAll(getEventMgr().makeProtoList());
        //下次可招募时间
        detailInfo.setNextCanRecruitTimeMs(_m_bo.getNextOpenRecruitTimeMs());
        //委托信息
        detailInfo.setEntrustInfo(makeEntrustInfo());
        //派遣信息
        detailInfo.setDispatchData(getHeroDispatchMgr().makeFullDispatchInfo());
        //玩家贡献信息
        Guild_MemberContributeInfo selfContributeInfo = getMemberMgr().getMemberContributeInfo(_cid);
        if (selfContributeInfo != null)
            detailInfo.setSelfContributeInfo(selfContributeInfo);
        //最新火星矿战报ID（用于红点判断）
        detailInfo.setLatestMarsMineBattleReportId(_m_marsMineBattleReportMgr.getLatestReportId());

        return detailInfo;
    }

    /**
     * 标记需要加载排行榜数据
     */
    public void setNeedLoadRankData()
    {
        _m_lazyLoader.setNeedDeal();
    }

    /**
     * 加载排行榜数据
     */
    public void loadRankData()
    {
        RankFixedInfo rankFixedInfo = _m_mgr.getServer().getRankFixedMgr().lookupRank(RefGeneral.Ref().guild_rank_fixed_id);
        if (rankFixedInfo == null)
            return;

        rankFixedInfo.makeRankBaseByKey(getGuildId(), false, new _ICallBackResultT<Rank_BaseItem>()
                {
                    @Override
                    public void onRunOver(Result _result, Rank_BaseItem _data)
                    {
                        if (!_result.isSucc())
                            return;

                        //联盟战力变更
                        onTotalEarningsChg(_data.getScore());
                    }
                });
    }
    
    /////////////////////////// 联盟活跃点 ///////////////////////////
    public void incrActivePoint(long _addValue, NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		setActivePoint((getActivePoint() + _addValue), _context);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    public void setActivePoint(long _value, NPPlayerContext _context)
    {
    	_lock();
    	
    	try
    	{
    		long newValue = _value;
    		
    		//检测是否超过当前公会积分的上限，如果超过，则扣除数据并发送礼包
    		RefGuildLevel guildLvlRef = RefGuildLevel.getMgr().get(getActivePointTargetLvl());
    		long needValue = guildLvlRef.gain_box_need_active_point;
    		if(null != guildLvlRef && needValue > 0 && newValue >= needValue)
    		{
    			long addCount = newValue / needValue;
    			newValue = newValue - needValue * addCount;
    			
    			_m_bo.setActivePoint(_m_mgr.getServer().getBM(), newValue);
    			_m_bo.setActivePointTargetLvl(_m_mgr.getServer().getBM(), _m_bo.getLevel());
    			_m_bo.saveAll(_m_mgr.getServer().getBM());
    			
    			//发送宝箱
    			_m_mgrGuildBoxMgr.addBox(0, guildLvlRef.gain_guild_box_id, addCount, _context);
    		}
    		else
    		{
    			_m_bo.saveActivePoint(_m_mgr.getServer().getBM(), newValue);
    		}
    		
    		//推送联盟活跃点
    		getMemberMgr().broadcastMsg(US2GCWriter_042_GuildRelatedOp.make_058_OnGuildActivePointChg(this));
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    /*************************
     *
     * 联盟相关玩家跨服处理操作 开始
     *
     */

    /**
     * 向玩家发送互助奖励
     */
    public void autoHelpDealReward(long _cid, int _dealCount)
    {
        if(0 == _cid)
            return ;

        //获取公会归属UsId
        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);

        GuildMarsAutoHelpDeal_2C rpc = new GuildMarsAutoHelpDeal_2C();
        rpc.req().setCid(_cid);
        rpc.req().setDealCount(_dealCount);

        getGuildMgr().getServer().rpc2us().requestToRepeat(usId, rpc, null, 3
                , ()-> {
                    USLog.error(getGuildMgr().getServer(), "player:{} guild:{} send rpc GuildMarsAutoHelpDeal fail.", _cid, _m_bo.getGuildId());
                });
    }

    /*********
     * 移除成员，通知US进行相关处理
     * @param _cid
     * @param _isKicked
     */
    public void memberRmv(long _cid, boolean _isKicked)
    {
        if(0 == _cid)
            return ;

        //获取公会归属UsId
        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);

        GuildMemberRmv_2C rpc = new GuildMemberRmv_2C();
        rpc.req().setCid(_cid);
        rpc.req().setGuildId(getGuildId());
        rpc.req().setGuildName(getName());
        rpc.req().setTimeMS(CommonFunc.getNowTimeMS());
        rpc.req().setIsKicked(_isKicked);

        getGuildMgr().getServer().rpc2us().requestToRepeat(usId, rpc, null, 3
                , ()-> {
                    USLog.error(getGuildMgr().getServer(), "player:{} guild:{} send rpc memberRmv fail.", _cid, _m_bo.getGuildId());
                });
    }

    /*************************
     *
     * 联盟相关玩家跨服处理操作 结束
     *
     */
}