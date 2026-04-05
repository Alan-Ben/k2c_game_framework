using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 粮食入口跟随tip
    /// </summary>
    public class GGUIMonoLevyFoodPointItem : _AGGUIMonoEntryPointFollowItemBase
    {
        [ALHeader("弹出tip位置")]
        public RectTransform tipStartPos;

        [ALHeader("上浮提示显示参数")]
        [ALHeader("最高速率")]
        public float tipAccMaxRate = 1.0f;

        [ALHeader("待显示tip达到这个数开始加速")]
        public int tipAccMinNum;

        [ALHeader("待显示tip达到这个数达到最高速")]
        public int tipAccMaxNum;
    }
}