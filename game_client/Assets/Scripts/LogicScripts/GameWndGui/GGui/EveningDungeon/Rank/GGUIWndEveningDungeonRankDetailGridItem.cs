namespace GOE
{
    /// <summary>
    /// 晚间副本排行榜详情item
    /// </summary>
    public class GGUIWndEveningDungeonRankDetailGridItem : _AGGUIWndBaseSubRankPlayerInfo<GGUIMonoEveningDungeonRankDetailGridItem, EveningDungeonRankInfo>
    {
        public GGUIWndEveningDungeonRankDetailGridItem(GGUIMonoEveningDungeonRankDetailGridItem  _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
        }

        protected override void _onWndInitDoneEx()
        {
        }
    }
}