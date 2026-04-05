using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 功能列表容器
    /// </summary>
    public class GGUIWndFuncDetailGrid: _ATNPGGUIWndShowAnimGrid<GGUIMonoFuncDetailGridItem,GGUIMonoFuncDetailGrid,GGUIWndFuncDetailGridItem>
    {
        //数据列表
        private List<FuncUnlockInfo> _m_itemDataList;
        //当前选中的item信息
        private FuncUnlockInfo _m_curSelectInfo;
        //点击选择
        private Action<FuncUnlockInfo, int> _m_aOnSelect;

        /// <summary>
        /// 选中item
        /// </summary>
        public Action<FuncUnlockInfo, int> onSelect
        {
            get { return _m_aOnSelect; }
            set { _m_aOnSelect = value; }
        }

        public GGUIWndFuncDetailGrid(GGUIMonoFuncDetailGrid gridMono) : base(gridMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_curSelectInfo = null;
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_curSelectInfo = null;
        }

        protected override void _onWndInitDone()
        {
        }

        protected override GGUIWndFuncDetailGridItem _createItemWnd(GGUIMonoFuncDetailGridItem _itemMono)
        {
            GGUIWndFuncDetailGridItem item = new GGUIWndFuncDetailGridItem(_itemMono);
            item.onSelect += _onClickSelect;
            return item;
        }
        protected override void _onRefreshItemWnd(GGUIWndFuncDetailGridItem _itemWnd, int _itemIdx)
        {
            if (null == _m_itemDataList || _itemIdx >= _m_itemDataList.Count)
                return;

            //获取数据对象
            FuncUnlockInfo showData = _m_itemDataList[_itemIdx];
            if (null == showData)
                return;

            _itemWnd.setInfo(showData);
            _itemWnd.setSelect(false);
            if (_m_curSelectInfo != null && _m_curSelectInfo.funcId == showData.funcId)
                _itemWnd.setSelect(true);

            //默认选中第一个
            if (_itemIdx == 0 && _m_curSelectInfo == null)
                _onClickSelect(_itemWnd);
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<FuncUnlockInfo> _itemDataList)
        {
            if (_itemDataList == null)
                return;

            _m_itemDataList = _itemDataList;
            setItemCount(_m_itemDataList.Count);
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        public void refreshList()
        {
            forceRefreshAllItem();
        }

        /// <summary>
        /// 设置选中信息
        /// </summary>
        /// <param name="_info"></param>
        public void setSelectInfo(FuncUnlockInfo _info)
        {
            _m_curSelectInfo = _info;
            forceRefreshAllItem();
        }

        /// <summary>
        /// 设置选中上一个
        /// </summary>
        public void setSelectLast()
        {
            if (_m_itemDataList == null || _m_curSelectInfo == null)
                return;

            for (int i = 0; i < _m_itemDataList.Count; i++)
            {
                if (_m_itemDataList[i].funcId == _m_curSelectInfo.funcId && i > 0)
                {
                    _m_curSelectInfo = _m_itemDataList[i - 1];
                    forceRefreshAllItem();
                    _m_aOnSelect?.Invoke(_m_curSelectInfo, i - 1);
                    return;
                }
            }
        }

        /// <summary>
        /// 设置选中下一个
        /// </summary>
        public void setSelectNext()
        {
            if (_m_itemDataList == null || _m_curSelectInfo == null)
                return;

            for (int i = 0; i < _m_itemDataList.Count; i++)
            {
                if (_m_itemDataList[i].funcId == _m_curSelectInfo.funcId && i < _m_itemDataList.Count - 1)
                {
                    _m_curSelectInfo = _m_itemDataList[i + 1];
                    forceRefreshAllItem();
                    _m_aOnSelect?.Invoke(_m_curSelectInfo, i + 1);
                    return;
                }
            }
        }

        /// <summary>
        /// 点击选中
        /// </summary>
        /// <param name="_item"></param>
        private void _onClickSelect(GGUIWndFuncDetailGridItem _item)
        {
            if (_item == null)
                return;

            _m_curSelectInfo = _item.funcUnlockInfo;
            _item.setSelect(true);
            forceRefreshAllItem();
            _m_aOnSelect?.Invoke(_item.funcUnlockInfo, _item.itemIdx);
        }
    }
}