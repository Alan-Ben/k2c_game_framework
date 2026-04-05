using UnityEngine;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 跟随窗口中，根据跟随主体创建的对应跟随逻辑对象
    /// 一般刷新位置等处理是跟随主体进行的操作
    /// 每个跟随对象创建一个跟随UI的时候，将向本主体进行注册，并在刷新的时候在主体进行统一处理
    /// </summary>
    public class ALGGUIWndCommonFollowItemInstance
    {
        //构造函数时的数据Id，后续不会因为其他情况而改变
        //当此数据非0的时候，在注册到父节点的时候会进行重复性验证，保证重复Id对象不会重复注册
        private long _m_lInstanceSerialize;

        //本实例管理器挂载的实例对象控制类
        private _AALCommonFollowInstance _m_iItemInstance;

        //本实例对象使用的UI对象队列
        private List<_AALGGUICommonFollowItemController> _m_lInstanceControllerList;

        public ALGGUIWndCommonFollowItemInstance(_AALCommonFollowInstance _instance)
        {
            _m_lInstanceSerialize = _instance.followInstanceSerialize;
            _m_iItemInstance = _instance;

            _m_lInstanceControllerList = new List<_AALGGUICommonFollowItemController>();
        }

        public long instanceSerialize
        {
            get { return _m_lInstanceSerialize; }
        }

        /// <summary>
        /// 初始化本对象的控制信息
        /// </summary>
        /// <param name="_rootWnd"></param>
        public void addController(_AALGGUICommonFollowItemController _controller)
        {
            //数据无效直接不做处理
            if (null == _controller)
                return;

            //添加到队列中
            _m_lInstanceControllerList.Add(_controller);
        }

        /// <summary>
        /// 从本对象管理器中移除某一个对象
        /// </summary>
        /// <param name="_controller"></param>
        /// <returns></returns>
        public bool removeController(_AALGGUICommonFollowItemController _controller)
        {
            if (null == _controller)
                return true;

            return _m_lInstanceControllerList.Remove(_controller);
        }

        /// <summary>
        /// 刷新所有Item的位置信息
        /// </summary>
        public void refreshAllItemPos()
        {
            //队列为空则不获取UI位置，可能避免一些计算
            if (null == _m_iItemInstance || (!_m_iItemInstance.isAlwaysRefreshPos && _m_lInstanceControllerList.Count <= 0))
                return;

            _AALGGUICommonFollowItemController tmpItem = null;

            //获取当前的UI位置
            Vector2 instanceUICenterPos = _m_iItemInstance.itemUICenterPos;

            //刷新所有子对象坐标
            for (int i = 0; i < _m_lInstanceControllerList.Count; i++)
            {
                tmpItem = _m_lInstanceControllerList[i];
                if (null == tmpItem)
                    continue;

                //刷新位置
                tmpItem._refreshItemPos(instanceUICenterPos);
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void discard()
        {
            //将所有控制对象都进行释放
            if (null != _m_lInstanceControllerList)
            {
                //拷贝数据对象，由于controller的释放会从本对象中改动队列，因此这边需要拷贝之后再移除处理
                List<_AALGGUICommonFollowItemController> rmvList = new List<_AALGGUICommonFollowItemController>();
                rmvList.AddRange(_m_lInstanceControllerList);

                //逐个释放
                _AALGGUICommonFollowItemController tmpItem = null;
                for (int i = 0; i < rmvList.Count; i++)
                {
                    tmpItem = rmvList[i];
                    if (null == tmpItem)
                        continue;

                    tmpItem.discard();
                }

                //最后为了保证切断联系，再对队列做一次清空处理
                _m_lInstanceControllerList.Clear();
            }

            _m_iItemInstance = null;
        }

        /// <summary>
        /// 本对象归属的FollowRoot窗口在显示的时候调用的处理本对象所有子控制对象的onRootShow处理
        /// </summary>
        public void onFollowRootShow()
        {
            if (null == _m_lInstanceControllerList)
                return;

            _AALGGUICommonFollowItemController tmpController = null;
            for (int i = 0; i < _m_lInstanceControllerList.Count; i++)
            {
                tmpController = _m_lInstanceControllerList[i];
                if (null == tmpController)
                    continue;

                tmpController.onFollowRootShow();
            }
        }

        /// <summary>
        /// 本对象归属的FollowRoot窗口在显示的时候调用的处理本对象所有子控制对象的onRootHide处理
        /// </summary>
        public void onFollowRootHide()
        {
            if(null == _m_lInstanceControllerList)
                return;

            _AALGGUICommonFollowItemController tmpController = null;
            for (int i = 0; i < _m_lInstanceControllerList.Count; i++)
            {
                tmpController = _m_lInstanceControllerList[i];
                if (null == tmpController)
                    continue;

                tmpController.onFollowRootHide();
            }   
        }
    }
}