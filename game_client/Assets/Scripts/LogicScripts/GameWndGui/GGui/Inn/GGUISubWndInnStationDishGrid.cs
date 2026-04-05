namespace GOE
{
    public class GGUISubWndInnStationDishGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoInnStationDishGridItem, GGUIMonoInnStationDishGrid, GGUISubWndInnStationDishGridItem>
    {
        private ReadOnlyList<InnDishInfo> _m_dishList;


        public GGUISubWndInnStationDishGrid(GGUIMonoInnStationDishGrid _wnd)
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
        protected override GGUISubWndInnStationDishGridItem _createItemWnd(GGUIMonoInnStationDishGridItem _itemMono)
        {
            return new GGUISubWndInnStationDishGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndInnStationDishGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null)
                return;

            if (_itemIdx < 0 || _itemIdx >= _m_dishList.Count)
                return;

            InnDishInfo dishInfo = _m_dishList[_itemIdx];
            if (dishInfo == null)
                return;

            _itemMono.refreshWnd(dishInfo);
        }


        public void refreshWnd(ReadOnlyList<InnDishInfo> _list)
        {
            _m_dishList = _list;
            setItemCount(_m_dishList.Count);
        }
    }
}
