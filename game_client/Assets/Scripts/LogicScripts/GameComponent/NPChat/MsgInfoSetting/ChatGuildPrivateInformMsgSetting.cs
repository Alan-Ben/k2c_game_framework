using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 联盟私聊公告信息
    /// </summary>
    public class ChatGuildPrivateInformMsgSetting : _AChatDataSetting
    {
        public ChatGuildPrivateInformMsgSetting() : base((int)ENPChatMsgType.GUILD_PRIVATE_INFORM)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatGuildPrivateInformMsgInfo();
        }
    }
}