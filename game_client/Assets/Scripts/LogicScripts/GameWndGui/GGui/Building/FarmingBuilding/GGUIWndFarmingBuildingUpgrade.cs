using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndFarmingBuildingUpgrade : _ANPGGUIBasicWnd<GGUIMonoFarmingBuildingUpgrade>
    {
        [NotNull] public static GGUIWndFarmingBuildingUpgrade instance { get { return _g_instance ??= new GGUIWndFarmingBuildingUpgrade(); } }
        private static GGUIWndFarmingBuildingUpgrade _g_instance;
        
        
        private TextureUpgradePropertyShow _m_previewUpgradeTex;
        private TextUpgradePropertyShow<int> _m_levelUpgrade;
        private TextUpgradePropertyShow<float> _m_bonusUpgrade;
        private TextUpgradePropertyShow<string> _m_clickEarningsUpgrade;
        private TextUpgradePropertyShow<int> _m_autoClickPerSecondUpgrade;
        private NPGGUIWndCommonItem _m_upgradeCost;
        
        private FarmingBuildingInfo _m_buildingInfo;


        public GGUIWndFarmingBuildingUpgrade() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoFarmingBuildingUpgrade.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFarmingBuildingUpgrade.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_previewUpgradeTex?.showWnd();
            _m_upgradeCost?.showWnd();
            
            refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_FARMING_BUILDING_UPGRADE_BTN, _onClickUpgradeBtn);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_FARMING_BUILDING_UPGRADE_BTN, _onClickUpgradeBtn);
            
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
            if (wnd.monoBonus != null)
                _m_bonusUpgrade = new TextUpgradePropertyShow<float>(wnd.monoBonus, TransKeyConst.common_percentage_num);
            if (wnd.monoClickEarnings != null)
                _m_clickEarningsUpgrade = new TextUpgradePropertyShow<string>(wnd.monoClickEarnings, string.Empty);
            if (wnd.monoAutoClickPerS != null)
                _m_autoClickPerSecondUpgrade = new TextUpgradePropertyShow<int>(wnd.monoAutoClickPerS, string.Empty);
            if (wnd.monoUpgradeCost != null)
                _m_upgradeCost = new NPGGUIWndCommonItem(wnd.monoUpgradeCost);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickUpgradeBtn);
        }
        
        
        public void refreshWnd(FarmingBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo?.levelData == null)
                return;
            
            FarmingBuildingLevelData nextLevelData = _m_buildingInfo.nextLevelData ?? _m_buildingInfo.levelData;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.name));
            _m_levelUpgrade?.setValue(_m_buildingInfo.level, nextLevelData.level);
            _m_bonusUpgrade?.setValue((_m_buildingInfo.levelData.earning_rate / 100f), (nextLevelData.earning_rate / 100f));
            _m_clickEarningsUpgrade?.setValue(_m_buildingInfo.levelData.tap_to_collect_num.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD), nextLevelData.tap_to_collect_num.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
            _m_autoClickPerSecondUpgrade?.setValue(_m_buildingInfo.levelData.auto_tap_num_per_sec, nextLevelData.auto_tap_num_per_sec);
            _m_upgradeCost?.setItem(_m_buildingInfo.levelData.upgrade_cost_item);
            _m_previewUpgradeTex?.setValue(_m_buildingInfo.getCurrentPreviewTexIndex(), _m_buildingInfo.getNextPreviewTexIndex());
            wnd.setLevelMax(_m_buildingInfo.levelMax);
        }
        
        
        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.forceCloseLastNode();
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
            
            NPPlayer.instance.buildingComp.reqFarmUpgradeLvl(_m_buildingInfo.id, _isSuc =>
            {
                if (_isSuc)
                    _onClickCloseBtn(null);
            });
        }
    }
}