using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 矿石itemContainer
    /// </summary>
    public class GGUIWndTreasureHuntOreItemContainer : _AGGUISubWndCommonContainer<GGUIMonoTreasureHuntOreItem, GGUIMonoTreasureHuntOreItemContainer, GGUIWndTreasureHuntOreItem>
    {
        private List<_ITreasureHuntOreInfo> _m_lOreInfoList;
        
        public GGUIWndTreasureHuntOreItemContainer(GGUIMonoTreasureHuntOreItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<_ITreasureHuntOreInfo> onOreClick; // 当矿石被点击 
        
        protected override void _onDiscard()
        {
            onOreClick = null;
                
            base._onDiscard();
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
        
        protected override void _refreshItemWnd(GGUIWndTreasureHuntOreItem _itemWnd, int _index)
        {
            if(_m_lOreInfoList == null || _index < 0 || _index >= _m_lOreInfoList.Count)
                return;
            
            _ITreasureHuntOreInfo oreInfo = _m_lOreInfoList.SafeGet(_index);
            _itemWnd.setData(oreInfo);
        }

        public void setData(List<_ITreasureHuntOreInfo> _oreInfoList)
        {
            _m_lOreInfoList = _oreInfoList;

            int itemCount = _m_lOreInfoList?.Count ?? 0;
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, itemCount <= 0);
            }
            
            refreshWnd(itemCount);
        }
        
        private void _onClickOre(_ITreasureHuntOreInfo _oreInfo)
        {
            onOreClick?.Invoke(_oreInfo);
        }
    }
}