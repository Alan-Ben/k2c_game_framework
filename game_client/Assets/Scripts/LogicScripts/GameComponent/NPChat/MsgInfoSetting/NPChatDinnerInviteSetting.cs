using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 宴会分享消息的设置类
    /// </summary>
    public class NPChatDinnerInviteSetting : _AChatDataSetting
    {
        public NPChatDinnerInviteSetting() : base((int)ENPChatMsgType.DINNER_INVITE)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new NPChatMsgDinnerInviteInfo();
        }
    }
}
