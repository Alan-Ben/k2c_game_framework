namespace GOE
{
    /// <summary>
    /// 玩家组合称号界面页签
    /// </summary>
    public class GGUIWndPlayerTitleComboPageTab : _ATNPGGUIWndCommonTab<EPlayerTitleComboTabType, GGUIWndPlayerTitleComboPageTab>
    {
        public GGUIWndPlayerTitleComboPageTab(NPGGUIMonoCommonTab _wnd, EPlayerTitleComboTabType _bagItemType) : base(_wnd, _bagItemType)
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
