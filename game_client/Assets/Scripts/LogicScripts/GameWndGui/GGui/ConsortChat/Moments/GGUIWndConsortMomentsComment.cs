using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndConsortMomentsComment : _ATALBasicUISubWnd<GGUIMonoConsortMomentsComment>
    {
        private ConsortChatCommentData _m_commentData;
        private GGUIWndConsortIconItem _m_consortCardItem;
        public GGUIWndConsortMomentsComment(GGUIMonoConsortMomentsComment _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (wnd.consortIconItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);
        }

        public void setInfo(ConsortChatCommentData _data)
        {
            _m_commentData = _data;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd || null == _m_commentData)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtSender, _m_commentData.commentSenderName());
            ALUGUICommon.setLabelTxt(wnd.txtContent, _m_commentData.getContent());
            bool isPlayerSend = _m_commentData.senderId == NPPlayer.instance.playerInfo.CID;

            if (!isPlayerSend)
            {
                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_commentData.senderId);
                _m_consortCardItem?.showWnd();
                _m_consortCardItem?.setInfo(consortInfo, 0);
            }
            ALUGUICommon.setGameObjEnable(wnd.playerSendShow, isPlayerSend);
            ALUGUICommon.setGameObjEnable(wnd.consortSendShow, !isPlayerSend);
        }
    }
}
