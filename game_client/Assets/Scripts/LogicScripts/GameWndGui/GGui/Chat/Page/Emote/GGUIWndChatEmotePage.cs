using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 聊天表情页面
    /// </summary>
    public class GGUIWndChatEmotePage : _ANPGGUIBasicSubWnd<GGUIMonoChatEmotePage>
    {
        private GGUIWndChatEmoteGrid  _m_wndChatEmoteGrid;

        private GGUIWndChatEmoteGroupContainer _m_emoteGroupContainer;

        private GChatEmoteGroupRefObj _m_curSelectedGroup;

        public event Action<GChatEmoteItemRefObj> onClickEmoteItem;
        public event Action onClickHide;
        
        public GGUIWndChatEmotePage(GGUIMonoChatEmotePage _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_wndChatEmoteGrid?.discard();
            _m_wndChatEmoteGrid = null;
            
            _m_emoteGroupContainer?.discard();
            _m_emoteGroupContainer = null;

            _m_curSelectedGroup = null;
            onClickHide = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
            if (null != wnd.emoteGrid)
            {
                _m_wndChatEmoteGrid = new GGUIWndChatEmoteGrid(wnd.emoteGrid);
                _m_wndChatEmoteGrid.onClickEmoteItem += _onClickEmoteItem;
            }

            if (null != wnd.emoteGroupContainer)
            {
                _m_emoteGroupContainer = new GGUIWndChatEmoteGroupContainer(wnd.emoteGroupContainer);
                _m_emoteGroupContainer.onSelectItemChg += _onSelectItemChg;
            }
        }

        private void _onClickEmoteItem(GChatEmoteItemRefObj _itemRefObj)
        {
            if (_itemRefObj == null)
                return;
            
            if (GCommon.getItemCount(ENPItemType.CHAT_EMOTE_GROUP, _itemRefObj.group_id) == 0)
            {
                GChatEmoteGroupRefObj groupRef = GRefdataCoreMgr.instance.chatEmoteGroupRefCore.getRef(_itemRefObj.group_id);
                string unlockDesc = groupRef == null ? TextTranslate.instance.getLanguage(TransKeyConst.chat_emote_group_lock_send_tip)
                    : TextTranslate.instance.getLanguage(groupRef.unlock_cond_desc, groupRef.unlock_cond_desc_args);
                
                NPGUIAddSceneCenterTip.instance.showTextInfo(unlockDesc);
                return;
            }
            
            // 发送消息
            onClickEmoteItem?.Invoke(_itemRefObj);
        }

        private void _onSelectItemChg(GGUIWndChatEmoteGroupContainerItem _itemWnd)
        {
            if(null == _itemWnd)
                return;
            if (_m_curSelectedGroup == _itemWnd.groupRef)
                return;
            _showEmoteItemGrid(_itemWnd.groupRef);
        }

        private void _showEmoteItemGrid(GChatEmoteGroupRefObj _groupRef)
        {
            if (null == _groupRef)
                return;
            if (null == _m_wndChatEmoteGrid)
                return;
            
            _m_curSelectedGroup = _groupRef;
            _m_wndChatEmoteGrid.showWnd();
            _m_wndChatEmoteGrid.setInfo(_m_curSelectedGroup.emoteItemList);
        }

        private void _clickClose(GameObject obj)
        {
            hideWnd();
            onClickHide?.Invoke();
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if (null != _m_emoteGroupContainer)
            {
                _m_emoteGroupContainer.showWnd();
                _m_emoteGroupContainer.setSelectInfo(_m_curSelectedGroup);
            }
        }

        public void hideWndToAniEnd()
        {
            _sampleAnimation(wnd.hideAniName, 1);
            
            hideWndWithoutAni();
        }

        private void _sampleAnimation(string _animationName, float _normalizeTime)
        {
            if (wnd == null || wnd.wndAnimation == null || string.IsNullOrEmpty(_animationName))
                return;
            
            wnd.wndAnimation.Sample(_animationName, _normalizeTime);
        }
    }
}