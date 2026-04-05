namespace GOE
{
    /// <summary>
    /// 伙伴信息页面页签
    /// </summary>
    public class GGUIWndHeroInfoTab : _ATNPGGUIWndCommonTab<EHeroInfoTabType, GGUIWndHeroInfoTab>
    {
        public GGUIWndHeroInfoTab(NPGGUIMonoCommonTab _wnd, EHeroInfoTabType _bagItemType) : base(_wnd, _bagItemType)
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
