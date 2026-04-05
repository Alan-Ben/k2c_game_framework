namespace GOE
{
    /// <summary>
    /// 竞技场战报界面页签
    /// </summary>
    public class GGUIWndArenaBattleReportTab : _ATNPGGUIWndCommonTab<EArenaBattleReportTabType, GGUIWndArenaBattleReportTab>
    {
        public GGUIWndArenaBattleReportTab(NPGGUIMonoCommonTab _wnd, EArenaBattleReportTabType _bagItemType) : base(_wnd, _bagItemType)
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
