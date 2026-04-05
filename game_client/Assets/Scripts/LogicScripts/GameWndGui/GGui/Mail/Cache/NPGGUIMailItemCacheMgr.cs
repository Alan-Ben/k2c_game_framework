using ALPackage;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace GOE
{
    public class NPGGUIMailItemCacheMgr
    {
        private static NPGGUIMailItemCacheMgr _g_instance = new NPGGUIMailItemCacheMgr();
        public static NPGGUIMailItemCacheMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIMailItemCacheMgr();
                return _g_instance;
            }
        }

        //对象缓存集合   每一种NPCommonAssetPathInfo 单位对应一个缓存池
        private Dictionary<long, NPGGUIMailItemCache_Normal> _m_dModelCacheMap = new Dictionary<long, NPGGUIMailItemCache_Normal>();
        private Dictionary<long, NPGGUIMailItemCache_Hero> _m_dModelCacheMap_Hero = new Dictionary<long, NPGGUIMailItemCache_Hero>();

        protected NPGGUIMailItemCacheMgr()
        {
        }

        /// <summary>
        /// 获得邮件列表预制体item
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <param name="_onLoaded"></param>
        public void popItem(long _uiPathId, Action<GGUISubWndMailItemPrefab_Normal> _onLoaded)
        {
            NPGGUIMailItemCache_Normal cacheNormal = null;
            //模型每个单位有一个缓存池
            if (_m_dModelCacheMap.TryGetValue(_uiPathId, out cacheNormal))
            {       
                //开始加载
                cacheNormal.popItem(_onLoaded);
            }
            else
            {
                //构建新对象
                cacheNormal = new NPGGUIMailItemCache_Normal(_uiPathId);
                _m_dModelCacheMap.Add(_uiPathId, cacheNormal);

                //初始化缓存
                cacheNormal.init(_uiPathId);
                //开始加载
                cacheNormal.popItem(_onLoaded);
            }
        }


        /// <summary>
        /// 回收缓存窗口
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_go"></param>
        public void pushBackCacheItem(long _uiPathId, GGUISubWndMailItemPrefab_Normal _go)
        {
            if (null == _go)
                return;

            NPGGUIMailItemCache_Normal cacheNormal = null;
            if (_m_dModelCacheMap.TryGetValue(_uiPathId, out cacheNormal))
                cacheNormal.pushBackCacheItem(_go);
            else
                _go.discard();

        }
        
        /// <summary>
        /// 获得邮件列表预制体item
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <param name="_onLoaded"></param>
        public void popItem_Hero(long _uiPathId, Action<GGUISubWndMailItemPrefab_Hero> _onLoaded)
        {
            NPGGUIMailItemCache_Hero cacheNormal = null;
            //模型每个单位有一个缓存池
            if (_m_dModelCacheMap_Hero.TryGetValue(_uiPathId, out cacheNormal))
            {       
                //开始加载
                cacheNormal.popItem(_onLoaded);
            }
            else
            {
                //构建新对象
                cacheNormal = new NPGGUIMailItemCache_Hero(_uiPathId);
                _m_dModelCacheMap_Hero.Add(_uiPathId, cacheNormal);

                //初始化缓存
                cacheNormal.init(_uiPathId);
                //开始加载
                cacheNormal.popItem(_onLoaded);
            }
        }


        /// <summary>
        /// 回收缓存窗口
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_go"></param>
        public void pushBackCacheItem_Hero(long _uiPathId, GGUISubWndMailItemPrefab_Hero _go)
        {
            if (null == _go)
                return;

            NPGGUIMailItemCache_Hero cacheNormal = null;
            if (_m_dModelCacheMap_Hero.TryGetValue(_uiPathId, out cacheNormal))
                cacheNormal.pushBackCacheItem(_go);
            else
                _go.discard();

        }
        
        /// <summary>
        /// 清理所有缓存
        /// </summary>
        public void discard()
        {
            foreach (NPGGUIMailItemCache_Normal item in _m_dModelCacheMap.Values)
            {
                item.discard();
            }
            _m_dModelCacheMap.Clear();
            foreach (NPGGUIMailItemCache_Hero item in _m_dModelCacheMap_Hero.Values)
            {
                item.discard();
            }
            _m_dModelCacheMap_Hero.Clear();
        }
    }
}
