namespace GOE
{
    /// <summary>
    /// <see cref="EditOperationMgr"/>> 的操作
    /// </summary>
    /// <remarks>
    /// 内部的 internal 成员不要使用，否则会出现难以预测的问题
    /// </remarks>
    public abstract class _AEditOperation
    {
        // 这个操作的上一个操作
        private _AEditOperation _m_lastOperation;
        // 这个操作的下一个操作
        private _AEditOperation _m_nextOperation;
        // 这个操作的索引
        private int _m_operationIndex;
        
        protected _AEditOperation()
        {
        }
        
        internal _AEditOperation lastOperation { get { return _m_lastOperation;} set { _m_lastOperation = value; } }
        internal _AEditOperation nextOperation { get { return _m_nextOperation;} set { _m_nextOperation = value; } }
        internal int operationIndex { get { return _m_operationIndex; } set { _m_operationIndex = value; } }
        
        public abstract void deal();
        public abstract void redo();
        public abstract void undo();
    }
}