using ALPackage;
using GC2GS.p033_SimpleActivityOp;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsDayContentTaskContainerItem : _ATALBasicUISubWnd<GGUIMonoSevenDayGoalsDayContentTaskContainerItem>
    {
        private NPGGUIWndProgress _m_progress;
        private GGUIWndCommonRewardContainer _m_rewardContainer;
        
        private SevenDayGoalsTaskRewardRefObj _m_refObj;
        
        
        public GGUISubWndSevenDayGoalsDayContentTaskContainerItem(GGUIMonoSevenDayGoalsDayContentTaskContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        
        protected override void _onShowWnd()
        {
            _m_progress?.showWnd();
            _m_rewardContainer?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_progress?.hideWnd();
            _m_rewardContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_progress?.resetWnd();
            _m_rewardContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_progress?.discard();
            _m_rewardContainer?.discard();
            
            _m_progress = null;
            _m_rewardContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onGotoBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onGetRewardBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoGoalProgress != null)
                _m_progress = new NPGGUIWndProgress(wnd.monoGoalProgress);
            if (wnd.monoRewardContainer != null)
                _m_rewardContainer = new GGUIWndCommonRewardContainer(wnd.monoRewardContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onGotoBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onGetRewardBtnClick);
        }


        public void refreshWnd(SevenDayGoalsTaskRewardRefObj _refObj)
        {
            _m_refObj = _refObj;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            SevenDayGoalsTaskRefObj taskRef = _m_refObj?.task_ref;
            if (taskRef == null)
                return;

            long curProgress = NPPlayer.instance.sevenDayGoalsComp.data.getTaskCount(_m_refObj.id);
            long targetProgress = _m_refObj.goal_count;
            bool hadGetReward = NPPlayer.instance.sevenDayGoalsComp.data.isTaskHadDrawReward(_m_refObj.id);
            bool isCompleted = curProgress >= targetProgress;
            ECommonRewardType rewardType = ECommonRewardType.NONE;
            if (hadGetReward)
                rewardType = ECommonRewardType.HAS_GET_REWARD;
            else if (isCompleted)
                rewardType = ECommonRewardType.CAN_GET_REWARD;
            else
                rewardType = ECommonRewardType.NOT_GET_REWARD;

            ALUGUICommon.setLabelTxt(wnd.txtTile, _m_refObj.getDescTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtScore, _m_refObj.gain_score);
            if (_m_progress != null)
            {
                _m_progress.setProgress((float)curProgress / targetProgress);
                string curCountStr = GCommon.getValueFormatStr(taskRef.process_num_format, curProgress);
                string allCountStr = GCommon.getValueFormatStr(taskRef.process_num_format, targetProgress);
                _m_progress.setProgressTxt(TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curCountStr, allCountStr), isCompleted);
            }
            
            _m_rewardContainer?.setRewardList(_m_refObj.reward_item_list, rewardType);
            wnd.setTaskState(rewardType);
        }
        
        
        private void _onGotoBtnClick(GameObject _)
        {
            _m_refObj?.task_ref?.go_to?.dealEffect();
        }
        private void _onGetRewardBtnClick(GameObject _)
        {
            if (_m_refObj == null)
                return;
            
            NPGSClientListener.sendRequestByLog(new GC2GS_033_005_ReqSevenDayGoalsDrawReward(_m_refObj.id), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    // todo: special effect
                }));
        }
    }
}