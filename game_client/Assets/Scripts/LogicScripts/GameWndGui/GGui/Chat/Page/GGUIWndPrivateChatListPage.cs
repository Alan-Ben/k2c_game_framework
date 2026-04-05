using System;
using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPrivateChatListPage:_ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoPrivateChatListPage>
    {
        /// <summary>
        /// 私聊频道列表
        /// </summary>
        private GGUIWndChatListGrid _m_chatCannleGrid;

        private NPGGUIWndCommonToggleEx _m_togSelected;

        public event Action<_INPChatInfo> itemClickAction;
        private bool _m_isSelectedTogOn = false;

        public GGUIWndPrivateChatListPage(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoPrivateChatListPage.assetPath; }
        protected override string _monoObjName { get => GGUIMonoPrivateChatListPage.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _hideSelected();
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_ON, _showSelected);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_OFF, _hideSelected);
        }

        protected override void _onHideWnd()
        {
            _m_chatCannleGrid?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_ON, _showSelected);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_OFF, _hideSelected);
        }

        protected override void _onReset()
        {
            _m_chatCannleGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_chatCannleGrid?.discard();
            _m_chatCannleGrid = null;
            
            _m_togSelected?.discard();
            _m_togSelected = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnSelected, _clickSelected);
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _clickCancel);
            ALUGUICommon.combineBtnClick(wnd.btnRemove, _clickRemove);
            
            if (null != wnd.privateChatGrid)
            {
                _m_chatCannleGrid = new GGUIWndChatListGrid(wnd.privateChatGrid);
                _m_chatCannleGrid.itemClickAction += _clickCannleItem;
            }

            if (null != wnd.togSelected)
            {
                _m_togSelected = new NPGGUIWndCommonToggleEx(wnd.togSelected);
                _m_togSelected.clickDelegate += _clickSelectedAll;
                _m_togSelected.setSelected(false);
            }

            _clickCancel(null);
        }

        /// <summary>
        /// 点击删除选中的聊天
        /// </summary>
        /// <param name="obj"></param>
        private void _clickRemove(GameObject obj)
        {
            List<_INPChatInfo> removeList = new List<_INPChatInfo>();
            _m_chatCannleGrid?.getSelectedList(removeList);
            if (removeList.Count == 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TransKeyConst.chat_channle_no_selected);
                return;
            }
            //删除频道
            NPPlayer.instance.chatComp.removePrivateChannelList(removeList);
            //删除置顶
            NPPlayer.instance.chatComp.setUpToTop(removeList,false);
            
        }

        /// <summary>
        /// 取消批量选择
        /// </summary>
        /// <param name="_"></param>
        private void _clickCancel(GameObject _)
        {
            WinMsg.SendMsg(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_OFF);
        }

        /// <summary>
        /// 点击开始批量选择
        /// </summary>
        /// <param name="_"></param>
        private void _clickSelected(GameObject _)
        {
            WinMsg.SendMsg(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_ON);
        }

        /// <summary>
        /// 批量选中tog
        /// </summary>
        /// <param name="obj"></param>
        private void _clickSelectedAll(NPGGUIWndCommonToggleEx obj)
        {
            if (null == _m_togSelected)
                return;
            _m_togSelected?.setSelected(!obj.isOn);

            WinMsg.SendMsg((_m_togSelected.isOn) ? WinMsgType.ON_CHAT_CHANNEL_SELECT_ALL : WinMsgType.ON_CHAT_CHANNEL_DIS_SELECT_ALL);
        }

        private void _showSelected()
        {
            _m_isSelectedTogOn = true;
            _refreshSelectedTogOn();
        }

        private void _hideSelected()
        {
            _m_isSelectedTogOn = false;
            _refreshSelectedTogOn();
            
            _m_togSelected?.setSelected(false);
            WinMsg.SendMsg(WinMsgType.ON_CHAT_CHANNEL_DIS_SELECT_ALL);
        }

        private void _refreshSelectedTogOn()
        {
            ALUGUICommon.setGameObjEnable(wnd.showSelectedGoList, _m_isSelectedTogOn);
            ALUGUICommon.setGameObjEnable(wnd.hideSelectedGoList, !_m_isSelectedTogOn);
        }

        /// <summary>
        /// 点击私聊频道
        /// </summary>
        /// <param name="_chatCannleInfo"></param>
        private void _clickCannleItem(_INPChatInfo _chatCannleInfo)
        {
            if (null == _chatCannleInfo)
                return;
            itemClickAction?.Invoke(_chatCannleInfo);
        }

        public void refreshWnd()
        {
            if(null == wnd)
                return;

            if (null != _m_chatCannleGrid)
            {
                _m_chatCannleGrid.showWnd();
                
                List<_INPChatInfo> infoList = GChatUtil.getPrivateeChatChannleList();
                
                _m_chatCannleGrid.refresh(infoList);
            }

            if (_m_isSelectedTogOn)
            {
                WinMsg.SendMsg(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_ON);
            }
            else
            {
                WinMsg.SendMsg(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_OFF);
            }
        }
    }
}