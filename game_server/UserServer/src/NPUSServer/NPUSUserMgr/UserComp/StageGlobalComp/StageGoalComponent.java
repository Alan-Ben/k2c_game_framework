package NPUSServer.NPUSUserMgr.UserComp.StageGlobalComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.Common_LongList;
import Common.PlayerEnum.EPlayerEventRecordType;
import Common.StageGoalObj.StageGoalTask_Info;
import Common.StageGoalObj.StageGoal_Info;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.StageGoalErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.StageGoal.RefStageGoal;
import NPGameRes.Refs.StageGoal.RefStageGoalBigStep;
import NPGameRes.Refs.StageGoal.RefStageGoalTask;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerStageGoalBO;
import USDB.Bo.PlayerStageGoalTaskBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

/**
 * 阶段任务管理对象，只管理当前阶段任务数据
 */
public class StageGoalComponent extends _ANPUserComponent
{
    //阶段数据
    private RefStageGoal _m_ref;
    //阶段目标子任务列表
    private ArrayList<StageGoalTaskInfo> _m_taskList;
    //展示任务列表
    private List<Long> _m_showTaskList;

    //当前阶段
    private PlayerStageGoalBO _m_bo;
    //已领取大阶段奖励列表
    private ArrayList<Long> _m_hadDrawBigStepList;
    //已领取大阶段首达奖励列表
    private ArrayList<Long> _m_hadDrawBigStepFirstReachList;

