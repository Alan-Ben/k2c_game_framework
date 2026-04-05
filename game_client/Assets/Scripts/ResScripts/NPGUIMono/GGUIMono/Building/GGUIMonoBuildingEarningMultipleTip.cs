using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 建筑收益暴击倍数提示tip
    /// </summary>
    public class GGUIMonoBuildingEarningMultipleTip : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("倍数数值")]
        public Text txtMultipleNum;
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