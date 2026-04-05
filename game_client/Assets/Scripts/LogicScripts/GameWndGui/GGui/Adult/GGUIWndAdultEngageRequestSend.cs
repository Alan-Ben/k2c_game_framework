using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAdultEngageRequestSend : _ATALBasicUIWnd<GGUIMonoAdultEngageRequestSend>
    {
        [NotNull] public static GGUIWndAdultEngageRequestSend instance { get { return _g_instance ??= new GGUIWndAdultEngageRequestSend(); } }
        private static GGUIWndAdultEngageRequestSend _g_instance;
        
        
        private GGUISubWndAdultEngageRequestSendPageTabList _m_pageTabList;
        private AdultInfo _m_myAdultInfo;
        
        
        public GGUIWndAdultEngageRequestSend() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAdultEngageRequestSend.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultEngageRequestSend.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_pageTabList?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_pageTabList?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_pageTabList?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_pageTabList?.discard();
            _m_pageTabList = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTabList != null)
                _m_pageTabList = new GGUISubWndAdultEngageRequestSendPageTabList(wnd.monoTabList);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }


        public void refreshWnd(AdultInfo _adultInfo)
        {
            _m_myAdultInfo = _adultInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            _m_pageTabList?.refreshWnd(_m_myAdultInfo);
        }


        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADULT_ENGAGE_REQUEST_SEND);
        }
    }
}