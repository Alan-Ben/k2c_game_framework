using System;
using System.Collections.Generic;
using ALPackage;
using Common.RankObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子入口
    /// </summary>
    public class GGUIWndConsortEnter : _ATALBasicUIWnd<GGUIMonoConsortEnter>
    {
        private static GGUIWndConsortEnter _g_instance;

        public static GGUIWndConsortEnter instance
        {
            get
            {
                if (null == _g_instance)
                {
                    _g_instance = new GGUIWndConsortEnter();
                }

                return _g_instance;
            }
        }

        //序列号
        private long _m_iOpSerialzie;

        private NPGGUIWndPlayerIcon _m_playerInfoIconWnd;

        private NPCommonSimplePlayerInfo _m_playerInfoData;

        public GGUIWndConsortEnter() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoConsortEnter.assetPath; }
        protected override string _monoObjName { get => GGUIMonoConsortEnter.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }


        protected override void _onShowWnd()
        {
            _refresh();
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_VISIT_REWARD_GET, _refresh);
        }

        protected override void _onHideWnd()
        {
            _m_iOpSerialzie = ALSerializeOpMgr.next();
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_VISIT_REWARD_GET, _refresh);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (null != _m_playerInfoIconWnd)
                _m_playerInfoIconWnd.discard();
            _m_playerInfoIconWnd = null;

            if (null != wnd)
            {
                ALUGUICommon.uncombineBtnClick(wnd.friendVisitEntry, _friendVisitEntryDidClick);
            }
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.playerInfoMono)
                _m_playerInfoIconWnd = new NPGGUIWndPlayerIcon(wnd.playerInfoMono);

            ALUGUICommon.combineBtnClick(wnd.friendVisitEntry, _friendVisitEntryDidClick);
        }

        private void _refresh()
        {
            if (null == wnd)
                return;

            bool isShowFriendVisit = false;
            //判断解锁
            if (GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.friend_visit_simple_unlock_id))
            {
                //服务端存储的上一次领取奖励时间
                long lastGainVisitRewardDay = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LAST_GAIN_VISIT_REWARD_DAY);
                //获取当前时间戳
                int timeNow = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);

                if (lastGainVisitRewardDay == 0 || lastGainVisitRewardDay != timeNow)
                    isShowFriendVisit = true;
            }

            ALUGUICommon.setGameObjEnable(wnd.friendVisitEntry, isShowFriendVisit);

            if (isShowFriendVisit)
            {
                //增加操作序列号
                _m_iOpSerialzie = ALSerializeOpMgr.next();
                long serialzie = _m_iOpSerialzie;
                _getVisitFriend((_info) =>
                {
                    if (serialzie != _m_iOpSerialzie || null == _info)
                        return;

                    if (null != _m_playerInfoIconWnd)
                        _m_playerInfoIconWnd.setPlayerInfo(_info);

                    _m_playerInfoData = _info;
                });
            }
        }

        private void _getVisitFriend(Action<NPCommonSimplePlayerInfo> _doneAction)
        {
            if (null == _doneAction)
                return;

            //获取存储的玩家信息
            long playerId = AccountSettingMgr.instance.accountSetting.friendVisitPlayerId;
            PlayerFriendItem item = null;
            if (playerId != 0 && !NPPlayer.instance.friendsComp.isShield(playerId))
            {
                item = NPPlayer.instance.friendsComp.getFriendItem(playerId);
                if (null == item)
                {
                    GCommon.reqPlayerInfo(playerId, (_info) =>
                    {
                        _doneAction(_info);
                    });
                }
                else
                {
                    item.getValue((_itemData) =>
                    {
                        if (null != _itemData)
                            _doneAction(_itemData.playerInfo);
                        else
                            _doneAction(null);
                    });
                }
            }
            else
            {
                NPPlayer.instance.friendsComp.getFriendsDataList((_itemList) =>
                {
                    if (null == _itemList)
                    {
                        _doneAction(null);
                        return;
                    }

                    //没有好友取排行榜随机
                    if (_itemList.Count == 0)
                    {
                        //过滤屏蔽好友
                        NPPlayer.instance.rankCommonComp.reqRankFixedBaseList(GRefdataCoreMgr.instance.npGeneral.power_rank_fixed_id, (_list) =>
                        {
                            if(null == _list)
                            {
                                _doneAction(null);
                                return;
                            }

                            List<Rank_BaseItem> randomlist = new List<Rank_BaseItem>();
                            List<Rank_BaseItem> baseList = _list.getBaseItemlist();
                            Rank_BaseItem temp = null;
                            for (int i = 0; i < baseList.Count; i++) 
                            {
                                temp = baseList[i];
                                if (null == temp)
                                    continue;

                                //没有被屏蔽加到随机数组
                                if (!NPPlayer.instance.friendsComp.isShield(temp.getKey()))
                                    randomlist.Add(temp);

                                if (randomlist.Count >= 10)
                                    break;
                            }

                            Rank_BaseItem item = randomlist.GetRandomItem();
                            if(null == item)
                            {
                                _doneAction(null);
                                return;
                            }

                            long playerCid = item.getKey();
                            if (playerCid == 0)
                            {
                                _doneAction(null);
                                return;
                            }
                            GCommon.reqPlayerInfo(playerCid, (_info) =>
                            {
                                AccountSettingMgr.instance.accountSetting.setFriendVisitPlayerId(_info.cid);
                                _doneAction(_info);
                            });
                        });
                        return;
                    }

                    //在线玩家列表
                    List<PlayerFriendItemData> itemList = new List<PlayerFriendItemData>();
                    PlayerFriendItemData targetItem = null;
                    PlayerFriendItemData temp = null;
                    for (int i = 0; i < _itemList.Count; i++)
                    {
                        temp = _itemList[i];
                        if (null == temp || null == temp.playerInfo)
                            continue;

                        if (temp.playerInfo.isOnline)
                            itemList.Add(temp);
                        else if (null == targetItem || temp.playerInfo.lastOfflineMs > targetItem.playerInfo.lastOfflineMs)
                            targetItem = temp;
                    }

                    if (itemList.Count > 0)
                        targetItem = itemList.GetRandomItem();

                    if (null != targetItem && null != targetItem.playerInfo)
                    {
                        AccountSettingMgr.instance.accountSetting.setFriendVisitPlayerId(targetItem.playerInfo.cid);
                        _doneAction(targetItem.playerInfo);
                    }
                    else
                        _doneAction(null);
                });
            }
        }

        /// <summary>
        /// 展示庭院拜访
        /// </summary>
        private void _friendVisitEntryDidClick(GameObject _go)
        {
            if (null == wnd || null == _m_playerInfoData)
                return;

            QueueMgr.instance.AddNode(new GFriendVisitNode(_m_playerInfoData));
        }
    }
}
