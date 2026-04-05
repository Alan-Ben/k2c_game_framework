using ALPackage;

namespace GOE
{
    public abstract class _AAnecdoteEventFollower<Mono> : _ATALGGUIWndCommonFollowItem<Mono> 
        where Mono : GGUIMonoAnecdoteFollower
    {
        private _AAnecdoteEventInfo _m_eventInfo;
        
        private NPGGuiWndTexture _m_eventIconWnd;
        private GGuiWndSprite _m_eventBgWnd;
        
        public _AAnecdoteEventFollower(Mono _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.imgEventIcon != null)
                _m_eventIconWnd = new NPGGuiWndTexture(wnd.imgEventIcon);

            if (wnd.imgEventBg != null)
                _m_eventBgWnd = new GGuiWndSprite(wnd.imgEventBg);

            _onWndInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            _m_eventIconWnd?.discard();
            _m_eventIconWnd = null;
            
            _m_eventBgWnd?.discard();
            _m_eventBgWnd = null;
            
            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            refreshWnd();
            
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _m_eventIconWnd?.hideWnd();
            _m_eventBgWnd?.hideWnd();
            
            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _m_eventIconWnd?.discardTexture();
            _m_eventBgWnd?.discardTexture();
            
            _onResetSub();
        }

        public void refreshWnd(_AAnecdoteEventInfo _eventInfo)
        {
            _m_eventInfo = _eventInfo;
            refreshWnd();
        }

        public void refreshWnd()
        {
            if(wnd == null || !_m_bIsShow || _m_eventInfo == null)
                return;

            if (_m_eventIconWnd != null)
            {
                if(_m_eventInfo.eventRef.event_entrance_view_icon == null || !_m_eventInfo.eventRef.event_entrance_view_icon.isValid())
                    _m_eventIconWnd.hideWnd();
                else
                {
                    _m_eventIconWnd.showWnd();
                    _m_eventIconWnd.setTexture(_m_eventInfo.eventRef.event_entrance_view_icon);
                }
            }

            if (_m_eventBgWnd != null)
            {
                if(_m_eventInfo.eventRef.event_entrance_view_icon_bg == null || !_m_eventInfo.eventRef.event_entrance_view_icon_bg.isValid())
                    _m_eventBgWnd.hideWnd();
                else
                {
                    _m_eventBgWnd.showWnd();
                    _m_eventBgWnd.setTexture(_m_eventInfo.eventRef.event_entrance_view_icon_bg);
                }
            }
            
            _onRefreshWnd();
        }

        protected abstract void _onWndInitDoneSub();
        protected abstract void _onDiscardSub();
        protected abstract void _onShowWndSub();
        protected abstract void _onHideWndSub();
        protected abstract void _onResetSub();
        
        protected abstract void _onRefreshWnd();
    }
}