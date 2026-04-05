using System;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 可以作为FollowInstance容器的接口对象
    /// 可以通过接口注册Instance对象，或者通过接口注册Instance的子对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface _IALCommonFollowInstanceContainer
    {
        /// <summary>
        /// 注册一个实例对象
        /// 只有先注册实例对象才有可能可以向对象注册子控制对象
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_instanceController"></param>
        void regInstance(_AALCommonFollowInstance _instance);

        /// <summary>
        /// 从本管理对象中移除一个实例对象
        /// </summary>
        /// <param name="_instanceId"></param>
        void removeInstance(_AALCommonFollowInstance _instance);

        /// <summary>
        /// 为对应的实例对象创建一个跟随窗口，并通过控制对象进行控制
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_itemController"></param>
        void addController(_AALCommonFollowInstance _instance, _AALGGUICommonFollowItemController _itemController);
    }
}