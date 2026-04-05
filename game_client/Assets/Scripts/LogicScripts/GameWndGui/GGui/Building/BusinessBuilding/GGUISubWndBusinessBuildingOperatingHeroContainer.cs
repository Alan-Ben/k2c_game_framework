using System;
using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndBusinessBuildingOperatingHeroContainer : _AGGUISubWndCommonContainer<GGUIMonoBusinessBuildingOperatingHeroContainerItem, GGUIMonoBusinessBuildingOperatingHeroContainer, GGUISubWndBusinessBuildingOperatingHeroContainerItem>
    {
        private readonly Action<GGUISubWndBusinessBuildingOperatingHeroContainerItem> _m_onItemClick;

        private long _m_employeeNum;
        private BusinessBuildingRefObj _m_buildingRef;
        private List<HeroInfo> _m_heroInfoList;
        private bool _m_showRedTip;
        
        
        public GGUISubWndBusinessBuildingOperatingHeroContainer(GGUIMonoBusinessBuildingOperatingHeroContainer _containerMono, Action<GGUISubWndBusinessBuildingOperatingHeroContainerItem> _onItemClick) 
            : base(_containerMono)
        {
            _m_onItemClick = _onItemClick;
            
            initWnd();
        }
        
        protected override void _refreshItemWnd(GGUISubWndBusinessBuildingOperatingHeroContainerItem _itemWnd, int _index)
        {
            if (_m_buildingRef?.hero_slot_employee_num_list == null)
                return;
            
            if (_index < 0 || _index > _m_buildingRef.hero_slot_employee_num_list.Count - 1)
                return;

            long slotEmployeeNum = _m_buildingRef.hero_slot_employee_num_list[_index];
            long lastSlotEmployeeNum = _index > 0 ? _m_buildingRef.hero_slot_employee_num_list[_index - 1] : 0;
            _itemWnd.refreshWnd(_m_employeeNum >= slotEmployeeNum, _m_employeeNum >= lastSlotEmployeeNum, slotEmployeeNum, _m_buildingRef, _m_heroInfoList.SafeGet(_index), _m_showRedTip);
        }
        protected override GGUISubWndBusinessBuildingOperatingHeroContainerItem _createItemWnd(GGUIMonoBusinessBuildingOperatingHeroContainerItem _itemMono)
        {
            return new GGUISubWndBusinessBuildingOperatingHeroContainerItem(_itemMono, _onItemClick);
        }
        
        
        private void _onItemClick(GGUISubWndBusinessBuildingOperatingHeroContainerItem _item)
        {
            _m_onItemClick?.Invoke(_item);
        }

        public void refreshWnd(long _employeeNum, BusinessBuildingRefObj _buildingRef, List<HeroInfo> _heroInfoList, bool _showRedTip = false)
        {
            _m_employeeNum = _employeeNum;
            _m_buildingRef = _buildingRef;
            _m_heroInfoList = _heroInfoList;
            _m_showRedTip = _showRedTip;
            refreshWnd(_m_buildingRef?.hero_slot_employee_num_list?.Count ?? 0);
        }
    }
}