using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会好友邀请item容器
    /// </summary>
    public class GGUIWndDinnerInviteFriendItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoDinnerInviteFriendItem,GGUIMonoDinnerInviteFriendItemGrid,GGUIWndDinnerInviteFriendItem>
    {
        private List<DinnerInviteInfo> _m_itemDataList;
        private GDinnerInfo _m_dinnerInfo;

        public GGUIWndDinnerInviteFriendItemGrid(GGUIMonoDinnerInviteFriendItemGrid _gridMono) : base(_gridMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_itemDataList.Clear();
            _m_itemDataList = null;

            _m_dinnerInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_itemDataList = new List<DinnerInviteInfo>();
        }

        protected override GGUIWndDinnerInviteFriendItem _createItemWnd(GGUIMonoDinnerInviteFriendItem _itemMono)
        {
            GGUIWndDinnerInviteFriendItem itemWnd = new GGUIWndDinnerInviteFriendItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(GDinnerInfo _dinnerInfo, List<DinnerInviteInfo> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_dinnerInfo = _dinnerInfo;
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList); 
            // _m_itemDataList.Sort(_sortFriend);
            setItemCount(_m_itemDataList.Count);
            ALUGUICommon.setGameObjEnable(wnd.emptyShowList,_m_itemDataList.Count == 0);
            ALUGUICommon.setGameObjEnable(wnd.emptyHideList,_m_itemDataList.Count != 0);
        }

        // private int _sortFriend(DinnerInviteInfo _x, DinnerInviteInfo _y)
        // {
        //     if (_x.playerInfo.isOnline && !_y.playerInfo.isOnline)
        //         return -1;
        //     if (!_x.playerInfo.isOnline && _y.playerInfo.isOnline)
        //         return 1;
        //     if (_x.playerInfo.isOnline && _y.playerInfo.isOnline)
        //     {
        //         if (_x.playerInfo.totalPower > _y.playerInfo.totalPower)
        //             return -1;
        //         if (_x.playerInfo.totalPower < _y.playerInfo.totalPower)
        //             return 1;
        //         if (_x.playerInfo.lastOnlineMs < _y.playerInfo.lastOnlineMs)
        //             return -1;
        //         if (_x.playerInfo.lastOnlineMs > _y.playerInfo.lastOnlineMs)
        //             return 1;
        //     }
        //     if (!_x.playerInfo.isOnline && !_y.playerInfo.isOnline)
        //     {
        //         if (_x.playerInfo.lastOfflineMs > _y.playerInfo.lastOfflineMs)
        //             return -1;
        //         if (_x.playerInfo.lastOfflineMs < _y.playerInfo.lastOfflineMs)
        //             return 1;
        //         if (_x.playerInfo.totalPower > _y.playerInfo.totalPower)
        //             return -1;
        //         if (_x.playerInfo.totalPower < _y.playerInfo.totalPower)
        //             return 1;
        //     }
        //     
        //     return 0;
        // }

        /// <summary>
        /// 设置所有已邀请
        /// </summary>
        public void refresnSec()
        {
            refreshAllItem((_item, _idx) =>
            {
                _item?.refreshInviteTimeCD();
            });
        }

        protected override void _onRefreshItemWnd(GGUIWndDinnerInviteFriendItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            DinnerInviteInfo info = _m_itemDataList[_itemIdx];
            
            bool hasJoin = false;
            if (_m_dinnerInfo != null && info != null) 
                hasJoin = _m_dinnerInfo.getJoinerInfo(info.cid) != null;
            _itemWnd?.setInfo(info, hasJoin);
        }
    }
}
