using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 子嗣配音气泡附加窗口
    /// </summary>
    public class GGUIMonoChildVoiceBubble : _AALBasicUIWndMono
    {
        [ALHeader("半身像")]
        public RawImage imgCardIcon;
        [ALHeader("头像")]
        public RawImage imgIcon;
        [ALHeader("头像背景")]
        public Image imgIconBg;
        [ALHeader("打字机附加窗口")]
        public GGUIMonoTextTypewriter monoTypewriter;
        [ALHeader("文本显示完气泡再隐藏的延迟时间")]
        public float playDoneDelayHideBubble = 2f;
    }
}