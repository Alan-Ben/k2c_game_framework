using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 可变大小的矿石itemContainer
    /// </summary>
    public class GGUIWndTreasureHuntOreItemSizeChangeableContainer : 
        _ATNPGGUIWndSizeChangeableContainer<GGUIMonoTreasureHuntOreItem, GGUIMonoTreasureHuntOreItemSizeChangeableContainer, GGUIWndTreasureHuntOreItem>
    {
        private List<_ITreasureHuntOreInfo> _m_lOreInfoList;
        
        public GGUIWndTreasureHuntOreItemSizeChangeableContainer(GGUIMonoTreasureHuntOreItemSizeChangeableContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<_ITreasureHuntOreInfo> onOreClick; // 当矿石被点击 

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            onOreClick = null;
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

        protected override GGUIWndTreasureHuntOreItem _createItemWnd(GGUIMonoTreasureHuntOreItem _itemMono)
        {
            GGUIWndTreasureHuntOreItem itemWnd = new GGUIWndTreasureHuntOreItem(_itemMono);
            itemWnd.onOreClick += _onClickOre;
            return itemWnd;
        }

        protected override void _discardItem(GGUIWndTreasureHuntOreItem _itemWnd)
        {
            if(_itemWnd != null)
                _itemWnd.onOreClick -= _onClickOre;
            
            base._discardItem(_itemWnd);
        }
        
        protected override bool _refreshItemWnd(int _index, GGUIWndTreasureHuntOreItem _itemWnd)
        {
            if(_m_lOreInfoList == null || _index < 0 || _index >= _m_lOreInfoList.Count || _itemWnd == null)
                return false;
            
            _ITreasureHuntOreInfo oreInfo = _m_lOreInfoList.SafeGet(_index);
            _itemWnd.setData(oreInfo);
            return true;
        }

        public void setData(List<_ITreasureHuntOreInfo> _oreInfoList)
        {
            _m_lOreInfoList = _oreInfoList;
            
            showItemList(_m_lOreInfoList?.Count ?? 0);
        }
        
        private void _onClickOre(_ITreasureHuntOreInfo _oreInfo)
        {
            onOreClick?.Invoke(_oreInfo);
        }
    }
}