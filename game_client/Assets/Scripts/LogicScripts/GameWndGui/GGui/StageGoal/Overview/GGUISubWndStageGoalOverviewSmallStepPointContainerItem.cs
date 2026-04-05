using ALPackage;

namespace GOE
{
    public class GGUISubWndStageGoalOverviewSmallStepPointContainerItem : _ATALBasicUISubWnd<GGUIMonoStageGoalOverviewSmallStepPointContainerItem>
    {
        private bool _m_isComplete;
    
    
        public GGUISubWndStageGoalOverviewSmallStepPointContainerItem(GGUIMonoStageGoalOverviewSmallStepPointContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd(bool _isComplete)
        {
            _m_isComplete = _isComplete;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            wnd.setComplete(_m_isComplete);
        }
    }
}