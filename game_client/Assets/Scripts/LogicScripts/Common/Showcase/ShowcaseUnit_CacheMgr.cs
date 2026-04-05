using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// Showcase Unit资源的缓存池管理器
    /// </summary>
    public class ShowcaseUnit_CacheMgr : _ACacheControllerMgr<GameObject, GameObject, ShowcaseUnit_Cache>
    {
        //最小缓存数
        private readonly int _m_iMinCacheCount;
        //最大缓存数
        private readonly int _m_iMaxCacheCount;
        
        public ShowcaseUnit_CacheMgr(string _rootFolderName, int _minCacheCount = 1, int _maxCacheCount = 3) : base(_rootFolderName)
        {
            this._m_iMinCacheCount = _minCacheCount;
            this._m_iMaxCacheCount = _maxCacheCount;
        }

        /// <summary>
        /// 加载Asset
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_onLoaded"></param>
        protected override void LoadResource(BasicResIndexInfo _index, Action<GameObject> _onLoaded)
        {
            ALAssetLoader<GameObject> assetLoader = new ALAssetLoader<GameObject>(GameResCore.instance, _index.assetPath, _index.objName);
            assetLoader.loadAsset((itemAsset) =>
            {
                if (null != _onLoaded)
                    _onLoaded(itemAsset);
            });
        }

        /// <summary>
        /// 创建缓存池
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_asset"></param>
        /// <returns></returns>
        protected override ShowcaseUnit_Cache CreateCache(BasicResIndexInfo _index)
        {
            return new ShowcaseUnit_Cache(_index, _m_RootGo, _m_iMinCacheCount, _m_iMaxCacheCount);
        }
    }
}