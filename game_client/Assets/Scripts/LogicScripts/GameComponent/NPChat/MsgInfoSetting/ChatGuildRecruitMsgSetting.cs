using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 联盟邀请信息
    /// </summary>
    public class ChatGuildRecruitMsgSetting : _AChatDataSetting
    {
        public ChatGuildRecruitMsgSetting() : base((int)ENPChatMsgType.GUILD_RECRUIT)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatGuildRecruitMsgInfo();
        }
    }
}