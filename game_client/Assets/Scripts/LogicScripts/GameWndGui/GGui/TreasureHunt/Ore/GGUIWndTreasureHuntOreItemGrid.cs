using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 矿石itemGrid
    /// </summary>
    public class GGUIWndTreasureHuntOreItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoTreasureHuntOreItem, GGUIMonoTreasureHuntOreItemGrid, GGUIWndTreasureHuntOreItem>
    {
        private List<_ITreasureHuntOreInfo> _m_lOreInfoList;
        private Func<_ITreasureHuntOreInfo, bool> _m_fCheckNeedShowRedTip;
        
        public GGUIWndTreasureHuntOreItemGrid(GGUIMonoTreasureHuntOreItemGrid _wnd, Func<_ITreasureHuntOreInfo, bool> _checkNeedShowRedTip = null) : base(_wnd)
        {
            _m_fCheckNeedShowRedTip = _checkNeedShowRedTip;
            
            initWnd();
        }

        public event Action<_ITreasureHuntOreInfo> onOreClick; // 当矿石被点击 

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            onOreClick = null;

            _m_fCheckNeedShowRedTip = null;
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

        protected override void _onRefreshItemWnd(GGUIWndTreasureHuntOreItem _itemWnd, int _itemIdx)
        {
            if(_m_lOreInfoList == null || _itemIdx < 0 || _itemIdx >= _m_lOreInfoList.Count)
                return;
            
            _ITreasureHuntOreInfo oreInfo = _m_lOreInfoList.SafeGet(_itemIdx);
            _itemWnd.setData(oreInfo);
            
            if(_m_fCheckNeedShowRedTip != null)
                _itemWnd.setShowRedTip(_m_fCheckNeedShowRedTip(oreInfo));
        }

        public void setData(List<_ITreasureHuntOreInfo> _oreInfoList)
        {
            _m_lOreInfoList = _oreInfoList;

            int itemCount = _m_lOreInfoList?.Count ?? 0;
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, itemCount <= 0);
            }
            
            setItemCount(itemCount);
        }

        public void refreshItem(long _oreId)
        {
            int index = _m_lOreInfoList?.FindIndex((info) => info != null && info.oreId == _oreId) ?? -1;
            if(index >= 0)
                forceRefreshItem(index);
        }
        
        private void _onClickOre(_ITreasureHuntOreInfo _oreInfo)
        {
            onOreClick?.Invoke(_oreInfo);
        }
    }
}