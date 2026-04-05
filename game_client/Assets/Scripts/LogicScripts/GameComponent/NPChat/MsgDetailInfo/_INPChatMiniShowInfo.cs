
namespace GOE
{
    /// <summary>
    /// 实现后可以用于<see cref="NPGGUISubWndMiniChat"/>显示
    /// </summary>
    public interface _INPChatMiniShowInfo
    {
        /// <summary>
        /// 迷你聊天显示窗里显示的发送者名字
        /// </summary>
        string getMiniSender();
        /// <summary>
        /// 迷你聊天显示窗里显示的内容
        /// </summary>
        string getMiniContent();
    }
}