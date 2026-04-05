using UnityEngine;
using System;
using ALPackage;
using ChatPackage;
using ChatPackage.Internal;

namespace GOE
{
    //聊天频道item
    public class GGUIWndChatChannleSimpleItem : _ATALBasicUISubWnd<GGUIMonoChatChannleSimpleItem>
    {
        //头像
        private NPGGuiWndTexture _m_playerIconWnd;

        //显示数据
        private _INPChatInfo _m_info;

        //点击回调
        private Action<_INPChatInfo> _m_clickAction;

        #region override
        public GGUIWndChatChannleSimpleItem(GGUIMonoChatChannleSimpleItem _wnd,Action<_INPChatInfo> _clickAction)
           : base(_wnd) 
        {
            initWnd();
            _m_clickAction = _clickAction;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.playerIconImg)
                _m_playerIconWnd = new NPGGuiWndTexture(wnd.playerIconImg);


            ALUGUICommon.combineBtnClick(wnd.clickGo, _clickGoDidClick);
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
            if (_m_info != null)
            {
                _AChatInfo oldInfo = _m_info as _AChatInfo;
                oldInfo.onReceiveMsg -= onRecieveMsg;
                _m_info = null;
            }
            
            if (null == wnd)
                return;

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.discard();
            _m_playerIconWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.clickGo, _clickGoDidClick);
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
            if (null == _m_info || null == wnd)
                return;

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.setTexture(_m_info.getChatIcon());
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_info.getChatName());
            ALUGUICommon.setGameObjEnable(wnd.redTipGo,!NPPlayer.instance.chatComp.getCurChatHasRead(_m_info));
        }

        //收到新消息
        private void onRecieveMsg(MsgInfo info)
        {
            _refresh();
        }

        private void _clickGoDidClick(GameObject _go)
        {
            NPPlayer.instance.chatComp.setCurChatReaded(_m_info);
            if (null != _m_clickAction)
                _m_clickAction(_m_info);
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.selectedShowList, _isSelected);
            ALUGUICommon.setGameObjEnable(wnd.selectedHideList, !_isSelected);

            if (wnd.playerIconImg != null)
            {
                wnd.playerIconImg.color = _isSelected ? wnd.selectedIconColor : wnd.unSelectedIconColor;
            }
        }
    }
}
