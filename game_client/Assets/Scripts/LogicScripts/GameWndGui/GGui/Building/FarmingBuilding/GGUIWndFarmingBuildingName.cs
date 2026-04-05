using ALPackage;

namespace GOE
{
    public class GGUIWndFarmingBuildingNameFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoFarmingBuildingName, GGUIWndFarmingBuildingName>
    {
        private readonly GResPathIndex _m_resIndex;
        private FarmingBuildingInfo _m_buildingInfo;
        
        
        public GGUIWndFarmingBuildingNameFollowItemController()
        {
            _m_resIndex = new GResPathIndex(1112);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndFarmingBuildingName _createItemWnd(GGUIMonoFarmingBuildingName _wndMono)
        {
            GGUIWndFarmingBuildingName wnd = new GGUIWndFarmingBuildingName(_wndMono);
            wnd.refreshWnd(_m_buildingInfo);
            wnd.showWnd();
            return wnd;
        }
        
        public void setBuildingInfo(FarmingBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            wnd?.refreshWnd(_m_buildingInfo);
        }
        public void refreshWnd()
        {
            if (wnd == null)
                return;

            wnd.refreshWnd();
        }
    }
    public class GGUIWndFarmingBuildingName : _ATALGGUIWndCommonFollowItem<GGUIMonoFarmingBuildingName>
    {
        private FarmingBuildingInfo _m_buildingInfo;
        
        
        public GGUIWndFarmingBuildingName(GGUIMonoFarmingBuildingName _wnd) : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd(FarmingBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtNameAndLevel,
                _m_buildingInfo != null
                    ? TextTranslate.instance.getLanguage(TransKeyConst.building_hudName_str_num, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.name), _m_buildingInfo.level)
                    : string.Empty);
        }
    }
}