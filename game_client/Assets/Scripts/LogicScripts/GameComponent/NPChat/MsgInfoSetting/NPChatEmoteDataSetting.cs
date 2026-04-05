
using ChatPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天表情消息的设置类
    /// </summary>
    public class NPChatEmoteDataSetting : _AChatDataSetting
    {
        public NPChatEmoteDataSetting() : base((int) ENPChatMsgType.EMOTE)
        {
        }

        public override _AMsgDetailInfo createMsgDetailInfo()
        {
            return new ChatEmoteMsgDetailInfo();
        }
    }
}