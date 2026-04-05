using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using Common.GuildObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟成员列表界面
    /// </summary>
    public class GGUIWndGuildMemberList : _ANPGGUIBasicWnd<GGUIMonoGuildMemberList>
    {
        private static GGUIWndGuildMemberList _g_instance = new GGUIWndGuildMemberList();
        public static GGUIWndGuildMemberList instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildMemberList();
                return _g_instance;
            }
        }

        //联盟成员列表
        private GGUIWndGuildMemberListGrid _m_wMemberListGrid;
        //盟主item
        private GGUIWndGuildMemberListGridItem _m_wLeaderItem;
        //弹劾倒计时
        private NPGGUIWndCommonCountDown _m_wImpeachCD;
        //展示序列号
        private long _m_lShowSerialize;

        public GGUIWndGuildMemberList() : base(EALUIWndLayer.NORMAL)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildMemberList.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildMemberList.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_EVENT_ADD, _onGuildEventChg);//联盟事件新增
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_EVENT_CHG, _onGuildEventChg);//联盟事件变更
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_EVENT_REMOVE, _onGuildEventChg);//联盟事件移除
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_MEMBER_BASE_INFO_CHG, _onMemberInfoChg);//联盟成员基础信息变更
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_SHOW_INFO_CHG, _onGuildShowInfoChg);//联盟展示信息变更
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_MEMBER_ADD, _onMemberChg);//联盟成员新增
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_MEMBER_REMOVE, _onMemberChg);//联盟成员移除
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_EVENT_ADD, _onGuildEventChg);//联盟事件新增
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_EVENT_CHG, _onGuildEventChg);//联盟事件变更
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_EVENT_REMOVE, _onGuildEventChg);//联盟事件移除
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_MEMBER_BASE_INFO_CHG, _onMemberInfoChg);//联盟成员基础信息变更
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_SHOW_INFO_CHG, _onGuildShowInfoChg);//联盟展示信息变更
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_MEMBER_ADD, _onMemberChg);//联盟成员新增
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_MEMBER_REMOVE, _onMemberChg);//联盟成员移除
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wMemberListGrid?.hideWnd();
            _m_wImpeachCD?.hideWnd();
            _m_wLeaderItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wMemberListGrid?.resetWnd();
            _m_wImpeachCD?.resetWnd();
            _m_wLeaderItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wMemberListGrid?.discard();
            _m_wMemberListGrid = null;
            _m_wImpeachCD?.discard();
            _m_wImpeachCD = null;
            _m_wLeaderItem?.discard();
            _m_wLeaderItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnNotice, _onClickNotice);
            ALUGUICommon.uncombineBtnClick(wnd.btnQuit, _onClickQuit);
            ALUGUICommon.uncombineBtnClick(wnd.btnImpeach, _onClickImpeach);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoMemberListGrid != null)
                _m_wMemberListGrid = new GGUIWndGuildMemberListGrid(wnd.monoMemberListGrid);

            if (wnd.monoImpeachCD != null)
                _m_wImpeachCD = new NPGGUIWndCommonCountDown(wnd.monoImpeachCD);

            if (wnd.monoLeader != null)
                _m_wLeaderItem = new GGUIWndGuildMemberListGridItem(wnd.monoLeader);

            ALUGUICommon.combineBtnClick(wnd.btnNotice, _onClickNotice);
            ALUGUICommon.combineBtnClick(wnd.btnQuit, _onClickQuit);
            ALUGUICommon.combineBtnClick(wnd.btnImpeach, _onClickImpeach);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshGridList();
            _refreshOther();
            _refreshImpeach();
        }

        //刷新成员列表
        private void _refreshGridList()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            List<GuildMemberInfo> memberList = new List<GuildMemberInfo>();
            if(guildInfo.memberList != null)
                memberList.AddRange(guildInfo.memberList);

            //先隐藏列表
            _m_wMemberListGrid?.hideWnd();
            _m_wLeaderItem?.hideWnd();

            //请求成员信息并且排序
            long serialize = _m_lShowSerialize;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(memberList.Count * 2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (serialize != _m_lShowSerialize)
                    return;

                //全部请求完成，开始展示列表
                //排序
                memberList.Sort(_sortMemberList);

                //如果成员列表未满，需要补充空数据来展示全部item
                GuildLevelRefObj guildLevelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(guildInfo.level);
                if (guildLevelRef != null)
                {
                    for (int i = guildInfo.memberCount; i < guildLevelRef.member_limit; i++)
                    {
                        memberList.Add(null);
                    }
                }

                //设置盟主信息
                if (_m_wLeaderItem != null && memberList.Count > 0)
                {
                    _m_wLeaderItem.showWnd();
                    _m_wLeaderItem.setInfo(memberList[0]);
                    memberList.RemoveAt(0);
                }

                //设置成员列表
                if (_m_wMemberListGrid != null)
                {
                    _m_wMemberListGrid.showWnd();
                    _m_wMemberListGrid.setShowData(memberList);
                }
            });

            //请求玩家信息
            long curMemberCount = memberList.Count;
            for (int i = 0; i < curMemberCount; i++)
            {
                memberList[i].getPlayerDetailInfo(_info =>
                {
                    stepCounter.addDoneStepCount();
                });
                memberList[i].getContributeInfo(_info =>
                {
                    stepCounter.addDoneStepCount();
                });
            }
        }

        //刷新其他信息
        private void _refreshOther()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            GuildLevelRefObj guildLevelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(guildInfo.level);
            if (guildLevelRef == null)
                return;

            //设置联盟成员数量
            ALUGUICommon.setLabelTxt(wnd.txtMemberCount,
                TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, guildInfo.memberCount,
                    guildLevelRef.member_limit));

            //设置退盟按钮显隐，除了盟主其他成员都需要展示
            GuildPositionRefObj positionRef = guildInfo.getSelfPositionRef();
            if (positionRef != null)
                ALUGUICommon.setGameObjEnable(wnd.btnQuit, positionRef.type != EGuildPositionType.LEADER);
        }

        //刷新弹劾按钮
        private void _refreshImpeach()
        {
            if (wnd == null)
                return;

            Guild_EventInfo eventInfo = NPPlayer.instance.guildComp.getEventInfo(EGuildEventType.IMPEACH_LEADER);

            //如果没有弹劾事件或者自己是盟主，不需要展示弹劾按钮
            if (eventInfo == null || NPPlayer.instance.guildComp.guildInfo?.leaderId == NPPlayer.instance.playerInfo?.CID)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnImpeach, false);
                return;
            }

            GuildEvent_ImpeachLeader impeachLeader = new GuildEvent_ImpeachLeader();
            impeachLeader.readPackage(eventInfo.getData());
            bool canShowBtn = impeachLeader.getState() == EGuildImpeachLeaderEventState.CAN_IMPEACH ||
                              impeachLeader.getState() == EGuildImpeachLeaderEventState.IN_IMPEACH;

            ALUGUICommon.setGameObjEnable(wnd.btnImpeach, canShowBtn);

            //是否有弹劾倒计时
            if (impeachLeader.getState() == EGuildImpeachLeaderEventState.IN_IMPEACH)
            {
                long leftTimeMs = impeachLeader.getRequestTimeMs() +
                    GRefdataCoreMgr.instance.npGeneral.guild_leader_impeach_message_available_within_hours * 3600000 - 
                    FpsAndPingMgr.instance.serverTimeTag;
                if (_m_wImpeachCD != null)
                {
                    if(leftTimeMs > 0)
                    {
                        _m_wImpeachCD.showWnd();
                        _m_wImpeachCD.setInfo(TimeUtil.msToSecCeiling(leftTimeMs),TransKeyConst.guild_inImpeachCD_cdTime);
                    }
                    else
                    {
                        _m_wImpeachCD.hideWnd();
                    }
                }
            }
            else
            {
                _m_wImpeachCD?.hideWnd();
            }
        }

        //排序联盟成员，职位高到低,在线>离线,离线时间短>离线时间长
        private int _sortMemberList(GuildMemberInfo _a, GuildMemberInfo _b)
        {
            if (_a == null || _b == null || _a.playerDetailInfo == null || _b.playerDetailInfo == null)
                return 0;

            int posComp = _a.positionId.CompareTo(_b.positionId);
            if (posComp != 0)
                return -posComp;

            int isOnlineComp = _a.playerDetailInfo.isOnline.CompareTo(_b.playerDetailInfo.isOnline);
            if (isOnlineComp != 0)
                return -isOnlineComp;

            int offlineTimeComp = _a.playerDetailInfo.lastOfflineMs.CompareTo(_b.playerDetailInfo.lastOfflineMs);
            if (offlineTimeComp != 0)
                return -offlineTimeComp;

            return _a.playerDetailInfo.cid.CompareTo(_b.playerDetailInfo.cid);
        }

        #region 点击事件

        //点击通知
        private void _onClickNotice(GameObject _go)
        {
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.BROADCAST_MESSAGE))
                return;

            //设置默认选择列表
            List<long>  selectCidList = new List<long>();
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo != null && guildInfo.memberList != null)
            {
                for (int i = 0; i < guildInfo.memberList.Count; i++)
                {
                    if (guildInfo.memberList[i] != null && guildInfo.memberList[i].cid != NPPlayer.instance.playerInfo.CID)
                        selectCidList.Add(guildInfo.memberList[i].cid);
                }
            }

            GGUIWndGuildNotice.instance.setSelectList(selectCidList);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildNotice.instance, GGUIWndGuildNotice.instance.showWnd, UINodeTagConst_Guild.C_GUILD_NOTICE);
        }

        //点击退出联盟
        private void _onClickQuit(GameObject _go)
        {
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guild_quitGuildConfirm_none),//是否要退出联盟？
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    NPPlayer.instance.guildComp.reqLeaveGuild(null);
                });
        }

        //点击弹劾按钮
        private void _onClickImpeach(GameObject _go)
        {
            //如果自己是盟主，不需要处理弹劾
            if (NPPlayer.instance.guildComp.guildInfo == null || NPPlayer.instance.guildComp.guildInfo.leaderId == NPPlayer.instance.playerInfo.CID)
                return;

            //如果没有弹劾事件则不处理
            Guild_EventInfo eventInfo = NPPlayer.instance.guildComp.getEventInfo(EGuildEventType.IMPEACH_LEADER);
            if (eventInfo == null)
                return;

            GuildEvent_ImpeachLeader impeachLeader = new GuildEvent_ImpeachLeader();
            impeachLeader.readPackage(eventInfo.getData());
            //如果不可弹劾不处理点击
            if (impeachLeader.getState() != EGuildImpeachLeaderEventState.CAN_IMPEACH &&
                impeachLeader.getState() != EGuildImpeachLeaderEventState.IN_IMPEACH)
                return;

            //如果已经点击过不再处理
            if (impeachLeader.getAgreeMemberCidList() != null &&
                impeachLeader.getAgreeMemberCidList().Contains(NPPlayer.instance.playerInfo.CID))
            {
                //已弹劾
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_alreadyImpeach_none);
                return;
            }


            //盟主已超过{0}小时未上线，是否弹劾盟主？
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guild_impeachConfirmTip_num,GRefdataCoreMgr.instance.npGeneral.guild_leader_impeach_offline_beyond_hours),
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    //请求弹劾
                    NPPlayer.instance.guildComp.reqGuildImpeachLeader(eventInfo.getDbId(), () =>
                    {
                        //弹劾成功
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_impeachSucceed_none);
                    });
                });
        }

        #endregion

        #region 消息事件

        //联盟成员信息变更
        private void _onMemberInfoChg(params object[] _objects)
        {
            _refreshWnd();
        }

        //联盟信息变更（盟主变更）
        private void _onGuildShowInfoChg()
        {
            _refreshWnd();
        }

        //弹劾事件变更
        private void _onGuildEventChg(params object[] _objects)
        {
            _refreshImpeach();
        }

        //联盟成员变更事件
        private void _onMemberChg(params object[] _objects)
        {
            _refreshWnd();
        }

        #endregion
    }
}