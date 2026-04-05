
using System;
using ALPackage;
using UnityEngine;
using ChatPackage.Internal;
using JetBrains.Annotations;

namespace ChatPackage
{
    /// <summary>
    /// 用于<see cref="GUISubWndChatMsgList"/>的item类
    /// </summary>
    /// <remarks>
    /// <para>你如果需要使用<see cref="GUISubWndChatMsgList"/>，就必须为不同的data实现这个类，然后使用registerCache进行设置，详情可以查看<see cref="Chat"/></para>
    /// <para>你实现这个类需要重点实现getHeight接口，实现这个接口时使用_originMono来获取对应的mono，注意这个_originMono可能为null，并且在高度发生变化的时候需要手动调用_refreshHeight方法来刷新高度</para>
    /// <para>对于_originMono会在这个类loadTemplate后，或load之后有值，其它时候为null，对于load和loadTemplate的区别你需要弄明白，可以看下面的介绍</para>
    /// <para>其它的只需要正常实现底层wnd的统一接口即可，更多的自定义内容可以查看<see cref="_ATALUGUIWndVerticalMultiSizeItem{T_MONO}"/></para>
    /// <para>这个类会重载loadOp和discard，在load时从缓存池中加载mono，discard时把mono放回缓存池</para>
    /// <para><b>特别注意：你在实现这个类时，请不要更改构造方法的参数内容</b></para>
    /// <para><b>特别注意：这个类会在内部自己调用load和discard方法，请不要自己随意调用相关的加载操作</b></para>
    /// <para>对于load和loadTemplate的区别，load上面写了是从缓存池中pop出Item，我们知道缓存池pop的Item都是一个模板对象的Clone，而loadTemplate则是获取到最原始的模板对象，这个对象是专门设计来获取高度的</para>
    /// </remarks>
    /// <typeparam name="T_MONO">对应的Mono类</typeparam>
    /// <typeparam name="T_DATA">对应的Data类</typeparam>
    public abstract class _AGUISubWndChatMsgListItem<T_MONO, T_DATA> : _ATALUGUIWndVerticalMultiSizeItem<T_MONO>, _IGUISubWndChatMsgListItem
        where T_MONO : _AGUIMonoChatMsgListItem
        where T_DATA : _IMsgItemData
    {
        // 用来加载自己的cacheMgr
        [NotNull] private GUICacheMgrChatMsgItem _m_cacheMgr;
        // 这个item的数据
        private readonly T_DATA _m_detailInfo;
        // 这个item的模板mono
        private T_MONO _m_originMono;
        // 管理这个item的父对象
        private readonly Transform _m_parent;
        // 当高度发生变化的事件
        private Action _m_aOnHeightChg;
        // 模板的加载序列号
        private int _m_iTemplateLoadSerialize;
        // 模板加载计数器
        [NotNull] private ALStepCounter _m_templateLoadCounter;
        
        protected _AGUISubWndChatMsgListItem(T_DATA _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr)
        {
            _m_detailInfo = _detailInfo;
            _m_parent = _parent;
            _m_cacheMgr = _cacheMgr;

            _m_templateLoadCounter = new ALStepCounter();
        }

        /// <summary>
        /// 当item高度发生变化的事件
        /// </summary>
        public event Action onHeightChg { add { _m_aOnHeightChg += value; } remove { _m_aOnHeightChg -= value; } }
        /// <summary>
        /// 这个item的数据
        /// </summary>
        public T_DATA detailInfo { get { return _m_detailInfo; } }
        
        /// <summary>
        /// 这个item的模板mono
        /// </summary>
        /// <remarks>
        /// <b>特别注意：这个属性是缓存池中的模板，请不要销毁这个mono</b>
        /// </remarks>
        protected T_MONO _originMono { get { return _m_originMono != null ? _m_originMono : wnd; } }
        
        /// <summary>
        /// 加载模板对象
        /// </summary>
        public void loadTemplate()
        {
            _m_iTemplateLoadSerialize = ALSerializeOpMgr.next();
            int serialize = _m_iTemplateLoadSerialize;
            _m_templateLoadCounter.resetAll();
            _m_templateLoadCounter.chgTotalStepCount(1);
            _m_templateLoadCounter.regAllDoneDelegate(_refreshHeight);
            _loadAdditionTemplate(_m_templateLoadCounter);
            _m_cacheMgr.getTemplate<T_MONO>(_m_detailInfo, (_template) =>
            {
                if (serialize != _m_iTemplateLoadSerialize)
                    return;

                _m_originMono = _template;
                _m_templateLoadCounter.addDoneStepCount();
            });
        }
        /// <summary>
        /// 释放模板对象
        /// </summary>
        public void discardTemplate()
        {
            _m_iTemplateLoadSerialize = ALSerializeOpMgr.next();
            _m_originMono = null;
            _discardAdditionTemplate();
        }

        /// <inheritdoc/>
        protected override Transform _getParentTransForm()
        {
            return _m_parent;
        }
        /// <summary>
        /// 当这个item的height发生变化时，刷新item的高度用的
        /// </summary>
        protected void _refreshHeight()
        {
            _m_aOnHeightChg?.Invoke();
        }
        /// <inheritdoc/>
        protected override void _onInViewport()
        {
            // 在进入视野时才加载对象
            load();
        }
        /// <inheritdoc/>
        protected override void _onOutViewport()
        {
            // 在退出视野时就回收
            discard();
        }
        /// <inheritdoc/>
        protected override void _loadOp()
        {
            _m_lLoadSerialize = _AALMonoMain.newObjSerialzie();
            
            if (_m_detailInfo == null)
                return;

            // 从缓存池中加载mono
            _m_cacheMgr.popItem<T_MONO>(_m_detailInfo, _getParentTransForm(), _loadDone);
        }
        /// <summary>
        /// 加载额外的模板
        /// </summary>
        /// <remarks>
        /// 如果你的item的高度依赖的除了originMono，还依赖别的，使用这个方法进行加载
        /// </remarks>
        protected abstract void _loadAdditionTemplate([NotNull] ALStepCounter _stepCounter);
        /// <summary>
        /// 释放额外的模板
        /// </summary>
        /// <remarks>
        /// 如果你的item的高度依赖的除了originMono，还依赖别的，使用这个方法进行卸载
        /// </remarks>
        protected abstract void _discardAdditionTemplate();
        /// <inheritdoc/>
        protected override void _discard()
        {
#region 基类_discard中没有修改的部分
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] _discard.");
            }

