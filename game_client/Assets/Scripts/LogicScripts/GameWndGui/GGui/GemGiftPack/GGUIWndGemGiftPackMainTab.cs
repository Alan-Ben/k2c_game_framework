namespace GOE
{
    /// <summary>
    /// 钻石礼包主页面页签
    /// </summary>
    public class GGUIWndGemGiftPackMainTab : _ATNPGGUIWndCommonTab<EGemGiftPackMainTabType, GGUIWndGemGiftPackMainTab>
    {
        public GGUIWndGemGiftPackMainTab(NPGGUIMonoCommonTab _wnd, EGemGiftPackMainTabType _tabType) : base(_wnd, _tabType)
        {
        }

        /// <summary>
        /// 设置点击tab
        /// </summary>
        public void setClickTab()
        {
            _onClickSelectButton(null);
        }
    }
}
