
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 纯表现的时间数据
    /// </summary>
    public class NPChatTimeMsgItemInfo : _IMsgItemData
    {
        // 消息的时间
        private long _m_msgTime;
        
        public NPChatTimeMsgItemInfo(long _time)
        {
            _m_msgTime = _time;
        }
        
        /// <summary>
        /// 这条消息的时间
        /// </summary>
        public long msgTimeMilliseconds { get { return _m_msgTime; } }
        public int msgType { get { return (int) ENPChatMsgType.TIME; } }
        public bool isMyMsg { get { return false; } }
    }
}