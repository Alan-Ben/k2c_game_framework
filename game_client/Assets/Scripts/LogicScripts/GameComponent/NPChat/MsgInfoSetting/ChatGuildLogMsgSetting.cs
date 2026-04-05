using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 联盟日志
    /// </summary>
    public class ChatGuildLogMsgSetting : _AChatDataSetting
    {
        public ChatGuildLogMsgSetting() : base((int)ENPChatMsgType.GUILD_LOG)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatGuildLogMsgInfo();
        }
    }
}