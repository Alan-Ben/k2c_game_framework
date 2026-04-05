namespace GOE
{
    /// <summary>
    /// 藏品详情页签
    /// </summary>
    public class GGUIWndEquipDetailPageTab : _ATNPGGUIWndCommonTab<EEquipDetailTabType, GGUIWndEquipDetailPageTab>
    {
        public GGUIWndEquipDetailPageTab(NPGGUIMonoCommonTab _wnd, EEquipDetailTabType _bagItemType) : base(_wnd, _bagItemType)
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
