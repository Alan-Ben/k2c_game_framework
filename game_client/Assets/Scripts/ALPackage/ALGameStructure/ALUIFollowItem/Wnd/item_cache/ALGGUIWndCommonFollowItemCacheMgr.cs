using UnityEngine;
using System;

using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 全局中用于跟随场景坐标进行跳转的Item的缓存管理对象
    /// 此对象作为单例，便于全局统一管理
    /// </summary>
    public class ALGGUIWndCommonFollowItemCacheMgr
    {
        //加载资源
        private _AALResourceCore _m_rcResCore;

        //所有缓存创建的UI所在的根节点对象
        private GameObject _m_gUIRootItem;
        private Transform _m_tUIRootItem;
        //每个对应索引的Item所在的缓存管理器集合
        private Dictionary<long, ALGGUIWndCommonFollowItemCacheObj> _m_dicItemCacheDic;

        protected ALGGUIWndCommonFollowItemCacheMgr(_AALResourceCore _resCore)
        {
            _m_rcResCore = _resCore;

            _m_gUIRootItem = new GameObject("followItem_root");
            _m_tUIRootItem = _m_gUIRootItem.transform;
            //设置加载不删除
            GameObject.DontDestroyOnLoad(_m_gUIRootItem);

            _m_dicItemCacheDic = new Dictionary<long, ALGGUIWndCommonFollowItemCacheObj>();
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void discard()
        {
            //逐个释放cache资源
            foreach (ALGGUIWndCommonFollowItemCacheObj cacheObj in _m_dicItemCacheDic.Values)
            {
                if (null == cacheObj)
                    continue;

                cacheObj.discard();
            }
            _m_dicItemCacheDic.Clear();
        }

        /// <summary>
        /// 根据资源索引获取对应的缓存Cache对象
        /// </summary>
        /// <param name="_resIndex"></param>
        /// <returns></returns>
        public ALGGUIWndCommonFollowItemCacheObj getCacheObj(_AALBasicLoadResIndexInfo _resIndex)
        {
            if (null == _resIndex)
                return null;

            //混合索引值
            long mergeId = ALCommon.mergeInt(_resIndex.mainId, _resIndex.subId);
            if (_m_dicItemCacheDic.ContainsKey(mergeId))
            {
                return _m_dicItemCacheDic[mergeId];
            }

            //创建新对象，并开始初始化
            ALGGUIWndCommonFollowItemCacheObj cacheObj = new ALGGUIWndCommonFollowItemCacheObj(_resIndex, _m_tUIRootItem, _m_rcResCore);
            //放入数据集
            _m_dicItemCacheDic.Add(mergeId, cacheObj);
            //开始初始化
            cacheObj.loadItemAndInit();

            //返回
            return cacheObj;
        }

        /// <summary>
        /// 从缓存中取出一个缓存对象
        /// </summary>
        /// <param name="_resIndex"></param>
        /// <param name="_popDelegate"></param>
        public void popItem(_AALBasicLoadResIndexInfo _resIndex, Action<ALGGUIMonoCommonFollowItem> _popDelegate)
        {
            ALGGUIWndCommonFollowItemCacheObj cacheObj = getCacheObj(_resIndex);
            if (null == cacheObj)
            {
                if (null != _popDelegate)
                    _popDelegate(null);

                return;
            }

            //调用pop处理
            cacheObj.popItem(_popDelegate);
        }

        /// <summary>
        /// 将item放回到缓存中
        /// </summary>
        /// <param name="_item"></param>
        public void pushbackCache(ALGGUIMonoCommonFollowItem _item)
        {
            if (null == _item)
                return;

            ///如果数据索引有问题需要报错
            if (null == _item._resIndex)
            {
                ALLog.Error($"follow item res index is empty!");
                return;
            }

            long mergeId = ALCommon.mergeInt(_item._resIndex.mainId, _item._resIndex.subId);

            //这边由于item中的资源索引都是cache设置的，所以可以假设有index，那么资源的cache对象就一定存在
            //所以这边就不做是否存在的判断了
            //获取缓存对象
            ALGGUIWndCommonFollowItemCacheObj cacheObj = _m_dicItemCacheDic[mergeId];
            //放回缓存
            cacheObj.pushBackCacheItem(_item);
        }
    }
}

