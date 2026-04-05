namespace GOE
{

    public class ConsortChatMsgResponseInfo : _AConsortChatMsgInfo
    {
        private ConsortChatDialogueSentenceRefObj _m_sentenceRef;

        public ConsortChatMsgResponseInfo(ConsortChatDialogueSentenceRefObj _sentenceRef, long _msgId): base (_msgId)
        {
            _m_sentenceRef = _sentenceRef;

        }

        public override EConsortChatMsgType msgType => EConsortChatMsgType.Response;
        public override long uiPathId => _m_sentenceRef?.dialog_res_path_id ?? 6206;
        public override long imageGroupId => _m_sentenceRef?.image_show_id ?? 0;

        public override string getContent()
        {
            if (_m_sentenceRef == null)
                return "Error";
            return _m_sentenceRef.getContent();
        }
    }
}