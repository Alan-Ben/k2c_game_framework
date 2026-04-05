using System;
using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndTravelConsortBarEventChooseCostItemContainer : _AGGUISubWndCommonContainer<GGUIMonoTravelConsortBarEventChooseCostItem, GGUIMonoTravelConsortBarEventChooseCostItemContainer, GGUIWndTravelConsortBarEventChooseCostItem>
    {
        private List<TravelEventConsortBarCostRefObj> _m_lCostRefObjList;//消耗数据列表
        private ETravelConsortUnlockStat _m_eSelectConsortUnlockStat;//选中妃子的的解锁状态
        public GGUIWndTravelConsortBarEventChooseCostItemContainer(GGUIMonoTravelConsortBarEventChooseCostItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<GGUIWndTravelConsortBarEventChooseCostItem> onSelectItem;

        protected override void _onDiscard()
        {
            onSelectItem = null;
        }

        protected override GGUIWndTravelConsortBarEventChooseCostItem _createItemWnd(GGUIMonoTravelConsortBarEventChooseCostItem _itemMono)
        {
            GGUIWndTravelConsortBarEventChooseCostItem itemWnd = new GGUIWndTravelConsortBarEventChooseCostItem(_itemMono);
            itemWnd.onSelectItem += _onItemSelect;
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndTravelConsortBarEventChooseCostItem _itemWnd, int _index)
        {
            if(_m_lCostRefObjList == null || _index < 0 || _index >= _m_lCostRefObjList.Count)
                return;
            
            _itemWnd.setData(_m_lCostRefObjList[_index], _m_eSelectConsortUnlockStat);
        }

        public void setData(List<TravelEventConsortBarCostRefObj> _costRefObjList, ETravelConsortUnlockStat _selectConsortUnlockStat)
        {
            _m_lCostRefObjList = _costRefObjList;
            _m_eSelectConsortUnlockStat = _selectConsortUnlockStat;
            
            refreshWnd(_m_lCostRefObjList?.Count ?? 0);
        }

        private void _onItemSelect(GGUIWndTravelConsortBarEventChooseCostItem _itemWnd)
        {
            onSelectItem?.Invoke(_itemWnd);
        }
    }
}