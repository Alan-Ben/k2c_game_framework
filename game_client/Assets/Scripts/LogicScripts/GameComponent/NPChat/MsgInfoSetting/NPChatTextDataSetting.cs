
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天文字消息的设置类
    /// </summary>
    public class NPChatTextDataSetting : _AChatDataSetting
    {
        public NPChatTextDataSetting() : base((int) ENPChatMsgType.TEXT)
        {
        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new NPChatTextMsgDetailInfo();
        }
    }
}