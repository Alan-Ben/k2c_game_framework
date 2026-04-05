using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// Go资源的缓存池管理器
    /// </summary>
    public class Go_CacheMgr : _ACacheControllerMgr<GameObject, GameObject, Go_Cache>
    {
        //最小缓存数
        private readonly int _m_iMinCacheCount;
        //最大缓存数
        private readonly int _m_iMaxCacheCount;

        public Go_CacheMgr(string _rootFolderName, int _minCacheCount = 1, int _maxCacheCount = 5) : base(_rootFolderName)
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
            NPGGoIndex goIndex = _index as NPGGoIndex;
            if(goIndex == null)
            {
                Debug.LogError(_index + " is not a NPGGoIndex");
                if(_onLoaded != null)
                {
                    _onLoaded(null);
                }
                return;
            }

            GGoResCore.instance.loadObj(_index, (_assetHandle) =>
            {
                if(_assetHandle == null || _assetHandle.loadedInfo == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    if(_onLoaded != null)
                    {
                        _onLoaded(null);
                    }
                    return;
                }
                //获取资源对象
                GameObject assetGo = _assetHandle.loadedInfo.obj;
                if(null == assetGo)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    if(_onLoaded != null)
                    {
                        _onLoaded(null);
                    }
                    return;
                }
                
                if(_onLoaded != null)
                {
                    _onLoaded(assetGo);
                }
            });
        }

        /// <summary>
        /// 创建缓存池
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_asset"></param>
        /// <returns></returns>
        protected override Go_Cache CreateCache(BasicResIndexInfo _index)
        {
            NPGGoIndex goIndex = _index as NPGGoIndex;
            if(goIndex == null)
            {
                Debug.LogError(_index + " is not a NPGGoIndex");
                return null;
            }
            return new Go_Cache(goIndex, _m_RootGo, _m_iMinCacheCount, _m_iMaxCacheCount);
        }
    }
}