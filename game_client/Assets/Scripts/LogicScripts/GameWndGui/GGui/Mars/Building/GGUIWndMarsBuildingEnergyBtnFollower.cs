using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingEnergyBtnFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingEnergyBtn, GGUIWndMarsBuildingEnergyBtnFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingEnergyBtnFollowerController()
        {
            _m_resIndex = new GResPathIndex(7118);
        }
        public GGUIWndMarsBuildingEnergyBtnFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            wnd?.refreshWnd(_m_buildingView);
        }
        public void tick()
        {
            wnd?.refreshWnd();
        }

        protected override GGUIWndMarsBuildingEnergyBtnFollower _createItemWnd(GGUIMonoMarsBuildingEnergyBtn _wndMono)
        {
            GGUIWndMarsBuildingEnergyBtnFollower wnd = new GGUIWndMarsBuildingEnergyBtnFollower(_wndMono);
            wnd.refreshWnd(_m_buildingView);
            wnd.showWnd();
            return wnd;
        }
    }

    public class GGUIWndMarsBuildingEnergyBtnFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingEnergyBtn>
    {
        private _IMarsBuildingView _m_buildingView;
        private bool _m_showProgress;
        private bool _m_isFullEnergy;


        public GGUIWndMarsBuildingEnergyBtnFollower(GGUIMonoMarsBuildingEnergyBtn _wnd) : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _setProgressThresholdShow(_m_showProgress);
            _setFullEnergyShow(_m_isFullEnergy);
            
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

            ALUGUICommon.uncombineBtnClick(wnd.btnGetEnergy, _onBtnGetEnergyClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnGetEnergy, _onBtnGetEnergyClick);
        }
        

        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            
            // Get energy data from building's energy property
            long storedEnergy = buildingInfo.energyProperty.storedEnergy;
            long maxStorage = buildingInfo.energyProperty.maxStorage;
            
            float energyProgress = maxStorage > 0 ? (float)storedEnergy / maxStorage : 0f;
            bool isFullEnergy = storedEnergy >= maxStorage;
            bool showProgress = energyProgress >= GRefdataCoreMgr.instance.npGeneral.mars_can_get_energy_progress_threshold;

            // Update progress bar
            if (wnd.sldEnergyProgress != null)
                wnd.sldEnergyProgress.normalizedValue = energyProgress;

            if (_m_showProgress != showProgress)
            {
                _m_showProgress = showProgress;
                _setProgressThresholdShow(_m_showProgress);
            }

            if (_m_isFullEnergy != isFullEnergy)
            {
                _m_isFullEnergy = isFullEnergy;
                _setFullEnergyShow(_m_isFullEnergy);
            }
        }
        

        private void _onBtnGetEnergyClick(GameObject _obj)
        {
            if (_m_buildingView == null)
                return;
                
            WinMsg.SendMsg(WinMsgType.TRIGGER_MARS_ENERGY_COLLECT);
        }
        private void _setProgressThresholdShow(bool _show)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.listOnProgressThresholdShow, false);
            ALUGUICommon.setGameObjEnable(wnd.listOnProgressThresholdHide, false);
            ALUGUICommon.setGameObjEnable(_show ? wnd.listOnProgressThresholdShow : wnd.listOnProgressThresholdHide, true);
        }
        private void _setFullEnergyShow(bool _isFull)
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.listFullEnergyShow, false);
            ALUGUICommon.setGameObjEnable(wnd.listFullEnergyHide, false);
            ALUGUICommon.setGameObjEnable(_isFull ? wnd.listFullEnergyShow : wnd.listFullEnergyHide, true);
        }
    }
}