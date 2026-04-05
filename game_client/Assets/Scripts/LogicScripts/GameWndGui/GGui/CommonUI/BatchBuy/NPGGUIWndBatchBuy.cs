using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 批量购买商品弹窗
    public class NPGGUIWndBatchBuy : _ANPGGUIBasicWnd<NPGGUIMonoBatchBuy>
    {
        private static NPGGUIWndBatchBuy _g_instance = new NPGGUIWndBatchBuy();
        public static NPGGUIWndBatchBuy instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGGUIWndBatchBuy();

                return _g_instance;
            }
        }

        // 获得的物品数据
        private NPCommonCostItem _m_gainItem;

        // 获得物品的基础展示数据
        private NPGGUIWndCommonItem _m_gainItemWnd;

        //打折资源加载器
        private CommonAssetLoader _m_discountAssetLoader;

        //额外资源加载器
        private CommonAssetLoader _m_exAssetLoader;

        // 使用数量
        private long _m_lUseCount = 1;

        //消耗展示item 用于展示
        private NPCommonCostItem _m_costShowItem;

        //单个消耗
        private NPCommonCostItem _m_onceCostItem;

        //次数递增表id
        private long _m_timePriceTypeId;

        //次数起始下标
        private int _m_timePriceStartTime;

        //打折配表
        private NPShopItemDiscountRefObj _m_discountRefObj;

        //消耗显示
        private NPGGUIWndCommonItem _m_costUseItem;

        // 数量计数器
        private GGUIWndBagPopCounter _m_wCounter;

        //点击确定按钮回调
        private Action<long> _m_confirmAction;

        public NPGGUIWndBatchBuy()
            : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return NPGGUIMonoBatchBuy.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoBatchBuy.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SHOP_AUTO_REFRESH, _shopAutoRefreshMsg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_ACTIVITY_SHOP_REFRESH, _shopAutoRefreshMsg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _shopAutoRefreshMsg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BATCH_BUY_CONFIRM, _simulateClickConfirmBtn);

            //绑定按钮
            ALUGUICommon.combineBtnClick(wnd.costUseBtn, _onConfirmBtnClick);
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);
        }

        protected override void _onHideWnd()
        {
            // 重置数量
            _m_lUseCount = 1;

            WinMsg.UnregisterMsgAct(WinMsgType.SHOP_AUTO_REFRESH, _shopAutoRefreshMsg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ACTIVITY_SHOP_REFRESH, _shopAutoRefreshMsg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _shopAutoRefreshMsg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BATCH_BUY_CONFIRM, _simulateClickConfirmBtn);

            //绑定按钮
            ALUGUICommon.uncombineBtnClick(wnd.costUseBtn, _onConfirmBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onCloseBtnClick);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_gainItem = null;
            _m_lUseCount = 1;
            _m_costShowItem = null;
            _m_onceCostItem = null;
            _m_discountRefObj = null;

            if (_m_gainItemWnd != null)
                _m_gainItemWnd.discard();
            _m_gainItemWnd = null;

            if (_m_wCounter != null)
                _m_wCounter.discard();
            _m_wCounter = null;

            if (null != _m_costUseItem)
                _m_costUseItem.discard();
            _m_costUseItem = null;

            if (null != _m_discountAssetLoader)
                _m_discountAssetLoader.discard();
            _m_discountAssetLoader = null;

            if (null != _m_exAssetLoader)
                _m_exAssetLoader.discard();
            _m_exAssetLoader = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd.gainItemMono != null)
                _m_gainItemWnd = new NPGGUIWndCommonItem(wnd.gainItemMono);

            if (wnd.useCounter != null)
            {
                _m_wCounter = new GGUIWndBagPopCounter(wnd.useCounter);
                _m_wCounter.regCounterChangedEvent(_onCounterChanged);
            }

            if (null != wnd.costUseItem)
                _m_costUseItem = new NPGGUIWndCommonItem(wnd.costUseItem);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_gainItem">获得的物品</param>
        /// <param name="_onceCostItem">单次需要消耗的物品</param>
        /// <param name="_lastBuyCount">剩余购买的个数限制</param>
        /// <param name="_buyCountMax">购买数量上限 -1表示无上限限制</param>
        /// <param name="_timePriceTypeId">次数递增表类型id</param>
        /// <param name="_timePriceStartTime">起始次数</param>

        /// <param name="_discountRefId">打折配表id</param>
        ///<param name="_exResId">额外显示的资源id</param>

        public void setData(NPCommonCostItem _gainItem, NPCommonCostItem _onceCostItem, long _lastBuyCount = -1, long _buyCountMax = -1, long _timePriceTypeId = 0, int _timePriceStartTime = 0, long _discountRefId = 0, long _exResId = 0, string _limitCountKey = null, Action<long> _confirmAction = null)
        {
            //免费的也不能批量购买
            if (null == _gainItem || null == _gainItem.item || null == wnd)
                return;

            _m_gainItem = _gainItem;
            _m_onceCostItem = _onceCostItem;
            _m_confirmAction = _confirmAction;
            _m_timePriceTypeId = _timePriceTypeId;
            _m_timePriceStartTime = _timePriceStartTime;

            bool isFree = _onceCostItem == null;
            //免费显示
            ALUGUICommon.setGameObjEnable(wnd.freeHideGoList, !isFree);
            ALUGUICommon.setGameObjEnable(wnd.freeShowGoList, isFree);

            _m_discountRefObj = null;
            if (0 != _discountRefId)
                _m_discountRefObj = GRefdataCoreMgr.instance.shopItemDiscountMap.getRef(_discountRefId);

            bool isLimit = _buyCountMax != -1;

            //防止数据错误
            if (isLimit && _lastBuyCount < 0)
                _lastBuyCount = 0;

            //设置获得物品
            if (_m_gainItemWnd != null)
                _m_gainItemWnd.setItem(_gainItem);
            ALUGUICommon.setLabelTxt(wnd.gainItemDetail, GCommon.getItemDesc(_gainItem.item.itemType, _gainItem.item.itemId));

            //设置打折标识
            if (null != wnd.discountPosParent)
            {
                if (null != _m_discountAssetLoader)
                {
                    _m_discountAssetLoader.discard();
                    if (null != _m_discountRefObj)
                        _m_discountAssetLoader.loadAsset(_m_discountRefObj.ui_res_id, wnd.discountPosParent);
                }
                else
                {
                    _m_discountAssetLoader = new CommonAssetLoader();
                    if (null != _m_discountRefObj)
                        _m_discountAssetLoader.loadAsset(_m_discountRefObj.ui_res_id, wnd.discountPosParent);
                }
            }

            //资源标识
            if (null != wnd.flagPosParent)
            {
                if (null != _m_exAssetLoader)
                {
                    _m_exAssetLoader.discard();
                    _m_exAssetLoader.loadAsset(_exResId, wnd.flagPosParent);
                }
                else
                {
                    _m_exAssetLoader = new CommonAssetLoader();
                    _m_exAssetLoader.loadAsset(_exResId, wnd.flagPosParent);
                }
            }

            //设置购买计数器
            if (null != _m_wCounter)
            {
                long count = GCommon.getCostItemCount(_m_onceCostItem);
                if (isLimit)
                    count = Math.Min(count, _lastBuyCount);
                if (isFree)
                {
                    if (isLimit)
                        count = _lastBuyCount;
                    else
                        count = 99;
                }
                _m_wCounter.init(count);
            }

            //刷新消耗显示 
            _refreshUseItemCount();

            //设置限购显示
            ALUGUICommon.setGameObjEnable(wnd.hasBuyLimitList, isLimit);
            //如果有限购
            if (isLimit)
            {
                string limitKey = TransKeyConst.shop_buyLimitCout_num_num;
                if (!string.IsNullOrEmpty(_limitCountKey))
                    limitKey = _limitCountKey;

                //设置限购文本
                ALUGUICommon.setLabelTxt(wnd.limitBuyTxt, TextTranslate.instance.getLanguage(limitKey, _lastBuyCount, _buyCountMax));
            }

            //数量为1时隐藏
            if (_gainItem.count == 1 && wnd.isOneLeftNeedHide)
                ALUGUICommon.setGameObjEnable(wnd.oneLeftHideGoList, false);

            bool isEnough = GCommon.isItemEnough(_m_costShowItem, false);
            ALUGUICommon.setGameObjEnable(wnd.costEnoughShowGoList, isEnough);
            ALUGUICommon.setGameObjEnable(wnd.costNoEnoughShowGoList, !isEnough);
        }

        /// <summary>
        /// 刷新消耗显示
        /// </summary>
        private void _refreshUseItemCount()
        {
            if (null == _m_costUseItem || null == _m_onceCostItem)
                return;

            //优先展示次数递增
            if (0 != _m_timePriceTypeId)
            {
                if (null == _m_costShowItem)
                    _m_costShowItem = GRefdataCoreMgr.instance.getTimesPriceCostItem(_m_timePriceTypeId, 1);
                long costCount = GRefdataCoreMgr.instance.getBatchTimePriceCostItem(_m_timePriceTypeId, _m_timePriceStartTime, _m_lUseCount);
                _m_costShowItem?.setCount(costCount);
            }
            else
            {
                //_m_onceCostItem是打折后的
                if (null == _m_costShowItem)
                    _m_costShowItem = new NPCommonCostItem(_m_onceCostItem.item, 1);
                _m_costShowItem.setCount(_m_onceCostItem.count * _m_lUseCount);
            }
            _m_costUseItem.setItem(_m_costShowItem);
        }

        /// <summary>
        /// 商店定时刷新通知
        /// </summary>
        private void _shopAutoRefreshMsg()
        {
            //关闭弹窗
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Shop.C_ADD_SHOP_BUY_ITEM_NODE);
        }

        // 响应确定按钮点击事件
        private void _onConfirmBtnClick(GameObject _go)
        {
            if (!GCommon.isItemEnough(_m_costShowItem, true))
                return;

            GCommon.showCommonBuyItemConfirm(_m_costShowItem, (count) =>
            {
                if (null != _m_confirmAction)
                    _m_confirmAction(count);

                //关闭弹窗
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Shop.C_ADD_SHOP_BUY_ITEM_NODE);

            },null, _m_lUseCount,_m_gainItem);
        }

        // 响应关闭按钮点击事件
        private void _onCloseBtnClick(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Shop.C_ADD_SHOP_BUY_ITEM_NODE);
        }

        // 响应计数器更改事件
        private void _onCounterChanged(long _newCount)
        {
            _m_lUseCount = _newCount;

            _refreshUseItemCount();
        }

        //模拟点击批量购买确认
        private void _simulateClickConfirmBtn()
        {
            _onConfirmBtnClick(null);
        }
    }
}
