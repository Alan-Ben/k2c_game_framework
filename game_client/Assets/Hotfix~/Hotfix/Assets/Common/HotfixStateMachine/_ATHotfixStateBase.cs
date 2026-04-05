using System;
using ALPackage;

namespace Hotfix
{
    /// <summary>
    /// 仿造主工程_ATALStateBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ATHotfixStateBase<T> where T : Enum
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
        
        protected _ATHotfixStateBase()
        {
            // 先自增一次序列号，给下一次 enter 使用
            _m_enterSerialize = ALSerializeOpMgr.next();
        }

        internal void enter()
        {
            //每次进入重新获取序列号
            _m_enterSerialize = ALSerializeOpMgr.next();

            _onEnter();
        }
        internal void exit()
        {
            _onExit();
            // 增加序列号，给下一次 enter 使用
            _m_enterSerialize = ALSerializeOpMgr.next();
        }

        internal void tick(float _deltaTime)
        {
            _onTick(_deltaTime);
        }
        /// <summary>
        /// 判断是否可以进入本状态，可以用于进行一些特殊行为判断的时候将代码放在状态内，而不需要放在外部逻辑
        /// </summary>
        public virtual bool canEnterSelf(_ATHotfixStateBase<T> _curState)
        {
            return true;
        }

        /// <summary>
        /// 进入的事件函数
        /// </summary>
        protected abstract void _onEnter();
        /// <summary>
        /// 当退出这个状态时的处理
        /// </summary>
        protected abstract void _onExit();
        /// <summary>
        /// 更新任务
        /// </summary>
        protected abstract void _onTick(float _deltaTime);
        /// <summary>
        /// 判断是否可以进入新状态
        /// </summary>
        public abstract bool canEnterState(_ATHotfixStateBase<T> _newState);
        /// <summary>
        /// 重置数据的处理函数
        /// </summary>
        public abstract void resetData();
    }
}