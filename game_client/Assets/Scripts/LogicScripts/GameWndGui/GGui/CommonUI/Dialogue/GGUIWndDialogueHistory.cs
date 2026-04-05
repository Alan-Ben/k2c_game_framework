using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 对话历史回顾窗口
    /// </summary>
    public class GGUIWndDialogueHistory : _ANPGGUIBasicWnd<GGUIMonoDialogueHistory>
    {
        private static GGUIWndDialogueHistory _g_instance;
        public static GGUIWndDialogueHistory instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndDialogueHistory();
                return _g_instance;
            }
        }

        private NPDialogueRefObj _m_dialogueRef;//对话配置数据
        private NPDialogueSentenceRefObj _m_curSentenceRef;//当前对话句子
        private Queue<int> _m_selectOptionIndexList;//选中的选项下标列表
        private List<NPGGUIWndDialogBox> _m_lDialogBoxHistoryList;//对话历史列表
        private List<GGUIWndDialogueHistoryOption> _m_lOptionHistoryList;//对话选项历史列表
        private Action _m_aOnHide;//隐藏事件

        private GGUIWndDialogueHistory() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoDialogueHistory.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoDialogueHistory.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_selectOptionIndexList?.Clear();
            _m_aOnHide?.Invoke();
        }

        protected override void _onReset()
        {
            if (_m_lDialogBoxHistoryList != null)
            {
                for (int i = 0; i < _m_lDialogBoxHistoryList.Count; i++)
                {
                    _m_lDialogBoxHistoryList[i]?.resetWnd();
                }
            }

            if (_m_lOptionHistoryList != null)
            {
                for (int i = 0; i < _m_lOptionHistoryList.Count; i++)
                {
                    _m_lOptionHistoryList[i]?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_lDialogBoxHistoryList != null)
            {
                for (int i = 0; i < _m_lDialogBoxHistoryList.Count; i++)
                {
                    _m_lDialogBoxHistoryList[i]?.discard();
                }
                _m_lDialogBoxHistoryList.Clear();
                _m_lDialogBoxHistoryList = null;
            }

            if (_m_lOptionHistoryList != null)
            {
                for (int i = 0; i < _m_lOptionHistoryList.Count; i++)
                {
                    _m_lOptionHistoryList[i]?.discard();
                }
                _m_lOptionHistoryList.Clear();
                _m_lOptionHistoryList = null;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lDialogBoxHistoryList = new List<NPGGUIWndDialogBox>();
            _m_lOptionHistoryList = new List<GGUIWndDialogueHistoryOption>();
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj">当前对话配置</param>
        /// <param name="_curSentenceRef">当前展示对话句子</param>
        /// <param name="_selectOptionIndexList">对话选项选择下标</param>
        public void setInfo(NPDialogueRefObj _refObj, NPDialogueSentenceRefObj _curSentenceRef, Queue<int> _selectOptionIndexList, Action _onHide = null)
        {
            _m_dialogueRef = _refObj;
            _m_curSentenceRef = _curSentenceRef;
            _m_aOnHide = _onHide;
            if (_selectOptionIndexList != null)
                _m_selectOptionIndexList = new Queue<int>(_selectOptionIndexList);

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_dialogueRef == null || _m_curSentenceRef == null)
                return;

            NPDialogueSentenceRefObj tempDialogueSentenceRefObj;
            long nextSentenceId = _m_dialogueRef.start_sentence_id;
            long dialogueCount = 0;
            while (nextSentenceId > 0 && dialogueCount < 100)
            {
                tempDialogueSentenceRefObj = GRefdataCoreMgr.instance.dialogueSentenceRefCore.getRef(nextSentenceId);
                if (tempDialogueSentenceRefObj == null)
                    break;

                //加入列表
                _addSentenceToHistory(tempDialogueSentenceRefObj);

                //如果有选项，将选项加入列表
                if (tempDialogueSentenceRefObj.responseOptions != null && tempDialogueSentenceRefObj.responseOptions.Count > 0 && _m_selectOptionIndexList != null && _m_selectOptionIndexList.Count > 0)
                {
                    int selectIndex = _m_selectOptionIndexList.Dequeue();

                    if (selectIndex >= tempDialogueSentenceRefObj.responseOptions.Count)
                        selectIndex = 0;

                    _addOptionToHistory(tempDialogueSentenceRefObj, selectIndex);
                    nextSentenceId = tempDialogueSentenceRefObj.getNextId(selectIndex);
                }
                else
                    nextSentenceId = tempDialogueSentenceRefObj.getNextId(0);

                //展示到当前句子就结束
                if (tempDialogueSentenceRefObj.id == _m_curSentenceRef.id)
                    break;

                dialogueCount++;//限制最多100条避免死循环
            }


            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                ALCommonActionMonoTask.addNextFrameTask(() =>
                {
                    if (wnd != null && wnd.chatHistoryScrollRect != null)
                        wnd.chatHistoryScrollRect.verticalNormalizedPosition = 0;
                });
            });
        }

        /// <summary>
        /// 加入对话历史列表中
        /// </summary>
        /// <param name="_sentenceRefObj"></param>
        private void _addSentenceToHistory(NPDialogueSentenceRefObj _sentenceRefObj)
        {
            if (wnd == null || _sentenceRefObj == null || _sentenceRefObj.chat_history_ui_path_id <= 0)
                return;

            if (_m_lDialogBoxHistoryList == null)
                _m_lDialogBoxHistoryList = new List<NPGGUIWndDialogBox>();

            NPGGUIWndDialogBox itemWnd = new NPGGUIWndDialogBox(_sentenceRefObj.chat_history_ui_path_id, wnd.goChatHistoryParent);
            itemWnd.load(() =>
            {
                itemWnd.showWnd();
                itemWnd.playSentence(_sentenceRefObj, null, null);
                itemWnd.setSentenceShowAllContentText();
            });
            _m_lDialogBoxHistoryList.Add(itemWnd);
        }

        /// <summary>
        /// 选项加入对话历史列表中
        /// </summary>
        /// <param name="_sentenceRefObj"></param>
        /// <param name="_selectIndex"></param>
        private void _addOptionToHistory(NPDialogueSentenceRefObj _sentenceRefObj, int _selectIndex)
        {
            if (wnd == null || _sentenceRefObj == null || _sentenceRefObj.responseOptions == null)
                return;

            if (_m_lOptionHistoryList == null)
                _m_lOptionHistoryList = new List<GGUIWndDialogueHistoryOption>();

            GGUIWndDialogueHistoryOption optionWnd = new GGUIWndDialogueHistoryOption(_sentenceRefObj.option_chat_history_ui_path_id, wnd.goChatHistoryParent);
            optionWnd.load(() =>
            {
                optionWnd.showWnd();
                optionWnd.setInfo(_sentenceRefObj.responseOptions, _selectIndex);
            });
            _m_lOptionHistoryList.Add(optionWnd);
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DIALOGUE_HISTORY);
        }
    }
}
