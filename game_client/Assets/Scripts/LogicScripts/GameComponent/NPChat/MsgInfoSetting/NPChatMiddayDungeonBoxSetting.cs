using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 宴会分享消息的设置类
    /// </summary>
    public class NPChatMiddayDungeonBoxSetting : _AChatDataSetting
    {
        public NPChatMiddayDungeonBoxSetting() : base((int)ENPChatMsgType.MIDDAY_DUNGEON_BOX)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new NPChatMsgMiddayDungeonBoxInfo();
        }
    }
}