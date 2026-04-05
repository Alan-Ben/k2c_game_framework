using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingEnergyYieldDetail : _ATALBasicUIWnd<GGUIMonoMarsBuildingEnergyYieldDetail>
    {
        [NotNull] public static GGUIWndMarsBuildingEnergyYieldDetail instance { get { return _g_instance ??= new GGUIWndMarsBuildingEnergyYieldDetail(); } }
        private static GGUIWndMarsBuildingEnergyYieldDetail _g_instance;

        
        private GGUISubWndMarsEnergyYieldDetailGrid _m_yieldDetailGrid;


        public GGUIWndMarsBuildingEnergyYieldDetail() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingEnergyYieldDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingEnergyYieldDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_yieldDetailGrid?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_yieldDetailGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_yieldDetailGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_yieldDetailGrid?.discard();
            _m_yieldDetailGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGrid != null)
                _m_yieldDetailGrid = new GGUISubWndMarsEnergyYieldDetailGrid(wnd.monoGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
        }


        public void refreshWnd() 
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_yieldDetailGrid?.refreshWnd();
        }
        

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_ENERGY_YIELD_DETAIL);
        }
    }
}