using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜页签列表
    /// </summary>
    public class GGUIWndRankRushMultipleDetailTabContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoRankRushMultipleDetailTabContainerItem, GGUIMonoRankRushMultipleDetailTabContainer, GGUIWndRankRushMultipleDetailTabContainerItem>
    {
        //item列表
        protected List<GGUIWndRankRushMultipleDetailTabContainerItem> _m_lItemList;
        //当前选中的item
        private GGUIWndRankRushMultipleDetailTabContainerItem _m_wCurSelectItem;
        //点击item事件
        private Action<GGUIWndRankRushMultipleDetailTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 当前选中的item
        /// </summary>
        public GGUIWndRankRushMultipleDetailTabContainerItem curSelectItem { get { return _m_wCurSelectItem; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndRankRushMultipleDetailTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndRankRushMultipleDetailTabContainer(GGUIMonoRankRushMultipleDetailTabContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndRankRushMultipleDetailTabContainerItem _createItemWnd(GGUIMonoRankRushMultipleDetailTabContainerItem _itemMono)
        {
            GGUIWndRankRushMultipleDetailTabContainerItem item = new GGUIWndRankRushMultipleDetailTabContainerItem(_itemMono);
            item.onClickItem += _onClickItem;
            return item;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if(_m_lItemList != null)
            {
                foreach (GGUIWndRankRushMultipleDetailTabContainerItem item in _m_lItemList)
                {
                    item?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;

            _m_wCurSelectItem = null;
            _m_aOnClickItem = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndRankRushMultipleDetailTabContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<ActivityRankRushInfo> _infoList)
        {
            if (wnd == null || _infoList == null || _m_lItemList == null)
                return;

            GGUIWndRankRushMultipleDetailTabContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _infoList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];

                itemWnd.showWnd();
                itemWnd.setInfo(_infoList[i]);
                itemWnd.setSelect(false);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }

            //默认选中第一个
            if (_m_wCurSelectItem == null && _m_lItemList.Count > 0)
                _m_wCurSelectItem = _m_lItemList[0];
            _m_wCurSelectItem?.setSelect(true);
            _m_aOnClickItem?.Invoke(_m_wCurSelectItem);
        }

        //点击选中item
        private void _onClickItem(GGUIWndRankRushMultipleDetailTabContainerItem _item)
        {
            if (_item == null || _m_wCurSelectItem == _item)
                return;

            _m_wCurSelectItem?.setSelect(false);
            _m_wCurSelectItem = _item;
            _m_wCurSelectItem.setSelect(true);

            //执行回调
            _m_aOnClickItem?.Invoke(_m_wCurSelectItem);

            //移动到可视范围内
            GCommon.setContainerMoveItemWithinRangeInHorizontal(_item.rectTransform, rectTransform, (RectTransform)wnd?.itemContainer?.transform);
        }
    }
}
