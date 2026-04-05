using System;
using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndTreasureHuntCatalogTabContainer : _AGGUISubWndCommonContainer<GGUIMonoTreasureHuntCatalogTab, GGUIMonoTreasureHuntCatalogTabContainer, GGUIWndTreasureHuntCatalogTab>
    {
        private List<TreasureHuntCatalogTabRefObj> _m_tabRefObjList;
        private int _m_iSelectedIndex; //选中的item下标

        public GGUIWndTreasureHuntCatalogTabContainer(GGUIMonoTreasureHuntCatalogTabContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<GGUIWndTreasureHuntCatalogTab> onTabClick;

        protected override void _onDiscard()
        {
            onTabClick = null;

            base._onDiscard();
        }

        protected override GGUIWndTreasureHuntCatalogTab _createItemWnd(GGUIMonoTreasureHuntCatalogTab _itemMono)
        {
            GGUIWndTreasureHuntCatalogTab tab = new GGUIWndTreasureHuntCatalogTab(_itemMono);
            tab.onTabClick += _onTabClick;
            return tab;
        }

        protected override void _discardItem(GGUIWndTreasureHuntCatalogTab _itemWnd)
        {
            if (_itemWnd != null)
                _itemWnd.onTabClick -= _onTabClick;
            
            base._discardItem(_itemWnd);
        }

        protected override void _refreshItemWnd(GGUIWndTreasureHuntCatalogTab _itemWnd, int _index)
        {
            if (_m_tabRefObjList == null || _index < 0 || _index >= _m_tabRefObjList.Count)
                return;

            _itemWnd.setData(_index, _m_tabRefObjList[_index]);
            _itemWnd.setSelected(_m_iSelectedIndex == _index);
        }

        public void setData(List<TreasureHuntCatalogTabRefObj> _tabRefObjList, int _selectedIndex = 0)
        {
            _m_tabRefObjList = _tabRefObjList;
            _m_iSelectedIndex = _selectedIndex;

            refreshWnd(_m_tabRefObjList?.Count ?? 0);
        }

        public void setSelectIndex(int _index)
        {
            int preSelectIndex = _m_iSelectedIndex;
            _m_iSelectedIndex = _index;

            refreshItem(preSelectIndex);
            refreshItem(_m_iSelectedIndex);
        }

        private void _onTabClick(GGUIWndTreasureHuntCatalogTab _tab)
        {
            onTabClick?.Invoke(_tab);
        }
    }
}