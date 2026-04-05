using System.Collections.Generic;
using Common.InnObj;

namespace GOE
{
    public class GGUISubWndInnGetCashRegisterRewardDetailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoInnGetCashRegisterRewardDetailGridItem, GGUIMonoInnGetCashRegisterRewardDetailGrid, GGUISubWndInnGetCashRegisterRewardDetailGridItem>
    {
        private List<Inn_DishSettleInfo> _m_serverDetailList;
        
        
        public GGUISubWndInnGetCashRegisterRewardDetailGrid(GGUIMonoInnGetCashRegisterRewardDetailGrid _wnd) 
            : base(_wnd)
        {
            initWnd();
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
        protected override GGUISubWndInnGetCashRegisterRewardDetailGridItem _createItemWnd(GGUIMonoInnGetCashRegisterRewardDetailGridItem _itemMono)
        {
            return new GGUISubWndInnGetCashRegisterRewardDetailGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndInnGetCashRegisterRewardDetailGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null || _m_serverDetailList == null || _itemIdx < 0 || _itemIdx >= _m_serverDetailList.Count)
                return;

            Inn_DishSettleInfo info = _m_serverDetailList[_itemIdx];
            _itemMono.refreshWnd(info);
        }


        public void refreshWnd(List<Inn_DishSettleInfo> _serverDetailList)
        {
            _m_serverDetailList = _serverDetailList;
            setItemCount(_m_serverDetailList?.Count ?? 0);
        }
    }
}