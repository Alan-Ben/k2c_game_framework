namespace GOE
{
    public class ConsortChatMsgTimeInfo : _AConsortChatMsgInfo
    {
        private long _m_timeMs;
        public ConsortChatMsgTimeInfo(long _msgId, long _timeMs): base (_msgId)
        {
            _m_timeMs = _timeMs;
        }
        public override EConsortChatMsgType msgType => EConsortChatMsgType.Time;
        public override long uiPathId => 6208;

        public long getTimeMs
        {
            get
            {
                return _m_timeMs;
            }
        }

        public override string getContent()
        {
            return "";
        }
    }
}