package NPUSServer.CommonActivityMgr.Core.ActivityState;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._AALProcess;
import Common.ActivityEnum.EActivityState;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.USLog;

/**
 * 活动状态机状态 - 已关闭
 *
 * 主要功能：
 * 1. 异步关闭所有排行榜实例
 * 2. 异步关闭所有阶段奖励实例
 *
 * 执行流程：
 * 1. enter() 时启动异步清理流程
 * 2. 清理完成后设置 _m_nextState 为 CAN_DISCARD
 * 3. 清理失败也设置 _m_nextState 为 CAN_DISCARD（降级处理）
 * 4. tick 机制通过 getCanTransToState() 驱动状态转换
 *
 * 设计特点：
 * - 纯粹的中间态，负责异步资源清理
 * - 遵循 tick 驱动的被动转换模式
 * - 在 enter() 中启动异步流程
 * - 异步完成后设置 _m_nextState，由 tick 驱动转换
 *
 * 线程安全：由活动管理器的 tick 机制保证单线程访问
 */
public class ActivityState_Closed extends _AActivityState
{
    // 存储异步流程完成后的下一个状态（null 表示未完成）
    private _AActivityState _m_nextState = null;

    public ActivityState_Closed(_AActivityBase _activity)
    {
        super(_activity);
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.CLOSED;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        //已关闭状态 只能转到 完成状态
        return EActivityState.CAN_DISCARD == _state.getStateType();
    }

    @Override
    public void enter(boolean _isTransByRestore)
    {
        if (!_isTransByRestore)
        {
            getActivity().onActivityClosed();
        }

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
        // 返回异步流程设置的下一个状态
        // null 表示异步流程尚未完成，保持当前状态
        return _m_nextState;
    }

    /**
     * 启动异步清理流程
     *
     * 执行流程：
     * 1. 创建异步流程对象
     * 2. 添加清理任务（关闭排行榜、阶段奖励）
     * 3. 启动流程并监听结果
     * 4. 成功/失败 → 设置 _m_nextState 为 CAN_DISCARD
     * 5. tick 机制通过 getCanTransToState() 驱动状态转换
     */
    private void _startCleanupProcess()
    {
        USLog.info(getActivity().getUSServer(),
            "ActivityState_Closed._startCleanupProcess - start async cleanup: activityId={}, instanceId={}",
            getActivity().getActivityId(), getActivity().getInstanceId());

        ALProcess process = ALProcess.CreateProcess("activity_closed_cleanup_process");

        // 关闭排行榜
        getActivity().closeAllRank(process);
        // 关闭阶段奖励
        getActivity().closeAllStepReward(process);

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
                // 清理失败
                USLog.error(getActivity().getUSServer(),
                    "ActivityState_Closed._startCleanupProcess - async cleanup failed, activityId={}, instanceId={}",
                    getActivity().getActivityId(), getActivity().getInstanceId());
            }

            @Override
            public void onRootProecssSuc()
            {
                // 清理成功，设置下一状态为 CAN_DISCARD
                USLog.info(getActivity().getUSServer(),
                    "ActivityState_Closed._startCleanupProcess - async cleanup success: next state will be CAN_DISCARD, activityId={}, instanceId={}",
                    getActivity().getActivityId(), getActivity().getInstanceId());

                // 设置下一个状态，由 tick 驱动转换
                _m_nextState = new ActivityState_CanDiscard(getActivity());
            }
        });
    }
}
