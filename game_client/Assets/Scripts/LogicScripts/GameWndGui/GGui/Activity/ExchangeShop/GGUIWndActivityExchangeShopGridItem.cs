using ALPackage;
using Common.GuildObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用活动兑换商店物品列表item
    /// </summary>
    public class GGUIWndActivityExchangeShopGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoActivityExchangeShopGridItem>
    {
        //商店物品信息
        private ActivityShopItemInfo _m_shopItemInfo;
        //要购买的物品
        private NPGGUIWndCommonItem _m_wSellItem;
        //消耗的道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //品质底图
        private NPGGuiWndTexture _m_wQualityBg;

        public GGUIWndActivityExchangeShopGridItem(GGUIMonoActivityExchangeShopGridItem _wnd)
           : base(_wnd) 
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wSellItem?.hideWnd();
            _m_wCostItem?.hideWnd();
            _m_wQualityBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSellItem?.resetWnd();
            _m_wCostItem?.resetWnd();
            _m_wQualityBg?.discardTexture();
        }

        protected override void _resetGridItem()
        {
            _m_wSellItem?.resetWnd();
            _m_wCostItem?.resetWnd();
            _m_wQualityBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wSellItem?.discard();
            _m_wSellItem = null;
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            _m_wQualityBg?.discard();
            _m_wQualityBg = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnBuy, _onClickBuy);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSellItem != null)
                _m_wSellItem = new NPGGUIWndCommonItem(wnd.monoSellItem);

            if(wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            if (null != wnd.qualityImg)
                _m_wQualityBg = new NPGGuiWndTexture(wnd.qualityImg);

            ALUGUICommon.combineBtnClick(wnd.btnBuy, _onClickBuy);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(ActivityShopItemInfo _shopItemInfo)
        {
            _m_shopItemInfo = _shopItemInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (_m_shopItemInfo == null || _m_shopItemInfo.activityShopItemRef == null || _m_shopItemInfo.activityShopItemRef.item == null || wnd == null)
                return;

            //要购买的物品
            _m_wSellItem?.showWnd();
            _m_wSellItem?.setItem(_m_shopItemInfo.activityShopItemRef.item);

            //消耗的道具
            _m_wCostItem?.showWnd();
            _m_wCostItem?.setItem(_m_shopItemInfo.getCostItemByDiscount());

            //剩余购买次数
            ALUGUICommon.setLabelTxt(wnd.txtLeftBuyCount, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, _m_shopItemInfo.leftBuyCount, _m_shopItemInfo.activityShopItemRef?.buy_num));

            //推荐标识
            ALUGUICommon.setGameObjEnable(wnd.goRecommend, _m_shopItemInfo.activityShopItemRef.is_recommend);

            //商品品质框
            NPQualityExtRefObj extRef = GCommon.getQualityExtRefObj(_m_shopItemInfo.activityShopItemRef.item.getItemType(), _m_shopItemInfo.activityShopItemRef.item.subId);
            if (_m_wQualityBg != null && extRef != null)
            {
                _m_wQualityBg.showWnd();
                _m_wQualityBg.setTexture(extRef.shop_item_bg);
            }

            //售罄显隐
            ALUGUICommon.setGameObjEnable(wnd.goSellOutShowList, _m_shopItemInfo.isSellOut);
            ALUGUICommon.setGameObjEnable(wnd.goSellOutHideList, !_m_shopItemInfo.isSellOut);
        }

        //点击购买
        private void _onClickBuy(GameObject _go)
        {
            if (_m_shopItemInfo == null || _m_shopItemInfo.activityShopItemRef == null)
                return;

            //是否售罄
            if (_m_shopItemInfo.isSellOut)
            {
                //售罄提示
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.activity_itemSellOutTip_none);
                return;
            }

            //打开批量购买弹窗
            GCommon.showCommonBuyItem(
                _m_shopItemInfo.activityShopItemRef.item,
                _m_shopItemInfo.getCostItemByDiscount(),
                (int) _m_shopItemInfo.leftBuyCount,
                (int) _m_shopItemInfo.activityShopItemRef.buy_num,
                _m_shopItemInfo.activityShopItemRef.times_price_type_id,
                (int) _m_shopItemInfo.hasBuyCount + 1, 0, 0, TransKeyConst.activity_activityShopBuyDailyLimit_num, (count) =>
                {
                    if (count <= 0)
                        return;

                    //请求购买
                    NPPlayer.instance.commonActivityComp.reqBuyActivityShopItem(
                        _m_shopItemInfo.activityInstanceId,
                        _m_shopItemInfo.activityShopItemRef.activity_shop_id,
                        _m_shopItemInfo.activityShopItemId,
                        (int)count);
                });
        }
    }
}
