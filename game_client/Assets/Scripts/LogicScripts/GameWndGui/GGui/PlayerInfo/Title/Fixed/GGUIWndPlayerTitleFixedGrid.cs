using System;
using System.Collections.Generic;
using Common.NpPlayerInfoObj;

namespace GOE
{
    /// <summary>
    /// 固定称号列表
    /// </summary>
    public class GGUIWndPlayerTitleFixedGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoPlayerTitleFixedGridItem, GGUIMonoPlayerTitleFixedGrid, GGUIWndPlayerTitleFixedGridItem>
    {
        //信息列表
        private List<PlayerTitleRefObj> _m_lInfoList;
        //当前选中的
        private PlayerTitleRefObj _m_lCurSelectRef;
        //点击选中
        private Action<GGUIWndPlayerTitleFixedGridItem> _m_aClickSelect;

        /// <summary>
        /// 当前选中的称号
        /// </summary>
        public PlayerTitleRefObj curSelectRef { get { return _m_lCurSelectRef; } }
        /// <summary>
        /// 点击选中
        /// </summary>
        public Action<GGUIWndPlayerTitleFixedGridItem> onClickSelect
        {
            get { return _m_aClickSelect; }
            set { _m_aClickSelect = value; }
        }

        public GGUIWndPlayerTitleFixedGrid(GGUIMonoPlayerTitleFixedGrid _wnd) : base(_wnd)
        {
            _m_lInfoList = new List<PlayerTitleRefObj>();
            initWnd();
        }

        protected override GGUIWndPlayerTitleFixedGridItem _createItemWnd(GGUIMonoPlayerTitleFixedGridItem _itemMono)
        {
            GGUIWndPlayerTitleFixedGridItem item = new GGUIWndPlayerTitleFixedGridItem(_itemMono);
            item.onClickSelect += _onClickSelect;
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndPlayerTitleFixedGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_lInfoList.Count)
                return;

            _itemWnd.setInfo(_m_lInfoList[_itemIdx]);
            _itemWnd.setSelect(_m_lCurSelectRef != null && _m_lCurSelectRef.id == _m_lInfoList[_itemIdx].id);
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

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_list"></param>
        public void setShowData(List<PlayerTitleRefObj> _refList)
        {
            if (_refList == null)
                return;

            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_refList);
            setItemCount(_m_lInfoList.Count);

            //设置默认选中当前佩戴的，否则选中第一个
            PlayerInfo_Title curTitle = NPPlayer.instance.titleComp.getCurWearCommonTitleInfo();
            if (curTitle != null)
                setSelect(curTitle.getId());
            else if (_m_lInfoList.Count > 0 && _m_lInfoList[0] != null)
                setSelect(_m_lInfoList[0].id);
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_id"></param>
        public void setSelect(long _id)
        {
            _m_lCurSelectRef = GRefdataCoreMgr.instance.playerTitleRefCore.getRef(_id);
            forceRefreshAllItem();
        }

        //点击item事件
        private void _onClickSelect(GGUIWndPlayerTitleFixedGridItem _item)
        {
            if (_item == null || _item.titleRef == null || (_m_lCurSelectRef != null && _m_lCurSelectRef.id == _item.titleRef.id))
                return;

            _m_lCurSelectRef = _item.titleRef;
            forceRefreshAllItem();
            _m_aClickSelect?.Invoke(_item);
        }
    }
}
