
using System.Collections.Generic;
using ALPackage;
using GC2GS.p017_ActivityOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基金战令通行证Grid Item
    /// </summary>
    public class GGUIWndFundBattlePassGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoFundBattlePassGridItem>
    {
        //阶段配置
        private ActivityFundStepRefObj _m_stepRef;
        //基金信息快照
        private FundInfoSnapshot _m_fundSnapshot;
        //免费奖励容器
        private GGUIWndCommonRewardContainer _m_wFreeRewardContainer;
        //付费奖励容器
        private GGUIWndCommonLockRewardContainer _m_wPaidRewardContainer;
        

        public GGUIWndFundBattlePassGridItem(GGUIMonoFundBattlePassGridItem _mono) : base(_mono)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_wFreeRewardContainer?.showWnd();
            _m_wPaidRewardContainer?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wFreeRewardContainer?.hideWnd();
            _m_wPaidRewardContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wFreeRewardContainer?.resetWnd();
            _m_wPaidRewardContainer?.resetWnd();
        }
        protected override void _resetGridItem()
        {
        }
        protected override void _onDiscard()
        {
            _m_wFreeRewardContainer?.discard();
            _m_wFreeRewardContainer = null;
            _m_wPaidRewardContainer?.discard();
            _m_wPaidRewardContainer = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClaimAll, _onClickClaimAll);
            ALUGUICommon.uncombineBtnClick(wnd.btnClaimAll2, _onClickClaimAll);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoFreeRewardContainer != null)
                _m_wFreeRewardContainer = new GGUIWndCommonRewardContainer(wnd.monoFreeRewardContainer);
            if (wnd.monoPaidRewardContainer != null)
                _m_wPaidRewardContainer = new GGUIWndCommonLockRewardContainer(wnd.monoPaidRewardContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClaimAll, _onClickClaimAll);
            ALUGUICommon.combineBtnClick(wnd.btnClaimAll2, _onClickClaimAll);
        }
        

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(ActivityFundStepRefObj _stepRef, FundInfoSnapshot _snapshot)
        {
            if (wnd == null || _stepRef == null || _snapshot == null)
                return;

            _m_stepRef = _stepRef;
            _m_fundSnapshot = _snapshot;

            //设置阶段文本
            ALUGUICommon.setLabelTxt(wnd.txtStep, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_stepRef.step));

            //设置目标值
            ALUGUICommon.setLabelTxt(wnd.txtGoalValue, TextTranslate.instance.getLanguage(TransKeyConst.fund_goal_value, _stepRef.need_count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtGoalValueBrief, TextTranslate.instance.getLanguage(TransKeyConst.fund_briefGoal_value, _stepRef.need_count.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            
            //设置进度条（相对于上一个 step 的进度）
            long totalScore = _m_fundSnapshot.totalScore;
            long prevNeedCount = _stepRef.prev_step_need_count;
            long currentNeedCount = _stepRef.need_count;
            if (wnd.sldProgress != null)
            {
                wnd.sldProgress.minValue = prevNeedCount;
                wnd.sldProgress.maxValue = currentNeedCount == prevNeedCount ? prevNeedCount + 1 : currentNeedCount;
                if (prevNeedCount == currentNeedCount)
                    wnd.sldProgress.normalizedValue = 1;
                else
                    wnd.sldProgress.value = totalScore;
            }

            //刷新免费档奖励
            _refreshFreeReward();
            //刷新付费档奖励
            _refreshPaidReward();

            //设置领取按钮状态
            wnd.setCanDrawState(totalScore >= _stepRef.need_count);
            wnd.setCanDrawPaidState(totalScore >= _stepRef.need_count && _m_fundSnapshot.checkHasBuyFund());
            
            // wnd.setSpecialStepState(_stepRef.is_special_step);
            // 以前是根据配置表的 is_special_step 字段来设置是否特殊阶段，现在改为根据是否包含特殊奖励来设置
            wnd.setSpecialStepState(_checkStepContainsSpecialReward());
        }
        

        //刷新免费档奖励
        private void _refreshFreeReward()
        {
            if (_m_wFreeRewardContainer == null || _m_stepRef == null || _m_fundSnapshot == null)
                return;

            bool canDraw = _m_fundSnapshot.totalScore >= _m_stepRef.need_count && _m_fundSnapshot.hadDrawFreeStep < _m_stepRef.step;
            bool hasDrawn = _m_fundSnapshot.hadDrawFreeStep >= _m_stepRef.step;
            
            ECommonRewardType rewardType = hasDrawn ? ECommonRewardType.HAS_GET_REWARD :
                (canDraw ? ECommonRewardType.CAN_GET_REWARD : ECommonRewardType.NOT_GET_REWARD);
            _m_wFreeRewardContainer.setRewardList(_m_stepRef.free_reward_item_list, rewardType);
        }
        //刷新付费档奖励
        private void _refreshPaidReward()
        {
            if (_m_wPaidRewardContainer == null || _m_stepRef == null || _m_fundSnapshot == null)
                return;

            bool hasBuy = _m_fundSnapshot.checkHasBuyFund();
            bool canDraw = hasBuy && _m_fundSnapshot.totalScore >= _m_stepRef.need_count && _m_fundSnapshot.hadDrawPayStep < _m_stepRef.step;
            bool hasDrawn = _m_fundSnapshot.hadDrawPayStep >= _m_stepRef.step;

            ECommonLockRewardType rewardType = hasDrawn ? ECommonLockRewardType.HAS_GET_REWARD :
                (hasBuy ? (canDraw ? ECommonLockRewardType.CAN_GET_REWARD : ECommonLockRewardType.NOT_GET_REWARD) : 
                    ECommonLockRewardType.LOCK);
            _m_wPaidRewardContainer.setRewardList(_m_stepRef.pay_reward_item_list, rewardType);
        }
        private void _onClickClaimAll(GameObject _)
        {
            if (_m_fundSnapshot == null)
                return;

            if (!_m_fundSnapshot.checkHasAnyRewardCanDraw())
                return;

            NPGSClientListener.sendMsgByLog(new GC2GS_017_018_ReqActivityFundDrawStepReward(_m_fundSnapshot.fundId));
        }
        private bool _checkStepContainsSpecialReward()
        {
            if (_m_stepRef == null || _m_fundSnapshot?.levelRef == null)
                return false;

            NPCommonCostItem specialReward = _m_fundSnapshot.levelRef.special_item;
            if (specialReward == null)
                return false;

            foreach (NPCommonCostItem item in _m_stepRef.free_reward_item_list)
            {
                if (item.getItemType() == specialReward.getItemType() && item.subId == specialReward.subId)
                    return true;
            }

            foreach (NPCommonCostItem item in _m_stepRef.pay_reward_item_list)
            {
                if (item.getItemType() == specialReward.getItemType() && item.subId == specialReward.subId)
                    return true;
            }

            return false;
        }
    }
}
