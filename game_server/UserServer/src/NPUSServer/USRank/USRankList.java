package NPUSServer.USRank;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._IALProcessAction;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.US_Service.Guild.GuildOnScoreChg;
import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_BaseSubItem;
import Common.RankObj.Rank_ItemDump;
import Common.ServerObj.ServerObj_RankObjInfo;
import GS2GC.p017_ActivityOp.GS2GC_017_053_OnActivityRankScoreChg;
import NP2CRS_RB.p001_CrossRankOp.*;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.CommonRank.RankObj;
import NPCommon.CommonRank.RankObjComparer._ARankObjComparer;
import NPCommon.CommonRank.RankSubObj;
import NPCommon.CommonRank._ARankList;
import NPCommon.CommonRank._IRankDBOper;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPEnum.ERankType;
import NPGameRes.Refs.Rank.RefRank;
import NPServerProtocolWriter.NP2CRS.Request.NP2CRS_R_Writer_001_BasicOp;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;
import USDB.Bo.UsRankBO;
import USDB.Bo.UsRankObjBO;
import USDB.Bo.UsRankSubObjBO;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.List;

/*********************
 * US服务器上的排行榜对象
 * @author mj
 *
 */
public class USRankList extends _ARankList
{
    private NPUserServer _m_server;

    //排行榜静态数据对象
    private RefRank _m_rankRef;

    //这里暂时不存储数据库对象，因为数据库对象基本无改动需求。这里将数据剥离出来存储
    //跨服分组实例ID
    private long _m_lCrossInstanceId;
    //跨服服务器ID
    private int _m_iCrossServerId;

    //是否正在变更跨服实例
    private boolean _m_bisProcessChgCross;
    
    //存储数据库对象，方便一些数据更改。如释放状态
    private UsRankBO _m_rankBO;

    public USRankList(NPUserServer _server, _IRankDBOper _rankDbOper, UsRankBO _bo, RefRank _rankRef,
            _ARankObjComparer _comparer)
    {
        super(_rankDbOper, _bo.getId(), _rankRef.rank_id
                , _rankRef.rank_num_max < 100 ? 100 : _rankRef.rank_num_max
                        , _comparer);

        _m_server = _server;

        _m_rankRef = _rankRef;

        _m_lCrossInstanceId = _bo.getCrossInstanceId();

        _m_rankBO = _bo;
        //通过规则获取对应的跨服服务器ID
        if(_m_lCrossInstanceId > 0)
        {
            _m_iCrossServerId = CommonFunc.parseCrossRankServerTypeId(_m_lCrossInstanceId);
        }
    }

    public NPUserServer getUSServer() {return _m_server;}
    public RefRank getRankRefCommon() {return _m_rankRef;}
    
    //判断是否有效，无效的情况下外围会调用关闭函数
    public boolean isEnable() {return !_m_rankBO.getIsDiscard();}

    /******************
     * 创建一个新数据对象返回，在进行子集排行处理的时候不会通过本函数创建排行数据对象
     * @param _objId
     * @param _score
     * @param _rank
     * @return
     */
    @Override
    protected RankObj _createRankObj(long _objId, long _scoreSourceId, long _score, int _rank)
    {
        BM bmObj = getUSServer().getBM();

        //创建数据BO
        UsRankObjBO bo = new UsRankObjBO();
        bo.setInstanceId(bmObj, getInstanceId());
        bo.setRankId(bmObj, getRankId());
        bo.setObjId(bmObj, _objId);
        bo.setScoreSourceId(bmObj, _scoreSourceId);
        bo.setScore(bmObj, _score);
        bo.setUpdatedMs(bmObj, CommonFunc.getNowTimeMS());
        bo.insert(bmObj);

        return new RankObj(bo.getId(), bo.getObjId(), bo.getScoreSourceId(), bo.getScore(), bo.getUpdatedMs(), _rank);
    }

