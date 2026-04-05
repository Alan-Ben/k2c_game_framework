using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴家人可为空头像列表
    /// </summary>
    public class GGUIWndHeroBlessNullableContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoHeroBlessNullableContainerItem, GGUIMonoHeroBlessNullableContainer, GGUIWndHeroBlessNullableContainerItem>
    {
        //item列表
        protected List<GGUIWndHeroBlessNullableContainerItem> _m_lItemList;
        //点击item
        private Action<GGUIWndHeroBlessNullableContainerItem> _m_aOnClickItem;
        //当前选中的item
        private GGUIWndHeroBlessNullableContainerItem _m_wCurSelectItem;

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndHeroBlessNullableContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndHeroBlessNullableContainer(GGUIMonoHeroBlessNullableContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndHeroBlessNullableContainerItem _createItemWnd(GGUIMonoHeroBlessNullableContainerItem _itemMono)
        {
            GGUIWndHeroBlessNullableContainerItem item = new GGUIWndHeroBlessNullableContainerItem(_itemMono);
            item.onSelectItem += _onClickItem;
            return item;
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            _m_wCurSelectItem = null;
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
            _m_lItemList = new List<GGUIWndHeroBlessNullableContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        /// <param name="_starSkillIdList"></param>
        public void showItemList(List<long> _idList)
        {
            if (wnd == null || _idList == null || _m_lItemList == null)
                return;

            bool isSelect = false;
            GGUIWndHeroBlessNullableContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < wnd.needShowItemCount; i++)
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

                long consortId = 0;
                if (_idList.Count > count)
                    consortId = _idList[count];

                itemWnd.showWnd();
                itemWnd.setInfo(consortId);
                //默认选中第一个
                if (!isSelect)
                {
                    isSelect = true;
                    itemWnd.setSelect(true);
                    _onClickItem(itemWnd);
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
        }

        //点击item事件
        private void _onClickItem(GGUIWndHeroBlessNullableContainerItem _item)
        {
            //如果点击的item是当前选中的，则不进行后续操作
            if(_item == null || _m_wCurSelectItem != null && _m_wCurSelectItem.consortId  == _item.consortId)
                return;

            //设置选中
            _m_wCurSelectItem?.setSelect(false);
            _m_wCurSelectItem = _item;
            _m_wCurSelectItem?.setSelect(true);
            
            //点击回调
            _m_aOnClickItem?.Invoke(_item);
        }
    }
}
