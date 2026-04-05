package NPUSServer.CommonActivityMgr.Core.ActivityState;

import Common.ActivityEnum.EActivityState;
import NPCommon.Util.CommonFunc;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;

public class ActivityState_LoadFromDb extends _AActivityState
{
    // 冲突等待处理器
    private final WaitingConflictHandler _m_waitingHandler;

    public ActivityState_LoadFromDb(_AActivityBase _activity)
    {
        super(_activity);

        _m_waitingHandler = new WaitingConflictHandler(getActivity(), EActivityState.LOAD_FROM_DB.name());
    }

    @Override
    public EActivityState getStateType()
    {
        return EActivityState.LOAD_FROM_DB;
    }

    @Override
    public boolean judgeCanToState(_AActivityState _state)
    {
        return _state.getStateType() == EActivityState.RESTORE;
    }

    @Override
    public void enter(boolean _isTransByRestore)
    {
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
        if (getActivity().getActivityMgr().isConflictExist(getActivity().getActivityId()))
        {
            _m_waitingHandler.handleWaiting(CommonFunc.getNowTimeMS());
            return null;
        }

        return new ActivityState_Restore(getActivity());
    }
}
