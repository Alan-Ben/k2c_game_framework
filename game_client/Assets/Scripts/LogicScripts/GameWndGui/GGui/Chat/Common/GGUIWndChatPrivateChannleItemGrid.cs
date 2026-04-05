using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;
using ChatPackage;

namespace GOE
{
    // 聊天私聊会话列表
    public class GGUIWndChatPrivateChannleItemGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoChatPrivateChannleItem, GGUIMonoChatPrivateChannleItemGrid, GGUIWndChatPrivateChannleItem>
    {
        //数据列表
        private List<_INPChatInfo> _m_infoList;

        //item 点击事件
        public event Action<_INPChatInfo> itemClickAction;
        private _INPChatInfo _m_curSelectedInfo;

        public GGUIWndChatPrivateChannleItemGrid(GGUIMonoChatPrivateChannleItemGrid _gridMono) : base(_gridMono)
        {
            _m_infoList = new List<_INPChatInfo>();
            initWnd();
        }


        protected override void _onDiscard()
        {
            _m_infoList.Clear();
            _m_curSelectedInfo = null;
            itemClickAction = null;
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        protected override GGUIWndChatPrivateChannleItem _createItemWnd(GGUIMonoChatPrivateChannleItem _itemMono)
        {
            // 创建对象
            GGUIWndChatPrivateChannleItem gridItem = new GGUIWndChatPrivateChannleItem(_itemMono, _itemDidClickDelegate);

            return gridItem;
        }

        /// <summary>
        /// 刷新当个数据显示
        /// </summary>
        /// <param name="_itemWnd"></param>
        /// <param name="_itemIdx"></param>
        protected override void _refreshItemwnd(GGUIWndChatPrivateChannleItem _itemWnd, int _itemIdx)
        {
            if (null == _m_infoList || _itemIdx >= _m_infoList.Count)
                return;

            //获取数据对象
            _INPChatInfo showData = _m_infoList[_itemIdx];
            if (null == showData)
                return;

            //刷新物品UI
            _itemWnd.setInfo(showData);
            _itemWnd.setSelected(_m_curSelectedInfo == showData);
        }

        public void refresh(List<_INPChatInfo> infoList)
        {
            if (null == infoList)
                return;
            _m_infoList.Clear();
            _m_infoList.AddRange(infoList);
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            setItemCount(_m_infoList.Count);
            forceRefreshAllItem();
        }

        //item 点击事件
        private void _itemDidClickDelegate(_INPChatInfo _info)
        {
            if (null != itemClickAction)
                itemClickAction(_info);
        }
        

        /// <summary>
        /// 外部调用设置选中
        /// </summary>
        /// <param name="_itemWnd"></param>
        public void setSelectItem(_INPChatInfo _info)
        {
            _m_curSelectedInfo = _info;
            forceRefreshAllItem();
            if (wnd == null)
                return;

            int index = _m_infoList?.IndexOf(_info) ?? -1;
            if (index < 0)
                return;
            
            MoveIfCantSeeItem(index, EScrollToItemType.NearestEdge);
        }
    }
}
