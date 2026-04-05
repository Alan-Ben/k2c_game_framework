using System;

namespace GOE
{
    /// <summary>
    /// 一个值连续改变的操作
    /// </summary>
    /// <remarks>
    /// 如果执行了新的操作，这个操作就会被中断
    /// </remarks>
    public abstract class _AValueChgEditOperation<T> : _AEditOperation
    {
        // 这个操作所属的操作管理器
        private readonly EditOperationMgr _m_operationMgr;
        // 当这个操作中止了
        private readonly Action _m_onOperationAbort;
        // 是否正在处理
        private bool _m_isDealing;

        /// <param name="_operationMgr">这个参数务必传入这个操作所属的管理器</param>
        /// <param name="_operationAbort">这个参数会在操作被中断时调用</param>
        protected _AValueChgEditOperation(EditOperationMgr _operationMgr, Action _operationAbort = null)
        {
            _m_operationMgr = _operationMgr;
            _m_onOperationAbort = _operationAbort;
            
            _m_isDealing = false;
        }

        /// <summary>
        /// 获取这个操作的 handle
        /// </summary>
        public Handle getHandle()
        {
            return new Handle()
            {
                setValue = _setValue,
                setDealComplete = _complete
            };
        }

        public sealed override void deal()
        {
            // 开始处理，如果已经开始处理了，或是管理器不存在，就返回
            if (_m_isDealing || _m_operationMgr == null)
                return;

            // 标记为开始处理
            _m_isDealing = true;
            // 调用开始处理
            _beginDeal();
            // 监听管理器的操作发生变化的事件，当操作发生变化，直接调用处理完成
            _m_operationMgr.onOperationChg += _onOperationChgComplete;
        }
        public sealed override void redo()
        {
            // redo 没有特殊的处理，和上面保持统一
            _onRedo();
        }
        public sealed override void undo()
        {
            // 调用 undo 时先尝试调用一次完成处理
            _complete();
            // 再调用 undo 的处理
            _onUndo();
        }

        /// <summary>
        /// 开始处理
        /// </summary>
        protected abstract void _beginDeal();
        /// <summary>
        /// 处理改变值
        /// </summary>
        protected abstract void _dealSetValue(T _value);
        /// <summary>
        /// 处理结束
        /// </summary>
        protected abstract void _endDeal();
        /// <summary>
        /// 重做时的处理
        /// </summary>
        protected abstract void _onRedo();
        /// <summary>
        /// 撤销时的处理
        /// </summary>
        protected abstract void _onUndo();

        private void _complete()
        {
            // 如果没在处理则返回
            if (!_m_isDealing)
                return;

            // 标记为停止处理了
            _m_isDealing = false;
            // 管理器移除事件，这里不需要监听了
            _m_operationMgr.onOperationChg -= _onOperationChgComplete;
            // 调用完成处理
            _endDeal();
            _m_onOperationAbort?.Invoke();
        }
        private void _setValue(T _value)
        {
            // 如果没有正在处理就不响应
            if (!_m_isDealing)
                return;
            
            _dealSetValue(_value);
        }
        private void _onOperationChgComplete()
        {
            if (_m_operationMgr.lastOperation == this)
                return;
            
            _complete();
        }

        public struct Handle
        {
            public Action<T> setValue;
            public Action setDealComplete;
        }
    }
}