    /**
     * 创建一个新的子集数据对象返回
     * @param _objId
     * @param _subObjId
     * @param _scoreSourceId
     * @param _score
     * @return
     */
    @Override
    protected RankSubObj _createRankSubObj(long _objId, long _subObjId, long _scoreSourceId, long _score)
    {
        BM bmObj = getUSServer().getBM();

        //创建数据BO
        UsRankSubObjBO bo = new UsRankSubObjBO();
        bo.setInstanceId(bmObj, getInstanceId());
        bo.setRankId(bmObj, getRankId());
        bo.setObjId(bmObj, _objId);
        bo.setSubObjId(bmObj, _subObjId);
        bo.setScoreSourceId(bmObj, _scoreSourceId);
        bo.setScore(bmObj, _score);
        bo.setUpdatedMs(bmObj, CommonFunc.getNowTimeMS());
        bo.insert(bmObj);

        return new RankSubObj(bo.getId(), bo.getSubObjId(), bo.getScoreSourceId(), bo.getScore(), bo.getUpdatedMs());
    }
    
    /***************
     * 在本排行开启的时候处理的函数
     * 
     * 如果因为锁而无法处理，则需要开启Syn任务处理（注意Syn任务的时序是不确定的，需要根据实际的状态做处理）
     */
    @Override
    protected void _onRankCreated_inLock()
    {
        //这里需要开启相关事件监听机制
        //如果因为锁而无法处理，则需要开启Syn任务处理（注意Syn任务的时序是不确定的，需要根据实际的状态做处理）
        //如果已经无效则不进行注册处理
        if(!isEnable())
            return ;
        
        //判断是否已经注册到跨服，如未注册到跨服，且需要注册的需要执行注册处理
        if(_m_lCrossInstanceId > 0 &&  !_m_rankBO.getHasReg())
        {
            _makeSureRegCross();
        }
    }

