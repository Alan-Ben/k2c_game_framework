using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndStageGoalBtn : _ATALBasicUISubWnd<GGUIMonoStageGoalBtn>
    {
        private NPGGuiWndTexture _m_stepIcon;
        
        
        public GGUISubWndStageGoalBtn(GGUIMonoStageGoalBtn _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_stepIcon?.showWnd();

            refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_STAGE_GOAL_CHG, refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.ON_STAGE_GOAL_TASK_CHG, refreshWnd);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_STAGE_GOAL_CHG, refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_STAGE_GOAL_TASK_CHG, refreshWnd);
            
            _m_stepIcon?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_stepIcon?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_stepIcon?.discard();
            _m_stepIcon = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnEnter, _onBtnEnterClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgStepIcon != null)
                _m_stepIcon = new NPGGuiWndTexture(wnd.imgStepIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnEnter, _onBtnEnterClicked);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            StageGoalBigStepRefObj refObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            if (refObj == null)
                return;
            
            _m_stepIcon?.setTexture(refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtStepName, refObj.getTitle);
            
            float progress = NPPlayer.instance.stageGoalComp.getStageGoalBigProgress();
            if (wnd.sldProgress != null)
            {
                wnd.sldProgress.minValue = 0;
                wnd.sldProgress.maxValue = 1;
                wnd.sldProgress.value = progress;
            }

            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, progress * 100f));
        }


        private void _onBtnEnterClicked(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndStageGoal.instance, UINodeTagConst.C_STAGE_GOAL, null, GGUIWndStageGoal.instance.setDefaultTab, 0);
        }
    }
}