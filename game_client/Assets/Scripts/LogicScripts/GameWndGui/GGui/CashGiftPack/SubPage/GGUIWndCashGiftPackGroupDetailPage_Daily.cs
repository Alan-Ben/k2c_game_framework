using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 礼包组详情页面-每日
    /// </summary>
    public class GGUIWndCashGiftPackGroupDetailPage_Daily : _AWndCashGiftPackGroupDetailPage<GGUIMonoCashGiftPackGroupDetailPage_Daily>
    {
        //礼包商品列表
        private GGUIWndCashGiftPackGroupGoodsContainer _m_wGoodsContainer;

        public GGUIWndCashGiftPackGroupDetailPage_Daily(long _uiResId, Transform _parent) : base(_uiResId, _parent)
        {
        }

        protected override void _onShowWndEx()
        {
        }
        
        protected override void _onHideWndEx()
        {
            _m_wGoodsContainer?.hideWnd();
        }
        
        protected override void _onResetEx()
        {
            _m_wGoodsContainer?.resetWnd();
        }
        
        protected override void _onDiscardEx()
        {
            _m_wGoodsContainer?.discard();
            _m_wGoodsContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnShop, _onClickShop);
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;

            if (wnd.monoGoodsContainer != null)
                _m_wGoodsContainer = new GGUIWndCashGiftPackGroupGoodsContainer(wnd.monoGoodsContainer);

            ALUGUICommon.combineBtnClick(wnd.btnShop, _onClickShop);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        protected override void _refreshWndEx()
        {
            _refreshGoodsContainer();
        }

        /// <summary>
        /// 商品列表移动到顶部
        /// </summary>
        protected override void _dealGoodsContainerMoveToTop()
        {
            _m_wGoodsContainer?.moveToTop();
        }

        //刷新商品列表
        private void _refreshGoodsContainer()
        {
            if (wnd == null || _m_giftPackGroupRefObj == null)
                return;

            _m_wGoodsContainer?.showWnd();
            _m_wGoodsContainer?.showItemList(_m_giftPackGroupRefObj.gift_pack_id_list);
        }

        // 点击积分商店按钮
        private void _onClickShop(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndCashGiftPackDailyScoreShop.instance,
                GGUIWndCashGiftPackDailyScoreShop.instance.showWnd, UINodeTagConst.C_CASH_GIFT_PACK_DAILY_SCORE_SHOP);
        }

        /// <summary>
        /// 消息通知礼包变化
        /// </summary>
        /// <param name="_objects"></param>
        protected override void _onGiftPackChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_giftPackGroupRefObj == null || _m_giftPackGroupRefObj.gift_pack_id_list == null)
                return;

            long giftPackId = (long)_objects[0];
            if (_m_giftPackGroupRefObj.gift_pack_id_list.Contains(giftPackId))
                _refreshGoodsContainer();
        }

        /// <summary>
        /// 消息通知礼包列表刷新
        /// </summary>
        /// <param name="_objects"></param>
        protected override void _onGiftPackListRefresh(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_giftPackGroupRefObj == null || _m_giftPackGroupRefObj.gift_pack_id_list == null)
                return;


            List<long> giftPackIdList = (List<long>)_objects[0];
            if (giftPackIdList == null || giftPackIdList.Count == 0)
                return;

            //如果当前礼包组的礼包列表中有变化，则刷新商品列表
            foreach (long giftPackId in giftPackIdList)
            {
                if (_m_giftPackGroupRefObj.gift_pack_id_list.Contains(giftPackId))
                {
                    _refreshGoodsContainer();
                    _m_wGoodsContainer?.moveToTop();
                    break;
                }
            }
        }
    }
}
