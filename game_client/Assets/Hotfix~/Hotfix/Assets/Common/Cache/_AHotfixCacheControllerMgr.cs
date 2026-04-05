using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 通用 Cache Mgr 的基类模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEMP"></typeparam>
    /// <typeparam name="T_Cache"></typeparam>
    public abstract class _AHotfixCacheControllerMgr<T, TEMP, T_Cache>
        where T_Cache : _AHotfixCacheController<T, TEMP>
    {
        // 每个index的缓存池集合
        [NotNull] private Dictionary<int, T_Cache> _m_CacheMap = new Dictionary<int, T_Cache>();
        //正在加载的cache回调管理数据集
        [NotNull] private Dictionary<int, Action<T_Cache>> _m_dLoadingCacheDic = new Dictionary<int, Action<T_Cache>>();
        //是否已经销毁
        private bool _m_hasDiscard = false;
        //缓存池父节点
        protected GameObject _m_RootGo;
        
        public _AHotfixCacheControllerMgr(string _rootFolderName)
        {            
            _m_RootGo = new GameObject();
            _m_RootGo.name = _rootFolderName;
            _m_RootGo.transform.position = Vector3.up * -10000;
            
            _m_hasDiscard = false;
        }

        public _AHotfixCacheControllerMgr(GameObject _cacheParent)
        {
            _m_RootGo = _cacheParent;

            _m_hasDiscard = false;
        }

        /// <summary>
        /// 加载Asset
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_onLoaded"></param>
        protected abstract void LoadResource(BasicResIndexInfo _index, Action<TEMP> _onLoaded);
        /// <summary>
        /// 创建缓存池
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_asset"></param>
        /// <returns></returns>
        protected abstract T_Cache CreateCache(BasicResIndexInfo _index);

        /// <summary>
        /// POP 一个对象
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_onLoaded"></param>
        public void popItem(BasicResIndexInfo _index, Action<BasicResIndexInfo, T> _onLoaded)
        {
            //已经销毁不处理
            if (_m_hasDiscard)
            {
                if(null != _onLoaded)
                    _onLoaded(_index, default(T));
                return;
            }
            
            //不合法数据不处理
            if (null == _index || _index.isValid() == false)
            {
                Debug.LogError("_index不合法: " + _index);
                if(null != _onLoaded)
                    _onLoaded(_index, default(T));
                return;
            }
            
            T_Cache cache = null;
            int key = _index.GetHashCode();
            //每个索引单位有一个缓存池
            if(_m_CacheMap.TryGetValue(key, out cache))
            {
                if(null != _onLoaded)
                    _onLoaded(_index, cache.popItem());
            }
            else
            {
                _loadCache(_index, 
                    (_cache) =>
                    {
                        if (null == _cache)
                        {
                            if (null != _onLoaded)
                                _onLoaded(_index, default(T));
                            return;
                        }

                        //调用回调
                        if (null != _onLoaded)
                            _onLoaded(_index, _cache.popItem());
                    });
            }
        }

        /// <summary>
        /// 加载对应资源的cache，并进行初始化
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_loadAction"></param>
        protected void _loadCache(BasicResIndexInfo _index, Action<T_Cache> _loadAction)
        {
            int idx = _index.GetHashCode();
            T_Cache cache = null;

            //先检索已有数据
            if (_m_CacheMap.TryGetValue(idx, out cache))
            {
                if (null != _loadAction)
                    _loadAction(cache);

                return;
            }

            //判断是否在加载
            if (_m_dLoadingCacheDic.ContainsKey(idx))
            {
                //增加回调
                _m_dLoadingCacheDic[idx] += _loadAction;
                return;
            }

            //添加回调
            Action<T_Cache> preDelegate = default(Action<T_Cache>);
            if (null != _loadAction)
                preDelegate += _loadAction;
            _m_dLoadingCacheDic.Add(idx, preDelegate);

            //开始加载处理
            //找不到对应cache的时候拿一个模板来创建一个新的对应cache
            LoadResource(_index, delegate (TEMP _asset)
            {
                if (_m_hasDiscard)
                {
                    _onCacheInited(_index, null);
                    return;
                }

                T_Cache tmpCache = null;
                //查询当前是否已有缓存对象
                if (!_m_CacheMap.TryGetValue(idx, out tmpCache))
                {
                    //构建新缓存池
                    tmpCache = CreateCache(_index);
                    if (tmpCache == null)
                    {
                        Debug.LogError($"{_index}的cache创建失败");
                        _onCacheInited(_index, null);
                        return;
                    }

                    _m_CacheMap.Add(idx, tmpCache);
                    //初始化缓存
                    tmpCache.init(_asset);

                    //调用回调
                    _onCacheInited(_index, tmpCache);
                }
                else
                {
                    //调用回调
                    _onCacheInited(_index, tmpCache);
                }
            });
        }

        /// <summary>
        /// 当cache加载完成之后调用回调的处理
        /// </summary>
        /// <param name="_cache"></param>
        protected void _onCacheInited(BasicResIndexInfo _index, T_Cache _cache)
        {
            int idx = _index.GetHashCode();

            //调用回调处理
            Action<T_Cache> curDelegate = null;
            if (_m_dLoadingCacheDic.TryGetValue(idx, out curDelegate))
            {
                if (null != curDelegate)
                    curDelegate(_cache);

                //移除数据
                _m_dLoadingCacheDic.Remove(idx);
            }
        }


        /// <summary>
        /// 回收一个对象
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_item"></param>
        public void pushBackItem(BasicResIndexInfo _index, T _item)
        {
            if (_m_hasDiscard)
            {
                UnityEngine.Object itemObj = _item as UnityEngine.Object;
                if(itemObj != null)
                    ALUnityCommon.releaseGameObj(itemObj);
#if UNITY_EDITOR
                Debug.LogWarning($"_AHotfixCacheControllerMgr 释放后还有往里push，建议确认一下:{_index}");
#endif
                return;
            }
            
            if (null == _item || _index == null || _index.isValid() == false)
                return;

            T_Cache cache = null;
            int key = _index.GetHashCode();
            if(_m_CacheMap.TryGetValue(key, out cache))
            {
                cache.pushBackCacheItem(_item);
            }
            else
            {
                Debug.LogError($"pushBackItem时，传入了不是这里pop的东西:{_index}");
                //如果不是这里创建的，销毁这个_item的资源
                UnityEngine.Object itemObj = _item as UnityEngine.Object;
                if(itemObj != null)
                {
                    ALUnityCommon.releaseGameObj(itemObj);
                }
            }
        }

        /// <summary>
        /// 是否加载过并还在缓存池
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public bool isLoadItem(BasicResIndexInfo _index)
        {
            if(_index == null || _index.isValid() == false)
            {
                Debug.LogError("_index不合法: " + _index);
                return false;
            }
            int key = _index.GetHashCode();
            if(_m_CacheMap.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 清理所有缓存
        /// </summary>
        public void discardAll()
        {
            //处理所有加载中的回调
            foreach (Action<T_Cache> cacheDelegate in _m_dLoadingCacheDic.Values)
            {
                if (null != cacheDelegate)
                    cacheDelegate(null);
            }
            _m_dLoadingCacheDic.Clear();

            foreach (T_Cache item in _m_CacheMap.Values)
            {
                item.discard();
            }
            _m_CacheMap.Clear();
            ALUnityCommon.releaseGameObj(_m_RootGo);
            _m_RootGo = null;
            _m_hasDiscard = true;
        }

        /// <summary>
        /// 清理所有未使用的缓存对象
        /// </summary>
        public void discardAllUnUseCacheItem()
        {
            foreach (T_Cache item in _m_CacheMap.Values)
            {
                item.discardAllUnUseCacheItem();
            }
            _m_CacheMap.Clear();
        }
        
        /// <summary>
        /// 释放单个cacheController
        /// </summary>
        /// <param name="_cacheController"></param>
        public void discard(int _key)
        {
            if(_m_hasDiscard)
                return;
            
            if(_m_CacheMap.TryGetValue(_key, out T_Cache cache))
            {
                _m_CacheMap.Remove(_key);
                cache.discard();
            }
            else
            {
                Debug.LogError($"discard时，key没有注册：{_key}");
            }
        }
        
        /// <summary>
        ///  释放单个cacheController
        /// </summary>
        /// <param name="_cacheController"></param>
        public void discard(BasicResIndexInfo _index)
        {
            if(null == _index || !_index.isValid())
                return;
            
            int key = _index.GetHashCode();
            discard(key);
        }

        /// <summary>
        /// 加载Cache
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_onLoaded"></param>
        public void loadCache(BasicResIndexInfo _index, Action<T_Cache> _onLoaded)
        {
            int key = _index.GetHashCode();
            if (isLoadItem(_index))
            {
                _onLoaded?.Invoke(_m_CacheMap[key]);
            }
            else
            {
                _loadCache(_index, _onLoaded);
            }
        }
    }
}