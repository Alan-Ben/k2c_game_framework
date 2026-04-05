
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 系统消息消息的设置类
    /// </summary>
    public class NPChatSystemDataSetting : _AChatDataSetting
    {
        public NPChatSystemDataSetting() : base((int) ENPChatMsgType.SYSTEM)
        {
        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new NPChatTextMsgSystemInfo();
        }
    }
}