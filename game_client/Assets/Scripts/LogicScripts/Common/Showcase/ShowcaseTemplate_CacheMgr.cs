using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// ShowcaseTemplate资源的缓存池管理器
    /// </summary>
    public class ShowcaseTemplate_CacheMgr : _ACacheControllerMgr<NPShowcaseTemplateMono, NPShowcaseTemplateMono, ShowcaseTemplate_Cache>
    {
        //最小缓存数
        private readonly int _m_iMinCacheCount;
        //最大缓存数
        private readonly int _m_iMaxCacheCount;
        
        public ShowcaseTemplate_CacheMgr(string _rootFolderName, int _minCacheCount = 1, int _maxCacheCount = 5) : base(_rootFolderName)
        {
            this._m_iMinCacheCount = _minCacheCount;
            this._m_iMaxCacheCount = _maxCacheCount;
        }

        /// <summary>
        /// 加载Asset
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_onLoaded"></param>
        protected override void LoadResource(BasicResIndexInfo _index, Action<NPShowcaseTemplateMono> _onLoaded)
        {
            NPGShowcaseIndex goIndex = _index as NPGShowcaseIndex;
            if(goIndex == null)
            {
                Debug.LogError(_index + " is not a NPGShowcaseIndex");
                if(_onLoaded != null)
                {
                    _onLoaded(null);
                }
                return;
            }

            ALAssetLoader<GameObject> assetLoader = new ALAssetLoader<GameObject>(GameResCore.instance, _index.assetPath, _index.objName);
            assetLoader.loadAsset((itemAsset) =>
            {
                NPShowcaseTemplateMono showcaseTemplateMono = null;

                if (null != itemAsset)
                {
                    showcaseTemplateMono = itemAsset.GetComponent<NPShowcaseTemplateMono>();
                }
                else
                {
                    Debug.LogError($"showcase template itemAsset is Null");
                }
                
                if (null != _onLoaded)
                    _onLoaded(showcaseTemplateMono);
            });
        }

        /// <summary>
        /// 创建缓存池
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_asset"></param>
        /// <returns></returns>
        protected override ShowcaseTemplate_Cache CreateCache(BasicResIndexInfo _index)
        {
            NPGShowcaseIndex goIndex = _index as NPGShowcaseIndex;
            if(goIndex == null)
            {
                Debug.LogError(_index + " is not a NPGShowcaseIndex");
                return null;
            }
            return new ShowcaseTemplate_Cache(goIndex, _m_RootGo, _m_iMinCacheCount, _m_iMaxCacheCount);
        }
    }
}