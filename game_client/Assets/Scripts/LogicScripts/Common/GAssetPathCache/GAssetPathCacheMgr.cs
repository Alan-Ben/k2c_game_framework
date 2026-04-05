using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 简单的用来加载 NPCommonAssetPathInfo 的管理器
    /// </summary>
    public class GAssetPathCacheMgr
    {
        private const int _m_iMinCacheCount = 1;
        private const int _m_iMaxCacheCount = 30;

        private static GAssetPathCacheMgr _g_instance;
        [NotNull] public static GAssetPathCacheMgr instance { get { return _g_instance ??= new GAssetPathCacheMgr(); } }

        [NotNull] private readonly GameObject _m_cacheRoot;//根节点
        [NotNull] private readonly Dictionary<NPCommonAssetPathInfo, NPSpaceGameObjectCache> _m_dCacheDic;//缓存池字典(NPCommonAssetPathInfo重写了Equals和GetHashCode所以可以直接作为字典的key)

        private GAssetPathCacheMgr()
        {
            _m_cacheRoot = new GameObject("go_of_NPCommonAssetPathInfo");
            _m_cacheRoot.SetActive(false);
            _m_cacheRoot.transform.localPosition = Vector3.up * -10000;
            Object.DontDestroyOnLoad(_m_cacheRoot);
            _m_dCacheDic = new Dictionary<NPCommonAssetPathInfo, NPSpaceGameObjectCache>();
        }


        public void discard()
        {
            foreach (NPSpaceGameObjectCache cache in _m_dCacheDic.Values)
            {
                cache?.discard();
            }

            _m_dCacheDic.Clear();
        }

        public void popItem(NPCommonAssetPathInfo _assetPathInfo, Action<GameObject> _onPop)
        {
            if (_assetPathInfo == null || _m_cacheRoot == null || !_assetPathInfo.enable)
            {
                _onPop?.Invoke(null);
                return;
            }

            NPSpaceGameObjectCache cache = null;

            //不存在缓存池，加载并添加到字典
            if (!_m_dCacheDic.TryGetValue(_assetPathInfo, out cache))
            {
                cache = new NPSpaceGameObjectCache(_assetPathInfo.asset_path, _assetPathInfo.obj_name, _m_cacheRoot, _m_iMinCacheCount, _m_iMaxCacheCount);
                cache.load();
                _m_dCacheDic[_assetPathInfo] = cache;
            }

            //缓存池加载完成后调用popItem
            cache.regLoadDoneDelegate(() =>
            {
                GameObject go = cache.popItem();
                ALUGUICommon.setGameObjEnable(go, true);
                _onPop?.Invoke(go);
            });
        }

        public void pushbackItem(NPCommonAssetPathInfo _assetPathInfo, GameObject _unitView)
        {
            if(_unitView == null)
                return;

            if (_assetPathInfo == null)
            {
                GameObject.Destroy(_unitView);
                return;
            }

            NPSpaceGameObjectCache cache = null;

            //回收
            if (!_m_dCacheDic.TryGetValue(_assetPathInfo, out cache))
            {
                Debug.LogError($"【GAssetPathCacheMgr.pushbackItem Error】缓存池不存在，NPCommonAssetPathInfo:{_assetPathInfo}");
                GameObject.Destroy(_unitView);
                return;
            }
            else
            {
                if (cache == null)
                {
                    Debug.LogError($"【NPGGoIndexCacheMgr.pushbackItem Error】缓存池为null，，NPCommonAssetPathInfo:{_assetPathInfo}");
                    GameObject.Destroy(_unitView);
                    return;
                }
                else
                {
                    cache.pushBackCacheItem(_unitView);
                }
            }
        }
    }
}