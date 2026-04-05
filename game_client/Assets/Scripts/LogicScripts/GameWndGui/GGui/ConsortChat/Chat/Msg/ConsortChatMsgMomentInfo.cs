using JetBrains.Annotations;

namespace GOE
{

    public class ConsortChatMsgMomentInfo : _AConsortChatMsgInfo
    {
        [NotNull]private ConsortMomentSaver _m_momentSaver;

        public ConsortMomentSaver momentSaver => _m_momentSaver;
        public ConsortChatMsgMomentInfo([NotNull]ConsortMomentSaver _momentSaver): base (_momentSaver.momentInstanceId)
        {
            _m_momentSaver = _momentSaver;
        }
        public override EConsortChatMsgType msgType => EConsortChatMsgType.Moments;
        public override long uiPathId => 6209;

        public override string getContent()
        {
            return TextTranslate.instance.getLanguage(_m_momentSaver.momentData?.content);
        }

        public string getLikePlayers()
        {
            string likeInfo = "";
            bool addedPlayer = false;
            string splitter = TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_moment_like_player_splitter);
            if (_m_momentSaver.momentData != null)
                for (var i = 0; i < _m_momentSaver.momentData.likeConsortList.Count; i++)
                {
                    var id = _m_momentSaver.momentData.likeConsortList[i];
                    if (!addedPlayer && NPPlayer.instance.playerInfo.CID == id)
                    {
                        addedPlayer = true;
                        likeInfo += TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_moment_player_high_light,
                            NPPlayer.instance.playerInfo.PlayerName);
                    }
                    else
                    {
                        GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(id);
                        if (consortInfo != null)
                            likeInfo += consortInfo.consortTransName;
                    }

                    if (i < _m_momentSaver.momentData.likeConsortList.Count - 1)
                        likeInfo += splitter;
                }

            return likeInfo;
        }

        /// <summary>
        /// 获取朋友圈发送时间
        /// </summary>
        /// <returns></returns>
        public string getMomentTime()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_moment_send_time_str, TimeUtil.millisecondsToTime_Max(FpsAndPingMgr.instance.serverTimeTag - _m_momentSaver.momentInstanceId));
        }
    }
}