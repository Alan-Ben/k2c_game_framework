using UnityEngine;
using ALPackage;
using System;
using System.Collections.Generic;
using Common.ActivityEnum;

namespace GOE
{
    /// <summary>
    /// 礼包批量购买商品弹窗
    /// </summary>
    public class GGUIWndGiftPackBatchBuy : _ANPGGUIBasicWnd<GGUIMonoGiftPackBatchBuy>
    {
        private static GGUIWndGiftPackBatchBuy _g_instance = new GGUIWndGiftPackBatchBuy();
        public static GGUIWndGiftPackBatchBuy instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGiftPackBatchBuy();

                return _g_instance;
            }
        }

        //礼包信息
        private _IGiftPackInfo _m_giftPackInfo;
        //活动实例id
        private long _m_lActivityInstanceId;
        //获得的物品列表
        private NPGGUIWndCommonItemContainer _m_wGainItemContainer;
        //使用数量
        private long _m_lUseCount = 1;
        //消耗展示item 用于展示
        private NPCommonCostItem _m_costShowItem;
        //消耗显示
        private NPGGUIWndCommonItem _m_costUseItem;
        // 数量计数器
        private GGUIWndBagPopCounter _m_wCounter;
        //点击确定按钮回调
        private Action<int> _m_confirmAction;

        public GGUIWndGiftPackBatchBuy() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGiftPackBatchBuy.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGiftPackBatchBuy.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _shopAutoRefreshMsg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _shopAutoRefreshMsg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            // 重置数量
            _m_lUseCount = 1;

            if (_m_wGainItemContainer != null)
                _m_wGainItemContainer.hideWnd();

            if (_m_wCounter != null)
                _m_wCounter.hideWnd();

            if (null != _m_costUseItem)
                _m_costUseItem.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wGainItemContainer != null)
                _m_wGainItemContainer.resetWnd();

            if (_m_wCounter != null)
                _m_wCounter.resetWnd();

            if (null != _m_costUseItem)
                _m_costUseItem.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_lUseCount = 1;
            _m_costShowItem = null;

            if (_m_wGainItemContainer != null)
                _m_wGainItemContainer.discard();
            _m_wGainItemContainer = null;

            if (_m_wCounter != null)
                _m_wCounter.discard();
            _m_wCounter = null;

            if (null != _m_costUseItem)
                _m_costUseItem.discard();
            _m_costUseItem = null;

            if (wnd == null)
                return;

            //绑定按钮
            ALUGUICommon.uncombineBtnClick(wnd.costUseBtn, _onConfirmBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onCloseBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGainItemContainer != null)
                _m_wGainItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoGainItemContainer);

            if (wnd.useCounter != null)
            {
                _m_wCounter = new GGUIWndBagPopCounter(wnd.useCounter);
                _m_wCounter.regCounterChangedEvent(_onCounterChanged);
            }

            if (wnd.costUseItem != null)
                _m_costUseItem = new NPGGUIWndCommonItem(wnd.costUseItem);

            //绑定按钮
            ALUGUICommon.combineBtnClick(wnd.costUseBtn, _onConfirmBtnClick);
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onCloseBtnClick);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_info">礼包信息</param>
        /// <param name="_confirmAction">点击购买回调</param>
        public void setData(_IGiftPackInfo _info, long _activityInstanceId, Action<int> _confirmAction = null)
        {
            _m_giftPackInfo = _info;
            _m_lActivityInstanceId = _activityInstanceId;
            _m_confirmAction = _confirmAction;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (_m_giftPackInfo == null || wnd == null)
                return;

            //免费显示
            bool isFree = _m_giftPackInfo.isFree;
            bool isLimit = _m_giftPackInfo.buyCountMax > 0;
            ALUGUICommon.setGameObjEnable(wnd.freeHideGoList, !isFree);
            ALUGUICommon.setGameObjEnable(wnd.freeShowGoList, isFree);

            //刷新获得物品列表
            _refreshGainItemList();

            //设置购买计数器
            if (null != _m_wCounter)
            {
                int count = (int)GCommon.getCostItemCount(_m_giftPackInfo.getCostItemByDiscount());
                if (isLimit)
                    count = (int)Math.Min(count, _m_giftPackInfo.leftBuyCount);
                if (isFree)
                {
                    if (isLimit)
                        count = (int)_m_giftPackInfo.leftBuyCount;
                    else
                        count = 99;
                }
                _m_wCounter.init(count);
            }

            //刷新消耗显示 
            _refreshUseItemCount();

            //设置限购显示
            ALUGUICommon.setGameObjEnable(wnd.hasBuyLimitList, isLimit);
            //如果有限购,设置限购文本
            if (isLimit)
                ALUGUICommon.setLabelTxt(wnd.limitBuyTxt, TextTranslate.instance.getLanguage(TransKeyConst.shop_buyLimitCout_num_num, _m_giftPackInfo.leftBuyCount, _m_giftPackInfo.buyCountMax));

            bool isEnough = GCommon.isItemEnough(_m_costShowItem, false);
            ALUGUICommon.setGameObjEnable(wnd.costEnoughShowGoList, isEnough);
            ALUGUICommon.setGameObjEnable(wnd.costNoEnoughShowGoList, !isEnough);
        }

        //刷新获得物品列表
        private void _refreshGainItemList()
        {
            if (_m_giftPackInfo == null || wnd == null)
                return;

            //设置获得物品
            if (_m_wGainItemContainer != null)
            {
                List<NPCommonCostItem> gainItemList = new List<NPCommonCostItem>();
                if (_m_giftPackInfo.gainItemList != null)
                {
                    for (int i = 0; i < _m_giftPackInfo.gainItemList.Count; i++)
                    {
                        NPCommonCostItem temp = new NPCommonCostItem(_m_giftPackInfo.gainItemList[i]);
                        temp.setCount(temp.count * _m_lUseCount);
                        gainItemList.Add(temp);
                    }
                }
                _m_wGainItemContainer.showWnd();
                _m_wGainItemContainer.showItemList(gainItemList);
            }
        }

        /// <summary>
        /// 刷新消耗显示
        /// </summary>
        private void _refreshUseItemCount()
        {
            if (_m_giftPackInfo == null)
                return;

            NPCommonCostItem _costItem = _m_giftPackInfo.getCostItemByDiscount();
            if (_costItem == null)
                return;

            //优先展示次数递增
            if (0 != _m_giftPackInfo.timePriceTypeId)
            {
                if (null == _m_costShowItem)
                    _m_costShowItem = GRefdataCoreMgr.instance.getTimesPriceCostItem(_m_giftPackInfo.timePriceTypeId, 1);
                long costCount = GRefdataCoreMgr.instance.getBatchTimePriceCostItem(_m_giftPackInfo.timePriceTypeId, (int)_m_giftPackInfo.hasBuyCount + 1, (int)_m_lUseCount);

                //打折
                if (_m_giftPackInfo.discount > 0)
                    costCount = costCount * _m_giftPackInfo.discount / 10000;

                _m_costShowItem.setCount(costCount);
            }
            else
            {
                //costItem是打折后的
                if (null == _m_costShowItem)
                    _m_costShowItem = new NPCommonCostItem(_costItem.item, 1);
                _m_costShowItem.setCount(_costItem.count * _m_lUseCount);
            }
            _m_costUseItem?.setItem(_m_costShowItem);
        }

        /// <summary>
        /// 商店定时刷新通知
        /// </summary>
        private void _shopAutoRefreshMsg()
        {
            //关闭弹窗
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GIFT_PACK_BATCH_BUY);
        }

        // 响应确定按钮点击事件
        private void _onConfirmBtnClick(GameObject _go)
        {
            if (_m_giftPackInfo == null || (!_m_giftPackInfo.isFree && (_m_costShowItem == null || !GCommon.isItemEnough(_m_costShowItem, true))))
                return;
            
            bool needConfirm = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.SHOP_BUY_ITEM_CONFIRM);
            if (needConfirm && !_m_giftPackInfo.isFree)
            {
                NPMesMgr.instance.showCostItemTogMes(_m_costShowItem,
                    (_toggle) =>
                    {
                        //记录今日不再提醒
                        if (_toggle)
                        {
                            AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.SHOP_BUY_ITEM_CONFIRM);
                        }

                        _m_confirmAction?.Invoke((int)_m_lUseCount);

                        //关闭弹窗
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GIFT_PACK_BATCH_BUY);
                    }, null,
                    TransKeyConst.shop_buyItem_title_none,
                    TextTranslate.instance.getLanguage(TransKeyConst.activity_crystalGiftPackBuyConfirm_str, _m_costShowItem.count, _m_costShowItem.getItemName(), (int)_m_lUseCount, _m_giftPackInfo.giftPackName));
            }
            else
            {
                _m_confirmAction?.Invoke((int)_m_lUseCount);
                //关闭弹窗
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GIFT_PACK_BATCH_BUY);
            }
        }

        // 响应关闭按钮点击事件
        private void _onCloseBtnClick(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GIFT_PACK_BATCH_BUY);
        }

        // 响应计数器更改事件
        private void _onCounterChanged(long _newCount)
        {
            _m_lUseCount = _newCount;
            _refreshUseItemCount();
            _refreshGainItemList();
        }

        //活动状态变化
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long instanceId = (long)_objects[1];
            if (instanceId == _m_lActivityInstanceId)
            {
                _ABaseActivityInfo info = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(instanceId);
                // 活动关闭或不在进行中，关闭界面
                if (info != null && !info.isPlaying)
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GIFT_PACK_BATCH_BUY);
            }
        }
    }
}
