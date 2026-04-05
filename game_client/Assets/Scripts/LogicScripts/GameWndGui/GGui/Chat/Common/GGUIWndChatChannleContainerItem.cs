using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using GS2GC.p004_PlayerOp;
using ChatPackage;
using ChatPackage.Internal;

namespace GOE
{
    //聊天频道会话item
    public class GGUIWndChatChannleContainerItem : _ATALBasicUISubWnd<GGUIMonoChatChannleContainerItem>
    {
        private GGUIWndChatChannleSimpleItem _m_channleItemWnd;
        
        //显示数据
        private _INPChatInfo _m_info;

        //点击回调
        private Action<_INPChatInfo> _m_clickAction;

        #region override
        public GGUIWndChatChannleContainerItem(GGUIMonoChatChannleContainerItem _wnd,Action<_INPChatInfo> _clickAction)
           : base(_wnd)
        {
            _m_clickAction = _clickAction;
            initWnd();
        }
        
        //数据
        public _INPChatInfo info { get => _m_info; }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            
            if (null != wnd.channleItem)
            {
                _m_channleItemWnd = new GGUIWndChatChannleSimpleItem(wnd.channleItem, _clickChannleItem);
            }

        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_channleItemWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_channleItemWnd?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (_m_info != null)
            {
                _AChatInfo oldInfo = _m_info as _AChatInfo;
                oldInfo.onReceiveMsg -= onRecieveMsg;
                _m_info = null;
            }
            
            if (null == wnd)
                return;
            
            _m_channleItemWnd?.discard();
            _m_channleItemWnd = null;
        }
        #endregion

        // 初始化UI
        public void setInfo(_INPChatInfo _info)
        {
            if (null != _m_info)
            {
                _AChatInfo oldInfo = _m_info as _AChatInfo;
                oldInfo.onReceiveMsg -= onRecieveMsg;
            }
            _m_info = _info;
            _AChatInfo chatInfo = _m_info as _AChatInfo;
            if (null != chatInfo)
                chatInfo.onReceiveMsg += onRecieveMsg;

            _refresh();
        }

        private void _refresh()
        {
            if (null == _m_info)
                return;

            if (null != _m_channleItemWnd)
            {
                _m_channleItemWnd.showWnd();
                _m_channleItemWnd.setInfo(_m_info);
            }
        }

        //收到新消息
        private void onRecieveMsg(MsgInfo info)
        {
            _refresh();
        }

        private void _clickChannleItem(_INPChatInfo obj)
        {
            if (null != _m_clickAction)
                _m_clickAction(_m_info);
        }

        public void setSelected(bool _isSelected)
        {
            _m_channleItemWnd?.setSelected(_isSelected);
        }
    }
}
