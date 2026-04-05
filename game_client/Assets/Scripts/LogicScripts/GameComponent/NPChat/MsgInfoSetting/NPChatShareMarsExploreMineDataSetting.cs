
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天伙伴分享消息的设置类
    /// </summary>
    public class NPChatShareMarsExploreMineDataSetting : _AChatDataSetting
    {
        public NPChatShareMarsExploreMineDataSetting() 
            : base((int) ENPChatMsgType.SHARE_MARS_EXPLORE_MINE)
        {
        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatShareMarsExploreMineMsgDetailInfo();
        }
    }
}