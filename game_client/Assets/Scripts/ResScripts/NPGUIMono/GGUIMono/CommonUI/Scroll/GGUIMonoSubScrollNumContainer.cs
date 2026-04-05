using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    [System.Serializable]
    public class ScrollNumDuration
    {
        [ALHeader("增加最小数量（-1代表无穷）")]
        public long minNum;
        [ALHeader("增加最大数量（-1代表无穷）")]
        public long maxNum;
        [ALHeader("在这个数量区间需要滚动几秒")]
        public float duration;
    }

    /// <summary>
    /// 展示滚动文本列表
    /// </summary>
    public class GGUIMonoSubScrollNumContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoSubScrollNumContainerItem>
    {
        [ALHeader("数字滚动曲线")]
        public AnimationCurve digitCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [ALHeader("数字滚动总时间区间")]
        public List<ScrollNumDuration> digitDurationList;
    }
}