package NPUSServer.CommonActivityMgr.Core.ActivityState;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._AALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ActivityEnum.EActivityState;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.USLog;
import NPUSServer.UsActivityScheduleMgr.UsActivityReportSettlingToSsTask;

/**
 * 活动状态机状态 - 结算中
 *
 * 主要功能：
 * 1. 异步注销排行榜事件监听
 * 2. 异步注销阶段奖励事件监听
 * 3. 异步注销活动事件监听
 * 4. 异步注销活动关联礼包
 * 5. 等待所有排行榜结算完毕
 * 6. 向 SS 报告结算状态
 *
 * 执行流程：
 * 1. enter() 时启动异步清理流程
 * 2. 清理完成后设置 _m_cleanupDone 标志
 * 3. getCanTransToState() 检查多个条件：清理完成、排行榜结算、时间到达
 * 4. 所有条件满足后返回 REWARDING 状态
 * 5. tick 机制驱动状态转换
 *
 * 设计特点：
 * - 纯粹的中间态，负责异步清理和等待结算
 * - 遵循 tick 驱动的被动转换模式
 * - 在 enter() 中启动异步流程
 * - 异步完成后设置标志，等待其他条件满足后由 tick 驱动转换
 *
 * 线程安全：由活动管理器的 tick 机制保证单线程访问
 */
public class ActivityState_Settling extends _AActivityState
{
    // 标记清理流程是否已完成（成功或失败）
    private boolean _m_cleanupDone = false;
    // 标记是否已向 SS 报告过结算状态
    private boolean _m_hadReportSettling = false;

    public ActivityState_Settling(_AActivityBase _activity)
    {
        super(_activity);
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.SETTLING;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        //冻结中状态 只能转到 冻结状态
        return _state.getStateType() == EActivityState.REWARDING;
    }

    @Override
    public void enter(boolean _isTransByRestore)
    {
        // 通知活动管理器活动状态变更
        getActivity().getActivityMgr().onActivityStateChg(getActivity(), getStateType());

        getActivity().pushActivityStateChg(getStateType());

        // 启动异步清理流程
        _startCleanupProcess();
    }

    @Override
    public void quit()
    {
    }

    @Override
    public _AActivityState getCanTransToState(long _nowMs)
    {
        // 如果清理流程还未完成，则不可转换
        if (!_m_cleanupDone)
            return null;

        // 检查是否已经向 SS 报告过结算状态
        if (!_m_hadReportSettling)
        {
            _m_hadReportSettling = true;

            // 通知 SS 活动进入结算状态
            ALSynTaskManager.getInstance().regTask(
                new UsActivityReportSettlingToSsTask(getActivity().getUSServer(), getActivity().getSchedule())
            );
        }

        // 判断所有排行榜是否都已经结算完毕
        if (!getActivity().isAllRankCanDrawReward())
        {
            USLog.warn(getActivity().getUSServer(),
                "ActivityState_Settling.getCanTransToState - waiting for all rank settle: activityId={}, instanceId={}",
                getActivity().getActivityId(), getActivity().getInstanceId());
            return null;
        }

        // 如果当前时间小于活动结算时间，则返回 null
        if (_nowMs < getActivity().getSettleTimeMs())
            return null;

        // 所有条件都满足，转换到 REWARDING 状态
        return new ActivityState_Rewarding(getActivity());
    }

    /**
     * 启动异步清理流程
     *
     * 执行流程：
     * 1. 创建异步流程对象
     * 2. 添加清理任务（注销排行榜、奖励、事件、礼包）
     		补充任务1：记录当前公会数据，用于后续结算
     * 3. 启动流程并监听结果
     * 4. 成功/失败 → 标记 _m_cleanupDone = true
     * 5. getCanTransToState() 检查所有条件后返回下一状态
     * 6. tick 机制驱动状态转换
     */
    private void _startCleanupProcess()
    {
        USLog.info(getActivity().getUSServer(),
            "ActivityState_Settling._startCleanupProcess - start async cleanup: activityId={}, instanceId={}",
            getActivity().getActivityId(), getActivity().getInstanceId());

        ALProcess process = ALProcess.CreateProcess("activity_settling_cleanup_process");

        // 注销排行榜事件
        getActivity().unRegAllRankEvent(process);
        // 注销阶段奖励事件
        getActivity().unRegAllStepRewardEvent(process);
        // 注销活动事件
        getActivity().unRegActivityEvent(process);
        // 注销活动关联礼包
        getActivity().unRegActivityGiftPack(process);
        //记录当前公会数据，用于后续结算
        getActivity().dumpAllGuild(process);
        //记录当前本服的队伍数据（队长是本服且不能更换）
        getActivity().dumpAllTeam(process);

        // 开启流程并监听结果
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public long monitorTimeMS(String _processTag)
            {
                return 1000; // 每秒检查一次超时
            }

            @Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
                // 记录具体错误信息
                getActivity().onActivityStateTransErr(_processTag, _exInfo, _ex);
            }

            @Override
            public void onProcessFailStop(_AALProcess _process)
            {
                // 记录失败的流程标签
                getActivity().onActivityStateTransFail(_process.getFullProcessTag());
            }

            @Override
            public void onRootProecssStop()
            {
                // 清理失败，记录错误日志
                USLog.error(getActivity().getUSServer(),
                    "ActivityState_Settling._startCleanupProcess - async cleanup failed: activityId={}, instanceId={}",
                    getActivity().getActivityId(), getActivity().getInstanceId());
            }

            @Override
            public void onRootProecssSuc()
            {
                // 清理成功，标记完成
                USLog.info(getActivity().getUSServer(),
                    "ActivityState_Settling._startCleanupProcess - async cleanup success: activityId={}, instanceId={}, waiting for rank settle and time",
                    getActivity().getActivityId(), getActivity().getInstanceId());

                // 标记清理完成，等待排行榜结算和时间条件满足
                _m_cleanupDone = true;
            }
        });
    }
}
