using UnityEngine;
using System;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 跟随窗口中，根据Item资源索引进行分类管理的集合管理对象
    /// 一个资源索引放一个集合，是为了方便进行UI性能优化。
    /// 
    /// Group中的Cache对象来源于父节点，因此cache是可以多窗口共用的
    /// </summary>
    public class ALGGUIWndCommonFollowItemGroup
    {
        //窗口的UI根节点对象
        private RectTransform _m_rtRootRT;
        private ALGGUIWndCommonFollowItemCacheMgr _m_cmCacheMgr;

        //本集合自动创建的集合根节点对象
        private GameObject _m_goGroupRoot;
        private RectTransform _m_rtGroupRootRT;

        //资源索引对象及Cache对象
        private long _m_lResId;
        private _AALBasicLoadResIndexInfo _m_iResIndex;
        private ALGGUIWndCommonFollowItemCacheObj _m_cCacheObj;

        //已经通过本对象创建的队列
        private List<_AALGGUICommonFollowItemController> _m_lGroupControllerList;

        protected internal ALGGUIWndCommonFollowItemGroup(RectTransform _rootRT, _AALBasicLoadResIndexInfo _itemIndex, ALGGUIWndCommonFollowItemCacheMgr _cacheMgr)
        {
            //设置根节点
            _m_rtRootRT = _rootRT;
            _m_cmCacheMgr = _cacheMgr;

            _m_iResIndex = _itemIndex;
            _m_lResId = ALCommon.mergeInt(_m_iResIndex.mainId, _m_iResIndex.subId);

            //创建集合根节点
            _m_goGroupRoot = new GameObject(_m_iResIndex.objName);
            _m_rtGroupRootRT = _m_goGroupRoot.AddComponent<RectTransform>();
            //设置父节点
            _m_rtGroupRootRT.SetParent(_m_rtRootRT);

            //设置子节点信息
            _m_rtGroupRootRT.localPosition = Vector3.zero;
            _m_rtGroupRootRT.localScale = Vector3.one;

            //获取缓存对象
            _m_cCacheObj = _m_cmCacheMgr.getCacheObj(_m_iResIndex);

            _m_lGroupControllerList = new List<_AALGGUICommonFollowItemController>();
        }

        public long resId { get { return _m_lResId; } }
        public _AALBasicLoadResIndexInfo itemIndex { get { return _m_iResIndex; } }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void discard()
        {
            //将所有控制对象都进行释放
            if (null != _m_lGroupControllerList)
            {
                //拷贝数据对象，由于controller的释放会从本对象中改动队列，因此这边需要拷贝之后再移除处理
                List<_AALGGUICommonFollowItemController> rmvList = new List<_AALGGUICommonFollowItemController>();
                rmvList.AddRange(_m_lGroupControllerList);

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
                _m_lGroupControllerList.Clear();
            }

            //销毁根节点
            ALUnityCommon.releaseGameObj(_m_goGroupRoot);
            _m_goGroupRoot = null;
            _m_rtGroupRootRT = null;
        }

        /// <summary>
        /// 取出一个实例对象放入回调处理
        /// </summary>
        /// <param name="_itemDelegate"></param>
        public void popItem(_AALGGUICommonFollowItemController _itemController)
        {
            if (null == _itemController)
                return;

            if(null == _m_cCacheObj)
                return;

            //放入队列
            _m_lGroupControllerList.Add(_itemController);
            
            //从cache创建处理
            _m_cCacheObj.popItem(
                (ALGGUIMonoCommonFollowItem _itemMono) =>
                {
                    //检查control是否还有效
                    if (_m_lGroupControllerList.Contains(_itemController))
                    {
                        //设置对象的相关操作
                        _initNewItemMono(_itemMono);
                        //调用回调
                        if(!_itemController._onMonoCreate(_itemMono))
                        {
                            //放入缓存池
                            _m_cCacheObj.pushBackCacheItem(_itemMono);
                        }
                    }
                    //无效了放回缓存
                    else
                    {
                        //放入缓存池
                        _m_cCacheObj.pushBackCacheItem(_itemMono); 
                    }
                });
        }

        /// <summary>
        /// 将对应的资源放回数据集
        /// </summary>
        /// <param name="_itemMono"></param>
        public void pushbackItem(_AALGGUICommonFollowItemController _itemController)
        {
            if (null == _itemController)
                return;

            //检索是否在队列中
            if(!_m_lGroupControllerList.Remove(_itemController))
            {
                //不在队列则直接返回失败
                ALLog.Error($"Pushback Item when item is not create by this group! {_itemController.followItemIndex}");
                return;
            }

            ALGGUIMonoCommonFollowItem itemMono = _itemController.itemMono;
            if(null == itemMono)
                return;
            
            //放入缓存池
            _m_cCacheObj.pushBackCacheItem(itemMono);
            //调用缓存清空处理
            _itemController._onMonoPushback(itemMono);
        }

        /// <summary>
        /// 初始化一个新的Item对象
        /// </summary>
        /// <param name="_itemMono"></param>
        protected void _initNewItemMono(ALGGUIMonoCommonFollowItem _itemMono)
        {
            if (null == _itemMono || null == _itemMono.transform)
                return;

            //设置父节点
            _itemMono.transform.SetParent(_m_rtGroupRootRT);
            //设置比例等基础信息
            _itemMono.transform.localPosition = Vector3.zero;
            _itemMono.transform.localScale = Vector3.one;
        }
    }
}


