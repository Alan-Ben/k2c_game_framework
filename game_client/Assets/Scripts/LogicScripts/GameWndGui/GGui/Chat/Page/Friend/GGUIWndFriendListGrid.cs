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
    public class GGUIWndFriendListGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoFriendListGridItem, GGUIMonoFriendListGrid, GGUIWndFriendListGridItem>
    {
        //数据列表
        private List<PlayerFriendItemData> _m_infoList;
        private List<PlayerFriendGroup> _m_groupList;
        private List<FriendGroupBarController> _m_barControllerList;
        private long _m_groupBarPathId = 1314;//bar的资源id
        private List<long> _m_aniShowItemList;
        private List<long> _m_aniHideItemList;
        private List<PlayerFriendItemData> _m_allFriendItemList;

        public GGUIWndFriendListGrid(GGUIMonoFriendListGrid _gridMono) : base(_gridMono)
        {
            _m_allFriendItemList = new List<PlayerFriendItemData>();
            _m_infoList = new List<PlayerFriendItemData>();
            _m_groupList = new List<PlayerFriendGroup>();
            _m_barControllerList = new List<FriendGroupBarController>();
            _m_aniShowItemList = new List<long>();
            _m_aniHideItemList = new List<long>();
            initWnd();
        }


        protected override void _onDiscard()
        {
            _m_allFriendItemList?.Clear();
            _m_infoList.Clear();
            _m_groupList.Clear();
            _clearTypeBars();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_GROUP_CHG, setInfo);
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_CHG,_onRefreshFriend);
            _m_aniShowItemList.Clear();
            _m_aniHideItemList.Clear();
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
            _refreshWnd(false);
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_GROUP_CHG, setInfo);
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_CHG,_onRefreshFriend);
        }

        protected override void _onWndInitDone()
        {
        }
        protected override GGUIWndFriendListGridItem _createItemWnd(GGUIMonoFriendListGridItem _itemMono)
        {
            // 创建对象
            GGUIWndFriendListGridItem gridItem = new GGUIWndFriendListGridItem(_itemMono);
            return gridItem;
        }
        
        /// <summary>
        /// 刷新当个数据显示
        /// </summary>
        /// <param name="_itemWnd"></param>
        /// <param name="_itemIdx"></param>
        protected override void _refreshItemwnd(GGUIWndFriendListGridItem _itemWnd, int _itemIdx)
        {
            if (null == _m_infoList || _itemIdx >= _m_infoList.Count || _itemIdx < 0)
                return;

            //获取数据对象
            PlayerFriendItemData showData = _m_infoList[_itemIdx];
            if (null == showData)
                return;

            //刷新物品UI
            _itemWnd.setInfo(showData);
            if (_m_aniShowItemList.Contains(_itemWnd.info.cid))
            {
                _itemWnd.dealShowAni();
            }
            else
            {
                _itemWnd.dealShowWndAni();
            }
        }

        public void setInfo()
        {
            _m_groupList?.Clear();
            NPPlayer.instance.friendsComp.getFriendGroupList(_m_groupList);
            _addTypeBars(_m_groupList);
            _refreshWnd(true);
        }

        private void _onRefreshFriend()
        {
            _refreshWnd(false);
        }
        
        private void _addTypeBars(List<PlayerFriendGroup> _groupList)
        {
            FriendGroupBarController barWnd;
            for (int i = 0; i < _groupList.Count; i++)
            {
                if (_m_barControllerList.Count > i)
                {
                    barWnd = _m_barControllerList[i];
                }
                else
                {
                    barWnd = new FriendGroupBarController(_m_groupBarPathId, wnd.gridAreaUIObj, _onBarSelected);
                    _m_barControllerList.Add(barWnd);
                    addBar(barWnd);
                }

                barWnd.reset();//先重置，显示默认
                barWnd.setBarData(_groupList[i]);
            }
            
            
            //移除多余bar
            if (_m_barControllerList.Count > _groupList.Count)
            {
                for (int i = _groupList.Count; i < _m_barControllerList.Count; i++)
                {
                    removeBar(_m_barControllerList[i]);
                    _m_barControllerList[i].discard();
                }

                _m_barControllerList.RemoveRange(_groupList.Count, _m_barControllerList.Count - _groupList.Count);
            }
            
        }

        /// <summary>
        /// bar选中装填变化
        /// </summary>
        /// <param name="_obj"></param>
        private void _onBarSelected(FriendGroupBarController _obj)
        {
            PlayerFriendGroup group =_m_groupList.Find((b) =>
            {
                return b.groupIndex == _obj.groupIdx;
            });
            if (_obj.isShowItem)//记录需要显示show动画的item，然后刷新列表
            {
                _m_aniShowItemList.Clear();
                if (null != group)
                {
                    _m_aniShowItemList.AddRange(group.playerList);
                }
                
                _refreshWnd(false);
                
                ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                {
                    _m_aniShowItemList.Clear();
                });
            }
            else // 先播放对应的隐藏动画再刷新列表
            {
                _m_aniHideItemList.Clear();
                if (null != group)
                {
                    _m_aniHideItemList.AddRange(group.playerList);
                }

                List<GGUIWndFriendListGridItem> hideItemList = new List<GGUIWndFriendListGridItem>();
                refreshAllItem((_itemWnd, _idx) =>
                {
                    if (null == _itemWnd.info)
                        return;
                    if (_m_aniHideItemList.Contains(_itemWnd.info.cid))
                    {
                        hideItemList.Add(_itemWnd);
                    }
                });

                ALStepCounter hideItemStepCounter = new ALStepCounter();
                hideItemStepCounter.chgTotalStepCount(hideItemList.Count);
                hideItemStepCounter.regAllDoneDelegate(() =>
                {
                    _refreshWnd(false);
                    _m_aniHideItemList.Clear();
                });
                
                foreach (GGUIWndFriendListGridItem item in hideItemList)
                {
                    item.dealHideAni(hideItemStepCounter.addDoneStepCount);
                }
            }
        }
        
    
        private FriendGroupBarController _getTypeBar(long _cid)
        {
            PlayerFriendGroup group =_m_groupList.Find((b) =>
            {
                return b.playerList.Contains(_cid);
            });
            if (null != group)
            {
                return _getTypeBar(group.groupIndex);
            }
            return null;
        }
        private FriendGroupBarController _getTypeBar(int _groupIndex)
        {
            foreach (FriendGroupBarController typeBar in _m_barControllerList)
            {
                if (typeBar.groupIdx == _groupIndex)
                    return typeBar;
            }
            return null;
        }

        private void _clearTypeBars()
        {
            if (null != _m_barControllerList)
            {
                foreach (FriendGroupBarController groupBarController in _m_barControllerList)
                {
                    removeBar(groupBarController);
                    groupBarController.discard();
                }
                _m_barControllerList.Clear();
            }
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd(bool _isForce)
        {
            _getItemListWithBar(_isForce,() =>
            {
                if (wnd == null || _m_infoList == null || _m_allFriendItemList == null)
                    return;

                _m_infoList.Sort(_getTypeSort);
            
                setItemCount(_m_infoList.Count);
                _refreshBarIndex();
                forceRefreshBar();
                
                ALUGUICommon.setGameObjEnable(wnd.emptyShowList, _m_allFriendItemList.Count == 0);
            });
        }
        private void _refreshBarIndex()
        {
            int count = 0;
            FriendGroupBarController typeBar = null;
            for (int i=0;i< _m_barControllerList.Count;i++)
            {
                typeBar = _m_barControllerList[i];
                if (null == typeBar)
                    continue;
                typeBar.setInsertIndex(count);
                count += typeBar.isShowItem ? _m_groupList[i].playerList.Count : 0;
            }
        }
        
        /// <summary>
        /// 根据页签获取对应数据
        /// </summary>
        /// <param name="_tabType"></param>
        /// <summary>
        public void _getItemListWithBar(bool _isForce,Action _dealDone)
        {
            if (null == _m_infoList)
                return;
            //取出数据库的数据
            NPPlayer.instance.friendsComp.getFriendsDataList((_list) =>
            {
                _m_allFriendItemList.Clear();
                _m_allFriendItemList.AddRange(_list);
                _m_infoList.Clear();
                FriendGroupBarController _typeBar = null;
                //转成展示数据
                for (int i = 0; i < _m_allFriendItemList.Count; i++)
                {
                    PlayerFriendItemData item = _m_allFriendItemList[i];
                    if (null == item)
                        continue;

                    _typeBar = _getTypeBar(item.cid);
                    if (null == _typeBar || !_typeBar.isShowItem) //是否显示，显示才把item加入显示列表
                        continue;
                
                    _m_infoList.Add(item);
                }
                _dealDone?.Invoke();
            },_isForce);

        }
        
        /// <summary>
        /// 根据分组排序
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        private int _getTypeSort(PlayerFriendItemData _x, PlayerFriendItemData _y)
        {
            int _xIndex=  _m_groupList.FindIndex((group) =>
            {
                return group.playerList.Contains(_x.cid);
            });
            
            int _yIndex=  _m_groupList.FindIndex((group) =>
            {
                return group.playerList.Contains(_y.cid);
            });
            if (_xIndex < _yIndex)
                return -1;
            if (_xIndex > _yIndex)
                return 1;            
            return _getSort(_x,_y);
        }

        /// <summary>
        /// 同组内的好友排序
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        private int _getSort(PlayerFriendItemData _x, PlayerFriendItemData _y)
        {
            if (_x.playerInfo.isOnline && !_y.playerInfo.isOnline)
                return -1;
            if (!_x.playerInfo.isOnline && _y.playerInfo.isOnline)
                return 1;
            if (_x.playerInfo.isOnline && _y.playerInfo.isOnline)
            {
                if (_x.playerInfo.totalPower > _y.playerInfo.totalPower)
                    return -1;
                if (_x.playerInfo.totalPower < _y.playerInfo.totalPower)
                    return 1;
                if (_x.playerInfo.lastOnlineMs > _y.playerInfo.lastOnlineMs)
                    return 1;
                if (_x.playerInfo.lastOnlineMs < _y.playerInfo.lastOnlineMs)
                    return -1;
            }

            if (!_x.playerInfo.isOnline && !_y.playerInfo.isOnline)
            {
                if (_x.playerInfo.lastOfflineMs > _y.playerInfo.lastOfflineMs)
                    return -1;
                if (_x.playerInfo.lastOfflineMs < _y.playerInfo.lastOfflineMs)
                    return 1;
                if (_x.playerInfo.totalPower > _y.playerInfo.totalPower)
                    return -1;
                if (_x.playerInfo.totalPower < _y.playerInfo.totalPower)
                    return 1;
            }
            
            return _x.cid.CompareTo(_y.cid);
        }
    }
}
