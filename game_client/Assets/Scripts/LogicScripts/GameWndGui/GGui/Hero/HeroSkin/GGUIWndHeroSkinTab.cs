namespace GOE
{
    /// <summary>
    /// 伙伴皮肤界面页签
    /// </summary>
    public class GGUIWndHeroSkinTab : _ATNPGGUIWndCommonTab<EHeroSkinTabType, GGUIWndHeroSkinTab>
    {
        public GGUIWndHeroSkinTab(NPGGUIMonoCommonTab _wnd, EHeroSkinTabType _bagItemType) : base(_wnd, _bagItemType)
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
