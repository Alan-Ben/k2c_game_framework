using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合图鉴item列表
    /// </summary>
    public class GGUIWndTreasureHuntCompositeCatalogItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoTreasureHuntCompositeCatalogItem, GGUIMonoTreasureHuntCompositeCatalogItemGrid, GGUIWndTreasureHuntCompositeCatalogItem>
    {
        private List<TreasureHuntCompositeCatalogInfo> _m_lCompositeCatalogInfoList;
        private Func<TreasureHuntCompositeCatalogInfo, bool> _m_fCheckNeedShowRedTip;
        
        public GGUIWndTreasureHuntCompositeCatalogItemGrid(GGUIMonoTreasureHuntCompositeCatalogItemGrid _wnd, Func<TreasureHuntCompositeCatalogInfo, bool> _checkNeedShowRedTip = null) : base(_wnd)
        {
            _m_fCheckNeedShowRedTip = _checkNeedShowRedTip;
            
            initWnd();
        }
        
        public event Action<GGUIWndTreasureHuntCompositeCatalogItem> onItemClick; 

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            onItemClick = null;
            
            _m_fCheckNeedShowRedTip = null;
            _m_lCompositeCatalogInfoList = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lCompositeCatalogInfoList = null;
        }

        protected override void _onReset()
        {
            _m_lCompositeCatalogInfoList = null;
        }

        protected override GGUIWndTreasureHuntCompositeCatalogItem _createItemWnd(GGUIMonoTreasureHuntCompositeCatalogItem _itemMono)
        {
            GGUIWndTreasureHuntCompositeCatalogItem itemWnd = new GGUIWndTreasureHuntCompositeCatalogItem(_itemMono);
            itemWnd.onItemClick += _onItemClick;
            return itemWnd;
        }

        protected override void _onRefreshItemWnd(GGUIWndTreasureHuntCompositeCatalogItem _itemWnd, int _itemIdx)
        {
            if(_itemWnd == null || _m_lCompositeCatalogInfoList == null || _itemIdx < 0 || _itemIdx >= _m_lCompositeCatalogInfoList.Count)
                return;
            
            TreasureHuntCompositeCatalogInfo compositeCatalogInfo = _m_lCompositeCatalogInfoList.SafeGet(_itemIdx);
            _itemWnd.setData(compositeCatalogInfo);
            
            if(_m_fCheckNeedShowRedTip != null)
                _itemWnd.setShowRedTip(_m_fCheckNeedShowRedTip(compositeCatalogInfo));
        }

        public void setData(List<TreasureHuntCompositeCatalogInfo> _compositeCatalogInfoList)
        {
            _m_lCompositeCatalogInfoList = _compositeCatalogInfoList;

            int itemCount = _m_lCompositeCatalogInfoList?.Count ?? 0;
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, itemCount <= 0);
            }
            
            setItemCount(itemCount);
        }

        public void refreshItem(long _compositeCatalogId)
        {
            int index = _m_lCompositeCatalogInfoList?.FindIndex((info) => info != null && info.compositeCatalogId == _compositeCatalogId) ?? -1;
            if(index >= 0)
                forceRefreshItem(index);
        }
     
        /// <summary>
        /// 
        /// </summary>
        private void _onItemClick(GGUIWndTreasureHuntCompositeCatalogItem _itemWnd)
        {
            onItemClick?.Invoke(_itemWnd);
        }
    }
}