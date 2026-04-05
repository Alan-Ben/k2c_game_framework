
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天伙伴分享消息的设置类
    /// </summary>
    public class NPChatShareHeroDataSetting : _AChatDataSetting
    {
        public NPChatShareHeroDataSetting() : base((int) ENPChatMsgType.SHARE_HERO)
        {
        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatShareHeroMsgDetailInfo();
        }
    }
}