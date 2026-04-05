
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 用来加载通用的 SubWnd 的 Cache 管理器
    /// </summary>
    public abstract class _ANPGGUISubWndCacheMgr<T_WND, T_MONO> 
        where T_WND : _ANPGGUIBasicSubWnd<T_MONO> 
        where T_MONO : _AALBasicUIWndMono
    {
        // 缓存池的最小和最大数量
        private readonly int _m_minCount;
        private readonly int _m_maxCount;
        private readonly bool _m_isWndLoadSync;

        [NotNull] private readonly GameObject _m_cacheRoot;//根节点
        [NotNull] private readonly Dictionary<long, NPGGUISubWndCache<T_WND, T_MONO>> _m_dCacheDic;//缓存池字典


        public _ANPGGUISubWndCacheMgr(string _name, int _minCount, int _maxCount, bool _isWndLoadSync = true)
        {
            _m_minCount = _minCount;
            _m_maxCount = _maxCount;
            _m_isWndLoadSync = _isWndLoadSync;

            _m_cacheRoot = new GameObject(_name);
            _m_cacheRoot.transform.localPosition = Vector3.up * -10000;
            Object.DontDestroyOnLoad(_m_cacheRoot);
            _m_dCacheDic = new Dictionary<long, NPGGUISubWndCache<T_WND, T_MONO>>();
        }


        public void discard()
        {
            foreach (NPGGUISubWndCache<T_WND, T_MONO> cache in _m_dCacheDic.Values)
            {
                cache?.discard();
            }

            _m_dCacheDic.Clear();
        }

        public void popItem(long _resPathId, Action<T_WND> _onPop)
        {
            if (_m_cacheRoot == null)
            {
                _onPop?.Invoke(null);
                return;
            }

            NPGGUISubWndCache<T_WND, T_MONO> cache;
            //不存在缓存池，加载并添加到字典
            if (!_m_dCacheDic.TryGetValue(_resPathId, out cache))
            {
                cache = new NPGGUISubWndCache<T_WND, T_MONO>(UIResPathAssistant.getAssetPath(_resPathId), UIResPathAssistant.getObjName(_resPathId), this);
                cache.load();
                _m_dCacheDic[_resPathId] = cache;
            }

            //缓存池加载完成后调用popItem
            cache.regLoadDoneDelegate(() =>
            {
                T_WND go = cache.popItem();
                _onPop?.Invoke(go);
            });
        }

        public void pushbackItem(long _resPathId, T_WND _wnd)
        {
            if (_wnd == null)
                return;

            NPGGUISubWndCache<T_WND, T_MONO> cache;
            //回收
            if (!_m_dCacheDic.TryGetValue(_resPathId, out cache))
            {
                Debug.LogError($"【_ANPGGUISubWndCacheMgr<T_WND, T_MONO> Error】缓存池不存在，_resPathId : {_resPathId}");
                return;
            }
            else
            {
                if (cache == null)
                {
                    Debug.LogError($"【_ANPGGUISubWndCacheMgr<T_WND, T_MONO> Error】缓存池为null，_resPathId : {_resPathId}");
                    return;
                }
                else
                {
                    cache.pushBackCacheItem(_wnd);
                }
            }
        }
        
        protected abstract T_WND createWnd(T_MONO _mono);

        private class NPGGUISubWndCache<T_WND, T_MONO> : _ANPGGUISubWndCache<T_WND, T_MONO>
            where T_WND : _ANPGGUIBasicSubWnd<T_MONO>
            where T_MONO : _AALBasicUIWndMono
        {
            [NotNull] private readonly _ANPGGUISubWndCacheMgr<T_WND, T_MONO> _m_cacheMgr;
            
            public NPGGUISubWndCache(string _assetPath, string _objName, [NotNull] _ANPGGUISubWndCacheMgr<T_WND, T_MONO> _cacheMgr) 
                : base(_assetPath, _objName, _cacheMgr._m_cacheRoot, _cacheMgr._m_minCount, _cacheMgr._m_maxCount, _cacheMgr._m_isWndLoadSync)
            {
                _m_cacheMgr = _cacheMgr;
            }

            protected override T_WND _createWnd(T_MONO _mono)
            {
                return _m_cacheMgr.createWnd(_mono);
            }
        }
    }
}