using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 可变大小的奇物itemContainer
    /// </summary>
    public class GGUIWndTreasureHuntTreasureItemSizeChangeableContainer : 
        _ATNPGGUIWndSizeChangeableContainer<GGUIMonoTreasureHuntTreasureItem, GGUIMonoTreasureHuntTreasureItemSizeChangeableContainer, GGUIWndTreasureHuntTreasureItem>
    {
        private List<_ITreasureHuntTreasureInfo> _m_lTreasureInfoList;
        
        public GGUIWndTreasureHuntTreasureItemSizeChangeableContainer(GGUIMonoTreasureHuntTreasureItemSizeChangeableContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<_ITreasureHuntTreasureInfo> onTreasureClick; // 当奇物被点击 

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            onTreasureClick = null;
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

        protected override GGUIWndTreasureHuntTreasureItem _createItemWnd(GGUIMonoTreasureHuntTreasureItem _itemMono)
        {
            GGUIWndTreasureHuntTreasureItem itemWnd = new GGUIWndTreasureHuntTreasureItem(_itemMono);
            itemWnd.onTreasureClick += _onClickTreasure;
            return itemWnd;
        }

        protected override void _discardItem(GGUIWndTreasureHuntTreasureItem _itemWnd)
        {
            if(_itemWnd != null)
                _itemWnd.onTreasureClick -= _onClickTreasure;
            
            base._discardItem(_itemWnd);
        }
        
        protected override bool _refreshItemWnd(int _index, GGUIWndTreasureHuntTreasureItem _itemWnd)
        {
            if(_m_lTreasureInfoList == null || _index < 0 || _index >= _m_lTreasureInfoList.Count || _itemWnd == null)
                return false;
            
            _ITreasureHuntTreasureInfo treasureInfo = _m_lTreasureInfoList.SafeGet(_index);
            _itemWnd.setData(treasureInfo);
            return true;
        }

        public void setData(List<_ITreasureHuntTreasureInfo> _treasureInfoList)
        {
            _m_lTreasureInfoList = _treasureInfoList;
            
            showItemList(_m_lTreasureInfoList?.Count ?? 0);
        }
        
        private void _onClickTreasure(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            onTreasureClick?.Invoke(_treasureInfo);
        }
    }
}