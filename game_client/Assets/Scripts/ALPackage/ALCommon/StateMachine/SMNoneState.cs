using System;

namespace ALPackage
{
    /// <summary>
    /// 一个空状态，什么都不做
    /// </summary>
    public class SMNoneState<T> : _ATALStateBase<T> where T : Enum
    {
        public override T state { get { return default; } }

        public override bool canEnterState(_ATALStateBase<T> _newState)
        {
            return true;
        }
        /// <summary>
        /// 重置数据的处理函数
        /// </summary>
        public override void resetData()
        {
        }

        protected override void _onEnter()
        {
        }
        protected override void _onExit()
        {
        }
        protected override void _onTick(float _deltaTime)
        {
        }
    }
}