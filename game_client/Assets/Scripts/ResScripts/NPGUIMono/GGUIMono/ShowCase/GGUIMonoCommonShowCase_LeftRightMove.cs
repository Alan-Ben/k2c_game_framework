using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 支持左右滑动的showcase
    /// </summary>
    public class GGUIMonoCommonShowCase_LeftRightMove : GGUIMonoCommonShowCase
    {
        [ALHeader("滑动多少距离算是移动下一个")]
        [Range(0, 1000)]
        public float nextOffsetDistance = 300;
        [ALHeader("回正自动滑动时间（秒）")]
        public float moveTimeS = 0.5f;
    }
}