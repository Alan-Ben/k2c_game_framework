using System;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 跟随UI对象的子窗口接口类
    /// 使用接口类定义，是为了进行实际数据集合的管理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface _IALGGUIWndCommonFollowItem
    {
        /// <summary>
        /// 资源数据对象
        /// </summary>
        _AALBasicLoadResIndexInfo followItemIndex { get; }

        /// <summary>
        /// 根据跟随主实例的中心UI位置设置对应父节点的UI位置
        /// 如果子类有特殊需求，可以进行重载处理
        /// </summary>
        /// <param name="_instanceUICenterPos"></param>
        void setUIPos(Vector3 _instanceUICenterPos);
    }
}