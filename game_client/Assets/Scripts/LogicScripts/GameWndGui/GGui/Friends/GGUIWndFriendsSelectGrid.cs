using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;

namespace GOE
{
    // 好友选择列表容器
    public class GGUIWndFriendsSelectGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoFriendsSelectGridItem, GGUIMonoFriendsSelectGrid, GGUIWndFriendsSelectGridItem>
    {
        private List<PlayerFriendItemData> _m_dataList;

        private Action<PlayerFriendItemData> _m_selectAction;

        //构造函数
        public GGUIWndFriendsSelectGrid(GGUIMonoFriendsSelectGrid _containerMono, Action<PlayerFriendItemData> _selectAction) : base(_containerMono)
        {
            _m_dataList = new List<PlayerFriendItemData>();
            _m_selectAction = _selectAction;
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

        }

        protected override void _onHideWnd()
        {

        }
        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_dataList.Clear();
            _m_dataList = null;
            _m_selectAction = null;
        }

        // 创建对象
        protected override GGUIWndFriendsSelectGridItem _createItemWnd(GGUIMonoFriendsSelectGridItem _itemMono)
        {
            // 创建对象
            GGUIWndFriendsSelectGridItem gridItem = new GGUIWndFriendsSelectGridItem(_itemMono);
            return gridItem;
        }

        protected override void _onRefreshItemWnd(GGUIWndFriendsSelectGridItem _itemWnd, int _itemIdx)
        {
            if (null == _itemWnd)
                return;

            if (_itemIdx >= _m_dataList.Count)
                return;

            //获取数据
            PlayerFriendItemData data = _m_dataList[_itemIdx];
            if (null == data)
                return;

            _itemWnd.setItem(data, _selectDelegate);
        }

        #endregion
         

        private void _refresh()
        {
            NPPlayer.instance.friendsComp.getFriendsDataList(_itemDataList => {
                _m_dataList.Clear();
                _m_dataList.AddRange(_itemDataList);
                _m_dataList.Sort(_getSortRule);
                setItemCount(_m_dataList.Count);
                ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, _m_dataList.Count == 0);
            });
        }

        /// <summary>
        /// 排序规则
        /// </summary>
        private int _getSortRule(PlayerFriendItemData _x, PlayerFriendItemData _y)
        {
            //繁荣度
            return _x.playerInfo.totalPower.CompareTo(_y.playerInfo.totalPower);
        }

        private void _selectDelegate(PlayerFriendItemData _itemData)
        {
            if (null != _m_selectAction)
                _m_selectAction(_itemData);
        }
    }
}
