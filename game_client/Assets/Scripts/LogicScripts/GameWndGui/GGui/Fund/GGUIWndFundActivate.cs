using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基金激活购买页面
    /// </summary>
    public class GGUIWndFundActivate : _ATALBasicUIWnd<GGUIMonoFundActivate>
    {
        [NotNull] public static GGUIWndFundActivate getInstance(long _uiPathId)
        {
            if (!instanceDic.TryGetValue(_uiPathId, out GGUIWndFundActivate instance))
            {
                instance = new GGUIWndFundActivate(_uiPathId);
                instanceDic[_uiPathId] = instance;
            }
            return instance;
        }
        [NotNull] private static Dictionary<long, GGUIWndFundActivate> instanceDic = new Dictionary<long, GGUIWndFundActivate>();

        private readonly long _m_uiPathId;
        
        private FundInfoSnapshot _m_fundSnapshot;
        private NPGGuiWndTexture _m_wBannerTexture;
        private NPGGUIWndCommonItemContainer _m_wActivateToObtainContainer;
        private NPGGUIWndCommonItemContainer _m_wActivateMoreGoalsContainer;
        //性价比物品
        private NPGGUIWndCommonItem _m_wProfitItem;
        //通用购买按钮
        private GGUIWndCommonBuyButton _m_wBuyButton;


        private GGUIWndFundActivate(long _uiPathId)
            : base(EALUIWndLayer.ADDITION)
        {
            _m_uiPathId = _uiPathId;
        }


        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_wBannerTexture?.showWnd();
            _m_wActivateToObtainContainer?.showWnd();
            _m_wActivateMoreGoalsContainer?.showWnd();
            _m_wProfitItem?.showWnd();

            _refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wBannerTexture?.hideWnd();
            _m_wActivateToObtainContainer?.hideWnd();
            _m_wActivateMoreGoalsContainer?.hideWnd();
            _m_wProfitItem?.hideWnd();
            _m_wBuyButton?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wBannerTexture?.discardTexture();
            _m_wActivateToObtainContainer?.resetWnd();
            _m_wActivateMoreGoalsContainer?.resetWnd();
            _m_wProfitItem?.resetWnd();
            _m_wBuyButton?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_wBannerTexture?.discard();
            _m_wBannerTexture = null;
            _m_wActivateToObtainContainer?.discard();
            _m_wActivateToObtainContainer = null;
            _m_wActivateMoreGoalsContainer?.discard();
            _m_wActivateMoreGoalsContainer = null;
            _m_wProfitItem?.discard();
            _m_wProfitItem = null;
            _m_wBuyButton?.discard();
            _m_wBuyButton = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgBanner != null)
                _m_wBannerTexture = new NPGGuiWndTexture(wnd.imgBanner);
            if (wnd.monoActivateToObtain != null)
                _m_wActivateToObtainContainer = new NPGGUIWndCommonItemContainer(wnd.monoActivateToObtain);
            if (wnd.monoActivateMoreGoals != null)
                _m_wActivateMoreGoalsContainer = new NPGGUIWndCommonItemContainer(wnd.monoActivateMoreGoals);
            if (wnd.monoProfitItem != null)
                _m_wProfitItem = new NPGGUIWndCommonItem(wnd.monoProfitItem);
            if (wnd.monoBuyButton != null)
            {
                _m_wBuyButton = new GGUIWndCommonBuyButton(wnd.monoBuyButton);
                _m_wBuyButton.onClickButton += _onClickBuyButton;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        /// <summary>
        /// 设置基金信息
        /// </summary>
        public void refreshWnd(FundInfoSnapshot _snapshot)
        {
            _m_fundSnapshot = _snapshot;
            _refreshWnd();
        }


        private void _refreshWnd()
        {
            if (wnd == null || _m_fundSnapshot == null || !_m_bIsShow)
                return;

            ActivityFundLevelRefObj levelRef = _m_fundSnapshot.levelRef;
            if (levelRef == null)
                return;

            GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_m_fundSnapshot.levelRef.gift_pack_id);

            //设置标题
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(levelRef.activity_fund_name, levelRef.activity_fund_name_args));
            
            //设置性价比
            ALUGUICommon.setLabelTxt(wnd.txtProfitTip, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, levelRef.profit_per / 100));
            _m_wProfitItem?.setItem(levelRef.profit_item);

            //设置横幅
            _m_wBannerTexture?.setTexture(levelRef.banner);

            //设置激活提示
            ALUGUICommon.setLabelTxt(wnd.txtActivateTip, TextTranslate.instance.getLanguage(levelRef.activate_tip, levelRef.activate_tip_args));
            
            //设置购买按钮
            _m_wBuyButton?.showWnd();
            _m_wBuyButton?.setInfoList(giftPackRef?.cost_list);

            //刷新可立即获得的奖励
            _refreshActivateToObtain();

            //刷新更多目标奖励
            _refreshActivateMoreGoals();
        }
        /// <summary>
        /// 刷新激活后可立即获得的奖励
        /// </summary>
        private void _refreshActivateToObtain()
        {
            if (_m_wActivateToObtainContainer == null || _m_fundSnapshot == null)
                return;

            List<ActivityFundStepRefObj> stepRefList = _m_fundSnapshot.getStepRefList();
            List<NPCommonCostItem> obtainItemList = new List<NPCommonCostItem>();

            long totalScore = _m_fundSnapshot.totalScore;
            int hadDrawPayStep = _m_fundSnapshot.hadDrawPayStep;

            //遍历当前等级的所有阶段，找出可以立即领取的付费奖励
            for (int i = 0; i < stepRefList.Count; i++)
            {
                ActivityFundStepRefObj stepRef = stepRefList[i];
                //积分已达到且付费档未领取
                if (totalScore >= stepRef.need_count && hadDrawPayStep < stepRef.step)
                {
                    if (stepRef.pay_reward_item_list != null)
                        obtainItemList.AddRange(stepRef.pay_reward_item_list);
                }
            }
            
            if (_m_fundSnapshot.levelRef.activate_exp_count > 0)
                obtainItemList.Add(new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.activity_fund_battle_pass_exp_item, _m_fundSnapshot.levelRef.activate_exp_count));

            //显示列表（合并同类物品）
            List<_IItem> mergedList = obtainItemList.toMergeItemDataList(true);
            _m_wActivateToObtainContainer.showWnd();
            _m_wActivateToObtainContainer.showItemList(mergedList);

            //设置无物品状态
            wnd.setNoItemObtainState(mergedList == null || mergedList.Count == 0);
        }
        /// <summary>
        /// 刷新激活后可获得的更多目标奖励（除立即可获得外的剩余付费奖励）
        /// </summary>
        private void _refreshActivateMoreGoals()
        {
            if (_m_wActivateMoreGoalsContainer == null || _m_fundSnapshot == null)
                return;

            List<ActivityFundStepRefObj> stepRefList = _m_fundSnapshot.getStepRefList();
            List<NPCommonCostItem> moreGoalsItemList = new List<NPCommonCostItem>();

            long totalScore = _m_fundSnapshot.totalScore;

            //遍历当前等级的所有阶段，找出积分尚未达到的付费奖励
            for (int i = 0; i < stepRefList.Count; i++)
            {
                ActivityFundStepRefObj stepRef = stepRefList[i];
                //积分未达到的阶段才算"更多目标"
                if (totalScore < stepRef.need_count)
                {
                    if (stepRef.pay_reward_item_list != null)
                        moreGoalsItemList.AddRange(stepRef.pay_reward_item_list);
                }
            }

            //显示列表（合并同类物品）
            List<_IItem> mergedList = moreGoalsItemList.toMergeItemDataList(true);
            _m_wActivateMoreGoalsContainer.showWnd();
            _m_wActivateMoreGoalsContainer.showItemList(mergedList);

            //设置无物品状态
            wnd.setNoItemMoreGoalsState(mergedList == null || mergedList.Count == 0);
        }
        private void _onClickClose(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_FUND_ACTIVATE);
        }
        //点击购买按钮
        private void _onClickBuyButton(NPCommonCostItem _costItem, bool _isSellOut)
        {
            if (_isSellOut)
                return;

            GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_m_fundSnapshot.levelRef.gift_pack_id);
            if (giftPackRef == null)
                return;

            if (_costItem == null || _costItem.getItemType() != ENPItemType.PAY)
            {
                //免费购买或道具购买
                NPPlayer.instance.giftPackComp.reqBuyGiftPack(giftPackRef.id);
            }
            else
            {
                //支付流程
                GCommon.reqPay(giftPackRef, () => _onClickClose(null), null);
            }
        }
    }
}
