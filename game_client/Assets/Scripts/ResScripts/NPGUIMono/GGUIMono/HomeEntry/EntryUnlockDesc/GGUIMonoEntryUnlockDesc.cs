using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 入口解锁描述跟随item
    /// </summary>
    public class GGUIMonoEntryUnlockDesc : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("解锁描述")]
        public Text txtDesc;
        [ALHeader("延迟播放隐藏动画时间秒")]
        public float delayHideTime;
    }
}