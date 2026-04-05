using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 好友选择界面 容器item
    public class GGUIWndFriendsSelectGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoFriendsSelectGridItem>
    {
        //好友数据
        private PlayerFriendItemData _m_itemData;

        //玩家信息
        private NPGGUIWndPlayerIcon _m_playerIconWnd;

        //选择回调
        private Action<PlayerFriendItemData> _m_selectAction;
        public GGUIWndFriendsSelectGridItem(GGUIMonoFriendsSelectGridItem _wnd) : base(_wnd)
        {

        }

        protected override void _onDiscard()
        {
            if (null != _m_playerIconWnd)
                _m_playerIconWnd.discard();
            _m_playerIconWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.selectBtn, _selectBtnDidClick);
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {

        }

        //重置Grid单个对象
        protected override void _resetGridItem()
        {

        }

        protected override void _onShowWnd()
        {
            _refresh();
        }

        // 初始化
        protected override void _onWndInitDone()
        {
            if (null != wnd.playerIconMono)
                _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerIconMono);

            ALUGUICommon.combineBtnClick(wnd.selectBtn, _selectBtnDidClick);
        }

        //设置数据
        public void setItem(PlayerFriendItemData _itemData,Action<PlayerFriendItemData> _selectAction)
        {
            if (null == _itemData)
                return;

            _m_itemData = _itemData;
            _m_selectAction = _selectAction;
            _refresh();
        }

        //刷新
        private void _refresh()
        {
            if (null == wnd || null == _m_itemData)
                return;

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.setPlayerInfo(_m_itemData.playerInfo);

        }

        //选择按钮
        private void _selectBtnDidClick(GameObject _go)
        {
            if (null != _m_selectAction)
                _m_selectAction(_m_itemData);
        }

    }
}
