using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamMiniInfo : _ATALBasicUISubWnd<GGUIMonoMarsExploreTeamMiniInfo>
    {
        private GGUISubWndMarsExploreTeamMiniInfoContainer _m_subWndTeamContainer;
        private bool _m_isExpanded;


        public GGUISubWndMarsExploreTeamMiniInfo(GGUIMonoMarsExploreTeamMiniInfo _wnd)
            : base(_wnd)
        {
            _m_isExpanded = true;
            
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_subWndTeamContainer?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_subWndTeamContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_subWndTeamContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnShow, _onClickShow);
            ALUGUICommon.uncombineBtnClick(wnd.btnHide, _onClickHide);

            _m_subWndTeamContainer?.discard();
            _m_subWndTeamContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnShow, _onClickShow);
            ALUGUICommon.combineBtnClick(wnd.btnHide, _onClickHide);

            if (wnd.monoTeamContainer != null)
                _m_subWndTeamContainer = new GGUISubWndMarsExploreTeamMiniInfoContainer(wnd.monoTeamContainer);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_subWndTeamContainer?.refreshWnd();
            wnd.wndAnimation.Sample(_m_isExpanded ? wnd.showAnimName : wnd.hideAnimName, 1);
        }


        private void _onClickShow(GameObject _go)
        {
            _m_isExpanded = true;
            if (wnd == null)
                return;
            
            wnd.wndAnimation.ForcePlay(wnd.showAnimName);
        }
        private void _onClickHide(GameObject _go)
        {
            _m_isExpanded = false;
            if (wnd == null)
                return;
            
            wnd.wndAnimation.ForcePlay(wnd.hideAnimName);
        }
    }
}
