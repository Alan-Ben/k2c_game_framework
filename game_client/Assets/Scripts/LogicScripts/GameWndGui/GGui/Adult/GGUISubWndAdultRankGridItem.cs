
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultRankGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoAdultRankGridItem>
    {
        private GGUISubWndChildInfo _m_adultInfoWnd;
        
        
        public GGUISubWndAdultRankGridItem(GGUIMonoAdultRankGridItem _wnd) 
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
            
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onDetailBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onDetailBtnClick);
        }
        protected override void _resetGridItem()
        {
        }
        
        
        private void _onDetailBtnClick(GameObject _obj)
        {
        }
    }
}