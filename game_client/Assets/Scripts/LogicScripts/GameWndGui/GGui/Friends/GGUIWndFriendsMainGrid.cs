using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;

namespace GOE
{
    // 好友列表容器
    public class GGUIWndFriendsMainGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoFriendsMainGridItem, GGUIMonoFriendsMainGrid, GGUIWndFriendsMainGridItem>
    {
        private List<PlayerFriendItemData> _m_dataList;

        //构造函数
        public GGUIWndFriendsMainGrid(GGUIMonoFriendsMainGrid _containerMono) : base(_containerMono)
        {
            _m_dataList = new List<PlayerFriendItemData>();
            initWnd();
        }

        #region override方法
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
        }

        protected override void _onShowWnd()
        {
            if (null == wnd)
                return;

            _refresh();

            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_CHG, _refresh);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_CHG, _refresh);
        }
        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_dataList.Clear();
            _m_dataList = null;
        }

        // 创建对象
        protected override GGUIWndFriendsMainGridItem _createItemWnd(GGUIMonoFriendsMainGridItem _itemMono)
        {
            // 创建对象
            GGUIWndFriendsMainGridItem gridItem = new GGUIWndFriendsMainGridItem(_itemMono);
            return gridItem;
        }

        protected override void _onRefreshItemWnd(GGUIWndFriendsMainGridItem _itemWnd, int _itemIdx)
        {
            if (null == _itemWnd)
                return;

            if (_itemIdx >= _m_dataList.Count)
                return;

            //获取数据
            PlayerFriendItemData data = _m_dataList[_itemIdx];
            if (null == data)
                return;

            _itemWnd.setItem(data);
        }

        #endregion

        private void _refresh()
        {
            _m_dataList.Clear();

            NPPlayer.instance.friendsComp.getFriendsDataList(_itemDataList => {
                _m_dataList.Clear();
                _m_dataList.AddRange(_itemDataList);
                _m_dataList.Sort(_getSortRule);
                setItemCount(_m_dataList.Count);
                ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, _m_dataList.Count == 0);
            },true);
            
            //设置好友数量
            ALUGUICommon.setLabelTxt(wnd.friendNumTxt, NPPlayer.instance.friendsComp.getFriendsListCount() + "/" + NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.FRIEND_NUM));
        }

        /// <summary>
        /// 排序规则
        /// </summary>
        private int _getSortRule(PlayerFriendItemData _x, PlayerFriendItemData _y)
        {
            int res = 0;

            //在线状态
            res = _x.playerInfo.isOnline.CompareTo(_y.playerInfo.isOnline);
            if (res != 0)
                return -res;

            //离线时间戳
            if (!_x.playerInfo.isOnline && !_y.playerInfo.isOnline)
            {
                res = _x.playerInfo.lastOfflineMs.CompareTo(_y.playerInfo.lastOfflineMs);
                if (res != 0)
                    return -res;
            }

            //繁荣度
            return _x.playerInfo.totalPower.CompareTo(_y.playerInfo.totalPower);
        }
    }
}
