using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 用于在其他地方判断是否有适用了Layer层级控制对象mono的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface _IALUILayerBasicNodeMonoInterface
    {
        /// <summary>
        /// 获取在Mono中的Transform对象
        /// </summary>
        Transform trans { get; }
        /// <summary>
        /// 检测函数
        /// </summary>
        void check();
    }
}
