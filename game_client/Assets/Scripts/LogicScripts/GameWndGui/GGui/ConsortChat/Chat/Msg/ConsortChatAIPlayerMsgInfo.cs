namespace GOE
{
    public class ConsortChatAIPlayerMsgInfo : _AConsortChatMsgInfo
    {
        private string _m_content;
        public ConsortChatAIPlayerMsgInfo(long _msgId, string _content): base (_msgId)
        {
            _m_content = _content;
        }

        public override EConsortChatMsgType msgType => EConsortChatMsgType.Response;
        public override long uiPathId => 6206;

        public override string getContent()
        {
            return _m_content;
        }
    }
}