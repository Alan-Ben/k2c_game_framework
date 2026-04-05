using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndConsortChatPageMoments : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoConsortChatPageMoments>
    {
        private GGUIWndConsortMomentsList _m_wMomentsList; // 界面上的朋友圈列表窗口
        private long _m_curSelectMoment = 0;
        public GGUIWndConsortChatPageMoments( Transform _parent) : base(_parent)
        {
        }
    
        protected override string _monoAssetPath { get => UIResPathAssistant.getAssetPath(6202); }
        protected override string _monoObjName { get => UIResPathAssistant.getObjName(6202); }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        protected override bool isShowAniPlayOnlyOne { get => true; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_CHAT_MOMENTS_CHG, _onMomentsChange);
        }
    
        protected override void _onHideWnd()
        {
            _m_wMomentsList?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_CHAT_MOMENTS_CHG, _onMomentsChange);
        }
    
        protected override void _onReset()
        {
            _m_wMomentsList?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_wMomentsList?.discard();
            _m_wMomentsList = null;
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSendComment, _onBtnSendCommentClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnCloseComment, _onBtnCloseCommentClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnConsortReplyUnread, _onBtnUnReadConsortReplyClick);
            }
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.momentsList != null)
                _m_wMomentsList = new GGUIWndConsortMomentsList(wnd.momentsList, _onSelectMomentComment);
            if (wnd.inputComment != null)
                wnd.inputComment.characterLimit = GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_one_msg_max_word_count;
            ALUGUICommon.combineBtnClick(wnd.btnSendComment, _onBtnSendCommentClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseComment, _onBtnCloseCommentClick);
            ALUGUICommon.combineBtnClick(wnd.btnConsortReplyUnread, _onBtnUnReadConsortReplyClick);
            ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, false);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            NPPlayer.instance.consortChatComp.consortMomentsInfo.load();
            NPPlayer.instance.consortChatComp.consortMomentsInfo.regLoadDoneDelegate(() =>
            {
                _m_wMomentsList?.showWnd();
                _m_wMomentsList?.setShowData(NPPlayer.instance.consortChatComp.consortMomentsInfo);
                AccountSettingMgr.instance.consortMomentSaverMgr.setRead();
            });
            ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, false);

            int interactionCount = AccountSettingMgr.instance.consortMomentSaverMgr.unReadConsortAICommentCount;
            ALUGUICommon.setLabelTxt(wnd.txtConsortInteractionCount, TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_moment_consort_interaction_tip,
                    interactionCount));

            //是否有新互动消息
            bool hasInteraction = interactionCount > 0;
            ALUGUICommon.setGameObjEnable(wnd.unreadConsortReplyShowGos, hasInteraction);

            //如果有新互动消息，朋友圈列表需要调整高度
            float interactionItemHeight = hasInteraction ? wnd.unreadTopPaddingValue : 0;
            _m_wMomentsList?.setInteractionItemHeight(interactionItemHeight);
        }
        
        private void _onMomentsChange()
        {
            _refreshWnd();
        }

        /// <summary>
        /// 点击朋友圈上的评论按钮，弹出评论弹窗
        /// </summary>
        /// <param name="_momentInstanceId"></param>
        private void _onSelectMomentComment(long _momentInstanceId)
        {
            ConsortMomentSaver saver = AccountSettingMgr.instance.consortMomentSaverMgr.getMomentSaver(_momentInstanceId);
            if (saver == null)
                return;
            if (saver.isPlayerComment())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.consort_chat_moment_already_comment_tip);
                return;
            }
            _m_curSelectMoment = _momentInstanceId;
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, true);
        }
        /// <summary>
        /// 点击发送评论按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnSendCommentClick(GameObject _)
        {
            if(_m_curSelectMoment == 0 || wnd == null || wnd.inputComment == null )
                return;
            string output = wnd.inputComment.text;
            if (string.IsNullOrEmpty(output))
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_input_empty_tips));
                return;
            }
            NPPlayer.instance.consortChatComp.consortMomentsInfo.addPlayerComment(_m_curSelectMoment, output);
            wnd.inputComment.text = string.Empty; // 清空输入框
            _m_curSelectMoment = 0;
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, false);
            // 让列表进行刷新
            _m_wMomentsList?.refreshShowedItems();
        }

        /// <summary>
        /// 点击关闭评论按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnCloseCommentClick(GameObject _)
        {
            _m_curSelectMoment = 0;
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, false);
        }
        private void _onBtnUnReadConsortReplyClick(GameObject _)
        {
            GGUIWndConsortMomentInteraction.instance.setInfo(AccountSettingMgr.instance.consortMomentSaverMgr.getUnreadConsortAICommentList());
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndConsortMomentInteraction.instance, UINodeTagConst.C_CONSORT_CHAT_MOMENT_INTERATION, null, null, 0);
        }
    }
}