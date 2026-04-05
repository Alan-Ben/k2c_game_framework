using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainer : _AGGUISubWndCommonContainer<GGUIMonoBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem, GGUIMonoBusinessBuildingSelectOperatingHeroConfirmHeroContainer, GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem>
    {
        private List<HeroInfo> _m_heroList;
        private BusinessBuildingRefObj _m_newBuildingRef;
        
        
        public GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainer(GGUIMonoBusinessBuildingSelectOperatingHeroConfirmHeroContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }
        

        protected override GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem _createItemWnd(GGUIMonoBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem _itemMono)
        {
            return new GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem _itemWnd, int _index)
        {
            if (_m_heroList == null)
                return;

            if (_index < 0 || _index > _m_heroList.Count - 1)
                return;

            _itemWnd.refreshWnd(_m_heroList[_index], _m_newBuildingRef);
        }
        

        public void refreshWnd(List<HeroInfo> _heroList, BusinessBuildingRefObj _newBuildingRef)
        {
            _m_heroList = _heroList;
            _m_newBuildingRef = _newBuildingRef;
            refreshWnd(_m_heroList?.Count ?? 0);
        }
    }
}