using ALPackage;

namespace GOE
{
    public class ConsortChatMsgTyping : _AConsortChatMsgInfo
    {
        private bool _m_bIsShowTyping = false;
        
        public ConsortChatMsgTyping(long _msgId): base (_msgId)
        {
            _m_bIsShowTyping = false;
        }

        public override EConsortChatMsgType msgType => EConsortChatMsgType.Sentence;
        public override long uiPathId => 6205;
        public override bool showTyping => true;

        public override string getContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_msg_loading_mini_content);
        }
        
        public override string getMiniContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_msg_loading_mini_content);
        }
    }
}