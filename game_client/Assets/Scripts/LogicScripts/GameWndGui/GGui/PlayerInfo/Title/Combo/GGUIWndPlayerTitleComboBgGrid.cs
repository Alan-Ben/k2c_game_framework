using System;
using System.Collections.Generic;
using Common.NpPlayerInfoObj;

namespace GOE
{
    /// <summary>
    /// 组合称号文本列表
    /// </summary>
    public class GGUIWndPlayerTitleComboBgGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoPlayerTitleComboBgGridItem, GGUIMonoPlayerTitleComboBgGrid, GGUIWndPlayerTitleComboBgGridItem>
    {
        //信息列表
        private List<PlayerTitleComboBgInfo> _m_lInfoList;
        //当前选中的id
        private long _m_lCurSelectId;
        //点击选中
        private Action<GGUIWndPlayerTitleComboBgGridItem> _m_aClickSelect;

        /// <summary>
        /// 点击选中
        /// </summary>
        public Action<GGUIWndPlayerTitleComboBgGridItem> onClickSelect
        {
            get { return _m_aClickSelect; }
            set { _m_aClickSelect = value; }
        }

        public GGUIWndPlayerTitleComboBgGrid(GGUIMonoPlayerTitleComboBgGrid _wnd) : base(_wnd)
        {
            _m_lInfoList = new List<PlayerTitleComboBgInfo>();
            initWnd();
        }

        protected override GGUIWndPlayerTitleComboBgGridItem _createItemWnd(GGUIMonoPlayerTitleComboBgGridItem _itemMono)
        {
            GGUIWndPlayerTitleComboBgGridItem item = new GGUIWndPlayerTitleComboBgGridItem(_itemMono);
            item.onClickSelect += _onClickSelect;
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndPlayerTitleComboBgGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_lInfoList.Count)
                return;

            _itemWnd.setInfo(_m_lInfoList[_itemIdx]);
            _itemWnd.setSelect(_m_lCurSelectId == _m_lInfoList[_itemIdx].id);
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
        public void setShowData(List<PlayerTitleComboBgInfo> _idList)
        {
            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_idList);
            setItemCount(_m_lInfoList.Count);
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_id"></param>
        public void setSelect(long _id)
        {
            _m_lCurSelectId = _id;
            forceRefreshAllItem();
        }

        private void _onClickSelect(GGUIWndPlayerTitleComboBgGridItem _item)
        {
            if (_item == null || _item.bgInfo == null)
                return;

            //是否已经解锁
            if (_item.bgInfo.isUnlock)
            {
                int preIndex = -1;
                int curIndex = -1;
                for (int i = 0; i < _m_lInfoList.Count; i++)
                {
                    if (_m_lInfoList[i] == null)
                        continue;

                    if (_m_lInfoList[i].id == _m_lCurSelectId)
                        preIndex = i;
                    if (_m_lInfoList[i].id == _item.bgInfo.id)
                        curIndex = i;
                }

                //设置当前选中
                _m_lCurSelectId = _item.bgInfo.id;

                //刷新对应item
                if (preIndex >= 0)
                    forceRefreshItem(preIndex);
                if (curIndex >= 0)
                    forceRefreshItem(curIndex);
            }

            _m_aClickSelect?.Invoke(_item);
        }
    }
}
