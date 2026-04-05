namespace GOE
{
    /// <summary>
    /// 冲榜详情主界面页签
    /// </summary>
    public class GGUIWndRankRushDetailTab : _ATNPGGUIWndCommonTab<ERankRushDetailTabType, GGUIWndRankRushDetailTab>
    {
        public GGUIWndRankRushDetailTab(NPGGUIMonoCommonTab _wnd, ERankRushDetailTabType _tabType) : base(_wnd, _tabType)
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
