namespace GOE
{
    /// <summary>
    /// 分享页签
    /// </summary>
    public class NPGGUIWndCommonShareTab : _ATNPGGUIWndCommonTab<ENPGGUICommonShareTabType, NPGGUIWndCommonShareTab>
    {
        public NPGGUIWndCommonShareTab(NPGGUIMonoCommonTab _wnd, ENPGGUICommonShareTabType _shareTabType) : base(_wnd, _shareTabType)
        {
            initWnd();
        }
    }
}
