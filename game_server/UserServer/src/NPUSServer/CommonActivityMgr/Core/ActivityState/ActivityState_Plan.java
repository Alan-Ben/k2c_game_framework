package NPUSServer.CommonActivityMgr.Core.ActivityState;

import Common.ActivityEnum.EActivityState;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;

/**
 * 活动状态机状态 - 待开启状态
 *
 * 主要功能：
 * 1. 进行同步的配置验证和准备工作
 * 2. 检查冲突管理，确认能否开始
 * 3. 快速转换到 INITIALIZING 状态进行异步初始化
 *
 * 设计特点：
 * - 只做轻量级同步操作
 * - 不包含异步流程（异步初始化已移至 INITIALIZING 状态）
 * - 快速转换到下一状态
 *
 * 线程安全：由活动管理器的 tick 机制保证单线程访问
 */
public class ActivityState_Plan extends _AActivityState
{
    // 冲突等待处理器
    private final WaitingConflictHandler _m_waitingHandler;

    public ActivityState_Plan(_AActivityBase _activity)
    {
        super(_activity);
        _m_waitingHandler = new WaitingConflictHandler(_activity, EActivityState.PLAN.name());
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.PLAN;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        // PLAN 状态只能转到 INITIALIZING 状态
        return EActivityState.INITIALIZING == _state.getStateType();
    }

    @Override
    public void enter(boolean _isTransByRestore)
    {
        //通知活动管理器活动状态变更
        getActivity().getActivityMgr().onActivityStateChg(getActivity(), getStateType());
    }

    @Override
    public void quit()
    {
        // 清理等待信息
        _m_waitingHandler.reset(0L);
    }

    @Override
    public _AActivityState getCanTransToState(long _nowMs)
    {
        // 如果当前时间小于活动开始时间，则返回 null
        if (_nowMs < getActivity().getStartTimeMs())
            return null;

        if (getActivity().getActivityMgr().isConflictExist(getActivity().getActivityId()))
        {
            // 存在冲突，无法开始，保持当前状态
            _m_waitingHandler.handleWaiting(_nowMs);
            return null;
        }

        // 直接返回 INITIALIZING 状态，让它处理异步初始化
        return new ActivityState_Initializing(getActivity());
    }
}
