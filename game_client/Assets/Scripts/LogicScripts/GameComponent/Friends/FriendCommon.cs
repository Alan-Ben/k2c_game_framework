using ALPackage;
using NPCommon;
using NPEnum;
using System;
using System.Collections.Generic;

namespace GOE
{
    public static class FriendCommon
    {
        //发送好友申请
        public static void sendAddFriendRequest(long _cid, Action _dealDone = null, Action _failAction = null)
        {
            if (_cid == NPPlayer.instance.playerInfo.CID)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_send_self_none);
                return;
            }

            if (NPPlayer.instance.friendsComp.isShield(_cid))
            {
                //您已屏蔽对方，添加好友需要取消屏蔽操作
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_send_isShield_none);
                return;
            }
            
            if (NPPlayer.instance.friendsComp.isFriend(_cid))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_send_isFriend_none);
                return;
            }

            //自己好友数量已经达到上限
            if (NPPlayer.instance.friendsComp.isFriendsCountMax())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_countIsMax_none);
                return;
            }

            Dictionary<long, long> dic = NPPlayer.instance.friendsComp.requestDic;
            if (null == dic || !dic.ContainsKey(_cid))
            {
                NPPlayer.instance.friendsComp.addSendRequestFriends(_cid, FpsAndPingMgr.instance.serverTimeTagS);
                NPPlayer.instance.friendsComp.reqSendFriendApply(_cid, () =>
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_send_suc_none);
                    _dealDone?.Invoke();
                },_failAction);
                return;
            }

            //取出时间戳
            long value = 0;
            bool suc = dic.TryGetValue(_cid, out value);
            if (!suc)
                return;

            //如果还在间隔时间内 则不可发送
            if (FpsAndPingMgr.instance.serverTimeTagS - value < GRefdataCoreMgr.instance.npGeneral.friend_apply_send_margin_secs)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_sendRequestIsInMargin_num, TimeUtil.millisecondsToTime_Two(GRefdataCoreMgr.instance.npGeneral.friend_apply_send_margin_secs * 1000)));
                return;
            }
            else
            {
                //超出的话更新记录
                NPPlayer.instance.friendsComp.addSendRequestFriends(_cid, FpsAndPingMgr.instance.serverTimeTagS);
                NPPlayer.instance.friendsComp.reqSendFriendApply(_cid, () =>
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_send_suc_none);
                    _dealDone?.Invoke();
                },_failAction);
                return;
            }
        }
        
        /// <summary>
        /// 展示好友选择
        /// </summary>
        /// <param name="_doneAction"></param>
        public static void showFriendSelect(Action<PlayerFriendItemData> _doneAction)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFriendsSelect.instance, () =>
            {
                GGUIWndFriendsSelect.instance.showWnd();
                GGUIWndFriendsSelect.instance.setSelectAction(_doneAction);
            }, UINodeTagConst_Friends.C_ADD_FRIEND_SELECT_NODE);
        }

        /// <summary>
        /// 展示添加好友
        /// </summary>
        public static void showAddFriend()
        {
            QueueMgr.instance.AddNode(new GMainQueueSearchFriendNode());
        }

        /// <summary>
        /// 展示好友申请列表
        /// </summary>
        public static void showFriendsRequest()
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndRequestFriends.instance, () =>
            {
                GGUIWndRequestFriends.instance.showWnd();
            }, UINodeTagConst_Friends.C_ADD_REQUEST_FRIENDS_NODE);
        }

        public static void showPlayerInfo(NPCommonSimplePlayerInfo _info)
        {
            if (null == _info)
                return;
            if (_info.cid == NPPlayer.instance.playerInfo.CID)
                QueueMgr.instance.AddNode(new GNodePlayerInfo());
            else
            {
                QueueMgr.instance.addNode_InGame_MainUIMainScene(GMainGUIAddSceneOtherPlayerInfo.instance, EUIQueueStageType.MAIN, UINodeTagConst.C_Main_OtherPlayerInfoNode, false, 0);
                GMainGUIAddSceneOtherPlayerInfo.instance.setData(_info);
            }
        }

        public static void showPlayerInfo(long _playerCid)
        {

            if (_playerCid == NPPlayer.instance.playerInfo.CID)
                QueueMgr.instance.AddNode(new GNodePlayerInfo());
            else
            {
                GCommon.reqPlayerInfo(_playerCid, (_info) =>
                {
                    QueueMgr.instance.addNode_InGame_MainUIMainScene(GMainGUIAddSceneOtherPlayerInfo.instance, EUIQueueStageType.MAIN, UINodeTagConst.C_Main_OtherPlayerInfoNode, false);
                    GMainGUIAddSceneOtherPlayerInfo.instance.setData(_info);
                });

            }
        }

        /// <summary>
        /// 屏蔽玩家
        /// </summary>
        /// <param name="_playerInfo"></param>
        public static void setShieldPlayer(NPCommonSimplePlayerInfo _playerInfo, Action _dealDone = null)
        {
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.friend_shield_content_none, _playerInfo.name),//您是否要分享当前穿搭？
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    NPPlayer.instance.friendsComp.reqSetShieldPlayer(_playerInfo.cid, (_info) =>
                    {
                        _dealDone?.Invoke();
                    });
                }, true,TransKeyConst.friend_shield_title_none);
        }

        /// <summary>
        /// 取消屏蔽玩家
        /// </summary>
        /// <param name="_playerInfo"></param>
        public static void setUnShieldPlayer(NPCommonSimplePlayerInfo _playerInfo)
        {
            
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.friend_unshield_content_none, _playerInfo.name),//是否取消屏蔽玩家{0}
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    NPPlayer.instance.friendsComp.reqUnsetShieldPlayer(_playerInfo.cid);
                }, true,TransKeyConst.friend_unshield_title_none);
        }
    }
}