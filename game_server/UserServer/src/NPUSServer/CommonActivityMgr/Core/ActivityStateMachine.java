package NPUSServer.CommonActivityMgr.Core;

import Common.ActivityEnum.EActivityState;
import NPUSServer.CommonActivityMgr.Core.ActivityState.*;
import NPUSServer.USLog;

/***********
 * 活动状态机
 */
public class ActivityStateMachine
{
    /****************
     * 活动对象
     */
    private _AActivityBase _m_activity;
    /**
     * 战斗状态机的当前状态
     */
    private _AActivityState _m_curState;

    public ActivityStateMachine(_AActivityBase _activity)
    {
        _m_activity = _activity;
    }

    public _AActivityBase getActivity()
    {
        return _m_activity;
    }

    public _AActivityState getCurState()
    {
        return _m_curState;
    }

    /*****************
     * 设置指定活动状态
     * @param _state
     * @return
     */
    protected boolean initSetStatus(EActivityState _state)
    {
        _AActivityState handler = getStateHandler(_state);
        if (null == handler)
        {
            USLog.error(_m_activity.getUSServer(),
                    "activity:{} instanceId:{} set state:{} fail, not handler", _m_activity.getActivityId(), _m_activity.getInstanceId(), _state);
            return false;
        }

        _m_curState = handler;

        return true;
    }

    /**
     * 转换到指定状态,判断能否转换的条件
     * @param _state 指定状态
     */
    protected void transToState(_AActivityState _state)
    {
        //判断是否能转换到指定状态
        if (!_m_curState.judgeCanToState(_state))
            return;

        //设置到指定状态
        _setToState(_state);
    }

    /**
     * 实际设置状态机到某个状态
     * @param _state 指定状态
     */
    private void _setToState(_AActivityState _state)
    {
        if (_m_curState != null)
        {
            _m_curState.quit();
        }

        //记录下原状态
        _AActivityState preState = _m_curState;

        //变更活动当前状态
        _m_curState = _state;

        if (_m_curState.needSaveToDb())
            getActivity().saveState(_m_curState.getStateType());

        _onStateChg(preState, _m_curState);

        if (_m_curState != null)
        {
            _m_curState.enter(preState != null && preState.getStateType() == EActivityState.RESTORE);
        }
    }

    /**
     * 状态变更通知
     * @param _perState 前一个状态
     * @param _curState 当前状态
     */
    private void _onStateChg(_AActivityState _perState, _AActivityState _curState)
    {
        USLog.info(_m_activity.getUSServer(), "activity:{} instanceId:{} tran state from [{}] to [{}]",
                getActivity().getActivityId(),
                getActivity().getInstanceId(),
                _perState != null ? _perState.getStateType() : "null",
                _curState != null ? _curState.getStateType() : "null");
    }

    /************************
     * 获取对应状态的handler对象
     * @param _state
     * @return
     */
    public _AActivityState getStateHandler(EActivityState _state)
    {
        switch (_state)
        {
            case PLAN:
                return new ActivityState_Plan(_m_activity);

            case INITIALIZING:
                return new ActivityState_Initializing(_m_activity);

            case PLAYING:
                return new ActivityState_Playing(_m_activity);

            case SETTLING:
                return new ActivityState_Settling(_m_activity);

            case REWARDING:
                return new ActivityState_Rewarding(_m_activity);

            case CLOSED:
                return new ActivityState_Closed(_m_activity);

            case CAN_DISCARD:
                return new ActivityState_CanDiscard(_m_activity);

            case RESTORE:
                return new ActivityState_Restore(_m_activity);

            case LOAD_FROM_DB:
                return new ActivityState_LoadFromDb(_m_activity);

            default:
                USLog.error(_m_activity.getUSServer(),
                        "activity:{} instanceId:{} get state handler:{} fail", _m_activity.getActivityId(), _m_activity.getInstanceId(), _state);
                return null;
        }
    }
}
