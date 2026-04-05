using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndBusinessBuildingRnDPageDevelopContainer : _AGGUISubWndCommonContainer<GGUIMonoBusinessBuildingRnDPageDevelopContainerItem, GGUIMonoBusinessBuildingRnDPageDevelopContainer, GGUISubWndBusinessBuildingRnDPageDevelopContainerItem>
    {
        private List<BusinessBuildingDevelopRefObj> _m_developList;
        private BusinessBuildingInfo _m_buildingInfo;
        
        
        public GGUISubWndBusinessBuildingRnDPageDevelopContainer(GGUIMonoBusinessBuildingRnDPageDevelopContainer _containerMono) 
            : base(_containerMono)
        {
            initWnd();
        }
        

        protected override GGUISubWndBusinessBuildingRnDPageDevelopContainerItem _createItemWnd(GGUIMonoBusinessBuildingRnDPageDevelopContainerItem _itemMono)
        {
            return new GGUISubWndBusinessBuildingRnDPageDevelopContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndBusinessBuildingRnDPageDevelopContainerItem _itemWnd, int _index)
        {
            BusinessBuildingDevelopRefObj refObj = _m_developList.SafeGet(_index);
            if (refObj == null)
                return;
            
            _itemWnd.refreshWnd(refObj, _m_buildingInfo, _index);
        }


        public void refreshWnd(List<BusinessBuildingDevelopRefObj> _developList, BusinessBuildingInfo _buildingInfo)
        {
            _m_developList = _developList;
            _m_buildingInfo = _buildingInfo;
            refreshWnd(_m_developList?.Count ?? 0);
        }
    }
}