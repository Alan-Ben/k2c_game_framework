using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 弹幕容器的缓存缓存池管理器
    /// </summary>
    public class GGUIWndCommentContainerItemCacheMgr : _ACommonAssetCacheControllerMgr<GGUIWndCommentContainerItem, GGUIMonoCommentContainerItem, GGUIWndCommentContainerItemCache>
    {
        private static GGUIWndCommentContainerItemCacheMgr _g_instance = new GGUIWndCommentContainerItemCacheMgr();
        [NotNull]public static GGUIWndCommentContainerItemCacheMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndCommentContainerItemCacheMgr();
                return _g_instance;
            }
        }
        
        //最小缓存数
        private readonly int _m_iMinCacheCount;
        //最大缓存数
        private readonly int _m_iMaxCacheCount;
        
        public GGUIWndCommentContainerItemCacheMgr() : base("GGUIMonoCommentContainerItem")
        {
            this._m_iMinCacheCount = 1;
            this._m_iMaxCacheCount = 5;
        }

        /// <summary>
        /// 加载Asset
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_onLoaded"></param>
        protected override void LoadResource(NPCommonAssetPathInfo _index, Action<GGUIMonoCommentContainerItem> _onLoaded)
        {
            ALAssetLoader<GameObject> assetLoader = new ALAssetLoader<GameObject>(GameResCore.instance, _index.asset_path, _index.obj_name);
            assetLoader.loadAsset((itemAsset) =>
            {
                if (null == itemAsset)
                {
                    if (null != _onLoaded)
                        _onLoaded(null);
                    return;
                }
                GGUIMonoCommentContainerItem itemMono = itemAsset.GetComponent<GGUIMonoCommentContainerItem>();
                if (null != _onLoaded)
                    _onLoaded(itemMono);
            });
        }

        /// <summary>
        /// 创建缓存池
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_asset"></param>
        /// <returns></returns>
        protected override GGUIWndCommentContainerItemCache CreateCache(NPCommonAssetPathInfo _index)
        {
            return new GGUIWndCommentContainerItemCache(_m_RootGo.transform, _m_iMinCacheCount, _m_iMaxCacheCount);
        }
    }
}