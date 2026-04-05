using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndFriendListPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoFriendListPage>
    {
        
        private GGUIWndFriendListGrid _m_friendListGrid;
        private GGUIWndFriendGroupSortItemContainer _m_groupSortItemContainer;

        public GGUIWndFriendListPage(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoFriendListPage.assetPath; }
        protected override string _monoObjName { get => GGUIMonoFriendListPage.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_CHG,_onRefreshFriend);
            
        }

        protected override void _onHideWnd()
        {
            _m_groupSortItemContainer?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_CHG,_onRefreshFriend);
        }

        protected override void _onReset()
        {
            _m_groupSortItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_friendListGrid?.discard();
            _m_friendListGrid = null;
            
            _m_groupSortItemContainer?.discard();
            _m_groupSortItemContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnAddFriend, _clickAddFriend);
            ALUGUICommon.combineBtnClick(wnd.btnShield, _clickShield);
            ALUGUICommon.combineBtnClick(wnd.btnAddGroup, _clickAddGroup);
            ALUGUICommon.combineBtnClick(wnd.btnSortGroup, _clickSortGroup);
            if (null != wnd.friendListGrid)
            {
                _m_friendListGrid = new GGUIWndFriendListGrid(wnd.friendListGrid);
            }

            if (null != wnd.groupSortItemContainer)
            {
                _m_groupSortItemContainer = new GGUIWndFriendGroupSortItemContainer(wnd.groupSortItemContainer);
                _m_groupSortItemContainer.onClickHide += _onClickHideSortContainer;
            }
        }

        /// <summary>
        /// 分组排序
        /// </summary>
        /// <param name="obj"></param>
        private void _clickSortGroup(GameObject obj)
        {
            if(null == _m_groupSortItemContainer)
                return;
            _m_groupSortItemContainer.showWnd();
            ALUGUICommon.setGameObjEnable(wnd.onSortHide,false);
            ALUGUICommon.setGameObjEnable(wnd.onSortShow,true);
        }

        /// <summary>
        /// 分组排序关闭的时候
        /// </summary>
        private void _onClickHideSortContainer()
        {
            _m_groupSortItemContainer?.hideWnd();
            ALUGUICommon.setGameObjEnable(wnd.onSortHide,true);
            ALUGUICommon.setGameObjEnable(wnd.onSortShow,false);
        }

        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="obj"></param>
        private void _clickAddGroup(GameObject obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFriendAddGruop.instance,GGUIWndFriendAddGruop.instance.showWnd);
        }

        private void _clickShield(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFriendShieldList.instance, GGUIWndFriendShieldList.instance.showWnd);
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="obj"></param>
        private void _clickAddFriend(GameObject obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFriendAdd.instance, GGUIWndFriendAdd.instance.showWnd);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if (null != _m_friendListGrid)
            {
                _m_friendListGrid.showWnd();
                _m_friendListGrid.setInfo();
            }

            ALUGUICommon.setGameObjEnable(wnd.onSortHide,true);
            ALUGUICommon.setGameObjEnable(wnd.onSortShow,false);
            _refreshFriendCount();
        }

        /// <summary>
        /// 刷新好友数量
        /// </summary>
        private void _refreshFriendCount()
        {
            if (null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtFriendCount,
                TextTranslate.instance.getLanguage(TransKeyConst.friends_count_num,
                    NPPlayer.instance.friendsComp.getFriendsListCount(),
                    NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.FRIEND_NUM)));
        }

        /// <summary>
        /// 好友变动的时候
        /// </summary>
        private void _onRefreshFriend()
        {
            _refreshFriendCount();
        }
    }
}