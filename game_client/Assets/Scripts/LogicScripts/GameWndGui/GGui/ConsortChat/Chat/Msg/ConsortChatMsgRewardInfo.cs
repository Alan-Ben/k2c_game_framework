namespace GOE
{
    public class ConsortChatMsgRewardInfo : _AConsortChatMsgInfo
    {
        private bool _m_isLastMsg = false;
        private ConsortChatDialogueRefObj _m_dialogueRef;
        private ConsortPresetChatDialogue _m_dialogueInfo;
        public ConsortChatDialogueRefObj dialogueRef => _m_dialogueRef;
        public ConsortChatMsgRewardInfo(ConsortPresetChatDialogue _dialogueInfo, ConsortChatDialogueRefObj _dialogueRef, long _msgId, bool _isLastMsg): base (_msgId)
        {
            _m_dialogueInfo = _dialogueInfo;
            _m_dialogueRef = _dialogueRef;
            _m_isLastMsg = _isLastMsg;
        }
        public override EConsortChatMsgType msgType => EConsortChatMsgType.Reward;
        public override long uiPathId => 6208;

        public long getRewardTimeMs
        {
            get
            {
                if (_m_dialogueInfo != null && _m_dialogueInfo.getRewardTimeMs != 0) return _m_dialogueInfo.getRewardTimeMs;
                return FpsAndPingMgr.instance.serverTimeTag;
            }
        }

        public override string getContent()
        {
            if (_m_dialogueInfo != null && _m_dialogueInfo.isLastDialogue)
                return TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_msg_dialogue_end_desc);
            else
                return TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_msg_dialogue_recent_desc);
        }

        public void tryReqGetReward()
        {
            _m_dialogueInfo?.tryReqDrawReward();
        }
    }
}