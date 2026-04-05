using ALPackage;

namespace GOE
{
    public class ConsortChatMsgSentenceInfo : _AConsortChatMsgInfo
    {
        private ConsortChatDialogueSentenceRefObj _m_sentenceRef;

        
        public ConsortChatMsgSentenceInfo(ConsortChatDialogueSentenceRefObj _sentenceRef, long _msgId): base (_msgId)
        {
            _m_sentenceRef = _sentenceRef;
        }
        

        public override EConsortChatMsgType msgType => EConsortChatMsgType.Sentence;
        public override long uiPathId => _m_sentenceRef?.dialog_res_path_id ?? 6205;
        public override long imageGroupId => _m_sentenceRef?.image_show_id ?? 0;
        public override bool needShowOption =>  _m_sentenceRef != null && _m_sentenceRef.response_sentence_list != null && _m_sentenceRef.response_sentence_list.Count > 0;


        public override string getContent()
        {
            if (_m_sentenceRef == null)
                return "Error";
            return _m_sentenceRef.getContent();
        }
        
        public override string getMiniContent()
        {
            if (_m_sentenceRef == null)
                return "Error";
            return _m_sentenceRef.getContent();
        }
    }
}