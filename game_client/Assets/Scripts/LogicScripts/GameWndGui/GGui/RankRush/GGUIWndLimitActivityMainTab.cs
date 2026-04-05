namespace GOE
{
    /// <summary>
    /// 限时活动主界面页签
    /// </summary>
    public class GGUIWndLimitActivityMainTab : _ATNPGGUIWndCommonTab<ELimitActivityTabType, GGUIWndLimitActivityMainTab>
    {
        public GGUIWndLimitActivityMainTab(NPGGUIMonoCommonTab _wnd, ELimitActivityTabType _tabType) : base(_wnd, _tabType)
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
