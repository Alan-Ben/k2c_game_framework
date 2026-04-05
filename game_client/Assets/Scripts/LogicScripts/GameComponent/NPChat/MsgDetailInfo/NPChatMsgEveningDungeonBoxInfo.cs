using ChatPackage;
using Common.NpChatObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 晚间副本宝箱的分享消息
    /// </summary>
    public class NPChatMsgEveningDungeonBoxInfo : _ANPPlayerChatMsgDetailInfo<ChatObj_MiddayDungeonBox>, _INPChatMiniShowInfo
    {
        private MiddayDungeonBoxRefObj _m_boxRefObj;
        
        public NPChatMsgEveningDungeonBoxInfo() : base((int)ENPChatMsgType.EVENING_DUNGEON_BOX)
        {

        }

        public MiddayDungeonBoxRefObj boxRefObj
        {
            get
            {
                if (_m_boxRefObj == null || _m_boxRefObj.id != content.getBoxId())
                {
                    _m_boxRefObj = GRefdataCoreMgr.instance.middayDungeonBoxRefCore.getRef(content.getBoxId());
                }
                return _m_boxRefObj;
            }
        }
        
        public string getMiniSender()
        {
            return sender.getCName();
        }

        public string getMiniContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_chat_box_mini_content);
        }
    }
}
