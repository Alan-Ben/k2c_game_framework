using System;
using ALPackage;
using ChatPackage.Internal;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 用于纯显示对象使用，直接加载 GameObject 的缓存池
    /// </summary>
    public class NPSpaceGameObjectCache: _AALUnsafeLoadCacheController<GameObject, GameObject>
    {
        // 资源路径和资源名
        private string _m_assetPath;
        private string _m_objName;
        // 缓存中cache的父对象
        [NotNull] private readonly GameObject _m_gRootGo;
        // 是否当帧必须全部加载完成
        private readonly bool _m_bIsWndLoadSync;

        internal NPSpaceGameObjectCache(string _assetPath, string _objName, [NotNull] GameObject _parent, int _minCount, int _maxCount, bool _isWndLoadSync = true)
            : base(_minCount, _maxCount)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
            
            _m_gRootGo = _parent;
            _m_bIsWndLoadSync = _isWndLoadSync;
        }

        protected override string _warningTxt
        {
            get
            {
                return "GameObject<" + _m_assetPath + "_" + _m_objName + ">";
            }
        }
        /// <summary>
        /// 需要加载的模板的路径
        /// </summary>
        public string assetPath
        {
            get { return _m_assetPath; }
        }
        /// <summary>
        /// 需要加载的模板的名字
        /// </summary>
        public string objName
        {
            get { return _m_objName; }
        }

        /// <inheritdoc/>
        protected override GameObject _createItem(GameObject _template)
        {
            if (_template == null)
                return null;

            // 使用模板实例化一个新的
            GameObject go = Object.Instantiate(_template);

            // 先放到缓存池的父对象下面
            if (go != null)
                go.transform.SetParent(_m_gRootGo.transform);

            // 返回创建的对象
            return go;
        }
        /// <inheritdoc/>
        protected override void _discardItem(GameObject _item)
        {
            if (null == _item || null == _item.transform)
                return;

            // 释放加载CreateItem中的东西
            ALUnityCommon.releaseGameObj(_item);
        }
        /// <inheritdoc/>
        protected override void _onInit(GameObject _template)
        { }
        /// <inheritdoc/>
        protected override void _resetItem(GameObject _item)
        {
            if (null == _item || null == _item.transform)
                return;

            // 先Disable，再修改parent，防止两个地方都dirty，提高性能
            ALUGUICommon.setGameObjDisable(_item.gameObject);

            // 回收到缓存池的父对象下
            _item.transform.SetParent(_m_gRootGo.transform);
            _item.transform.localPosition = Vector3.zero;
            //_item.transform.localScale = Vector3.zero;
        }
        /// <inheritdoc/>
        protected override void _loadTemplate(Action<GameObject> _loaded)
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

                // 调用完成回调
                _loaded.Invoke(_go);
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
                GameObject temp = _assetObj.load<GameObject>(_m_objName);
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
                ALLocalResLoaderMgr.instance?.loadSynUIAsset(_m_assetPath, _m_objName, _loadUIDelegate, null, _loadLocalUIDelegate, GameResCore.instance);
            else
                ALLocalResLoaderMgr.instance?.loadUIAsset(_m_assetPath, _m_objName, _loadUIDelegate, null, _loadLocalUIDelegate, GameResCore.instance);
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
        protected override void _discardTemplate(GameObject _template)
        {
        }
    }
}