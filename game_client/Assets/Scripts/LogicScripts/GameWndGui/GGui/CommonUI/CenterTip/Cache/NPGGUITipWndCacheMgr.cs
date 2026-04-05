using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace GOE
{
    /// <summary>
    /// tip缓存池管理器，只做缓存池管理，pop和pushBack在外部调用
    /// </summary>
    public class NPGGUITipWndCacheMgr
    {
        private static NPGGUITipWndCacheMgr _g_instance;
        public static NPGGUITipWndCacheMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUITipWndCacheMgr();
                return _g_instance;
            }
        }

        private readonly GameObject _m_cacheRoot;
        private Dictionary<long, _INPTipCache> _m_dTipCacheDic;//缓存池字典<提示配置id,缓存池>

        private NPGGUITipWndCacheMgr()
        {
            _m_cacheRoot = new GameObject("tip_item_cache_root");
            _m_cacheRoot.transform.localPosition = Vector3.up * -10000;
            UnityEngine.Object.DontDestroyOnLoad(_m_cacheRoot);
            _m_dTipCacheDic = new Dictionary<long, _INPTipCache>();
        }

        /// <summary>
        /// 销毁所有缓存池
        /// </summary>
        public void clear()
        {
            if (_m_dTipCacheDic != null)
            {
                foreach (_INPTipCache cache in _m_dTipCacheDic.Values)
                {
                    cache?.discard();
                }
                _m_dTipCacheDic.Clear();
            }
        }

        /// <summary>
        /// 加载tip缓存池
        /// </summary>
        /// <typeparam name="_T_WND"></typeparam>
        /// <typeparam name="_T_MONO"></typeparam>
        /// <param name="_tipRef">唯一id</param>
        /// <param name="_onLoaded">加载完成回调</param>
        public void loadCache<_T_WND, _T_MONO>(NPCenterTipsRefObj _tipRef, Action<_TNPGGUITipWndCache<_T_WND, _T_MONO>> _onLoaded)
            where _T_WND : _ATNPGGUIWndTip<_T_MONO>
            where _T_MONO : NPGGUIMonoCommonTip
        {
            if (_tipRef == null || _m_dTipCacheDic == null)
                return;

            if (_m_dTipCacheDic.ContainsKey(_tipRef.id))
            {
                //类型判断
                if (_m_dTipCacheDic[_tipRef.id] is _TNPGGUITipWndCache<_T_WND, _T_MONO>)
                {
                    //只在内部做一次类型强转
                    _onLoaded?.Invoke(_m_dTipCacheDic[_tipRef.id] as _TNPGGUITipWndCache<_T_WND, _T_MONO>);
                }
                else
                {
                    Debug.LogError($"【NPGGUITipWndCacheMgr Error】loadCache：加载缓存池错误，tipId:{_tipRef.id}已经加载过，且类型为{_m_dTipCacheDic[_tipRef.id].GetType()}");
                    _onLoaded?.Invoke(null);
                }
            }
            else
            {
                _createCache<_T_WND, _T_MONO>(_tipRef, (_cache) =>
                {
                    _onLoaded?.Invoke(_cache);
                });
            }
        }

        /// <summary>
        /// 加载tip模板并添加到缓存池字典中
        /// </summary>
        /// <typeparam name="_T_WND"></typeparam>
        /// <typeparam name="_T_MONO"></typeparam>
        /// <param name="_tipRef"></param>
        /// <param name="_onLoaded"></param>
        private void _createCache<_T_WND, _T_MONO>(NPCenterTipsRefObj _tipRef, Action<_TNPGGUITipWndCache<_T_WND, _T_MONO>> _onLoaded)
            where _T_WND : _ATNPGGUIWndTip<_T_MONO>
            where _T_MONO : NPGGUIMonoCommonTip
        {
            if (_tipRef == null)
                return;

            //获取资源路径
            string assetPath = UIResPathAssistant.getAssetPath(_tipRef.res_id);
            string objName = UIResPathAssistant.getObjName(_tipRef.res_id);

            //加载模板
            ALAssetLoader<GameObject> alAssetLoader = new ALAssetLoader<GameObject>(GameResCore.instance, assetPath, objName);
            alAssetLoader.loadAsset((_tipTemp) =>
            {
                if (_tipTemp == null)
                {
                    Debug.LogError($"【NPGGUITipWndCacheMgr Error】loadCache：加载缓存池错误，找不到资源，路径：{assetPath}资源名：{objName}");
                    _onLoaded?.Invoke(null);
                    return;
                }

                if (_m_dTipCacheDic == null)
                {
                    Debug.LogError($"【NPGGUITipWndCacheMgr Error】loadCache：加载缓存池错误，缓存池字典为null");
                    _onLoaded?.Invoke(null);
                    return;
                }

                _TNPGGUITipWndCache<_T_WND, _T_MONO> tipCache = null;
                //缓存池已经存在
                if (_m_dTipCacheDic.ContainsKey(_tipRef.id))
                {
                    //类型判断
                    if (_m_dTipCacheDic[_tipRef.id] is _TNPGGUITipWndCache<_T_WND, _T_MONO>)
                    {
                        //只在内部做一次类型强转
                        tipCache = _m_dTipCacheDic[_tipRef.id] as _TNPGGUITipWndCache<_T_WND, _T_MONO>;
                    }
                    else
                    {
                        Debug.LogError($"【NPGGUITipWndCacheMgr Error】loadCache：加载缓存池错误，tipId:{_tipRef.id}已经加载过，且类型为{_m_dTipCacheDic[_tipRef.id].GetType()}");
                    }
                }
                else
                {
                    //获取预制体上的脚本
                    _T_MONO tipMono = _tipTemp.GetComponent<_T_MONO>();

                    //脚本存在则创建对应缓存池
                    if (tipMono != null)
                    {
                        //创建新缓存池
                        tipCache = new _TNPGGUITipWndCache<_T_WND, _T_MONO>(_m_cacheRoot.transform, 1, 10);
                        tipCache.init(tipMono);
                        _m_dTipCacheDic.Add(_tipRef.id, tipCache);
                    }
                    else
                    {
                        Debug.LogError($"【NPGGUITipWndCacheMgr Error】loadCache：加载缓存池错误，tip模板UI资源{_tipTemp.name}上没有挂载脚本{typeof(_T_MONO)}");
                    }
                }
                _onLoaded?.Invoke(tipCache);
            });
        }
    }
}
