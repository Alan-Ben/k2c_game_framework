
using ALPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBuildingUnbuiltNonsense : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("碎碎念的内容")]
        public TextMeshProUGUI txtNonsense;
        [ALHeader("显示时的动画，动画结束后会自动销毁")]
        public Animation showAnimation;
        public string showAnimationName;
        [ALHeader("延迟多久删除")]
        public float deleteDelay;
    }
}