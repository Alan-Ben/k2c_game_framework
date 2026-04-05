using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public enum EConsortChatMsgType
    {
        Sentence = 0, // 对话句子
        Response = 1, // 响应选项
        Reward = 2, // 奖励
        AI = 3, // AI回复
        Moments = 4, // 朋友圈
        Time = 5, //时间分割
    }
    public abstract class _AConsortChatMsgInfo
    {
        public abstract EConsortChatMsgType msgType { get;}
        public abstract long uiPathId { get; }
        public virtual long imageGroupId => 0;
        public virtual bool showTyping => false;
        public virtual bool needShowOption => false;

        // 消息的唯一id
        protected long _m_lMsgId;

        /// <summary>
        /// 这条消息的唯一id
        /// </summary>
        public long msgId { get { return _m_lMsgId; } }

        public Action onMsgUpdate;
        
        
        public _AConsortChatMsgInfo(long _msgId)
        {
            _m_lMsgId = _msgId;
        }


        public abstract string getContent();

        public virtual string getMiniContent()
        {
            return getContent();
        }
    }

    



    
}