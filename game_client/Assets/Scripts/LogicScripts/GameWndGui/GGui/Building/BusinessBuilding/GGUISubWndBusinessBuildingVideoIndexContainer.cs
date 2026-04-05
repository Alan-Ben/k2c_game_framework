using System;

namespace GOE
{
    public class GGUISubWndBusinessBuildingVideoIndexContainer : _AGGUISubWndCommonContainer<GGUIMonoBusinessBuildingVideoIndexContainerItem, GGUIMonoBusinessBuildingVideoIndexContainer, GGUISubWndBusinessBuildingVideoIndexContainerItem>
    {
        private readonly Action<int> _m_onItemClick;
        private int _m_selectedIndex;
        
        
        public GGUISubWndBusinessBuildingVideoIndexContainer(GGUIMonoBusinessBuildingVideoIndexContainer _containerMono, Action<int> _onItemClick) : base(_containerMono)
        {
            _m_onItemClick = _onItemClick;
            initWnd();
        }
        

        protected override GGUISubWndBusinessBuildingVideoIndexContainerItem _createItemWnd(GGUIMonoBusinessBuildingVideoIndexContainerItem _itemMono)
        {
            return new GGUISubWndBusinessBuildingVideoIndexContainerItem(_itemMono, _onItemClick);
        }
        protected override void _refreshItemWnd(GGUISubWndBusinessBuildingVideoIndexContainerItem _itemWnd, int _index)
        {
            _itemWnd.refreshWnd(_index, _index == _m_selectedIndex);
        }


        public void refreshWnd(int _indexCount, int _selectedIndex)
        {
            _m_selectedIndex = _selectedIndex;
            refreshWnd(_indexCount);
        }
        
        
        private void _onItemClick(int _index)
        {
            _m_onItemClick?.Invoke(_index);
        }
    }
}