using ChatPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带玩家头像，还有带功能解锁判断的跳转功能的聊天信息窗体基类
    /// </summary>
    public abstract class _ANPGGUIMonoPlayerChatMsgItem_JumpTo : _ANPGGUIMonoPlayerChatMsgItem
    {
        [ALHeader("点击跳转")]
        public GameObject btnJump;
    }
}
