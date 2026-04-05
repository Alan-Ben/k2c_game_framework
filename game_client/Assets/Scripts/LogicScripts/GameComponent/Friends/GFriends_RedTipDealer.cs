using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class PlayerFriendsComponent
    {
        private class RedTipDealer
        {
            private PlayerFriendsComponent _m_component;

            public RedTipDealer(PlayerFriendsComponent _component)
            {
                _m_component = _component;
            }

            /// <summary>
            /// 初始化聊天红点
            /// </summary>
            public void init()
            {
                _refreshRed();
                WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_APPLY_CHG, _refreshRed);
            }

            public void clear()
            {
                // RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_FRIEND_APPLY_ENTER, 0);
                // RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_FRIEND_ENTER, 0);
                WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_APPLY_CHG, _refreshRed);
            }

            /// <summary>
            /// 玩家信息详细红点
            /// </summary>
            private void _refreshRed()
            {
                int count = 0;
                foreach (PlayerFriendApplyItem item in _m_component._m_applyList)
                {
                    if(null == AccountSettingMgr.instance.accountSetting)
                        continue;
                    if(item.applyTimeS <= AccountSettingMgr.instance.accountSetting.friendApplyShowTimeS)
                        continue;
                    count++;
                }
                
                
                _ARedTipNode applyRedNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_FRIEND_APPLY_ENTER);
                if (null != applyRedNode)
                {
                    applyRedNode.setCount(count);
                }
            }
        }
    }
}