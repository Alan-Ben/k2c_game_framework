using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 连续点击按钮
    /// </summary>
    public class GGUIMonoContinuousClickBtn : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALInfo("下面两个配置为在n秒内连续点击m次")]
        [ALHeader("(n)需要特殊处理的连续点击间隔时间（秒）")]
        public float durationTimeSec;
        [ALHeader("(m)间隔时间内触发特殊处理的点击次数")]
        [Min(2)]
        public int continuousClickCount=2;
    }
}

