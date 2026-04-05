package NPUSServer.CommonActivityMgr.Core.ActivityState;

import Common.ActivityEnum.EActivityState;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;

/**
 * 活动状态机状态 - 领奖期
 * ------------------------
 * 在进入领奖期状态时，需要向SS同步活动领奖期状态
 * 等到所有排行榜都结算完毕后，才能转到已关闭状态
 */
public class ActivityState_Rewarding extends _AActivityState
{
    public ActivityState_Rewarding(_AActivityBase _activity)
    {
        super(_activity);
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.REWARDING;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        //冻结中状态 只能转到 已关闭状态
        return _state.getStateType() == EActivityState.CLOSED;
    }

    @Override
    public void enter(boolean _isTransByRestore)
    {
        if (!_isTransByRestore)
        {
            getActivity().onActivityEnd();
        }

        //通知活动管理器活动状态变更
        getActivity().getActivityMgr().onActivityStateChg(getActivity(), getStateType());

        getActivity().pushActivityStateChg(getStateType());
    }

    @Override
    public void quit()
    {
    }

    @Override
    public _AActivityState getCanTransToState(long _nowMs)
    {
        //如果当前时间小于活动关闭时间，则返回null
        if (_nowMs < getActivity().getCloseTimeMs())
            return null;

        return new ActivityState_Closed(getActivity());
    }

}
