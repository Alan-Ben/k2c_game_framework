using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class SfxCache
    {
        private static SfxCache _g_instance = new SfxCache();
        public static SfxCache instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new SfxCache();
                return _g_instance;
            }
        }

        //索引值对应的缓存管理对象
        private Dictionary<long, WCGSfxGoCache> _m_dicSfxCache;

        //特效总对象的根节点对象
        private GameObject _m_goRootGo;

        public SfxCache()
        {
            _m_dicSfxCache = new Dictionary<long, WCGSfxGoCache>();

            _m_goRootGo = new GameObject();
            _m_goRootGo.name = "sfx_root";
            _m_goRootGo.transform.localScale = Vector3.one;
            _m_goRootGo.transform.position = Vector3.up * -10000;

            //设置不被删除
            GameObject.DontDestroyOnLoad(_m_goRootGo);
        }


        //根据合并后的ID从缓存中获取go
        public void popCacheGo(NPSfxRefObj _ref, Action<NPSfxMono> _delegate)
        {
            if (null == _delegate)
                return;

            if(null == _ref || _ref.sfx_index == null)
            {
                _delegate(null);
                return;
            }

            //拼凑Id
            long sfxMergeIndex = ALCommon.mergeInt(_ref.sfx_index.mainId, _ref.sfx_index.subId);

            WCGSfxGoCache cacheObj = null;
            if(!_m_dicSfxCache.TryGetValue(sfxMergeIndex, out cacheObj))
            {
                GSfxResCore.instance.loadObj(_ref.sfx_index, (resObj) =>
                {
                    //连续调用两次相同的loadObj时，可能会出现第二次调用时已经有cacheObj的情况
                    if (_m_dicSfxCache.ContainsKey(sfxMergeIndex))
                    {
                        popCacheGo(_ref, _delegate);
                        return;
                    }
                    
                    GameObject sfx = resObj.loadedInfo.obj;
                    if(null == sfx)
                    {
                        _delegate(null);
                        return;
                    }

                    NPSfxMono prefab = sfx.GetComponent<NPSfxMono>();
                    //此时需要构建cache对象
                    if (prefab == null)
                    {
                        _delegate(null);
                        return;
                    }

                    //构建对象
                    cacheObj = new WCGSfxGoCache(sfxMergeIndex, _m_goRootGo.transform, _ref.max_cache_count, _ref.min_cache_count);
                    //加入映射表
                    _m_dicSfxCache.TryAdd(sfxMergeIndex, cacheObj);

                    //初始化cache
                    cacheObj.init(prefab);

                    popCacheGo(_ref, _delegate);
                });
                
                return;
            }

            if(null == cacheObj)
            {
                _delegate(null);
                return;
            }

            //返回
            NPSfxMono sfxGo = cacheObj.popItem();
            if(null == sfxGo)
            {
                _delegate(null);
                return;
            }

            //返回GO
            _delegate(sfxGo);
        }

        //根据合并后的ID加入缓存
        public void AddToCache(NPGSfxIndex _sfxIndex, NPSfxMono _sfxGo)
        {
            AddToCache(ALCommon.mergeInt(_sfxIndex.mainId, _sfxIndex.subId), _sfxGo);
        }
        public void AddToCache(long _mergeIndex, NPSfxMono _sfxGo)
        {
            if(_sfxGo == null)
                return;

            if(!_m_dicSfxCache.ContainsKey(_mergeIndex))
                return;

            _m_dicSfxCache[_mergeIndex].pushBackCacheItem(_sfxGo);
        }

        /********************
         * 释放所有资源
         **/
        public void discard()
        {
            foreach(WCGSfxGoCache cache in _m_dicSfxCache.Values)
            {
                cache.discard();
            }
            _m_dicSfxCache.Clear();
        }
        
        public void discardAllUnUseCacheItem()
        {
            if(null == _m_dicSfxCache || _m_dicSfxCache.Count == 0)
                return;
            
            foreach(WCGSfxGoCache cache in _m_dicSfxCache.Values)
            {
                if(null == cache)
                    continue;
                
                cache.discardAllUnUseCacheItem();
            }
        }
    }
}
