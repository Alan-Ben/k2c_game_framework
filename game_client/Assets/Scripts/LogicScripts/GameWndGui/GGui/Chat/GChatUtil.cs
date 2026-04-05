using System;
using System.Collections.Generic;
using ChatPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class GChatUtil
    {
        [NotNull]
        public static List<_INPChatInfo> getPrivateeChatChannleList()
        {
            
            List<_AChatInfo> chatInfoList = new List<_AChatInfo>();
            NPPlayer.instance.chatComp.getAllPrivateChat(chatInfoList);
            // NPPlayer.instance.chatComp.getAllChatInfo(chatInfoList);
            List<_INPChatInfo> infoList = new List<_INPChatInfo>();
            for (int i = 0; i < chatInfoList.Count; i++)
            {
                _AChatInfo temp = chatInfoList[i];
                if (null == temp)
                    continue;
                if (temp is NPPrivateChatInfo privateChatInfo)
                {
                    if(string.IsNullOrEmpty(privateChatInfo.chatInfoTag))
                        continue;

                    long removeTime = 0;
                    NPPlayer.instance.chatComp.accountSaver?.historyChatRemoveTimeDic?.TryGetValue(privateChatInfo.chatInfoTag, out removeTime);
                    //筛选规则
                    if(privateChatInfo.getChatTimeMS() < removeTime)
                        continue;

                    if (privateChatInfo.userInfo != null && NPPlayer.instance.friendsComp.isShield(privateChatInfo.userInfo.cid))
                    {
                        NPPlayer.instance.chatComp.removePrivateChannel(temp as _INPChatInfo);
                        continue;
                    }
                    
                    infoList.Add(temp as _INPChatInfo);
                }
            }
            infoList.Sort(_sortPrivateChatChannle);

            return infoList;
        }
        

        /// <summary>
        /// 私聊排序规则
        /// </summary>
        private static int _sortPrivateChatChannle(_INPChatInfo _x, _INPChatInfo _y)
        {
            NPPrivateChatInfo privateChatInfoX = _x as NPPrivateChatInfo;
            NPPrivateChatInfo privateChatInfoY = _y as NPPrivateChatInfo;
            if (privateChatInfoX.isUpToTop && !privateChatInfoY.isUpToTop)
                return -1;
            if (!privateChatInfoX.isUpToTop && privateChatInfoY.isUpToTop)
                return 1;
            if (privateChatInfoX.getChatTimeMS() > privateChatInfoY.getChatTimeMS())
                return -1;
            if (privateChatInfoX.getChatTimeMS() < privateChatInfoY.getChatTimeMS())
                return 1;
            //排序规则
            return -_x.getSortId().CompareTo(_y.getSortId());
        }

        /// <summary>
        /// 发送聊天分享
        /// </summary>
        /// <param name="_shareType">分享类型</param>
        /// <param name="_msgInfo">消息</param>
        /// <param name="_dealSucc">成功回调</param>
        public static void sendChatShareMsg(EChatShareType _shareType, _AMsgDetailInfo _msgInfo,Action _dealSucc = null)
        {
            sendChatShareMsg(ENPChatRoomType.NONE, _shareType, _msgInfo, _dealSucc);
        }

        /// <summary>
        /// 发送聊天分享
        /// </summary>
        /// <param name="_type">聊天频道</param>
        /// <param name="_shareType">分享类型</param>
        /// <param name="_msgInfo">消息</param>
        /// <param name="_dealSucc">成功回调</param>
        public static void sendChatShareMsg(ENPChatRoomType _type, EChatShareType _shareType, _AMsgDetailInfo _msgInfo, Action _dealSucc = null)
        {
            //分享的聊天频道
            _AChatInfo curChatInfo = null;

            //未指定类型则取当前聊天频道
            if (_type == ENPChatRoomType.NONE)
                curChatInfo = NPPlayer.instance.chatComp.curChatInfo;
            else
                curChatInfo = NPPlayer.instance.chatComp.getRoomChatInfo(_type);

            Dictionary<EChatShareType, long> dic = NPPlayer.instance.chatComp.shareDic;
            if (null == dic || !dic.ContainsKey(_shareType))
            {
                if (null != curChatInfo && curChatInfo.sendMsg(_msgInfo))
                {
                    NPPlayer.instance.chatComp.addChatShareTime(_shareType, FpsAndPingMgr.instance.serverTimeTagS);
                    _dealSucc?.Invoke();
                }
                return;
            }

            //取出时间戳
            long value = 0;
            bool suc = dic.TryGetValue(_shareType, out value);
            if (!suc)
                return;

            //如果还在间隔时间内 则不可发送
            if (FpsAndPingMgr.instance.serverTimeTagS - value < GRefdataCoreMgr.instance.npGeneral.chat_share_send_margin_secs)
            {
                switch (_shareType)
                {
                    case EChatShareType.HERO:    
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_shareHeroIsInMargin_num, TimeUtil.millisecondsToTime_Two(GRefdataCoreMgr.instance.npGeneral.chat_share_send_margin_secs * 1000)));
                        break;
                    case EChatShareType.CHILD:    
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_shareChildIsInMargin_num, TimeUtil.millisecondsToTime_Two(GRefdataCoreMgr.instance.npGeneral.chat_share_send_margin_secs * 1000)));
                        break;
                    case EChatShareType.CONSORT:    
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_shareConsortIsInMargin_num, TimeUtil.millisecondsToTime_Two(GRefdataCoreMgr.instance.npGeneral.chat_share_send_margin_secs * 1000)));
                        break;
                    case EChatShareType.CONSORT_CG:    
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_shareConsortCGIsInMargin_num, TimeUtil.millisecondsToTime_Two(GRefdataCoreMgr.instance.npGeneral.chat_share_send_margin_secs * 1000)));
                        break;
                }
                return;
            }
            if (null != curChatInfo && curChatInfo.sendMsg(_msgInfo))
            {
                NPPlayer.instance.chatComp.addChatShareTime(_shareType, FpsAndPingMgr.instance.serverTimeTagS);
                _dealSucc?.Invoke();
            }
        }
    }
}