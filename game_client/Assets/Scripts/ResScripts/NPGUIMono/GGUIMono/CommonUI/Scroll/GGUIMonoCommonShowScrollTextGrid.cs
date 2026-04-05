using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 展示滚动文本列表
    /// </summary>
    public class GGUIMonoCommonShowScrollTextGrid : _TALUGUIMonoGridWnd<GGUIMonoCommonShowScrollTextGridItem>
    {
        [ALHeader("滚动速度曲线")]
        public AnimationCurve scrollSpeedCurve = AnimationCurve.Linear(0,0,1,1);
    }
}