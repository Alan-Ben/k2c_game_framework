using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 简单的用来加载 NPGGoIndex 的管理器
    /// </summary>
    public class GGoIndexCacheMgr
    {
        private const int _m_iMinCacheCount = 1;
        private const int _m_iMaxCacheCount = 30;


        private static GGoIndexCacheMgr _g_instance;
        [NotNull] public static GGoIndexCacheMgr instance { get { return _g_instance ??= new GGoIndexCacheMgr(); } }

        [NotNull] private readonly GameObject _m_cacheRoot;//根节点
        [NotNull] private readonly Dictionary<long, NPSpaceGameObjectCache> _m_dCacheDic;//缓存池字典

        private GGoIndexCacheMgr()
        {
            _m_cacheRoot = new GameObject("go_of_NPGGoIndex");
            _m_cacheRoot.SetActive(false);
            _m_cacheRoot.transform.localPosition = Vector3.up * -10000;
            Object.DontDestroyOnLoad(_m_cacheRoot);
            _m_dCacheDic = new Dictionary<long, NPSpaceGameObjectCache>();
        }


        public void discard()
        {
            foreach (NPSpaceGameObjectCache cache in _m_dCacheDic.Values)
            {
                cache?.discard();
            }

            _m_dCacheDic.Clear();
        }

        public void popItem(NPGGoIndex _goIndex, Action<GameObject> _onPop)
        {
            if (_goIndex == null || _m_cacheRoot == null || !_goIndex.isValid())
            {
                _onPop?.Invoke(null);
                return;
            }

            //合并索引
            long id = ALCommon.mergeInt(_goIndex.mainId, _goIndex.subId);
            NPSpaceGameObjectCache cache = null;

            //不存在缓存池，加载并添加到字典
            if (!_m_dCacheDic.TryGetValue(id, out cache))
            {
                cache = new NPSpaceGameObjectCache(_goIndex.assetPath, _goIndex.objName, _m_cacheRoot, _m_iMinCacheCount, _m_iMaxCacheCount);
                cache.load();
                _m_dCacheDic[id] = cache;
            }

            //缓存池加载完成后调用popItem
            cache.regLoadDoneDelegate(() =>
            {
                GameObject go = cache.popItem();
                ALUGUICommon.setGameObjEnable(go, true);
                _onPop?.Invoke(go);
            });
        }

        public void pushbackItem(NPGGoIndex _goIndex, GameObject _unitView)
        {
            if (_goIndex == null || _unitView == null)
                return;

            //合并索引
            long id = ALCommon.mergeInt(_goIndex.mainId, _goIndex.subId);
            NPSpaceGameObjectCache cache = null;

            //回收
            if (!_m_dCacheDic.TryGetValue(id, out cache))
            {
                Debug.LogError($"【NPGGoIndexCacheMgr.pushbackItem Error】缓存池不存在，NPGGoIndex mainId:{_goIndex.mainId},subId:{_goIndex.subId}");
                // 释放加载CreateItem中的东西
                ALUnityCommon.releaseGameObj(_unitView);
                return;
            }
            else
            {
                if (cache == null)
                {
                    Debug.LogError($"【NPGGoIndexCacheMgr.pushbackItem Error】缓存池为null，NPGGoIndex mainId:{_goIndex.mainId},subId:{_goIndex.subId}");
                    // 释放加载CreateItem中的东西
                    ALUnityCommon.releaseGameObj(_unitView);
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