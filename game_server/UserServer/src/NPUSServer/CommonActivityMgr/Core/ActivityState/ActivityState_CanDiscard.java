package NPUSServer.CommonActivityMgr.Core.ActivityState;

import Common.ActivityEnum.EActivityState;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;

/**
 * 活动状态机状态 - 已完成状态
 * 可以进行销毁操作
 */
public class ActivityState_CanDiscard extends _AActivityState
{
    public ActivityState_CanDiscard(_AActivityBase _activity)
    {
        super(_activity);
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.CAN_DISCARD;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        //销毁状态 不能再转变状态
        return false;
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
    }


    @Override
    public _AActivityState getCanTransToState(long _nowMs)
    {
        return null;
    }
}
