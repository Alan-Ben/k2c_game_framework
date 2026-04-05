
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天妃子分享消息的设置类
    /// </summary>
    public class NPChatShareConsortDataSetting : _AChatDataSetting
    {
        public NPChatShareConsortDataSetting() : base((int) ENPChatMsgType.SHARE_CONSORT)
        {
        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatShareConsortMsgDetailInfo();
        }
    }
}