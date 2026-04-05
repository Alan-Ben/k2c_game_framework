using System;
using ALPackage;

namespace Hotfix
{
    /// <summary>
    /// 一个状态通用的基类
    /// </summary>
    /// <remarks>
    /// 外部实现的时候不要直接继承这个类，继承<see cref="_AHotfixSimpleState{T}"/>系列的类
    /// </remarks>
    public abstract class _AHotfixSimpleStateBase<T> where T : Enum
    {
        // enter 的序列号，序列号还是一样的说明还处于同一次 enter 之中
        private int _m_enterSerialize;
        /// <summary>
        /// enter 的序列号，序列号还是一样的说明还处于同一次 enter 之中
        /// </summary>
        public int enterSerialize { get { return _m_enterSerialize; } }
        /// <summary>
        /// 这个状态是什么状态
        /// </summary>
        public abstract T state { get; }

        protected _AHotfixSimpleStateBase()
        {
            // 先自增一次序列号，给下一次 enter 使用
            _m_enterSerialize = ALSerializeOpMgr.next();
        }

        internal void exit()
        {
            _onExit();
            // 增加序列号，给下一次 enter 使用
            _m_enterSerialize = ALSerializeOpMgr.next();
        }

        /// <summary>
        /// 当退出这个状态时的处理
        /// </summary>
        protected abstract void _onExit();
        /// <summary>
        /// 判断是否可以进入新状态
        /// </summary>
        public abstract bool canEnterState(T _newState);
    }
    /// <summary>
    /// 一个状态机使用的状态
    /// </summary>
    /// <remarks>
    /// 切换到这个状态不需要参数
    /// </remarks>
    public abstract class _AHotfixSimpleState<T> : _AHotfixSimpleStateBase<T> where T : Enum
    {
        internal void enter()
        {
            _onEnter();
        }

        protected abstract void _onEnter();
    }
    /// <summary>
    /// 一个状态机使用的状态
    /// </summary>
    /// <remarks>
    /// 切换到这个状态需要一个参数
    /// </remarks>
    public abstract class _AHotfixSimpleState<T, PARAM> : _AHotfixSimpleStateBase<T> where T : Enum
    {
        internal void enter(PARAM _param)
        {
            _onEnter(_param);
        }

        protected abstract void _onEnter(PARAM _param);
    }
    /// <summary>
    /// 一个状态机使用的状态
    /// </summary>
    /// <remarks>
    /// 切换到这个状态需要两个参数
    /// </remarks>
    public abstract class _AHotfixSimpleState<T, PARAM_1, PARAM_2> : _AHotfixSimpleStateBase<T> where T : Enum
    {
        internal void enter(PARAM_1 _param1, PARAM_2 _param2)
        {
            _onEnter(_param1, _param2);
        }

        protected abstract void _onEnter(PARAM_1 _param1, PARAM_2 _param2);
    }
    /// <summary>
    /// 一个状态机使用的状态
    /// </summary>
    /// <remarks>
    /// 切换到这个状态需要三个参数
    /// </remarks>
    public abstract class _AHotfixSimpleState<T, PARAM_1, PARAM_2, PARAM_3> : _AHotfixSimpleStateBase<T> where T : Enum
    {
        internal void enter(PARAM_1 _param1, PARAM_2 _param2, PARAM_3 _param3)
        {
            _onEnter(_param1, _param2, _param3);
        }

        protected abstract void _onEnter(PARAM_1 _param1, PARAM_2 _param2, PARAM_3 _param3);
    }
    /// <summary>
    /// 一个状态机使用的状态
    /// </summary>
    /// <remarks>
    /// 切换到这个状态需要四个参数
    /// </remarks>
    public abstract class _AHotfixSimpleState<T, PARAM_1, PARAM_2, PARAM_3, PARAM_4> : _AHotfixSimpleStateBase<T> where T : Enum
    {
        internal void enter(PARAM_1 _param1, PARAM_2 _param2, PARAM_3 _param3, PARAM_4 _param4)
        {
            _onEnter(_param1, _param2, _param3, _param4);
        }

        protected abstract void _onEnter(PARAM_1 _param1, PARAM_2 _param2, PARAM_3 _param3, PARAM_4 _param4);
    }
}
