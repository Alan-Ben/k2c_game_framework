using System;
using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndTreasureHuntLabSelectContainer : _AGGUISubWndCommonContainer<GGUIMonoTreasureHuntLabSelectContainerItem, GGUIMonoTreasureHuntLabSelectContainer, GGUIWndTreasureHuntLabSelectContainerItem>
    {
        private List<TreasureHuntLabRefObj> _m_lLabRefObjList;
        private TreasureHuntLabRefObj _m_selectedLabRefObj;
        
        public GGUIWndTreasureHuntLabSelectContainer(GGUIMonoTreasureHuntLabSelectContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<TreasureHuntLabRefObj> onLabSelect;

        protected override void _onDiscard()
        {
            onLabSelect = null;
            
            base._onDiscard();
        }

        protected override void _discardItem(GGUIWndTreasureHuntLabSelectContainerItem _itemWnd)
        {
            _itemWnd.onSelect -= _onLabSelect;
            
            base._discardItem(_itemWnd);
        }

        protected override GGUIWndTreasureHuntLabSelectContainerItem _createItemWnd(GGUIMonoTreasureHuntLabSelectContainerItem _itemMono)
        {
            GGUIWndTreasureHuntLabSelectContainerItem itemWnd = new GGUIWndTreasureHuntLabSelectContainerItem(_itemMono);
            itemWnd.onSelect += _onLabSelect;
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndTreasureHuntLabSelectContainerItem _itemWnd, int _index)
        {
            if(_m_lLabRefObjList == null || _index < 0 || _index >= _m_lLabRefObjList.Count)
                return;
            
            TreasureHuntLabRefObj labRefObj = _m_lLabRefObjList[_index];
            _itemWnd.setData(labRefObj);
            _itemWnd.setSelect(labRefObj == _m_selectedLabRefObj);
        }

        public void setData(List<TreasureHuntLabRefObj> _labRefObjList, TreasureHuntLabRefObj _selectedLabRefObj)
        {
            _m_lLabRefObjList = _labRefObjList;
            _m_selectedLabRefObj = _selectedLabRefObj;

            refreshWnd(_m_lLabRefObjList?.Count ?? 0);
        }

        private void _onLabSelect(TreasureHuntLabRefObj _labRefObj)
        {
            onLabSelect?.Invoke(_labRefObj);
        }
    }
}