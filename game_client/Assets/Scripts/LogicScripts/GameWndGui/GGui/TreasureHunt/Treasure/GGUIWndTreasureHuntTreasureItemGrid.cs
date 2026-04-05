using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 奇物itemGrid
    /// </summary>
    public class GGUIWndTreasureHuntTreasureItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoTreasureHuntTreasureItem, GGUIMonoTreasureHuntTreasureItemGrid, GGUIWndTreasureHuntTreasureItem>
    {
        private List<_ITreasureHuntTreasureInfo> _m_lTreasureInfoList;
        private Func<_ITreasureHuntTreasureInfo, bool> _m_fCheckNeedShowRedTip;
        
        public GGUIWndTreasureHuntTreasureItemGrid(GGUIMonoTreasureHuntTreasureItemGrid _wnd, Func<_ITreasureHuntTreasureInfo, bool> _checkNeedShowRedTip) : base(_wnd)
        {
            _m_fCheckNeedShowRedTip = _checkNeedShowRedTip;
            
            initWnd();
        }

        public event Action<_ITreasureHuntTreasureInfo> onTreasureClick; // 当奇物被点击 

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            onTreasureClick = null;

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

        protected override GGUIWndTreasureHuntTreasureItem _createItemWnd(GGUIMonoTreasureHuntTreasureItem _itemMono)
        {
            GGUIWndTreasureHuntTreasureItem itemWnd = new GGUIWndTreasureHuntTreasureItem(_itemMono);
            itemWnd.onTreasureClick += _onClickTreasure;
            return itemWnd;
        }

        protected override void _onRefreshItemWnd(GGUIWndTreasureHuntTreasureItem _itemWnd, int _itemIdx)
        {
            if(_m_lTreasureInfoList == null || _itemIdx < 0 || _itemIdx >= _m_lTreasureInfoList.Count)
                return;
            
            _ITreasureHuntTreasureInfo treasureInfo = _m_lTreasureInfoList.SafeGet(_itemIdx);
            _itemWnd.setData(treasureInfo);
            
            if(_m_fCheckNeedShowRedTip != null)
                _itemWnd.setShowRedTip(_m_fCheckNeedShowRedTip(treasureInfo));
        }

        public void setData(List<_ITreasureHuntTreasureInfo> _treasureInfoList)
        {
            _m_lTreasureInfoList = _treasureInfoList;

            int itemCount = _m_lTreasureInfoList?.Count ?? 0;
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, itemCount <= 0);
            }
            
            setItemCount(itemCount);
        }

        public void refreshItem(long _treasureId)
        {
            int index = _m_lTreasureInfoList?.FindIndex((info) => info != null && info.treasureId == _treasureId) ?? -1;
            if(index >= 0)
                forceRefreshItem(index);
        }
        
        private void _onClickTreasure(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            onTreasureClick?.Invoke(_treasureInfo);
        }
    }
}