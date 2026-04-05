
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 聊天气泡的缓存
    /// </summary>
    public class NPGGUICacheChatMsgItemTextBubble : _AALLoadCacheController<NPGGUIWndChatMsgItemTextBubble, NPGGUIMonoChatMsgItemTextBubble>
    {
        // 对应的配置
        private readonly string _m_assetPath;
        private readonly string _m_objName;
        // 缓存中cache的父对象
        [NotNull] private readonly GameObject _m_gRootGo;
        // 是否当帧必须全部加载完成
        private readonly bool _m_bIsWndLoadSync;

        // 构建时设置用于构建的内容
        public NPGGUICacheChatMsgItemTextBubble(string _assetPath, string _objName, [NotNull] GameObject _parent, int _minCacheCount = 5, int _maxCacheCount = 20, bool _isWndLoadSync = true)
            : base(_minCacheCount, _maxCacheCount)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
            _m_gRootGo = _parent;
            _m_bIsWndLoadSync = _isWndLoadSync;
        }

        // 底层会使用的警告信息文字
        protected override string _warningTxt
        {
            get
            {
                return "Chat Bubble<" + _m_assetPath + _m_objName + ">";
            }
        }
        /// <summary>
        /// 需要加载的模板的路径
        /// </summary>
        public string assetPath { get { return _m_assetPath; } }
        /// <summary>
        /// 需要加载的模板的名字
        /// </summary>
        public string objName { get { return _m_objName; } }
        /// <summary>
        /// 加载完后的模板
        /// </summary>
        /// <remarks>
        /// 如果加载失败或者没有加载完有可能为null
        /// </remarks>
        public NPGGUIMonoChatMsgItemTextBubble template { get { return _m_tTemplateObj; } }

        /// <inheritdoc/>
        protected override NPGGUIWndChatMsgItemTextBubble _createItem(NPGGUIMonoChatMsgItemTextBubble _template)
        {
            if (_template == null)
                return null;

            // 使用模板实例化一个新的
            NPGGUIMonoChatMsgItemTextBubble wnd = Object.Instantiate(_template);

            // 先放到缓存池的父对象下面
            if (wnd != null)
            {
                wnd.transform.SetParent(_m_gRootGo.transform, false);
                wnd.transform.localPosition = Vector3.zero;
            }

            // 返回创建的对象
            return new NPGGUIWndChatMsgItemTextBubble(wnd);
        }
        /// <inheritdoc/>
        protected override void _discardItem(NPGGUIWndChatMsgItemTextBubble _item)
        {
            if (null == _item)
                return;

            // 释放加载CreateItem中的东西
            ALUnityCommon.releaseGameObj(_item.wnd);
        }
        /// <inheritdoc/>
        protected override void _onInit(NPGGUIMonoChatMsgItemTextBubble _template)
        { }
        /// <inheritdoc/>
        protected override void _resetItem(NPGGUIWndChatMsgItemTextBubble _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            // 先Disable，再修改parent，防止两个地方都dirty，提高性能
            _item.resetWnd();

            // 回收到缓存池的父对象下
            _item.wnd.transform.SetParent(_m_gRootGo.transform, false);
            _item.wnd.transform.localPosition = Vector3.zero;
        }
        /// <inheritdoc/>
        protected override void _loadTemplate(Action<NPGGUIMonoChatMsgItemTextBubble> _loaded)
        {
            if (_loaded == null)
                return;

            // 本地资源的加载完成回调
            void _loadGoDelegate(GameObject _go)
            {
                if (null == _go)
                {
                    Debug.LogError($"Load GUI Asset: {_m_assetPath}, {_m_objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }
                
                // 实例化一个新的对象，后续需要修改这个对象的字体，为了不改到 prefab 
                _go = Object.Instantiate(_go, _m_gRootGo.transform, false);
                if (null == _go)
                {
                    Debug.LogError($"Load GUI Asset: {_m_assetPath}, {_m_objName}, res file instantiate fail!");
                    _loaded.Invoke(null);
                    return;
                }
                
                _go.SetActive(false);
                NPGGUIMonoChatMsgItemTextBubble mono = _go.GetComponent<NPGGUIMonoChatMsgItemTextBubble>();
                if (mono == null)
                {
                    Debug.LogError($"Load GUI Asset: {_m_assetPath}, {_m_objName}, res file load fail!");
                    _loaded.Invoke(null);
                    return;
                }

                // 调用完成回调
                _loaded.Invoke(mono);
            }

            if (GameResCore.instance != null)
            {
                // 开始加载，根据设置，调用底层包方法进行加载
                ALAssetLoader<GameObject> loader = new ALAssetLoader<GameObject>(GameResCore.instance, _m_assetPath, _m_objName
#if UNITY_EDITOR
            , ".prefab", "t:prefab"
#endif
            , _m_bIsWndLoadSync);

                loader.loadAsset(_loadGoDelegate);
            }
            else
            {
                _loaded.Invoke(null);
            }
        }
        /// <inheritdoc/>
        protected override void _discardTemplate(NPGGUIMonoChatMsgItemTextBubble _template)
        {
            // 释放加载 LoadTemplate 中的东西，因为 loadTemplate 中实例化了一个 copy 这边把它释放掉
            if (null == _template)
                return;

            ALUnityCommon.releaseGameObj(_template.gameObject);
        }
    }
}