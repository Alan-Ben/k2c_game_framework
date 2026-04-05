namespace GOE
{
    public class GGUIWndEveningDungeonRankAndRewardDetailTab : _ATNPGGUIWndCommonTab<EEveningDungeonRankAndRewardDetailTabType, GGUIWndEveningDungeonRankAndRewardDetailTab>
    {
        public GGUIWndEveningDungeonRankAndRewardDetailTab(NPGGUIMonoCommonTab _wnd, EEveningDungeonRankAndRewardDetailTabType _tabType) : base(_wnd, _tabType)
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