            //获取新序列号
            _m_lLoadSerialize = -1;

            //判断是否加载完成，是则隐藏本窗口
            hideWnd();

#if AL_PUERTS
            //尝试执行对应的Puerts脚本
            if (null != _puertsMgr && null != wnd && !string.IsNullOrEmpty(wnd.puertsScriptsPath))
            {
                PuertsWndFunc discardFunc = _puertsMgr.execGeneralScripts<PuertsWndFunc>($"require('{wnd.puertsScriptsPath}').discard;");

                //调用后即刻重置
                if (discardFunc != null)
                    discardFunc(this.wnd);
                discardFunc = null;
            }
#endif

            //调用事件函数
            _onDiscard();

            _m_rtRectTransform = null;

            //释放所有对象
            if (null != _m_dicTmpObj)
            {
                _m_dicTmpObj.Clear();
            }
#endregion

            // 由释放资源改为回收到cache里
            if (_m_monoWnd != null && _m_detailInfo != null)
                _m_cacheMgr.pushBackItem(_m_detailInfo, _m_monoWnd.gameObject);

            _m_monoWnd = default;
        }

        // 缓存池加载完成之后的处理
        private void _loadDone(T_MONO _mono)
        {
            // 判断是否已经被discard了
            if (_m_lLoadSerialize == -1)
            {
                // 如果已经被discard了，就尝试回收这个对象
                if (_mono != null && _m_detailInfo != null)
                    _m_cacheMgr.pushBackItem(_m_detailInfo, _mono.gameObject);
                // 然后直接返回，不做任何处理
                return;
            }
            
            // 赋值到底层的mono上
            _m_monoWnd = _mono;
            // 接着走底层正常的流程
            _initWnd();
            // 直接调用showWnd显示
            showWnd();
        }
        
#region 无用重载内容
        protected sealed override string _monoAssetPath
        {
            get
            {
                ChatUtility.logError_DebugOnly("_AGUISubWndChatMsgListItem.assetPath在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return string.Empty;
            }
        }

        protected sealed override string _monoObjName
        {
            get
            {
                ChatUtility.logError_DebugOnly("_AGUISubWndChatMsgListItem.objName在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return string.Empty;
            }
        }

        protected sealed override _AALResourceCore _resourceCore
        {
            get
            {
                ChatUtility.logError_DebugOnly("_AGUISubWndChatMsgListItem.resourceCore在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return null;
            }
        }
#endregion
    }
}