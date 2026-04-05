using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 奇物itemContainer
    /// </summary>
    public class GGUIWndTreasureHuntTreasureItemContainer : _AGGUISubWndCommonContainer<GGUIMonoTreasureHuntTreasureItem, GGUIMonoTreasureHuntTreasureItemContainer, GGUIWndTreasureHuntTreasureItem>
    {
        private List<_ITreasureHuntTreasureInfo> _m_lTreasureInfoList;
        
        public GGUIWndTreasureHuntTreasureItemContainer(GGUIMonoTreasureHuntTreasureItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<_ITreasureHuntTreasureInfo> onTreasureClick; // 当奇物被点击 
        
        protected override void _onDiscard()
        {
            onTreasureClick = null;
                
            base._onDiscard();
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
        
        protected override void _refreshItemWnd(GGUIWndTreasureHuntTreasureItem _itemWnd, int _index)
        {
            if(_m_lTreasureInfoList == null || _index < 0 || _index >= _m_lTreasureInfoList.Count)
                return;
            
            _ITreasureHuntTreasureInfo treasureInfo = _m_lTreasureInfoList.SafeGet(_index);
            _itemWnd.setData(treasureInfo);
        }

        public void setData(List<_ITreasureHuntTreasureInfo> _treasureInfoList)
        {
            _m_lTreasureInfoList = _treasureInfoList;

            int itemCount = _m_lTreasureInfoList?.Count ?? 0;
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, itemCount <= 0);
            }
            
            refreshWnd(itemCount);
        }
        
        private void _onClickTreasure(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            onTreasureClick?.Invoke(_treasureInfo);
        }
    }
}