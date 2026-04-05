using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingHomeInfo : _ATALBasicUIWnd<GGUIMonoMarsBuildingHomeInfo>
    {
        [NotNull] public static GGUIWndMarsBuildingHomeInfo instance { get { return _g_instance ??= new GGUIWndMarsBuildingHomeInfo(); } }
        private static GGUIWndMarsBuildingHomeInfo _g_instance;
        

        private NPGGUIWndCommonToggleEx _m_switchOnToggleWnd;
        private NPGGUIWndCommonToggleEx _m_switchOverdriveToggleWnd;
        private GGUISubWndMarsBuildingHomeYieldSpeedometer _m_yieldSpeedometerWnd;
        private _IMarsBuildingView _m_homeNormalBuildingView;
        
        private new bool _m_bIsShow;


        public GGUIWndMarsBuildingHomeInfo() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingHomeInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingHomeInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_bIsShow = true;

            _m_switchOnToggleWnd?.showWnd();
            _m_switchOverdriveToggleWnd?.showWnd();
            _m_yieldSpeedometerWnd?.showWnd();

            refreshWnd();

            _trySelectBuildingView();

            NPPlayer.instance.marsComp.onOxygenValueChanged += _onOxygenValueChanged;
            NPPlayer.instance.marsComp.onTotalPeopleNumChanged += _onTotalPeopleNumChanged;

            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_HOME_INFO_SWITCH_ON_TOGGLE, _onSimulateClickSwitchOnToggle);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_HOME_INFO_SWITCH_ON_TOGGLE, _onSimulateClickSwitchOnToggle);

            NPPlayer.instance.marsComp.onTotalPeopleNumChanged -= _onTotalPeopleNumChanged;
            NPPlayer.instance.marsComp.onOxygenValueChanged -= _onOxygenValueChanged;

            _tryUnselectBuildingView();

            _m_switchOnToggleWnd?.hideWnd();
            _m_switchOverdriveToggleWnd?.hideWnd();
            _m_yieldSpeedometerWnd?.hideWnd();

            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_switchOnToggleWnd?.resetWnd();
            _m_switchOverdriveToggleWnd?.resetWnd();
            _m_yieldSpeedometerWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_switchOnToggleWnd?.discard();
            _m_switchOnToggleWnd = null;
            _m_switchOverdriveToggleWnd?.discard();
            _m_switchOverdriveToggleWnd = null;
            _m_yieldSpeedometerWnd?.discard();
            _m_yieldSpeedometerWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoSwitchOn != null)
            {
                _m_switchOnToggleWnd = new NPGGUIWndCommonToggleEx(wnd.monoSwitchOn);
                _m_switchOnToggleWnd.clickDelegate += _onSwitchOnToggleClick;
            }
            if (wnd.monoSwitchOverdrive != null)
            {
                _m_switchOverdriveToggleWnd = new NPGGUIWndCommonToggleEx(wnd.monoSwitchOverdrive);
                _m_switchOverdriveToggleWnd.clickDelegate += _onSwitchOverdriveToggleClick;
            }
            if (wnd.monoYieldSpeedometer != null)
                _m_yieldSpeedometerWnd = new GGUISubWndMarsBuildingHomeYieldSpeedometer(wnd.monoYieldSpeedometer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _tryUnselectBuildingView();
            _m_homeNormalBuildingView = _buildingView;
            _trySelectBuildingView();
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_homeNormalBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_homeNormalBuildingView.buildingInfo;
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, buildingInfo.level));
            
            refreshSwitchState(true);
            refreshOxygenValue();
            refreshTotalPeopleNum();
        }
        public void refreshSwitchState(bool _immediate = false)
        {
            if (wnd == null || !_m_bIsShow || _m_homeNormalBuildingView == null)
                return;
            
            MarsBuildingInfo buildingInfo = _m_homeNormalBuildingView.buildingInfo;
            
            bool isOn = buildingInfo.oxygenYieldProperty.isOn;
            bool isOverdrive = buildingInfo.oxygenYieldProperty.isOverdrive;
            
            _m_switchOnToggleWnd?.setSelected(isOn);
            _m_switchOverdriveToggleWnd?.setSelected(isOverdrive);
            wnd.setSwitchState(isOn, isOverdrive);
            
            long energyCost = buildingInfo.oxygenYieldProperty.energyConsumePerMin;
            ALUGUICommon.setLabelTxt(wnd.txtEnergyCost, TextTranslate.instance.getLanguage(TransKeyConst.mars_energyConsumePerMin_num, energyCost));
            
            long oxygenYield = buildingInfo.oxygenYieldProperty.valueWithoutAdjustCoefficient;
            ALUGUICommon.setLabelTxt(wnd.txtOxygenYield, oxygenYield);
            _m_yieldSpeedometerWnd?.setValue(oxygenYield, _immediate);
        }
        public void refreshOxygenValue()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            long oxygenValue = NPPlayer.instance.marsComp.oxygenValue;
            wnd.setOxygenValue(oxygenValue);
        }
        public void refreshTotalPeopleNum()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            long totalPeopleNum = NPPlayer.instance.marsComp.totalPeopleNum;
            ALUGUICommon.setLabelTxt(wnd.txtPeopleCount, totalPeopleNum);
        }
        

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_HOME_INFO);
        }
        private void _onBtnUpgradeClick(GameObject _obj)
        {
            if (_m_homeNormalBuildingView == null)
                return;
                
            _onBtnCloseClick(null);
            GGUIWndMarsBuildingHomeUpgrade.instance.refreshWnd(_m_homeNormalBuildingView);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingHomeUpgrade.instance, GGUIWndMarsBuildingHomeUpgrade.instance.showWnd,
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_HOME_UPGRADE, false, false);
        }
        private void _onSwitchOnToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_homeNormalBuildingView == null)
                return;
                
            bool isOn = !_toggle.isOn;
            bool isOverdrive = _m_homeNormalBuildingView.buildingInfo.oxygenYieldProperty.isOverdrive;
            
            NPPlayer.instance.marsComp.buildingSubComponent.setSwitches(_m_homeNormalBuildingView.buildingInfo.refObj.id, isOn, isOverdrive);
            refreshSwitchState();
        }
        private void _onSwitchOverdriveToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_homeNormalBuildingView == null)
                return;

            bool isOn = _m_homeNormalBuildingView.buildingInfo.oxygenYieldProperty.isOn;
            bool isOverdrive = !_toggle.isOn;
            
            if (isOverdrive && !isOn)
                isOn = true;

            NPPlayer.instance.marsComp.buildingSubComponent.setSwitches(_m_homeNormalBuildingView.buildingInfo.refObj.id, isOn, isOverdrive);
            refreshSwitchState();
        }
        private void _trySelectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_homeNormalBuildingView == null)
                return;

            _m_homeNormalBuildingView.setSelected(true);
            MainAdditionMarsTDScene.instance.focusToPos(_m_homeNormalBuildingView.position, wnd.focusViewportPos, wnd.focusScale, wnd.focusTime);
        }
        private void _tryUnselectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_homeNormalBuildingView == null)
                return;

            _m_homeNormalBuildingView?.setSelected(false);
            MainAdditionMarsTDScene.instance.cancelThePosFocus(wnd.focusTime);
        }
        
        private void _onOxygenValueChanged(long _)
        {
            refreshOxygenValue();
        }
        private void _onTotalPeopleNumChanged(long _)
        {
            refreshTotalPeopleNum();
        }

        private void _onSimulateClickSwitchOnToggle()
        {
            if (_m_switchOnToggleWnd == null)
                return;
            _onSwitchOnToggleClick(_m_switchOnToggleWnd);
        }
    }
}