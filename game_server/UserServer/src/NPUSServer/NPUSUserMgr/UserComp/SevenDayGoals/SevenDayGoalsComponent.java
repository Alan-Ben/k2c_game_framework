package NPUSServer.NPUSUserMgr.UserComp.SevenDayGoals;

import ALBasicServer.ALProcess.ALProcess;
import Common.ActivityEnum.EActivityState;
import Common.MailObj.Mail_Data;
import CommonEnum.ECommonActivityType;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_004_RetSevenDayGoalsInfo;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_104_OnSevenDayGoalScoreChg;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_107_OnSevenDayGoalRewardDraw;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_108_OnSevenDayGoalStepRewardDraw;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.SevenDayGoalsErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.SevenDayGoals.RefSevenDayGoalsStepReward;
import NPGameRes.Refs.SevenDayGoals.RefSevenDayGoalsTask;
import NPGameRes.Refs.SevenDayGoals.RefSevenDayGoalsTaskReward;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.USLog;
import USDB.Bo.PlayerSevenDayGoalsBO;
import USDB.Bo.PlayerSevenDayGoalsStepRewardDrawRecordBO;
import USDB.Bo.PlayerSevenDayGoalsTaskCounterBO;
import USDB.Bo.PlayerSevenDayGoalsTaskRewardDrawRecordBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class SevenDayGoalsComponent extends _ANPUserComponent implements _IHandlerHolder
{
    private PlayerSevenDayGoalsBO _m_bo;
    private List<Long> _m_hadDrawStepRewardList;
    private List<Long> _m_hadDrawTaskRewardList;
    private Map<Long, SevenDayGoalsTaskCounter> _m_taskCounterMap;

    private boolean _m_needInitOtherData;//是否需要初始化其他数据

    public SevenDayGoalsComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.SEVEN_DAY_GOALS);

        _m_hadDrawStepRewardList = new ArrayList<>();
        _m_hadDrawTaskRewardList = new ArrayList<>();
        _m_taskCounterMap = new HashMap<>();
    }

    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("seven_day_goals_comp_init");
        //初始化加载，加载过程如果出现异常，则直接加载失败
        //步骤1: 主数据加载
        process.addResDelegateProcess(action -> _initMainBo(action::dealAction), "init_main_bo",
                null, false);
        //步骤2: 任务额外计数数据加载
        process.addResDelegateProcess(action -> _initTaskExtraCounterBo(action::dealAction), "init_task_extra_counter_bo",
                null, false);
        //步骤3: 任务奖励领取数据加载
        process.addResDelegateProcess(action -> _initTaskRewardDrawRecordBo(action::dealAction), "init_task_reward_draw_record_bo",
                null, false);
        //步骤4: 任务阶段奖励领取数据加载
        process.addResDelegateProcess(action -> _initStepRewardDrawRecordBo(action::dealAction), "init_step_reward_draw_record_bo",
                null, false);

        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "SevenDayGoalsComponent _init fail stop");
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 初始化
     * @param _handler
     */
    private void _initMainBo(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerSevenDayGoalsBO.class).findOne("cid", getUserData().getCid(),
                new _ASelectCallback<PlayerSevenDayGoalsBO>()
                {
                    @Override
                    public void dealSuc(PlayerSevenDayGoalsBO _bo)
                    {
                        _m_bo = _bo;

                        if (_m_bo.getActivityInstanceId() != 0)
                            _m_needInitOtherData = true;

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        if (getHasErr())
                        {
                            _handler.onRunOver(false);
                            return;
                        }

                        PlayerSevenDayGoalsBO bo = new PlayerSevenDayGoalsBO();
                        bo.setCid(getUSServer().getBM(), getUserData().getCid());
                        bo.insert(getUSServer().getBM());

                        _m_bo = bo;

                        _handler.onRunOver(true);
                    }
                });
    }

    /**
     * 初始化任务奖励领取数据
     * @param _handler
     */
    private void _initTaskExtraCounterBo(_ICallBackBool _handler)
    {
        if (!_m_needInitOtherData)
        {
            _handler.onRunOver(true);
            return;
        }

        getUSServer().getBM().getBM(PlayerSevenDayGoalsTaskCounterBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerSevenDayGoalsTaskCounterBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerSevenDayGoalsTaskCounterBO> _boList)
                    {
                        for (PlayerSevenDayGoalsTaskCounterBO bo : _boList)
                        {
                            RefSevenDayGoalsTask ref = RefSevenDayGoalsTask.getMgr().get(bo.getTaskId());
                            if (ref == null)
                            {
                                USLog.error(getUSServer(), "SevenDayGoalsComponent _initTaskExtraCounterBo task ref is null cid:{} taskId:{}", bo.getCid(), bo.getTaskId());
                                continue;
                            }

                            SevenDayGoalsTaskCounter taskCounter = new SevenDayGoalsTaskCounter(SevenDayGoalsComponent.this, ref, bo);
                            _m_taskCounterMap.put(ref.Id(), taskCounter);
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    /**
     * 初始化任务阶段奖励领取数据
     * @param _handler
     */
    private void _initTaskRewardDrawRecordBo(_ICallBackBool _handler)
    {
        if (!_m_needInitOtherData)
        {
            _handler.onRunOver(true);
            return;
        }

        getUSServer().getBM().getBM(PlayerSevenDayGoalsTaskRewardDrawRecordBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerSevenDayGoalsTaskRewardDrawRecordBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerSevenDayGoalsTaskRewardDrawRecordBO> _boList)
                    {
                        for (PlayerSevenDayGoalsTaskRewardDrawRecordBO bo : _boList)
                        {
                            _m_hadDrawTaskRewardList.add(bo.getRefId());
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    /**
     * 初始化任务阶段奖励领取数据
     * @param _handler
     */
    private void _initStepRewardDrawRecordBo(_ICallBackBool _handler)
    {
        if (!_m_needInitOtherData)
        {
            _handler.onRunOver(true);
            return;
        }

        getUSServer().getBM().getBM(PlayerSevenDayGoalsStepRewardDrawRecordBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerSevenDayGoalsStepRewardDrawRecordBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerSevenDayGoalsStepRewardDrawRecordBO> _boList)
                    {
                        for (PlayerSevenDayGoalsStepRewardDrawRecordBO bo : _boList)
                        {
                            _m_hadDrawStepRewardList.add(bo.getRefId());
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        getUSServer().getCommActivityMgr().activityStateChg.addHandler(this, new HandlerTwo<_AActivityBase, EActivityState>()
        {
            @Override
            public void handle(_AActivityBase _activity, EActivityState _state)
            {
                getUserData().lockUser();
                try
                {
                    //判断是否是七日目标活动
                    if (_activity.getActivityTypeId() != ECommonActivityType.SEVEN_DAY_GOALS.ordinal())
                        return;

                    if (_m_bo.getActivityInstanceId() == _activity.getInstanceId())
                    {
                        if (_AActivityBase.CLOSE_STATE.contains(_state))
                        {
                            _m_bo.saveActivityInstanceId(getUSServer().getBM(), 0);

                            //活动关闭时的处理
                            _onActivityClose();

                            //检查是否有正在运行的七日目标活动
                            _checkHasActivityRunning();
                        }
                    } else if (_m_bo.getActivityInstanceId() == 0)
                    {
                        if (_AActivityBase.RUNNING_STATE.contains(_state))
                        {
                            _m_bo.saveActivityInstanceId(getUSServer().getBM(), _activity.getInstanceId());

                            //注册所有事件监听
                            _regAllEvtEntry();
                        }
                    }
                } finally
                {
                    getUserData().unlockUser();
                }
            }
        });

        getUserData().lockUser();
        try
        {
            //初始化所有需要的任务计数器
            _initAllTaskCounter();

            //如果活动实例ID不为0，且活动在运行中，则注册事件
            if (_m_bo.getActivityInstanceId() != 0)
            {
                _AActivityBase activity = getUSServer().getCommActivityMgr().lookupActivity(_m_bo.getActivityInstanceId());
                if (activity != null && activity.isRunning())
                {
                    _regAllEvtEntry();
                }else
                {
                    _m_bo.saveActivityInstanceId(getUSServer().getBM(), 0);

                    //活动关闭时的处理
                    _onActivityClose();

                    //检查是否有正在运行的七日目标活动
                    _checkHasActivityRunning();
                }
            }

            //检查是否有正在运行的七日目标活动
            _checkHasActivityRunning();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    public void dispose()
    {
        getUserData().lockUser();
        try
        {
            getUSServer().getCommActivityMgr().activityStateChg.clear(this);

            _unRegAllEvtEntry();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否有正在运行的七日目标活动
     */
    private void _checkHasActivityRunning()
    {
        List<_AActivityBase> activityList = getUSServer().getCommActivityMgr().lookupActivityByType(ECommonActivityType.SEVEN_DAY_GOALS.ordinal());
        if (!activityList.isEmpty())
        {
            for (_AActivityBase activity : activityList)
            {
                if (!activity.isRunning())
                    continue;

                _m_bo.saveActivityInstanceId(getUSServer().getBM(), activity.getInstanceId());

                //注册所有事件监听
                _regAllEvtEntry();

                break;
            }
        }
    }

    private void _regAllEvtEntry()
    {
        for (SevenDayGoalsTaskCounter taskCounter : _m_taskCounterMap.values())
        {
            taskCounter.regEvtEntry();
        }
    }

    /**
     * 关联活动关闭处理
     */
    private void _onActivityClose()
    {
        _unRegAllEvtEntry();

        //补发奖励
        _dispatchReward();

        //清空数据
        _m_hadDrawTaskRewardList.clear();
        _m_hadDrawStepRewardList.clear();
        _m_taskCounterMap.clear();

        _m_bo.saveScore(getUSServer().getBM(), 0);

        getUSServer().getBM().getBM(PlayerSevenDayGoalsStepRewardDrawRecordBO.class).delAll("cid", getUserData().getCid());
        getUSServer().getBM().getBM(PlayerSevenDayGoalsTaskRewardDrawRecordBO.class).delAll("cid", getUserData().getCid());
        getUSServer().getBM().getBM(PlayerSevenDayGoalsTaskCounterBO.class).delAll("cid", getUserData().getCid());

        //重新初始化所有任务计数器
        _initAllTaskCounter();
    }

    private void _unRegAllEvtEntry()
    {
        //反注册所有的事件监听
        for (SevenDayGoalsTaskCounter taskCounter : _m_taskCounterMap.values())
        {
            taskCounter.unRegEvtEntry();
        }
    }

    /**
     * 补发奖励
     */
    private void _dispatchReward()
    {
        int serverStartDay = getUSServer().getServerStartDay();

        NPItemCostCollector_nosafe itemCollector = new NPItemCostCollector_nosafe();
        //统计任务奖励
        List<RefSevenDayGoalsTask> refTaskList = RefSevenDayGoalsTask.getMgr().getList();
        for (RefSevenDayGoalsTask ref : refTaskList)
        {
            long count = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), ref.process_cur_count, null)
                    + getTaskExtraCount(ref.task_id);

            for (RefSevenDayGoalsTaskReward refTaskReward : ref.task_reward_list)
            {
                if (_m_hadDrawTaskRewardList.contains(refTaskReward.Id()))
                    continue;

                if (refTaskReward.goal_count > count)
                    continue;

                //如果没达到开服要求时间，则不发放奖励
                if (refTaskReward.day > serverStartDay)
                    continue;

                itemCollector.addItemList(refTaskReward.reward_item_list);
            }
        }
        //统计阶段奖励
        List<RefSevenDayGoalsStepReward> refStepRewardList = RefSevenDayGoalsStepReward.getMgr().getList();
        for (RefSevenDayGoalsStepReward ref : refStepRewardList)
        {
            if (_m_hadDrawStepRewardList.contains(ref.Id()))
                continue;

            if (_m_bo.getScore() < ref.need_score)
                continue;

            itemCollector.addItemList(ref.gain_item_list);
        }

        //发送邮件
        if (!itemCollector.isEmpty())
        {
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().seven_day_goals_mail_id);
            mailData.getItemList().getItemList().addAll(itemCollector.getItemListP());
            MailSystem.addMail(getUSServer(), getUserData().getCid(), mailData, NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_CLOSE));
        }
    }

    /**
     * 初始化所有任务计数器
     */
    protected void _initAllTaskCounter()
    {
        List<RefSevenDayGoalsTask> taskRefList = RefSevenDayGoalsTask.getMgr().getList();
        for (RefSevenDayGoalsTask ref : taskRefList)
        {
            SevenDayGoalsTaskCounter taskCounter = lookupTaskCounter(ref.Id());
            if (taskCounter != null)
                continue;

            if (ref.trigger_event_id == 0)
                continue;

            _m_taskCounterMap.put(ref.Id(), new SevenDayGoalsTaskCounter(this, ref));
        }
    }

    /**
     * 查找任务计数对象
     * @param _taskId
     * @return
     */
    public SevenDayGoalsTaskCounter lookupTaskCounter(long _taskId)
    {
        getUserData().lockUser();
        try
        {
            return _m_taskCounterMap.get(_taskId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找任务计数对象
     * @param _ref
     * @return
     */
    public SevenDayGoalsTaskCounter ensureTaskCounter(RefSevenDayGoalsTask _ref)
    {
        getUserData().lockUser();
        try
        {
            SevenDayGoalsTaskCounter taskCounter = lookupTaskCounter(_ref.Id());
            if (taskCounter != null)
                return taskCounter;

            taskCounter = new SevenDayGoalsTaskCounter(this, _ref);
            _m_taskCounterMap.put(_ref.Id(), taskCounter);
            return taskCounter;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取任务额外计数
     * @param _taskId
     * @return
     */
    public long getTaskExtraCount(long _taskId)
    {
        getUserData().lockUser();
        try
        {
            SevenDayGoalsTaskCounter taskCounter = lookupTaskCounter(_taskId);
            if (taskCounter == null)
                return 0;

            return taskCounter.getExtraCount();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取任务奖励
     */
    public Result drawTaskReward(long _refId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //查询任务奖励
            RefSevenDayGoalsTaskReward ref = RefSevenDayGoalsTaskReward.getMgr().get(_refId);
            if (ref == null)
                return CommErr.REF_NOT_FOUND;

            //检查是否已经领取
            if (_m_hadDrawTaskRewardList.contains(_refId))
                return SevenDayGoalsErr.TASK_REWARD_HAD_DRAW;

            //检查是否达到开服天数要求
            if (getUSServer().getServerStartDay() < ref.day)
                return SevenDayGoalsErr.SERVER_START_DAY_NOT_REACH;

            //查询任务配置
            RefSevenDayGoalsTask refTask = RefSevenDayGoalsTask.getMgr().get(ref.task_id);
            if (refTask == null)
                return CommErr.REF_NOT_FOUND;

            //检查任务是否完成
            long count = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), refTask.process_cur_count, null)
                    + getTaskExtraCount(ref.task_id);
            if (count < ref.goal_count)
                return SevenDayGoalsErr.TASK_NOT_REACH;

            //标记已领取
            markDrawTaskReward(_refId, _context);

            //领取奖励
            getUserData().gainItemList(ref.reward_item_list, _context);

            //领取奖励分数
            gainScore(ref.gain_score, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取奖励分数
     * @param _gainScore
     * @param _context
     */
    public void gainScore(int _gainScore, NPPlayerContext _context)
    {
        try
        {
            _m_bo.saveScore(getUSServer().getBM(), _m_bo.getScore() + _gainScore);
            getUserData().lockUser();
            getUserData().sendMsgToGC(new GS2GC_033_104_OnSevenDayGoalScoreChg(_m_bo.getScore()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 标记已领取任务奖励
     * @param _refId
     * @param _context
     */
    public void markDrawTaskReward(long _refId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            PlayerSevenDayGoalsTaskRewardDrawRecordBO bo = new PlayerSevenDayGoalsTaskRewardDrawRecordBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setRefId(getUSServer().getBM(), _refId);
            bo.insert(getUSServer().getBM());

            _m_hadDrawTaskRewardList.add(_refId);

            getUserData().sendMsgToGC(new GS2GC_033_107_OnSevenDayGoalRewardDraw(_refId));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取阶段奖励
     * @param _refId
     * @param _context
     * @return
     */
    public Result drawStepReward(long _refId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //查询任务奖励
            RefSevenDayGoalsStepReward ref = RefSevenDayGoalsStepReward.getMgr().get(_refId);
            if (ref == null)
                return CommErr.REF_NOT_FOUND;

            //检查是否已经领取
            if (_m_hadDrawStepRewardList.contains(_refId))
                return SevenDayGoalsErr.STEP_REWARD_HAD_DRAW;

            if (_m_bo.getScore() < ref.need_score)
                return SevenDayGoalsErr.STEP_REWARD_NOT_REACH;

            //领取奖励
            getUserData().gainItemList(ref.gain_item_list, _context);

            //标记已领取
            markDrawStepReward(_refId, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 标记已领取阶段奖励
     * @param _refId
     * @param _context
     */
    public void markDrawStepReward(long _refId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            PlayerSevenDayGoalsStepRewardDrawRecordBO bo = new PlayerSevenDayGoalsStepRewardDrawRecordBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setRefId(getUSServer().getBM(), _refId);
            bo.insert(getUSServer().getBM());

            _m_hadDrawStepRewardList.add(_refId);

            getUserData().sendMsgToGC(new GS2GC_033_108_OnSevenDayGoalStepRewardDraw(_refId));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 填充协议
     * @param _proto
     */
    public void fillProto(GS2GC_033_004_RetSevenDayGoalsInfo _proto)
    {
        getUserData().lockUser();
        try
        {
            _proto.getHadDrawRewardList().addAll(_m_hadDrawTaskRewardList);
            _proto.setScore(_m_bo.getScore());
            _proto.getHadDrawStepRewardList().addAll(_m_hadDrawStepRewardList);
            for (SevenDayGoalsTaskCounter taskCounter : _m_taskCounterMap.values())
            {
                //计数为0，则不做下发
                if (taskCounter.getExtraCount() == 0)
                    continue;

                _proto.getTaskList().add(taskCounter.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }

    }

    /**
     * 增加计数
     * @param _taskId
     * @param _count
     * @return
     */
    public Result addCounter(long _taskId, long _count, NPPlayerContext _context)
    {
        RefSevenDayGoalsTask refTask = RefSevenDayGoalsTask.getMgr().get(_taskId);
        if (refTask == null)
            return CommErr.REF_NOT_FOUND;

        SevenDayGoalsTaskCounter taskCounter = ensureTaskCounter(refTask);
        if (taskCounter == null)
            return CommErr.REF_NOT_FOUND;

        taskCounter.addCount(_count, _context);
        return Result.SUCC;
    }

    /**
     * 重置计数
     * @param _taskId
     * @param _context
     * @return
     */
    public Result resetCounter(long _taskId, NPPlayerContext _context)
    {
        RefSevenDayGoalsTask refTask = RefSevenDayGoalsTask.getMgr().get(_taskId);
        if (refTask == null)
            return CommErr.REF_NOT_FOUND;

        SevenDayGoalsTaskCounter taskCounter = ensureTaskCounter(refTask);
        if (taskCounter == null)
            return CommErr.REF_NOT_FOUND;

        taskCounter.setCount(0, _context);
        return Result.SUCC;
    }
}
