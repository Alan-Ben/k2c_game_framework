using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用事件窗口
    /// </summary>
    public class _AGGUIMonoCommonSimpleEvent : _AALBasicUIWndMono
    {
        [ALHeader("事件名")]
        public TextEx txtEventName;
        [ALHeader("事件详细描述")]
        public TextEx txtEventDetailDesc;
    }
}