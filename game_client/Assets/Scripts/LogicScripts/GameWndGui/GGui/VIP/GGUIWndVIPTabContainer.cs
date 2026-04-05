using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP页签列表
    /// </summary>
    public class GGUIWndVIPTabContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoVIPTabContainerItem, GGUIMonoVIPTabContainer, GGUIWndVIPTabContainerItem>
    {
        //item列表
        protected List<GGUIWndVIPTabContainerItem> _m_lItemList;
        //当前选中的item
        private GGUIWndVIPTabContainerItem _m_wCurSelectItem;
        //点击item事件
        private Action<GGUIWndVIPTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 当前选中的item
        /// </summary>
        public GGUIWndVIPTabContainerItem curSelectItem { get { return _m_wCurSelectItem; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndVIPTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndVIPTabContainer(GGUIMonoVIPTabContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndVIPTabContainerItem _createItemWnd(GGUIMonoVIPTabContainerItem _itemMono)
        {
            GGUIWndVIPTabContainerItem item = new GGUIWndVIPTabContainerItem(_itemMono);
            item.onClickItem += _onClickItem;
            return item;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
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
            _m_lItemList = new List<GGUIWndVIPTabContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<VipRefObj> _refList)
        {
            if (wnd == null || _refList == null || _m_lItemList == null)
                return;

            GGUIWndVIPTabContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _refList.Count; i++)
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
                itemWnd.setInfo(_refList[i]);
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

            _m_wCurSelectItem?.setSelect(true);
        }

        /// <summary>
        /// 设置选中item
        /// </summary>
        /// <param name="_vip"></param>
        public void setSelectItem(long _vip, bool _isSmoothMove)
        {
            if (_m_wCurSelectItem != null && 
                _m_wCurSelectItem.vipRef != null &&
                _m_wCurSelectItem.vipRef.vip_lvl == _vip)
                return;
            
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] != null && 
                    _m_lItemList[i].vipRef != null &&
                    _m_lItemList[i].vipRef.vip_lvl == _vip)
                {
                    if (!_isSmoothMove)
                    {
                        _m_wCurSelectItem?.setSelect(false);
                        _m_wCurSelectItem = _m_lItemList[i];
                        _m_wCurSelectItem.setSelect(true);
                        _m_aOnClickItem?.Invoke(_m_wCurSelectItem);
                        _moveToTarget(i);
                    }
                    else
                        _onClickItem(_m_lItemList[i]);
                    break;
                }
            }
        }

        //移动到目标位置
        private void _moveToTarget(int _index)
        {
            if (wnd == null || null == wnd.itemTemplate)
                return;

            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                // 计算content的坐标活动范围
                if (wnd != null &&
                    wnd.itemTemplate != null &&
                    wnd.itemContainer != null &&
                    wnd.itemContainer.transform != null &&
                    wnd.scrollRect != null &&
                    wnd.scrollRect.transform != null)
                {
                    float contentRange = ((RectTransform)wnd.itemContainer.transform).rect.width - ((RectTransform)wnd.scrollRect.transform).rect.width;
                    moveToHorizontalRate(1 - ((_index * (((RectTransform) wnd.itemTemplate.transform).rect.width + ((HorizontalLayoutGroup) wnd.itemContainer).spacing)) / contentRange));
                }
            });
        }

        //点击选中item
        private void _onClickItem(GGUIWndVIPTabContainerItem _item)
        {
            if (_item == null || _item.vipRef != null && _m_wCurSelectItem != null && _m_wCurSelectItem.vipRef != null && _item.vipRef.vip_lvl == _m_wCurSelectItem.vipRef.vip_lvl)
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
