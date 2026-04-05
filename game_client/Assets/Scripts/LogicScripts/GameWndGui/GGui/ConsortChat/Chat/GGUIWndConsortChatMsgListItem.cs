using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndConsortChatMsgListItem : _ATALUGUIWndVerticalMultiSizeItem<GGUIMonoConsortChatMsgListItem>
    {
        private _AConsortChatMsgInfo _m_msgInfo;
        // 管理这个item的父对象
        private readonly Transform _m_parent;
        //加载序列号
        protected long _m_lLoadSerialize;
        
        
        // 这个msgItem的高度
        private float _m_fHeight;
        
        // 当高度发生变化的事件
        private Action _m_aOnHeightChg;
        // 模板的加载序列号
        private int _m_iTemplateLoadSerialize;
        // 模板加载计数器
        [NotNull] private ALStepCounter _m_templateLoadCounter;
        
        // 这个item的模板mono
        private GGUIMonoConsortChatMsgListItem _m_originMono;

        private GGUIWndConsortIconItem _m_consortCardItem;

        private GGUISubWndConsortChatImageGroupPreview _m_imageGroupPreview;

        private NPGGUIWndCommonItemContainer _m_rewardContainer;
        private long _m_consortId;

        private long _m_refreshHeighSerializeOp;
        private bool _m_canGetRealHeight = false;
        private float _m_realHeight;
        
        /// <summary>
        /// 这个item的模板mono
        /// </summary>
        /// <remarks>
        /// <b>特别注意：这个属性是缓存池中的模板，请不要销毁这个mono</b>
        /// </remarks>
        protected GGUIMonoConsortChatMsgListItem _originMono { get { return _m_originMono != null ? _m_originMono : wnd; } }
        
        /// <summary>
        /// 当item高度发生变化的事件
        /// </summary>
        public event Action onHeightChg { add { _m_aOnHeightChg += value; } remove { _m_aOnHeightChg -= value; } }
        
        public GGUIWndConsortChatMsgListItem(_AConsortChatMsgInfo _msgInfo, long _consortId, Transform _parent)
        {
            _m_msgInfo = _msgInfo;
            _m_parent = _parent;
            _m_consortId = _consortId;
            _m_templateLoadCounter = new ALStepCounter();
        }
        
        protected override void _onShowWnd()
        {
            if (_m_msgInfo != null) _m_msgInfo.onMsgUpdate += _refreshWnd;
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            if (_m_msgInfo != null) _m_msgInfo.onMsgUpdate -= _refreshWnd;
            _m_refreshHeighSerializeOp = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_refreshHeighSerializeOp = ALSerializeOpMgr.next();
        }

        protected override void _onDiscard()
        {
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
            _m_imageGroupPreview?.discard();
            _m_imageGroupPreview = null;
            _m_rewardContainer?.discard();
            _m_rewardContainer = null;
            _m_typingSer = ALSerializeOpMgr.next();
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if(null == wnd)
                return;
            if (wnd.consortIconItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);
            if (wnd.imageGroupPreview != null)
            {
                if (_m_msgInfo != null && _m_msgInfo.imageGroupId > 0)
                {
                    _m_imageGroupPreview = new GGUISubWndConsortChatImageGroupPreview(wnd.imageGroupPreview);
                }
            }

            if (wnd.rewardContainer != null)
            {
                _m_rewardContainer = new NPGGUIWndCommonItemContainer(wnd.rewardContainer);
            }
        }

        public void setConsortId(long _consortId)
        {
            _m_consortId = _consortId;
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            if (_m_consortCardItem != null)
            {
                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortId);
                _m_consortCardItem.showWnd();
                _m_consortCardItem.setInfo(consortInfo, 0);
            }

            if (_m_msgInfo != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtText, _m_msgInfo.getContent());
                if (_m_imageGroupPreview != null)
                {
                    _m_imageGroupPreview.showWnd();
                    _m_imageGroupPreview.setInfo(_m_msgInfo.imageGroupId, wnd.chatImageGroupUIPathId);
                }

                if (_m_msgInfo.msgType == EConsortChatMsgType.Reward)
                {
                    if (_m_msgInfo is ConsortChatMsgRewardInfo rewardMsg)
                    {
                        if (rewardMsg.dialogueRef != null)
                        {
                            if (_m_rewardContainer != null)
                            {
                                _m_rewardContainer.showItemList(rewardMsg.dialogueRef.reward_item_list);
                                _m_rewardContainer.showWnd();
                            }

                            ALUGUICommon.setLabelTxt(wnd.txtIntimacy, rewardMsg.dialogueRef.reward_intimacy);
                            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.getChatTimeShow(rewardMsg.getRewardTimeMs));
                            rewardMsg.tryReqGetReward();
                        }
                    }
                }
                else if (_m_msgInfo.msgType == EConsortChatMsgType.Time)
                {
                    if (_m_msgInfo is ConsortChatMsgTimeInfo timeMsg)
                        ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.getChatTimeShow(timeMsg.getTimeMs));
                }

                if (_m_msgInfo.showTyping)
                {
                    ALUGUICommon.setGameObjEnable(wnd.typingShowGos, true);
                    ALUGUICommon.setGameObjEnable(wnd.typingHideGos, false);
                    _m_typingSer = ALSerializeOpMgr.next();
                    _refreshTyping(_m_typingSer);
                }
                else
                {
                    _m_typingSer = ALSerializeOpMgr.next();
                    ALUGUICommon.setGameObjEnable(wnd.typingShowGos, false);
                    ALUGUICommon.setGameObjEnable(wnd.typingHideGos, true);
                }
            }
            
            _m_canGetRealHeight = true;
            _m_realHeight = _m_fHeight;
            RectTransform originRectTransform = wnd.transform as RectTransform;
            if (wnd.txtText != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.txtText.rectTransform);
            
            if (originRectTransform != null)
            {
                _m_realHeight = originRectTransform.rect.height;
                _refreshHeight();
            }
        }

        #region Typing 输入中

        private int _m_typingSer;
        private int _m_typingCount;

        /// <summary>
        /// 刷新输入中
        /// </summary>
        private void _refreshTyping(int _timeDownSer)
        {
            if (null == wnd)
                return;
            if (null == _m_msgInfo)
                return;

            if (_timeDownSer != _m_typingSer)
                return;
            string typingStr = "";
            _m_typingCount++;
            _m_typingCount %= 6;
            for (int i = 0; i < _m_typingCount; i++)
            {
                typingStr += ".";
            }
            ALUGUICommon.setLabelTxt(wnd.txtTyping,  typingStr);
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTyping(_timeDownSer);
            },wnd.typingLoopDelay);
        }

        #endregion
      
        /// <inheritdoc/>
        protected override Transform _getParentTransForm()
        {
            return _m_parent;
        }

        public override float getHeight()
        {
            // 如果已经取到实际高度了，则直接返回实际高度
            if (_m_canGetRealHeight)
            {
                return _m_realHeight;
            }
            if (_m_fHeight > 0)
                return _m_fHeight;

            if (_originMono == null)
                _m_fHeight = 0;
            else
            {
                // 如果两个模板有一个没取到，就返回0
                if (_originMono == null)
                    _m_fHeight = 0;
                else
                {
                    // 获取rectTransform
                    RectTransform originRectTransform = _originMono.transform as RectTransform;
                    if (originRectTransform != null)
                        _m_fHeight = originRectTransform.rect.height;
                }
            }
            return _m_fHeight;
        }
        
        /// <summary>
        /// 加载模板对象
        /// </summary>
        public void loadTemplate()
        {
            _m_iTemplateLoadSerialize = ALSerializeOpMgr.next();
            if(_m_msgInfo == null)
                return;
            long uiPathId = _m_msgInfo.uiPathId;
            int serialize = _m_iTemplateLoadSerialize;
            _m_templateLoadCounter.resetAll();
            _m_templateLoadCounter.chgTotalStepCount(1);
            _m_templateLoadCounter.regAllDoneDelegate(_refreshHeight);
            // _loadAdditionTemplate(_m_templateLoadCounter);
            ConsortChatMsgCacheMgr.instance.getTemplate<GGUIMonoConsortChatMsgListItem>(uiPathId, (_template) =>
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
            // _discardAdditionTemplate();
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
            if (_m_msgInfo == null)
                return;
            // 从缓存池中加载mono
            ConsortChatMsgCacheMgr.instance.popItem<GGUIMonoConsortChatMsgListItem>(_m_msgInfo.uiPathId, _getParentTransForm(), _loadDone);
        }
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
            if (_m_monoWnd != null && _m_msgInfo != null)
                ConsortChatMsgCacheMgr.instance.pushBackItem(_m_msgInfo.uiPathId, _m_monoWnd.gameObject);

            _m_monoWnd = default;
        }
        // 缓存池加载完成之后的处理
        private void _loadDone(GGUIMonoConsortChatMsgListItem _mono)
        {
            // 判断是否已经被discard了
            if (_m_lLoadSerialize == -1)
            {
                // 如果已经被discard了，就尝试回收这个对象
                if (_mono != null && _m_msgInfo != null)
                    ConsortChatMsgCacheMgr.instance.pushBackItem(_m_msgInfo.uiPathId, _mono.gameObject);
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
                Debug.LogError_EditorOnly("_AGUISubWndChatMsgListItem.assetPath在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return string.Empty;
            }
        }

        protected sealed override string _monoObjName
        {
            get
            {
                Debug.LogError_EditorOnly("_AGUISubWndChatMsgListItem.objName在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return string.Empty;
            }
        }

        protected sealed override _AALResourceCore _resourceCore
        {
            get
            {
                Debug.LogError_EditorOnly("_AGUISubWndChatMsgListItem.resourceCore在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return null;
            }
        }
        #endregion
        
        public static float getHeight(Text _text, string _value, float maxTextWidth)
        {
            if (_text == null)
                return 0;

            TextGenerator tg = _text.cachedTextGeneratorForLayout;

            TextGenerationSettings ts = _text.GetGenerationSettings(new Vector2(maxTextWidth, 0.0f));
            return tg.GetPreferredHeight(_value, ts);
        }
    }
}
