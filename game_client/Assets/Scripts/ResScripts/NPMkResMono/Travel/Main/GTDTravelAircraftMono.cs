using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历飞机状态
    /// </summary>
    public enum ETravelAircraftState
    {
        NONE,
        Idle,//空闲
        Moving,//移动中
    }
    
    public class GTDTravelAircraftMono : MonoBehaviour
    {
        [ALHeader("飞行时间")]
        public float flyTime;

        [ALHeader("贝塞尔曲线控制点距离[起点终点连线]的距离 / (起点终点连线距离 / 2) 的比例范围，简单理解：越靠近0曲度越小")]
        public WCGFloatRange bezierControlPointDisRate = new WCGFloatRange(0.1f, 1f);
        
        [ALHeader("飞机动画")]
        public Animation animation;
        [ALHeader("飞行动画名")]
        public string flyAniName;
    }
}