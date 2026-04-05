
using UnityEngine;

using System;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 枚举操作处理器
    /// </summary>
    public class EnumOperationDealer<T> where T : Enum
    {
        // 存放对应枚举对应的操作方法
        [NotNull] private readonly Dictionary<T, _AEnumOperation> _m_dOperations;
        
        public EnumOperationDealer()
        {
            _m_dOperations = new Dictionary<T, _AEnumOperation>();
        }

        /// <summary>
        /// 设置某个枚举的操作方法
        /// </summary>
        public void setOperation(T _enumType, [NotNull] _AEnumOperation _operation)
        {
            _m_dOperations[_enumType] = _operation;
        }
        /// <summary>
        /// 清空所有设置
        /// </summary>
        public void clear()
        {
            _m_dOperations.Clear();
        }
        /// <summary>
        /// 执行某个枚举的操作
        /// </summary>
        public void doOperation(T _enumType, object _params)
        {
            if (_m_dOperations.TryGetValue(_enumType, out _AEnumOperation operation))
            {
                operation.deal(_params);
            }
        }
    }
}