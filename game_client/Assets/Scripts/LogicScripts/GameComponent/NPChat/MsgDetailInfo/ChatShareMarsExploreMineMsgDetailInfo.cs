
using NPEnum;
using ChatPackage;
using Common.MarsObj;
using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 聊天火星探索矿分享信息
    /// </summary>
    public class ChatShareMarsExploreMineMsgDetailInfo : _AMsgDetailInfo<NPCommon_ChatContent_MarsExploreMineShare, NPCommon_ChatPlayerContent>, _INPChatMiniShowInfo
    {
        private MarsExploreMineRefObj _m_mineRefObj;
        
        
        public ChatShareMarsExploreMineMsgDetailInfo() 
            : base((int) ENPChatMsgType.SHARE_MARS_EXPLORE_MINE)
        {
        }

        public override bool isMyMsg { get { return sender.getCid() == NPPlayer.instance.playerInfo.CID; } }

        public override void readByBytes(byte[] _content, byte[] _sender)
        {
            base.readByBytes(_content, _sender);
            _m_mineRefObj = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(content.getRefId());
        }

        public string getMineNameTranslated()
        {
            if (_m_mineRefObj == null)
                return string.Empty;

            return TextTranslate.instance.getLanguage(_m_mineRefObj.name);
        }
        public int getMineLevel()
        {
            if (_m_mineRefObj == null)
                return 0;

            return _m_mineRefObj.mine_lvl;
        }
        /// <inheritdoc/>
        public string getMiniSender()
        {
            return sender.getCName();
        }
        /// <inheritdoc/>
        public string getMiniContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_share_mars_explore_mine_str, getMineNameTranslated());
        }
    }
}
