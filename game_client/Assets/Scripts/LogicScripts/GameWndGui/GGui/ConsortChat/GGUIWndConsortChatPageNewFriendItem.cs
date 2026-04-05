using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndConsortChatPageNewFriendItem : _ANPGGUIBasicGridItemWnd<GGUIMonoConsortChatPageNewFriendItem>
    {
        private long _m_consortId;
        private GGUIWndConsortIconItem _m_consortCardItem;
        
        public GGUIWndConsortChatPageNewFriendItem(GGUIMonoConsortChatPageNewFriendItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_ADD_FRIEND_INDEX, _simulateClickAdd);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_ADD_FRIEND_INDEX, _simulateClickAdd);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
            if (wnd != null) ALUGUICommon.uncombineBtnClick(wnd.btnAdd, _onBtnAddClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.consortIconItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);
            ALUGUICommon.combineBtnClick(wnd.btnAdd, _onBtnAddClick);
        }
        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(long _consortId)
        {
            _m_consortId = _consortId;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            
            _m_consortCardItem?.showWnd();
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortId);
            _m_consortCardItem?.setInfo(consortInfo, 0); 
            bool hasAdded = NPPlayer.instance.consortChatComp.hasAddConsortFriend(_m_consortId);
            ALUGUICommon.setGameObjEnable(wnd.waitAddShowGos, !hasAdded);
            ALUGUICommon.setGameObjEnable(wnd.addedShowGos, hasAdded);
        }

        private void _onBtnAddClick(GameObject _o)
        {
            NPPlayer.instance.consortChatComp.reqConsortAddChatFriend(_m_consortId);
            _refreshWnd();
        }

        //模拟点击添加按钮
        private void _simulateClickAdd(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long curIndex = (long) _objects[0];
            if (curIndex == itemIdx)
                _onBtnAddClick(null);
        }
    }
}
