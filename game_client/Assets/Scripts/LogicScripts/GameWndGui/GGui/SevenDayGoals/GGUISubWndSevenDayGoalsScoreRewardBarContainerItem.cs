using ALPackage;
using GC2GS.p033_SimpleActivityOp;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsScoreRewardBarContainerItem : _ATALBasicUISubWnd<GGUIMonoSevenDayGoalsScoreRewardBarContainerItem>
    {
        private NPGGUIWndCommonItem _m_characterRewardItem;
        private NPGGUIWndCommonItem _m_otherRewardItem;

        private SevenDayGoalsStepRewardRefObj _m_rewardRef;
        
        
        public GGUISubWndSevenDayGoalsScoreRewardBarContainerItem(GGUIMonoSevenDayGoalsScoreRewardBarContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        
        protected override void _onShowWnd()
        {
            _m_characterRewardItem?.showWnd();
            _m_otherRewardItem?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_characterRewardItem?.hideWnd();
            _m_otherRewardItem?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_characterRewardItem?.resetWnd();
            _m_otherRewardItem?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_characterRewardItem?.discard();
            _m_otherRewardItem?.discard();
            
            _m_characterRewardItem = null;
            _m_otherRewardItem = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnGainReward, _onBtnGainRewardClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCharacterRewardItem != null)
                _m_characterRewardItem = new NPGGUIWndCommonItem(wnd.monoCharacterRewardItem);
            if (wnd.monoOtherRewardItem != null)
                _m_otherRewardItem = new NPGGUIWndCommonItem(wnd.monoOtherRewardItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnGainReward, _onBtnGainRewardClick);
        }
        

        public void refreshWnd(SevenDayGoalsStepRewardRefObj _rewardRef)
        {
            _m_rewardRef = _rewardRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            NPCommonCostItem rewardItem = _m_rewardRef?.gain_item_list.GetFirst();
            long needScore = _m_rewardRef?.need_score ?? 0;
            long curScore = NPPlayer.instance.sevenDayGoalsComp.data.score;
            ENPItemType itemType = rewardItem?.getItemType() ?? ENPItemType.HERO;
            bool hadGetReward = NPPlayer.instance.sevenDayGoalsComp.data.isStepRewardHadDraw(_m_rewardRef?.id ?? 0);
            
            _m_characterRewardItem?.setItem(rewardItem);
            _m_otherRewardItem?.setItem(rewardItem);
            ALUGUICommon.setLabelTxt(wnd.txtScoreRequire, needScore);
            wnd.setItemType(itemType);
            wnd.setTaskState(hadGetReward, curScore >= needScore);
        }
        

        private void _onBtnGainRewardClick(GameObject _)
        {
            if (_m_rewardRef == null)
                return;
            
            NPGSClientListener.sendRequestByLog(new GC2GS_033_006_ReqSevenDayGoalsDrawStepReward(_m_rewardRef.id), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    // todo: special effect
                }));
        }
    }
}