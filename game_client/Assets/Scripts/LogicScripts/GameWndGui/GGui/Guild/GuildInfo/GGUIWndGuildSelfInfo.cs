using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟信息界面
    /// </summary>
    public class GGUIWndGuildSelfInfo : _ANPGGUIBasicWnd<GGUIMonoGuildSelfInfo>
    {
        private static GGUIWndGuildSelfInfo _g_instance = new GGUIWndGuildSelfInfo();
        public static GGUIWndGuildSelfInfo instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildSelfInfo();
                return _g_instance;
            }
        }

        //联盟基础信息附加窗口
        private GGUIWndGuildSubBaseInfo _m_wGuildBaseInfo;
        //显示序列号
        private long _m_lShowSerialize;
        //原本的宣言
        private string _m_sOriDeclaration;
        //原本的公告
        private string _m_sOriAnnouncement;
        //招募倒计时
        private NPGGUIWndCommonCountDown _m_wRecruitCountDownWnd;

        public GGUIWndGuildSelfInfo() : base(EALUIWndLayer.NORMAL)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildSelfInfo.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildSelfInfo.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_SHOW_INFO_CHG, _onGuildShowInfoChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_CAN_RECRUIT_TIME_CHG, _onRecruitTimeChg);
            _m_lShowSerialize = ALSerializeOpMgr.next();

            //记录原本内容
            _m_sOriDeclaration = null;
            _m_sOriAnnouncement = null;
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo != null)
            {
                _m_sOriDeclaration = guildInfo.declaration;
                _m_sOriAnnouncement = guildInfo.announcement;
            }

            //刷新窗口
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_SHOW_INFO_CHG, _onGuildShowInfoChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_CAN_RECRUIT_TIME_CHG, _onRecruitTimeChg);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wGuildBaseInfo?.hideWnd();
            _m_wRecruitCountDownWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wGuildBaseInfo?.resetWnd();
            _m_wRecruitCountDownWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wGuildBaseInfo?.discard();
            _m_wGuildBaseInfo = null;
            _m_wRecruitCountDownWnd?.discard();
            _m_wRecruitCountDownWnd = null;

            if (wnd == null)
                return;

            if (wnd.inputDeclaration != null)
            {
                wnd.inputDeclaration.onValueChanged?.RemoveAllListeners();
                wnd.inputDeclaration.onEndEdit?.RemoveAllListeners();
            }

            if (wnd.inputAnnouncement != null)
            {
                wnd.inputAnnouncement.onValueChanged?.RemoveAllListeners();
                wnd.inputAnnouncement.onEndEdit?.RemoveAllListeners();
            }

            ALUGUICommon.uncombineBtnClick(wnd.btnChgFlag, _onClickChgFlag);
            ALUGUICommon.uncombineBtnClick(wnd.btnChgName, _onClickChgName);
            ALUGUICommon.uncombineBtnClick(wnd.btnLeaderInfo, _onClickLeaderInfo);
            ALUGUICommon.uncombineBtnClick(wnd.btnPublicRecruit, _onClickPublicRecruit);
            ALUGUICommon.uncombineBtnClick(wnd.btnDismiss, _onClickDismiss);
            ALUGUICommon.uncombineBtnClick(wnd.btnJoinCondition, _onClickJoinCondition);
            ALUGUICommon.uncombineBtnClick(wnd.btnApply, _onClickApply);
            ALUGUICommon.uncombineBtnClick(wnd.btnLog, _onClickLog);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGuildBaseInfo != null)
                _m_wGuildBaseInfo = new GGUIWndGuildSubBaseInfo(wnd.monoGuildBaseInfo);

            if (wnd.inputDeclaration != null)
            {
                wnd.inputDeclaration.onValueChanged?.AddListener(_onDeclarationEditChg);
                wnd.inputDeclaration.onEndEdit?.AddListener(_onDeclarationEditEnd);
            }

            if (wnd.inputAnnouncement != null)
            {
                wnd.inputAnnouncement.onValueChanged?.AddListener(_onAnnouncementEditChg);
                wnd.inputAnnouncement.onEndEdit?.AddListener(_onAnnouncementEditEnd);
            }

            if (wnd.monoRecruitCD != null)
                _m_wRecruitCountDownWnd = new NPGGUIWndCommonCountDown(wnd.monoRecruitCD);

            ALUGUICommon.combineBtnClick(wnd.btnChgFlag, _onClickChgFlag);
            ALUGUICommon.combineBtnClick(wnd.btnChgName, _onClickChgName);
            ALUGUICommon.combineBtnClick(wnd.btnLeaderInfo, _onClickLeaderInfo);
            ALUGUICommon.combineBtnClick(wnd.btnPublicRecruit, _onClickPublicRecruit);
            ALUGUICommon.combineBtnClick(wnd.btnDismiss, _onClickDismiss);
            ALUGUICommon.combineBtnClick(wnd.btnJoinCondition, _onClickJoinCondition);
            ALUGUICommon.combineBtnClick(wnd.btnApply, _onClickApply);
            ALUGUICommon.combineBtnClick(wnd.btnLog, _onClickLog);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshBaseInfo();
            _refreshRecruitCD();
            _setEditPermission();
        }

        //刷新基础信息
        private void _refreshBaseInfo()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            if (_m_wGuildBaseInfo != null)
            {
                _m_wGuildBaseInfo.showWnd();
                _m_wGuildBaseInfo.setBaseInfo(guildInfo);
                _m_wGuildBaseInfo.setWealth(guildInfo.guildWealth);

                //先重置盟主显示
                ALUGUICommon.setGameObjEnable(wnd.btnLeaderInfo, false);
                _m_wGuildBaseInfo.setLeaderName("");

                //设置联盟盟主信息
                if (guildInfo.leaderId == NPPlayer.instance.playerInfo.CID)
                {
                    //盟主是自己
                    _m_wGuildBaseInfo.setLeaderName(NPPlayer.instance.playerInfo.PlayerName, TransKeyConst.guild_leaderName_name);
                }
                else
                {
                    //盟主是其他人
                    long serialzie = _m_lShowSerialize;
                    GCommon.reqPlayerInfo(guildInfo.leaderId, _info =>
                    {
                        if (serialzie != _m_lShowSerialize || _info == null)
                            return;

                        _m_wGuildBaseInfo.setLeaderName(_info.name, TransKeyConst.guild_leaderName_name);
                        ALUGUICommon.setGameObjEnable(wnd.btnLeaderInfo, true);
                    });
                }
            }

            //设置宣言
            if(wnd.inputDeclaration != null)
                wnd.inputDeclaration.text = guildInfo.declaration;
            long declarationLength = CharacterDetermineMgr.instance.getUnicodeStringLength(wnd.inputDeclaration?.text);
            WCGIntRange declarationLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_declaration_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputDeclarationCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, declarationLength, declarationLengthRange?.max));

            //设置公告
            if(wnd.inputAnnouncement != null)
                wnd.inputAnnouncement.text = guildInfo.announcement;
            long announcementLength = CharacterDetermineMgr.instance.getUnicodeStringLength(wnd.inputAnnouncement?.text);
            WCGIntRange announcementLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_announcement_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputAnnouncementCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, announcementLength, announcementLengthRange?.max));
        }

        //刷新招募倒计时
        private void _refreshRecruitCD()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            long leftTimeMs = guildInfo.nextCanRecruitTimeMs - FpsAndPingMgr.instance.serverTimeTag;

            if (_m_wRecruitCountDownWnd != null)
            {
                if (leftTimeMs > 0)
                {
                    _m_wRecruitCountDownWnd.showWnd();
                    _m_wRecruitCountDownWnd.setInfo(TimeUtil.msToSecCeiling(leftTimeMs), null);
                }
                else
                    _m_wRecruitCountDownWnd.hideWnd();
            }
        }

        //刷新旗帜
        private void _refreshFlag()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            if (_m_wGuildBaseInfo != null)
                _m_wGuildBaseInfo.setFlag(guildInfo.flagId);
        }

        //刷新联盟名称
        private void _refreshGuildName()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            if (_m_wGuildBaseInfo != null)
                _m_wGuildBaseInfo.setName(guildInfo.simpleName, guildInfo.name, guildInfo.guildId);
        }

        //设置是否有编辑权限
        private void _setEditPermission()
        {
            if (wnd == null)
                return;

            //是否有更改联盟信息权限
            bool havePermission = NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.CHANGE_GUILD_INFO);

            if (wnd.inputAnnouncement != null)
                wnd.inputAnnouncement.interactable = havePermission;

            if(wnd.inputDeclaration != null)
                wnd.inputDeclaration.interactable = havePermission;
        }

        //宣言编辑
        private void _onDeclarationEditChg(string _str)
        {
            if (wnd == null || wnd.inputDeclaration == null)
                return;

            string targetString = wnd.inputDeclaration.text;
            CharacterDetermineMgr.instance.replaceIllegalCharacter(false, ref targetString);
            wnd.inputDeclaration.text = targetString;
            long declarationLength = CharacterDetermineMgr.instance.getUnicodeStringLength(targetString);
            WCGIntRange declarationLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_declaration_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputDeclarationCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, declarationLength, declarationLengthRange?.max));
        }

        //宣言编辑完成
        private void _onDeclarationEditEnd(string _str)
        {
            if (wnd == null || wnd.inputDeclaration == null)
                return;

            string targetString = wnd.inputDeclaration.text;
            CharacterDetermineMgr.instance.replaceIllegalCharacter(false, ref targetString);
            wnd.inputDeclaration.text = targetString;

            long declarationLength = CharacterDetermineMgr.instance.getUnicodeStringLength(targetString);
            WCGIntRange declarationLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_declaration_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputDeclarationCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, declarationLength, declarationLengthRange?.max));

            if (_m_sOriDeclaration == targetString)
                return;

            //判断长度
            if (declarationLengthRange != null)
            {
                if (declarationLength < declarationLengthRange.min)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputDeclarationUnderLimit_none);
                else if(declarationLength > declarationLengthRange.max)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputDeclarationOverLimit_none);
            }

            //请求变更
            NPPlayer.instance.guildComp.reqChgGuildDeclaration(targetString, () =>
            {
                _m_sOriDeclaration = targetString;
                //修改成功
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_chgInfoSuc_none);
            });
        }

        //公告编辑
        private void _onAnnouncementEditChg(string _str)
        {
            if (wnd == null || wnd.inputAnnouncement == null)
                return;

            string targetString = wnd.inputAnnouncement.text;
            CharacterDetermineMgr.instance.replaceIllegalCharacter(false, ref targetString);
            wnd.inputAnnouncement.text = targetString;

            long announcementLength = CharacterDetermineMgr.instance.getUnicodeStringLength(targetString);
            WCGIntRange announcementLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_announcement_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputAnnouncementCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, announcementLength, announcementLengthRange?.max));
        }

        //公告编辑完成
        private void _onAnnouncementEditEnd(string _str)
        {
            if (wnd == null || wnd.inputAnnouncement == null)
                return;

            string targetString = wnd.inputAnnouncement.text;
            CharacterDetermineMgr.instance.replaceIllegalCharacter(false, ref targetString);
            wnd.inputAnnouncement.text = targetString;

            long announcementLength = CharacterDetermineMgr.instance.getUnicodeStringLength(targetString);
            WCGIntRange announcementLengthRange = GRefdataCoreMgr.instance.npGeneral.guild_announcement_length_limit;
            ALUGUICommon.setLabelTxt(wnd.txtInputAnnouncementCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, announcementLength, announcementLengthRange?.max));

            if (_m_sOriAnnouncement == targetString)
                return;

            //判断长度
            if (announcementLengthRange != null)
            {
                if (announcementLength < announcementLengthRange.min)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAnnouncementUnderLimit_none);
                else if (announcementLength > announcementLengthRange.max)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_inputAnnouncementOverLimit_none);
            }

            //请求变更
            NPPlayer.instance.guildComp.reqChgGuildAnnouncement(targetString, () =>
            {
                _m_sOriAnnouncement = targetString;
                //修改成功
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_chgInfoSuc_none);
            });
        }

        #region 消息事件

        //联盟展示信息变更
        private void _onGuildShowInfoChg()
        {
            _refreshFlag();
            _refreshGuildName();
        }

        //可招募时间变更
        private void _onRecruitTimeChg()
        {
            _refreshRecruitCD();
        }

        #endregion

        #region 点击事件

        //点击更换旗帜
        private void _onClickChgFlag(GameObject _go)
        {
            //是否有更改联盟信息权限
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.CHANGE_GUILD_INFO))
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            GuildFlagRefObj flagRef = GRefdataCoreMgr.instance.guildFlagRefCore.getRef(guildInfo.flagId);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildFlagSelect.instance, () =>
            {
                GGUIWndGuildFlagSelect.instance.showWnd();
                GGUIWndGuildFlagSelect.instance.setInfo(flagRef, true, _selectFlagRef =>
                {
                    if (_selectFlagRef == null || guildInfo.flagId == _selectFlagRef.id)
                        return;

                    //请求更换旗帜
                    NPPlayer.instance.guildComp.reqChgGuildFlag(_selectFlagRef.id, ()=>
                    { 
                        //修改成功
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_chgInfoSuc_none);
                        _refreshFlag();
                    });
                });
            }, UINodeTagConst_Guild.C_GUILD_SELECT_FLAG);
        }

        //点击更换名称
        private void _onClickChgName(GameObject _go)
        {
            //是否有更改联盟信息权限
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.CHANGE_GUILD_INFO))
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildChangeName.instance, () =>
            {
                GGUIWndGuildChangeName.instance.showWnd();
                GGUIWndGuildChangeName.instance.setInfo((_name, _simpleName) =>
                {
                    //请求更换名称
                    NPPlayer.instance.guildComp.reqChgGuildName(_name, _simpleName, ()=>
                    {
                        //修改成功
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_chgInfoSuc_none);
                        _refreshGuildName();
                    });
                });
            }, UINodeTagConst_Guild.C_GUILD_CHANGE_NAME);
        }

        //点击查看盟主信息
        private void _onClickLeaderInfo(GameObject _go)
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            if (wnd == null || wnd.leaderInfoLocateRectTrans == null)
                return;

            long serialize = _m_lShowSerialize;
            GCommon.reqPlayerInfo(guildInfo.leaderId, (_info) =>
            {
                if (serialize != _m_lShowSerialize || wnd == null || wnd.leaderInfoLocateRectTrans == null)
                    return;

                GCommon.showPlayerInfoWndTip(_info, wnd.leaderInfoLocateRectTrans, 0, Game.instance.mainCamera?.uiRootRectTrans);
            });
        }

        //点击公开招募按钮
        private void _onClickPublicRecruit(GameObject _go)
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            //可招募剩余时间
            long leftTimeMs = guildInfo.nextCanRecruitTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            if (leftTimeMs > 0)
                return;

            NPPlayer.instance.guildComp.reqOpenRecruit(null);
        }

        //点击遣散按钮
        private void _onClickDismiss(GameObject _go)
        {
            //检查是否有权限
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.DISSOLVE_GUILD, true))
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            //需要没有其他成员才能遣散联盟
            if (guildInfo.memberCount > 1)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_canNotDismissGuild_none);
            }
            else
            {
                NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guild_dismissGuildConfirmDesc_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                    null,
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        NPPlayer.instance.guildComp.reqDissolveGuild(() =>
                        {
                            //联盟已解散
                            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_dissolveGuildSuc_none);
                        });
                    });
            }
        }

        //点击加入条件按钮
        private void _onClickJoinCondition(GameObject _go)
        {
            //判断是否有权限
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.TOGGLE_FREE_JOIN, true))
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildChangeApplyCondition.instance,
                GGUIWndGuildChangeApplyCondition.instance.showWnd, UINodeTagConst_Guild.C_GUILD_APPLY_CONDITION);
        }

        //点击联盟申请按钮
        private void _onClickApply(GameObject _go)
        {
            //判断是否有权限
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.PROCESS_JOIN_REQUEST, true))
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildOthersApplyList.instance, GGUIWndGuildOthersApplyList.instance.showWnd, UINodeTagConst_Guild.C_GUILD_OTHERS_APPLY_LIST);
        }

        //点击联盟日志
        private void _onClickLog(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildLog.instance, GGUIWndGuildLog.instance.showWnd, UINodeTagConst_Guild.C_GUILD_LOG);
        }

        #endregion
    }
}