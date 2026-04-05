namespace GOE
{
    /// <summary>
    /// 伙伴信息页面页签
    /// </summary>
    public class GGUIWndHeroLockInfoTab : _ATNPGGUIWndCommonTab<EHeroLockInfoTabType, GGUIWndHeroLockInfoTab>
    {
        public GGUIWndHeroLockInfoTab(NPGGUIMonoCommonTab _wnd, EHeroLockInfoTabType _bagItemType) : base(_wnd, _bagItemType)
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
