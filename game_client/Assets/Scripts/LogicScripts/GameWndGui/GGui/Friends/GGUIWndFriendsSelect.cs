using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    // 好友申请列表
    public class GGUIWndFriendsSelect : _ANPGGUIBasicWnd<GGUIMonoFriendsSelect>
    {

        private static GGUIWndFriendsSelect _g_instance = new GGUIWndFriendsSelect();
        public static GGUIWndFriendsSelect instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndFriendsSelect();
                return _g_instance;
            }
        }

        private GGUIWndFriendsSelectGrid _m_gridWnd;

        private Action<PlayerFriendItemData> _m_selectAction;

        protected GGUIWndFriendsSelect()
           : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoFriendsSelect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFriendsSelect.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        #region override
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.gridMono)
                _m_gridWnd = new GGUIWndFriendsSelectGrid(wnd.gridMono, _selectDelegate);

            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }
        protected override void _onShowWnd()
        {
            if (null != _m_gridWnd)
                _m_gridWnd.showWnd();
        }

        protected override void _onHideWnd()
        {
            if (null != _m_gridWnd)
                _m_gridWnd.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_gridWnd)
                _m_gridWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_gridWnd)
                _m_gridWnd.discard();
            _m_gridWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }

        #endregion

        public void setSelectAction(Action<PlayerFriendItemData> _selectAction)
        {
            _m_selectAction = _selectAction;
        }

        private void _selectDelegate(PlayerFriendItemData _itemData)
        {
            if (null != _m_selectAction)
                _m_selectAction(_itemData);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Friends.C_ADD_FRIEND_SELECT_NODE);
        }

        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Friends.C_ADD_FRIEND_SELECT_NODE);
        }
    }


    
}
