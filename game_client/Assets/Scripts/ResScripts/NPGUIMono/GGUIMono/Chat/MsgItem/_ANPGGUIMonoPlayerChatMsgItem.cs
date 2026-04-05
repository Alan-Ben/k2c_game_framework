using ChatPackage;

namespace GOE
{
    /// <summary>
    /// 带玩家头像的聊天信息窗体基类
    /// </summary>
    public abstract class _ANPGGUIMonoPlayerChatMsgItem : _AGUIMonoChatMsgListItem
    {
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon monoPlayer;
    }
}
