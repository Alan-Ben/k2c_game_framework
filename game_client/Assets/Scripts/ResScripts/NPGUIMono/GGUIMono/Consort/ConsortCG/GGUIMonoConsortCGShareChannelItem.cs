using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 家人CG分享聊天频道项
    /// </summary>
    public class GGUIMonoConsortCGShareChannelItem : _AALBasicUIWndMono
    {
        [ALHeader("频道类型")]
        public ENPChatRoomType channelType;
        [ALHeader("tab配置")]
        public NPGGUIMonoCommonTab monoTab;
    }
}