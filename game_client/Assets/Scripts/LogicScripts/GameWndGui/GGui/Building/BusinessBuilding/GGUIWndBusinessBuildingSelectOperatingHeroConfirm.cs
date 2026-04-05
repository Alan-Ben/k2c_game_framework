using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingSelectOperatingHeroConfirm : _ANPGGUIBasicWnd<GGUIMonoBusinessBuildingSelectOperatingHeroConfirm>
    {
        [NotNull] public static GGUIWndBusinessBuildingSelectOperatingHeroConfirm instance { get { return _g_instance ??= new GGUIWndBusinessBuildingSelectOperatingHeroConfirm(); } }
        private static GGUIWndBusinessBuildingSelectOperatingHeroConfirm _g_instance;


        private GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainer _m_heroContainer;
        
        private List<HeroInfo> _m_heroList;
        private BusinessBuildingRefObj _m_newBuildingRef;
        private Action _m_doConfirm;
        

        public GGUIWndBusinessBuildingSelectOperatingHeroConfirm() 
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoBusinessBuildingSelectOperatingHeroConfirm.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBusinessBuildingSelectOperatingHeroConfirm.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_heroContainer?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_heroContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_heroContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_heroContainer?.discard();
            _m_heroContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroContainer != null)
                _m_heroContainer = new GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainer(wnd.monoHeroContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClicked);
        }
        
        
        public void refreshWnd(List<HeroInfo> _heroList, BusinessBuildingRefObj _newBuildingRef)
        {
            _m_heroList = _heroList;
            _m_newBuildingRef = _newBuildingRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            _m_heroContainer?.refreshWnd(_m_heroList, _m_newBuildingRef);
        }
        public void setConfirmFunc(Action _doConfirm)
        {
            _m_doConfirm = _doConfirm;
        }


        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onBtnConfirmClicked(GameObject _)
        {
            _onBtnCloseClicked(null);
            _m_doConfirm?.Invoke();
        }
    }
}