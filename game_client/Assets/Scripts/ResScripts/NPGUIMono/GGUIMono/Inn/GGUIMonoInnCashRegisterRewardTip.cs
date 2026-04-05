using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnCashRegisterRewardTip : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("奖励的数值")]
        public Text txtRewardNum;
        [ALHeader("奖励的图标")]
        public RawImage imgRewardIcon;
        [ALHeader("显示时的动画，动画结束后会自动销毁")]
        public Animation showAnimation;
        public string showAnimationName;
        [ALHeader("延迟多久删除")]
        public float deleteDelay;
    }
}