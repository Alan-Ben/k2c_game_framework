
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天妃子CG分享消息的设置类
    /// </summary>
    public class NPChatShareConsortCGDataSetting : _AChatDataSetting
    {
        public NPChatShareConsortCGDataSetting() : base((int) ENPChatMsgType.SHARE_CONSORT_CG)
        {
        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatShareConsortCGMsgDetailInfo();
        }
    }
}