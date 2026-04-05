using UnityEngine;
using System;
using ALPackage;
using ChatPackage;
using ChatPackage.Internal;

namespace GOE
{
    //聊天频道详细的item
    public class GGUIWndChatChannleItem : _ATALBasicUISubWnd<GGUIMonoChatChannleItem>
    {
        //头像
        private NPGGuiWndTexture _m_playerIconWnd;

        //显示数据
        private _INPChatInfo _m_info;

        //点击回调
        private Action<_INPChatInfo> _m_clickAction;

        #region override
        public GGUIWndChatChannleItem(GGUIMonoChatChannleItem _wnd,Action<_INPChatInfo> _clickAction)
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
            if (null == _m_info)
                return;

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.setTexture(_m_info.getChatIcon());

            ALUGUICommon.setLabelTxt(wnd.playerNameTxt, _m_info.getChatName());
            ALUGUICommon.setLabelTxt(wnd.contentTxt, _m_info.getChatContent());
            
            long _millisecondsCount = FpsAndPingMgr.instance.serverTimeTag - _m_info.getChatTimeMS();
            if (_millisecondsCount < 0)
                _millisecondsCount = 0;
            long secondCount = TimeUtil.msToSecCeiling(_millisecondsCount);
            int min = (int)(secondCount / 60);
            if (min > 0)
            {
                ALUGUICommon.setLabelTxt(wnd.msgTimeS, TextTranslate.instance.getLanguage(
                    TransKeyConst.chat_channle_msg_time_str,
                    TimeUtil.getPassTimeShow(_m_info.getChatTimeMS())));
            }
            else //1分钟内单独显示
            {
                ALUGUICommon.setLabelTxt(wnd.msgTimeS, TextTranslate.instance.getLanguage(TransKeyConst.chat_channle_msg_time_less_min_none));
            }
            ALUGUICommon.setGameObjEnable(wnd.redTipGo,!NPPlayer.instance.chatComp.getCurChatHasRead(_m_info));
            
            ALUGUICommon.setGameObjEnable(wnd.noMsgHideList, _m_info.getChatTimeMS() > 0);
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
            
            ALUGUICommon.setGameObjEnable(wnd.selectedShowList,_isSelected);
        }
    }
}
