using GOE;

namespace Hotfix
{
    /// <summary>
    /// Hotfix 滚动列表 可播放item显示动画的 container基类mono
    /// </summary>
    public class _AHotfixShowAnimContainerBaseMono : _AHotfixContainerBaseMono
    {
        [HotfixMonoAttribute("是否开启显示窗口逐个播放item的显示动画")]
        public bool openItemShowAnim = false;
        [HotfixMonoAttribute("显示窗口的第一批Item播放刷新动画的播放间隔")]
        public float firstItemShowAnimDelay = 0.2f;
    }
}