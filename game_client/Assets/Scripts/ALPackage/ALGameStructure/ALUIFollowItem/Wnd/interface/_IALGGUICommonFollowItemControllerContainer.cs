using System;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// UI中跟随场景对象的控制对象容器
    /// 此接口放在RootWnd中，以供ItemController做反向调用处理（如资源释放操作等）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface _IALGGUICommonFollowItemControllerContainer
    {
        /// <summary>
        /// 取出一个对象，并将对象的创建结果调用到回调中
        /// </summary>
        /// <param name="_itemDelegate"></param>
        void _popItem(_AALGGUICommonFollowItemController _itemController);
        /// <summary>
        /// 释放跟随UI对象
        /// </summary>
        void _releaseItem(_AALGGUICommonFollowItemController _itemController);
    }
}