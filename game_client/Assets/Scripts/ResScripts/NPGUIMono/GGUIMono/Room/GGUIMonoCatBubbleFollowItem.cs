using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum ECatBubbleFollowItemAniType
    {
        [InspectorName("SHOW（显示动画）")]
        SHOW,
        [InspectorName("HIDE（隐藏动画）")]
        HIDE,
    }
    /// <summary>
    /// 猫咪气泡跟随item
    /// </summary>
    public class GGUIMonoCatBubbleFollowItem : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("气泡文本")]
        public Text txtBubble;
        [ALHeader("气泡切换动画")]
        public CommonAnimationShowTypeInfo<ECatBubbleFollowItemAniType> switchAni;
    }
}