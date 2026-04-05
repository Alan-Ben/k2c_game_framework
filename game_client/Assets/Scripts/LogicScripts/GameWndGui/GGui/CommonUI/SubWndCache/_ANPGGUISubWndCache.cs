using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 一个 sub wnd 的 cache 实体类
    /// </summary>
    public abstract class _ANPGGUISubWndCache<T_WND, T_MONO> : _AALUnsafeLoadCacheController<T_WND, T_MONO>
        where T_WND : _ANPGGUIBasicSubWnd<T_MONO>
        where T_MONO : _AALBasicUIWndMono
    {
        // 资源路径和资源名
        private string _m_assetPath;
        private string _m_objName;

        // 缓存中 cache 的父对象
        [NotNull] private readonly GameObject _m_gRootGo;
        // 是否当帧必须全部加载完成
        private readonly bool _m_bIsWndLoadSync;

        public _ANPGGUISubWndCache(string _assetPath, string _objName, [NotNull] GameObject _parent, int _minCount, int _maxCount, bool _isWndLoadSync = true) : base(
            _minCount, _maxCount)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;

            _m_gRootGo = _parent;
            _m_bIsWndLoadSync = _isWndLoadSync;
        }

        protected override string _warningTxt
        {
            get { return "SubWnd<" + _m_assetPath + "_" + _m_objName + ">"; }
        }

        /// <inheritdoc/>
        protected override T_WND _createItem(T_MONO _template)
        {
            if (_template == null)
                return null;

            // 使用模板实例化一个新的
            T_MONO mono = Object.Instantiate(_template, _m_gRootGo.transform, true);

            // 先放到缓存池的父对象下面
            if (mono == null)
                return null;

            T_WND wnd = _createWnd(mono);

            // 返回创建的对象
            return wnd;
        }

        /// <inheritdoc/>
        protected override void _discardItem(T_WND _item)
        {
            T_MONO mono = null;
            if(_item != null)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }

        /// <inheritdoc/>
        protected override void _onInit(T_MONO _template)
        {
        }

        /// <inheritdoc/>
        protected override void _resetItem(T_WND _item)
        {
            if (null == _item || null == _item.wnd)
                return;

            _item.resetWnd();
            
            // 回收到缓存池的父对象下
            _item.wnd.transform.SetParent(_m_gRootGo.transform);
            _item.wnd.transform.localPosition = Vector3.zero;
        }

        /// <inheritdoc/>
        protected override void _loadTemplate(Action<T_MONO> _loaded)
        {
            if (_loaded == null)
                return;

#if UNITY_EDITOR
            // 本地资源的加载完成回调
            void _loadLocalUIDelegate(GameObject _go)
            {
                if (null == _go)
                {
                    Debug.LogError($"Load GUI Asset: {_m_assetPath}, {_m_objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                // 加载objName对应的资源
                T_MONO temp = _go.GetComponent<T_MONO>();
                if (temp == null)
                {
                    Debug.LogError($"Load GUI Asset: {_m_assetPath}, {_m_objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                // 调用完成回调
                _loaded.Invoke(temp);
            }
#endif
            // AB资源加载完成后的回调
            void _loadUIDelegate(bool _isSuc, ALAssetBundleObj _assetObj)
            {
                if (!_isSuc || _assetObj == null)
                {
                    Debug.LogError($"Load GUI Asset: {_m_assetPath}, {_m_objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                // 加载objName对应的资源
                GameObject _go = _assetObj.load<GameObject>(_m_objName);
                if (_go == null)
                {
                    Debug.LogError($"Load GUI Asset: {_m_assetPath}, {_m_objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }
                T_MONO temp = _go.GetComponent<T_MONO>();
                if (temp == null)
                {
                    Debug.LogError($"Load GUI Asset: {_m_assetPath}, {_m_objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                // 调用完成回调
                _loaded.Invoke(temp);
            }

            // 开始加载，根据设置，调用底层包方法进行加载
#if UNITY_EDITOR
            if (_m_bIsWndLoadSync)
                ALLocalResLoaderMgr.instance?.loadSynUIAsset(_m_assetPath, _m_objName, _loadUIDelegate, null,
                    _loadLocalUIDelegate, GameResCore.instance);
            else
                ALLocalResLoaderMgr.instance?.loadUIAsset(_m_assetPath, _m_objName, _loadUIDelegate, null,
                    _loadLocalUIDelegate, GameResCore.instance);
#else
            if (GameResCore.instance != null)
            {
                if (_m_bIsWndLoadSync)
                    GameResCore.instance.loadSynAsset(_m_assetPath, _loadUIDelegate, null);
                else
                    GameResCore.instance.loadAsset(_m_assetPath, _loadUIDelegate, null);
            }
            else
            {
                _loaded.Invoke(null);
            }
#endif
        }

        /// <inheritdoc/>
        protected override void _discardTemplate(T_MONO _template)
        {
        }

        protected abstract T_WND _createWnd(T_MONO _mono);
    }
}