using ALPackage;

namespace GOE
{
    public class _AGGUIMonoPosFixContainer<_T_ITEM_MONO> : _ATNPGGUIMonoShowAnimContainer<_T_ITEM_MONO>
        where _T_ITEM_MONO : _AALBasicUIWndMono
    {
        [ALHeader("修正位置的持续时间")]
        public float fixPosDuration;
        [ALHeader("修正时使用的缓动类型")]
        public EaseType easeType;
        [ALHeader("拖拽结束时的速度阈值")]
        public float dragEndVelocityThreshold;
    }
}