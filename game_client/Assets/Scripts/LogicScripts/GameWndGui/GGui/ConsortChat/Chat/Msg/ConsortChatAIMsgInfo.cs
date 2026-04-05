namespace GOE
{
    
    public class ConsortChatAIMsgInfo : _AConsortChatMsgInfo
    {
        private string _m_content;
        public ConsortChatAIMsgInfo(long _msgId, string _content): base (_msgId)
        {
            _m_content = _content;
        }

        public override EConsortChatMsgType msgType => EConsortChatMsgType.AI;
        public override long uiPathId => 6205;

        public override string getContent()
        {
            return _m_content;
        }
    }
}