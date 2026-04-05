package NPMiniGame.GameLife;

/**
 * @description: 小游戏生命周期状态机
 * @author: ricci
 * @date: 2022-07-30 13:44:47
 */
public class NPMiniGameLifeMachine
{
    /**
     * 状态机当前状态
     */
    private _IMiniGameLifeState _m_curState;


    public NPMiniGameLifeMachine()
    {
    }

    public _IMiniGameLifeState getCurState()
    {
        return _m_curState;
    }


    /**
     * 进入某状态
     * @param _newState 新状态
     * @return 是否切换成功
     */
    public boolean transToState(_IMiniGameLifeState _newState)
    {
        //当前存在状态，且当前状态无法转换为新状态，判定为转换失败
        if (_m_curState != null && !_m_curState.judgeCanTransToThatState(_newState))
        {
            return false;
        }

        //切换状态
        __changeState(_newState);

        return true;
    }


    /**
     * 变更当前状态
     * @param _newState 新状态
     */
    protected void __changeState(_IMiniGameLifeState _newState)
    {
        //退出当前状态
        if (_m_curState != null)
        {
            _m_curState.quitState();
        }
        //记录变更前状态
        _IMiniGameLifeState perState = _m_curState;
        //变更当前状态
        _m_curState = _newState;

        //执行进入当前状态函数
        if (_m_curState != null)
        {
            _m_curState.enterState();
        }
        __onStateChg(perState, _m_curState);
    }

    /**
     * 变更状态通知
     * @param _perState 变更前状态
     * @param _curState 变更后状态
     */
    protected void __onStateChg(_IMiniGameLifeState _perState, _IMiniGameLifeState _curState)
    {
//        CommLog.info("NPMiniGameLifeMachine: tran state from [{}] to [{}]",
//                _perState != null ? _perState.getStateType() : "null",
//                _curState != null ? _curState.getStateType() : "null");
    }

}
