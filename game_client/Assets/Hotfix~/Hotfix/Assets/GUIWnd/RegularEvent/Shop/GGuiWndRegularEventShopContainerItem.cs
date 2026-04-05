using ALPackage;
using Common.ActivityEnum;
using CommonEnum;
using GOE;
using NPEnum;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动商店商品item
    /// </summary>
    public class GGuiWndRegularEventShopContainerItem : _AHotfixBaseSubWnd<GGUIMonoRegularEventShopContainerItem>
    {
        //商品信息
        private RegularEventShopItem _m_shopItemInfo;
        //目标道具
        private NPGGUIWndCommonItem _m_wTargetItem;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        // 数量计数器
        private GGUIWndBagPopCounter _m_wCounter;
        // 使用数量
        private long _m_lUseCount = 1;

        public GGuiWndRegularEventShopContainerItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lUseCount = 1;
            _m_wTargetItem?.hideWnd();
            _m_wCostItem?.hideWnd();
            _m_wCounter?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTargetItem?.resetWnd();
            _m_wCostItem?.resetWnd();
            _m_wCounter?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_lUseCount = 1;
            _m_wTargetItem?.discard();
            _m_wTargetItem = null;
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            _m_wCounter?.discard();
            _m_wCounter = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnFree, _onClickFree);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnBuy, _onClickBuy);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnAccess, _onClickAccess);
        }

        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            if (hotfixWnd.monoTargetItem != null)
                _m_wTargetItem = new NPGGUIWndCommonItem(hotfixWnd.monoTargetItem);

            if(hotfixWnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(hotfixWnd.monoCostItem);

            if (hotfixWnd.monoUseCounter != null)
            {
                _m_wCounter = new GGUIWndBagPopCounter(hotfixWnd.monoUseCounter);
                _m_wCounter.regCounterChangedEvent(_onCounterChanged);
            }

            ALUGUICommon.combineBtnClick(hotfixWnd.btnFree, _onClickFree);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnBuy, _onClickBuy);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnAccess, _onClickAccess);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_shopItem"></param>
        public void setInfo(RegularEventShopItem _shopItem)
        {
            if(hotfixWnd == null || _shopItem == null)
                return;

            _m_shopItemInfo = _shopItem;
            _refreshWnd();
        }

        //刷新界面
        private void _refreshWnd()
        {
            if (hotfixWnd == null || _m_shopItemInfo == null || _m_shopItemInfo.regularEventShopItemRef == null)
                return;

            //初始化数量计数器
            _m_wCounter?.init(_m_shopItemInfo.leftBuyCount);

            //刷新数量消耗
            _refreshCount();

            //目标道具
            _m_wTargetItem?.showWnd();
            _m_wTargetItem?.setItem(_m_shopItemInfo.regularEventShopItemRef.item);

            //增加积分描述
            ALUGUICommon.setLabelTxt(hotfixWnd.txtUseAddPoint, TextTranslate.instance.getLanguage(HotfixTransKeyConst.regularEvent_addScore_num.replaceActivity(_m_shopItemInfo.regularEventShopItemRef.activity_id), _m_shopItemInfo.regularEventShopItemRef.use_gain_activity_currency_count));

            //刷新状态
            ALUGUICommon.setGameObjEnable(hotfixWnd.goFreeShowList, _m_shopItemInfo.isFree);
            ALUGUICommon.setGameObjEnable(hotfixWnd.goFreeHideList, !_m_shopItemInfo.isFree);
            ALUGUICommon.setGameObjEnable(hotfixWnd.goSellOutShowList, _m_shopItemInfo.isSellOut);
            ALUGUICommon.setGameObjEnable(hotfixWnd.goSellOutHideList, !_m_shopItemInfo.isSellOut);
        }

        //刷新数量消耗
        private void _refreshCount()
        {
            if (hotfixWnd == null || _m_shopItemInfo == null || _m_shopItemInfo.regularEventShopItemRef == null)
                return;

            //消耗道具
            if (_m_shopItemInfo.regularEventShopItemRef.buy_cost != null && _m_shopItemInfo.regularEventShopItemRef.buy_cost.getItemType() != ENPItemType.NONE)
            {
                NPCommonCostItem costItem = new NPCommonCostItem(_m_shopItemInfo.regularEventShopItemRef.buy_cost);
                costItem.setCount(costItem.count * _m_lUseCount);
                _m_wCostItem?.showWnd();
                _m_wCostItem?.setItem(costItem);
            }
            else
                _m_wCostItem?.hideWnd();

            //购买限制
            ALUGUICommon.setLabelTxt(hotfixWnd.txtBuyLimit, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, _m_lUseCount, _m_shopItemInfo.leftBuyCount));
        }

        //计数器更改事件
        private void _onCounterChanged(long _newCount)
        {
            _m_lUseCount = _newCount;
            _refreshCount();
        }

        //点击免费购买
        private void _onClickFree(GameObject _go)
        {
            if (_m_shopItemInfo == null || !_m_shopItemInfo.isFree || _m_shopItemInfo.regularEventShopItemRef == null)
                return;

            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_shopItemInfo.regularEventShopItemRef.activity_id);
            if (activityInfo == null || !activityInfo.isPlaying)
            {
                //活动已结束，关闭窗口
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_activity_alreadyEnd_none);
                QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.REGULAR_EVENT_SHOP);
                return;
            }

            HotfixNPPlayer.instance.regularEventComponent.reqRegularActivityShopBuyItem(_m_shopItemInfo.activityInstanceId, _m_shopItemInfo.regularEventShopItemRef.id, (int)_m_lUseCount);
        }

        //点击消耗购买
        private void _onClickBuy(GameObject _go)
        {
            if (_m_shopItemInfo == null || _m_shopItemInfo.regularEventShopItemRef == null)
                return;

            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_shopItemInfo.regularEventShopItemRef.activity_id);
            if (activityInfo == null || !activityInfo.isPlaying)
            {
                //活动已结束，关闭窗口
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_activity_alreadyEnd_none);
                QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.REGULAR_EVENT_SHOP);
                return;
            }

            NPCommonCostItem costItem = new NPCommonCostItem(_m_shopItemInfo.regularEventShopItemRef.buy_cost);
            costItem.setCount(costItem.count * _m_lUseCount);

            if (!GCommon.isItemEnough(costItem, true))
                return;

            HotfixNPPlayer.instance.regularEventComponent.reqRegularActivityShopBuyItem(_m_shopItemInfo.activityInstanceId, _m_shopItemInfo.regularEventShopItemRef.id, (int)_m_lUseCount);
        }

        //点击获取途径按钮
        private void _onClickAccess(GameObject _go)
        {
            if (_m_shopItemInfo == null || _m_shopItemInfo.regularEventShopItemRef == null || _m_shopItemInfo.regularEventShopItemRef.item == null)
                return;

            GCommon.popItemAccessWays(_m_shopItemInfo.regularEventShopItemRef.item.getItemType(), _m_shopItemInfo.regularEventShopItemRef.item.subId);
        }
    }
}