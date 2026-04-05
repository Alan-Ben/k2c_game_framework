
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 一个GameObject的缓存池
    /// </summary>
    /// <remarks>
    /// 因为聊天包希望可以支持所有类型的msgItem，所以就将msgItem的缓存概括为GameObject，pop出来之后再做获取处理
    /// <para>你可以使用：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>_AALLoadCacheController的相关处理</term>
    ///             <description>
    ///             主要为popItem和pushBackCacheItem，其它的可以参考<see cref="_AALLoadCacheController{T, TEMP}"/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>template</term>
    ///             <description>
    ///             你可以用这个属性，拿到加载到的最原始的模板对象，但是要注意不要把它卸载了
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal class ChatGameObjectCache : _AALLoadCacheController<GameObject, GameObject>
    {
        // 对应的设置
        [NotNull] private readonly GUICacheMgrChatMsgItemSetting _m_setting;
        // 缓存中cache的父对象
        [NotNull] private readonly GameObject _m_gRootGo;
        // 是否当帧必须全部加载完成
        private readonly bool _m_bIsWndLoadSync;

        // 构建时设置用于构建的内容
        internal ChatGameObjectCache([NotNull] GUICacheMgrChatMsgItemSetting _setting, [NotNull] GameObject _parent, int _minCacheCount = 1, int _maxCacheCount = 5, bool _isWndLoadSync = true)
            : base(_minCacheCount, _maxCacheCount)
        {
            _m_setting = _setting;
            _m_gRootGo = _parent;
            _m_bIsWndLoadSync = _isWndLoadSync;
        }

        // 底层会使用的警告信息文字
        protected override string _warningTxt
        {
            get
            {
                return "ChatPackage GameObject<" + _m_setting.assetPath + "_" + _m_setting.objName + ">";
            }
        }
        /// <summary>
        /// 需要加载的模板的路径
        /// </summary>
        public string assetPath
        {
            get { return _m_setting.assetPath; }
        }
        /// <summary>
        /// 需要加载的模板的名字
        /// </summary>
        public string objName
        {
            get { return _m_setting.objName; }
        }
        /// <summary>
        /// 加载完后的模板
        /// </summary>
        /// <remarks>
        /// 如果加载失败或者没有加载完有可能为null
        /// </remarks>
        public GameObject template
        {
            get { return _m_tTemplateObj; }
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
                    Debug.LogError($"Load GUI Asset: {_m_setting.assetPath}, {_m_setting.objName}, res file load fail!");
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
                    Debug.LogError($"Load GUI Asset: {_m_setting.assetPath}, {_m_setting.objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                // 加载objName对应的资源
                GameObject temp = _assetObj.load<GameObject>(_m_setting.objName);
                if (temp == null)
                {
                    Debug.LogError($"Load GUI Asset: {_m_setting.assetPath}, {_m_setting.objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                // 调用完成回调
                _loaded.Invoke(temp);
            }

            // 开始加载，根据设置，调用底层包方法进行加载
#if UNITY_EDITOR
            if (_m_bIsWndLoadSync)
                ALLocalResLoaderMgr.instance?.loadSynUIAsset(_m_setting.assetPath, _m_setting.objName, _loadUIDelegate, null, _loadLocalUIDelegate, _m_setting.resourceCore);
            else
                ALLocalResLoaderMgr.instance?.loadUIAsset(_m_setting.assetPath, _m_setting.objName, _loadUIDelegate, null, _loadLocalUIDelegate, _m_setting.resourceCore);
#else
            if (_m_setting.resourceCore != null)
            {
                if (_m_bIsWndLoadSync)
                    _m_setting.resourceCore.loadSynAsset(_m_setting.assetPath, _loadUIDelegate, null);
                else
                    _m_setting.resourceCore.loadAsset(_m_setting.assetPath, _loadUIDelegate, null);
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