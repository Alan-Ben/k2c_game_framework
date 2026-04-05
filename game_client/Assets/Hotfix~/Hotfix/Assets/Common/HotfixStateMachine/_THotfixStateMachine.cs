using System;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 状态机, 仿造主工程_TALStateMachine
    /// </summary>
    /// <typeparam name="STATE_T"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class _THotfixStateMachine<STATE_T, T> where T : Enum where STATE_T : _ATHotfixStateBase<T>
    {
        //状态工厂对象
        private _ATHotfixStateFactory<STATE_T, T> _m_stateFactory;
        //当前状态对应类
        private Type _m_tType;
        // 当前所处的状态
        [NotNull] private STATE_T _m_curState;

        public _THotfixStateMachine(_ATHotfixStateFactory<STATE_T, T> _factory)
        {
            _m_stateFactory = _factory;
            // 构造一个空状态
            _m_tType = null;
            _m_curState = null;
        }
        /// <summary>
        /// 默认状态不在工厂控制器之内
        /// </summary>
        /// <param name="_factory"></param>
        /// <param name="_defaultState"></param>
        public _THotfixStateMachine(_ATHotfixStateFactory<STATE_T, T> _factory, STATE_T _defaultState)
        {
            _m_stateFactory = _factory;
            // 构造一个空状态
            _m_tType = null;
            _m_curState = _defaultState;
        }

        /// <summary>
        /// 当状态机的状态发生变化
        /// </summary>
        public event Action onStateChg;

        /// <summary>
        /// 当前所处的状态
        /// </summary>
        [NotNull] public STATE_T curState { get { return _m_curState; } }

        /// <summary>
        /// 
        /// </summary>
        public void discard()
        {
            if (null != _m_curState)
            {
                _m_curState.exit();
                //放回缓存
                _pushbackState(_m_tType, _m_curState);
                _m_tType = null;
                _m_curState = null;
            }

            //调用子类释放
            _onDiscard();
        }

        /// <summary>
        /// 执行当前状态的一次 tick
        /// </summary>
        public void tick(float _deltaTime)
        {
            if(null != _m_curState)
                _m_curState.tick(_deltaTime);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public bool changeState(Type _stateType)
        {
            //取出一个状态
            STATE_T newState = _popState(_stateType);
            if (null == newState)
                return false;

            if (null != _m_curState && !_m_curState.canEnterState(newState))
            {
                _pushbackState(_stateType, newState);
                return false;
            }

            if(!newState.canEnterSelf(_m_curState))
            {
                _pushbackState(_stateType, newState);
                return false;
            }

            if (null != _m_curState)
            {
                _m_curState.exit();
                //放回缓存
                _pushbackState(_m_tType, _m_curState);
                _m_tType = null;
            }

            _m_tType = _stateType;
            _m_curState = newState;

            if (null != _m_curState)
                _m_curState.enter();

            onStateChg?.Invoke();

            return true;
        }
        /// <summary>
        /// 切换到某一个状态，并通过带入的函数设置状态值
        /// </summary>
        public bool changeState<S_T>() where S_T : STATE_T
        {
            return changeState<S_T>(null);
        }
        public bool changeState<S_T>(Action<S_T> _setStateAction) where S_T : STATE_T
        {
            //取出一个状态
            S_T newState = (S_T)_popState(typeof(S_T));
            if (null == newState)
                return false;

            //设置变量
            if (null != _setStateAction)
                _setStateAction(newState);

            if (null != _m_curState && !_m_curState.canEnterState(newState))
            {
                _pushbackState(typeof(S_T), newState);
                return false;
            }

            if (!newState.canEnterSelf(_m_curState))
            {
                _pushbackState(typeof(S_T), newState);
                return false;
            }

            if (null != _m_curState)
            {
                _m_curState.exit();
                //放回缓存
                _pushbackState(_m_tType, _m_curState);
                _m_tType = null;
            }

            _m_tType = typeof(S_T);
            _m_curState = newState;

            if (null != _m_curState)
                _m_curState.enter();

            onStateChg?.Invoke();

            return true;
        }
        /// <summary>
        /// 检查状态类型，并尝试切换到某一个状态，并通过带入的函数设置状态值
        /// 假设状态本身一致则不会获取新缓存处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_stateType"></param>
        /// <param name="_setStateAction"></param>
        public bool checkAndChangeState<S_T>() where S_T : STATE_T
        {
            return checkAndChangeState<S_T>(null);
        }
        public bool checkAndChangeState<S_T>(Action<S_T> _setStateAction) where S_T : STATE_T
        {
            //判断状态是否一致，一致则直接处理
            if (_m_curState is S_T)
            {
                if (null != _setStateAction)
                    _setStateAction((S_T)_m_curState);

                return true;
            }

            //取出一个状态
            S_T newState = (S_T)_popState(typeof(S_T));
            if (null == newState)
                return false;

            //设置变量
            if (null != _setStateAction)
                _setStateAction(newState);

            if (null != _m_curState && !_m_curState.canEnterState(newState))
            {
                _pushbackState(typeof(S_T), newState);
                return false;
            }

            if (!newState.canEnterSelf(_m_curState))
            {
                _pushbackState(typeof(S_T), newState);
                return false;
            }

            if (null != _m_curState)
            {
                _m_curState.exit();
                //放回缓存
                _pushbackState(_m_tType, _m_curState);
                _m_tType = null;
            }

            _m_tType = typeof(S_T);
            _m_curState = newState;

            if (null != _m_curState)
                _m_curState.enter();

            onStateChg?.Invoke();

            return true;
        }

        /// <summary>
        /// 检查状态序列号，并对指定类型状态进行设置处理
        /// </summary>
        /// <typeparam name="S_T"></typeparam>
        /// <param name="_stateSerialize"></param>
        /// <param name="_setStateAction"></param>
        /// <returns></returns>
        public bool checkAndSetStateData<S_T>(long _stateSerialize, Action<S_T> _setStateAction) where S_T : STATE_T
        {
            //判断状态是否一致，且类型也需要一致
            if (_m_curState is S_T && _stateSerialize == _m_curState.enterSerialize)
            {
                if (null != _setStateAction)
                    _setStateAction((S_T)_m_curState);

                return true;
            }

            return false;
        }


        /// <summary>
        /// 强制切换状态
        /// </summary>
        public void setState(Type _stateType)
        {
            //取出一个状态
            STATE_T newState = _popState(_stateType);
            if (null == newState)
                return;

            if (null != _m_curState)
            {
                _m_curState.exit();
                //放回缓存
                _pushbackState(_m_tType, _m_curState);
                _m_tType = null;
            }

            _m_tType = _stateType;
            _m_curState = newState;

            if (null != _m_curState)
                _m_curState.enter();

            onStateChg?.Invoke();
        }
        public void setState<S_T>() where S_T : STATE_T
        {
            setState<S_T>(null);
        }
        public void setState<S_T>(Action<S_T> _setStateAction) where S_T : STATE_T
        {
            //取出一个状态
            S_T newState = (S_T)_popState(typeof(S_T));
            if (null == newState)
                return;

            //设置变量
            if (null != _setStateAction)
                _setStateAction(newState);

            if (null != _m_curState)
            {
                _m_curState.exit();
                //放回缓存
                _pushbackState(_m_tType, _m_curState);
                _m_tType = null;
            }

            _m_tType = typeof(S_T);
            _m_curState = newState;

            if (null != _m_curState)
                _m_curState.enter();

            onStateChg?.Invoke();
        }


        /// <summary>
        /// 检查当前状态序列号之后，匹配情况下强制切换状态
        /// </summary>
        public void checkAndSetState(long _curStateSerialize, Type _stateType)
        {
            //序列号不匹配直接处理
            if (null != _m_curState && _m_curState.enterSerialize != _curStateSerialize)
                return;

            setState(_stateType);
        }
        public void checkAndSetState<S_T>(long _curStateSerialize) where S_T : STATE_T
        {
            //序列号不匹配直接处理
            if (null != _m_curState && _m_curState.enterSerialize != _curStateSerialize)
                return;

            setState<S_T>();
        }
        public void checkAndSetState<S_T>(long _curStateSerialize, Action<S_T> _setStateAction) where S_T : STATE_T
        {
            //序列号不匹配直接处理
            if (null != _m_curState && _m_curState.enterSerialize != _curStateSerialize)
                return;

            setState<S_T>(_setStateAction);
        }
        public void checkAndSetState<SRC_T, S_T>(Func<SRC_T, bool> _judgeFunc) where SRC_T : STATE_T where S_T : STATE_T
        {
            if (null == _judgeFunc)
                return;

            //判断当前状态是否匹配类型，不匹配则不处理
            if (!(_m_curState is SRC_T))
                return;

            //序列号不匹配直接处理
            if (null != _m_curState && !_judgeFunc((SRC_T)_m_curState))
                return;

            setState<S_T>();
        }
        public void checkAndSetState<SRC_T, S_T>(Func<SRC_T, bool> _judgeFunc, Action<S_T> _setStateAction) where SRC_T : STATE_T where S_T : STATE_T
        {
            if (null == _judgeFunc)
                return;

            //判断当前状态是否匹配类型，不匹配则不处理
            if (!(_m_curState is SRC_T))
                return;

            //序列号不匹配直接处理
            if (null != _m_curState && !_judgeFunc((SRC_T)_m_curState))
                return;

            setState<S_T>(_setStateAction);
        }

        /// <summary>
        /// 取出一个状态对象
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        protected STATE_T _popState(Type _type)
        {
            if (null == _m_stateFactory)
                return null;

            STATE_T state = _m_stateFactory.popItem(_type);
            _onPopState(state);
            return state;
        }

        protected void _pushbackState(Type _type, STATE_T _state)
        {
            if (null == _m_stateFactory || null == _type)
                return ;

            _onPushBackState(_state);
            _m_stateFactory.pushbackItem(_type, _state);
        }

        /// <summary>
        /// 从状态工厂取出状态后的处理
        /// </summary>
        protected virtual void _onPopState(STATE_T _state)
        {
        }
        /// <summary>
        /// 在将状态放回工厂前的处理
        /// </summary>
        protected virtual void _onPushBackState(STATE_T _state)
        {
        }

        /// <summary>
        /// 释放操作实践
        /// </summary>
        protected virtual void _onDiscard()
        {

        }
    }
}