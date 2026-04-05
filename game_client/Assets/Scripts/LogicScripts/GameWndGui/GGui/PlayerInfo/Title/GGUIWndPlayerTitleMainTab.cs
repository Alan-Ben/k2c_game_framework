namespace GOE
{
    /// <summary>
    /// 玩家称号详情界面页签
    /// </summary>
    public class GGUIWndPlayerTitleMainTab : _ATNPGGUIWndCommonTab<EPlayerTitleTabType, GGUIWndPlayerTitleMainTab>
    {
        public GGUIWndPlayerTitleMainTab(NPGGUIMonoCommonTab _wnd, EPlayerTitleTabType _bagItemType) : base(_wnd, _bagItemType)
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
