using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingRnD : _ANPGGUIBasicWnd<GGUIMonoBusinessBuildingRnD>
    {
        public static GGUIWndBusinessBuildingRnD instance { get { return _g_instance ??= new GGUIWndBusinessBuildingRnD(); } }
        private static GGUIWndBusinessBuildingRnD _g_instance;


        private BusinessBuildingInfo _m_buildingInfo;
        private GGUISubWndBusinessBuildingRnDPageTabList _m_tabList;
        

        public GGUIWndBusinessBuildingRnD() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingRnD.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingRnD.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_tabList?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_tabList?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_tabList?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_tabList?.discard();
            _m_tabList = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTabList != null)
                _m_tabList = new GGUISubWndBusinessBuildingRnDPageTabList(wnd.monoTabList);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
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

            ALUGUICommon.setLabelTxt(wnd.txtBuildingName, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.name));
            _m_tabList?.refreshWnd(_m_buildingInfo);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_BUSINESS_BUILDING_RND);
        }
    }
}