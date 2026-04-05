using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 添加好友主界面
    public class GGUIWndFriendsMain : _ANPGGUIBasicWnd<GGUIMonoFriendsMain>
    {

        private static GGUIWndFriendsMain _g_instance;
        public static GGUIWndFriendsMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndFriendsMain();
                return _g_instance;
            }
        }

        private GGUIWndFriendsMainGrid _m_gridWnd;

        protected GGUIWndFriendsMain()
           : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoFriendsMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFriendsMain.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.gridMono)
                _m_gridWnd = new GGUIWndFriendsMainGrid(wnd.gridMono);

            ALUGUICommon.combineBtnClick(wnd.addFriendBtn, _addFriendBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.friendRequestBtn, _friendRequestBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.blockListBtn, _blockListBtnDidClick);
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

            ALUGUICommon.uncombineBtnClick(wnd.addFriendBtn, _addFriendBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.friendRequestBtn, _friendRequestBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.blockListBtn, _blockListBtnDidClick);
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        private void _addFriendBtnDidClick(GameObject _go)
        {
            FriendCommon.showAddFriend();
        }

        /// <summary>
        /// 好友申请列表
        /// </summary>
        private void _friendRequestBtnDidClick(GameObject _go)
        {
            FriendCommon.showFriendsRequest();
        }

        /// <summary>
        /// 屏蔽列表
        /// </summary>
        private void _blockListBtnDidClick(GameObject _go)
        {
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_sysUnOpen_none);
        }
    }
}