    public StageGoalComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.STAGE_GOAL);

        _m_taskList = new ArrayList<>();
        _m_hadDrawBigStepList = new ArrayList<>();
        _m_showTaskList = new ArrayList<>();
        _m_hadDrawBigStepFirstReachList = new ArrayList<>();
    }

    private void _lock()
    {
        getUserData().lockUser();
    }

    private void _unlock()
    {
        getUserData().unlockUser();
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("stage_goal_comp_init");

        //步骤1 : 当前阶段目标数据加载
        process.addResDelegateProcess(action -> _initStageGoalFromDB(action::dealAction), "stgae_goal_init",
                () -> USLog.error(getUSServer(), "load stage-goal comp fail, cid:{}", getUserData().getCid()), false);
        //步骤2 : 当前阶段目标的任务数据加载
        process.addResDelegateProcess(action -> _initStageGoalTaskFromDB(action::dealAction), "stgae_goal_task_init",
                () -> USLog.error(getUSServer(), "load stage-goal comp fail, cid:{}", getUserData().getCid()), false);
        //步骤3 : 检查当前任务数据是否有效，是否需要更新下一阶段任务，是否补齐任务数据
        process.addResDelegateProcess(action -> _checkStageGoal(action::dealAction), "stgae_goal_check",
                () -> USLog.error(getUSServer(), "load stage-goal comp fail, cid:{}", getUserData().getCid()), false);

        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "load stage-goal comp fail, cid:{}", getUserData().getCid());
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
     * 从数据库中加载阶段目标数据
     * @param _handler
     */
    private void _initStageGoalFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerStageGoalBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerStageGoalBO>()
        {
            @Override
            public void dealSuc(PlayerStageGoalBO _bo)
            {
                _m_ref = RefStageGoal.getMgr().get(_bo.getStep());
                if (null == _m_ref)
                {
                    USLog.error(getUSServer(), "init stage-goal ref fail cid:{} step:{}", getUserData().getCid(), _bo.getStep());
                    _handler.onRunOver(false);
                    return;
                }

                _m_bo = _bo;

                if (null != _bo.getHadDrawBigStepList())
                {
                    Common_LongList listObj = new Common_LongList();
                    listObj.readPackage(ByteBuffer.wrap(_bo.getHadDrawBigStepList()));

                    _m_hadDrawBigStepList.addAll(listObj.getValueList());
                }

                if (null != _bo.getHadDrawBigStepFirstReachList())
                {
                    Common_LongList listObj = new Common_LongList();
                    listObj.readPackage(ByteBuffer.wrap(_bo.getHadDrawBigStepFirstReachList()));

                    _m_hadDrawBigStepFirstReachList.addAll(listObj.getValueList());
                }

                _handler.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                if (getHasErr())
                {
                    USLog.error(getUSServer(), "init stage-goal bo fail cid:{}", getUserData().getCid());
                    _handler.onRunOver(false);
                    return;
                }

                //预先获取第1阶段配置，如果没有，则表示尚未配置，则不再进行后续初始化操作
                //注：即使未配置，不影响玩家进入游戏
                _m_ref = RefStageGoal.getMgr().get(1L);
                if (null == _m_ref)
                {
                    USLog.error(getUSServer(), "player:{} not init stage-goal data, not set first ref.", getUserData().getCid());
                    _handler.onRunOver(false);
                    return;
                }

                //存储阶段数据
                BM bmObj = getUSServer().getBM();
                PlayerStageGoalBO bo = new PlayerStageGoalBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setStep(bmObj, 1);
                bo.insert(bmObj);

                _m_bo = bo;

                _handler.onRunOver(true);
            }
        });
    }

    /**
     * 从数据库中加载阶段目标任务数据
     * @param _handler
     */
    private void _initStageGoalTaskFromDB(_ICallBackBool _handler)
    {
        final StageGoalComponent comp = this;

        getUSServer().getBM().getBM(PlayerStageGoalTaskBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerStageGoalTaskBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load stage-goal-task fail.", getUserData().getCid());
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerStageGoalTaskBO> _boList)
            {
                for (PlayerStageGoalTaskBO bo : _boList)
                {
                    if (null == bo)
                        continue;

                    if (bo.getStep() != _m_ref.step)
                        continue;

                    RefStageGoalTask taskRef = RefStageGoalTask.getMgr().get(bo.getTaskId());
                    if (null == taskRef)
                        continue;

                    StageGoalTaskInfo taskInfo = new StageGoalTaskInfo(comp, bo, taskRef);
                    _m_taskList.add(taskInfo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    /**
     * 检查阶段目标是否需要更新，是否有新的子任务数据
     * @param _handler
     */
    private void _checkStageGoal(_ICallBackBool _handler)
    {
        if (null == _m_ref)
        {
            _handler.onRunOver(true);
            return;
        }

        for (int i = 0; i < _m_ref.task_list.size(); i++)
        {
            long taskId = _m_ref.task_list.get(i);

            if (null == lookupTask(taskId))
            {
                RefStageGoalTask taskRef = RefStageGoalTask.getMgr().get(taskId);
                if (null == taskRef)
                {
                    USLog.error(getUSServer(), "player:{} taskId:{} build stage-goal-task fail, not find ref.", getUserData().getCid(), taskId);
                    continue;
                }

                if (taskRef.simple_unlock_id == 0)
                {
                    StageGoalTaskInfo subTask = new StageGoalTaskInfo(this, _m_bo.getStep(), taskRef);
                    _m_taskList.add(subTask);
                }else
                {
                    _m_showTaskList.add(taskId);
                }
            }
        }

        _handler.onRunOver(true);
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        _lock();
        try
        {
            for (StageGoalTaskInfo info : _m_taskList)
            {
                if (null == info)
                    continue;

                info.regEvtEntry();
            }

            if (_m_bo.getIsDone()) //主任务已经完成，进入下一个阶段
            {
                dealNextStep(true, getUserData().getPlayerInitContext());
            }
        } finally
        {
            _unlock();
        }
    }

    @Override
    public void dispose()
    {
        _lock();
        try
        {
            for (StageGoalTaskInfo info : _m_taskList)
            {
                if (null == info)
                    continue;

                info.unregEvtEntry();
            }
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 构造初始化数据
     *
     * @param _proto
     */
    public void makeProto(StageGoal_Info _proto)
    {
        _lock();
        try
        {
            _proto.setStep(_m_bo.getStep());
            _proto.setIsDone(_m_bo.getIsDone());
            for (StageGoalTaskInfo subTask : _m_taskList)
            {
                _proto.addTaskList(subTask.toProto());
            }
            for (Long taskId : _m_showTaskList)
            {
                _proto.addTaskList(new StageGoalTask_Info(taskId, 0, false));
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找任务对象
     * @param _taskId
     * @return
     */
    public StageGoalTaskInfo lookupTask(long _taskId)
    {
        _lock();
        try
        {
            for (StageGoalTaskInfo taskInfo : _m_taskList)
            {
                if (taskInfo.getTaskId() == _taskId)
                {
                    return taskInfo;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**********************
     * 完成指定阶段任务
     *
     * @param _stepId
     * @param _context
     */
    public Result doneStep(long _stepId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (_stepId != _m_ref.step)
                return CommErr.PARAM_ERROR;

            //如果阶段目标已经完成，则不再处理
            if (_m_bo.getIsDone())
                return StageGoalErr.STAGE_GOAL_STEP_REWARD_HAD_DRAW;

            //检查是否达到开服时间的要求
            if (_m_ref.next_step_need_server_start_day != 0 && getUSServer().getServerStartDay() < _m_ref.next_step_need_server_start_day)
                return StageGoalErr.STAGE_GOAL_SERVER_START_DAY_NOT_ENOUGH_TO_NEXT_STEP;

            //检查是否满足解锁条件
            if (!NPPlayerConditionDealerMgr.IsEnable(_m_ref.next_step_simple_unlock_id, getUserData(), null))
                return StageGoalErr.STAGE_GOAL_UNLOCK_CONDITION_NOT_MEET_TO_NEXT_STEP;

            //判断是否所有任务都已完成
            if (!isAllTaskDrawed())
            {
                // 领取所有可领取的任务奖励
                for (StageGoalTaskInfo task : _m_taskList)
                {
                    if (task.isRewardDrawed())
                        continue;

                    if (!task.canDone())
                        return StageGoalErr.STAGE_GOAL_TASK_NOT_DONE;

                    // 有任务奖励未领取
                    Result drawTaskResult = task.drawReward(_context);
                    if (!drawTaskResult.isSucc())
                        return drawTaskResult;
                }

                // 检查是否所有任务奖励都已领取
                for (StageGoalTaskInfo task : _m_taskList)
                {
                    if (!task.isRewardDrawed())
                        return StageGoalErr.STAGE_GOAL_TASK_NOT_DONE;
                }
            }

            getUserData().gainItemList(_m_ref.reward_item_list, _context);

            //设置阶段目标完成标志位
            _m_bo.saveIsDone(getUSServer().getBM(), true);

            //设置阶段目标完成时间
            getUserData().getEventRecordComp().setRecord(EPlayerEventRecordType.STAGE_GOAL_FINISH_TIME_MS.ordinal(),
                    _m_ref.Id(), CommonFunc.getNowTimeMS());

            //如果没有进入下一个阶段，需要告知客户端当前阶段的变化
            if (!dealNextStep(false, _context))
            {
                getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_071_OnStageGoalChg(getUserData()));
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**********************
     * 完成指定阶段任务
     *
     * @param _bigStepId
     * @param _context
     */
    public Result doneBigStep(long _bigStepId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            RefStageGoalBigStep refBigStep = RefStageGoalBigStep.getMgr().get(_bigStepId);
            if (refBigStep == null)
                return CommErr.REF_NOT_FOUND;

            //检查是否已经领取奖励
            if (_m_hadDrawBigStepList.contains(_bigStepId))
                return StageGoalErr.STAGE_GOAL_BIG_STEP_REWARD_HAD_DRAW;

            long doneStep = getDoneStep();

            if (refBigStep.done_need_draw_all_step_reward)
            {
                for (RefStageGoal refStageGoal : refBigStep.stage_goal_list)
                {
                    if (doneStep < refStageGoal.step)
                        return StageGoalErr.STAGE_GOAL_STEP_NOT_DONE; // 尚未完成该阶段目标
                }
            }else
            {
                //未完成的阶段
                long notDoneStep = -1;

                for (RefStageGoal refStageGoal : refBigStep.stage_goal_list)
                {
                    if (doneStep < refStageGoal.step)
                    {
                        // 如果有多个未完成的阶段则返回错误
                        if (notDoneStep != -1)
                            return StageGoalErr.STAGE_GOAL_STEP_NOT_DONE; // 尚未完成该阶段目标
                        else
                            notDoneStep = refStageGoal.step; // 尚未完成该阶段目标
                    }
                }

                //尝试完成未完成的阶段目标
                if (notDoneStep != -1)
                {
                    Result result = doneStep(notDoneStep, _context);
                    if (!result.isSucc())
                        return result;
                }
            }

            _m_hadDrawBigStepList.add(_bigStepId);

            getUserData().gainItemList(refBigStep.reward_item_list, _context);

            //保存数据
            Common_LongList hadDrawBigStepList = new Common_LongList();
            hadDrawBigStepList.getValueList().addAll(_m_hadDrawBigStepList);
            _m_bo.saveHadDrawBigStepList(getUSServer().getBM(), hadDrawBigStepList.makePackage().array());

            //推送任务完成数据
            getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_069_OnStageGoalBigStepRewardDraw(_bigStepId));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**********************
     * 完成指定阶段任务
     *
     * @param _bigStepId
     * @param _context
     */
    public Result drawBigStepFirstReachReward(long _bigStepId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            boolean isUnlock = getUSServer().getStageGoalFirstReachMgr().isUnlock(_bigStepId);
            if (!isUnlock)
                return StageGoalErr.STAGE_GOAL_BIG_STEP_FIRST_REACH_REWARD_NOT_UNLOCK;

            //检查是否已经领取奖励
            if (_m_hadDrawBigStepFirstReachList.contains(_bigStepId))
                return StageGoalErr.STAGE_GOAL_BIG_STEP_FIRST_REACH_REWARD_HAD_DRAW;

            RefStageGoalBigStep refStageGoalBigStep = RefStageGoalBigStep.getMgr().get(_bigStepId);
            if (refStageGoalBigStep == null)
                return  CommErr.REF_NOT_FOUND;

            _m_hadDrawBigStepFirstReachList.add(_bigStepId);

            getUserData().gainItemList(refStageGoalBigStep.first_reach_reward_item_list, _context);

            //保存数据
            Common_LongList hadDrawBigStepFirstReachList = new Common_LongList();
            hadDrawBigStepFirstReachList.getValueList().addAll(_m_hadDrawBigStepFirstReachList);
            _m_bo.saveHadDrawBigStepFirstReachList(getUSServer().getBM(), hadDrawBigStepFirstReachList.makePackage().array());

            //推送任务完成数据
            getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_078_OnStageGoalBigStepFirstReachRewardDraw(_bigStepId));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置阶段目标到指定步骤的通用方法
     * @param _targetStep         目标步骤
     * @param _shouldPushProtocol 是否推送协议
     * @param _context            操作上下文
     * @return 设置是否成功
     */
    private boolean setStageStep(long _targetStep, boolean _shouldPushProtocol, NPPlayerContext _context)
    {
        _lock();
        try
        {
            RefStageGoal nextRef = RefStageGoal.getMgr().get(_targetStep);
            if (null == nextRef)
                return false;

            //清除当前任务数据
            clearAllTask();

            //更新下一个阶段数据
            _m_ref = nextRef;
            //设置完成标志位
            _m_bo.setIsDone(getUSServer().getBM(), false);
            _m_bo.setStep(getUSServer().getBM(), _targetStep);
            _m_bo.saveAllMarked(getUSServer().getBM());

            //构建下一个阶段所有任务数据
            buildAllTask();

            //根据参数决定是否推送协议
            if (_shouldPushProtocol)
                getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_071_OnStageGoalChg(getUserData()));

        } finally
        {
            _unlock();
        }

        //记录大阶段首达数据
        RefStageGoalBigStep refBigStep = RefStageGoalBigStep.getMgr().getBigStep(_targetStep);
        if (refBigStep != null)
            getUSServer().getStageGoalFirstReachMgr().recordFirstReach(getCid(),
                    getUserData().getPlayerComponent().getName(), refBigStep, CommonFunc.getNowTimeMS());

        getUSServer().getStageGoalFirstReachMgr().recordTop1PlayerData(getCid(), getUserData().getPlayerComponent().getEarnings(), _targetStep);

        return true;
    }

    /*********************
     * 执行下一个阶段目标数据
     *
     * @param _bInit
     * @param _context
     * @return
     */
    protected boolean dealNextStep(boolean _bInit, NPPlayerContext _context)
    {
        // 检查当前阶段是否已完成
        if (!_m_bo.getIsDone())
            return false;

        long nextStep = _m_ref.step + 1;

        // 调用通用方法，非初始化时推送协议
        return setStageStep(nextStep, !_bInit, _context);
    }

    /*****************
     * 清除当前所有任务数据
     */
    protected void clearAllTask()
    {
        //移除旧阶段任务数据
        getUSServer().getBM().getBM(PlayerStageGoalTaskBO.class).delAll("cid", getUserData().getCid());

        //移除子任务数据
        for (StageGoalTaskInfo task : _m_taskList)
        {
            if (null == task)
                continue;

            task.unregEvtEntry();
        }

        _m_taskList.clear();
        _m_showTaskList.clear();
    }

    /**********************
     * 构造所有任务数据
     */
    protected void buildAllTask()
    {
        if (null == _m_ref)
        {
            USLog.error(getUSServer(), "player:{} build All Task fail, not find ref", getUserData().getCid());
            return;
        }

        //构造子任务数据
        for (Long taskId : _m_ref.task_list)
        {
            RefStageGoalTask taskRef = RefStageGoalTask.getMgr().get(taskId);
            if (null == taskRef)
            {
                USLog.error(getUSServer(), "player:{} taskId:{} build stage-goal-task fail, not find ref.", getUserData().getCid(), taskId);
                continue;
            }

            if (taskRef.simple_unlock_id == 0)
            {
                StageGoalTaskInfo subTask = new StageGoalTaskInfo(this, _m_bo.getStep(), taskRef);
                _m_taskList.add(subTask);
                //注册监听
                subTask.regEvtEntry();
            }else
            {
                _m_showTaskList.add(taskId);
            }
        }
    }

    /**************
     * 强制设置到指定阶段
     *
     * @param _step
     * @param _context
     * @return
     */
    public boolean cmdSetStep(long _step, NPPlayerContext _context)
    {
        // 调用通用方法，强制设置时总是推送协议
        return setStageStep(_step, true, _context);
    }

    /**
     * 是否指定阶段完成
     * @param _step
     * @return
     */
    public boolean isSelectStageDone(long _step)
    {
        _lock();
        try
        {
            return getDoneStep() >= _step;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 是否所有任务完成
     * @return
     */
    public boolean isAllTaskDrawed()
    {
        _lock();
        try
        {
            for (StageGoalTaskInfo taskInfo : _m_taskList)
            {
                if (!taskInfo.isRewardDrawed())
                    return false;
            }
            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取当前阶段目标已完成的阶段
     * @return
     */
    public long getDoneStep()
    {
        _lock();
        try
        {
            if (_m_bo.getIsDone())
                return _m_bo.getStep();

            return _m_bo.getStep() - 1;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取已领取大阶段奖励列表
     * @return
     */
    public List<Long> getHadDrawBigStepList()
    {
        getUserData().lockUser();
        try
        {
            return new ArrayList<>(_m_hadDrawBigStepList);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取已领取大阶段首达奖励列表
     * @return
     */
    public List<Long> getHadDrawBigStepFirstReachList()
    {
        getUserData().lockUser();
        try
        {
            return new ArrayList<>(_m_hadDrawBigStepFirstReachList);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清空大阶段奖励领取记录
     * 清空玩家已领取的大阶段奖励记录
     */
    public void clearBigStepDrawRecords()
    {
        _lock();
        try
        {
            // 清空内存中的大阶段奖励领取记录列表
            _m_hadDrawBigStepList.clear();

            // 清空数据库中的大阶段奖励领取记录数据
            _m_bo.saveHadDrawBigStepList(getUSServer().getBM(), null);

        } finally
        {
            _unlock();
        }
    }

    /**
     * 清空大阶段首达奖励领取记录
     * 清空玩家已领取的大阶段首达奖励记录
     */
    public void clearBigStepFirstReachDrawRecords()
    {
        _lock();
        try
        {
            // 清空内存中的大阶段首达奖励领取记录列表
            _m_hadDrawBigStepFirstReachList.clear();

            // 清空数据库中的大阶段首达奖励领取记录数据
            _m_bo.saveHadDrawBigStepFirstReachList(getUSServer().getBM(), null);

        } finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("{");
        sb.append("\"step\":").append(_m_bo.getStep()).append(",");
        sb.append("\"is_done\":").append(_m_bo.getIsDone()).append(",");
        sb.append("\"had_draw_big_step_list\":").append(_m_hadDrawBigStepList.toString()).append(",");
        sb.append("\"had_draw_big_step_first_reach_list\":").append(_m_hadDrawBigStepFirstReachList.toString()).append(",");
        sb.append("\"task_list\":[");
        for (int i = 0; i < _m_taskList.size(); i++)
        {
            if (i > 0)
                sb.append(",");
            sb.append(_m_taskList.get(i).toString());
        }
        sb.append("]");
        sb.append("}");
        return sb.toString();
    }

}
