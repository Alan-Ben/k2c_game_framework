using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class ChatSystemLogMsgSetting : _AChatDataSetting
    {
        public ChatSystemLogMsgSetting() : base((int)ENPChatMsgType.SYSTEM_LOG)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatSystemLogMsgInfo();
        }
    }
}