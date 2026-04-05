using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 晚间副本宝箱分享消息的设置类
    /// </summary>
    public class NPChatEveningDungeonBoxSetting : _AChatDataSetting
    {
        public NPChatEveningDungeonBoxSetting() : base((int)ENPChatMsgType.EVENING_DUNGEON_BOX)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new NPChatMsgEveningDungeonBoxInfo();
        }
    }
}
