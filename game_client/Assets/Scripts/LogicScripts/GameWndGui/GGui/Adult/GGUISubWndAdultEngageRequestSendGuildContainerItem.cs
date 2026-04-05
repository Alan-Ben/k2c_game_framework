using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultEngageRequestSendGuildContainerItem : _ANPGGUIBasicSubWnd<GGUIMonoAdultEngageRequestSendGuildContainerItem>
    {
        private GGUISubWndChildInfo _m_adultInfoWnd;
        
        
        public GGUISubWndAdultEngageRequestSendGuildContainerItem(GGUIMonoAdultEngageRequestSendGuildContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

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
            
            ALUGUICommon.uncombineBtnClick(wnd.btnEngage, _onEngageBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnEngage, _onEngageBtnClick);
        }
        
        
        private void _onEngageBtnClick(GameObject _obj)
        {
        }
    }
}