using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndConsortMomentDetail : _ATALBasicUIWnd<GGUIMonoConsortMomentDetail>
    {
        private static GGUIWndConsortMomentDetail _g_instance = new GGUIWndConsortMomentDetail();
    
        public static GGUIWndConsortMomentDetail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndConsortMomentDetail();
                return _g_instance;
            }
        }

        private GGUIWndConsortMomentsListItem _m_wMomentWnd;
        private ConsortChatMsgMomentInfo _m_curMoment;
        
        public GGUIWndConsortMomentDetail() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoConsortMomentDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoConsortMomentDetail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            if (_m_wMomentWnd != null)
            {
                _m_wMomentWnd.discard();
                _m_wMomentWnd = null;
            }
        }
    
        protected override void _onReset()
        {
            
        }
    
        protected override void _onDiscard()
        {
            if (_m_wMomentWnd != null)
            {
                _m_wMomentWnd.discard();
                _m_wMomentWnd = null;
            }
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSendComment, _onBtnSendCommentClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnCloseComment, _onBtnCloseCommentClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);

            }
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.inputComment != null)
                wnd.inputComment.characterLimit = GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_one_msg_max_word_count;
            ALUGUICommon.combineBtnClick(wnd.btnSendComment, _onBtnSendCommentClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseComment, _onBtnCloseCommentClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, false);

        }
        public void setInfo(long  _momentInstanceId)
        {
            ConsortChatMsgMomentInfo momentMsg = NPPlayer.instance.consortChatComp.consortMomentsInfo.getMomentMsg(_momentInstanceId);
      
            _m_curMoment = momentMsg;
            _refreshWnd();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if(_m_curMoment == null)
                return;
            if (_m_wMomentWnd != null)
            {
                _m_wMomentWnd.discard();
                _m_wMomentWnd = null;
            }
            GGUIWndConsortMomentsListItem moment = new GGUIWndConsortMomentsListItem(_m_curMoment, wnd.contentRoot, _onSelectMomentComment);
            _m_wMomentWnd = moment;
            if (_m_wMomentWnd != null)
            {
                _m_wMomentWnd.load();
                _m_wMomentWnd.showWnd();
            }
            ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, false);
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
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, true);
        }
        
        /// <summary>
        /// 点击发送评论按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnSendCommentClick(GameObject _)
        {
            if(_m_curMoment == null || _m_curMoment.momentSaver == null || wnd == null || wnd.inputComment == null )
                return;
            string output = wnd.inputComment.text;
            if (string.IsNullOrEmpty(output))
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_input_empty_tips));
                return;
            }
            NPPlayer.instance.consortChatComp.consortMomentsInfo.addPlayerComment(_m_curMoment.momentSaver.momentInstanceId, output);
            wnd.inputComment.text = string.Empty; // 清空输入框
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, false);
            // 让列表进行刷新
            _refreshWnd();
        }

        /// <summary>
        /// 点击关闭评论按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnCloseCommentClick(GameObject _)
        {
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.openCommentShowList, false);
        }

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_CHAT_MOMENT_DETAIL);
        }

    }
}