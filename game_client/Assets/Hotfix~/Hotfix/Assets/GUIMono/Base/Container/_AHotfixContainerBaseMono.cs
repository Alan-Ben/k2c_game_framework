using GOE;
using UnityEngine.UI;

namespace Hotfix
{
    /// <summary>
    /// Hotfix里面container基类mono
    /// </summary>
    public class _AHotfixContainerBaseMono : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("拖拽的scroll rect对象")]
        public ScrollRect scrollRect;
        [HotfixMonoAttribute("单元对象存放的容器")]
        public LayoutGroup itemContainer;
        [HotfixMonoAttribute("单元模板对象")]
        public GGUIHotfixCommonMono itemTemplate;
    }
}