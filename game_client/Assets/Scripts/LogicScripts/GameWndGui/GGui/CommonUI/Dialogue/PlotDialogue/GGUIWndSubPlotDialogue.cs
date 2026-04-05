using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 剧情对话附加窗口
    /// </summary>
    public class GGUIWndSubPlotDialogue : _ATALBasicUISubWnd<GGUIMonoSubPlotDialogue>
    {
        private static _AShowCaseUnitInfoObj[] _g_lEmptyUnitList = new _AShowCaseUnitInfoObj[0];//空列表，用于showcase只加载舞台
        private NPDialogueRefObj _m_refObj;//对话配置
        private NPDialogueSentenceRefObj _m_curSentenceRef;//当前句子配置
        private NPGGUIWndDialogueOptionContainer _m_wOptionContainer;//回应选项容器
        private NPGGUIWndCommonShowCase _m_wShowcaseWnd;//对话演出showcase
        private long _m_lShowSerial;//显示序列号
        private Action _m_onShowLastSentence;//开始展示最后一句回调
        private Action _m_aDoneAction;//对话结束回调
        private List<NPGGUIWndDialogBox> _m_lDialogBoxHistoryList;//对话历史列表
        private List<GGUIWndPlotDialogueOptionItem> _m_lDialogOptionHistoryList;//对话选项历史列表
        private NPGGUIWndDialogBox _m_curShowDialogItem;//当前正在展示的对话item
        private ALProcess _m_pProcessObj;//表现步骤管理对象
        private bool _m_bIsHideList;//是否隐藏列表
        private List<long> _m_lAudioInstanceIdList;//音效资源实例id列表

        public GGUIWndSubPlotDialogue(GGUIMonoSubPlotDialogue _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            DialogAudioMgr.instance.regNowDialogWnd(wnd);
        }

        protected override void _onHideWnd()
        {
            _m_refObj = null;

            _m_wShowcaseWnd?.hideWnd();

            _m_lShowSerial = ALSerializeOpMgr.next();

            _m_curShowDialogItem = null;

            //重置对话历史
            _resetDialogueHistory();

            _m_bIsHideList = false;

            if (_m_lAudioInstanceIdList != null)
            {
                for (int i = 0; i < _m_lAudioInstanceIdList.Count; i++)
                {
                    PlayAudioMgr.instance.stopClip(_m_lAudioInstanceIdList[i]);
                }
                _m_lAudioInstanceIdList.Clear();
            }
            
            DialogAudioMgr.instance.unRegNowDialogWnd(wnd);
        }

        protected override void _onReset()
        {
            _m_refObj = null;

            _m_aDoneAction = null;
            _m_onShowLastSentence = null;

            _m_curShowDialogItem = null;

            _m_wOptionContainer?.resetWnd();

            _m_wShowcaseWnd?.resetWnd();

            _m_lShowSerial = ALSerializeOpMgr.next();

            if (_m_lDialogBoxHistoryList != null)
            {
                for (int i = 0; i < _m_lDialogBoxHistoryList.Count; i++)
                {
                    _m_lDialogBoxHistoryList[i].resetWnd();
                }
            }

            if (_m_lDialogOptionHistoryList != null)
            {
                for (int i = 0; i < _m_lDialogOptionHistoryList.Count; i++)
                {
                    _m_lDialogOptionHistoryList[i].resetWnd();
                }
            }

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
            _m_refObj = null;

            _m_aDoneAction = null;
            _m_onShowLastSentence = null;

            _m_curShowDialogItem = null;

            if (_m_wOptionContainer != null)
            {
                _m_wOptionContainer.onSelectItem -= _onSelectOptionItem;
                _m_wOptionContainer.discard();
            }
            _m_wOptionContainer = null;

            _m_wShowcaseWnd?.discard();
            _m_wShowcaseWnd = null;

            _m_lShowSerial = ALSerializeOpMgr.next();

            if (_m_lDialogBoxHistoryList != null)
            {
                for (int i = 0; i < _m_lDialogBoxHistoryList.Count; i++)
                {
                    _m_lDialogBoxHistoryList[i].discard();
                }
                _m_lDialogBoxHistoryList.Clear();
                _m_lDialogBoxHistoryList = null;
            }

            if (_m_lDialogOptionHistoryList != null)
            {
                for (int i = 0; i < _m_lDialogOptionHistoryList.Count; i++)
                {
                    _m_lDialogOptionHistoryList[i].discard();
                }
                _m_lDialogOptionHistoryList.Clear();
                _m_lDialogOptionHistoryList = null;
            }

            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;

            _m_bIsHideList = false;

            _m_lAudioInstanceIdList?.Clear();
            _m_lAudioInstanceIdList = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.uncombineBtnClick(wnd.btnNextEx, _onClickNext);
            ALUGUICommon.uncombineBtnClick(wnd.btnShowDialogue, _onClickShowAndHideDialogue);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lAudioInstanceIdList = new List<long>();

            //选项容器
            if (wnd.monoOptionContainer != null)
            {
                _m_wOptionContainer = new NPGGUIWndDialogueOptionContainer(wnd.monoOptionContainer);
                _m_wOptionContainer.onSelectItem += _onSelectOptionItem;
                _m_wOptionContainer.hideWnd();
            }

            //对话演出showcase
            if (wnd.monoShowcase != null)
                _m_wShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowcase);

            _m_lDialogBoxHistoryList = new List<NPGGUIWndDialogBox>();
            _m_lDialogOptionHistoryList = new List<GGUIWndPlotDialogueOptionItem>();

            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.combineBtnClick(wnd.btnNextEx, _onClickNext);
            ALUGUICommon.combineBtnClick(wnd.btnShowDialogue, _onClickShowAndHideDialogue);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void playAudio(long _audioId)
        {
            long audioInstanceId = PlayAudioMgr.instance.playClip(_audioId);
            _m_lAudioInstanceIdList?.Add(audioInstanceId);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        /// <param name="_doneAction"></param>
        public void setInfo(NPDialogueRefObj _refObj, Action _onShowLastSentence, Action _doneAction = null)
        {
            if (wnd == null || _refObj == null)
                return;

            //设置数据
            _m_lShowSerial = ALSerializeOpMgr.next();
            _m_refObj = _refObj;
            _m_onShowLastSentence = _onShowLastSentence;
            _m_aDoneAction = _doneAction;
            _m_bIsHideList = false;
            _m_curShowDialogItem = null;
            //先隐藏选项
            _m_wOptionContainer?.hideWnd();
            //重置对话历史
            _resetDialogueHistory();

            //重置动画
            wnd.clickShowDialogueBtnAni?.sample(EHeroRecommendDialogueAniType.SHOW, 1);
            //隐藏打开对话按钮
            wnd.aniShowDialogueBtn?.sample(0);
            //列表移动到顶部
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd != null && wnd.chatHistoryScrollRect != null)
                    wnd.chatHistoryScrollRect.verticalNormalizedPosition = 1;
            });

            //刷新窗口，显示第一句话
            _refreshWnd();
            _dealShowSentence(_m_refObj.start_sentence_id);
            _refreshHaveOptionState(false);
        }

        /// <summary>
        /// 设置对话全部展示，有选项的话默认选第一个
        /// </summary>
        /// <param name="_isShowRecordBtn">是否展示隐藏记录按钮</param>
        /// <param name="_isShowDialogueList">是否默认显示记录</param>
        public void setInfoAndDisplayAll(NPDialogueRefObj _refObj, bool _isShowRecordBtn, bool _isShowDialogueList, Action _onShowLast, Action _doneAction = null)
        {
            if (wnd == null || _refObj == null)
                return;

            //设置数据
            _m_lShowSerial = ALSerializeOpMgr.next();
            _m_refObj = _refObj;
            _m_onShowLastSentence = _onShowLast;
            _m_aDoneAction = _doneAction;
            _m_curSentenceRef = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(_m_refObj.start_sentence_id);
            _m_curShowDialogItem = null;
            //先隐藏选项
            _m_wOptionContainer?.hideWnd();
            //重置对话历史
            _resetDialogueHistory();
            //是否展示按钮
            wnd.aniShowDialogueBtn?.sample(_isShowRecordBtn ? 1 : 0);

            //直接设置全部对话列表
            NPDialogueSentenceRefObj lastSentenceRefObj = _setDisplayAll();
            //是否隐藏列表
            _m_bIsHideList = !_isShowDialogueList;
            if (_m_bIsHideList)
                wnd.clickShowDialogueBtnAni?.sample(EHeroRecommendDialogueAniType.HIDE, 1);
            else
                wnd.clickShowDialogueBtnAni?.sample(EHeroRecommendDialogueAniType.SHOW, 1);

            //列表移动到底部
            ALCommonActionMonoTask.addNextFrameTask(()=>ALCommonActionMonoTask.addNextFrameTask(_listMoveToBottom));

            //刷新窗口
            _refreshWnd();
            _refreshHaveOptionState(false);

            //显示第一句话的效果
            long showSerial = _m_lShowSerial;
            _process2_showCaseLoad(showSerial, () =>
            {
                _process3_preEffect(showSerial, ()=>
                {
                    _process4_beginEffect();
                    if(showSerial == _m_lShowSerial)
                        _m_curSentenceRef = lastSentenceRefObj;
                });
            });

            //执行回调
            _onShowLastSentence(false);
            _onDialogueEnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_refObj == null)
                return;

            //showcase只加载舞台，加载通过effect实现
            if (wnd.monoShowcase != null)
                wnd.monoShowcase.cameraType = _m_refObj.is_main_node ? EShowcaseCameraType.MainCamera : EShowcaseCameraType.RTCamera;

            _m_wShowcaseWnd?.showWnd(_g_lEmptyUnitList, _m_refObj.showcase_index);
        }

        //刷新是否有选项显隐状态
        private void _refreshHaveOptionState(bool _haveOption)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goHaveOptionHideList, !_haveOption);
            ALUGUICommon.setGameObjEnable(wnd.goHaveOptionShowList, _haveOption);
        }

        /// <summary>
        /// 设置展示所有对话，返回最后一个对话数据
        /// </summary>
        /// <returns></returns>
        private NPDialogueSentenceRefObj _setDisplayAll()
        {
            //添加第一个对话
            NPDialogueSentenceRefObj tempDialogueSentenceRefObj = _m_curSentenceRef;
            _addSentenceToHistory(tempDialogueSentenceRefObj, true, null);

            //遍历添加后面所有对话
            long nextSentenceId = tempDialogueSentenceRefObj.getNextId(0);
            long dialogueCount = 0;
            while (nextSentenceId > 0 && dialogueCount < 100)
            {
                tempDialogueSentenceRefObj = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(nextSentenceId);
                if (tempDialogueSentenceRefObj == null)
                    break;

                //加入列表
                _addSentenceToHistory(tempDialogueSentenceRefObj, true, null);

                //如果有选项，默认取第一个加入对话列表
                if (tempDialogueSentenceRefObj.responseOptions != null && tempDialogueSentenceRefObj.responseOptions.Count > 0)
                {
                    NPDialogueResponseOptionRefObj selectResponseRef = tempDialogueSentenceRefObj.responseOptions.GetFirst();
                    _addSentenceToHistory(selectResponseRef, null);
                }

                nextSentenceId = tempDialogueSentenceRefObj.getNextId(0);
                dialogueCount++;//限制最多100条避免死循环
            }

            return tempDialogueSentenceRefObj;
        }

        //处理展示流程
        private void _dealShowSentence(long _refId)
        {
            long showSerial = _m_lShowSerial;
            _checkIsLastSentence(_refId);
            _m_curSentenceRef = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(_refId);
            _m_pProcessObj = ALProcess.CreateProcess("plot_dialogue");
            _m_pProcessObj
                .addDelegateProcess(_onDone=>_process1_lastEndEffect(showSerial, _onDone))//执行上个句子结束效果
                .addDelegateProcess(_onDone=>_process2_showCaseLoad(showSerial, _onDone))//加载showcase
                .addDelegateProcess(_onDone=>_process3_preEffect(showSerial, _onDone))//执行预效果
                .addProcess(_process4_beginEffect)//执行开始效果
                .addDelegateProcess(_onDone=>_process5_addHistoryList(showSerial, _onDone))//加入对话历史列表
                .deal();
        }

        //执行上个句子结束效果
        private void _process1_lastEndEffect(long serial, Action _onDone)
        {
            if (_m_lShowSerial != serial)
            {
                _onDone?.Invoke();
                return;
            }

            //执行结束效果，并延迟结束
            if (_m_curSentenceRef != null)
            {
                _m_curSentenceRef.endEffect?.dealEffect();
                ALCommonTaskController.CommonActionAddMonoTask(_onDone, _m_curSentenceRef.end_delay);
            }
            else
                _onDone?.Invoke();
        }

        //加载showcase
        private void _process2_showCaseLoad(long serial, Action _onDone)
        {
            if (_m_lShowSerial != serial)
            {
                _onDone?.Invoke();
                return;
            }

            //部分effect依赖showcase，所以需要先加载showcase舞台再执行效果
            if (_m_wShowcaseWnd != null)
                _m_wShowcaseWnd.regInitDoneDelegate(_onDone);
            else
                _onDone?.Invoke();
        }

        //执行预效果
        private void _process3_preEffect(long serial, Action _onDone)
        {
            if (_m_lShowSerial != serial || _m_curSentenceRef == null)
            {
                _onDone?.Invoke();
                return;
            }

            //执行预效果
            _m_curSentenceRef.preEffect?.dealEffect();

            //延迟开始
            ALCommonTaskController.CommonActionAddMonoTask(_onDone, _m_curSentenceRef.start_delay);
        }

        //执行开始效果
        private void _process4_beginEffect()
        {
            if (_m_curSentenceRef == null)
                return;

            //执行开始效果
            _m_curSentenceRef.beginEffect?.dealEffect();
        }

        //加入对话历史列表
        private void _process5_addHistoryList(long serial, Action _onDone)
        {
            if (_m_lShowSerial != serial || _m_curSentenceRef == null)
            {
                _onDone?.Invoke();
                return;
            }

            if (_m_curSentenceRef.plot_dialog_ui_path_id <= 0)
            {
                Debug.LogError_EditorOnly($"!!!!有语句没有配置plot_dialog_ui_path_id资源对话框：{_m_curSentenceRef.id}");
                _onDone?.Invoke();
                return;
            }

            _addSentenceToHistory(_m_curSentenceRef, false, _onDone);
        }

        /// <summary>
        /// 检查是否是最后一句
        /// </summary>
        /// <param name="_sentenceId"></param>
        private void _checkIsLastSentence(long _sentenceId)
        {
            NPDialogueSentenceRefObj sentenceRef = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(_sentenceId);
            if (sentenceRef == null || (sentenceRef.getNextId(0) <= 0 && (sentenceRef.responseOptions == null || sentenceRef.responseOptions.Count == 0)))
                _onShowLastSentence(true);
        }

        /// <summary>
        /// 展示最后一句对话
        /// </summary>
        private void _onShowLastSentence(bool _needPlayAni)
        {
            if (_m_onShowLastSentence == null)
                return;

            //是否播放隐藏对话按钮的显示动画
            if(_needPlayAni)
                wnd?.aniShowDialogueBtn?.forcePlay();
            Action onShowLast = _m_onShowLastSentence;
            _m_onShowLastSentence = null;
            onShowLast.Invoke();
        }

        /// <summary>
        /// 对话结束
        /// </summary>
        private void _onDialogueEnd()
        {
            if (_m_aDoneAction == null)
                return;

            Action onDone = _m_aDoneAction;
            _m_aDoneAction = null;
            onDone.Invoke();
        }

        /// <summary>
        /// 重置对话历史
        /// </summary>
        private void _resetDialogueHistory()
        {
            if (_m_lDialogBoxHistoryList != null)
            {
                for (int i = 0; i < _m_lDialogBoxHistoryList.Count; i++)
                {
                    _m_lDialogBoxHistoryList[i].discard();
                }
                _m_lDialogBoxHistoryList.Clear();
            }

            if (_m_lDialogOptionHistoryList != null)
            {
                for (int i = 0; i < _m_lDialogOptionHistoryList.Count; i++)
                {
                    _m_lDialogOptionHistoryList[i].discard();
                }
                _m_lDialogOptionHistoryList.Clear();
            }
        }

        //列表移动到底部
        private void _listMoveToBottom()
        {
            if (wnd == null || wnd.chatHistoryScrollRect == null || wnd.goChatHistoryParent == null)
                return;

            //设置content位置到底部
            RectTransform scrollRectRectTransform = (RectTransform)wnd.chatHistoryScrollRect.transform;
            RectTransform contentRectTransform = (RectTransform)wnd.goChatHistoryParent.transform;
            if (scrollRectRectTransform != null && contentRectTransform != null)
            {
                float contentY = contentRectTransform.rect.height - scrollRectRectTransform.rect.height;
                if (contentY < 0)
                    contentY = 0;
                contentRectTransform.localPosition = new Vector3(contentRectTransform.localPosition.x, contentY);
            }
        }

        #region 加入对话历史列表中

        /// <summary>
        /// 加入对话历史列表中
        /// </summary>
        /// <param name="_sentenceRefObj"></param>
        private void _addSentenceToHistory(NPDialogueSentenceRefObj _sentenceRefObj, bool _isSetFinal, Action _onDone)
        {
            if (wnd == null || _sentenceRefObj == null)
            {
                _onDone?.Invoke();
                return;
            }

            if (_m_lDialogBoxHistoryList == null)
                _m_lDialogBoxHistoryList = new List<NPGGUIWndDialogBox>();

            NPGGUIWndDialogBox itemWnd = new NPGGUIWndDialogBox(_sentenceRefObj.plot_dialog_ui_path_id, wnd.goChatHistoryParent);
            _m_curShowDialogItem = itemWnd;
            itemWnd.load(() =>
            {
                itemWnd.showWnd();
                if (_isSetFinal)
                {
                    itemWnd.playSentence(_sentenceRefObj, null, null, _listMoveToBottom);
                    itemWnd.setSentenceShowAllContentText();
                    
                    _onDone?.Invoke();
                }
                else
                    itemWnd.playSentence(_sentenceRefObj, ()=>
                    {
                        _m_curShowDialogItem = null;
                        
                        //列表移动到底部
                        if (wnd != null && wnd.chatHistoryScrollRect != null)
                            wnd.chatHistoryScrollRect.verticalNormalizedPosition = 0;
                        _onDone?.Invoke();
                    }, 
                    null,
                    _listMoveToBottom);

                //列表移动到底部
                if (wnd != null && wnd.chatHistoryScrollRect != null)
                    wnd.chatHistoryScrollRect.verticalNormalizedPosition = 0;
            });
            _m_lDialogBoxHistoryList.Add(itemWnd);
        }

        /// <summary>
        /// 加入对话历史列表中
        /// </summary>
        /// <param name="_optionRefObj"></param>
        private void _addSentenceToHistory(NPDialogueResponseOptionRefObj _optionRefObj, Action _onDone)
        {
            if (wnd == null || _optionRefObj == null)
            {
                _onDone?.Invoke();
                return;
            }

            if (_m_lDialogOptionHistoryList == null)
                _m_lDialogOptionHistoryList = new List<GGUIWndPlotDialogueOptionItem>();

            GGUIWndPlotDialogueOptionItem itemWnd = new GGUIWndPlotDialogueOptionItem(_optionRefObj.plot_dialog_ui_path_id, wnd.goChatHistoryParent);
            itemWnd.load(() =>
            {
                itemWnd.showWnd();
                itemWnd.setInfo(_optionRefObj.option_desc);

                //列表移动到底部
                if (wnd != null && wnd.chatHistoryScrollRect != null)
                    wnd.chatHistoryScrollRect.verticalNormalizedPosition = 0;

                if (_onDone != null)
                    _onDone();
            });
            _m_lDialogOptionHistoryList.Add(itemWnd);
        }

        #endregion


        #region 点击事件

        /// <summary>
        /// 点击展示下一句
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickNext(GameObject _go)
        {
            if (_m_curSentenceRef == null)
            {
                _onDialogueEnd();
                return;
            }

            //如果句子展示正在进行中则增加显示序列,直接设置对话item显示全部内容
            if (_m_pProcessObj != null && _m_pProcessObj.isRunning)
            {
                if (_m_curShowDialogItem != null)
                {
                    _m_lShowSerial = ALSerializeOpMgr.next();
                    _m_curShowDialogItem.setSentenceShowAllContentText();
                    _m_curShowDialogItem = null;
                }
            }
            else
            {
                //有选项，显示选项子窗体；选择回应回调中执行句子切换
                if (_m_wOptionContainer != null && _m_curSentenceRef.responseOptions != null &&
                    _m_curSentenceRef.responseOptions.Count > 0)
                {
                    _m_wOptionContainer.showWnd();
                    _m_wOptionContainer.showItemList(_m_curSentenceRef.responseOptions);
                    
                    _refreshHaveOptionState(true);
                }
                //无选项，直接执行句子切换
                else
                {
                    long nextSentenceId = _m_curSentenceRef.getNextId(0);
                    if (nextSentenceId <= 0)
                        _onDialogueEnd();
                    else
                        _dealShowSentence(nextSentenceId);

                    _refreshHaveOptionState(false);
                }
            }
        }

        /// <summary>
        /// 点击了选项
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onSelectOptionItem(NPGGUIWndDialogueOptionItem _itemWnd)
        {
            if (_itemWnd == null || _itemWnd.optionRef == null || _m_curSentenceRef == null)
                return;

            //选择选项之后隐藏选项窗体
            _m_wOptionContainer?.hideWnd();
            //加入对话记录列表
            _addSentenceToHistory(_itemWnd.optionRef, () =>
            {
                //播放对应选项的下一个对话id
                long nextId = _m_curSentenceRef.getNextId(_itemWnd.optionIndex);
                if (nextId <= 0)
                {
                    _onShowLastSentence(true);
                    _onDialogueEnd();
                }
                else
                    _dealShowSentence(nextId);

                _refreshHaveOptionState(false);
            });
        }

        /// <summary>
        /// 点击显示隐藏对话
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickShowAndHideDialogue(GameObject _go)
        {
            if (wnd == null || wnd.clickShowDialogueBtnAni == null)
                return;

            _m_bIsHideList = !_m_bIsHideList;
            if (!_m_bIsHideList)
                _listMoveToBottom();
            wnd.clickShowDialogueBtnAni.forcePlay(_m_bIsHideList
                ? EHeroRecommendDialogueAniType.HIDE
                : EHeroRecommendDialogueAniType.SHOW);
        }

        #endregion
    }


}