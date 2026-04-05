using ALPackage;

namespace GOE
{
    public class GGUISubWndInnLevelIconContainerItem : _ATALBasicUISubWnd<GGUIMonoInnLevelIconContainerItem>
    {
        private InnLevelRefObj _m_levelRef;
        private int _m_starIndex;
        private NPGGuiWndTexture _m_iconWnd;


        public GGUISubWndInnLevelIconContainerItem(GGUIMonoInnLevelIconContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
        }


        public void refreshWnd(InnLevelRefObj _levelRef)
        {
            _m_levelRef = _levelRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_levelRef == null)
                return;

            _m_iconWnd?.setTexture(_m_levelRef.star_icon);
        }
    }
}