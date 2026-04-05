using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 礼包组标题对象池
    /// </summary>
    public class GCashGiftPackGroupTitleCacheMgr
    {
        private static GCashGiftPackGroupTitleCacheMgr _g_instance;
        [NotNull]
        public static GCashGiftPackGroupTitleCacheMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GCashGiftPackGroupTitleCacheMgr();
                return _g_instance;
            }
        }


        //最小缓存数量
        private const int _m_iMinCacheCount = 1;
        //最大缓存数量
        private const int _m_iMaxCacheCount = 10;
        //不同资源缓存池
        [NotNull] private Dictionary<long, GCashGiftPackGroupTitleCache> _m_dCacheDic;
        //根节点
        [NotNull] private readonly GameObject _m_cacheRoot;

        public GCashGiftPackGroupTitleCacheMgr()
        {
            _m_cacheRoot = new GameObject("gift_pack_group_title_cache_root");
            _m_cacheRoot.SetActive(false);
            _m_cacheRoot.transform.localPosition = Vector3.up * -10000;
            Object.DontDestroyOnLoad(_m_cacheRoot);
            _m_dCacheDic = new Dictionary<long, GCashGiftPackGroupTitleCache>();
        }

        /// <summary>
        /// 获取item
        /// </summary>
        /// <param name="_uiResId"></param>
        /// <param name="_itemParent"></param>
        /// <param name="_onPopAction"></param>
        public void popItem(long _uiResId, Transform _itemParent, Action<GGUIWndSubCashGiftPackGroupTitle>  _onPopAction)
        {
            if (_onPopAction == null)
                return;

            if (_m_cacheRoot == null || _uiResId <= 0 || _itemParent == null)
            {
                _onPopAction?.Invoke(null);
                return;
            }

            //路径
            string assetPath = UIResPathAssistant.getAssetPath(_uiResId);
            string objectName = UIResPathAssistant.getObjName(_uiResId);

            // 临时的cache
            GCashGiftPackGroupTitleCache cache = null;
            // Cache加载完后的处理
            void onCacheLoaded()
            {
                if (cache == null)
                {
                    ALLog.Error($"prefab路径为{assetPath} {objectName}的资源，对应的cache为null");
                    _onPopAction.Invoke(null);
                    return;
                }

                GGUIWndSubCashGiftPackGroupTitle wnd = cache.popItem();
                if (wnd == null || wnd.wnd == null)
                {
                    ALLog.Error($"prefab路径为{assetPath} {objectName}的资源，对应的cache popItem出现问题了，pop出的item为null!");
                    _onPopAction.Invoke(null);
                    return;
                }

                if (wnd.wnd.transform != null)
                {
                    wnd.wnd.transform.SetParent(_itemParent, false);
                    wnd.wnd.transform.localPosition = Vector3.zero;
                }
                if(wnd.rectTransform != null)
                    wnd.rectTransform.anchoredPosition = Vector2.zero;

                // 一切都结束了之后调用回调
                _onPopAction(wnd);
            }

            // 尝试获取对应的cache
            if (_m_dCacheDic.TryGetValue(_uiResId, out cache) && cache != null)
            {
                // 如果cache已经有了，就在cache加载完后调用加载完后的处理
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
            else
            {
                // 如果没有获取cache，就构建一个新的
                cache = new GCashGiftPackGroupTitleCache(_uiResId, _m_cacheRoot.transform, _m_iMinCacheCount, _m_iMaxCacheCount);
                // 添加到字典中
                _m_dCacheDic[_uiResId] = cache;

                // 开始加载，并注册加载完成后的回调
                cache.load(onCacheLoaded);
            }
        }

        /// <summary>
        /// 回收item
        /// </summary>
        /// <param name="_uiResId"></param>
        /// <param name="_item"></param>
        public void pushBackCacheItem(long _uiResId, GGUIWndSubCashGiftPackGroupTitle _item)
        {
            // 如果item为null或者索引msgInfo为null，都认为无效
            if (_item == null)
                return;

            // 尝试找到对应的缓存池
            if (_m_dCacheDic.TryGetValue(_uiResId, out GCashGiftPackGroupTitleCache cache) && cache != null)
            {
                // 放回对应的缓存池
                cache.pushBackCacheItem(_item);
            }
            else
            {
                // 如果没有找到缓存池，弹出错误提示
                ALLog.Error($"GCashGiftPackGroupTitleCacheMgr 出现了未知的异常，Cache回收资源时，未找到对应缓存池");
                ALUnityCommon.releaseGameObj(_item.wnd);
            }
        }

        /// <summary>
        /// 销毁item
        /// </summary>
        public void discard()
        {
            foreach (GCashGiftPackGroupTitleCache cache in _m_dCacheDic.Values)
            {
                cache?.forceDiscard();
            }
            _m_dCacheDic.Clear();
        }
    }
}