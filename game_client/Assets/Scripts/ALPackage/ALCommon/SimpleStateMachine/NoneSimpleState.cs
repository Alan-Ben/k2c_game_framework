
using System;

namespace ALPackage
{
    /// <summary>
    /// 一个空状态，什么都不做
    /// </summary>
    public class NoneSimpleState<T> : _ASimpleState<T> where T : Enum
    {
        public override T state { get { return default; } }

        public override bool canEnterState(T _newState)
        {
            return true;
        }

        protected override void _onEnter()
        {
        }
        protected override void _onExit()
        {
        }
    }
}