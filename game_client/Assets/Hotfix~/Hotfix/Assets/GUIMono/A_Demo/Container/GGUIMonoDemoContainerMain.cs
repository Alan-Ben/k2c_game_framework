using GOE;

namespace Hotfix
{
    /// <summary>
    /// 范例包含container的主窗口mono
    /// </summary>
    public class GGUIMonoDemoContainerMain : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("测试container用")]
        public GGUIHotfixCommonMono containerMono;
    }
}