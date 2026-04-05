using ALPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 对话主窗体
    /// </summary>
    public class NPGGUIWndDialogue : _ANPGGUIBasicWnd<NPGGUIMonoDialogue>
    {
        private static NPGGUIWndDialogue _g_instance;
        public static NPGGUIWndDialogue instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndDialogue();
                return _g_instance;
            }
        }

        //空列表，用于showcase只加载舞台
        private static _AShowCaseUnitInfoObj[] _g_lEmptyUnitList = new _AShowCaseUnitInfoObj[0];

        private NPDialogueRefObj _m_refObj;//对话配置
        private NPDialogueSentenceRefObj _m_curSentenceRef;//当前句子配置
        private NPGGUIWndDialogBox _m_wDialogWnd;//对话框子窗体
        private NPGGUIWndCommonShowCase _m_wShowcaseWnd;//对话演出showcase
        private long _m_lShowSerial;//显示序列号
        private Action _m_aDialogEndAction;//对话展示完成回调(这时对话Node还未退出)
        private Action _m_aDealCloseDialog;//关闭对话处理, 若非null, 则不会自动退出对话, 会调用这个方法, 应该由外部调用者关闭对话
        private Action<ENPClientSpecialDoalogueDealType> _m_dealEffectAction;
        private NPGGUIWndCommonTab _m_wAutoPlayTab;//自动播放按钮
        private GGUIWndSimpleComicSubWnd _m_comicSubWnd;//漫画子窗口
        private List<long> _m_lAudioInstanceIdList;//音效资源实例id列表
        private long _m_bgMusicInstanceId;//背景音乐id

        [NotNull] private Queue<int> _m_lSelectOptionIndexList;//选中的选项下标列表
        
        /// <summary>
        /// 是否正在展示对话
        /// </summary>
        public bool isShowingDialogue { get { return _m_refObj != null; } }

        private NPGGUIWndDialogue() : base(EALUIWndLayer.ADDITION)
        {
            _m_dealEffectAction = null;
        }

        protected override string _monoAssetPath { get { return NPGGUIMonoDialogue.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoDialogue.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            DialogAudioMgr.instance.regNowDialogWnd(wnd);

            _m_wAutoPlayTab?.showWnd();
            _m_wAutoPlayTab?.setEnable(true);
            _m_wAutoPlayTab?.setSelected(Game.instance.dialogueCanAutoPlay);

            enableDialogBoxParent(true);
        }

        protected override void _onHideWnd()
        {
            _m_refObj = null;
            
            _m_wDialogWnd?.hideWnd();

            _m_wShowcaseWnd?.hideWnd();

            _m_wAutoPlayTab?.hideWnd();

            _m_lShowSerial = ALSerializeOpMgr.next();

            _m_lSelectOptionIndexList.Clear();
            
            _m_comicSubWnd?.hideWnd();

            if (_m_lAudioInstanceIdList != null)
            {
                for (int i = 0; i < _m_lAudioInstanceIdList.Count; i++)
                {
                    PlayAudioMgr.instance.stopClip(_m_lAudioInstanceIdList[i]);
                }
                _m_lAudioInstanceIdList.Clear();
            }

            //恢复之前的bgm
            if(_m_bgMusicInstanceId > 0)
                PlayAudioMgr.instance.stopBackgroundMusicByInstanceID(_m_bgMusicInstanceId, true);
            _m_bgMusicInstanceId = 0;
            
            DialogAudioMgr.instance.unRegNowDialogWnd(wnd);
        }

        protected override void _onReset()
        {
            _m_refObj = null;
            _m_aDialogEndAction = null;
            _m_aDealCloseDialog = null;

            _m_wDialogWnd?.resetWnd();

            _m_wShowcaseWnd?.resetWnd();

            _m_wAutoPlayTab?.resetWnd();
            
            _m_comicSubWnd?.resetWnd();;

            _m_lShowSerial = ALSerializeOpMgr.next();
            
            if (_m_lAudioInstanceIdList != null)
            {
                for (int i = 0; i < _m_lAudioInstanceIdList.Count; i++)
                {
                    PlayAudioMgr.instance.stopClip(_m_lAudioInstanceIdList[i]);
                }
                _m_lAudioInstanceIdList.Clear();
            }
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            _m_dealEffectAction = null;
            _m_refObj = null;
            _m_aDialogEndAction = null;
            _m_aDealCloseDialog = null;

            _m_wDialogWnd?.discard();
            _m_wDialogWnd = null;

            _m_wShowcaseWnd?.discard();
            _m_wShowcaseWnd = null;

            _m_wAutoPlayTab?.discard();
            _m_wAutoPlayTab = null;

            _m_comicSubWnd?.discard();
            _m_comicSubWnd = null;

            _m_lAudioInstanceIdList?.Clear();
            _m_lAudioInstanceIdList = null;

            _m_lShowSerial = ALSerializeOpMgr.next();

            _m_lSelectOptionIndexList.Clear();
            _m_lSelectOptionIndexList = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnSkip, _onClickBtnSkip);
            ALUGUICommon.uncombineBtnClick(wnd.btnShowHistory, _onClickShowHistory);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lSelectOptionIndexList = new Queue<int>();
            _m_lAudioInstanceIdList = new List<long>();

            //对话演出showcase
            if (wnd.monoShowcase != null)
                _m_wShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowcase);

            if (wnd.monoAutoPlayTab)
            {
                _m_wAutoPlayTab = new NPGGUIWndCommonTab(wnd.monoAutoPlayTab);
                _m_wAutoPlayTab.clickDelegate += _onClickAutoPlay;
            }


            ALUGUICommon.combineBtnClick(wnd.btnSkip, _onClickBtnSkip);
            ALUGUICommon.combineBtnClick(wnd.btnShowHistory, _onClickShowHistory);
        }

        #region 点击事件

        /// <summary>
        /// 点击跳过按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnSkip(GameObject _go)
        {
            if (null == _m_curSentenceRef || null == _m_refObj)
            {
                //发送埋点-跳过对话
                GCommon.sendStepReport(TraceConst.SKIP_DIALOGUE);
                _onDialogueEnd();
                return;
            }
            
            //不能跳过点击无效
            if(_m_curSentenceRef.isMust)
                return;
            
            long sentenceId = _findToMustSentence(_m_curSentenceRef);
            
            //找不到直接关闭对话
            if (sentenceId == -1)
            {
                //先保存自动播放选择，暂停自动播放
                bool saveAutoPlaySelect = _m_wAutoPlayTab != null ? _m_wAutoPlayTab.isOn : false;
                _m_wAutoPlayTab?.setSelected(false);

                NPMesMgr.instance.showWarningTipMes(
                    ()=>
                    {
                        //发送埋点-跳过对话
                        GCommon.sendStepReport(TraceConst.SKIP_DIALOGUE.setMark($"{_m_refObj?.id},{_m_curSentenceRef?.id}"));
                        _onDialogueEnd();
                    }, 
                    () =>
                    {
                        //恢复自动播放
                        _m_wAutoPlayTab?.setSelected(saveAutoPlaySelect);
                        _onSentenceShowEnd();
                    },
                    ENPWarningType.COMMON_DIALOGUE_SKIP_CONFIRM,
                    TransKeyConst.dialogue_skip_confirm_title_none,
                    TextTranslate.instance.getLanguage(TransKeyConst.dialogue_skip_confirm_desc_none));
            }
            else
            {
                if (_m_wDialogWnd != null) 
                    _m_wDialogWnd.setSentenceStateFinal();

                _dealSentenceChg(sentenceId, true);
            }
        }

        //找到后面必须查看的的对话语句
        //返回-1代表找不到有选项的对话
        private long _findToMustSentence(NPDialogueSentenceRefObj _dialogueSentenceRef)
        {
            if (null == _dialogueSentenceRef)
                return -1;

            long nextId = _dialogueSentenceRef.getNextId(0);
            if (nextId == -1)
                return -1;
            
            NPDialogueSentenceRefObj nextRef = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(nextId);
            if (null != nextRef && !nextRef.isForceAutoSkip() && nextRef.isMust)
                return nextId;

            return _findToMustSentence(nextRef);
        }

        /// <summary>
        /// 对话框点击了下一句
        /// </summary>
        private void _onDialogClickNext()
        {
            if (_m_curSentenceRef == null)
                return;

            //有选项，显示选项子窗体；选择回应回调中执行句子切换
            if (_m_curSentenceRef.responseOptions != null && _m_curSentenceRef.responseOptions.Count > 0)
            {
                _dealShowOption(_m_curSentenceRef.responseOptions);
            }
            //无选项，直接执行句子切换
            else
            {
                _dealSentenceChg(_m_curSentenceRef.getNextId(0));
            }
        }

        /// <summary>
        /// 点击了选项
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onSelectOptionItem(NPGGUIWndDialogueOptionItem _itemWnd)
        {
            if (_itemWnd == null || _m_curSentenceRef == null)
                return;

            //记录选项下标
            _m_lSelectOptionIndexList.Enqueue(_itemWnd.optionIndex);

            //播放对应选项的下一个对话id
            _dealSentenceChg(_m_curSentenceRef.getNextId(_itemWnd.optionIndex));
        }

        /// <summary>
        /// 点击自动播放
        /// </summary>
        private void _onClickAutoPlay(bool _isSelect)
        {
            //不能跳过点击无效
            if(null != _m_curSentenceRef && _m_curSentenceRef.isMust)
                return;
            
            _m_wAutoPlayTab?.setSelected(_isSelect);
            Game.instance.dialogueCanAutoPlay = _isSelect;
            if (_isSelect && _m_wDialogWnd != null && _m_wDialogWnd.getDialogBoxState() == NPGGUIWndDialogBox.ENPDialogBoxState.END)
            {
                _m_wDialogWnd?.setSentenceStateFinal();
                _dealAutoPlay();
            }
        }

        /// <summary>
        /// 点击展示对话历史
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickShowHistory(GameObject _go)
        {
            //先保存自动播放选择，暂停自动播放
            bool saveAutoPlaySelect = _m_wAutoPlayTab != null ? _m_wAutoPlayTab.isOn : false;
            _m_wAutoPlayTab?.setSelected(false);

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDialogueHistory.instance, () =>
            {
                GGUIWndDialogueHistory.instance.showWnd();
                GGUIWndDialogueHistory.instance.setInfo(_m_refObj, _m_curSentenceRef, _m_lSelectOptionIndexList, () =>
                {
                    //恢复自动播放
                    _m_wAutoPlayTab?.setSelected(saveAutoPlaySelect);
                    _onSentenceShowEnd();
                });
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_DIALOGUE_HISTORY, false, true);
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 刷新窗体
        /// </summary>
        private void _refreshWnd()
        { 
            if (wnd == null)
            {
                return;
            }
            if (_m_refObj == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goListShowOnCanSkip, true);
                return;
            }

            enableDialogBoxParent(true);
            
            //跳过相关显示
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnCanSkip, _m_refObj.can_skip);
            //回顾相关显示
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnCanReview, _m_refObj.can_review);
            //自动播放相关显示
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnCanAutoPlay, _m_refObj.can_auto_play);

            //showcase只加载舞台，加载通过effect实现
            if(wnd.monoShowcase != null)
                wnd.monoShowcase.cameraType = _m_refObj.is_main_node ? EShowcaseCameraType.MainCamera : EShowcaseCameraType.RTCamera;

            _m_wShowcaseWnd?.showWnd(_g_lEmptyUnitList, _m_refObj.showcase_index);
        }

        /// <summary>
        /// 执行句子切换，包括当前句子的结束，以及下个句子的开始
        /// </summary>
        /// <param name="_refId"></param>
        private void _dealSentenceChg(long _refId, bool _needDealSkipEffect = false)
        {
            //记录序列号
            long showSerial = _m_lShowSerial = ALSerializeOpMgr.next();

            //执行结束效果，并延迟结束
            if (_m_curSentenceRef != null)
            {
                _m_curSentenceRef.endEffect?.dealEffect();
                ALCommonTaskController.CommonActionAddMonoTask(onEndDelayDone, _m_curSentenceRef.end_delay);
            }
            else
            {
                onEndDelayDone();
            }

            //实际结束对话
            void onEndDelayDone()
            {
                //序列号改变，不处理
                if (showSerial != _m_lShowSerial)
                    return;

                //下一句id小于0，对话结束
                if (_refId < 0)
                {
                    _onDialogueEnd();
                    return;
                }

                NPDialogueSentenceRefObj nextRef = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(_refId);
                if (nextRef == null)
                {
#if UNITY_EDITOR
                    ALLog.Error($"对话 sentence_id：{_refId} 找不到对应配置");
#endif
                    _onDialogueEnd();
                    return;
                }

                //显示下一条句子
                _showSentence(nextRef, _needDealSkipEffect);
            }
        }

        /// <summary>
        /// 显示一条句子
        /// </summary>
        /// <param name="_sentenceRefObj"></param>
        private void _showSentence(NPDialogueSentenceRefObj _sentenceRefObj, bool _needDealSkipEffect = false)
        {
            if (null == _m_refObj || _sentenceRefObj == null || wnd == null)
                return;

            _m_curSentenceRef = _sentenceRefObj;
            // 若句子可以被强制跳过，直接找它的下一句
            if (_m_curSentenceRef.isForceAutoSkip())
            {
                NPDialogueSentenceRefObj nextRefObj = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(_m_curSentenceRef.getNextId(0));
                if (nextRefObj == null)//若没有下一句了， 结束对话
                {
                    _onDialogueEnd();
                    return;
                }
                
                // 若还有后续对话, 继续显示
                _showSentence(nextRefObj, true);
                return;
            }
            
            //发送埋点-展示对话句子
            GCommon.sendStepReport(TraceConst.SHOW_DIALOGUE_SENTENCE.setMark($"{ _sentenceRefObj.id}"));
            //有配置选项的语句不显示跳过按钮
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnCanSkip, _m_refObj.can_skip && !_m_curSentenceRef.isHideSkipBtn);
            // 没配置回顾样式的语句不显示回顾按钮
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnCanReview, _m_refObj.can_review && _m_curSentenceRef.chat_history_ui_path_id > 0);
            // 没配置回顾样式的语句不显示回顾按钮
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnCanAutoPlay, _m_refObj.can_auto_play && !_m_curSentenceRef.isHideAutoPlayBtn);

            
            //有漫画直接只处理漫画
            if (_m_curSentenceRef.comic_id != 0)
            {
                _doShowComic();
            }
            else
            {
                _doShowDialog(_needDealSkipEffect);   
            }
        }

        //展示漫画的处理
        private void _doShowComic()
        {
            if (wnd == null || _m_curSentenceRef == null)
                return;
            
            if (null == _m_comicSubWnd)
            {
                _m_comicSubWnd = new GGUIWndSimpleComicSubWnd(wnd.comicParent);
                _m_comicSubWnd.load();
                _m_comicSubWnd.regSkipAndDoneAction(onComicDone, onComicDone);
            }
            
            _m_comicSubWnd.regLoadDoneDelegate(() =>
            {
                _m_comicSubWnd.showWnd();
                _m_comicSubWnd.initShowInfo(_m_curSentenceRef.comic_id);
            });
            
            //漫画结束的处理
            void onComicDone()
            {
                //隐藏窗口
                if (_m_comicSubWnd != null) 
                    _m_comicSubWnd.hideWnd();

                //切换到下一句
                if (_m_curSentenceRef != null) 
                    _dealSentenceChg(_m_curSentenceRef.getNextId(0));
            }
        }

        //处理对话的展示
        private void _doShowDialog(bool _needDealSkipEffect = false)
        {
            //记录序列号
            long showSerial = _m_lShowSerial = ALSerializeOpMgr.next();
            
            //部分effect依赖showcase，所以需要先加载showcase舞台再执行效果
            if (_m_wShowcaseWnd != null)
            {
                _m_wShowcaseWnd.regInitDoneDelegate(onShowcaseTempLoaded);
            }
            else
            {
                onShowcaseTempLoaded();
            }

            //showcase舞台加载完毕
            void onShowcaseTempLoaded()
            {
                //序列号改变，不处理
                if (showSerial != _m_lShowSerial || _m_curSentenceRef == null)
                    return;

                //执行预效果
                _m_curSentenceRef.preEffect?.dealEffect();

                //延迟开始
                ALCommonTaskController.CommonActionAddMonoTask(onStartDelayDone, _m_curSentenceRef.start_delay);

                //实际开始对话
                void onStartDelayDone()
                {
                    //序列号改变，不处理
                    if (showSerial != _m_lShowSerial || _m_curSentenceRef == null)
                        return;

                    //执行跳过效果
                    if(_needDealSkipEffect)
                        _m_curSentenceRef.skipEffect?.dealEffect();
                    //执行开始效果
                    _m_curSentenceRef.beginEffect?.dealEffect();
                    _loadDialogueBox(() =>
                    {
                        _m_wDialogWnd?.showWnd();
                        _m_wDialogWnd?.playSentence(_m_curSentenceRef, _onSentenceShowEnd, _onDialogClickNext);
                    });
                }
            }
        }
        
        /// <summary>
        /// 显示选项
        /// </summary>
        /// <param name="_optionList"></param>
        private void _dealShowOption(List<NPDialogueResponseOptionRefObj> _optionList)
        {
            _loadDialogueBox(() =>
            {
                _m_wDialogWnd?.showWnd();
                _m_wDialogWnd?.showOptionList(_optionList, _onSelectOptionItem);
            });
        }

        /// <summary>
        /// 加载对话框
        /// </summary>
        /// <param name="_onDone"></param>
        private void _loadDialogueBox(Action _onDone)
        {
            if (wnd == null || _m_curSentenceRef == null)
            {
                _onDone?.Invoke();
                return;
            }

            //资源路径id为0，表示不处理文本
            if (_m_curSentenceRef.dialog_res_path_id == 0)
            {
                Debug.LogError_EditorOnly($"!!!!有语句没有配置资源对话框：{_m_curSentenceRef.id}");
                _onDialogueEnd();
                _onDone?.Invoke();
                return;
            }

            //如果对话框样式改变了，重新加载
            if (_m_wDialogWnd == null || _m_wDialogWnd.resPathId != _m_curSentenceRef.dialog_res_path_id)
            {
                _m_wDialogWnd?.discard();
                _m_wDialogWnd = new NPGGUIWndDialogBox(_m_curSentenceRef.dialog_res_path_id, wnd.dialogParent);
                _m_wDialogWnd.load(_onDone);
            }
            //否则直接播放
            else
                _onDone?.Invoke();
        }

        /// <summary>
        /// 处理自动播放
        /// </summary>
        private void _dealAutoPlay()
        {
            if (_m_curSentenceRef == null)
                return;

            //有选项，显示选项子窗体；选择回应回调中执行句子切换
            if (_m_curSentenceRef.responseOptions != null && _m_curSentenceRef.responseOptions.Count > 0)
            {
                _dealShowOption(_m_curSentenceRef.responseOptions);
            }
            //无选项，直接执行句子切换
            else
            {
                _dealSentenceChg(_m_curSentenceRef.getNextId(0));
            }
        }

        /// <summary>
        /// 一个句子的打字机效果结束
        /// </summary>
        private void _onSentenceShowEnd()
        {
            if (wnd == null)
                return;

            //如果自动播放，延时自动展示下一句
            //或者配置了自动跳过下一句
            if ((null != _m_curSentenceRef && _m_curSentenceRef.auto_next_time_ms > 0 )
                || (_m_wAutoPlayTab != null && _m_wAutoPlayTab.isOn && _m_refObj != null && _m_refObj.can_auto_play))
            {
                float delayTime = wnd.autoPlayInterval;
                if (_m_curSentenceRef != null && _m_curSentenceRef.auto_next_time_ms > 0)
                {
                    delayTime = _m_curSentenceRef.auto_next_time_ms / 1000f;
                }
                
                long showSerial = _m_lShowSerial;
                ALCommonActionMonoTask.addMonoTask(()=>
                {
                    if (showSerial != _m_lShowSerial)
                        return;

                    _m_wDialogWnd?.setSentenceStateFinal();
                    _dealAutoPlay();
                }, delayTime);
            }
        }

        /// <summary>
        /// 对话结束
        /// </summary>
        private void _onDialogueEnd()
        {
            //发送埋点-结束对话
            if(_m_refObj != null)
                GCommon.sendStepReport(TraceConst.END_DIALOGUE.setMark($"{_m_refObj.id}"));

            Action action = _m_aDialogEndAction;
            _m_aDialogEndAction = null;
            action?.Invoke();

            if (_m_aDealCloseDialog == null)//若外部没有传入关闭对话的方法，默认关闭NPGMainQueueDialogueNode
            {
                QueueMgr.instance.forceCloseNodeByType(typeof(NPGMainQueueDialogueNode));
            }
            else//外部有传入关闭方法直接使用外部方法
            {
                _m_aDealCloseDialog.Invoke();
                _m_aDealCloseDialog = null;
            }
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        /// <param name="_dialogEndAction">对话展示完成回调(这时对话Node还未退出)</param>
        /// <param name="_needAutoPlay"></param>
        /// <param name="_dealCloseDialog">关闭对话处理, 若非null, 则不会自动退出对话, 会调用这个方法, 应该由外部调用者关闭对话</param>
        public void setInfo(NPDialogueRefObj _refObj, Action _dialogEndAction = null, bool _needAutoPlay = false, Action _dealCloseDialog = null)
        {
            if (_refObj == null)
            {
                _m_refObj = null;
                _m_aDialogEndAction = _dialogEndAction;
                _m_aDealCloseDialog = _dealCloseDialog;
                ALUGUICommon.setGameObjEnable(wnd.goListShowOnCanSkip, true);
                return;
            }
            _m_dealEffectAction = null;
            _m_refObj = _refObj;
            _m_aDialogEndAction = _dialogEndAction;
            _m_aDealCloseDialog = _dealCloseDialog;
            _m_curSentenceRef = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(_m_refObj.start_sentence_id);
            if (null == _m_curSentenceRef)
            {
#if UNITY_EDITOR
                ALLog.Error($"对话id：{_refObj.id}的start_sentence_id：{_m_refObj.start_sentence_id} 找不到对应配置");
#endif
                _onDialogueEnd();
                return;
            }
            //发送埋点-开启对话
            GCommon.sendStepReport(TraceConst.START_DIALOGUE.setMark($"{_refObj.id}"));

            _m_lSelectOptionIndexList.Clear();

            //播放开始的bgm
            if (_m_refObj.bgm_audio_id > 0)
            {
                _m_bgMusicInstanceId = PlayAudioMgr.instance.playBackgroundMusic(_m_refObj.bgm_audio_id);
            }

            _refreshWnd();
            _showSentence(_m_curSentenceRef);
            
            _onClickAutoPlay(_needAutoPlay || Game.instance.dialogueCanAutoPlay);
        }

        public void forceDialogueEnd()
        {
            _onDialogueEnd();
        }
        
        /// <summary>
        /// 显示或者隐藏对话框挂载父节点
        /// </summary>
        public void enableDialogBoxParent(bool _isEnable)
        {
            if (wnd == null || !isShow)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.dialogParent, _isEnable);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void playAudio(long _audioId)
        {
            long audioInstanceId = PlayAudioMgr.instance.playClip(_audioId);
            _m_lAudioInstanceIdList?.Add(audioInstanceId);
        }
        
        #endregion

        #region 回调相关

        /// <summary>
        /// 注册类型回调
        /// </summary>
        /// <param name="_dealAction"></param>
        public void regDealType(Action<ENPClientSpecialDoalogueDealType> _dealAction)
        {
            _m_dealEffectAction += _dealAction;
        }
        
        /// <summary>
        /// 反注册回调
        /// </summary>
        /// <param name="_dealAction"></param>
        public void unregDealType(Action<ENPClientSpecialDoalogueDealType> _dealAction)
        {
            _m_dealEffectAction -= _dealAction;
        }
        
        /// <summary>
        /// 执行类型回调
        /// </summary>
        /// <param name="_specialDealType"></param>
        public void callDealType(ENPClientSpecialDoalogueDealType _specialDealType)
        {
            
            if (null != _m_dealEffectAction)
            {
                _m_dealEffectAction.Invoke(_specialDealType);
            }
            _m_dealEffectAction = null;
        }

        #endregion

    }
}
