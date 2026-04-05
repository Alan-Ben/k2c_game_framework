package NPUSServer.CommonActivityMgr.Core;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ActivityEnum.EActivityState;
import Common.ActivityObj.Activity_Info;
import Common.ActivityObj.Activity_PlayerData;
import Common.ActivityObj.Activity_ShopInfo;
import Common.CommonFuncObj.CrystalGiftPack_Info;
import CommonEnum.EActivityItemExpireTimeType;
import EventSystem.NPHandlerEntry;
import NPCommon.Enum.NPServerEnum.ENPUserDataState;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerParam;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Activity.RefActivity;
import NPGameRes.Refs.Activity.RefActivityEvent;
import NPGameRes.Refs.Activity.RefActivityTeam;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.StepReward.RefStepRewardSet;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.Common.Event.Events.Event_P_CROSS_DAY;
import NPUSServer.CommonActivityMgr.CommonActivityMgr;
import NPUSServer.CommonActivityMgr.CommonActivityMgr.ActivityStepRewardBaseInfo;
import NPUSServer.CommonActivityMgr.Core.ActivityState._AActivityState;
import NPUSServer.CommonActivityMgr.Core.CrystalGiftPack.ActivityCrystalGiftPackMgr;
import NPUSServer.CommonActivityMgr.Core.GameLogicDealer.ActivityGameLogicDealer;
import NPUSServer.CommonActivityMgr.Core.Rank.ActivityRankInfo;
import NPUSServer.CommonActivityMgr.Core.Shop.ActivityShopMgr;
import NPUSServer.CommonActivityMgr.Core.StepReward.ActivityStepRewardEventTaskInfo;
import NPUSServer.CommonActivityMgr.Core.StepReward.ActivityStepRewardInfo;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPEvent.EventMgr.EventObj._INPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;
import NPUSServer.NPUserServer;
import NPUSServer.TeamActivityMgr.TeamActivityInfo;
import NPUSServer.USLog;
import NPUSServer.UsActivityScheduleMgr.UsActivityScheduleInfo;
import USDB.Bo.*;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
public abstract class _AActivityBase implements _IHandlerHolder
{
    private NPUserServer _m_server;

    private UsActivityScheduleInfo _m_scheduleInfo;
    //活动配置
    private RefActivity _m_ref;
    //活动基础数据BO
    private ActivityBaseBO _m_bo;
    //活动状态机
    private ActivityStateMachine _m_machine;
    //排行榜数据列表
    private ArrayList<ActivityRankInfo> _m_alRankList;
    //阶段奖励数据列表
    private ArrayList<ActivityStepRewardInfo> _m_alStepRewardList;
    //玩家商店管理器
    private ActivityShopMgr _m_shopMgr;
    //组队功能
    private ActivityTeamDealer _m_teamDealer;
    //GameLogicDealer
    private ActivityGameLogicDealer _m_gameLogicDealer;
    //玩家钻石礼包管理器
    private ActivityCrystalGiftPackMgr _m_crystalGiftPackMgr;
    //结算时联盟盟主数据
    private ActivitySettleGuildMgr _m_mgrSettleGuildMgr;
    //结算时队伍队长数据
    private ActivitySettleTeamMgr _m_mgrSettleTeamMgr;
    //锁对象a
    private MutexAtom _m_mutex;

    //活动子配置：触发配置
    private RefActivityEvent _m_refEvent;
    private NPHandlerEntry<_INPGlobalUserEventObj> _m_evtEntry;

    public static final List<EActivityState> RUNNING_STATE = Arrays.asList(
            EActivityState.PLAYING,
            EActivityState.SETTLING,
            EActivityState.REWARDING
    );

    public static final List<EActivityState> CLOSE_STATE = Arrays.asList(
            EActivityState.CLOSED,
            EActivityState.CAN_DISCARD
    );

    protected _AActivityBase(NPUserServer _server, ActivityBaseBO _bo)
    {
        _m_server = _server;

        _m_bo = _bo;
        _m_machine = new ActivityStateMachine(this);
        _m_alRankList = new ArrayList<>();
        _m_alStepRewardList = new ArrayList<>();
        _m_shopMgr = new ActivityShopMgr(this);
        _m_crystalGiftPackMgr = new ActivityCrystalGiftPackMgr(this);
        _m_mgrSettleGuildMgr = new ActivitySettleGuildMgr(this, _m_bo);
        _m_mgrSettleTeamMgr = new ActivitySettleTeamMgr(this, _m_bo);

        _m_mutex = new MutexAtom();
    }

    /**
     * 初始化活动队伍处理对象
     */
    private void _initTeamDealer()
    {
        RefActivityTeam ref = RefActivityTeam.getMgr().get(getActivityId());
        if(null == ref)
            return;

        _m_teamDealer = new ActivityTeamDealer(this, ref);

        // 向跨服队伍服务器注册当前 US，使其纳入分组管理
        _m_teamDealer._regUs();
    }

