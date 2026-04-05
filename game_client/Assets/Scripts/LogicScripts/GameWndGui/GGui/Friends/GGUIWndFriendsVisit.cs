using ALPackage;
using Common.NpChatObj;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 好友拜访弹窗
    public class GGUIWndFriendsVisit : _ANPGGUIBasicWnd<GGUIMonoFriendsVisit>
    {
        private static GGUIWndFriendsVisit _g_instance = new GGUIWndFriendsVisit();
        public static GGUIWndFriendsVisit instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndFriendsVisit();
                return _g_instance;
            }
        }

        protected GGUIWndFriendsVisit()
           : base(EALUIWndLayer.NORMAL)
        {
        }

        /// <summary>
        /// show 动画是否只播放一次
        /// </summary>
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        private NPCommonSimplePlayerInfo _m_playerInfo;
        //玩家形象
        private NPGGUIWndCommonShowCase _m_playerShowCaseWnd;

        private NPGGUIWndPlayerIcon _m_playerInfoWnd;

        private NPGGUIWndCommonItemContainer _m_itemContainerWnd;

        //序列号
        private long _m_iOpSerialzie;

        private bool _m_isReq;
        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoFriendsVisit.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoFriendsVisit.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.showcaseMono)
                _m_playerShowCaseWnd = new NPGGUIWndCommonShowCase(wnd.showcaseMono);
            if (null != wnd.playerInfoMono)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.playerInfoMono);

            if (null != wnd.itemContainerMono)
                _m_itemContainerWnd = new NPGGUIWndCommonItemContainer(wnd.itemContainerMono);

            ALUGUICommon.combineBtnClick(wnd.rewardBtn, _rewardBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.showDialogBtn, _showDialogBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }
        protected override void _onShowWnd()
        {
            if (null == wnd)
                return;
        }

        protected override void _onHideWnd()
        {
            _m_iOpSerialzie = ALSerializeOpMgr.next();
            _m_playerShowCaseWnd?.hideWnd();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_playerShowCaseWnd?.discard();
            _m_playerShowCaseWnd = null;

            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;

            _m_itemContainerWnd?.discard();
            _m_itemContainerWnd = null;
            _m_isReq = false;
            if (null != wnd)
            {
                ALUGUICommon.uncombineBtnClick(wnd.rewardBtn, _rewardBtnDidClick);
                ALUGUICommon.uncombineBtnClick(wnd.showDialogBtn, _showDialogBtnDidClick);
                ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);

            }
        }

        public void setPlayerInfo(NPCommonSimplePlayerInfo _playerInfo)
        {
            _m_playerInfo = _playerInfo;
            _refresh();
        }
        private void _refresh()
        {
            if (null == _m_playerInfo)
                return;

            if (null != _m_playerInfoWnd)
                _m_playerInfoWnd.setPlayerInfo(_m_playerInfo);
        }

        //展示对话按钮
        private void _showDialogBtnDidClick(GameObject _go)
        {
            if (null == wnd || null == wnd.dialogIdList || wnd.dialogIdList.Count == 0)
                return;

            ALUGUICommon.setGameObjEnable(wnd.dialogStartHideGoList, false);
            long dialogId = wnd.dialogIdList.GetRandomItem();
            GCommon.enterDialogueNode(dialogId, () =>
            {
                ALUGUICommon.setGameObjEnable(wnd.dialogEndShowGoList, true);
            });
        }

        private void _rewardBtnDidClick(GameObject _go)
        {
            if (null == wnd)
                return;

            if (_m_isReq)
                return;

            _m_isReq = true;
            _m_iOpSerialzie = ALSerializeOpMgr.next();
            long serialzie = _m_iOpSerialzie;
            Action<List<NPCommon.NPCommon_ItemInfo>> afterGetRewardAction = (_list) =>
            {
                if (null == wnd || null == _m_itemContainerWnd || serialzie != _m_iOpSerialzie)
                    return;

                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serialzie != _m_iOpSerialzie)
                        return;

                    ALUGUICommon.setGameObjEnable(wnd.getRewardShowGoList, true);
                    ALUGUICommon.setGameObjEnable(wnd.getRewardHideGoList, false);

                    for (int i = 0;i < _list.Count; i ++)
                    {
                        NPCommon.NPCommon_ItemInfo item = _list[i];
                        if (null == item)
                            continue;

                        NPGGUIWndCommonItem itemWnd = _m_itemContainerWnd.getIndexItemWnd(i);
                        if (null == itemWnd)
                            continue;

                        GCommon.showItemParticle(item, itemWnd.rectTransform,GRefdataCoreMgr.instance.npGeneral.friend_visit_particle_id);
                    }

                    if(null != wnd.rewardAni && !string.IsNullOrEmpty(wnd.particalAniStr))
                    {
                        wnd.rewardAni.ForcePlay(wnd.particalAniStr);
                    }
                }, wnd.getRewardMarginTime);
            };

            //播放avatar动作
            if (null != _m_playerShowCaseWnd && !string.IsNullOrEmpty(wnd.playerAniName))
                _m_playerShowCaseWnd.playAnim(0, wnd.playerAniName);

            //领取奖励
            NPPlayer.instance.friendsComp.reqGainVisitOtherPlayerReward((_list) =>
            {
                //清空存储的玩家id
                AccountSettingMgr.instance.accountSetting.setFriendVisitPlayerId(0);

                if (null == wnd || serialzie != _m_iOpSerialzie)
                    return;

                if (null != _m_itemContainerWnd)
                {
                    List<NPCommonCostItem> itemList = NPCommonCostItem.switchList(_list);
                    _m_itemContainerWnd.showItemList(itemList);
                }

                if (null != wnd.rewardAni && !string.IsNullOrEmpty(wnd.rewardAniStr))
                {
                    wnd.rewardAni.Play(wnd.rewardAniStr, () =>
                    {
                        afterGetRewardAction(_list);
                    });
                }
                else
                {
                    afterGetRewardAction(_list);
                }
            }, () => {
                _m_isReq = false;
            });
        }



        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Friends.C_ADD_FRIEND_VISIT_NODE);
        }
    }



}
