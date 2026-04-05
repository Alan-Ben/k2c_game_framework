using ALPackage;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsDayContent : _ATALBasicUISubWnd<GGUIMonoSevenDayGoalsDayContent>
    {
        private GGUISubWndSevenDayGoalsDayContentPageTabList _m_tabList;

        private int _m_selectedDay;
        
        
        public GGUISubWndSevenDayGoalsDayContent(GGUIMonoSevenDayGoalsDayContent _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        
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
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.tabList != null)
                _m_tabList = new GGUISubWndSevenDayGoalsDayContentPageTabList(wnd.tabList);
        }
        

        public void refreshWnd(int _selectedDay, bool _reset)
        {
            _m_selectedDay = _selectedDay;
            refreshWnd(_reset);
        }
        public void refreshWnd(bool _reset = false)
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            _m_tabList?.refreshWnd(_m_selectedDay, _reset);
        }
        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRedTip()
        {
            _m_tabList?.refreshRedTip();
        }
    }
}