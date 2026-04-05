using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingUpgrade : _ANPGGUIBasicWnd<GGUIMonoBusinessBuildingUpgrade>
    {
        [NotNull] public static GGUIWndBusinessBuildingUpgrade instance { get { return _g_instance ??= new GGUIWndBusinessBuildingUpgrade(); } }
        private static GGUIWndBusinessBuildingUpgrade _g_instance;
        
        
        private TextureUpgradePropertyShow _m_previewUpgradeTex;
        private TextUpgradePropertyShow<int> _m_levelUpgrade;
        private TextUpgradePropertyShow<long> _m_employeeLimitUpgrade;
        private TextUpgradePropertyShow<float> _m_employeeEarningRate;
        private NPGGUIWndCommonItem _m_upgradeCost;
        
        private BusinessBuildingInfo _m_buildingInfo;
        
        
        public GGUIWndBusinessBuildingUpgrade() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingUpgrade.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingUpgrade.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_previewUpgradeTex?.showWnd();
            _m_upgradeCost?.showWnd();

            refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_CONFIRM_BTN, _onClickUpgradeBtn);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BUSINESS_BUILDING_UPGRADE_CONFIRM_BTN, _onClickUpgradeBtn);
            
            _m_previewUpgradeTex?.hideWnd();
            _m_upgradeCost?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_previewUpgradeTex?.discardTexture();
            _m_upgradeCost?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_previewUpgradeTex?.discard();
            _m_upgradeCost?.discard();
            
            _m_previewUpgradeTex = null;
            _m_upgradeCost = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickUpgradeBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoPreviewTex != null)
                _m_previewUpgradeTex = new TextureUpgradePropertyShow(wnd.monoPreviewTex);
            if (wnd.monoLevel != null)
                _m_levelUpgrade = new TextUpgradePropertyShow<int>(wnd.monoLevel, string.Empty);
            if (wnd.monoEmployeeLimit != null)
                _m_employeeLimitUpgrade = new TextUpgradePropertyShow<long>(wnd.monoEmployeeLimit, string.Empty);
            if (wnd.monoEmployeeEarningsRate != null)
                _m_employeeEarningRate = new TextUpgradePropertyShow<float>(wnd.monoEmployeeEarningsRate, TransKeyConst.common_percentage_num);
            if (wnd.monoUpgradeCost != null)
                _m_upgradeCost = new NPGGUIWndCommonItem(wnd.monoUpgradeCost);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickUpgradeBtn);
        }


        public void refreshWnd(BusinessBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.name));
            _m_previewUpgradeTex?.setValue(_m_buildingInfo.getCurrentPreviewTexIndex(), _m_buildingInfo.getNextPreviewTexIndex());
            _m_levelUpgrade?.setValue(_m_buildingInfo.level, _m_buildingInfo.level + 1);
            _m_employeeLimitUpgrade?.setValue(_m_buildingInfo.maxEmployeeNum, _m_buildingInfo.baseRef.employee_base_max_count + _m_buildingInfo.baseRef.addition_employee_count_per_level * _m_buildingInfo.level);
            _m_employeeEarningRate?.setValue(100 + (_m_buildingInfo.levelRef?.earning_rate ?? 0) / 100f, 100 + (_m_buildingInfo.nextLevelRef?.earning_rate ?? 0) / 100f);
            _m_upgradeCost?.setItem(_m_buildingInfo.levelRef.upgrade_cost_item);
        }
        
        
        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BUSINESS_BUILDING_UPGRADE);
        }
        private void _onClickUpgradeBtn()
        {
            if (wnd == null)
                return;
            
            _onClickUpgradeBtn(wnd.btnUpgrade);
        }
        private void _onClickUpgradeBtn(GameObject _)
        {
            if (_m_buildingInfo == null)
                return;

            BusinessBuildingInfo buildingInfo = _m_buildingInfo;
            NPPlayer.instance.buildingComp.reqBusinessUpgradeLvl(buildingInfo.id, _isSuc =>
            {
                if (_isSuc)
                {
                    _onClickCloseBtn(null);
                    GGUIWndBusinessBuilding.instance.setVideoIndex(0, buildingInfo.level - 1);
                    GGUIWndBusinessBuildingUpgradeSuccess.instance.refreshWnd(buildingInfo.baseRef, buildingInfo.level);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBusinessBuildingUpgradeSuccess.instance, GGUIWndBusinessBuildingUpgradeSuccess.instance.showWnd, UINodeTagConst.C_BUSINESS_BUILDING_UPGRADE_SUC);
                }
            });
        }
    }
}