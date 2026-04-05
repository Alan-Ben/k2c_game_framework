namespace GOE
{
    /// <summary>
    /// 伙伴套系及战力详情界面页签
    /// </summary>
    public class GGUIWndHeroSuitAndPowerTab : _ATNPGGUIWndCommonTab<EHeroSuitAndPowerTabType, GGUIWndHeroSuitAndPowerTab>
    {
        public GGUIWndHeroSuitAndPowerTab(NPGGUIMonoCommonTab _wnd, EHeroSuitAndPowerTabType _bagItemType) : base(_wnd, _bagItemType)
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
