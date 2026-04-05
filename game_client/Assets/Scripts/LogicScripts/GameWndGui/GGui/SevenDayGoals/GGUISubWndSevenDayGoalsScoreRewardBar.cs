using ALPackage;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsScoreRewardBar : _ATALBasicUISubWnd<GGUIMonoSevenDayGoalsScoreRewardBar>
    {
        private GGUISubWndSevenDayGoalsScoreRewardBarContainer _m_rewardItemContainer;
        private GGUISubWndSevenDayGoalsScoreRewardBarContainerItem _m_lastSpecItem;
        
        
        public GGUISubWndSevenDayGoalsScoreRewardBar(GGUIMonoSevenDayGoalsScoreRewardBar _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        
        protected override void _onShowWnd()
        {
            _m_rewardItemContainer?.showWnd();
            _m_lastSpecItem?.showWnd();

            refreshWnd();

            NPPlayer.instance.sevenDayGoalsComp.data.onScoreChg += refreshWnd;
            NPPlayer.instance.sevenDayGoalsComp.data.onStepRewardChg += refreshWnd;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.sevenDayGoalsComp.data.onScoreChg -= refreshWnd;
            NPPlayer.instance.sevenDayGoalsComp.data.onStepRewardChg -= refreshWnd;
            
            _m_rewardItemContainer?.hideWnd();
            _m_lastSpecItem?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_rewardItemContainer?.resetWnd();
            _m_lastSpecItem?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_rewardItemContainer?.discard();
            _m_lastSpecItem?.discard();
            
            _m_rewardItemContainer = null;
            _m_lastSpecItem = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRewardContainer != null)
                _m_rewardItemContainer = new GGUISubWndSevenDayGoalsScoreRewardBarContainer(wnd.monoRewardContainer);
            if (wnd.monoLastSpecItem != null)
                _m_lastSpecItem = new GGUISubWndSevenDayGoalsScoreRewardBarContainerItem(wnd.monoLastSpecItem);
        }
        

        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            SevenDayGoalsStepRewardRefObj lastRewardRef = GRefdataCoreMgr.instance.sevenDayGoalsStepRewardRefCore.refList.GetLast();
            long score = NPPlayer.instance.sevenDayGoalsComp.data.score;

            ALUGUICommon.setLabelTxt(wnd.txtCurProcess, score);
            if (wnd.sldProcess != null)
            {
                wnd.sldProcess.minValue = 0;
                wnd.sldProcess.maxValue = lastRewardRef?.need_score ?? 0;
                wnd.sldProcess.value = score;
            }
            
            _m_rewardItemContainer?.refreshWnd();
            _m_lastSpecItem?.refreshWnd(lastRewardRef);
        }
        public void refreshWnd(long _stepId)
        {
            refreshWnd();
        }
    }
}