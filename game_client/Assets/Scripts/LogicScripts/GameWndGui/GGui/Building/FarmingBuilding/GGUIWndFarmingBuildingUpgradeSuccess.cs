using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndFarmingBuildingUpgradeSuccess : _ANPGGUIBasicWnd<GGUIMonoFarmingBuildingUpgradeSuccess>
    {
        [NotNull] public static GGUIWndFarmingBuildingUpgradeSuccess instance { get { return _g_instance ??= new GGUIWndFarmingBuildingUpgradeSuccess(); } }
        private static GGUIWndFarmingBuildingUpgradeSuccess _g_instance;
        
        
        private TextureUpgradePropertyShow _m_previewUpgradeTex;
        private TextUpgradePropertyShow<int> _m_levelUpgrade;
        private TextUpgradePropertyShow<float> _m_bonusUpgrade;
        private TextUpgradePropertyShow<string> _m_clickEarningsUpgrade;
        private TextUpgradePropertyShow<int> _m_autoClickPerSecondUpgrade;
        
        private FarmingBuildingRefObj _m_buildingRef;
        private int _m_currentLevel;
        private FarmingBuildingLevelData _m_lastLevelData;
        private FarmingBuildingLevelData _m_currentLevelData;
        

        public GGUIWndFarmingBuildingUpgradeSuccess() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoFarmingBuildingUpgradeSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFarmingBuildingUpgradeSuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_previewUpgradeTex?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_previewUpgradeTex?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_previewUpgradeTex?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_previewUpgradeTex?.discard();
            _m_previewUpgradeTex = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
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
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }


        public void refreshWnd(FarmingBuildingRefObj _buildingRef, int _currentLevel)
        {
            _m_buildingRef = _buildingRef;
            _m_currentLevel = _currentLevel;
            if (_m_buildingRef != null)
            {
                FarmingBuildingLevelRefObj lastLevelRef = GRefdataCoreMgr.instance.getFarmingBuildingLevelRefObj(_m_buildingRef.building_id, _currentLevel - 1);
                if (lastLevelRef != null)
                    _m_lastLevelData = new FarmingBuildingLevelData(_buildingRef, lastLevelRef, _currentLevel - 1);
                FarmingBuildingLevelRefObj currentLevelRef = GRefdataCoreMgr.instance.getFarmingBuildingLevelRefObj(_m_buildingRef.building_id, _currentLevel);
                if (currentLevelRef != null)
                    _m_currentLevelData = new FarmingBuildingLevelData(_buildingRef, currentLevelRef, _currentLevel);
            }
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingRef == null || _m_lastLevelData == null || _m_currentLevelData == null)
                return;
            
            _m_previewUpgradeTex?.setValue(_m_lastLevelData.preview_tex_index, _m_currentLevelData.preview_tex_index);
            ALUGUICommon.setLabelTxt(wnd.txtUpgradeDesc, TextTranslate.instance.getLanguage(_m_buildingRef.level_up_desc));
            _m_levelUpgrade?.setValue(_m_lastLevelData.level, _m_lastLevelData.level + 1);
            _m_bonusUpgrade?.setValue((_m_lastLevelData.earning_rate / 100f), (_m_currentLevelData.earning_rate / 100f));
            _m_clickEarningsUpgrade?.setValue(_m_lastLevelData.tap_to_collect_num.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD), _m_currentLevelData.tap_to_collect_num.ToLargeString((PrimitiveExtension.ELargeStringType.GOLD)));
            _m_autoClickPerSecondUpgrade?.setValue(_m_lastLevelData.auto_tap_num_per_sec, _m_currentLevelData.auto_tap_num_per_sec);
        }
        
        
        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}