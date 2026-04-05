namespace GOE
{
    /// <summary>
    /// 藏品主页面页签
    /// </summary>
    public class GGUIWndEquipMainListTab : _ATNPGGUIWndCommonTab<EEquipMainTabType, GGUIWndEquipMainListTab>
    {
        public GGUIWndEquipMainListTab(NPGGUIMonoCommonTab _wnd, EEquipMainTabType _bagItemType) : base(_wnd, _bagItemType)
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
