using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 商店子页签列表
    /// </summary>
    public class GGUIWndShopSubTabContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoShopSubTabContainerItem, GGUIMonoShopSubTabContainer, GGUIWndShopSubTabContainerItem>
    {
        //窗口容器
        private List<GGUIWndShopSubTabContainerItem> _m_lItemList;
        //当前选中的item
        private GGUIWndShopSubTabContainerItem _m_curSelectItem;
        //点击item事件
        private Action<GGUIWndShopSubTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndShopSubTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndShopSubTabContainer(GGUIMonoShopSubTabContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            //重置选择
            _m_curSelectItem?.setSelect(false);
            _m_curSelectItem = null;
        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndShopSubTabContainerItem>();
        }

        protected override GGUIWndShopSubTabContainerItem _createItemWnd(GGUIMonoShopSubTabContainerItem _itemMono)
        {
            // 创建对象
            GGUIWndShopSubTabContainerItem item = new GGUIWndShopSubTabContainerItem(_itemMono);
            item.onClickItem += _onClickTabItem;
            return item;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(List<long> _shopIdList, long _targetShopId)
        {
            if (wnd == null || _shopIdList == null)
                return;

            GGUIWndShopSubTabContainerItem firstItem = null;
            GGUIWndShopSubTabContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _shopIdList.Count; i++)
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
                itemWnd.setInfo(_shopIdList[i]);
                itemWnd.setSelect(false);

                //记录第一个item
                if (firstItem == null)
                    firstItem = itemWnd;

                //如果有目标商店id，则选中目标商店
                if (_targetShopId > 0 && _targetShopId == _shopIdList[i])
                {
                    _m_curSelectItem = itemWnd;
                    _m_curSelectItem.setSelect(true);
                    _m_aOnClickItem?.Invoke(_m_curSelectItem);
                }

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

            //如果没有选中项，则默认选中第一个
            if (_m_curSelectItem == null)
            {
                _m_curSelectItem = firstItem;
                _m_curSelectItem?.setSelect(true);
                _m_aOnClickItem?.Invoke(_m_curSelectItem);
            }
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refrshRedTip()
        {
            if (_m_lItemList == null)
                return;

            foreach (GGUIWndShopSubTabContainerItem _item in _m_lItemList)
            {
                _item?.refreshRedTip();
            }
        }

        /// <summary>
        /// 点击item事件
        /// </summary>
        /// <param name="_item"></param>
        private void _onClickTabItem(GGUIWndShopSubTabContainerItem _item)
        {
            if (_item != null && _m_curSelectItem != null && _m_curSelectItem == _item)
                return;

            _m_curSelectItem?.setSelect(false);
            _m_curSelectItem = _item;
            _m_curSelectItem?.setSelect(true);

            _m_aOnClickItem?.Invoke(_item);
        }
    }
}
