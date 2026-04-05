using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GPlayerMailComponent
    {
        private class RedTipDealer
        {
            private GPlayerMailComponent _m_mailInfo;
            
            public RedTipDealer(GPlayerMailComponent _component)
            {
                _m_mailInfo = _component;
            }

            /// <summary>
            /// 初始化聊天红点
            /// </summary>
            public void init()
            {
                _refreshRed();
                
                WinMsg.RegisterMsgAct(WinMsgType.ON_SET_MAIL_BRIEF, _refreshRed);
                WinMsg.RegisterMsgAct(WinMsgType.ON_ADD_MAIL, _refreshRed);
                WinMsg.RegisterMsgAct(WinMsgType.ON_DEL_MAIL, _refreshRed);
                WinMsg.RegisterMsgAct(WinMsgType.ON_MAIL_STAT_CHG, _refreshRed);
                WinMsg.RegisterMsgAct(WinMsgType.ON_CHG_MAIL, _refreshRed);
                _refreshRed();
            }

            public void clear()
            {
                WinMsg.UnregisterMsgAct(WinMsgType.ON_SET_MAIL_BRIEF, _refreshRed);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_ADD_MAIL, _refreshRed);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_DEL_MAIL, _refreshRed);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_MAIL_STAT_CHG, _refreshRed);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_CHG_MAIL, _refreshRed);
            }
            
            /// <summary>
            /// 玩家信息详细红点
            /// </summary>
            private void _refreshRed()
            {
                int count = Mathf.Max(NPPlayer.instance.mailComp.getUnReadCount(), NPPlayer.instance.mailComp.getUnGetRewardCount());
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_MAIL_ENTER, count);
            }
        }
        
    }
}