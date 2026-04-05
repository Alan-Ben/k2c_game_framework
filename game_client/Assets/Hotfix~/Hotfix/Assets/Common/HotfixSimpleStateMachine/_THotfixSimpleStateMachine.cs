
using System;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 一个极其简单的状态机，只有状态进入和退出调用
    /// author: Coda
    /// </summary>
    public class _THotfixSimpleStateMachine<T> where T : Enum
    {
        // 当前所处的状态
        [NotNull] private _AHotfixSimpleStateBase<T> _m_curState;

        public _THotfixSimpleStateMachine()
        {
            // 构造一个空状态
            _m_curState = new HotfixNoneSimpleState<T>();
        }

        /// <summary>
        /// 当状态机的状态发生变化
        /// </summary>
        public event Action<T, T> onStateChg;

        /// <summary>
        /// 当前所处的状态
        /// </summary>
        [NotNull] public _AHotfixSimpleStateBase<T> curState { get { return _m_curState; } }

        /// <summary>
        /// 切换状态
        /// </summary>
        public void changeState([NotNull] _AHotfixSimpleState<T> _state)
        {
            if (!_m_curState.canEnterState(_state.state))
                return;

            T lastStateType = _m_curState.state;
            _m_curState.exit();
            _m_curState = _state;
            _state.enter();
            onStateChg?.Invoke(lastStateType, _state.state);
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <remarks>
        /// 目标状态的进入带有一个参数
        /// </remarks>
        public void changeState<PARAM_1>([NotNull] _AHotfixSimpleState<T, PARAM_1> _state, PARAM_1 _param1)
        {
            if (!_m_curState.canEnterState(_state.state))
                return;

            T lastStateType = _m_curState.state;
            _m_curState.exit();
            _m_curState = _state;
            _state.enter(_param1);
            onStateChg?.Invoke(lastStateType, _state.state);
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <remarks>
        /// 目标状态的进入带有两个参数
        /// </remarks>
        public void changeState<PARAM_1, PARAM_2>([NotNull] _AHotfixSimpleState<T, PARAM_1, PARAM_2> _state, PARAM_1 _param1, PARAM_2 _param2)
        {
            if (!_m_curState.canEnterState(_state.state))
                return;

            T lastStateType = _m_curState.state;
            _m_curState.exit();
            _m_curState = _state;
            _state.enter(_param1, _param2);
            onStateChg?.Invoke(lastStateType, _state.state);
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <remarks>
        /// 目标状态的进入带有三个参数
        /// </remarks>
        public void changeState<PARAM_1, PARAM_2, PARAM_3>([NotNull] _AHotfixSimpleState<T, PARAM_1, PARAM_2, PARAM_3> _state, PARAM_1 _param1, PARAM_2 _param2, PARAM_3 _param3)
        {
            if (!_m_curState.canEnterState(_state.state))
                return;

            T lastStateType = _m_curState.state;
            _m_curState.exit();
            _m_curState = _state;
            _state.enter(_param1, _param2, _param3);
            onStateChg?.Invoke(lastStateType, _state.state);
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <remarks>
        /// 目标状态的进入带有四个参数
        /// </remarks>
        public void changeState<PARAM_1, PARAM_2, PARAM_3, PARAM_4>([NotNull] _AHotfixSimpleState<T, PARAM_1, PARAM_2, PARAM_3, PARAM_4> _state, PARAM_1 _param1, PARAM_2 _param2, PARAM_3 _param3, PARAM_4 _param4)
        {
            if (!_m_curState.canEnterState(_state.state))
                return;

            T lastStateType = _m_curState.state;
            _m_curState.exit();
            _m_curState = _state;
            _state.enter(_param1, _param2, _param3, _param4);
            onStateChg?.Invoke(lastStateType, _state.state);
        }
    }
}
