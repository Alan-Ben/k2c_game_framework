using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBuildingCoinEarningTip : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("金币的数值")]
        public Text txtCoinNum;
        [ALHeader("显示时的动画，动画结束后会自动销毁")]
        public Animation showAnimation;
        public string showAnimationName;
        [ALHeader("延迟多久删除")]
        public float deleteDelay;
        [ALHeader("位置的随机范围和偏移的父节点")]
        public float randomRadius;
        public Transform offsetParent;
    }
}