    /**
     * 初始化活动游戏逻辑处理对象
     */
    private void _initGameLogicDealer()
    {
        if(getGameLogicInstanceId() > 0)
        {
            _m_gameLogicDealer = new ActivityGameLogicDealer(this);

            // 向游戏逻辑服务器注册当前 US，使其纳入广播管理
            _m_gameLogicDealer._regUs();
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
    //这部分锁留给子系统使用
    protected void _activityLock()
    {
    	_lock();
    }
    protected void _activityUnlock()
    {
    	_unlock();
    }

    public NPUserServer getUSServer() {return _m_server;}

    public CommonActivityMgr getActivityMgr()
    {
        return _m_server.getCommActivityMgr();
    }

    public final RefActivity getRef()
    {
        return _m_ref;
    }

    public final ActivityBaseBO getBo()
    {
        return _m_bo;
    }

    public final long getInstanceId()
    {
        return _m_bo.getId();
    }

    public final long getActivityId()
    {
        return _m_ref.activity_id;
    }

    public final long getCrossInstanceId()
    {
        return _m_bo.getCrossInstanceId();
    }

    public final long getGameLogicInstanceId()
    {
        return _m_bo.getGameLogicInstanceId();
    }

    public UsActivityScheduleInfo getSchedule()
    {
        return _m_scheduleInfo;
    }

    public ActivityShopMgr getShopMgr()
    {
        return _m_shopMgr;
    }

    public ActivityCrystalGiftPackMgr getCrystalGiftPackMgr()
    {
        return _m_crystalGiftPackMgr;
    }

    public ActivitySettleGuildMgr getSettleGuildMgr()
    {
        return _m_mgrSettleGuildMgr;
    }

    public ActivitySettleTeamMgr getSettleTeamMgr() {return _m_mgrSettleTeamMgr;}

    public ActivityTeamDealer getTeamDealer()
    {
        return _m_teamDealer;
    }

    public ActivityGameLogicDealer getGameLogicDealer()
    {
        return _m_gameLogicDealer;
    }

    /*******************
     * 通过US列表来检查是否跨服
     * @return
     */
    public final boolean isCross()
    {
        return getCrossInstanceId() > 0;
    }

    /**
     * 检查活动是否在运行中
     * @return
     */
    public boolean isRunning()
    {
        return RUNNING_STATE.contains(getCurState().getStateType());
    }

    /**
     * 检查活动是否在关闭中
     * @return
     */
    public boolean isClosing()
    {
        return CLOSE_STATE.contains(getCurState().getStateType());
    }

    /**
     * 检查是否在指定状态中
     * @param _states
     * @return
     */
    public boolean inState(EActivityState... _states)
    {
        EActivityState curStateType = getCurState().getStateType();
        for (EActivityState state : _states)
        {
            if (curStateType == state)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * 返回活动开启时间
     * @return
     */
    public long getStartTimeMs()
    {
        return getBo().getStartMs();
    }

    /**
     * 返回活动结束时间
     * @return
     */
    public long getEndTimeMs()
    {
        return getBo().getEndMs();
    }

    /**
     * 返回活动结算时间
     * @return
     */
    public long getSettleTimeMs()
    {
        return getBo().getSettleMs();
    }

    /**
     * 返回活动结束时间
     * @return
     */
    public long getCloseTimeMs()
    {
        return getBo().getCloseMs();
    }

    public final ActivityStateMachine getMachine()
    {
        return _m_machine;
    }

    /*******************
     * 获取当前状态机所处的状态
     * @return
     */
    public final _AActivityState getCurState()
    {
        return _m_machine.getCurState();
    }

    /******
     * 活动对象新创建初始化
     * @return
     */
    public boolean initNew(UsActivityScheduleInfo _scheduleInfo)
    {
        _m_ref = RefActivity.getMgr().get(getBo().getActivityId());
        if (null == _m_ref)
        {
            USLog.error(_m_server, "activity:{} initNew not found ref!", getBo().getActivityId());
            return false;
        }

        //预设置到无状态模式
        if (!_m_machine.initSetStatus(EActivityState.PLAN))
        {
            USLog.error(_m_server, "activity:{} instanceId:{} set machine state:{} fail."
                    , getBo().getActivityId(), getBo().getId(), getBo().getCurState());
            return false;
        }

        //子类创建初始化数据
        if (!_subInitNew())
        {
            USLog.error(_m_server, "activity:{} instanceId:{} _subInitNew fail."
                    , getBo().getActivityId(), getBo().getId());
            return false;
        }

        //活动触发配置
        _m_refEvent = RefActivityEvent.getMgr().get(_m_ref.activity_id);
        //关联的排期信息
        _m_scheduleInfo = _scheduleInfo;

        //创建排行榜和阶段奖励对象
        createAllRank();
        createAllStepReward();

        //初始化商店
        _m_shopMgr.initShops();
        _m_crystalGiftPackMgr.initGroups();

        if (_m_ref.rank_id_list.contains(RefGeneral.Ref().mars_power_rank_id))
        {
            // 标记已经开启过火星实力排行榜
            getUSServer().markHadOpenMarsPowerRank();
        }

        //初始化组队功能
        _initTeamDealer();

        //初始化游戏主体处理逻辑功能
        _initGameLogicDealer();

        return true;
    }

    /**********************
     * 初始化活动数据
     * @return
     */
    public boolean s_init(UsActivityScheduleInfo _scheduleInfo)
    {
        _m_ref = RefActivity.getMgr().get(getBo().getActivityId());
        if (null == _m_ref)
        {
            USLog.error(_m_server, "activity:{} s_init not found ref!", getBo().getActivityId());
            return false;
        }

        //根据活动当前状态设置状态机
        if (getBo().getCurState() == EActivityState.PLAN.ordinal())
        {
            //预设置到无状态模式
            if (!_m_machine.initSetStatus(EActivityState.PLAN))
            {
                USLog.error(_m_server, "activity:{} instanceId:{} set machine state:{} fail."
                        , getBo().getActivityId(), getBo().getId(), getBo().getCurState());
                return false;
            }
        } else
        {
            //如果活动已经进行到后续状态，则设置为准备状态
            if (!_m_machine.initSetStatus(EActivityState.LOAD_FROM_DB))
            {
                USLog.error(_m_server, "activity:{} instanceId:{} set machine state:{} fail."
                        , getBo().getActivityId(), getBo().getId(), getBo().getCurState());
                return false;
            }
        }

        //子类静态初始化数据
        if (!_subInitStatic())
        {
            USLog.error(_m_server, "activity:{} instanceId:{} _init fail."
                    , getBo().getActivityId(), getBo().getId());
            return false;
        }

        //活动触发配置
        _m_refEvent = RefActivityEvent.getMgr().get(_m_ref.activity_id);
        //关联的排期信息
        _m_scheduleInfo = _scheduleInfo;

        //初始化商店
        _m_shopMgr.initShops();
        _m_crystalGiftPackMgr.initGroups();

        //初始化组队功能
        _initTeamDealer();

        //初始化游戏主体处理逻辑功能
        _initGameLogicDealer();

        return true;
    }

    /******************
     * 初始化后动排行榜数据
     * @param _bo
     */
    public void initRank(ActivityRankBO _bo)
    {
        ActivityRankInfo rank = new ActivityRankInfo(this, _bo);
        _m_alRankList.add(rank);
    }

    /******************
     * 初始化阶段奖励数据
     * @param _bo
     */
    public void initStepReward(ActivityStepRewardBO _bo)
    {
        ActivityStepRewardInfo stepRewardInfo = new ActivityStepRewardInfo(this, _bo);
        _m_alStepRewardList.add(stepRewardInfo);
    }

    /**
     * 初始化排行榜领取记录
     * @param _recordBo 领取记录
     */
    public void initRankRewardDrawRecord(ActivityRankRewardInfoBO _recordBo)
    {
        ActivityRankInfo rankInfo = lookupRank(_recordBo.getRankId());
        if (rankInfo == null)
        {
            USLog.error(_m_server, "_AActivityBase initRankRewardDrawRecord rankInfo not found, activity:{} instanceId:{} rank:{}"
                    , getBo().getActivityId(), _recordBo.getRankId());
            return;
        }

        rankInfo.initRewardDrawRecord(_recordBo);
    }

    /**
     * 初始化阶段奖励领取记录
     * @param _recordBo 领取记录
     */
    public void initStepRewardDrawRecord(ActivityStepRewardDrawRecordBO _recordBo)
    {
        ActivityStepRewardInfo stepRewardInfo = lookupStepReward(_recordBo.getStepRewardId());
        if (stepRewardInfo == null)
        {
            USLog.error(_m_server, "_AActivityBase initStepRewardDrawRecord stepRewardInfo not found, activity:{} instanceId:{} stepReward:{}"
                    , getBo().getActivityId(), _recordBo.getStepRewardId());
            return;
        }

        stepRewardInfo.initRewardDrawRecord(_recordBo);
    }

    /**
     * 初始化阶段奖励领取记录
     * @param _recordBo 领取记录
     */
    public void initStepRewardMailRecord(ActivityStepRewardMailRecordBO _recordBo)
    {
        ActivityStepRewardInfo stepRewardInfo = lookupStepReward(_recordBo.getStepRewardId());
        if (stepRewardInfo == null)
        {
            USLog.error(_m_server, "_AActivityBase initStepRewardMailRecord stepRewardInfo not found, activity:{} instanceId:{} stepReward:{}"
                    , getBo().getActivityId(), _recordBo.getStepRewardId());
            return;
        }

        stepRewardInfo.initRewardMailRecord(_recordBo);
    }

    /*****************
     * 设置时间戳
     */
    public void cmdEnd()
    {
        _m_bo.saveEndMs(getUSServer().getBM(), CommonFunc.getNowTimeMS() + 1);
        //活动变更推送
        onActivityInfoChg();
    }

    /*****************
     * 设置时间戳
     */
    public void cmdSettle()
    {
        long targetTimeMs = CommonFunc.getNowTimeMS() + 1;
        //需要处理结算时间
        if (_m_bo.getEndMs() > targetTimeMs)
            _m_bo.setEndMs(getUSServer().getBM(), targetTimeMs);
        _m_bo.setSettleMs(getUSServer().getBM(), targetTimeMs);
        _m_bo.saveAllMarked(getUSServer().getBM());
        //活动变更推送
        onActivityInfoChg();
    }

    public void cmdClose()
    {
        long targetTimeMs = CommonFunc.getNowTimeMS() + 1;
        //需要处理结算时间
        if (_m_bo.getEndMs() > targetTimeMs)
            _m_bo.setEndMs(getUSServer().getBM(), targetTimeMs);
        //需要处理结算时间
        if (_m_bo.getSettleMs() > targetTimeMs)
            _m_bo.setSettleMs(getUSServer().getBM(), targetTimeMs);
        //处理结束时间
        _m_bo.setCloseMs(getUSServer().getBM(), targetTimeMs);
        _m_bo.saveAllMarked(getUSServer().getBM());
        //活动变更推送
        onActivityInfoChg();
    }

    public Activity_Info toProto()
    {
        Activity_Info proto = new Activity_Info();
        proto.setInstanceId(getInstanceId());
        proto.setActivityId(getActivityId());
        proto.setStartTimeMs(_m_bo.getStartMs());
        proto.setEndTimeMs(_m_bo.getEndMs());
        proto.setSettleTimeMs(_m_bo.getSettleMs());
        proto.setCloseTimeMs(_m_bo.getCloseMs());
        proto.setState(getCurState().getStateType());
        proto.getUsIdList().addAll(_m_scheduleInfo.getUsIdList());
        proto.setUsGroupId(_m_scheduleInfo.getUsGroupId());
        return proto;
    }

    /**
     * 活动变更推送
     */
    public void onActivityInfoChg()
    {
        Activity_Info proto = toProto();
        ALSynTaskManager.getInstance().regTask(() ->
        {
            getUSServer().getUsUserMgr().broadCastMessage(US2GCWriter_017_ActivityOp.make_051_OnActivityChg(proto));
        });
    }

    /*******************
     * 检查并更新当前活动状态
     * @param _nowMs
     */
    public final void checkState(long _nowMs)
    {
        //查询下一个状态
        _AActivityState nextState = getCurState().getCanTransToState(_nowMs);
        //如果存在下一个状态则切换
        if (nextState == null)
            return;

        //尝试切换状态
        getMachine().transToState(nextState);
    }

    /**
     * 保存活动状态
     * @param _targetState 目标状态
     */
    public void saveState(EActivityState _targetState)
    {
        //更新状态
        getBo().saveCurState(getUSServer().getBM(), _targetState.ordinal());
    }

    /***********
     * 运行中状态
     * @return
     */
    public final boolean isPlaying()
    {
        return EActivityState.PLAYING == getCurState().getStateType();
    }

    /************
     * 冻结中状态
     * @return
     */
    public final boolean isRewarding()
    {
        return EActivityState.REWARDING == getCurState().getStateType();
    }

    /*************
     * 已关闭状态
     * @return
     */
    public final boolean isClosed()
    {
        return EActivityState.CLOSED == getCurState().getStateType();
    }

    /*************
     * 已完成状态
     * @return
     */
    public final boolean canDiscard()
    {
        return EActivityState.CAN_DISCARD == getCurState().getStateType();
    }

    /*************
     * 是否可以转到已完成状态
     * @return
     */
    public boolean isRankAndStepRewardClosed()
    {
        return _m_alRankList.isEmpty() && _m_alStepRewardList.isEmpty();
    }

    /**
     * 活动开启时
     */
    public void onActivityStart()
    {
        //实例对象开启后操作
        _onActivityStart();
        //补偿活动开启前已触发的跨天事件（活动注册事件监听前，在线玩家已触发跨天事件导致漏算）
        _compensateMissedCrossDayEvent();
    }

    /**
     * 活动冻结时
     */
    public void onActivityEnd()
    {
        //实例对象冻结后操作
        _onActivityEnd();
    }

    /**
     * 活动关闭时
     */
    public void onActivityClosed()
    {
        //实例对象关闭后操作
        _onActivityClosed();
    }

    /**
     * 补偿活动开启前已触发的跨天事件
     * 由于活动开启（0点后几秒）晚于跨天事件触发（0点整），在线玩家的跨天事件在活动注册监听前已被触发，
     * 导致配置了P_CROSS_DAY事件的阶段奖励漏算当天登录。此方法在活动首次进入PLAYING状态时异步补偿。
     */
    private void _compensateMissedCrossDayEvent()
    {
        //筛选配置了P_CROSS_DAY事件的阶段奖励
        ArrayList<ActivityStepRewardInfo> crossDayStepRewards = null;

        _lock();
        try{
            if (_m_alStepRewardList.isEmpty())
                return;

            for (ActivityStepRewardInfo stepRewardInfo : _m_alStepRewardList)
            {
                RefStepRewardSet ref = stepRewardInfo.getStepRewardRef();
                if (ref == null || ref.logic_event == null)
                    continue;
                if (ref.getLogicEventId() == Event_P_CROSS_DAY.ID)
                {
                    if (crossDayStepRewards == null)
                        crossDayStepRewards = new ArrayList<>();

                    crossDayStepRewards.add(stepRewardInfo);
                }
            }
        }finally
        {
            _unlock();
        }

        if (crossDayStepRewards == null || crossDayStepRewards.isEmpty())
            return;

        //异步执行补偿，避免阻塞活动开启流程
        ArrayList<ActivityStepRewardInfo> finalCrossDayStepRewards = crossDayStepRewards;

        ALSynTaskManager.getInstance().regTask(() ->
        {
            int todayTag = CommonFunc.getTimeTagYYYYMMDD(getStartTimeMs() / 1000);
            ArrayList<NPUSUserData> allUserData = getUSServer().getUsUserMgr().getAllCacheUserData();

            for (NPUSUserData userData : allUserData)
            {
                //只补偿已加载完成的在线玩家
                if (userData.getUserDataState() != ENPUserDataState.LOADED)
                    continue;

                //检查玩家今天是否已触发过跨天事件（LAST_LOGIN_DATE == 今天）
                int lastLoginDate = (int) userData.getParam(ENPPlayerParam.LAST_LOGIN_DATE);
                if (lastLoginDate != todayTag)
                    continue;

                long cid = userData.getCid();

                for (ActivityStepRewardInfo stepRewardInfo : finalCrossDayStepRewards)
                {
                    //强制设值为1，避免与已触发的跨天事件重复累加
                    stepRewardInfo.onScoreChange(cid, 0, 1, true);
                }
            }
        });
    }

    /**
     * 检查注册活动事件
     */
    private void _regActivityEvent()
    {
        //注册该事件监听
        int triggerEventId = _m_refEvent.getTriggerEventId();
        if (triggerEventId <= 0)
            return;

        _m_evtEntry = getUSServer().getGlobalEventHandlerMgr().regHandler(triggerEventId, this, new HandlerTwo<_ALogicEventBase, _INPGlobalUserEventObj>()
        {
            @Override
            public void handle(_ALogicEventBase _evt, _INPGlobalUserEventObj _eventObj)
            {
                NPUSUserData userData = _eventObj.getUserData();
                if (userData == null)
                    return;

                NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
                
                //检查触发条件
                if (!NPPlayerConditionDealerMgr.IsEnable(_m_refEvent.trigger_condition, userData, varInfo))
                    return;

                NPPlayerEffectDealer.dealEffect(_m_refEvent.effects.getPlayerEffectList(), userData, varInfo, (NPPlayerContext) _evt.getContext());
            }
        });
    }

    /**
     * 销毁活动数据
     */
    public final void discard()
    {
        _lock();
        try
        {
            //通知排期对应活动销毁
            _m_scheduleInfo.onActivityDiscard();

            //销毁排行榜
            getUSServer().getBM().getBM(ActivityRankBO.class).delAll("instance_id", getInstanceId());
            getUSServer().getBM().getBM(ActivityRankRewardInfoBO.class).delAll("activity_instance_id", getInstanceId());

            //销毁阶段奖励
            getUSServer().getBM().getBM(ActivityStepRewardBO.class).delAll("instance_id", getInstanceId());
            getUSServer().getBM().getBM(ActivityStepRewardDrawRecordBO.class).delAll("activity_instance_id", getInstanceId());
            getUSServer().getBM().getBM(ActivityStepRewardMailRecordBO.class).delAll("activity_instance_id", getInstanceId());

            //销毁玩家相关数据
            getUSServer().getBM().getBM(ActivityPlayerShopInfoBO.class).delAll("instance_id", getInstanceId());
            getUSServer().getBM().getBM(ActivityPlayerShopBuyRecordBO.class).delAll("instance_id", getInstanceId());
            getUSServer().getBM().getBM(ActivityPlayerCrystalGiftPackInfoBO.class).delAll("instance_id", getInstanceId());
            getUSServer().getBM().getBM(ActivityPlayerCrystalGiftPackBuyRecordBO.class).delAll("instance_id", getInstanceId());

            //移除数据
            getBo().del(getUSServer().getBM());

            //队伍销毁处理
            if(null != _m_teamDealer)
            {
                _m_teamDealer._onDiscard();
            }

            //游戏逻辑销毁处理
            if(null != _m_gameLogicDealer)
            {
                _m_gameLogicDealer._onDiscard();
            }

            //执行子类实例销毁操作
            _discard();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找对应排行榜信息
     * @param _rankId
     * @return
     */
    public ActivityRankInfo lookupRank(long _rankId)
    {
        _lock();
        try
        {
            for (ActivityRankInfo rank : _m_alRankList)
            {
                if (null == rank)
                    continue;

                if (rank.getRankId() == _rankId)
                    return rank;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找对应阶段奖励信息
     * @param _stepRewardId
     * @return
     */
    public ActivityStepRewardInfo lookupStepReward(long _stepRewardId)
    {
        _lock();
        try
        {
            for (ActivityStepRewardInfo stepRewardInfo : _m_alStepRewardList)
            {
                if (null == stepRewardInfo)
                    continue;

                if (stepRewardInfo.getStepRewardId() == _stepRewardId)
                    return stepRewardInfo;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /***************
     * 开启排行榜，已经开启不再重新开启
     */
    public void createAllRank()
    {
        _lock();
        try
        {
            for (Long rankId : _m_ref.rank_id_list)
            {
                if (null == lookupRank(rankId))
                {
                    ActivityRankBO bo = new ActivityRankBO();
                    bo.setInstanceId(getUSServer().getBM(), getInstanceId());
                    bo.setRankId(getUSServer().getBM(), rankId);
                    bo.insert(getUSServer().getBM());

                    ActivityRankInfo rank = new ActivityRankInfo(this, bo);
                    _m_alRankList.add(rank);
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /***************
     * 开启阶段奖励，已经开启不再重新开启
     */
    public void createAllStepReward()
    {
        _lock();
        try
        {
            for (Long stepRewardId : _m_ref.step_reward_set_id_list)
            {
                if (null == lookupStepReward(stepRewardId))
                {
                    ActivityStepRewardBO bo = new ActivityStepRewardBO();
                    bo.setInstanceId(getUSServer().getBM(), getInstanceId());
                    bo.setStepRewardId(getUSServer().getBM(), stepRewardId);
                    bo.insert(getUSServer().getBM());

                    ActivityStepRewardInfo rank = new ActivityStepRewardInfo(this, bo);
                    _m_alStepRewardList.add(rank);
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注册排行榜事件
     * @param _process 处理对象
     */
    public void initRankEvent(ALProcess _process)
    {
        _lock();
        try
        {
            for (ActivityRankInfo rankInfo : _m_alRankList)
            {
                //注册排行榜事件
                _process.addResDelegateProcess(_action -> rankInfo.regRankingEvent(_action::dealAction),
                        "rank_reg_event:" + rankInfo.getRankId(), null, false);
            }
        } finally
        {
            _unlock();
        }
    }


    /**
     * 初始化排行榜实例
     * @param _process 处理对象
     */
    public void initRankInstance(ALProcess _process)
    {
        _lock();
        try
        {
            for (ActivityRankInfo rankInfo : _m_alRankList)
            {
                //创建排行榜实例
                _process.addResDelegateProcess(_action -> rankInfo.createRankInstance(_action::dealAction),
                        "rank_create_instance:" + rankInfo.getRankId(), null, false);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注册阶段奖励事件
     * @param _process 处理对象
     */
    public void initStepRewardEvent(ALProcess _process)
    {
        _lock();
        try
        {
            for (ActivityStepRewardInfo stepRewardInfo : _m_alStepRewardList)
            {
                //注册阶段奖励事件
                _process.addResDelegateProcess(_action -> stepRewardInfo.regRankingEvent(_action::dealAction),
                        "step_reward_reg_event:" + stepRewardInfo.getStepRewardId(), null, false);
                
                //注册该阶段的事件任务
                ArrayList<ActivityStepRewardEventTaskInfo> eventTaskList = stepRewardInfo.getEventTaskMgr().getEventTaskList();
                for(int i = 0; i < eventTaskList.size(); i++)
                {
                	ActivityStepRewardEventTaskInfo eventTask = eventTaskList.get(i);
                	if(null == eventTask)
                		continue;
                	
                	_process.addResDelegateProcess(_action -> eventTask.regRankingEvent(_action::dealAction),
                            "step_reward_event_task_reg_event:" + stepRewardInfo.getStepRewardId() + ", task:" + eventTask.getEventTaskId(), null, false);
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注册阶段奖励事件
     * @param _process 处理对象
     */
    public void initActivityEvent(ALProcess _process)
    {
        //注册活动配置
        if (null != _m_refEvent)
        {
            _process.addResDelegateProcess(_action ->
            {
                _regActivityEvent();
                _action.dealAction(true);
            }, "init_activity_event", null, false);
        }

        _process.addResDelegateProcess(_action ->
        {
            //注册其他事件
            _regExtraEvent();
            _action.dealAction(true);
        }, "init_activity_extra_event", null, false);
    }

    /**
     * 注册关联礼包
     * @param _process 处理对象
     */
    public void initActivityGiftPack(ALProcess _process)
    {
        _process.addResDelegateProcess(_action ->
        {
            List<Long> relativeGiftPackIdList = _m_ref.getRelativeGiftPackIdList();
            if (relativeGiftPackIdList != null)
                getUSServer().getGiftPackLifeCycleMgr().onActivityInstanceOpened(getInstanceId(), relativeGiftPackIdList, getEndTimeMs());
            _action.dealAction(true);
        }, "init_activity_gift_pack", null, false);
    }

    /**
     * 初始化活动热更配表
     *
     * 执行流程：
     * 1. 检查活动是否有排期信息
     * 2. 调用排期管理器激活热更配表
     * 3. 将激活操作添加到异步处理流程中
     *
     * @param _process 处理对象
     */
    public void initActiveHotRef(ALProcess _process)
    {
        _process.addResDelegateProcess(_action ->
                _initActivityHotRef(_action::dealAction), "init_activity_hot_ref", null, false);
    }

    /**
     * 初始化活动热更配表
     * @param _action
     */
    public void _initActivityHotRef(_ICallBackBool _action)
    {
        // 检查是否有排期信息
        if (_m_scheduleInfo == null)
        {
            USLog.error(getUSServer(), "init activity hot ref failed, schedule info is null, activityId={}, instanceId={}",
                    getActivityId(), getInstanceId());
            _action.onRunOver(false);
            return;
        }

        // 激活热更配表
        boolean result = getUSServer().getUsActivityScheduleMgr().activateScheduleHotRef(getInstanceId());
        if (result)
        {
            USLog.info(getUSServer(), "init activity hot ref success, activityId={}, instanceId={}",
                    getActivityId(), getInstanceId());

            _action.onRunOver(true);
        }
        else
        {
            USLog.warn(getUSServer(), "init activity hot ref failed retry in 3 secs, activityId={}, instanceId={}",
                    getActivityId(), getInstanceId());

            //如果失败则创建一个3秒后的任务重试
            ALSynTaskManager.getInstance().regTask(() -> _initActivityHotRef(_action), 3000);
        }
    }

    /**
     * 初始化阶段奖励实例
     * @param _process 处理对象
     */
    public void initStepRewardInstance(ALProcess _process)
    {
        _lock();
        try
        {
            for (ActivityStepRewardInfo stepRewardInfo : _m_alStepRewardList)
            {
                //创建阶段奖励实例
                _process.addResDelegateProcess(_action -> stepRewardInfo.createStepRewardInstance(_action::dealAction),
                        "step_reward_create_instance:" + stepRewardInfo.getStepRewardId(), null, false);
            }
        } finally
        {
            _unlock();
        }
    }


    /**
     * 注销排行榜事件
     */
    public void unRegAllRankEvent(ALProcess _process)
    {
        _lock();
        try
        {
            for (ActivityRankInfo rankInfo : _m_alRankList)
            {
                _process.addResDelegateProcess(action -> rankInfo.unRegRankingEvent(action::dealAction),
                        "rank_unreg_event:" + rankInfo.getRankId(), null, false);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注销排行榜事件
     */
    public void unRegAllStepRewardEvent(ALProcess _process)
    {
        _lock();
        try
        {
            for (ActivityStepRewardInfo stepRewardInfo : _m_alStepRewardList)
            {
                _process.addResDelegateProcess(action -> stepRewardInfo.unRegRankingEvent(action::dealAction),
                        "step_reward_unreg_event:" + stepRewardInfo.getStepRewardId(), null, false);
                
                //注销该阶段的事件任务
                ArrayList<ActivityStepRewardEventTaskInfo> eventTaskList = stepRewardInfo.getEventTaskMgr().getEventTaskList();
                for(int i = 0; i < eventTaskList.size(); i++)
                {
                	ActivityStepRewardEventTaskInfo eventTask = eventTaskList.get(i);
                	if(null == eventTask)
                		continue;
                	
                	_process.addResDelegateProcess(_action -> eventTask.unRegRankingEvent(_action::dealAction),
                            "step_reward_event_task_unreg_event:" + stepRewardInfo.getStepRewardId() + ", task:" + eventTask.getEventTaskId(), null, false);
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 注销活动事件
     */
    public void unRegActivityEvent(ALProcess _process)
    {
        //注册活动配置
        if (null != _m_evtEntry)
        {
            _process.addResDelegateProcess(_action ->
            {
                //注销活动注册事件
                getUSServer().getGlobalEventHandlerMgr().unregHandler(_m_evtEntry);
                _m_evtEntry = null;
                _action.dealAction(true);
            }, "unreg_activity_event", null, false);
        }
    }

    /**
     * 注销活动关联礼包
     */
    public void unRegActivityGiftPack(ALProcess _process)
    {
        _process.addResDelegateProcess(_action ->
        {
            List<Long> relativeGiftPackIdList = _m_ref.getRelativeGiftPackIdList();
            if (relativeGiftPackIdList != null)
                getUSServer().getGiftPackLifeCycleMgr().onActivityInstanceSetting(getInstanceId(), relativeGiftPackIdList);
            _action.dealAction(true);
        }, "unreg_activity_gift_pack", null, false);
    }
    
    /**
     * 记录当前本服所有公会必要数据，用于后续结算使用
     * @param _process
     */
    public void dumpAllGuild(ALProcess _process)
    {
        _process.addResDelegateProcess(_action ->
        {
        	ArrayList<GuildInfo> guildList = getUSServer().getGuildMgr().getGuildList();
        	_m_mgrSettleGuildMgr.dumpAllLeaderCid(guildList);
        	
            _action.dealAction(true);
        }, "dump_all_guild", null, false);
    }

    /**
     * 记录当前本服所有队伍必要数据，用于后续结算使用
     * @param _process
     */
    public void dumpAllTeam(ALProcess _process)
    {
        _process.addResDelegateProcess(_action ->
        {
            ArrayList<TeamActivityInfo> teamList = getUSServer().getTeamActivityMgr().getTeamListByActivityInstanceId(getInstanceId());
            _m_mgrSettleTeamMgr.dumpAllLeaderCid(teamList);

            _action.dealAction(true);
        }, "dump_all_team", null, false);
    }

    /*****************
     * 关闭排行榜
     */
    public void closeAllRank(ALProcess _process)
    {
        _lock();
        try
        {
            for (ActivityRankInfo rankInfo : _m_alRankList)
            {
                //1.补发排行榜奖励
                _process.addResDelegateProcess(action -> rankInfo.sendNotDrawRankReward(action::dealAction),
                        "rank_send_not_draw_reward:" + rankInfo.getRankId(), null, false);
                //2.销毁排行榜实例
                _process.addResDelegateProcess(action -> rankInfo.discardRankInstance(action::dealAction),
                        "rank_discard_instance:" + rankInfo.getRankId(), null, false);
            }
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 关闭阶段奖励
     */
    public void closeAllStepReward(ALProcess _process)
    {
        _lock();
        try
        {
            for (ActivityStepRewardInfo stepRewardInfo : _m_alStepRewardList)
            {
                //1.补发阶段奖励奖励
                _process.addResDelegateProcess(action -> stepRewardInfo.sendNotDrawStepReward(action::dealAction),
                        "step_reward_send_not_draw_reward:" + stepRewardInfo.getStepRewardId(), null, false);
                //2.销毁阶段奖励实例
                _process.addResDelegateProcess(action -> stepRewardInfo.discardStepRewardInstance(action::dealAction),
                        "step_reward_discard_instance:" + stepRewardInfo.getStepRewardId(), null, false);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置可以发放奖励
     */
    /**
     * 重新调整流程
     		1. 进入领奖期，不再拉取排行榜存入US
     		2. 记录本服US所有联盟的盟主CID，用于联盟冲榜盟主玩家认定
     		3. 玩家手动领取奖励，需要在本服US检查领取奖励记录
     		4. 活动结束后，再去排行榜拉取排行榜数据下发到US，进行检查后发送邮件
     		5. 第4点完成后，才可以删除第2，3点的数据
     */
    public void setCanSendReward()
    {
        _lock();
        try
        {	
            for (ActivityRankInfo rankInfo : _m_alRankList)
            {
                rankInfo.setCanSendReward();
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否所有排行榜都可以发放奖励
     * @return
     */
    public boolean isAllRankCanDrawReward()
    {
        _lock();
        try
        {
            for (ActivityRankInfo rankInfo : _m_alRankList)
            {
                if (!rankInfo.canDrawReward())
                    return false;
            }
            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 活动状态变更推送
     * @param _state 活动状态
     */
    public void pushActivityStateChg(EActivityState _state)
    {
        ALSynTaskManager.getInstance().regTask(() ->
                getUSServer().getUsUserMgr().broadCastMessage(US2GCWriter_017_ActivityOp.make_056_OnActivityStateChg(getInstanceId(), _state)));
    }

    /**
     * 活动状态转换异常
     * @param _processTag 进程标签
     * @param _exInfo     异常信息
     * @param _ex         异常对象
     */
    public void onActivityStateTransErr(String _processTag, String _exInfo, Exception _ex)
    {
        getUSServer().getDDAlert().err("_AActivityBase onActivityStateTransErr",
                "_AActivityBase state trans err, Process:{} run onException extInfo:{} errType:{}", _processTag, _exInfo, _ex.getMessage(), _ex);
    }

    /**
     * 活动状态转换失败
     * @param _processTag 进程标签
     */
    public void onActivityStateTransFail(String _processTag)
    {
        getUSServer().getDDAlert().err("_AActivityBase onActivityStateTransFail",
                "_AActivityBase state trans fail stop, Process:{}", _processTag);
    }

    /**
     * 填充玩家数据
     * @param _dataList
     */
    public void fillPlayerData(long _cid, List<Activity_PlayerData> _dataList)
    {
        Activity_ShopInfo shopInfo = getShopMgr().makeProtoShopInfo(_cid);
        CrystalGiftPack_Info crystalGiftPackInfo = getCrystalGiftPackMgr().makeProtoGiftPackInfo(_cid);
        //如果商店和礼包都没有数据，则不需要填充
        if (null != shopInfo || crystalGiftPackInfo != null)
        {
            Activity_PlayerData playerData = new Activity_PlayerData();
            playerData.setActivityInstanceId(getInstanceId());
            if (null != shopInfo)
                playerData.setShopInfo(shopInfo);
            if (null != crystalGiftPackInfo)
                playerData.setCrystalGiftPackInfo(crystalGiftPackInfo);
            _dataList.add(playerData);
        }
    }

    @Override
    public String toString()
    {
        _lock();

        try
        {
            StringBuilder sb = new StringBuilder();
            //基础数据
            sb.append("instanceId:").append(getInstanceId())
                    .append("\nusGroupId:").append(getSchedule().getUsGroupId())
                    .append("\nactivityId:").append(getActivityId())
                    .append("\ncrossInstance:").append(getCrossInstanceId())
                    .append("\nstate").append(getCurState().getStateType())
                    .append("\nstartTS:").append(CommonFunc.getTimeStringMs(getBo().getStartMs()))
                    .append("\nendTS:").append(CommonFunc.getTimeStringMs(getBo().getEndMs()))
                    .append("\ncloseTS:").append(CommonFunc.getTimeStringMs(getBo().getCloseMs()));
            //排行榜数据
            sb.append("\nrankSize:").append(_m_alRankList.size());
            for (ActivityRankInfo rankInfo : _m_alRankList)
            {
                if (null == rankInfo)
                    continue;

                sb.append("\n").append("[").append(rankInfo.getRankInstanceId()).append("]:").append(rankInfo.getRankId());
            }

            return sb.toString();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取活动物品过期时间
     * @param _expireType
     * @return
     */
    public long getActivityItemExpireTimeMs(EActivityItemExpireTimeType _expireType)
    {
        switch (_expireType)
        {
            case REWARDING:
                return _m_bo.getEndMs();
            case CLOSED:
                return _m_bo.getCloseMs();
            default:
                return 0;
        }
    }

    /**
     * 填充阶段奖励信息
     * @param _list
     */
    public void fillStepRewardInfo(List<ActivityStepRewardBaseInfo> _list)
    {
        _lock();
        try
        {
            for (ActivityStepRewardInfo stepRewardInfo : _m_alStepRewardList)
            {
                ActivityStepRewardBaseInfo stepReward = new ActivityStepRewardBaseInfo();
                stepReward.activityInstanceId = getInstanceId();
                stepReward.stepReward = stepRewardInfo;
                _list.add(stepReward);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否存在指定排行榜实例
     * @param _rankInstanceId
     * @return
     */
    public boolean hasRankInstanceId(long _rankInstanceId)
    {
        _lock();
        try
        {
            for (ActivityRankInfo rankInfo : _m_alRankList)
            {
                if (rankInfo.getRankInstanceId() == _rankInstanceId)
                    return true;
            }
            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 根据不同的活动构造各自的玩家数据对象，默认是null，活动具体类覆盖该方法重新构造。
     * @return
     */
    public _IALProtocolStructure makePlayerObj(NPUSUserData _userData)
    {
        return null;
    }

    public abstract int getActivityTypeId();

    /****************
     * 活动实例新创建，动态初始化，如果有其它数据，子类自行创建
     * @return
     */
    protected abstract boolean _subInitNew();

    /****************
     * 活动实例从数据库加载，静态初始化，如果有其它数据，子类自行加载
     * @return
     */
    protected abstract boolean _subInitStatic();

    protected abstract void _regExtraEvent();

    protected abstract void _onActivityStart();

    protected abstract void _onActivityEnd();

    protected abstract void _onActivityClosed();

    protected abstract void _discard();
}
