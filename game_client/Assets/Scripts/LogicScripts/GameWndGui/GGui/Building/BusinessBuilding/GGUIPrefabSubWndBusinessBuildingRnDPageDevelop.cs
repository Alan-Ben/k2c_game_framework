
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndBusinessBuildingRnDPageDevelop : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoBusinessBuildingRnDPageDevelop>
    {
        private BusinessBuildingInfo _m_buildingInfo;
        private GGUISubWndCommonPropertyItem _m_totalBonus;
        private GGUISubWndBusinessBuildingRnDPageDevelopContainer _m_developContainer;
        
        
        public GGUIPrefabSubWndBusinessBuildingRnDPageDevelop(Transform _parent) 
            : base(_parent)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingRnDPageDevelop.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingRnDPageDevelop.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_developContainer?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_developContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_developContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_developContainer?.discard();
            _m_developContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoDevelopContainer != null)
                _m_developContainer = new GGUISubWndBusinessBuildingRnDPageDevelopContainer(wnd.monoDevelopContainer);
            if (wnd.monoDevelopBonus != null)
                _m_totalBonus = new GGUISubWndCommonPropertyItem(wnd.monoDevelopBonus);
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

            ALUGUICommon.setLabelTxt(wnd.txtBuildingLevel, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingPageLevel_level, _m_buildingInfo.level));
            _m_totalBonus?.refreshWnd(_m_buildingInfo.bonusJudgeParts);
            List<BusinessBuildingDevelopRefObj> developList = GRefdataCoreMgr.instance.getBusinessBuildingDevelopRefList(_m_buildingInfo.id);
            _m_developContainer?.refreshWnd(developList, _m_buildingInfo);
        }
    }
}