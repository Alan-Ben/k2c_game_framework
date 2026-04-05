using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndConsortChatPageChat : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoConsortChatPageChat>
    {
        private GGUIWndConsortChatPageNewFriendItemGrid _m_addFriendItemGrid;
        private GGUIWndConsortChatPageChatItemGrid _m_chatItemGrid;

    
        public GGUIWndConsortChatPageChat(Transform _parent) : base(_parent)
        {
        }
    
        protected override string _monoAssetPath { get => UIResPathAssistant.getAssetPath(6201); }
        protected override string _monoObjName { get => UIResPathAssistant.getObjName(6201); }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_CHAT_PRESET_CHAT_CHG, _onChatPresetChange);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_CHAT_ADD_FRIEND, _onChatItemListChange);

        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_CHAT_PRESET_CHAT_CHG, _onChatPresetChange);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_CHAT_ADD_FRIEND, _onChatItemListChange);

        }
    
        protected override void _onReset()
        {
            
        }
    
        protected override void _onDiscard()
        {
            if(_m_chatItemGrid != null)
                _m_chatItemGrid.discard();
            _m_chatItemGrid = null;
            
            if(_m_addFriendItemGrid != null)
                _m_addFriendItemGrid.discard();
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.addFriendItemGrid != null)
            {
                _m_addFriendItemGrid = new GGUIWndConsortChatPageNewFriendItemGrid(wnd.addFriendItemGrid);
            }
            if (wnd.chatItemGrid != null)
            {
                _m_chatItemGrid = new GGUIWndConsortChatPageChatItemGrid(wnd.chatItemGrid);
            }
        }

        public void setPage()
        {
            _refreshWnd();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if (_m_addFriendItemGrid != null)
            {
                List<long> addFriendList = NPPlayer.instance.consortChatComp.getChatNewFriendList();
                if (addFriendList.Count > 0)
                {
                    _m_addFriendItemGrid.showItemList(addFriendList);
                    _m_addFriendItemGrid.showWnd();
                    _m_addFriendItemGrid.moveToTopNextFrame();
                }
                else
                {
                    _m_addFriendItemGrid.hideWnd();
                }
            }
            List<long> commonChatList = getConsortList();
            _m_chatItemGrid?.showItemList( commonChatList);
            ALUGUICommon.setGameObjEnable(wnd.noChatItemShow, commonChatList?.Count <= 0);
        }

        private List<long> getConsortList()
        {
            return NPPlayer.instance.consortChatComp.getChatConsortList();
        }
        

        /// <summary>
        /// 预设聊天消息发生变化
        /// </summary>
        private void _onChatPresetChange()
        {
            List<long> commonChatList = getConsortList();
            _m_chatItemGrid?.showItemList(commonChatList);
            ALUGUICommon.setGameObjEnable(wnd.noChatItemShow, commonChatList?.Count <= 0);
        }

        /// <summary>
        /// 当添加好友时，聊天列表发生变化
        /// </summary>
        private void _onChatItemListChange()
        {
            List<long> commonChatList = getConsortList();
            _m_chatItemGrid?.showItemList(commonChatList);
            ALUGUICommon.setGameObjEnable(wnd.noChatItemShow, commonChatList?.Count <= 0);

        }
    }
}