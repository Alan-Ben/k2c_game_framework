using ALPackage;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤item列表
    /// </summary>
    public class GGUIWndPlayerSkinContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoPlayerSkinContainerItem, GGUIMonoPlayerSkinContainer, GGUIWndPlayerSkinContainerItem>
    {
        //item列表
        protected List<GGUIWndPlayerSkinContainerItem> _m_lItemList;
        //点击item
        private Action<GGUIWndPlayerSkinContainerItem> _m_aOnClickItem;
        //当前选中的item
        private GGUIWndPlayerSkinContainerItem _m_wCurSelectItem;

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndPlayerSkinContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndPlayerSkinContainer(GGUIMonoPlayerSkinContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndPlayerSkinContainerItem _createItemWnd(GGUIMonoPlayerSkinContainerItem _itemMono)
        {
            GGUIWndPlayerSkinContainerItem item = new GGUIWndPlayerSkinContainerItem(_itemMono);
            item.onSelectItem += _onClickItem;
            return item;
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onBagItemChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onBagItemChg);
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
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndPlayerSkinContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<PlayerSkinRefObj> _playerSkinRef, long _selectSkinRefId = 0)
        {
            if (wnd == null || _playerSkinRef == null || _m_lItemList == null)
                return;

            bool isSelect = false;
            GGUIWndPlayerSkinContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _playerSkinRef.Count; i++)
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
                itemWnd.setInfo(_playerSkinRef[i]);
                itemWnd.setSelect(false);
                //默认选中当前的皮肤 或者选中 指定的皮肤
                if (!isSelect &&
                    ((_selectSkinRefId <= 0 && _playerSkinRef[i].id == NPPlayer.instance.playerInfo.getCurrentSkinId()) ||
                     (_selectSkinRefId > 0 && _selectSkinRefId == _playerSkinRef[i].id)))
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
        private void _onClickItem(GGUIWndPlayerSkinContainerItem _item)
        {
            if (wnd == null || _m_wCurSelectItem != null && _item != null && _m_wCurSelectItem.playerSkinRef != null &&
                _item.playerSkinRef != null && _m_wCurSelectItem.playerSkinRef.id == _item.playerSkinRef.id)
                return;

            _m_wCurSelectItem?.setSelect(false);
            _m_wCurSelectItem = _item;
            _m_wCurSelectItem?.setSelect(true);
            _m_aOnClickItem?.Invoke(_item);

            //设置超出的item移动到里面
            GCommon.setContainerMoveItemWithinRangeInHorizontal(_item?.rectTransform, rectTransform, (RectTransform)wnd.itemContainer?.transform);
        }

        //背包道具变更
        private void _onBagItemChg(params object[] _objs)
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _m_lItemList[i]?.refreshRedTip();
            }
        }
    }
}
