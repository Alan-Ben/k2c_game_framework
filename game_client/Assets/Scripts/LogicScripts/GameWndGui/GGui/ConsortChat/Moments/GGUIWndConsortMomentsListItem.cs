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
    public class GGUIWndConsortMomentsListItem : _ATALUGUIWndVerticalMultiSizeItem<GGUIMonoConsortMomentsListItem>
    {
        private ConsortChatMsgMomentInfo _m_msgInfo;
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
        private GGUIMonoConsortMomentsListItem _m_originMono;
        /// <summary>
        /// 这个item的模板mono
        /// </summary>
        /// <remarks>
        /// <b>特别注意：这个属性是缓存池中的模板，请不要销毁这个mono</b>
        /// </remarks>
        protected GGUIMonoConsortMomentsListItem _originMono { get { return _m_originMono != null ? _m_originMono : wnd; } }
        
        /// <summary>
        /// 当item高度发生变化的事件
        /// </summary>
        public event Action onHeightChg { add { _m_aOnHeightChg += value; } remove { _m_aOnHeightChg -= value; } }
        
        
        private NPGGUIWndCommonToggleEx _m_toggleLike;
        private NPGGUIWndCommonToggleEx _m_toggleComment;
        private GGUIWndConsortMomentsCommentContainer _m_commentContainer;
        private GGUIWndMomentsImageItemContainer _m_imageContainer;
        private GGUIWndConsortIconItem _m_consortCardItem;
        
        private Action<long> _m_onSelectMomentComment;

        private bool _m_canGetRealHeight = false;
        private float _m_realHeight;
        
        
        public GGUIWndConsortMomentsListItem(_AConsortChatMsgInfo _msgInfo, Transform _parent, Action<long> _onSelectMomentComment)
        {
            _m_msgInfo = _msgInfo as ConsortChatMsgMomentInfo;
            _m_parent = _parent;
            _m_onSelectMomentComment = _onSelectMomentComment;
            _m_templateLoadCounter = new ALStepCounter();
        }
        
        protected override void _onShowWnd()
        {
            refreshWnd();
            if (_m_msgInfo != null && _m_msgInfo.momentSaver != null)
            {
                _m_msgInfo.momentSaver.onMomentDataChange += refreshWnd;
            }
        }

        protected override void _onHideWnd()
        {
            if (_m_msgInfo != null && _m_msgInfo.momentSaver != null)
            {
                _m_msgInfo.momentSaver.onMomentDataChange -= refreshWnd;
            }
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_toggleLike?.discard();
            _m_toggleLike = null;
            _m_toggleComment?.discard();
            _m_toggleComment = null;
            _m_commentContainer?.discard();
            _m_commentContainer = null;
            _m_imageContainer?.discard();
            _m_imageContainer = null;
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if(null == wnd)
                return;
            if (null != wnd.toggleLike)
            {
                _m_toggleLike = new NPGGUIWndCommonToggleEx(wnd.toggleLike);
                _m_toggleLike.clickDelegate += _onToggleLikeClick;
                _m_toggleLike.setSelected(false);
                _m_toggleLike.showWnd();
            }
            if (null != wnd.toggleComment)
            {
                _m_toggleComment = new NPGGUIWndCommonToggleEx(wnd.toggleComment);
                _m_toggleComment.clickDelegate += _onToggleCommentClick;
                _m_toggleComment.setSelected(false);
                _m_toggleComment.showWnd();
            }

            if (null != wnd.commentContainer)
            {
                _m_commentContainer = new GGUIWndConsortMomentsCommentContainer(wnd.commentContainer);
            }

            if (wnd.imageContainer != null)
            {
                _m_imageContainer = new GGUIWndMomentsImageItemContainer(wnd.imageContainer);
            }
            if (wnd.consortIconItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);
        }
        
        public void refreshWnd()
        {
            if (wnd == null || _m_msgInfo == null) 
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtContent, _m_msgInfo.getContent());
            ALUGUICommon.setLabelTxt(wnd.txtLikePlayers, _m_msgInfo.getLikePlayers());
            
            ALUGUICommon.setLabelTxt(wnd.txtTime, _m_msgInfo.getMomentTime());


            if(_m_msgInfo.momentSaver == null)
                return;
            
            if (_m_toggleLike != null) _m_toggleLike.setSelected(_m_msgInfo.momentSaver.isPlayerLike());
            if (_m_toggleComment != null) _m_toggleComment.setSelected(_m_msgInfo.momentSaver.isPlayerComment());
            ALUGUICommon.setGameObjEnable(wnd.hasLikeShowGos, _m_msgInfo.momentSaver.isAnyLike());
            
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_msgInfo.momentSaver.momentData.consortId);
            _m_consortCardItem?.showWnd();
            _m_consortCardItem?.setInfo(consortInfo, 0);
            
            if (_m_commentContainer != null)
            {
                if (_m_msgInfo.momentSaver.commentList != null && _m_msgInfo.momentSaver.commentList.Count > 0)
                {
                    _m_commentContainer.showWnd();
                    _m_commentContainer.showItemList(_m_msgInfo.momentSaver.commentList);
                    ALUGUICommon.setGameObjEnable(wnd.hasCommentShowGos, true);

                }
                else
                {
                    _m_commentContainer.hideWnd();
                    ALUGUICommon.setGameObjEnable(wnd.hasCommentShowGos, false);
                }
            }

            if (_m_imageContainer != null)
            {

                _m_imageContainer.showWnd();
                if (_m_msgInfo.momentSaver.momentData.imageList == null || _m_msgInfo.momentSaver.momentData.imageList.Count <= 0)
                {
                    ConsortMomentsBgGroupRefObj bgGroupRef = GRefdataCoreMgr.instance.consortMomentsBgGroupRefCore.refList.GetRandomItem();
                    ConsortMomentsConsortGroupRefObj consortGroupRef = GRefdataCoreMgr.instance.consortMomentsConsortGroupRefCore.refList.GetRandomItem();


                    int count = GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_image_random_count.getRandomValue();
                    List<ConsortMomentImageData> imageDataList = new List<ConsortMomentImageData>();

                    List<EConsortChatShotType> shotTypeList = new List<EConsortChatShotType>();
                    _refreshShotTypeList(ref shotTypeList);
                    List<long> bgImgIdList = new List<long>();
                    List<long> actorImgIdList = new List<long>();
                    bgImgIdList.AddRange(bgGroupRef.bg_img_id_list);
                    actorImgIdList.AddRange(consortGroupRef.consort_img_id_list);

                    for (int i = 0; i < count; i++)
                    {
                        if (shotTypeList.Count <= 0)
                            _refreshShotTypeList(ref shotTypeList);
                        if (count == 1 && i == 0)
                            shotTypeList.Remove(EConsortChatShotType.EmptyShot);
                        
                        var type = shotTypeList.GetRandomItemAndRemove();
                        if (bgImgIdList.Count <= 0)
                            bgImgIdList.AddRange(bgGroupRef.bg_img_id_list);
                        if (actorImgIdList.Count <= 0)
                            actorImgIdList.AddRange(consortGroupRef.consort_img_id_list);

                        long actorId = actorImgIdList.GetRandomItemAndRemove();
                        long bgId = bgImgIdList.GetRandomItemAndRemove();
                        imageDataList.Add(new ConsortMomentImageData(type, bgId, actorId));
                    }
                    _m_imageContainer.showItemList(imageDataList);
                }
                else
                {
                    _m_imageContainer.showItemList(_m_msgInfo.momentSaver.momentData.imageList);
                }
            }
            
            _m_canGetRealHeight = true;
            _m_realHeight = _m_fHeight;
            RectTransform originRectTransform = wnd.transform as RectTransform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(originRectTransform);
            if (originRectTransform != null)
            {
                _m_realHeight = originRectTransform.rect.height;
                _refreshHeight();
            }

            // 延迟到下一帧再检查一下高度是不是对的，不对则重新刷新高度
            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                if (originRectTransform != null)
                {
                    float tmpHeight = originRectTransform.rect.height;
                    if (Math.Abs(tmpHeight - _m_realHeight) > 0.01f)
                    {
                        _m_realHeight = tmpHeight;
                        _refreshHeight();
                    }

                }
            });
        }
        
        private static void _refreshShotTypeList([NotNull]ref List<EConsortChatShotType> _shotTypeList)
        {
            _shotTypeList.Clear();
            _shotTypeList.Add(EConsortChatShotType.LongShot);
            _shotTypeList.Add(EConsortChatShotType.MidShot);
            _shotTypeList.Add(EConsortChatShotType.ShortShot);
            _shotTypeList.Add(EConsortChatShotType.EmptyShot);
        }
        private void _onToggleLikeClick(NPGGUIWndCommonToggleEx obj)
        {
            if(_m_msgInfo != null && _m_msgInfo.momentSaver != null)
            {
                _m_msgInfo.momentSaver.changeMomentPlayerLike();
                bool isPlayerLike = _m_msgInfo.momentSaver.isPlayerLike();
                bool isAnyLike = _m_msgInfo.momentSaver.isAnyLike();
                _m_toggleLike?.setSelected(isPlayerLike);
                if (wnd != null)
                {
                    //播放点赞音效
                    if (isPlayerLike)
                        PlayAudioMgr.instance.playClip(wnd.clickLikeAudioId);

                    ALUGUICommon.setLabelTxt(wnd.txtLikePlayers, _m_msgInfo.getLikePlayers());
                    ALUGUICommon.setGameObjEnable(wnd.hasLikeShowGos, isAnyLike);
                }
            }
        }
        
        private void _onToggleCommentClick(NPGGUIWndCommonToggleEx obj)
        {
            if (_m_msgInfo != null)
            {
                _m_onSelectMomentComment?.Invoke(_m_msgInfo.msgId);
                if (_m_toggleComment != null && _m_msgInfo.momentSaver != null) _m_toggleComment.setSelected(_m_msgInfo.momentSaver.isPlayerComment());
            }
        }

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
                _m_fHeight = _originMono.defaultHeight;
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
            ConsortChatMsgCacheMgr.instance.getTemplate<GGUIMonoConsortMomentsListItem>(uiPathId, (_template) =>
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
            ConsortChatMsgCacheMgr.instance.popItem<GGUIMonoConsortMomentsListItem>(_m_msgInfo.uiPathId, _getParentTransForm(), _loadDone);
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
        private void _loadDone(GGUIMonoConsortMomentsListItem _mono)
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
                Debug.LogError_EditorOnly("_AGUISubWndMomentsListItem.assetPath在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return string.Empty;
            }
        }

        protected sealed override string _monoObjName
        {
            get
            {
                Debug.LogError_EditorOnly("_AGUISubWndMomentsListItem.objName在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return string.Empty;
            }
        }

        protected sealed override _AALResourceCore _resourceCore
        {
            get
            {
                Debug.LogError_EditorOnly("_AGUISubWndMomentsListItem.resourceCore在这里没有任何作用，因为load会根据msgType从cache里直接获取");
                return null;
            }
        }
        #endregion
    }
}
