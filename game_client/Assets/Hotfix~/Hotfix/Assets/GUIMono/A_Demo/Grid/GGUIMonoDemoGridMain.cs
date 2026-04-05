using GOE;

namespace Hotfix
{
    /// <summary>
    /// 范例包含grid的主窗口mono
    /// </summary>
    public class GGUIMonoDemoGridMain : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("测试grid用")]
        public GGUIHotfixGridMono gridMono;
        [HotfixMonoAttribute("测试grid包含bar用")]
        public GGUIHotfixGridMono gridWithBarMono;
    }
}