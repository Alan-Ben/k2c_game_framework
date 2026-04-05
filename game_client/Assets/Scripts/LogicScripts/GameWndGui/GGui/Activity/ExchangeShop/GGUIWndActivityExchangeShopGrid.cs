using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用活动兑换商店物品列表
    /// </summary>
    public class GGUIWndActivityExchangeShopGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoActivityExchangeShopGridItem, GGUIMonoActivityExchangeShopGrid, GGUIWndActivityExchangeShopGridItem>
    {
        //信息列表
        private List<ActivityShopItemInfo> _m_lInfoList;

        public GGUIWndActivityExchangeShopGrid(GGUIMonoActivityExchangeShopGrid _wnd) : base(_wnd)
        {
            _m_lInfoList = new List<ActivityShopItemInfo>();
            initWnd();
        }

        protected override GGUIWndActivityExchangeShopGridItem _createItemWnd(GGUIMonoActivityExchangeShopGridItem _itemMono)
        {
            GGUIWndActivityExchangeShopGridItem item = new GGUIWndActivityExchangeShopGridItem(_itemMono);
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndActivityExchangeShopGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx < 0 || _itemIdx >= _m_lInfoList.Count)
                return;

            _itemWnd?.setInfo(_m_lInfoList[_itemIdx]);
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        public void setShowData(List<ActivityShopItemInfo> _shopInfoList)
        {
            if (wnd == null)
                return;

            _m_lInfoList.Clear();
            if(_shopInfoList != null)
                _m_lInfoList.AddRange(_shopInfoList);

            setItemCount(_m_lInfoList.Count);
            ALUGUICommon.setGameObjEnable(wnd?.goEmptyShowList, _m_lInfoList?.Count <= 0);
        }
    }
}
