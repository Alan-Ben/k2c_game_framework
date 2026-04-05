using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndConsortChatPageChatItem : _ANPGGUIBasicGridItemWnd<GGUIMonoConsortChatPageChatItem>
    {
        private long _m_consortId;
        private GGUIWndConsortIconItem _m_consortCardItem;
        
        public GGUIWndConsortChatPageChatItem(GGUIMonoConsortChatPageChatItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_CHAT_INDEX, _simulateClickChat);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_CHAT_INDEX, _simulateClickChat);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
            if (wnd != null) ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.consortIconItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
        }
        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(long _consortId)
        {
            _m_consortId = _consortId;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortId);
            _m_consortCardItem?.showWnd();
            _m_consortCardItem?.setInfo(consortInfo, 0);
     
            ConsortPresetChatInfo chatInfo = NPPlayer.instance.consortChatComp.getConsortPresetChatInfo(_m_consortId);
            if (chatInfo != null)
            {
                chatInfo.getMiniContent(setContent);
                ALUGUICommon.setGameObjEnable(wnd.unreadShowGos, chatInfo.unread);
            }
        }

        private void setContent(string _content)
        {
            ALUGUICommon.setLabelTxt(wnd.chatContent, _content);
        }


        private void _onBtnClick(GameObject _o)
        {
            GGUIWndConsortChatMsgDetail.instance.setInfo(_m_consortId);
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndConsortChatMsgDetail.instance, UINodeTagConst.C_CONSORT_CHAT_MSG_DETAIL, 0);

        }


        //模拟点击聊天按钮
        private void _simulateClickChat(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long curIndex = (long)_objects[0];
            if (curIndex == itemIdx)
                _onBtnClick(null);
        }
    }
}