    /*************
     * 确保注册到跨服排行
     */
    private void _makeSureRegCross()
    {
        if(_m_lCrossInstanceId <= 0)
            return ;

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal()
                , _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_003_RegCrossRank(_m_lCrossInstanceId, _m_rankRef.Id(), getUSServer().getServerTypeId())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        //NP2CRS_RB_001_001_RegCrossRank proto = (NP2CRS_RB_001_001_RegCrossRank) _proto;
                        
                        USLog.info(getUSServer(), "reg cross rank:{} instanceId:{} crossInstanceId:{} crossServerId:{} suc."
                                , _m_rankRef.Id(), getInstanceId(), _m_lCrossInstanceId, _m_iCrossServerId);

                        //设置已经注册完成
                        _m_rankBO.saveHasReg(getUSServer().getBM(), true);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(getUSServer(), "reg cross rank:{} instanceId:{} crossInstanceId:{} crossServerId:{} fail - retry after 3 sec"
                                , _m_rankRef.Id(), getInstanceId(), _m_lCrossInstanceId, _m_iCrossServerId);

                        //3秒后再次发起处理
                        ALSynTaskManager.getInstance().regTask(()->
                        {
                            _makeSureRegCross();
                        }, 3000);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_003_RegCrossRank();
                    }
                });
    }
    
    /*****************
     * 在本排行关闭的时候触发的事件函数
     * 
     * 如果因为锁而无法处理，则需要开启Syn任务处理（注意Syn任务的时序是不确定的，需要根据实际的状态做处理）
     */
    @Override
    protected void _onRankClose_inLock()
    {
        //这里需要关闭相关事件监听机制
        //如果因为锁而无法处理，则需要开启Syn任务处理（注意Syn任务的时序是不确定的，需要根据实际的状态做处理）

    }

    /**
     * 排行对象被动变更排名时触发的锁内事件
     * 注意：这里的逻辑不是自己主动改变的情况，是被动改变的时候触发的
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     *
     * 功能：推送排名变动通知给客户端
     *
     * @param _rankObj 被动变更排名的对象
     * @param _preRank 原排名
     */
    @Override
    protected void _onRankObjRankBeChanged_inLock(RankObj _rankObj, int _preRank)
    {
        if (_preRank <= 10)
        {
        	if(_m_rankRef.rank_type == ERankType.PLAYER)
        	{
                // 通过syn任务异步推送，避免在锁内执行网络操作
                ALSynTaskManager.getInstance().regTask(() -> {
                    _sendRankScoreChgToPlayer(_rankObj.getObjId(), _preRank, _rankObj.getRank(), _rankObj.getScore());
                });
        	}
        	else if(_m_rankRef.rank_type == ERankType.GUILD)
        	{
                // 通过syn任务异步推送，避免在锁内执行网络操作
                ALSynTaskManager.getInstance().regTask(() -> {
                    _sendRankScoreChgToGuild(_rankObj.getObjId(), _preRank, _rankObj.getRank(), _rankObj.getScore());
                });
        	}
        }
    }

    /**
     * 排行对象主动变更排名时触发的锁内事件
     * 注意：这里是对象因自身分数变化导致排名变更
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     *
     * 功能：推送排名变动通知给客户端
     *
     * @param _rankObj 主动变更排名的对象
     * @param _preRank 原排名
     */
    @Override
    protected void _onRankObjRankChanged_inLock(RankObj _rankObj, int _preRank)
    {
        if (_m_rankRef.rank_type == ERankType.PLAYER)
        {
            // 通过syn任务异步推送，避免在锁内执行网络操作
            ALSynTaskManager.getInstance().regTask(() -> {
                _sendRankScoreChgToPlayer(_rankObj.getObjId(), _preRank, _rankObj.getRank(), _rankObj.getScore());
            });
        }
        else if (_m_rankRef.rank_type == ERankType.GUILD)
        {
            // 通过syn任务异步推送，避免在锁内执行网络操作
            ALSynTaskManager.getInstance().regTask(() -> {
                _sendRankScoreChgToGuild(_rankObj.getObjId(), _preRank, _rankObj.getRank(), _rankObj.getScore());
            });
        }
    }

    /**
     * 发送排行榜分数变更通知给指定玩家
     *
     * 执行流程：
     * 1. 根据cid查找玩家数据
     * 2. 检查玩家是否在线
     * 3. 构造并发送GS2GC_017_053_OnActivityRankScoreChg协议
     *
     * @param _cid 玩家CID
     * @param _oriRank 原排名
     * @param _curRank 当前排名
     * @param _score 当前分数
     */
    private void _sendRankScoreChgToPlayer(long _cid, int _oriRank, int _curRank, long _score)
    {
        _AActivityBase activityBase = _m_server.getCommActivityMgr().lookupActivityByRankInstanceId(getInstanceId());
        if (activityBase == null)
            return;

        // 获取玩家数据
        NPUSUserData userData = _m_server.getUsUserMgr().lookupCacheUserData(_cid);

        // 检查玩家是否在线
        if (userData == null)
            return;

        // 构造并发送协议
        userData.sendMsgToGC(new GS2GC_017_053_OnActivityRankScoreChg(
                activityBase.getInstanceId(),    // 活动实例ID
                getRankId(),        // 排行榜ID
                _oriRank,           // 原排名
                _curRank,           // 当前排名
                _score              // 当前分数
        ));
    }
    
    /**
     * 发送排行榜分数变更通知给指定公会的所有玩家
     * 
     * @param _guildId
     * @param _oriRank
     * @param _curRank
     * @param _score
     */
    private void _sendRankScoreChgToGuild(long _guildId, int _oriRank, int _curRank, long _score)
    {
    	_AActivityBase activityBase = _m_server.getCommActivityMgr().lookupActivityByRankInstanceId(getInstanceId());
        if (activityBase == null)
            return;
    	
        GuildInfo guild = _m_server.getGuildMgr().lookupGuild(_guildId);
        if(null == guild)
        	return;
        
        GS2GC_017_053_OnActivityRankScoreChg proto = new GS2GC_017_053_OnActivityRankScoreChg(
                activityBase.getInstanceId(),    // 活动实例ID
                getRankId(),        // 排行榜ID
                _oriRank,           // 原排名
                _curRank,           // 当前排名
                _score              // 当前分数
        );
        
        guild.getMemberMgr().broadcastMsg(proto);
    }

    /**
     * 排行榜元素变更通知
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     * 
     * @param _rankObj    变更后的排行榜元素 如果需要新分数或新排名，可以从对象中直接取
     * @param _oldScore   旧分数
     * @param _oldRank    旧排行
     * @param _m_cContext
     */
    @Override
    protected void _onRankObjChg_inLock(RankObj _rankObj, long _oldScoreSourceId, long _oldScore, int _oldRank, _IContext _m_cContext)
    {
        //非跨服排行榜
        if(_m_iCrossServerId <= 0)
        {
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal()
                , _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_010_SetCrossRankScore(_m_lCrossInstanceId, getRankId(),
                        _rankObj.getObjId(), _rankObj.getScoreSourceId(), _rankObj.getScore(), _rankObj.getUpdatedMs())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_010_SetCrossRankScore();
                    }
                });
    }
    @Override
    protected void _onRankSubObjChg_inLock(RankObj _rankObj, RankSubObj _rankSubObj, long _oldScoreSourceId, long _oldScore, int _oldRank, _IContext _m_cContext)
    {
        //非跨服排行榜
        if(_m_iCrossServerId <= 0)
        {
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal()
                , _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_011_SetCrossRankSubScore(_m_lCrossInstanceId, getRankId(),
                        _rankObj.getObjId(), _rankSubObj.getSubObjId(), _rankSubObj.getScoreSourceId(),
                        _rankSubObj.getScore(), _rankSubObj.getUpdatedMs())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_011_SetCrossRankSubScore();
                    }
                });
    }

    
    /******************
     * 设置这个排行榜需要被销毁
     * 
     * 后续子类应该根据实际排行榜情况直接删除或者向跨服排行榜服务器发送请求，并在确认跨服排行接收到删除请求后本地才可以删除
     * 如果是总排行，可能需要等待所有子对象清理完毕后才能进行删除处理
     * 
     * @param _realDiscardAction 实际处理删除数据的处理对象
     */
    @Override
    protected void _setRankListNeedDiscard(_IALProcessAction _realDiscardAction)
    {
        //设置本排行榜的状态为释放状态，并根据是否跨服子榜发送请求处理。
        _m_rankBO.saveIsDiscard(getUSServer().getBM(), true);
        
        //在请求正常处理后，本服的排行榜会直接调用回调释放
        if(_m_iCrossServerId <= 0)
        {
            //无跨服处理，直接释放
            if(null != _realDiscardAction)
                _realDiscardAction.dealAction();
            
            return ;
        }
        
        //进行跨服处理
        _unregCrossRankServer(_realDiscardAction);
    }

    /**
     * 注销跨服排行服务器中信息的处理函数，单独处理可能存在回调的重复调用
     * @param _realDiscardAction 实际处理删除数据的处理对象
     */
    protected void _unregCrossRankServer(_IALProcessAction _realDiscardAction)
    {
        //进行跨服处理
        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal()
                , _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_004_UnregCrossRank(_m_lCrossInstanceId, getRankId(), getUSServer().getServerTypeId())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        USLog.info(getUSServer(), "unreg cross:{} rank:{} instanceId:{} crossServerId:{} suc."
                                , _m_lCrossInstanceId, _m_rankRef.Id(), getInstanceId(), _m_iCrossServerId);

                        _realDiscardAction.dealAction();
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(getUSServer(), "unreg cross:{} rank:{} instanceId:{} crossServerId:{} fail."
                                , _m_lCrossInstanceId, _m_rankRef.Id(), getInstanceId(), _m_iCrossServerId);
                        
                        //3秒后再次发起处理
                        ALSynTaskManager.getInstance().regTask(()->
                        {
                            _unregCrossRankServer(_realDiscardAction);
                        }, 3000);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_004_UnregCrossRank();
                    }
                });
    }

    /**
     * 分数变更处理
     * @param _cid      玩家ID
     * @param _chgValue 变更值
     */
    public void onScoreChg(long _cid, long _scoreSourceId, long _chgValue)
    {
        if (_m_rankRef == null)
            return;

        //区分玩家/团队提交分数
        if (_m_rankRef.rank_type == ERankType.GUILD) //玩家组织排行榜
        {
            NPUSUserData userData = _m_server.getUsUserMgr().lookupCacheUserData(_cid);
            if(null != userData)
            {
                userData.safeCall(() -> {
                    //发送RPC到Guild服务器处理
                    GuildOnScoreChg rpc = new GuildOnScoreChg();
                    rpc.req().setRankInstanceId(getInstanceId());
                    rpc.req().setCid(_cid);
                    rpc.req().setGuildId(userData.getGuildComponent().getGuildId());
                    rpc.req().setScoreSourceId(_scoreSourceId);
                    rpc.req().setChgValue(_chgValue);

                    int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(userData.getGuildComponent().getGuildId());

                    getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                            , ()-> {
                                USLog.error(getUSServer(), "player:{} guild:{} send rpc GuildOnScoreChg fail.", _cid, userData.getGuildComponent().getGuildId());
                            });
                });
            }
            else {
                PlayerCacheFunc.getData(getUSServer(), _cid, new HandlerTwo<Boolean, UserOfflineTmpDataInfo_PlayerCache>()
                {
                    @Override
                    public void handle(Boolean _isSuc, UserOfflineTmpDataInfo_PlayerCache _cacheInfo)
                    {
                        if (!_isSuc || _cacheInfo == null)
                            return ;

                        //发送RPC到Guild服务器处理
                        GuildOnScoreChg rpc = new GuildOnScoreChg();
                        rpc.req().setRankInstanceId(getInstanceId());
                        rpc.req().setCid(_cid);
                        rpc.req().setGuildId(_cacheInfo.getPlayerCache().getGuildInfo().getGuildId());
                        rpc.req().setScoreSourceId(_scoreSourceId);
                        rpc.req().setChgValue(_chgValue);

                        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_cacheInfo.getPlayerCache().getGuildInfo().getGuildId());

                        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                                , ()-> {
                                    USLog.error(getUSServer(), "player:{} guild:{} send rpc GuildOnScoreChg fail.", _cid, userData.getGuildComponent().getGuildId());
                                });
                    }
                });
            }
        } else if (_m_rankRef.rank_type == ERankType.PLAYER)//玩家排行榜
        {
            if (_m_rankRef.is_set)
            {
                setScore(_cid, _scoreSourceId, _chgValue, _m_rankRef.set_greater, null);
            } else
            {
                chgScore(_cid, _scoreSourceId, _chgValue, null);
            }
        }
    }
    public void onRPCGuildScoreChg(long _cid, long _guildId, long _scoreSourceId, long _chgValue)
    {
        if (_m_rankRef == null)
            return;

        //区分玩家/团队提交分数
        if (_m_rankRef.rank_type == ERankType.GUILD) //玩家组织排行榜
        {
            if (_m_rankRef.is_set)
            {
                setSubScore(_guildId, _cid, _scoreSourceId, _chgValue, _m_rankRef.set_greater, null);
            } else
            {
                chgSubScore(_guildId, _cid, _scoreSourceId, _chgValue, null);
            }
        }
    }

    /**
     * 构造跨服排行榜基础数据列表
     * @param _limit   限制数量
     * @param _callback 回调
     */
    public void makeCrossRankBaseList(int _limit, _ICallBackResultT<List<Rank_BaseItem>> _callback)
    {
        if (_m_lCrossInstanceId <= 0)
        {
            _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal(), _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_021_GetCrossRankBaseList(_m_lCrossInstanceId, getRankId(), _limit)
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_021_GetCrossRankBaseList();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CRS_RB_001_021_GetCrossRankBaseList proto = (NP2CRS_RB_001_021_GetCrossRankBaseList) _proto;
                        _callback.onRunOver(Result.SUCC, proto.getRankItemList());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                    }
                });
    }

    /**
     * 构造跨服排行榜基础数据列表
     * @param _limitRank 限制数量
     * @param _callback  回调
     */
    public void dumpCrossRankBaseListByUs(int _usId, int _limitRank, _ICallBackResultT<List<Rank_ItemDump>> _callback)
    {
        if (_m_lCrossInstanceId <= 0)
        {
            _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal(), _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_024_DumpCrossRankListByUs(_m_lCrossInstanceId, getRankId(), _usId, _limitRank)
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_024_DumpCrossRankListByUs();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CRS_RB_001_024_DumpCrossRankListByUs proto = (NP2CRS_RB_001_024_DumpCrossRankListByUs) _proto;
                        _callback.onRunOver(Result.SUCC, proto.getRankItemList());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                    }
                });
    }

    /**
     * 构造跨服排行榜基础数据 通过排名
     * @param _rank   排名
     * @param _callback 回调
     */
    public void makeCrossRankBaseByRank(int _rank, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        if (_m_lCrossInstanceId <= 0)
        {
            _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal(), _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_022_GetCrossRankBaseByRank(_m_lCrossInstanceId, getRankId(), _rank)
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_022_GetCrossRankBaseByRank();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CRS_RB_001_022_GetCrossRankBaseByRank proto = (NP2CRS_RB_001_022_GetCrossRankBaseByRank) _proto;
                        _callback.onRunOver(Result.SUCC, proto.getRankItem().getKey() == 0 ? null : proto.getRankItem());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                    }
                });
    }

    /**
     * 构造跨服排行榜基础数据 通过key
     * @param _key  key
     * @param _callback 回调
     */
    public void makeCrossRankBaseByKey(long _key, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        if (_m_lCrossInstanceId <= 0)
        {
            _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal(), _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_023_GetCrossRankBaseByKey(_m_lCrossInstanceId, getRankId(), _key)
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_023_GetCrossRankBaseByKey();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CRS_RB_001_023_GetCrossRankBaseByKey proto = (NP2CRS_RB_001_023_GetCrossRankBaseByKey) _proto;

                        _callback.onRunOver(Result.SUCC, proto.getRankItem());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                    }
                });
    }
    
    public void makeCrossRankBaseByKey2(long _key, long _subKey, _ICallBackResultT<Rank_BaseItem> _callback)
    {
        if (_m_lCrossInstanceId <= 0)
        {
            _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal(), _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_025_GetCrossRankBaseByKey2(_m_lCrossInstanceId, getRankId(), _key, _subKey)
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_025_GetCrossRankBaseByKey2();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CRS_RB_001_025_GetCrossRankBaseByKey2 proto = (NP2CRS_RB_001_025_GetCrossRankBaseByKey2) _proto;

                        _callback.onRunOver(Result.SUCC, proto.getRankItem());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                    }
                });
    }
    
    public void makeRankBaseSubListByKey(long _key, _ICallBackResultT<List<Rank_BaseSubItem>> _callback)
    {
        if (_m_lCrossInstanceId <= 0)
        {
            _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal(), _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_026_GetCrossRankBaseSubListByKey(_m_lCrossInstanceId, getRankId(), _key)
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_026_GetCrossRankBaseSubListByKey();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                    	NP2CRS_RB_001_026_GetCrossRankBaseSubListByKey proto = (NP2CRS_RB_001_026_GetCrossRankBaseSubListByKey) _proto;

                        _callback.onRunOver(Result.SUCC, proto.getSubList());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                    }
                });
    }

    /**
     * 获取跨服排行榜列表大小
     * @param _callback 回调
     */
    public void getCrossRankListSize(_ICallBackResultT<Integer> _callback)
    {
        if (_m_lCrossInstanceId <= 0)
        {
            _callback.onRunOver(CommErr.SYSTEM_UNLOCK, null);
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal(), _m_iCrossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_020_GetCrossRankListSize(_m_lCrossInstanceId, getRankId())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_020_GetCrossRankListSize();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CRS_RB_001_020_GetCrossRankListSize proto = (NP2CRS_RB_001_020_GetCrossRankListSize) _proto;
                        _callback.onRunOver(Result.SUCC, proto.getRankSize());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                    }
                });
    }

    /**
     * 跨服排行榜实例变更
     */
    public void chgCrossInstance(long _newCrossInstanceId)
    {
        //如果新的跨服实例ID和当前的跨服实例ID相同，则不进行处理
        //如果已经设置了新的跨服实例ID，则不进行处理
        if (_m_lCrossInstanceId == _newCrossInstanceId)
            return;

        _lock();
        try{
            //检查是否正在处理切换
            if (_m_bisProcessChgCross)
            {
                USLog.error(getUSServer(), "rank chgCrossInstance is processing. rank:{} instanceId:{}", _m_rankRef.Id(), getInstanceId());
                return;
            }

            //切换跨服
            _dealChgCross(_newCrossInstanceId);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 确保变更跨服分组
     */
    public void _dealChgCross(long _newCrossInstanceId)
    {
        //如果正在处理切换，则不进行处理
        if (_m_bisProcessChgCross)
            return;

        //标记为正在切换
        _m_bisProcessChgCross = true;

        //开启处理
        ALProcess process = ALProcess.CreateProcess("USRankList chgCrossInstance");

        //如果已经注册到跨服，需要先进行注销
        if (_m_lCrossInstanceId != 0)
        {
            process.addDelegateProcess(_action -> _unregCrossRankServer(()->
            {
                _m_lCrossInstanceId = 0;
                _m_iCrossServerId = 0;

                _m_rankBO.setCrossInstanceId(getUSServer().getBM(), 0);
                _m_rankBO.setHasReg(getUSServer().getBM(), false);
                _m_rankBO.saveAllMarked(getUSServer().getBM());

                _action.dealAction();
            }));
        }

        //判断是否需要注册新的跨服
        if (_newCrossInstanceId != 0)
        {
            //1.注册新的跨服
            process.addDelegateProcess(_action -> _regNewCrossRankServer(_newCrossInstanceId, _action), null, false);

            //2.更新所有数据到CRS
            _updateAllDataToCRS(_newCrossInstanceId, process);

            //3.设置新的跨服实例ID
            process.addActionProcess(new _IALProcessAction()
            {
                @Override
                public void dealAction()
                {
                    _lock();
                    try
                    {
                        _m_lCrossInstanceId = _newCrossInstanceId;
                        _m_iCrossServerId = CommonFunc.parseCrossRankServerTypeId(_m_lCrossInstanceId);
                    } finally
                    {
                        _unlock();
                    }

                    _m_rankBO.setCrossInstanceId(getUSServer().getBM(), _m_lCrossInstanceId);
                    _m_rankBO.setHasReg(getUSServer().getBM(), true);
                    _m_rankBO.saveAllMarked(getUSServer().getBM());
                }
            });
        }

        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                getUSServer().getDDAlert().err("chgCrossInstance process stop.", "chgCrossInstance process stop, rank:{} instanceId:{} crossInstanceId:{} crossServerId:{}"
                        , _m_rankRef.Id(), getInstanceId(), _m_lCrossInstanceId, _m_iCrossServerId);

                _m_bisProcessChgCross = false;
            }

            @Override
            public void onRootProecssSuc()
            {
                _lock();
                try{
                    _m_bisProcessChgCross = false;
                }finally
                {
                    _unlock();
                }
            }
        });
    }

    /**
     * 注册跨服排行服务器中信息的处理函数，单独处理可能存在回调的重复调用
     * @param _action 注册完成后的处理对象
     */
    protected void _regNewCrossRankServer(long _crossInstanceId,_IALProcessAction _action)
    {
        int crossServerId = CommonFunc.parseCrossRankServerTypeId(_crossInstanceId);

        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal()
                , crossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_003_RegCrossRank(_crossInstanceId, _m_rankRef.Id(), getUSServer().getServerTypeId())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        //NP2CRS_RB_001_001_RegCrossRank proto = (NP2CRS_RB_001_001_RegCrossRank) _proto;

                        USLog.info(getUSServer(), "reg new cross rank:{} instanceId:{} crossInstanceId:{} crossServerId:{} suc."
                                , _m_rankRef.Id(), getInstanceId(), _crossInstanceId, crossServerId);

                        _action.dealAction();
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(getUSServer(), "reg new cross rank:{} instanceId:{} crossInstanceId:{} crossServerId:{} errCode:{} fail - retry after 3 sec"
                                , _m_rankRef.Id(), getInstanceId(), _crossInstanceId, crossServerId, _errCode);

                        //3秒后再次发起处理
                        ALSynTaskManager.getInstance().regTask(()->
                        {
                            _regNewCrossRankServer(_crossInstanceId, _action);
                        }, 3000);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_003_RegCrossRank();
                    }
                });
    }

    /**
     * 同步所有本地数据到CRS
     */
    protected void _updateAllDataToCRS(long _newCrossInstanceId, ALProcess _process)
    {
        //获取所有排行榜对象
        List<ServerObj_RankObjInfo> allRankObjInfoList = makeAllRankObjInfoList();

        //需要进行分页处理, 每页100个
        int pageSize = 100;
        int pageCount = (allRankObjInfoList.size() + pageSize - 1) / pageSize;

        int crossServerId = CommonFunc.parseCrossRankServerTypeId(_newCrossInstanceId);

        ALProcess[] list = new ALProcess[pageCount];
        for (int i = 0; i < pageCount; i++)
        {
            int index = i;

            list[i] = ALProcess.CreateProcess("group_push_process_sub");
            list[i].addDelegateProcess(_action ->
                    USRankList.this._updateDataToCRS(crossServerId, _newCrossInstanceId, index, pageSize, allRankObjInfoList, _action), "group_push_process_" + i);
        }

        _process.addMultiProcess("group_push_process_main",list);
    }

    /**
     * 分组推送
     */
    protected void _updateDataToCRS(int _crossServerId, long _newCrossInstanceId, int _index, int _pageSize,
                                    List<ServerObj_RankObjInfo> _rankList, _IALProcessAction _action)
    {
        //分页处理
        int start = _index * _pageSize;
        int end = Math.min((_index + 1) * _pageSize, _rankList.size());

        //发送请求
        getUSServer().sendRequestToBSServer(EServerType.CROSS_RANK.ordinal()
                , _crossServerId
                , NP2CRS_R_Writer_001_BasicOp.make_008_RegUploadRankData(_newCrossInstanceId, _m_rankRef.Id(), _rankList.subList(start, end))
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_008_RegUploadRankData();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        _action.dealAction();
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(getUSServer(), "update data to cross rank:{} instanceId:{} crossInstanceId:{} crossServerId:{} fail"
                                , _m_rankRef.Id(), getInstanceId(), _newCrossInstanceId, _crossServerId);

                        //3秒后再次发起处理
                        ALSynTaskManager.getInstance().regTask(()-> _updateDataToCRS(_crossServerId, _newCrossInstanceId, _index, _pageSize, _rankList, _action), 3000);
                    }
                });
    }
}
