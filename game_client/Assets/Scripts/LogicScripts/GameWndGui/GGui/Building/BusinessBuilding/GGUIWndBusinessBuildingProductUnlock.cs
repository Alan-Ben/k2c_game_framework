using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingProductUnlock : _ANPGGUIBasicWnd<GGUIMonoBusinessBuildingProductUnlock>
    {
        public static GGUIWndBusinessBuildingProductUnlock instance { get { return _g_instance ??= new GGUIWndBusinessBuildingProductUnlock(); } }
        private static GGUIWndBusinessBuildingProductUnlock _g_instance;

        
        private BusinessBuildingProductRefObj _m_productRef;
        private BusinessBuildingInfo _m_buildingInfo;
        
        private NPGGuiWndTexture _m_iconWnd;
        private GGUISubWndCommonBonusShower _m_productBonus;


        private GGUIWndBusinessBuildingProductUnlock() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingProductUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingProductUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            _m_productBonus?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
            _m_productBonus?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
            _m_productBonus?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_productBonus?.discard();
            
            _m_iconWnd = null;
            _m_productBonus = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgProductIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgProductIcon);
            if (wnd.monoProductBonus != null)
                _m_productBonus = new GGUISubWndCommonBonusShower(wnd.monoProductBonus);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        

        public void refreshWnd(BusinessBuildingProductRefObj _productRef, BusinessBuildingInfo _buildingInfo)
        {
            _m_productRef = _productRef;
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_productRef == null || _m_buildingInfo == null)
                return;
            
            _m_iconWnd?.setTexture(_m_productRef.icon);
            ALUGUICommon.setLabelTxt(wnd.txtProductName, TextTranslate.instance.getLanguage(_m_productRef.name));
            _m_productBonus?.refreshWnd(_m_productRef.add_bonus.unionBonus, _m_buildingInfo.bonusJudgeParts);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BUSINESS_BUILDING_PRODUCT_UNLOCK);
        }
    }
}