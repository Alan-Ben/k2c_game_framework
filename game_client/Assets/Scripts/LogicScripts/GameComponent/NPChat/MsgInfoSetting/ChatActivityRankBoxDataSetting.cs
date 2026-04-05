using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 冲榜宝箱分享消息的设置类
    /// </summary>
    public class ChatActivityRankBoxDataSetting : _AChatDataSetting
    {
        public ChatActivityRankBoxDataSetting() : base((int)ENPChatMsgType.ACTIVITY_RANK_BOX)
        {

        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatMsgActivityRankBoxInfo();
        }
    }
}
