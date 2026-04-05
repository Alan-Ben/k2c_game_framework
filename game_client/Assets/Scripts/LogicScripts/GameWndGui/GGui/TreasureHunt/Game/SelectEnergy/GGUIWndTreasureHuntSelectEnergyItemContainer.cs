using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 太空寻宝选择能源item容器
    /// </summary>
    public class GGUIWndTreasureHuntSelectEnergyItemContainer : _AGGUISubWndCommonContainer<GGUIMonoTreasureHuntSelectEnergyItem, GGUIMonoTreasureHuntSelectEnergyItemContainer, GGUIWndTreasureHuntSelectEnergyItem>
    {
        private List<NPCommonItem> _m_lItemDataList;
        private NPCommonItem _m_iSelectedItemData;//选中的道具
        private NPCommonItem _m_iUsingItemData;//正在使用的道具
        
        public GGUIWndTreasureHuntSelectEnergyItemContainer(GGUIMonoTreasureHuntSelectEnergyItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<NPCommonItem> onClickEnergyItem;//当点击某个能源道具

        protected override void _onDiscard()
        {
            onClickEnergyItem = null;
                
            base._onDiscard();
        }
        
        protected override GGUIWndTreasureHuntSelectEnergyItem _createItemWnd(GGUIMonoTreasureHuntSelectEnergyItem _itemMono)
        {
            GGUIWndTreasureHuntSelectEnergyItem itemWnd = new GGUIWndTreasureHuntSelectEnergyItem(_itemMono);
            itemWnd.onItemClick += _onItemClick;

            return itemWnd;
        }

        protected override void _discardItem(GGUIWndTreasureHuntSelectEnergyItem _itemWnd)
        {
            if(_itemWnd != null)
                _itemWnd.onItemClick -= _onItemClick;

            base._discardItem(_itemWnd);
        }
        
        protected override void _refreshItemWnd(GGUIWndTreasureHuntSelectEnergyItem _itemWnd, int _index)
        {
            if(_m_lItemDataList == null || _index < 0 || _index >= _m_lItemDataList.Count)
                return;
            
            NPCommonItem itemData = _m_lItemDataList.SafeGet(_index);
            _itemWnd.setData(itemData);
            
            // 设置选中状态
            _itemWnd.setSelected(itemData == _m_iSelectedItemData);
            
            // 设置使用状态
            _itemWnd.setUsing(itemData == _m_iUsingItemData);
        }

        public void setData(List<NPCommonItem> _itemDataList, NPCommonItem _selectedItemData = null, NPCommonItem _usingItemData = null)
        {
            _m_lItemDataList = _itemDataList;
            _m_iSelectedItemData = _selectedItemData;
            _m_iUsingItemData = _usingItemData;

            int itemCount = _m_lItemDataList?.Count ?? 0;
            refreshWnd(itemCount);
        }

        public void setSelectedItem(NPCommonItem _selectedItemData)
        {
            if(_m_iSelectedItemData == _selectedItemData)
                return;
                
            NPCommonItem preSelectedItem = _m_iSelectedItemData;
            _m_iSelectedItemData = _selectedItemData;
            
            // 刷新之前选中的item
            _refreshItemByItemData(preSelectedItem);
            // 刷新当前选中的item
            _refreshItemByItemData(_m_iSelectedItemData);
        }

        public void setUsingItem(NPCommonItem _usingItemData)
        {
            if(_m_iUsingItemData == _usingItemData)
                return;
                
            NPCommonItem preUsingItem = _m_iUsingItemData;
            _m_iUsingItemData = _usingItemData;
            
            // 刷新之前使用的item
            _refreshItemByItemData(preUsingItem);
            // 刷新当前使用的item
            _refreshItemByItemData(_m_iUsingItemData);
        }

        /// <summary>
        /// 根据item数据刷新对应的itemWnd
        /// </summary>
        /// <param name="_itemData"></param>
        private void _refreshItemByItemData(NPCommonItem _itemData)
        {
            if(_itemData == null || _m_lItemDataList == null)
                return;

            int index = _m_lItemDataList.IndexOf(_itemData);
            refreshItem(index);
        }
        
        private void _onItemClick(GGUIWndTreasureHuntSelectEnergyItem _itemWnd)
        {
            if(_itemWnd == null || _itemWnd.itemData == null)
                return;
                
            onClickEnergyItem?.Invoke(_itemWnd.itemData);
        }
    }
}