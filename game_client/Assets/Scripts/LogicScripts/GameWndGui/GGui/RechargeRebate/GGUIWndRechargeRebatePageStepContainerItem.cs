using ALPackage;
using Common.RechargeRebateEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 充值返利组步骤列表item
    /// </summary>
    public class GGUIWndRechargeRebatePageStepContainerItem : _ATALBasicUISubWnd<GGUIMonoRechargeRebatePageStepContainerItem>
    {
        //组信息
        private RechargeRebateInfo _m_rechargeRebateInfo;
        //步骤信息
        private RechargeRebateStepRefObj _m_stepRef;
        //奖励道具列表
        private GGUIWndCommonRewardContainer _m_wItemContainer;
        //特殊道具
        private NPGGUIWndCommonItem _m_wSpecialItem;

        public GGUIWndRechargeRebatePageStepContainerItem(GGUIMonoRechargeRebatePageStepContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wItemContainer?.hideWnd();
            _m_wSpecialItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
            _m_wSpecialItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            _m_wSpecialItem?.discard();
            _m_wSpecialItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnRecharge, _onClickRecharge);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new GGUIWndCommonRewardContainer(wnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnRecharge, _onClickRecharge);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(RechargeRebateStepRefObj _stepRef, RechargeRebateInfo _info)
        {
            _m_rechargeRebateInfo = _info;
            _m_stepRef = _stepRef;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_stepRef == null || _m_rechargeRebateInfo == null || _m_rechargeRebateInfo.groupRefObj == null)
                return;

            //领奖状态
            ECommonRewardType rewardType = _m_rechargeRebateInfo.getStepRewardType(_m_stepRef);
            //礼包名称
            ALUGUICommon.setLabelTxt(wnd.txtGiftPackName, TextTranslate.instance.getLanguage(_m_stepRef.name, _m_stepRef.name_args));
            //奖励列表
            _m_wItemContainer?.showWnd();
            _m_wItemContainer?.setRewardList(_m_stepRef.reward_list, rewardType);
            //进度
            long curCount = _m_rechargeRebateInfo.curCount;
            ALUGUICommon.setSliderScale(wnd.sldProgress, curCount * 1.0f / _m_stepRef.target_count);
            switch (_m_rechargeRebateInfo.groupRefObj.type)
            {
                case ERechargeRebateType.DAILY:
                case ERechargeRebateType.TOTAL:
                    ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.rechargeRebate_stepVipExpProgress_num_num, curCount, _m_stepRef.target_count));
                    break;
                case ERechargeRebateType.DAYS:
                    ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.rechargeRebate_stepDayProgress_num_num, curCount, _m_stepRef.target_count));
                    break;
            }
            //领奖状态显隐
            NPCommonEnumStatInfo<ECommonRewardType>.setStat(wnd.goRewardStatList, rewardType);
        }

        //点击充值按钮
        private void _onClickRecharge(GameObject _go)
        {
            //切换到购买钻石页面
            if(GGUIWndCashGiftPackMain.instance.isShow)
                GGUIWndCashGiftPackMain.instance.setSelectTab(ECashGiftPackMainTabType.GEM);
        }

        //点击领奖按钮
        private void _onClickGetReward(GameObject _go)
        {
            if (_m_stepRef == null || _m_rechargeRebateInfo == null || _m_rechargeRebateInfo.groupRefObj == null)
                return;

            if (_m_rechargeRebateInfo.getStepRewardType(_m_stepRef) == ECommonRewardType.CAN_GET_REWARD)
            {
                NPPlayer.instance.rechargeRebateComp.reqRechargeRebateDrawReward(_m_rechargeRebateInfo.activityInstanceId, _m_stepRef.group_id, _m_stepRef.step, null);
            }
        }
    }
}