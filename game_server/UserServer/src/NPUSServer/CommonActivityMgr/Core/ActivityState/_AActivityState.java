package NPUSServer.CommonActivityMgr.Core.ActivityState;

import Common.ActivityEnum.EActivityState;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;

/*******************
 * 活动状态机状态
 */
public abstract class _AActivityState
{
    /****************
     * 活动对象
     */
    private _AActivityBase _m_activity;

    protected _AActivityState(_AActivityBase _activity)
    {
        _m_activity = _activity;
    }

    public _AActivityBase getActivity()
    {
        return _m_activity;
    }

    /**
     * 获得状态类型
     * @return 状态id
     */
    abstract public EActivityState getStateType();

    /**
     * 判断能否切入状态
     * @param _state 上一个状态
     * @return boolean
     */
    abstract public boolean judgeCanToState(_AActivityState _state);

    /**
     * 进入本状态处理
     */
    abstract public void enter(boolean _isTransByRestore);

    /**
     * 离开本状态处理
     */
    abstract public void quit();

    /**
     * 获取可以变更的目标状态
     * @param _nowMs 当前时间
     * @return 目标状态
     */
    public abstract _AActivityState getCanTransToState(long _nowMs);

    public boolean needSaveToDb()
    {
    	return true;
    }
}
