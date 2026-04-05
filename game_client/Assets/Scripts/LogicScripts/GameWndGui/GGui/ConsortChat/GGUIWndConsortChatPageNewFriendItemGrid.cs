using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndConsortChatPageNewFriendItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoConsortChatPageNewFriendItem,GGUIMonoConsortChatPageNewFriendItemGrid,GGUIWndConsortChatPageNewFriendItem>
    {
        private List<long> _m_itemDataList = new List<long>();

        public GGUIWndConsortChatPageNewFriendItemGrid(GGUIMonoConsortChatPageNewFriendItemGrid gridMono) : base(gridMono)
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
            if (_m_itemDataList != null) _m_itemDataList.Clear();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndConsortChatPageNewFriendItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemMono.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndConsortChatPageNewFriendItem _createItemWnd(GGUIMonoConsortChatPageNewFriendItem _itemMono)
        {
            GGUIWndConsortChatPageNewFriendItem itemWnd = new GGUIWndConsortChatPageNewFriendItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<long> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            setItemCount(_m_itemDataList.Count);
        } //移动到顶部
        public void moveToTopNextFrame()
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(moveToTop);
        }
    }
}
