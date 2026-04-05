using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 现金礼包页签item容器
    /// </summary>
    public class GGUIWndActivityGiftPackTabContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoActivityGiftPackTabContainerItem, GGUIMonoActivityGiftPackTabContainer, GGUIWndActivityGiftPackTabContainerItem>
    {
        //item列表
        protected List<GGUIWndActivityGiftPackTabContainerItem> _m_lItemList;
        //当前选中的item
        private GGUIWndActivityGiftPackTabContainerItem _m_wCurSelectItem;
        //点击item事件
        private Action<GGUIWndActivityGiftPackTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndActivityGiftPackTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }
        /// <summary>
        /// 当前选中的item
        /// </summary>
        public GGUIWndActivityGiftPackTabContainerItem curSelectItem { get { return _m_wCurSelectItem; } }

        public GGUIWndActivityGiftPackTabContainer(GGUIMonoActivityGiftPackTabContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndActivityGiftPackTabContainerItem _createItemWnd(GGUIMonoActivityGiftPackTabContainerItem _itemMono)
        {
            GGUIWndActivityGiftPackTabContainerItem item = new GGUIWndActivityGiftPackTabContainerItem(_itemMono);
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
            _m_lItemList = new List<GGUIWndActivityGiftPackTabContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(_ABaseActivityInfo _activityInfo)
        {
            if (_activityInfo == null || _m_lItemList == null)
                return;

            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_activityInfo.activityId);
            if (activityMainRef == null)
            {
                Debug.LogError($"[GGUIWndActivityGiftPackTabContainer showItemList] activityMainRef is null, activityId:{_activityInfo.activityId}");
                return;
            }

            GGUIWndActivityGiftPackTabContainerItem itemWnd = null;
            int count = 0;


            //先添加钻石礼包页签
            if (activityMainRef.crystal_gift_pack_group_id > 0)
            {
                if (_m_lItemList.Count == 0)
                {
                    itemWnd = addItemWnd();
                    _m_lItemList.Add(itemWnd);
                }
                else
                    itemWnd = _m_lItemList[0];
                itemWnd.showWnd();
                itemWnd.setInfo(EActivityGiftPackTabType.CRYSTAL, activityMainRef.crystal_gift_pack_group_id, true, false);
                count++;
            }


            //添加现金礼包页签
            for (int i = 0; i < activityMainRef.cash_gift_pack_group_id_list.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (count >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[count];
                itemWnd.showWnd();
                itemWnd.setInfo(EActivityGiftPackTabType.CASH, activityMainRef.cash_gift_pack_group_id_list[i], false, i == (activityMainRef.cash_gift_pack_group_id_list.Count - 1));
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

            //如果当前选中的item为空，设置选中
            if (_m_wCurSelectItem == null)
            {
                //如果当前选中的item为空，则选中第一个现金礼包，否则选中第一个钻石礼包
                if (_m_lItemList.Count > 1)
                    _onClickItem(_m_lItemList[1]);
                else if (_m_lItemList.Count > 0)
                    _onClickItem(_m_lItemList[0]);
            }

            //如果只有一个页签，隐藏列表
            if (_m_lItemList == null || _m_lItemList.Count == 1)
                hideWnd();
        }

        //点击选中item
        private void _onClickItem(GGUIWndActivityGiftPackTabContainerItem _item)
        {
            if (_item == null || (_m_wCurSelectItem != null && _m_wCurSelectItem.giftPackTabType == _item.giftPackTabType && _m_wCurSelectItem.targetId == _item.targetId))
                return;

            _m_wCurSelectItem?.setSelect(false);
            _m_wCurSelectItem = _item;
            _m_wCurSelectItem.setSelect(true);

            _m_aOnClickItem?.Invoke(_m_wCurSelectItem);
        }
    }
}
