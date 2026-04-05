using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingBuildBtnFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingBuildBtn, GGUIWndMarsBuildingBuildBtnFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingBuildBtnFollowerController()
        {
            _m_resIndex = new GResPathIndex(7102);
        }
        public GGUIWndMarsBuildingBuildBtnFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingBuildBtnFollower _createItemWnd(GGUIMonoMarsBuildingBuildBtn _wndMono)
        {
            GGUIWndMarsBuildingBuildBtnFollower wnd = new GGUIWndMarsBuildingBuildBtnFollower(_wndMono);
            wnd.refreshWnd(_m_buildingView);
            wnd.showWnd();
            return wnd;
        }
        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            wnd?.refreshWnd(_m_buildingView);
        }
    }

    public class GGUIWndMarsBuildingBuildBtnFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingBuildBtn>
    {
        private _IMarsBuildingView _m_buildingView;
        
        
        public GGUIWndMarsBuildingBuildBtnFollower(GGUIMonoMarsBuildingBuildBtn _wnd) : base(_wnd)
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

            ALUGUICommon.uncombineBtnClick(wnd.btnBuild, _onBtnBuildClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnBuild, _onBtnBuildClick);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_buildingView == null || _m_buildingView.buildingInfo == null)
                return;

            bool canBuild = NPPlayer.instance.marsComp.buildingSubComponent.hasLeisureQueue &&
                            _m_buildingView.buildingInfo.state == MarsBuildingInfo.StateType.Unbuilt &&
                            _m_buildingView.buildingInfo.checkCanBuild(true);
            
            ALUGUICommon.setGameObjEnable(wnd.canBuildShowGoList, canBuild);
        }


        private void _onBtnBuildClick(GameObject _obj)
        {
            _m_buildingView?.triggerClick();
        }
    }
}