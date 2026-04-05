
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天子嗣分享消息的设置类
    /// </summary>
    public class NPChatShareChildDataSetting : _AChatDataSetting
    {
        public NPChatShareChildDataSetting() : base((int) ENPChatMsgType.SHARE_CHILD)
        {
        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatShareChildMsgDetailInfo();
        }
    }
}