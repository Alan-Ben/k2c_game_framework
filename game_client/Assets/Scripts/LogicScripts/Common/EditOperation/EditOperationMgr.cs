
using System;
// ReSharper disable PossibleNullReferenceException

namespace GOE
{
    /// <summary>
    /// 操作管理器
    /// </summary>
    /// <remarks>
    /// 主要支持 Undo Redo 功能
    /// </remarks>
    public class EditOperationMgr
    {
        // 当前的第一个操作和最后一个操作
        private _AEditOperation _m_firstOperation;
        private _AEditOperation _m_lastOperation;
        // 第0个操作的索引号
        private int _m_zeroOperationIndex;
        // 当前的操作节点，可能是 null
        private _AEditOperation _m_curOperation;
        
        // 储存的操作数量上限，最低 1
        private readonly int _m_maxOperationCount;
        
        /// <summary>
        /// 设置储存的操作数量上限的同时构造这个类
        /// </summary>
        public EditOperationMgr(int _maxOperationCount)
        {
            // 赋值并限制最小数量
            _m_maxOperationCount = _maxOperationCount;
            if (_m_maxOperationCount < 1)
                _m_maxOperationCount = 1;

            _m_zeroOperationIndex = 0;
        }
        
        /// <summary>
        /// 当前操作发生了变化
        /// </summary>
        public event Action onOperationChg;

        /// <summary>
        /// 目前储存的操作总数
        /// </summary>
        public int totalOperationCount
        {
            // 如果不存在最后一个操作，那么一共 0 个操作，如果存在，最后一个操作的索引减去表示 0 个操作的索引就是总操作数
            get { return _m_lastOperation == null ? 0 : _m_lastOperation.operationIndex - _m_zeroOperationIndex; }
        }
        /// <summary>
        /// 是否可以进行 redo 操作
        /// </summary>
        public bool canRedo
        {
            // 如果当前操作点是 null 就尝试使用第一个操作，如果当前操作不是 null，就尝试取出当前操作的下一个操作
            get { return (_m_curOperation == null ? _m_firstOperation : _m_curOperation.nextOperation) != null; }
        }
        /// <summary>
        /// 是否可以进行 undo 操作
        /// </summary>
        public bool canUndo { get { return _m_curOperation != null; } }
        /// <summary>
        /// 最后一个操作
        /// </summary>
        public _AEditOperation lastOperation { get { return _m_lastOperation; } }

        /// <summary>
        /// 重做
        /// </summary>
        public void redo()
        {
            if (!canRedo)
                return;
            
            // 如果当前操作点是 null 就尝试使用第一个操作，如果当前操作不是 null，就尝试取出当前操作的下一个操作
            _m_curOperation = _m_curOperation == null ? _m_firstOperation : _m_curOperation.nextOperation;
            _m_curOperation.redo();
            onOperationChg?.Invoke();
        }
        /// <summary>
        /// 撤销
        /// </summary>
        public void undo()
        {
            if (!canUndo) 
                return;
            
            // 撤销当前的操作，并把当前的操作设置为上一个操作
            _m_curOperation.undo();
            _m_curOperation = _m_curOperation.lastOperation;
            onOperationChg?.Invoke();
        }
        /// <summary>
        /// 清空内部所有数据
        /// </summary>
        public void clearData()
        {
            _m_firstOperation = null;
            _m_lastOperation = null;
            _m_curOperation = null;
            _m_zeroOperationIndex = 0;
            onOperationChg?.Invoke();
        }
        /// <summary>
        /// 执行一个操作
        /// </summary>
        public void dealOperation(_AEditOperation _newOperation)
        {
            if (_newOperation == null)
                return;

            // 如果当前的操作存在，就尝试加入到当前的操作之后，并且后面的操作全部丢弃
            if (_m_curOperation != null)
            {
                // 把新增的操作插入到当前操作之后，同时由于后面的操作都丢弃了，所以这个新增的操作就是最后一个操作
                _newOperation.lastOperation = _m_curOperation;
                _newOperation.nextOperation = null;
                _newOperation.operationIndex = _m_curOperation.operationIndex + 1;
                _m_curOperation.nextOperation = _newOperation;
                _m_lastOperation = _newOperation;
            }
            // 如果当前的操作不存在，就把这个操作当成第一个操作，并且丢弃后面的所有操作
            else
            {
                // 赋值，并且由于后面的操作都被丢弃了，所以这个新增的操作也是最后一个操作
                _newOperation.lastOperation = null;
                _newOperation.nextOperation = null;
                _newOperation.operationIndex = 1;
                _m_lastOperation = _m_firstOperation = _newOperation;
                _m_zeroOperationIndex = 0;
            }

            // 如果目前的操作总数超过了限制的数量，就把第一个操作移除掉
            if (totalOperationCount > _m_maxOperationCount)
            {
                // 旧的 first 的 next 就是新的 first，清空新的 first 的 lastOperation，并把 zeroOperationIndex 设置为新的值
                _m_firstOperation = _m_firstOperation.nextOperation;
                _m_firstOperation.lastOperation = null;
                _m_zeroOperationIndex = _m_firstOperation.operationIndex - 1;
            }
            
            // 新操作设置为当前操作，并且处理
            _m_curOperation = _newOperation;
            _newOperation.deal();
            onOperationChg?.Invoke();
        }
    }
}