using System;
using ALPackage;
using System.Collections;
using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;


//聊天频道container
public class GGUIWndFriendGroupSortItemContainer : _ATNPGGUIWndShowAnimContainer <GGUIMonoFriendGroupSortItem, GGUIMonoFriendGroupSortItemContainer, GGUIWndFriendGroupSortItem>
    {
        
        public List<GGUIWndFriendGroupSortItem> _m_lItemGroupList;//子控件列表
        private List<PlayerFriendGroup> _m_infoList;
        public event Action onClickHide;//点击确定或者取消之后的回调

        public GGUIWndFriendGroupSortItemContainer(GGUIMonoFriendGroupSortItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            showItemList();
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_GROUP_CHG, showItemList);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_GROUP_CHG, showItemList);
        }

        protected override void _onReset()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscard()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
            
            _m_infoList?.Clear();
            _m_infoList = null;

            onClickHide = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            _m_infoList = new List<PlayerFriendGroup>();
            _m_lItemGroupList = new List<GGUIWndFriendGroupSortItem>();
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _clickCancel);
            ALUGUICommon.combineBtnClick(wnd.btnComfirm, _clickComfirm);
        }

        /// <summary>
        /// 点击取消
        /// </summary>
        /// <param name="obj"></param>
        private void _clickCancel(GameObject obj)
        {
            onClickHide?.Invoke();
        }

        /// <summary>
        /// 点击确定更换排序
        /// </summary>
        /// <param name="obj"></param>
        private void _clickComfirm(GameObject obj)
        {
            //发消息保存排序
            List<long> groupList = new List<long>();
            for (int i = 0; i < _m_infoList.Count; i++)
            {
                groupList.Add(_m_infoList[i].dbId);
            }
            NPPlayer.instance.friendsComp.reqChgFriendGroupOrderList(groupList, (_info) =>
            {
                onClickHide?.Invoke();
            });
        }

        protected override GGUIWndFriendGroupSortItem _createItemWnd(GGUIMonoFriendGroupSortItem _itemMono)
        {
            GGUIWndFriendGroupSortItem itemWnd = new GGUIWndFriendGroupSortItem(_itemMono);
            itemWnd.onDrag += _dragItemWnd;
            return itemWnd;
        }

        private void showItemList()
        {
            List<PlayerFriendGroup> groupList = new List<PlayerFriendGroup>();
            NPPlayer.instance.friendsComp.getFriendGroupList(groupList);
            showItemList(groupList);
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        public void showItemList(List<PlayerFriendGroup> _infoList)
        {
            if (null == _infoList)
                return;

            _m_infoList.Clear();
            _m_infoList.AddRange(_infoList);
            _m_infoList.Sort(_sortGroup);

            //先对item按照列表展示顺序排序
            if (_m_lItemGroupList != null && _m_lItemGroupList.Count > 0)
            {
                _m_lItemGroupList.Sort((_a, _b) =>
                {
                    if(_a == null || _b == null || _a.rectTransform == null || _b.rectTransform == null)
                        return 0;

                    int aIndex = _a.rectTransform.GetSiblingIndex();
                    int bIndex = _b.rectTransform.GetSiblingIndex();
                    return aIndex.CompareTo(bIndex);
                });
            }
            
            PlayerFriendGroup tempData = null;
            GGUIWndFriendGroupSortItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _m_infoList.Count; ++i)
            {
                tempData = _m_infoList[i];
                if (tempData == null)
                    continue;
                if (count >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[count];
                }

                tempItemWnd.setInfo(tempData);
                count++;
            }
            
            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            _refreshContentLayout();
        }

        /// <summary>
        /// 分组排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int _sortGroup(PlayerFriendGroup x, PlayerFriendGroup y)
        {
            if (x.groupIndex > y.groupIndex)
                return 1;
            if (x.groupIndex < y.groupIndex)
                return -1;
            return 0;
        }


        /// <summary>
        /// 拖拽某一个item
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _dragItemWnd(GGUIWndFriendGroupSortItem _itemWnd)
        {
            PlayerFriendGroup itemInfo = _itemWnd.info;
            PlayerFriendGroup targetInfo = null;
            foreach (GGUIWndFriendGroupSortItem sortItem in _m_lItemGroupList)
            {
                if(sortItem == _itemWnd)//同一个跳过
                    continue;
                if(!sortItem.isInSwitchRect(_itemWnd))
                    continue;
                targetInfo = sortItem.info;
                _switchGroupInfoIndex(itemInfo, targetInfo);
                //在切换范围内，切换位置
                sortItem.switchWitchItemWnd(_itemWnd);
                break;
            }
        }

        /// <summary>
        /// 数据换下标位置
        /// </summary>
        /// <param name="_itemInfo"></param>
        /// <param name="_targetInfo"></param>
        private void _switchGroupInfoIndex(PlayerFriendGroup _itemInfo, PlayerFriendGroup _targetInfo)
        {
            int itemIdx = _m_infoList.IndexOf(_itemInfo);
            int targetIdx = _m_infoList.IndexOf(_targetInfo);
            _m_infoList.Remove(_itemInfo);
            _m_infoList.Remove(_targetInfo);
            //从下标小的开始插入
            if (itemIdx > targetIdx)
            {
                _m_infoList.Insert(targetIdx, _itemInfo);
                _m_infoList.Insert(itemIdx, _targetInfo);
            }
            else
            {
                _m_infoList.Insert(itemIdx, _targetInfo);
                _m_infoList.Insert(targetIdx, _itemInfo);
            }
        }

        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            });
        }
}
