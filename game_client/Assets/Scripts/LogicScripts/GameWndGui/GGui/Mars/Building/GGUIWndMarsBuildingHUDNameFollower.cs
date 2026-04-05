using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingHUDNameFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingHUDName, GGUIWndMarsBuildingHUDNameFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingHUDNameFollowerController()
        {
            _m_resIndex = new GResPathIndex(7106);
        }
        public GGUIWndMarsBuildingHUDNameFollowerController(int _uiId)
        {
            _m_resIndex = new GResPathIndex(_uiId <= 0 ? 7106 : _uiId);
        }
        public GGUIWndMarsBuildingHUDNameFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingHUDNameFollower _createItemWnd(GGUIMonoMarsBuildingHUDName _wndMono)
        {
            GGUIWndMarsBuildingHUDNameFollower wnd = new GGUIWndMarsBuildingHUDNameFollower(_wndMono);
            wnd.refreshWnd(_m_buildingView);
            wnd.showWnd();
            return wnd;
        }
        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            wnd?.refreshWnd(_m_buildingView);
        }
        public void refreshUpgradeState()
        {
            wnd?.refreshUpgradeState();
        }
    }

    public class GGUIWndMarsBuildingHUDNameFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingHUDName>
    {
        private _IMarsBuildingView _m_buildingView;
        
        
        public GGUIWndMarsBuildingHUDNameFollower(GGUIMonoMarsBuildingHUDName _wnd) : base(_wnd)
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
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            ALUGUICommon.setLabelTxt(wnd.txtName, buildingInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtNameTMP, buildingInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, buildingInfo.level);
            ALUGUICommon.setLabelTxt(wnd.txtLevelTMP, buildingInfo.level);
            refreshUpgradeState();
        }
        public void refreshUpgradeState()
        {
            if (wnd == null || _m_buildingView?.buildingInfo == null)
                return;
            
            wnd.setUpgradable(_m_buildingView.buildingInfo.equipmentData.equipmentLevelProgressIsComplete && _m_buildingView.buildingInfo.checkCanUpgrade());
        }


        private void _onBtnClick(GameObject _)
        {
            _m_buildingView?.triggerClick();
        }
    }
}