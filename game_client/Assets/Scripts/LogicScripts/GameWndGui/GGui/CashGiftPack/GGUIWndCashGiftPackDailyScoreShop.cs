using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 现金礼包每日礼包积分兑换商店窗口
    /// </summary>
    public class GGUIWndCashGiftPackDailyScoreShop : _ANPGGUIBasicWnd<GGUIMonoCashGiftPackDailyScoreShop>
    {
        private static GGUIWndCashGiftPackDailyScoreShop _g_instance;
        public static GGUIWndCashGiftPackDailyScoreShop instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndCashGiftPackDailyScoreShop();
                return _g_instance;
            }
        }

        //拥有的积分
        private NPGGUIWndCommonItem _m_wCommonItem;
        //商品列表
        private GGUIWndShopNormalPageGrid _m_wShopGrid;

        public GGUIWndCashGiftPackDailyScoreShop() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoCashGiftPackDailyScoreShop.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoCashGiftPackDailyScoreShop.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SHOP_CHG, _shopChgMsg);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SHOP_CHG, _shopChgMsg);
            _m_wCommonItem?.hideWnd();
            _m_wShopGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCommonItem?.resetWnd();
            _m_wShopGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wCommonItem?.discard();
            _m_wCommonItem = null;
            _m_wShopGrid?.discard();
            _m_wShopGrid = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCommonItem != null)
                _m_wCommonItem = new NPGGUIWndCommonItem(wnd.monoCommonItem);

            if (wnd.monoShopGrid != null)
                _m_wShopGrid = new GGUIWndShopNormalPageGrid(wnd.monoShopGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            NPPlayerShop shop = NPPlayer.instance.shopComp.getShop(wnd.shopRefId);
            if (null == shop)
                return;

            if(wnd.costCurrencyType != ECurrency.NONE)
            {
                NPCommonCostItem currencyItem = new NPCommonCostItem(ENPItemType.CURRENCY,(long)wnd.costCurrencyType,GCommon.getItemCount(ENPItemType.CURRENCY, (long)wnd.costCurrencyType));
                _m_wCommonItem?.showWnd();
                _m_wCommonItem?.setItem(currencyItem);
            }

            _m_wShopGrid?.showWnd();
            _m_wShopGrid?.refresh(shop);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CASH_GIFT_PACK_DAILY_SCORE_SHOP);
        }

        /// <summary>
        /// 商店变动
        /// </summary>
        private void _shopChgMsg(params object[] _objs)
        {
            if (null == wnd || null == _objs || _objs.Length == 0)
                return;

            long shopRefId = (long)_objs[0];
            if (shopRefId != wnd.shopRefId)
                return;

            _refreshWnd();
        }
    }
}
