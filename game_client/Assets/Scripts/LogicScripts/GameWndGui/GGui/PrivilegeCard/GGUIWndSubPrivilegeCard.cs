using ALPackage;
using Common.PrivilegeCardEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 权益卡子窗口
    /// </summary>
    public class GGUIWndSubPrivilegeCard : _ATALBasicUISubWnd<GGUIMonoSubPrivilegeCard>
    {
        //权益卡类型
        private EPrivilegeCardType _m_eType;
        //权益卡配置信息
        private PrivilegeCardRefObj _m_privilegeCardRef;
        //是否正在领取奖励
        private bool _m_bIsGettingReward;
        //显示序列号
        private long _m_lShowSerialize;
        //购买按钮
        private GGUIWndCommonBuyButton _m_wBuyButton;
        //购买奖励列表
        private NPGGUIWndCommonItemContainer _m_wBuyRewardContainer;
        //每日奖励列表
        private NPGGUIWndCommonItemContainer _m_wDailyRewardContainer;
        //权益描述列表
        private GGUIWndPrivilegeCardDescContainer _m_wDescContainer;
        //定时任务
        private ALCommonEnableTaskController _m_cdTask;

        /// <summary>
        /// 权益卡类型
        /// </summary>
        public EPrivilegeCardType cardType { get { return _m_eType; } }

        public GGUIWndSubPrivilegeCard(EPrivilegeCardType _type, GGUIMonoSubPrivilegeCard _wnd) : base(_wnd)
        {
            _m_eType = _type;
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PRIVILEGE_CARD_CHG, _onPrivilegeCardChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PRIVILEGE_CARD_REMOVE, _onPrivilegeCardChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_privilegeCardRef = GRefdataCoreMgr.instance.privilegeCardRefCore.getRef((long) _m_eType);
            _refreshWnd();
            //开启定时检查
            _startCheck();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PRIVILEGE_CARD_CHG, _onPrivilegeCardChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PRIVILEGE_CARD_REMOVE, _onPrivilegeCardChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsGettingReward = false;

            _m_wBuyButton?.hideWnd();
            _m_wBuyRewardContainer?.hideWnd();
            _m_wDailyRewardContainer?.hideWnd();
            _m_wDescContainer?.hideWnd();
            _stopCheck();
        }

        protected override void _onReset()
        {
            _m_wBuyButton?.resetWnd();
            _m_wBuyRewardContainer?.resetWnd();
            _m_wDailyRewardContainer?.resetWnd();
            _m_wDescContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wBuyButton?.discard();
            _m_wBuyButton = null;
            _m_wBuyRewardContainer?.discard();
            _m_wBuyRewardContainer = null;
            _m_wDailyRewardContainer?.discard();
            _m_wDailyRewardContainer = null;
            _m_wDescContainer?.discard();
            _m_wDescContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnAlreadyGetReward, _onClickAlreadyGetReward);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoBuyButton != null)
            {
                _m_wBuyButton = new GGUIWndCommonBuyButton(wnd.monoBuyButton);
                _m_wBuyButton.onClickButton += _onClickBuy;
            }

            if(wnd.monoBuyRewardContainer != null)
                _m_wBuyRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoBuyRewardContainer);

            if(wnd.monoDailyRewardContainer != null)
                _m_wDailyRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoDailyRewardContainer);

            if(wnd.monoDescContainer != null)
                _m_wDescContainer = new GGUIWndPrivilegeCardDescContainer(wnd.monoDescContainer);

            ALUGUICommon.combineBtnClick(wnd.btnGetReward,_onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnAlreadyGetReward, _onClickAlreadyGetReward);
        }

        /// <summary>
        /// 显示手指指引
        /// </summary>
        public void showHandGuide()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goGuideHand, true);
            if (wnd.delayHideGuideHand > 0)
            {
                long curSerialize = _m_lShowSerialize;
                ALCommonActionMonoTask.addMonoTask(()=>
                {
                    if (curSerialize != _m_lShowSerialize)
                        return;

                    hideHandGuide();
                }, wnd.delayHideGuideHand);
            }
        }

        /// <summary>
        /// 隐藏手指指引
        /// </summary>
        public void hideHandGuide()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goGuideHand, false);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            _refreshCardInfo();
            _refreshState();
        }

        /// <summary>
        /// 刷新权益卡信息
        /// </summary>
        private void _refreshCardInfo()
        {
            if (_m_privilegeCardRef == null || wnd == null)
                return;

            GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_m_privilegeCardRef.gift_pack_id);
            //设置购买按钮
            _m_wBuyButton?.showWnd();
            _m_wBuyButton?.setInfoList(giftPackRef?.cost_list);

            //设置购买奖励
            _m_wBuyRewardContainer?.showWnd();
            _m_wBuyRewardContainer?.showItemList(giftPackRef?.item_list);

            //设置每日奖励
            _m_wDailyRewardContainer?.showWnd();
            _m_wDailyRewardContainer?.showItemList(_m_privilegeCardRef.daily_gain_item_list);

            //设置权益描述
            _m_wDescContainer?.showWnd();
            _m_wDescContainer?.setInfo(_m_privilegeCardRef.privilege_desc_title_list, _m_privilegeCardRef.privilege_desc_content_list);

            //设置奖励描述
            ALUGUICommon.setLabelTxt(wnd.txtRewardDesc, TextTranslate.instance.getLanguage(_m_privilegeCardRef.reward_desc, _m_privilegeCardRef.reward_desc_args));

            //设置折扣价值百分比
            ALUGUICommon.setLabelTxt(wnd.txtProfitPer, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, giftPackRef?.profit_per / 100));

            //重置剩余时间显示
            ALUGUICommon.setLabelTxt(wnd.txtLeftTime, "");

            //隐藏引导购买手
            hideHandGuide();
        }

        /// <summary>
        /// 刷新显示状态
        /// </summary>
        private void _refreshState()
        {
            if (_m_privilegeCardRef == null || wnd == null)
                return;

            PrivilegeCardInfo cardInfo = NPPlayer.instance.privilegeCardComp.getPrivilegeCardInfo(_m_eType);
            //是否激活
            bool isActivate = cardInfo != null && cardInfo.isActivate;
            //今日是否可领奖
            bool canGetReward = cardInfo != null && cardInfo.canGetRewardToday;
            //权益卡状态
            EPrivilegeCardRewardState state = EPrivilegeCardRewardState.NOT_ACTIVATE;
            if (isActivate && canGetReward)
                state = EPrivilegeCardRewardState.ACTIVATE_CAN_GET_REWARD;
            else if (isActivate && !canGetReward)
                state = EPrivilegeCardRewardState.ACTIVATE_ALREADY_GET_REWARD;
            else
                state = EPrivilegeCardRewardState.NOT_ACTIVATE;

            //设置显隐
            NPCommonEnumStatInfo<EPrivilegeCardRewardState>.setStat(wnd.privilegeCardStateList, state);
        }

        #region 倒计时

        /// <summary>
        /// 开启定时检查
        /// </summary>
        private void _startCheck()
        {
            _m_cdTask.setDisable();
            _m_cdTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCD, 0.2f);
        }

        /// <summary>
        /// 关闭定时检查
        /// </summary>
        private void _stopCheck()
        {
            _m_cdTask.setDisable();
        }

        /// <summary>
        /// 倒计时检查
        /// </summary>
        private void _tickCD()
        {
            if (wnd == null || _m_privilegeCardRef == null)
            {
                _stopCheck();
                return;
            }

            PrivilegeCardInfo cardInfo = NPPlayer.instance.privilegeCardComp.getPrivilegeCardInfo(_m_eType);
            if (cardInfo == null || !cardInfo.isActivate)
            {
                _stopCheck();
                _refreshState();
                return;
            }

            long leftTimeS = cardInfo.endTimeS - FpsAndPingMgr.instance.serverTimeTagS;
            ALUGUICommon.setLabelTxt(wnd.txtLeftTime, TextTranslate.instance.getLanguage(TransKeyConst.privilegeCard_activateLeftTime_str, TimeUtil.millisecondsToTime_Two(leftTimeS * 1000)));
        }

        #endregion

        #region 点击事件

        /// <summary>
        /// 点击领取奖励
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickGetReward(GameObject obj)
        {
            if (_m_bIsGettingReward)
                return;

            PrivilegeCardInfo cardInfo = NPPlayer.instance.privilegeCardComp.getPrivilegeCardInfo(_m_eType);

            if (cardInfo == null || !cardInfo.canGetRewardToday)
                return;

            //标记正在领取奖励
            _m_bIsGettingReward = true;

            //请求领取奖励
            long curSerialize = _m_lShowSerialize;
            NPPlayer.instance.privilegeCardComp.reqGainPrivilegeCardDailyReward(cardInfo.cardType, (_isSuc, _msg) =>
            {
                if (curSerialize != _m_lShowSerialize)
                    return;

                _m_bIsGettingReward = false;
            });
        }

        /// <summary>
        /// 点击已领取奖励按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickAlreadyGetReward(GameObject _go)
        {
            //今日奖励已领取
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.privilegeCard_alreadyGetRewardToday_none);
        }

        /// <summary>
        /// 点击购买
        /// </summary>
        /// <param name="_costItem"></param>
        /// <param name="_isSellOut"></param>
        private void _onClickBuy(NPCommonCostItem _costItem, bool _isSellOut)
        {
            if (wnd == null || _m_privilegeCardRef == null || _costItem == null || _costItem.getItemType() != ENPItemType.PAY)
                return;

            hideHandGuide();
            GiftPackRefObj giftPackRefObj = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_m_privilegeCardRef.gift_pack_id);
            if (giftPackRefObj == null)
                return;
            
            //请求购买
            GCommon.reqPay(giftPackRefObj, null, null);
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 权益卡变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onPrivilegeCardChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length <= 0)
                return;

            EPrivilegeCardType type = (EPrivilegeCardType)_objects[0];
            if (type == _m_eType)
            {
                _refreshState();
                //开启定时检查
                _startCheck();
            }
        }

        /// <summary>
        /// 跨天消息
        /// </summary>
        private void _onCrossDay()
        {
            _refreshState();
            //开启定时检查
            _startCheck();
        }

        #endregion

    }
}
