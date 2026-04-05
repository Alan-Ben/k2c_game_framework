using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 好友界面 容器item
    public class GGUIWndFriendsMainGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoFriendsMainGridItem>
    {
        //好友数据
        private PlayerFriendItemData _m_itemData;

        //头像
        private NPGGUIWndPlayerIcon _m_playerIconWnd;

        public GGUIWndFriendsMainGridItem(GGUIMonoFriendsMainGridItem _wnd) : base(_wnd)
        {

        }

        protected override void _onDiscard()
        {
            if (null != _m_playerIconWnd)
                _m_playerIconWnd.discard();
            _m_playerIconWnd = null;
        }

        protected override void _onHideWnd()
        {
            ALUGUICommon.uncombineBtnClick(wnd.chatBtn, _chatBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.moreBtn, _moreBtnDidClick);
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

            ALUGUICommon.combineBtnClick(wnd.chatBtn, _chatBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.moreBtn, _moreBtnDidClick);
        }

        // 初始化
        protected override void _onWndInitDone()
        {
            if (null != wnd.playerIconMono)
                _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerIconMono);
        }

        //设置数据
        public void setItem(PlayerFriendItemData _item)
        {
            if (null == _item)
                return;

            _m_itemData = _item;
            _refresh();
        }

        //刷新
        private void _refresh()
        {
            if (null == wnd || null == _m_itemData)
                return;

            if (null != _m_playerIconWnd)
            {
                _m_playerIconWnd.setPlayerInfo(_m_itemData.playerInfo);
            }

            ALUGUICommon.setGameObjEnable(wnd.onlineShowGoList, _m_itemData.playerInfo.isOnline);
            ALUGUICommon.setGameObjEnable(wnd.offlineShowGoList, !_m_itemData.playerInfo.isOnline);
            ALUGUICommon.setLabelTxt(wnd.lastOnlineTxt, getMarginTime(_m_itemData.playerInfo.lastOfflineMs));
        }

        //更多按钮
        private void _moreBtnDidClick(GameObject _go)
        {
            if (null == _m_itemData || null == _m_itemData.playerInfo)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFriendsMainGridItemTip.instance, () =>
            {
                GGUIWndFriendsMainGridItemTip.instance.showWnd(_m_itemData, wnd.moreTipFollowGo, wnd.moreTipInterval);

            }, EUIQueueStageType.MAIN, UINodeTagConst_Friends.C_ADD_FRIEND_BTNS_TIP, true, false, false);
        }

        //聊天按钮
        private void _chatBtnDidClick(GameObject _go)
        {
            if (null == _m_itemData)
                return;

            GCommon.jumpToPrivateChat(_m_itemData.cid);
        }

        /// <summary>
        /// 获取间隔时间
        /// </summary>
        /// <returns></returns>
        private string getMarginTime(long _lastOffLineTimeMs)
        {
            int nowDay = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);
            int offLineDay = TimeUtil.getTimeByYYYYMM(_lastOffLineTimeMs);
            int marginDay = nowDay - offLineDay;
            if (marginDay > 30)
                return TextTranslate.instance.getLanguage(TransKeyConst.friends_max_day_num, 30);
            else
                return TextTranslate.instance.getLanguage(TransKeyConst.friends_lastLoginTime_str, TimeUtil.millisecondsToTime_Max(FpsAndPingMgr.instance.serverTimeTag - _lastOffLineTimeMs));

        }

    }
}
