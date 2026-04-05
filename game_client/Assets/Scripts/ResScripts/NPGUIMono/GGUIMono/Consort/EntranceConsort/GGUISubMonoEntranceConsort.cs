using ALPackage;

namespace GOE
{
    /// <summary>
    /// 入口妃子显示子窗口
    /// </summary>
    public class GGUISubMonoEntranceConsort : _AALBasicUIWndMono
    {
        [ALHeader("默认显示的妃子id")]
        public long defaultShowConsortId = 0;
        
        [ALHeader("妃子tdShow")]
        public GGUIMonoConsortShowCaseSubWnd monoConsortShowCase;
        [ALHeader("初始播放的动画")]
        public string startAniTag;
        
        [ALHeader("打字机附加窗口")]
        public GGUIMonoTextTypewriter monoTypewriter;
        
        [ALHeader("文本显示完气泡隐藏的延迟时间")]
        public float hideBubbleDelay = 3f;
        [ALHeader("气泡隐藏后再显示下一个气泡的延迟时间")]
        public float showNextBubbleDelay = 3f;
    }
}