
using System.Collections.Generic;
using Common.MarsObj;
using UnityEngine;

namespace GOE
{
    public static class NPMsgDetailInfoFactory
    {
        
        /// <summary>
        /// 创建一个用来发送的妃子分享消息
        /// </summary>
        /// <param name="gottenConsortInfo"></param>
        public static ChatShareConsortMsgDetailInfo createShareConsortInfo(GGottenConsortInfo gottenConsortInfo)
        {
            ChatShareConsortMsgDetailInfo msg = new ChatShareConsortMsgDetailInfo();
            msg.setSenderIsMyself();
            msg.setContent(gottenConsortInfo);
            return msg;
        } 
        
        /// <summary>
        /// 创建一个用来发送的骑士分享消息
        /// </summary>
        /// <param name="_heroInfo"></param>
        public static ChatShareHeroMsgDetailInfo createShareHeroInfo(HeroInfo _heroInfo)
        {
            ChatShareHeroMsgDetailInfo msg = new ChatShareHeroMsgDetailInfo();
            msg.setSenderIsMyself();
            msg.setContent(_heroInfo);
            return msg;
        }

        /// <summary>
        /// 创建一个用来发送的子嗣分享消息
        /// </summary>
        /// <param name="_childInfo"></param>
        public static ChatShareChildMsgDetailInfo createShareChildInfo(ChildInfo _childInfo)
        {
            ChatShareChildMsgDetailInfo msg = new ChatShareChildMsgDetailInfo();
            msg.setSenderIsMyself();
            msg.setContent(_childInfo);
            return msg;
        }
        
        /// <summary>
        /// 创建一个用来发送的子嗣分享消息
        /// </summary>
        /// <param name="_childInfo"></param>
        public static ChatShareChildMsgDetailInfo createShareChildInfo(AdultInfo _adultInfo)
        {
            ChatShareChildMsgDetailInfo msg = new ChatShareChildMsgDetailInfo();
            msg.setSenderIsMyself();
            msg.setContent(_adultInfo);
            return msg;
        }
        
        /// <summary>
        /// 创建一个用来发送的表情消息
        /// </summary>
        /// <param name="_itemRefId">聊天表情的itemId</param>
        public static ChatEmoteMsgDetailInfo createEmoteInfo(long _itemRefId)
        {
            ChatEmoteMsgDetailInfo msg = new ChatEmoteMsgDetailInfo();
            msg.setSenderIsMyself();
            msg.setContent(_itemRefId);
            return msg;
        }       
        /// <summary>
        /// 创建一个用来发送的文字消息
        /// </summary>
        /// <param name="_text">想要发送的文字内容</param>
        public static NPChatTextMsgDetailInfo createTextInfo(string _text)
        {
            NPChatTextMsgDetailInfo msg = new NPChatTextMsgDetailInfo();
            msg.setSenderIsMyself();
            msg.setContent(_text);
            return msg;
        }        
        
        /// <summary>
        /// 创建用于分享的宴会分享消息
        /// </summary>
        /// <param name="_dinnerId"></param>
        /// <returns></returns>
        public static NPChatMsgDinnerInviteInfo createDinnerInviteMsgInfo(GDinnerInfo _dinnerInfo)
        {
            NPChatMsgDinnerInviteInfo msg = new NPChatMsgDinnerInviteInfo();
            msg.setSenderIsMyself();
            msg.setInfo(_dinnerInfo);
            return msg;
        }

        /// <summary>
        /// 创建一个用来发送的妃子CG分享消息
        /// </summary>
        /// <param name="_consortCGInfo"></param>
        public static ChatShareConsortCGMsgDetailInfo createShareConsortCGInfo(ConsortCgInfo _consortCGInfo)
        {
            ChatShareConsortCGMsgDetailInfo msg = new ChatShareConsortCGMsgDetailInfo();
            msg.setSenderIsMyself();
            msg.setContent(_consortCGInfo);
            return msg;
        }
    }
}