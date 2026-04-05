using System;
using System.Collections.Generic;
using ALPackage;
using GC2GS.p017_ActivityOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基金战令通行证页面
    /// </summary>
    public class GGUIWndFundBattlePass : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoFundBattlePass>
    {
        //资源id
        private long _m_lUIResId;
        //基金信息快照（UI专用）
        private FundInfoSnapshot _m_fundSnapshot;
        //奖励列表Grid
        private GGUIWndFundBattlePassGrid _m_wStepGrid;
        //最后一档奖励
        private GGUIWndFundBattlePassGridItem _m_wLastStepItem;
        //大奖物品
        private NPGGUIWndCommonItem _m_wSpecialItem;
        //性价比物品
        private NPGGUIWndCommonItem _m_wProfitItem;
        //横幅图片
        private NPGGuiWndTexture _m_bannerTexture;
        

        public GGUIWndFundBattlePass(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }
        

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_wStepGrid?.showWnd();
            _m_wLastStepItem?.showWnd();
            _m_wSpecialItem?.showWnd();
            _m_wProfitItem?.showWnd();
            _m_bannerTexture?.showWnd();

            _refreshWnd(true);

            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);

            _m_wStepGrid?.hideWnd();
            _m_wLastStepItem?.hideWnd();
            _m_wSpecialItem?.hideWnd();
            _m_wProfitItem?.hideWnd();
            _m_bannerTexture?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wStepGrid?.resetWnd();
            _m_wLastStepItem?.resetWnd();
            _m_wSpecialItem?.resetWnd();
            _m_wProfitItem?.resetWnd();
            _m_bannerTexture?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_wStepGrid?.discard();
            _m_wStepGrid = null;
            _m_wLastStepItem?.discard();
            _m_wLastStepItem = null;
            _m_wSpecialItem?.discard();
            _m_wSpecialItem = null;
            _m_wProfitItem?.discard();
            _m_wProfitItem = null;
            _m_bannerTexture?.discard();
            _m_bannerTexture = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnPreview, _onClickPreview);
            ALUGUICommon.uncombineBtnClick(wnd.btnShop, _onClickShop);
            ALUGUICommon.uncombineBtnClick(wnd.btnBuy, _onClickBuy);
            ALUGUICommon.uncombineBtnClick(wnd.btnClaimAll, _onClickClaimAll);
            ALUGUICommon.uncombineBtnClick(wnd.btnTaskDetail, _onClickTaskDetail);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //初始化Grid
            if (wnd.monoStepGrid != null)
                _m_wStepGrid = new GGUIWndFundBattlePassGrid(wnd.monoStepGrid, _onLastStepChanged);
            //初始化最后一档奖励
            if (wnd.monoLastStep != null)
                _m_wLastStepItem = new GGUIWndFundBattlePassGridItem(wnd.monoLastStep);
            //初始化大奖物品
            if (wnd.monoSpecialItem != null)
                _m_wSpecialItem = new NPGGUIWndCommonItem(wnd.monoSpecialItem);
            if (wnd.monoProfitItem != null)
                _m_wProfitItem = new NPGGUIWndCommonItem(wnd.monoProfitItem);
            if (wnd.imgBanner != null)
                _m_bannerTexture = new NPGGuiWndTexture(wnd.imgBanner);

            //绑定按钮事件
            ALUGUICommon.combineBtnClick(wnd.btnPreview, _onClickPreview);
            ALUGUICommon.combineBtnClick(wnd.btnShop, _onClickShop);
            ALUGUICommon.combineBtnClick(wnd.btnBuy, _onClickBuy);
            ALUGUICommon.combineBtnClick(wnd.btnClaimAll, _onClickClaimAll);
            ALUGUICommon.combineBtnClick(wnd.btnTaskDetail, _onClickTaskDetail);
        }
        

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(FundInfoSnapshot _snapshot)
        {
            _m_fundSnapshot = _snapshot;
            _refreshWnd(true);
        }
        /// <summary>
        /// 基金信息变化（由父页面调用）
        /// </summary>
        public void onFundInfoChg(FundInfoSnapshot _snapshot)
        {
            if (_snapshot == null || _m_fundSnapshot == null)
                return;

            if (_snapshot.fundId != _m_fundSnapshot.fundId)
                return;

            //刷新显示
            _refreshWnd(true);
        }
        
        
        //刷新窗口
        private void _refreshWnd(bool _resetGird = false)
        {
            if (wnd == null || _m_fundSnapshot == null)
                return;

            ActivityFundLevelRefObj levelRef = _m_fundSnapshot.levelRef;
            if (levelRef == null)
                return;

            //设置活动时间
            _ABaseActivityInfo relatedActivityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_fundSnapshot.activityInstanceId);
            if (relatedActivityInfo != null)
            {
                DateTime startTime = TimeUtil.FromUTCByTimeZone(relatedActivityInfo.startTimeMs);
                DateTime endTime = TimeUtil.FromUTCByTimeZone(relatedActivityInfo.endTimeMs);
                ALUGUICommon.setLabelTxt(wnd.txtActivityTime, TimeUtil.DateTime2String_DurationLong_YMD(startTime, endTime));
            }
            wnd.setActivityRelatedState(relatedActivityInfo != null);

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(levelRef.activity_fund_name, levelRef.activity_fund_name_args));
            
            //设置激活提示文字
            bool hasBuy = _m_fundSnapshot.checkHasBuyFund();
            ALUGUICommon.setLabelTxt(wnd.txtActivateTip, TextTranslate.instance.getLanguage(levelRef.activate_tip, levelRef.activate_tip_args));
            ALUGUICommon.setLabelTxt(wnd.txtActivatedTip, TextTranslate.instance.getLanguage(levelRef.activated_tip, levelRef.activated_tip_args));
            wnd.setActivatedState(hasBuy);

            //设置性价比
            ALUGUICommon.setLabelTxt(wnd.txtProfitTip, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, levelRef.profit_per / 100));
            _m_wProfitItem?.setItem(levelRef.profit_item);

            //设置经验名称
            if (_m_fundSnapshot.fundRef != null)
                ALUGUICommon.setLabelTxt(wnd.txtExpName, TextTranslate.instance.getLanguage(_m_fundSnapshot.fundRef.exp_value_name));

            //设置横幅
            _m_bannerTexture?.setTexture(levelRef.banner);

            //设置大奖
            _refreshSpecialItem();

            //刷新奖励列表
            if (_m_wStepGrid != null)
            {
                _m_wStepGrid.showItemList(_m_fundSnapshot);
                if (_resetGird)
                {
                    //滚动到下一个可领奖的位置
                    _m_wStepGrid.frameRefresh();
                    int scrollToIdx = _getNextCanDrawStepIdx();
                    _m_wStepGrid.MoveIfCantSeeItem(scrollToIdx, EScrollToItemType.Center, false);
                }
            }

            //刷新最后一档奖励
            _refreshLastStep();
            
            //设置购买按钮状态
            wnd.setCanDrawState(_m_fundSnapshot.checkHasAnyRewardCanDraw());
        }
        //刷新大奖
        private void _refreshSpecialItem()
        {
            if (wnd == null || _m_fundSnapshot == null)
                return;

            ActivityFundLevelRefObj levelRef = _m_fundSnapshot.levelRef;
            if (levelRef?.special_item == null)
            {
                //无大奖
                wnd.setSpecialItemState(false, false);
                return;
            }

            //有大奖，检查是否已获得
            bool gotSpecialItem = GCommon.isItemEnough(levelRef.special_item.getItemType(), levelRef.special_item.subId, 1, false);
            wnd.setSpecialItemState(true, gotSpecialItem);

            //设置大奖物品显示
            _m_wSpecialItem?.showWnd();
            _m_wSpecialItem?.setItem(levelRef.special_item);
        }
        //刷新特殊档奖励（显示当前滚动位置的下一个special step，找不到则显示最后一个）
        private void _refreshLastStep()
        {
            if (_m_wLastStepItem == null || _m_fundSnapshot == null || _m_wStepGrid == null)
                return;

            List<ActivityFundStepRefObj> stepList = _m_fundSnapshot.getStepRefList();
            if (stepList.Count == 0)
                return;

            //获取当前滚动列表最后可见的step索引
            int currentStepIdx = _m_wStepGrid.getLastVisibleStepIdx();

            //从当前step的下一个开始，找下一个special step
            ActivityFundStepRefObj targetStep = null;
            for (int i = currentStepIdx + 1; i < stepList.Count; i++)
            {
                if (stepList[i].is_special_step)
                {
                    targetStep = stepList[i];
                    break;
                }
            }

            //找不到下一个special step，则显示最后一个step
            targetStep ??= stepList[^1];

            _m_wLastStepItem.showWnd();
            _m_wLastStepItem.setInfo(targetStep, _m_fundSnapshot);
        }
        //滚动列表最后可见step变化
        private void _onLastStepChanged()
        {
            _refreshLastStep();
        }
        //获取第一个未领取的step索引
        private int _getNextCanDrawStepIdx()
        {
            if (_m_fundSnapshot == null)
                return 0;

            List<ActivityFundStepRefObj> stepList = _m_fundSnapshot.getStepRefList();
            if (stepList.Count == 0)
                return 0;

            int hadDrawFreeStep = _m_fundSnapshot.hadDrawFreeStep;
            int hadDrawPayStep = _m_fundSnapshot.hadDrawPayStep;
            bool hasBuy = _m_fundSnapshot.checkHasBuyFund();

            int nextFreeIdx = -1;
            int nextPayIdx = -1;

            for (int i = 0; i < stepList.Count; i++)
            {
                ActivityFundStepRefObj stepRef = stepList[i];

                //找第一个免费档未领取的
                if (nextFreeIdx < 0 && hadDrawFreeStep < stepRef.step)
                    nextFreeIdx = i;

                //找第一个付费档未领取的（已购买时）
                if (hasBuy && nextPayIdx < 0 && hadDrawPayStep < stepRef.step)
                    nextPayIdx = i;

                //两个都找到了就可以退出
                if (nextFreeIdx >= 0 && (!hasBuy || nextPayIdx >= 0))
                    break;
            }

            //取两者最小的索引，如果都没有则返回最后一个
            if (nextFreeIdx < 0 && nextPayIdx < 0)
                return stepList.Count - 1;

            if (nextFreeIdx < 0)
                return nextPayIdx;

            if (nextPayIdx < 0)
                return nextFreeIdx;

            return Mathf.Min(nextFreeIdx, nextPayIdx);
        }
        //点击奖励预览
        private void _onClickPreview(GameObject _go)
        {
            GGUIWndFundPreview.instance.refreshWnd(_m_fundSnapshot);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFundPreview.instance, GGUIWndFundPreview.instance.showWnd, UINodeTagConst.C_FUND_PREVIEW);
        }
        private void _onClickTaskDetail(GameObject _go)
        {
            GGUIWndFundTaskDetail.instance.refreshWnd(_m_fundSnapshot);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFundTaskDetail.instance, GGUIWndFundTaskDetail.instance.showWnd, UINodeTagConst.C_FUND_TASK_DETAIL);
        }
        //点击商店
        private void _onClickShop(GameObject _go)
        {
            if (_m_fundSnapshot == null)
                return;

            ActivityFundRefObj fundRef = _m_fundSnapshot.fundRef;
            if (fundRef is not { activity_id: > 0 })
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndActivityExchangeShop.instance, () =>
            {
                GGUIWndActivityExchangeShop.instance.showWnd();
                GGUIWndActivityExchangeShop.instance.setInfo(fundRef.activity_id);
            }, UINodeTagConst.C_ACTIVITY_EXCHANGE_SHOP);
        }
        //点击购买
        private void _onClickBuy(GameObject _go)
        {
            if (_m_fundSnapshot?.fundRef == null)
                return;

            GGUIWndFundActivate activateWnd = GGUIWndFundActivate.getInstance(_m_fundSnapshot.fundRef.activate_ui_res_id);
            activateWnd.refreshWnd(_m_fundSnapshot);
            QueueMgr.instance.addNode_InGame_SingleWnd(activateWnd, activateWnd.showWnd, UINodeTagConst.C_FUND_ACTIVATE);
        }
        //点击一键领取
        private void _onClickClaimAll(GameObject _go)
        {
            if (_m_fundSnapshot == null)
                return;

            if (!_m_fundSnapshot.checkHasAnyRewardCanDraw())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.fund_noRewardCanDraw_none);
                return;
            }

            NPGSClientListener.sendMsgByLog(new GC2GS_017_018_ReqActivityFundDrawStepReward(_m_fundSnapshot.fundId));
        }        
        private void _onBagItemChg(object[] _objs)
        {
            if (_objs == null || _objs.Length < 1)
                return;

            if (_objs[0] is not BagItem bagItem)
                return;

            if (_m_fundSnapshot?.levelRef?.distinguish_item == null)
                return;
            
            if (bagItem.itemType == _m_fundSnapshot.levelRef.distinguish_item.itemType &&
                bagItem.itemId == _m_fundSnapshot.levelRef.distinguish_item.itemId)
            {
                //购买了基金，刷新显示
                _refreshWnd(true);
            }
        }
    }
}
