using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingUpgradeSuccess : _ANPGGUIBasicWnd<GGUIMonoBusinessBuildingUpgradeSuccess>
    {
        [NotNull] public static GGUIWndBusinessBuildingUpgradeSuccess instance { get { return _g_instance ??= new GGUIWndBusinessBuildingUpgradeSuccess(); } }
        private static GGUIWndBusinessBuildingUpgradeSuccess _g_instance;
        
        
        private TextureUpgradePropertyShow _m_previewTex;
        private TextUpgradePropertyShow<int> _m_level;
        private TextUpgradePropertyShow<long> _m_employeeLimit;
        private TextUpgradePropertyShow<float> _m_employeeEarningsRate;
        private NPGGuiWndTexture _m_videoPreview;
        
        private BusinessBuildingRefObj _m_buildingRef;
        private int _m_currentLevel;
        private BusinessBuildingLevelRefObj _m_lastLevelRef;
        private BusinessBuildingLevelRefObj _m_currentLevelRef;
        
        
        public GGUIWndBusinessBuildingUpgradeSuccess() 
            : base(EALUIWndLayer.ADDITION)
        {
        }


        /// <summary>
        /// 当窗口关闭时触发，参数为是否有动画变更
        /// </summary>
        public event Action<bool, BusinessBuildingDevelopRefObj> onWndHide; 


        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingUpgradeSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingUpgradeSuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_previewTex?.showWnd();
            _m_videoPreview?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_previewTex?.hideWnd();
            _m_videoPreview?.hideWnd();
            
            BusinessBuildingDevelopRefObj lastDevelopRef = _getNewestDevelopRef(_m_lastLevelRef);
            BusinessBuildingDevelopRefObj currentDevelopRef = _getNewestDevelopRef(_m_currentLevelRef);
            onWndHide?.Invoke(lastDevelopRef != currentDevelopRef, currentDevelopRef);
        }
        protected override void _onReset()
        {
            _m_previewTex?.discardTexture();
            _m_videoPreview?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_previewTex?.discard();
            _m_previewTex = null;
            _m_videoPreview?.discard();
            _m_videoPreview = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoPreviewTex != null)
                _m_previewTex = new TextureUpgradePropertyShow(wnd.monoPreviewTex);
            if (wnd.monoLevel != null)
                _m_level = new TextUpgradePropertyShow<int>(wnd.monoLevel, string.Empty);
            if (wnd.monoEmployeeLimit != null)
                _m_employeeLimit = new TextUpgradePropertyShow<long>(wnd.monoEmployeeLimit, string.Empty);
            if (wnd.monoEmployeeEarningsRate != null)
                _m_employeeEarningsRate = new TextUpgradePropertyShow<float>(wnd.monoEmployeeEarningsRate, TransKeyConst.common_percentage_num);
            if (wnd.imgVideoPreview != null)
                _m_videoPreview = new NPGGuiWndTexture(wnd.imgVideoPreview);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        
        
        public void refreshWnd(BusinessBuildingRefObj _buildingRef, int _currentLevel)
        {
            _m_buildingRef = _buildingRef;
            _m_currentLevel = _currentLevel;
            _m_lastLevelRef = GRefdataCoreMgr.instance.getBusinessBuildingLevelRef(_buildingRef?.building_id ?? 0, _m_currentLevel - 1);
            _m_currentLevelRef = GRefdataCoreMgr.instance.getBusinessBuildingLevelRef(_buildingRef?.building_id ?? 0, _m_currentLevel);
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingRef == null || _m_lastLevelRef == null || _m_currentLevelRef == null)
                return;
            
            _m_previewTex?.setValue(_getTextIndex(_m_lastLevelRef), _getTextIndex(_m_currentLevelRef));
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(TransKeyConst.building_upgradeSuccessTitle_name, TextTranslate.instance.getLanguage(_m_buildingRef.name)));
            ALUGUICommon.setLabelTxt(wnd.txtUpgradeDesc, TextTranslate.instance.getLanguage(_m_buildingRef.level_up_desc));
            _m_level?.setValue(_m_currentLevel - 1, _m_currentLevel);
            _m_employeeLimit?.setValue(_getEmployeeLimit(_m_currentLevel - 1), _getEmployeeLimit(_m_currentLevel));
            _m_employeeEarningsRate?.setValue(100 + _m_lastLevelRef.earning_rate / 100f, 100 + _m_currentLevelRef.earning_rate / 100f);
            BusinessBuildingDevelopRefObj lastDevelopRef = _getNewestDevelopRef(_m_lastLevelRef);
            BusinessBuildingDevelopRefObj currentDevelopRef = _getNewestDevelopRef(_m_currentLevelRef);
            if (lastDevelopRef != currentDevelopRef)
            {
                ALUGUICommon.setGameObjEnable(wnd.listVideoUnlockShow, true);
                ALUGUICommon.setLabelTxt(wnd.txtVideoDesc, TextTranslate.instance.getLanguage(currentDevelopRef?.desc, currentDevelopRef?.desc_args));
                _m_videoPreview?.setTexture(currentDevelopRef?.icon);
            }
            else
                ALUGUICommon.setGameObjEnable(wnd.listVideoUnlockShow, false);
        }


        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BUSINESS_BUILDING_UPGRADE_SUC);
            
        }
        private NPGTextureIndex _getTextIndex(BusinessBuildingLevelRefObj _levelRef)
        {
            NPGTextureIndex texIndex = _levelRef?.preview_tex_index;
            if (texIndex == null || !texIndex.isValid())
                return _m_buildingRef?.preview_tex_index;
            
            return texIndex;
        }
        private BusinessBuildingDevelopRefObj _getNewestDevelopRef(BusinessBuildingLevelRefObj _levelRef)
        {
            BusinessBuildingDevelopRefObj newestDevelopRef = null;
            if (_levelRef != null)
                newestDevelopRef = GRefdataCoreMgr.instance.getNewestBusinessBuildingDevelopRef(_levelRef.building_id, _levelRef.level);
            
            return newestDevelopRef;
        }
        private long _getEmployeeLimit(int _level)
        {
            return _m_buildingRef.employee_base_max_count + _m_buildingRef.addition_employee_count_per_level * (_level - 1);
        }
    }
}