using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// Item做场景跟随的父窗口通用实现类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ATALGGUIWndCommonFollowRootWnd<T> : _ATALBasicUIWnd<T>, _IALGGUICommonFollowItemControllerContainer, _IALCommonFollowInstanceContainer where T : ALGGUIMonoCommonFollowRootWnd
    {
        private ALCommonEnableTaskController _m_upPosTask;//更新坐标Task

        //根据资源对象进行的集合管理对象
        private Dictionary<long, ALGGUIWndCommonFollowItemGroup> _m_dicFollowItemGroupDic;

        //根据子窗口归属父节点进行管理的管理集合对象
        private Dictionary<long, ALGGUIWndCommonFollowItemInstance> _m_dicFollowInstanceDic;
        private List<ALGGUIWndCommonFollowItemInstance> _m_lInstanceList;

        //默认情况下窗口为世界UI，属于GAME_WORLD_UI层级
        protected _ATALGGUIWndCommonFollowRootWnd() : base(EALUIWndLayer.GAME_WORLD_UI)
        {
            _m_dicFollowItemGroupDic = new Dictionary<long, ALGGUIWndCommonFollowItemGroup>();

            _m_dicFollowInstanceDic = new Dictionary<long, ALGGUIWndCommonFollowItemInstance>();
            _m_lInstanceList = new List<ALGGUIWndCommonFollowItemInstance>();
        }

        /// <summary>
        /// 显示窗口的事件函数
        /// </summary>
        protected override void _onShowWnd()
        {
            _m_upPosTask.setDisable();

            //创建一个每帧任务执行刷新处理
            _m_upPosTask = ALCommonTaskController.CommonEnableLateTickActionAddLaterMonoTask(_updateItemsPos);

            //调用所有子对象的onRootShow处理
            _dealAllInstanceOnRootShow();
        }

        /// <summary>
        /// 隐藏窗口的事件函数
        /// </summary>
        protected override void _onHideWnd()
        {
            _m_upPosTask.setDisable();

            //调用事件函数，处理所有子对象的父节点隐藏处理
            _dealAllInstanceOnRootHide();
        }

        /// <summary>
        /// 调用所有子窗口触发，父节点隐藏处理
        /// </summary>
        private void _dealAllInstanceOnRootShow()
        {
            if (null == _m_lInstanceList)
                return;

            ALGGUIWndCommonFollowItemInstance tmpInstance = null;
            for(int i = 0; i < _m_lInstanceList.Count; i++)
            {
                tmpInstance = _m_lInstanceList[i];
                if (null == tmpInstance)
                    continue;

                tmpInstance.onFollowRootShow();
            }
        }

        /// <summary>
        /// 调用所有子窗口触发，父节点隐藏处理
        /// </summary>
        private void _dealAllInstanceOnRootHide()
        {
            if(null == _m_lInstanceList)
                return;

            ALGGUIWndCommonFollowItemInstance tmpInstance = null;
            for (int i = 0; i < _m_lInstanceList.Count; i++)
            {
                tmpInstance = _m_lInstanceList[i];
                if (null == tmpInstance)
                    continue;

                tmpInstance.onFollowRootHide();
            }
        }

        /// <summary>
        /// 重置窗口数据的事件函数
        /// </summary>
        protected override void _onReset()
        {
        }

        /// <summary>
        /// 释放资源时触发的事件
        /// </summary>
        protected override void _onDiscard()
        {
            _m_upPosTask.setDisable();

            //释放所有Instance对象
            ALGGUIWndCommonFollowItemInstance instance = null;
            for (int i = 0; i < _m_lInstanceList.Count; i++)
            {
                instance = _m_lInstanceList[i];
                if (null == instance)
                    continue;

                //逐个释放
                instance.discard();
            }
            _m_lInstanceList.Clear();
            _m_dicFollowInstanceDic.Clear();

            //释放所有Group数据对象
            foreach (ALGGUIWndCommonFollowItemGroup group in _m_dicFollowItemGroupDic.Values)
            {
                if (null == group)
                    continue;

                //逐个释放
                group.discard();
            }
            _m_dicFollowItemGroupDic.Clear();
        }

        /// <summary>
        /// 窗口初始化完成调用的函数
        /// </summary>
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
        }

        /// <summary>
        /// 注册一个实例对象
        /// 只有先注册实例对象才有可能可以向对象注册子控制对象
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_instanceController"></param>
        public void regInstance(_AALCommonFollowInstance _instance)
        {
            if (null == _instance)
                return;

            //获取新序列号
            long newSerialize = ALSerializeOpMgr.next();
            //尝试设置信息，成功才能放入后续处理
            if(!_instance._setInstanceContainer(this, newSerialize))
                return;

            //注册数据对象
            ALGGUIWndCommonFollowItemInstance instance = new ALGGUIWndCommonFollowItemInstance(_instance);
            _m_dicFollowInstanceDic.Add(instance.instanceSerialize, instance);
            _m_lInstanceList.Add(instance);
        }

        /// <summary>
        /// 从本管理对象中移除一个实例对象
        /// </summary>
        /// <param name="_instanceId"></param>
        public void removeInstance(_AALCommonFollowInstance _instance)
        {
            if (null == _instance)
                return;

            //从数据集中尝试移除对象
            ALGGUIWndCommonFollowItemInstance removeInstance = null;
            if (!_m_dicFollowInstanceDic.TryGetValue(_instance.followInstanceSerialize, out removeInstance))
            {
                return;
            }

            //尝试重置数据
            if (!_instance._resetInstanceContainer(this))
                return;

            //移除数据
            _m_dicFollowInstanceDic.Remove(_instance.followInstanceSerialize);

            //释放数据对象，释放函数会将所有controller直接释放
            removeInstance.discard();
        }

        /// <summary>
        /// 为对应的实例对象创建一个跟随窗口，并通过控制对象进行控制
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_itemController"></param>
        public void addController(_AALCommonFollowInstance _instance, _AALGGUICommonFollowItemController _itemController)
        {
            if (null == _instance || null == _itemController)
                return;

            addController(_instance.followInstanceSerialize, _itemController);
        }
        public void addController(long _instanceSerialize, _AALGGUICommonFollowItemController _itemController)
        {
            if (null == _itemController)
                return;

            ALGGUIWndCommonFollowItemInstance itemInstance = null;
            //从数据集中获取对应的实例对象
            if (!_m_dicFollowInstanceDic.TryGetValue(_instanceSerialize, out itemInstance))
            {
                ALLog.Error($"Can not find reged instance for instanceId:{_instanceSerialize}");
                return;
            }

            //调用controller的初始化处理
            if (!_itemController._initItem(this, _instanceSerialize))
                return;

            //向实例注册器中注册对象
            itemInstance.addController(_itemController);
        }

        /// <summary>
        /// 取出一个对象，并将对象的创建结果调用到回调中
        /// </summary>
        /// <param name="_itemDelegate"></param>
        public void _popItem(_AALGGUICommonFollowItemController _itemController)
        {
            if (null == _itemController)
                return ;

            //获取对应的Group对象，并进行处理
            ALGGUIWndCommonFollowItemGroup groupObj = _getFollowItemGroup(_itemController.followItemIndex);
            if (null == groupObj)
            {
                ALLog.Error($"get follow item group fail! followindex:{_itemController.followItemIndex}");
                return ;
            }

            //执行取出缓存处理
            groupObj.popItem(_itemController);
        }
        /// <summary>
        /// 释放跟随UI对象
        /// </summary>
        public void _releaseItem(_AALGGUICommonFollowItemController _itemController)
        {
            if (null == _itemController)
                return;

            //先从实例对象中移除
            ALGGUIWndCommonFollowItemInstance itemInstance = null;
            //从数据集中获取对应的实例对象
            if (_m_dicFollowInstanceDic.TryGetValue(_itemController._followInstanceSerialize, out itemInstance))
            {
                //从实例管理器中删除对象
                itemInstance.removeController(_itemController);
            }

            //执行释放处理
            //获取对应的Group对象，并进行处理
            ALGGUIWndCommonFollowItemGroup groupObj = _getFollowItemGroup(_itemController.followItemIndex);
            if (null != groupObj)
            {
                //将资源放回
                groupObj.pushbackItem(_itemController);
            }
        }
        
        /// <summary>
        /// 刷新操作按钮的坐标
        /// </summary>
        private void _updateItemsPos()
        {
            if (wnd == null || !isShow)
                return;

            //刷新所有对象信息
            ALGGUIWndCommonFollowItemInstance instance = null;
            for(int i = 0; i < _m_lInstanceList.Count; i++)
            {
                instance = _m_lInstanceList[i];
                if (null == instance)
                    continue;

                instance.refreshAllItemPos();
            }
        }

        /// <summary>
        /// 根据资源索引，获取对应的Group管理对象
        /// </summary>
        /// <param name="_resIndex"></param>
        protected ALGGUIWndCommonFollowItemGroup _getFollowItemGroup(_AALBasicLoadResIndexInfo _resIndex)
        {
            if (null == _resIndex || null == wnd || null == _m_dicFollowItemGroupDic)
                return null;

            long mergeId = ALCommon.mergeInt(_resIndex.mainId, _resIndex.subId);
            ALGGUIWndCommonFollowItemGroup groupObj = null;
            //判断是否存在
            if(!_m_dicFollowItemGroupDic.TryGetValue(mergeId, out groupObj))
            {
                //获取失败则创建新对象
                groupObj = new ALGGUIWndCommonFollowItemGroup(wnd.itemRootParent, _resIndex, _followItemCacheMgr);
                //放入集合
                _m_dicFollowItemGroupDic.Add(mergeId, groupObj);
            }

            return groupObj;
        }

        /// <summary>
        /// 返回本窗口用于缓存跟随UI的缓存管理器对象
        /// 不同的rootwnd可以返回同一个cache对象，这样方便全项目共用cache
        /// </summary>
        protected abstract ALGGUIWndCommonFollowItemCacheMgr _followItemCacheMgr { get; }
    }
}