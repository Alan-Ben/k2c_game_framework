using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndBusinessBuildingRnDPageProduct : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoBusinessBuildingRnDPageProduct>
    {
        private BusinessBuildingInfo _m_buildingInfo;
        
        private GGUISubWndCommonPropertyItem _m_productBonus;
        private GGUISubWndBusinessBuildingRnDPageProductGrid _m_productGrid;
        
        
        public GGUIPrefabSubWndBusinessBuildingRnDPageProduct(Transform _parent) 
            : base(_parent)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingRnDPageProduct.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingRnDPageProduct.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_productBonus?.showWnd();
            _m_productGrid?.showWnd();
            
            refreshWnd();

            NPPlayer.instance.buildingComp.onBusinessBuildingChg += _onBusinessBuildingChg;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.buildingComp.onBusinessBuildingChg -= _onBusinessBuildingChg;
            
            _m_productBonus?.hideWnd();
            _m_productGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_productBonus?.resetWnd();
            _m_productGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_productBonus?.discard();
            _m_productGrid?.discard();
            _m_productBonus = null;
            _m_productGrid = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoProductBonus != null)
                _m_productBonus = new GGUISubWndCommonPropertyItem(wnd.monoProductBonus);
            if (wnd.monoProductGrid != null)
                _m_productGrid = new GGUISubWndBusinessBuildingRnDPageProductGrid(wnd.monoProductGrid);
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

            ALUGUICommon.setLabelTxt(wnd.txtCurEmployeeCount, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingPageEmployee_num, _m_buildingInfo.employeeNum));
            _m_productBonus?.refreshWnd(_m_buildingInfo.bonusJudgeParts);
            List<BusinessBuildingProductRefObj> productList = GRefdataCoreMgr.instance.getBusinessBuildingProductRefList(_m_buildingInfo.id);
            _m_productGrid?.refreshWnd(productList, _m_buildingInfo);
        }


        private void _onBusinessBuildingChg(BusinessBuildingInfo _buildingInfo)
        {
            if (_buildingInfo == _m_buildingInfo)
                refreshWnd();
        }
    }
}