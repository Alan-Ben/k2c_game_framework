using System;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndChildContainer : _AGGUISubWndCommonContainer<GGUIMonoChildContainerItem, GGUIMonoChildContainer, GGUISubWndChildContainerItem>
    {
        private readonly Action<SeatInfo> _m_onSeatClick;

        private ChildViewMgr _m_viewMgr;

        private int _m_iFirstLockItemIdnex;
        
        
        public GGUISubWndChildContainer(GGUIMonoChildContainer _wnd, Action<SeatInfo> _onSeatClick) 
            : base(_wnd)
        {
            _m_onSeatClick = _onSeatClick;
            _m_iFirstLockItemIdnex = -1;
            initWnd();
        }
        

        protected override GGUISubWndChildContainerItem _createItemWnd(GGUIMonoChildContainerItem _itemMono)
        {
            return new GGUISubWndChildContainerItem(_itemMono, _onSeatClick);
        }
        protected override void _refreshItemWnd(GGUISubWndChildContainerItem _itemWnd, int _index)
        {
            if (_m_viewMgr == null)
                return;
            if (_index < 0 || _index >= _m_viewMgr.seatList.Count)
                return;

            SeatInfo seatInfo = _m_viewMgr.seatList[_index];

            // 记录第一个锁定的席位索引
            if (!seatInfo.isUnlock() && _m_iFirstLockItemIdnex < 0)
                _m_iFirstLockItemIdnex = _index;

            _itemWnd.refreshWnd(seatInfo, seatInfo == _m_viewMgr.curSelectSeatInfo, _m_iFirstLockItemIdnex == _index);
        }


        public void refreshWnd(ChildViewMgr _viewMgr)
        {
            _m_viewMgr = _viewMgr;
            _m_iFirstLockItemIdnex = -1;
            refreshWnd(_m_viewMgr?.seatList.Count ?? 0);
        }
        public void showExpCollect(ChildInfo _childInfo, long _num)
        {
            if (_num <= 0)
                return;
            
            GGUISubWndChildContainerItem item = getItem(_item => _item.childInfo == _childInfo);
            item?.showExpCollect(_num);
        }
        public void showEnergyRecover(long _seatId, long _count)
        {
            if (_count <= 0)
                return;
            
            GGUISubWndChildContainerItem item = getItem(_item => _item.seatInfo != null && _item.seatInfo.id == _seatId);
            item?.showEnergyRecover(_count);
        }
        public void refreshItem(ChildInfo _childInfo)
        {
            GGUISubWndChildContainerItem item = getItem(_item => _item.childInfo == _childInfo);
            item?.refreshWnd();
        }
        public void playEducateSfx(ChildInfo _childInfo)
        {
            GGUISubWndChildContainerItem item = getItem(_item => _item.childInfo == _childInfo);
            item?.playEducateSfx();
        }

        /// <summary>
        /// 获取未命名且未毕业的子嗣的 RectTransform
        /// </summary>
        /// <returns></returns>
        public RectTransform getNamedNoGraduateRectTransform()
        {
            GGUISubWndChildContainerItem targetItem = getItem(_item => _item.childInfo != null && !string.IsNullOrEmpty(_item.childInfo.name) && !_item.childInfo.canGraduate());
            return targetItem?.rectTransform;
        }


        private void _onSeatClick(SeatInfo _seatInfo)
        {
            _m_onSeatClick?.Invoke(_seatInfo);
        }
    }
}