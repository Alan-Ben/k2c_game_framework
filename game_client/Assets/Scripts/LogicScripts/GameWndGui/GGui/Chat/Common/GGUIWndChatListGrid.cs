using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ALPackage;
using NPEnum;
using ChatPackage;

namespace GOE
{
    // 聊天私聊会话列表
    public class GGUIWndChatListGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoChatListGridItem, GGUIMonoChatListGrid, GGUIWndChatListGridItem>
    {
        //数据列表
        private List<_INPChatInfo> _m_infoList;
        private List<_INPChatInfo> _m_selectedList;
        private bool _m_isSelectedTogOn;

        //item 点击事件
        public event Action<_INPChatInfo> itemClickAction;

        public GGUIWndChatListGrid(GGUIMonoChatListGrid _gridMono) : base(_gridMono)
        {
            _m_infoList = new List<_INPChatInfo>();
            _m_selectedList = new List<_INPChatInfo>();
            initWnd();
        }


        protected override void _onDiscard()
        {
            _m_infoList.Clear();
            _m_selectedList.Clear();
            itemClickAction = null;
            _m_isSelectedTogOn = false;
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_ALL, _onSelectedAll);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_DIS_SELECT_ALL, _onDisSelectedAll);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_ON, _onSelectedTogOn);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_OFF, _onSelectedTogOff);
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_ALL, _onSelectedAll);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_DIS_SELECT_ALL, _onDisSelectedAll);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_ON, _onSelectedTogOn);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_CHANNEL_SELECT_TOG_OFF, _onSelectedTogOff);
        }

        protected override void _onWndInitDone()
        {
        }
        protected override GGUIWndChatListGridItem _createItemWnd(GGUIMonoChatListGridItem _itemMono)
        {
            // 创建对象
            GGUIWndChatListGridItem gridItem = new GGUIWndChatListGridItem(_itemMono, _itemDidClickDelegate, wnd == null ? null : wnd.scrollRect);
            gridItem.onSelectedItem += _onSelectedItem;
            gridItem.onDisSelectedItem += _onDisSelectedItem;
            return gridItem;
        }

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="_info"></param>
        private void _onSelectedItem(_INPChatInfo _info)
        {
            if (!_m_selectedList.Contains(_info))
            {
                _m_selectedList.Add(_info);
            }
        }

        /// <summary>
        /// 取消选中
        /// </summary>
        /// <param name="_info"></param>
        private void _onDisSelectedItem(_INPChatInfo _info)
        {
            if (_m_selectedList.Contains(_info))
            {
                _m_selectedList.Remove(_info);
            }
        }
        
        /// <summary>
        /// 选中所有
        /// </summary>
        private void _onSelectedAll()
        {
            _m_selectedList.Clear();
            _m_selectedList.AddRange(_m_infoList);
            forceRefreshAllItem();
        }

        /// <summary>
        /// 取消所有选中
        /// </summary>
        private void _onDisSelectedAll()
        {
            _m_selectedList.Clear();
            forceRefreshAllItem();
        }

        private void _onSelectedTogOn()
        {
            _m_isSelectedTogOn = true;
            forceRefreshAllItem();
        }

        private void _onSelectedTogOff()
        {
            _m_isSelectedTogOn = false;
            forceRefreshAllItem();
        }

        /// <summary>
        /// 刷新当个数据显示
        /// </summary>
        /// <param name="_itemWnd"></param>
        /// <param name="_itemIdx"></param>
        protected override void _refreshItemwnd(GGUIWndChatListGridItem _itemWnd, int _itemIdx)
        {
            if (null == _m_infoList || _itemIdx >= _m_infoList.Count)
                return;

            //获取数据对象
            _INPChatInfo showData = _m_infoList[_itemIdx];
            if (null == showData)
                return;

            //刷新物品UI
            _itemWnd.setInfo(showData);
            _itemWnd.setSelectedTogOn(_m_isSelectedTogOn);
            _itemWnd.setSelected(_m_selectedList.Contains(showData));
        }

        public void refresh(List<_INPChatInfo> infoList)
        {
            if (null == infoList)
                return;
            _m_infoList.Clear();
            _m_infoList.AddRange(infoList);
            _m_selectedList?.Clear();
            _m_isSelectedTogOn = false;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            setItemCount(_m_infoList.Count);
            
            ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, _m_infoList.Count == 0);
            forceRefreshAllItem();
        }

        //item 点击事件
        private void _itemDidClickDelegate(_INPChatInfo _info)
        {
            if (null != itemClickAction)
                itemClickAction(_info);
        }

        /// <summary>
        /// 获取选中的列表
        /// </summary>
        /// <param name="_removeList"></param>
        public void getSelectedList(List<_INPChatInfo> _removeList)
        {
            if (null == _removeList)
                return;
            _removeList.AddRange(_m_selectedList);
        }
    }
}
