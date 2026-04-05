using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public enum ENPScrollCheckDirectType
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

    /// <summary>
    /// 监视滚动条并触发刷新事件的窗体
    /// </summary>
    public class NPGGUIMonoScrollRefreshMonitor : _AALBasicUIWndMono
    {
        [ALHeader("检测的滚动条")]
        public ScrollRect scrollRect;
        [ALHeader("检测方向类型")]
        public ENPScrollCheckDirectType directType;
        [ALHeader("检测的距离，在检测方向上拖动超过边界这个距离则触发")]
        public float checkDistance = 30f;
    }
}
