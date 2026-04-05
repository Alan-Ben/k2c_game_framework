using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子showcase展示子窗口
    /// </summary>
    public class GGUIMonoConsortShowCaseSubWnd : _AALBasicUIWndMono
    {
        [ALHeader("妃子td showcase")]
        public GGUIMonoCommonShowCase monoShowcase;
                
        [ALHeader("进入时候妃子播放的动画名")]
        public string enterAniName; 
        
        
        [ALHeader("妃子形象在td showcase中的index(配置小于0的值时不显示)")]
        public int consortActorInShowCaseIndex = 0;
        [ALHeader("背景在td showcase中的index(配置小于0的值时不显示)")]
        public int bgInShowCaseIndex = 3;
    }
}