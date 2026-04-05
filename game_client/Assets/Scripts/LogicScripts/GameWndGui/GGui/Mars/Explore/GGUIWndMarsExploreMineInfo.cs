using ALPackage;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPCommon;
using System;
using Common.MarsObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreMineInfo : _ANPGGUIBasicWnd<GGUIMonoMarsExploreMineInfo>, _IMarsExploreTeamSelectDealer
    {
        private static GGUIWndMarsExploreMineInfo _g_instance;
        [NotNull] public static GGUIWndMarsExploreMineInfo instance { get { return _g_instance ??= new GGUIWndMarsExploreMineInfo(); } }

        //矿的实例Id
        private long _m_lMineInstanceId;
        //派遣队伍的处理函数
        private Action<long> _m_aSendTeamAction;
        private Mars_MineDynamic _m_mineInfo;
        private MarsExploreMineRefObj _m_rMineRef;
        private PlayerInfo_IconShow _m_playerInfo;
        private long _m_lShowSerialize;

        private NPGGuiWndTexture _m_bannerWnd;
        private NPGGuiWndTexture _m_iconWnd;
        private NPGGUIWndCommonItem _m_resourceIconWnd;
        private ALCommonEnableTaskController _m_tickTask;


        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreMineInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreMineInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public long targetPower { get { return _m_mineInfo?.getTeamPower() ?? 0; } }


        public GGUIWndMarsExploreMineInfo() 
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_bannerWnd?.showWnd();
            _m_iconWnd?.showWnd();
            _m_resourceIconWnd?.showWnd();

            //刷新数据
            refreshWnd();
            
            //开启tick
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);

            NPPlayer.instance.marsComp.exploreSubComponent.onMineChg += _onMineChg;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.marsComp.exploreSubComponent.onMineChg -= _onMineChg;
            
            _m_tickTask.setDisable();
            
            _m_bannerWnd?.hideWnd();
            _m_iconWnd?.hideWnd();
            _m_resourceIconWnd?.hideWnd();
            
            //刷新序列号，并重置数据表示这次数据无效了
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_mineInfo = null;
            _m_rMineRef = null;
            _m_playerInfo = null;
        }
        protected override void _onReset()
        {
            _m_bannerWnd?.discardTexture();
            _m_iconWnd?.discardTexture();
            _m_resourceIconWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_bannerWnd?.discard();
            _m_bannerWnd = null;
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            _m_resourceIconWnd?.discard();
            _m_resourceIconWnd = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoCollect, _onClickGoCollect);
            ALUGUICommon.uncombineBtnClick(wnd.btnReturnFromCollect, _onClickReturnFromCollect);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnShare, _onShareClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoResourceIcon != null)
                _m_resourceIconWnd = new NPGGUIWndCommonItem(wnd.monoResourceIcon);
            if (wnd.imgBanner != null)
                _m_bannerWnd = new NPGGuiWndTexture(wnd.imgBanner);
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnGoCollect, _onClickGoCollect);
            ALUGUICommon.combineBtnClick(wnd.btnReturnFromCollect, _onClickReturnFromCollect);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnShare, _onShareClick);
        }


        public void refreshWnd(long _mineInstanceId, Action<long> _sendTeamAction)
        {
            //设置实例Id
            _m_lMineInstanceId = _mineInstanceId;
            _m_aSendTeamAction = _sendTeamAction;
            refreshWnd();
        }
        /// <summary>
        /// 刷新矿信息
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            if (wnd.anim != null)
                wnd.anim.Sample(wnd.teamSelectCancelAnimName, 1f);
            
            //发送请求处理
            _m_lShowSerialize = ALSerializeOpMgr.next();
            long tmpSerialize = _m_lShowSerialize;

            // 先清空数据，以免 onMineChg 事件触发时的判断认为 mineInfo 已经初始化了
            _m_mineInfo = null;
            wnd.setLoadingShow(true);
            _reqServerInfo(_m_lMineInstanceId, (_mineInfo, _playerInfo) =>
            {
                if (_m_lShowSerialize != tmpSerialize)
                    return;
                
                wnd.setLoadingShow(false);

                _m_mineInfo = _mineInfo;
                _m_rMineRef = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_m_mineInfo.getRefId());
                _m_playerInfo = _playerInfo;
                _refreshWndInternal();
            }, closeWnd);
            
            //  二次确认（有模糊）弹窗打开会导致 本窗口移动到最前面，GGUIWndMarsExploreTeamSelect这个窗口到后面去了，不好优化模糊逻辑，临时这么处理
            // 【优化-0】火星探索-当目标对象战力大于玩家派遣队伍时，需要提示玩家是否派遣。如：敌方战力值高于你，是否确认派遣队伍？ https://www.teambition.com/task/695e7f7e90ace14bfe464d72
            if (_m_isSelectTeamShow && GGUIWndMarsExploreTeamSelect.instance.isShow)
            {
                //将窗口移到最前
                GCommon.moveTransformToLastAndRefreshLayer(GGUIWndMarsExploreTeamSelect.instance.getGameObj());
                if (wnd.anim != null) wnd.anim.Sample(wnd.teamSelectStartAnimName, 1f);
            }
        }


        private void _refreshWndInternal()
        {
            if (wnd == null || _m_rMineRef == null || _m_mineInfo == null)
                return;
            
            bool isEmpty = _m_playerInfo == null;
            bool isSelf = !isEmpty && _m_playerInfo.getCid() == NPPlayer.instance.playerInfo.CID;
            bool isGuildMember = !isEmpty && NPPlayer.instance.guildComp.isSameGuild(_m_playerInfo.getGuildId());
            wnd.setState(isEmpty, isSelf, isGuildMember);
            _m_bannerWnd?.setTexture(_m_rMineRef.banner);
            _m_iconWnd?.setTexture(_m_rMineRef.icon);
            _m_resourceIconWnd?.setItem(_m_rMineRef.res_type.toCommonItemData());
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_rMineRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_rMineRef.mine_lvl));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_rMineRef.desc));
            ALUGUICommon.setLabelTxt(wnd.txtResourceAmount, TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineResourceAmount_num, _m_rMineRef.res_num.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtPlayerName, _getPlayerNameString(_getNameColor(isSelf, isGuildMember)));
			ALUGUICommon.setLabelTxt(wnd.txtTeamPower, _m_mineInfo.getTeamPower().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            // 设置采集速度
            long oriCollectSpeed = _m_rMineRef.mars_mine_collects_speed * 60 * 60;
            long addedCollectSpeed = _getCollectSpeed() * 60 * 60 - oriCollectSpeed;
            ALUGUICommon.setLabelTxt(wnd.txtCollectSpeed,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineCollectSpeed_num_num
                    , oriCollectSpeed.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), addedCollectSpeed.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }
        private void _onClickGoCollect(GameObject _go)
        {
            if (_m_mineInfo == null)
                return;

            if (_m_playerInfo != null)
            {
                if (_m_playerInfo.getCid() == NPPlayer.instance.playerInfo.CID)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreMineOccupiedBySelf_none);
                    return;
                }

                if (NPPlayer.instance.guildComp.isSameGuild(_m_playerInfo.getGuildId()))
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreMineOccupiedByAlliance_none);
            }

            // 判断是否已经有队伍在前往该事件地点
            if (NPPlayer.instance.marsComp.exploreSubComponent.isTeamMarchTarget(_m_mineInfo.getId()))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreAlreadySendingTeamTip_none);
                return;
            }

            GGUIWndMarsExploreTeamSelect.instance.refreshWnd(this);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamSelect.instance, GGUIWndMarsExploreTeamSelect.instance.showWnd, EUIQueueStageType.MAIN,
                UINodeTagConst.C_MARS_EXPLORE_TEAM_SELECT, false, false);
        }
        private void _onClickReturnFromCollect(GameObject _go)
        {
            if (_m_mineInfo == null)
                return;

            //发送请求返回
            NPGSClientListener.sendMsgByLog(new GC2GS_041_012_ReqMarsTeamBack(_m_mineInfo.getOccupiedTeamId()));
            //直接关闭窗口
            closeWnd();
        }
        private void _onClickDetail(GameObject _go)
        {
            if (_m_mineInfo == null || _m_rMineRef == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreMineDetailInfo.instance, () =>
            {
                GGUIWndMarsExploreMineDetailInfo.instance.refreshWnd(_m_mineInfo, _m_rMineRef);
                GGUIWndMarsExploreMineDetailInfo.instance.showWnd();
            }, UINodeTagConst.C_MARS_EXPLORE_MINE_DETAIL_INFO);
        }
        private void _onClickClose(GameObject _go)
        {
            closeWnd();
        }
        private void _onShareClick(GameObject _go)
        {
            if (_m_mineInfo == null)
                return;

            NPRoomChatInfo guildChatRoom = NPPlayer.instance.chatComp.getRoomChatInfo(ENPChatRoomType.GUILD);
            if (guildChatRoom == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreMineChatShareNoGuildTip_none);
                return;
            }

            long lastShareTime = AccountSettingMgr.instance.accountSetting.lastShareMarsMineToGuildChatTime;
            long curTime = FpsAndPingMgr.instance.serverTimeTag;
            long shareInterval = curTime - lastShareTime;
            long shareIntervalLimit = GRefdataCoreMgr.instance.npGeneral.mars_explore_share_mine_to_guild_chat_interval_ms;
            long remainTime = shareIntervalLimit - shareInterval;
            if (remainTime > 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineChatShareTooFrequentTip_time, remainTime / 1000 / 60 + 1));
                return;
            }
            
            NPGSClientListener.sendRequestByLog(new GC2GS_041_025_ReqShareMarsMineToGuildChat(_m_mineInfo.getId()), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_exploreMineChatShareSuccessTip_none);
                    AccountSettingMgr.instance.accountSetting.setLastShareMarsMineToGuildChatTime(FpsAndPingMgr.instance.serverTimeTag);
                }));
        }
        /// <summary>
        /// 每秒tick的刷新处理
        /// </summary>
        private void _tick()
        {
            if (wnd == null || !_m_bIsShow || _m_mineInfo == null || _m_rMineRef == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtRemainResourceNum,
                TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineRemainResource_num, _getRemainNum().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            if (wnd.sldCollectProgress != null)
            {
                long remainNum = _getRemainNum();
                float progress = 1f - (float)remainNum / _m_rMineRef.res_num;
                wnd.sldCollectProgress.minValue = 0;
                wnd.sldCollectProgress.maxValue = 1;
                wnd.sldCollectProgress.value = progress;
            }
            long remainTime = _calculateRemainCollectTimeSec();
            long predictTime = _calculatePredictCollectTimeSec();
            ALUGUICommon.setLabelTxt(wnd.txtRemainCollectTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainTime));
            ALUGUICommon.setLabelTxt(wnd.txtPredictCollectTime, TimeUtil.millisecondsToTime_DayHourOrHMS(predictTime));
        }
        private long _calculateRemainCollectTimeSec()
        {
            if (_m_mineInfo == null)
                return 0;

            if (_m_mineInfo.getCollectSpeed() <= 0)
                return 0;

            return Mathf.FloorToInt(_getRemainNum() * 1000f / _m_mineInfo.getCollectSpeed());
        }
        private long _calculatePredictCollectTimeSec()
        {
            if (_m_mineInfo == null)
                return 0;
            
            long speed = NPPlayer.instance.marsComp.exploreSubComponent.calculateCollectSpeed(_m_mineInfo.getRefId());
            if (speed <= 0)
                return 0;

            return Mathf.FloorToInt(_getRemainNum() * 1000f / speed);
        }
        /// <summary>
        /// 选择完成队伍之后的处理
        /// </summary>
        public void dealSelectTeam(long _teamId)
        {
            //发送请求派遣队伍
            _m_aSendTeamAction?.Invoke(_teamId);
        }
        private bool _m_isSelectTeamShow = false;

        public void onTeamSelectWndShow()
        {
            if (wnd == null || wnd.anim == null)
                return;
            
            _m_isSelectTeamShow = true;
            wnd.anim.ForcePlay(wnd.teamSelectStartAnimName);
        }
        public void onTeamSelectWndHide()
        {
            if (wnd == null || wnd.anim == null)
                return;
            
            _m_isSelectTeamShow = false;
            wnd.anim.ForcePlay(wnd.teamSelectCancelAnimName);
        }
        public void closeWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_MINE_INFO);
        }


        private long _getRemainNum()
        {
            if (_m_mineInfo == null)
                return 0;
            
            if (_m_mineInfo.getCollectSpeed() <= 0)
                return _m_mineInfo.getRemainNum();
            
            long remain = Mathf.FloorToInt(_m_mineInfo.getRemainNum() - (FpsAndPingMgr.instance.serverTimeTag - _m_mineInfo.getOccupiedMs()) * _m_mineInfo.getCollectSpeed() / 1000f);
            return remain;
        }
        private string _getPlayerNameString(Color _nameColor)
        {
            if (_m_playerInfo == null)
                return string.Empty;
            
            string guildPlayerName;
            if(string.IsNullOrEmpty(_m_playerInfo.getGuildSimpleName()))
                guildPlayerName = _m_playerInfo.getPlayerName();
            else
                guildPlayerName = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOccupantName, 
                    _m_playerInfo.getGuildSimpleName(), _m_playerInfo.getPlayerName());
            guildPlayerName = GCommon.addColorForRichText(guildPlayerName, _nameColor);
            return guildPlayerName;
        }
        private Color _getNameColor(bool _isSelf, bool _isGuildMember)
        {
            if (wnd == null)
                return Color.white;
            
            Color showColor;
            if (_isSelf)
                showColor = wnd.selfNameColor;
            else if (_isGuildMember)
                showColor = wnd.guildMemberNameColor;
            else
                showColor = wnd.enemyNameColor;
            return showColor;
        }
        private long _getCollectSpeed()
        {
            if (_m_mineInfo == null)
                return 0;

            // 如果没有占领者，采集速度为估算值
            if (_m_mineInfo.getOccupiedCid() <= 0)
                return NPPlayer.instance.marsComp.exploreSubComponent.calculateCollectSpeed(_m_mineInfo.getRefId());
            
            return _m_mineInfo.getCollectSpeed();
        }
        private void _reqServerInfo(long _mineInstanceId, Action<Mars_MineDynamic, PlayerInfo_IconShow> _complete, Action _failed)
        {
            // 发送请求并在返回数据时刷新
            NPGSClientListener.sendRequestByLog(GSWriter_041_MarsExploreOp.make_013_ReqNoticeMarsMine(_mineInstanceId)
                , new CommonRequestCallbackProtocolDealer<GS2GC_041_013_RetNoticeMarsMine>(
                    (_res) =>
                    {
                        //获取数据
                        Mars_MineDynamic mineInfo = _res?.getInfo();
                        _reqPlayerInfo(mineInfo, _playerInfo => _complete?.Invoke(mineInfo, _playerInfo));
                    },(_err)=>
                    {
                        //上浮提示
                        NPGUIAddSceneCenterTip.instance.showErrorInfo(_err);
                        _failed?.Invoke();
                    }, true     //注意同时需要触发协议，更新矿数据
                ));
        }
        /// <summary>
        /// 请求当前玩家信息，并在回调刷新
        /// </summary>
        /// <param name="_complete"></param>
        protected void _reqPlayerInfo(Mars_MineDynamic _mineInfo, Action<PlayerInfo_IconShow> _complete)
        {
            if (null == _mineInfo)
            {
                _complete?.Invoke(null);
                return;
            }

            if (_mineInfo.getOccupiedCid() == 0)
            {
                _complete?.Invoke(null);
                return;
            }

            if (_mineInfo.getOccupiedCid() == NPPlayer.instance.playerInfo.CID)
            {
                PlayerInfo_IconShow selfInfo = NPPlayer.instance.playerInfo.getPlayerBriefInfo();
                _complete?.Invoke(selfInfo);
                return;
            }

            //发送请求请求玩家数据
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_mineInfo.getOccupiedCid()),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                {
                    if (!_isSuc)
                    {
                        _complete?.Invoke(null);
                        return;
                    }

                    PlayerInfo_IconShow briefInfo = _msg?.getPlayerBrief();
                    _complete?.Invoke(briefInfo);
                }));
        }
        private void _onMineChg(long _mineInstanceId, bool _)
        {
            if (_mineInstanceId != _m_lMineInstanceId)
                return;

            // 如果没有动态数据说明还在请求数据过程中，不做处理
            if (_m_mineInfo == null)
                return;

            //刷新数据
            refreshWnd();
        }
    }
}
