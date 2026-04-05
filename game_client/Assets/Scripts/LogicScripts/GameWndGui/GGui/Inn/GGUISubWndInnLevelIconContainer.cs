namespace GOE
{
    public class GGUISubWndInnLevelIconContainer : _AGGUISubWndCommonContainer<GGUIMonoInnLevelIconContainerItem, GGUIMonoInnLevelIconContainer, GGUISubWndInnLevelIconContainerItem>
    {
        private InnLevelRefObj _m_levelRef;


        public GGUISubWndInnLevelIconContainer(GGUIMonoInnLevelIconContainer _containerMono)
            : base(_containerMono)
        {
            initWnd();
        }


        protected override GGUISubWndInnLevelIconContainerItem _createItemWnd(GGUIMonoInnLevelIconContainerItem _itemMono)
        {
            return new GGUISubWndInnLevelIconContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndInnLevelIconContainerItem _itemWnd, int _index)
        {
            _itemWnd.refreshWnd(_m_levelRef);
        }


        public void refreshWnd(InnLevelRefObj _levelRef)
        {
            _m_levelRef = _levelRef;
            refreshWnd(_m_levelRef?.star_num ?? 0);
        }
    }
}