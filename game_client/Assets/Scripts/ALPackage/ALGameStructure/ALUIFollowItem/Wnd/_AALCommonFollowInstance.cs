using System;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 框架中，Item跟随的主体对象的接口定义类
    /// 每个跟随的主体对象通过实现本接口来实现对内部窗口的统一控制和处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AALCommonFollowInstance
    {
        //本对象注册的父容器对象
        private _IALCommonFollowInstanceContainer _m_ficFollowInstanceContainer;
        //在本对象注册到父节点之后会设置的实例Id
        private long _m_lFollowInstanceSerialize;

        protected _AALCommonFollowInstance()
        {
            _m_ficFollowInstanceContainer = null;
            _m_lFollowInstanceSerialize = 0;
        }

        protected internal _IALCommonFollowInstanceContainer followInstanceContainer { get { return _m_ficFollowInstanceContainer; } }
        protected internal long followInstanceSerialize { get { return _m_lFollowInstanceSerialize; } }

        /// <summary>
        /// 设置本对象所处的容器
        /// </summary>
        protected internal bool _setInstanceContainer(_IALCommonFollowInstanceContainer _container, long _serialize)
        {
            if(null != _m_ficFollowInstanceContainer)
            {
                ALLog.Error($"Set FollowInstance's Container err! when container already exists!");
                return false;
            }

            _m_ficFollowInstanceContainer = _container;
            _m_lFollowInstanceSerialize = _serialize;

            return true;
        }

        /// <summary>
        /// 父对象调用本对象重置信息的处理
        /// </summary>
        /// <param name="_container"></param>
        protected internal bool _resetInstanceContainer(_IALCommonFollowInstanceContainer _container)
        {
            if (_container != _m_ficFollowInstanceContainer)
                return false;

            _m_ficFollowInstanceContainer = null;
            _m_lFollowInstanceSerialize = 0;

            return true;
        }

        /// <summary>
        /// 销毁本对象，调用容器对象销毁本对象
        /// </summary>
        public void discard()
        {
            if (null == _m_ficFollowInstanceContainer)
                return;

            _m_ficFollowInstanceContainer.removeInstance(this);
        }

        /// <summary>
        /// 添加一个子对象，需要判断是否有注册，已经注册的通过注册载体处理
        /// </summary>
        /// <param name="_itemController"></param>
        public void addController(_AALGGUICommonFollowItemController _itemController)
        {
            if (null == _itemController)
                return;

            if (null == _m_ficFollowInstanceContainer)
            {
                ALLog.Error($"Can not reg controller before instance reged!");
                return;
            }

            //调用container的处理
            _m_ficFollowInstanceContainer.addController(this, _itemController);
        }

        /// <summary>
        /// 获取对象在UI中显示的中心位置
        /// 每个子UI对象根据这个位置来刷新各自的位置
        /// </summary>
        public abstract Vector2 itemUICenterPos { get; }

        /// <summary>
        /// 是否在controller队列为0时一直刷新坐标信息
        /// </summary>
        public abstract bool isAlwaysRefreshPos { get; }
    }
}