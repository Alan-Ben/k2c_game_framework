using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 好友申请界面 容器item
    public class GGUIWndRequestFriendsGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoRequestFriendsGridItem>
    {
        //好友申请数据
        private PlayerFriendApplyItemData _m_itemData;

        //头像
        private NPGGUIWndPlayerIcon _m_playerIconWnd;

        //刷新操作序列号
        private long _m_lRefreshSerialize;
        public GGUIWndRequestFriendsGridItem(GGUIMonoRequestFriendsGridItem _wnd) : base(_wnd)
        {

        }

        protected override void _onDiscard()
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.discard();
            _m_playerIconWnd = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.agreeBtn, _agreeBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.refuseBtn, _refuseBtnDidClick);
        }

        protected override void _onHideWnd()
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
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
            if (null == wnd)
                return;

            if (null != wnd.playerIconMono)
                _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerIconMono);

            ALUGUICommon.combineBtnClick(wnd.agreeBtn, _agreeBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.refuseBtn, _refuseBtnDidClick);
        }

        //设置数据
        public void setItem(PlayerFriendApplyItem _item)
        {
            if (null == _item)
                return;

            //刷新序列号
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            long opSerialize = _m_lRefreshSerialize;
            _item.getValue(_itemData =>
            {
                if (null == _itemData)
                    return;

                //判断操作序列号是否一致
                if (opSerialize != _m_lRefreshSerialize)
                    return;

                _m_itemData = _itemData;
                _refresh();
            });
        }

        //刷新
        private void _refresh()
        {
            if (null == wnd || null == _m_itemData)
                return;

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.setPlayerInfo(_m_itemData.playerInfo);

            bool isOnline = _m_itemData.playerInfo.isOnline;
            ALUGUICommon.setGameObjEnable(wnd.onlineShow, isOnline);
            ALUGUICommon.setGameObjEnable(wnd.onlineHide, !isOnline);
            ALUGUICommon.setLabelTxt(wnd.offLineTxt, TextTranslate.instance.getLanguage(
                TransKeyConst.friends_offline_time_str,
                TimeUtil.getPassTimeShow(_m_itemData.playerInfo.lastOfflineMs)));
        }

        //同意申请
        private void _agreeBtnDidClick(GameObject _go)
        {
            _sendReq(true);
        }

        //拒绝申请
        private void _refuseBtnDidClick(GameObject _go)
        {
            _sendReq(false);
        }

        private void _sendReq(bool _isAgree)
        {
            if (null == _m_itemData)
                return;

            string keyStr = TransKeyConst.friends_agree_add_str;
            if(!_isAgree)
                keyStr = TransKeyConst.friends_refuse_add_str;

            NPPlayer.instance.friendsComp.reqDealFriendApply(_isAgree, _m_itemData.cid,()=> {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(keyStr, _m_itemData.playerInfo.name));

                if (_isAgree)
                {
                    PlayAudioMgr.instance.playClip(wnd.addSuccAudioId);
                }
            });
        }


    }
}
