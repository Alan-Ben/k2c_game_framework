using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;

namespace GOE
{
    // 申请列表容器
    public class GGUIWndRequestFriendsGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoRequestFriendsGridItem, GGUIMonoRequestFriendsGrid, GGUIWndRequestFriendsGridItem>
    {
        private List<PlayerFriendApplyItem> _m_dataList;

        //构造函数
        public GGUIWndRequestFriendsGrid(GGUIMonoRequestFriendsGrid _containerMono) : base(_containerMono)
        {
            _m_dataList = new List<PlayerFriendApplyItem>();
            initWnd();
        }

        #region override方法
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
        }

        protected override void _onShowWnd()
        {
            if (null == wnd)
                return;

            _refresh();

            ALUGUICommon.combineBtnClick(wnd.allAgreeBtn, _allAgreeBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.allRefuseBtn, _allRefuseBtnDidClick);

            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_APPLY_CHG, _refresh);
        }

        protected override void _onHideWnd()
        {
            ALUGUICommon.uncombineBtnClick(wnd.allAgreeBtn, _allAgreeBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.allRefuseBtn, _allRefuseBtnDidClick);

            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_APPLY_CHG, _refresh);
        }
        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_dataList.Clear();
            _m_dataList = null;
        }

        // 创建对象
        protected override GGUIWndRequestFriendsGridItem _createItemWnd(GGUIMonoRequestFriendsGridItem _itemMono)
        {
            // 创建对象
            GGUIWndRequestFriendsGridItem gridItem = new GGUIWndRequestFriendsGridItem(_itemMono);
            return gridItem;
        }

        protected override void _onRefreshItemWnd(GGUIWndRequestFriendsGridItem _itemWnd, int _itemIdx)
        {
            if (null == _itemWnd)
                return;

            if (_itemIdx >= _m_dataList.Count)
                return;

            //获取数据
            PlayerFriendApplyItem data = _m_dataList[_itemIdx];
            if (null == data)
                return;

            _itemWnd.setItem(data);
        }

        #endregion

        private void _refresh()
        {
            _m_dataList.Clear();

            NPPlayer.instance.friendsComp.getApplyList(_m_dataList);

            _m_dataList.Sort(_getSortRule);

            setItemCount(_m_dataList.Count);

            ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, _m_dataList.Count == 0);
            ALUGUICommon.setGameObjEnable(wnd.allBtnShowGoList, _m_dataList.Count >= wnd.showAllBtnMinNum);

            ALUGUICommon.setLabelTxt(wnd.requestNumTxt,TextTranslate.instance.getLanguage(TransKeyConst.friends_request_count_num_num, NPPlayer.instance.friendsComp.getApplyListCount(), GRefdataCoreMgr.instance.npGeneral.friend_apply_limit));
        }

        /// <summary>
        /// 排序规则
        /// </summary>
        private int _getSortRule(PlayerFriendApplyItem _x, PlayerFriendApplyItem _y)
        {
            //申请时间
            return  _x.applyTimeS.CompareTo(_y.applyTimeS);
        }

        /// <summary>
        /// 一键同意
        /// </summary>
        private void _allAgreeBtnDidClick(GameObject _go)
        {
            PlayerFriendApplyItem temp = null;
            int count = _m_dataList.Count;
            for (int i = 0; i < _m_dataList.Count; i++)
            {
                temp = _m_dataList[i];
                if (null == temp)
                    continue;

                int tempI = i;
                NPPlayer.instance.friendsComp.reqDealFriendApply(true, temp.cid, () =>
                {
                    if (tempI == count - 1)
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_allAgree_none);

                });
            }
        }

        /// <summary>
        /// 一键拒绝
        /// </summary>
        private void _allRefuseBtnDidClick(GameObject _go)
        {
            //二次确认弹窗
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.friends_refuseContent_none)
                , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                , null
                , TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                , () =>
                {
                    PlayerFriendApplyItem temp = null;
                    for (int i = 0; i < _m_dataList.Count; i++)
                    {
                        temp = _m_dataList[i];
                        if (null == temp)
                            continue;

                        NPPlayer.instance.friendsComp.reqDealFriendApply(false, temp.cid);
                    }

                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_allRefuse_none);
                });

        }
    }
}
