using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 联盟成员火星矿被攻击信息
    /// </summary>
    public class ChatGuildMarsMineOccupyMsgSetting : _AChatDataSetting
    {
        public ChatGuildMarsMineOccupyMsgSetting() : base((int)ENPChatMsgType.GUILD_MARS_MINE_OCCUPY)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatGuildMarsMineOccupyMsgInfo();
        }
    }
}
