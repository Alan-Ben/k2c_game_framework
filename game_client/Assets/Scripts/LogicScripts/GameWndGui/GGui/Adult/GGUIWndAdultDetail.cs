using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAdultDetail : _ATALBasicUIWnd<GGUIMonoAdultDetail>
    {
        [NotNull] public static GGUIWndAdultDetail instance { get { return _g_instance ??= new GGUIWndAdultDetail(); } }
        private static GGUIWndAdultDetail _g_instance;
        
        
        private GGUISubWndChildInfo _m_adultInfoWnd;
        
        
        public GGUIWndAdultDetail() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAdultDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_adultInfoWnd?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_adultInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_adultInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_adultInfoWnd?.discard();
            _m_adultInfoWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        

        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}