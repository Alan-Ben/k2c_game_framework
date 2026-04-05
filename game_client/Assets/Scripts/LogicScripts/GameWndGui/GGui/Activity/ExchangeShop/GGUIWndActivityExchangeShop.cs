using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用活动兑换商店界面
    /// </summary>
    public class GGUIWndActivityExchangeShop : _ANPGGUIBasicWnd<GGUIMonoActivityExchangeShop>
    {
        private static GGUIWndActivityExchangeShop _g_instance;
        public static GGUIWndActivityExchangeShop instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndActivityExchangeShop();
                return _g_instance;
            }
        }

        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //当前兑换券
        private NPGGUIWndCommonItem _m_wActivityCurrencyItem;
        //商品列表
        private GGUIWndActivityExchangeShopGrid _m_wShopGrid;
        //定时任务
        private ALCommonEnableTaskController _m_iCheckTask;
        //商品列表信息
        private List<ActivityShopItemInfo> _m_lShopItemInfoList;

        public GGUIWndActivityExchangeShop() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoActivityExchangeShop.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoActivityExchangeShop.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_SHOP_REFRESH, _onShopRefresh);
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_SHOP_BUY_RECORD_CHG, _onActivityShopBuyRecordChg);
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CURRENCY_CHG, _onActivityCurrencyChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_SHOP_REFRESH, _onShopRefresh);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_SHOP_BUY_RECORD_CHG, _onActivityShopBuyRecordChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CURRENCY_CHG, _onActivityCurrencyChg);

            _m_wActivityCurrencyItem?.hideWnd();
            _m_wShopGrid?.hideWnd();
            _m_iCheckTask.setDisable();
            _m_lShopItemInfoList?.Clear();
            _m_lShopItemInfoList = null;
        }

        protected override void _onReset()
        {
            _m_wActivityCurrencyItem?.resetWnd();
            _m_wShopGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wActivityCurrencyItem?.discard();
            _m_wActivityCurrencyItem = null;

            _m_wShopGrid?.discard();
            _m_wShopGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoShopGrid != null)
                _m_wShopGrid = new GGUIWndActivityExchangeShopGrid(wnd.monoShopGrid);

            if (wnd.monoCommonItem != null)
                _m_wActivityCurrencyItem = new NPGGUIWndCommonItem(wnd.monoCommonItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_activityId"></param>
        public void setInfo(long _activityId)
        {
            if (wnd == null)
                return;

            //取最后一个活动
            _m_activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_activityId);

            //刷新窗口
            _refreshWnd();
            //设置定时检查任务
            _m_iCheckTask.setDisable();
            _m_iCheckTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_checkNeedRefresh, 1.0f);
        }

        //刷新界面
        private void _refreshWnd()
        {
            _refreshList();
            _refreshActivityCurrency();
        }

        //刷新商品列表
        private void _refreshList()
        {
            if (_m_activityInfo == null)
                return;

            if (_m_lShopItemInfoList == null)
                _m_lShopItemInfoList = new List<ActivityShopItemInfo>();
            _m_lShopItemInfoList.Clear();

            //商品列表
            _m_activityInfo.exchangeActivityShopInfo?.getActivityShopItemList(_m_lShopItemInfoList);
            _m_lShopItemInfoList?.Sort(_sortShopItemList);
            _m_wShopGrid?.showWnd();
            _m_wShopGrid?.setShowData(_m_lShopItemInfoList);
        }

        //刷新兑换券
        private void _refreshActivityCurrency()
        {
            if (_m_activityInfo == null || _m_activityInfo.exchangeActivityShopInfo == null)
                return;

            //活动货币id
            long activityCurrencyId = _m_activityInfo.exchangeActivityShopInfo.activityCurrencyId;

            //当前兑换券
            _m_wActivityCurrencyItem?.showWnd();
            _m_wActivityCurrencyItem?.setItem(new NPCommonCostItem(ENPItemType.ACTIVITY_CURRENCY, activityCurrencyId, GCommon.getItemCount(ENPItemType.ACTIVITY_CURRENCY, activityCurrencyId)));
        }

        //检查是否需要更新商店
        private void _checkNeedRefresh()
        {
            if (_m_activityInfo == null || _m_activityInfo.exchangeActivityShopInfo == null)
                return;

            long serverTimeMs = FpsAndPingMgr.instance.serverTimeTag;

            //如果超过刷新时间，则请求刷新
            if (_m_activityInfo.exchangeActivityShopInfo.nextRefreshTimeMs > 0 &&
                _m_activityInfo.exchangeActivityShopInfo.nextRefreshTimeMs <= serverTimeMs)
            {
                _m_iCheckTask.setDisable();
                NPPlayer.instance.commonActivityComp.reqRefreshActivityShop(_m_activityInfo.instanceId, _m_activityInfo.exchangeActivityShopInfo.activityShopId);
            }
        }

        //排序，未售罄>已售罄，推荐>未推荐，排序id从小到大
        private int _sortShopItemList(ActivityShopItemInfo _a, ActivityShopItemInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            int comp = _a.isSellOut.CompareTo(_b.isSellOut);
            if (comp != 0)
                return comp;

            comp = _a.isRecommend.CompareTo(_b.isRecommend);
            if (comp != 0) 
                return -comp;

            if (_a.activityShopItemRef == null || _b.activityShopItemRef == null)
                return 0;
            else
                return _a.activityShopItemRef.sort_id.CompareTo(_b.activityShopItemRef.sort_id);
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ACTIVITY_EXCHANGE_SHOP);
        }

        #region 消息事件

        //商店刷新
        private void _onShopRefresh(params object[] _objects)
        {
            if(_objects == null || _objects.Length == 0)
                return;

            long activityInstanceId = (long)_objects[0];
            if (_m_activityInfo == null || _m_activityInfo.instanceId != activityInstanceId)
                return;

            //刷新窗口
            _refreshWnd();
            //设置定时检查任务
            _m_iCheckTask.setDisable();
            _m_iCheckTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_checkNeedRefresh, 1.0f);
        }

        //商店购买记录变更
        private void _onActivityShopBuyRecordChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long activityInstanceId = (long)_objects[0];
            if (_m_activityInfo == null || _m_activityInfo.instanceId != activityInstanceId)
                return;

            //刷新窗口
            _refreshWnd();
        }

        //兑换券变更
        private void _onActivityCurrencyChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long activityCurrencyId = (long)_objects[0];
            if (_m_activityInfo == null || _m_activityInfo.exchangeActivityShopInfo == null || _m_activityInfo.exchangeActivityShopInfo.activityCurrencyId != activityCurrencyId)
                return;

            //刷新兑换券
            _refreshActivityCurrency();
        }

        #endregion
    }
}