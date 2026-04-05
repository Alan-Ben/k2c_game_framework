using ALPackage;
using Common.GuildEnum;
using Common.GuildObj;
using Common.MarsObj;
using CommonEnum;
using GC2GS.p032_GuildOp;
using GS2GC.p002_InitOp;
using GS2GC.p032_GuildOp;
using JetBrains.Annotations;
using NPEnum;
using Spine;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟管理器
    /// </summary>
    public class GuildComponent : _ANPBasicPlayerComponent
    {
        //联盟信息
        private GuildInfo _m_guildInfo;
        //请求加入的联盟id列表
        private HashSet<long> _m_lReqJoinGuildList;
        //联盟免cd加入次数
        private int _m_iGuildFreeJoinCdNum;
        //加入联盟cd结束时间
        private long _m_lJoinGuildCdEndTimeMs;
        //自己的7日贡献度
        private long _m_lSelfSevenDaysContribution;
        //自己的总贡献度
        private long _m_lSelfTotalContribution;
        //建设记录的信息
        private GuildConstructRemarkInfo _m_constructRemarkInfo;
        /// <summary>
        /// 自身每日更新数据(不要直接引用, 因为可能存在跨天数据重置, 要获取自身每日数据使用selfDailyData)
        /// </summary>
        private GuildMemberDailyData _m_selfDailyData;
        
        /// <summary>
        /// 记录上次选择派遣的伙伴id
        /// </summary>
        private long _m_lRecordLastSelectDispatchHeroId;

        private long _m_latestMarsMineBattleReportId;
        
        //联盟加成管理器
        [NotNull] private CommonUnionBonusMgr _m_unionBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.GUILD);
        
        public GuildComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.BASIC_INFO, ENPPlayerCompType.CHAT };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.GUILD; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 联盟信息
        /// </summary>
        public GuildInfo guildInfo { get { return _m_guildInfo; } }
        /// <summary>
        /// 加入联盟cd结束时间
        /// </summary>
        public long joinGuildCdEndTimeMs { get { return _m_iGuildFreeJoinCdNum > 0 ? 0 : _m_lJoinGuildCdEndTimeMs; } }
        /// <summary>
        /// 自己的7日贡献度
        /// </summary>
        public long selfSevenDaysContribution { get { return _m_lSelfSevenDaysContribution; } }
        /// <summary>
        /// 自己的总贡献度
        /// </summary>
        public long selfTotalContribution { get { return _m_lSelfTotalContribution; } }
        /// <summary>
        /// 正在请求加入联盟数量
        /// </summary>
        public int reqJoinGuildNum { get { return _m_lReqJoinGuildList?.Count ?? 0; } }
        /// <summary>
        /// 记录上次选择派遣的伙伴id
        /// </summary>
        public long recordLastSelectDispatchHeroId { get { return _m_lRecordLastSelectDispatchHeroId; } set { _m_lRecordLastSelectDispatchHeroId = value; } }
        public long latestMarsMineBattleReportId { get { return _m_latestMarsMineBattleReportId; } }

        /// <summary>
        /// 自身每日更新数据
        /// </summary>
        public GuildMemberDailyData selfDailyData
        {
            get
            {
                // 若跨天了, 自身每日数据还未更新的话, 重置自身每日数据
                if (_m_selfDailyData != null && _m_selfDailyData.date !=
                    NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LAST_LOGIN_DATE)) //找服务端确认过LAST_LOGIN_DATE跨天会更新
                    _m_selfDailyData.resetData();

                return _m_selfDailyData;
            }
        }
        
        public CommonUnionBonusMgr unionBonusMgr { get { return _m_unionBonusMgr; } }
        
        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqGuildInit();
        }

        protected override void _dealInit()
        {
            //初始化建设记录
            _m_constructRemarkInfo = new GuildConstructRemarkInfo();
            _m_constructRemarkInfo.sendRequest();
         
            _m_unionBonusMgr.clear();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _m_unionBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            WinMsg.RegisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCDCountChg);
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_GAIN, _onGainHero);
            WinMsg.RegisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCDChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);
            //检查联盟相关本地存储
            AccountSettingMgr.instance.guildSaver.checkGuildId();
        }

        //全部初始化完成
        public override void onAllCompInited()
        {
            base.onAllCompInited();
            _refreshAllRedTip();
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("GuildComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCDCountChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_GAIN, _onGainHero);
            WinMsg.UnregisterMsg(WinMsgType.ON_LAZY_CD_CHG, _onLazyCDChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);

            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_guildInfo = null;
            _m_lReqJoinGuildList?.Clear();
            _m_lReqJoinGuildList = null;
            _m_constructRemarkInfo = null;
            
            _m_unionBonusMgr.clear();
            _m_lRecordLastSelectDispatchHeroId = 0;
        }

        /// <summary>
        /// 是否加入联盟
        /// </summary>
        /// <returns></returns>
        public bool isJoinGuild()
        {
            return _m_guildInfo != null && _m_guildInfo.guildId > 0;
        }

        /// <summary>
        /// 判断是否是同一个联盟
        /// </summary>
        public bool isSameGuild(long _guildId)
        {
            return _m_guildInfo != null && _guildId > 0 && _m_guildInfo.guildId == _guildId;
        }

        /// <summary>
        /// 是否正在请求加入该联盟
        /// </summary>
        /// <param name="_guildId">联盟id</param>
        /// <returns></returns>
        public bool isApplyJoinGuild(long _guildId)
        {
            if (_m_lReqJoinGuildList == null || _m_lReqJoinGuildList.Count == 0)
                return false;

            return _m_lReqJoinGuildList.Contains(_guildId);
        }

        /// <summary>
        /// 检查是否拥有权限
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public bool checkHavePermission(EGuildPermissionType _type, bool _needTip = false)
        {
            if (!isJoinGuild())
                return false;

            GuildPositionRefObj selfPositionRef = _m_guildInfo?.getSelfPositionRef();
            if (selfPositionRef == null || selfPositionRef.permission_list == null)
                return false;

            bool havePermission = selfPositionRef.permission_list.Contains(_type);

            //如果没有权限是否需要弹提示
            if (!havePermission && _needTip)
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_dontHavePermission_none);

            return havePermission;
        }

        /// <summary>
        /// 根据职位获取
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public long getTargetPositionMemberCount(EGuildPositionType _type)
        {
            if (_m_guildInfo == null || _m_guildInfo.memberList == null)
                return 0;

            long count = 0;
            for (int i = 0; i < _m_guildInfo.memberList.Count; i++)
            {
                if (_m_guildInfo.memberList[i].positionId == (long) _type)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// 根据建设id获取是否已经捐献建设
        /// </summary>
        /// <param name="_constructRefId"></param>
        /// <returns></returns>
        public bool getIsConstructById(long _constructRefId)
        {
            if (_m_constructRemarkInfo == null || _m_constructRemarkInfo.constructClientInfo == null)
                return false;

            //服务器当前日期
            int curDate = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);
            if (_m_constructRemarkInfo.constructClientInfo.getDate() != curDate)
                return false;
            else
                return _m_constructRemarkInfo.constructClientInfo.getConstructRefIdList() != null &&
                       _m_constructRemarkInfo.constructClientInfo.getConstructRefIdList().Contains(_constructRefId);
        }

        /// <summary>
        /// 根据类型获取是否有对应事件
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public bool getHaveGuildEvent(EGuildEventType _type)
        {
            if (_m_guildInfo == null || _m_guildInfo.guildEventList == null)
                return false;

            for (int i = 0; i < _m_guildInfo.guildEventList.Count; i++)
            {
                if (_m_guildInfo.guildEventList[i] != null && _m_guildInfo.guildEventList[i].getType() == _type)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取事件信息
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public Guild_EventInfo getEventInfo(EGuildEventType _type)
        {
            if (_m_guildInfo == null || _m_guildInfo.guildEventList == null)
                return null;

            for (int i = 0; i < _m_guildInfo.guildEventList.Count; i++)
            {
                if (_m_guildInfo.guildEventList[i] != null && _m_guildInfo.guildEventList[i].getType() == _type)
                    return _m_guildInfo.guildEventList[i];
            }

            return null;
        }

        /// <summary>
        /// 更新玩家自身每日数据
        /// </summary>
        /// <param name="_data"></param>
        private void _updateSelfDailyData(Guild_MemberDailyData _data)
        {
            if(_data == null)
                return;
            
            if (_m_selfDailyData == null)
                _m_selfDailyData = new GuildMemberDailyData(_data);
            else
                _m_selfDailyData.updateInfo(_data);

            //刷新红点
            _refreshDonateRewardRedTip();
            _refreshDonateByCoinRedTip();
        }
        
        /// <summary>
        /// 更新玩家自身每周数据
        /// </summary>
        /// <param name="_data"></param>
        private void _updateSelfWeekData(Guild_MemberWeekData _data)
        {
            if(_data == null)
                return;
            
        }

        /// <summary>
        /// 更新联盟经验CommonItem数量
        /// </summary>
        private void _updateGuildExpCommonItemCount()
        {
            // 联盟经验数量变更
            NPPlayer.instance.commonItemCountComp.setCount(GRefdataCoreMgr.instance.npGeneral.guild_exp_common_item, _m_guildInfo?.exp ?? 0);
        }
        
        /// <summary>
        /// 更新联盟财富CommonItem数量
        /// </summary>
        private void _updateGuildWealthCommonItemCount()
        {
            // 联盟财富数量变更
            NPPlayer.instance.commonItemCountComp.setCount(GRefdataCoreMgr.instance.npGeneral.guild_wealth_common_item, _m_guildInfo?.guildWealth ?? 0);
        }

        /// <summary>
        /// 更新个人贡献度变更CommonItem数量
        /// </summary>
        private void _updateSelfTotalContribution()
        {
            // 个人贡献度变更
            NPPlayer.instance.commonItemCountComp.setCount(GRefdataCoreMgr.instance.npGeneral.personal_contribution_common_item, _m_guildInfo == null ? 0 : _m_lSelfTotalContribution);
        }

        #region 红点

        //刷新全部红点
        private void _refreshAllRedTip()
        {
            _refreshJoinRedTip();
            _refreshFreeDonateRedTip();
            _refreshDonateRewardRedTip();
            _refreshDonateByCoinRedTip();
            _refreshDispatchRedTip();
            _refreshEntrustRedTip();
            _refreshApplyRedTip();
            _refreshGuildBattleReportRedTip();
        }

        //刷新加入联盟红点
        private void _refreshJoinRedTip()
        {
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_NOT_JOIN, isJoinGuild() ? 0 : 1);
        }

        //刷新捐赠红点
        private void _refreshFreeDonateRedTip()
        {
            //免费捐赠次数
            long freeDonateCount = 0;

            if (isJoinGuild())
            {
                GRefdataCoreMgr.instance.guildConstructRefCore.dealAllRef(_constructRef =>
                {
                    //是否是免费的并且有剩余次数
                    if (_constructRef != null && (_constructRef.cost == null ||
                                                  !_constructRef.cost.IsValid ||
                                                  (_constructRef.free_condition != null && !_constructRef.free_condition.isEmpty && _constructRef.free_condition.IsEnable(null))))
                    {
                        NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_constructRef.fix_cd_id);
                        if (cdInfo != null && cdInfo.getCount() > 0)
                        {
                            freeDonateCount += cdInfo.getCount();
                        }
                    }
                });
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_FREE_DONATE, freeDonateCount);
        }

        //刷新捐赠奖励红点
        private void _refreshDonateRewardRedTip()
        {
            long rewardCount = 0;

            if (isJoinGuild())
            {
                GRefdataCoreMgr.instance.guildConstructRewardRefCore.dealAllRef(_constructRewardRef =>
                {
                    if (_constructRewardRef != null)
                    {
                        // 获取当前分数
                        long curScore = _m_guildInfo?.guildConstructList?.rewardPoint ?? 0;
                        if (curScore >= _constructRewardRef.num)//若当前进度达到本阶段需要进度(当前分数 >= 本阶段分数)
                        {
                            if (selfDailyData != null && !selfDailyData.hasDrawConstructReward((int)_constructRewardRef.num))
                                rewardCount++;
                        }
                    }
                });
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_DONATE_REWARD, rewardCount);
        }

        //刷新捐赠可使用金币红点
        private void _refreshDonateByCoinRedTip()
        {
            bool haveCoinDonate = false;
            if (isJoinGuild())
            {
                for (int i = 0; i < GRefdataCoreMgr.instance.guildConstructRefCore.refList.Count; i++)
                {
                    GuildConstructRefObj _constructRef = GRefdataCoreMgr.instance.guildConstructRefCore.refList[i];
                    if (_constructRef != null && 
                        _constructRef.cost != null && 
                        _constructRef.cost.IsValid && 
                        _constructRef.cost.getCurrencyType() == ECurrency.SILVER && 
                        GCommon.isItemEnough(_constructRef.cost,false))
                    {
                        NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_constructRef.fix_cd_id);
                        if (cdInfo != null && cdInfo.getCount() > 0)
                        {
                            haveCoinDonate = true;
                            break;
                        }
                    }
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_CAN_DONATE_BY_COIN, haveCoinDonate ? 1 : 0);
        }

        //刷新派遣代表红点
        private void _refreshDispatchRedTip()
        {
            bool canDispatchHero = false;
            if (isJoinGuild())
            {
                long curDispatchHeroId = _m_guildInfo.getDispatchHeroId(NPPlayer.instance.playerInfo.CID);

                //未派遣代表
                if (curDispatchHeroId <= 0 && _m_guildInfo.dispatchInfoList != null)
                {
                    for (int i = 0; i < _m_guildInfo.dispatchInfoList.Count; i++)
                    {
                        GuildDispatchInfo dispatchInfo = _m_guildInfo.dispatchInfoList[i];
                        //是否还有空位并且有对应属性伙伴
                        if (dispatchInfo != null &&
                            (dispatchInfo.dispatchHeroInfoList == null || 
                            dispatchInfo.dispatchHeroInfoList.Count < GRefdataCoreMgr.instance.npGeneral.each_attr_can_dispatch_hero_num) && 
                            NPPlayer.instance.heroComponent.getOwnCount(_info => { return _info.specAttrType == dispatchInfo.specAttrType; }) > 0)
                        {
                            canDispatchHero = true;
                            break;
                        }
                    }
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_CAN_DISPATCH, canDispatchHero ? 1 : 0);
        }

        /// <summary>
        /// 刷新联盟事务红点
        /// </summary>
        private void _refreshEntrustRedTip()
        {
            long entrustCount = 0;
            if (isJoinGuild())
            {
                PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(GRefdataCoreMgr.instance.npGeneral.deal_entrust_lazy_cd_id);
                //次数达到20才显示红点
                entrustCount = lazyCdInfo != null && (lazyCdInfo.getCount() >= 20) ? lazyCdInfo.getCount() : 0;
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_HAVE_ENTRUST, entrustCount);
        }

        //刷新加入请求红点
        private void _refreshApplyRedTip()
        {
            long applyCount = 0;
            if (isJoinGuild() && checkHavePermission(EGuildPermissionType.PROCESS_JOIN_REQUEST))
            {

                List<GuildJoinRequestInfo> infoList = _m_guildInfo.joinRequestList;
                applyCount = infoList != null ? infoList.Count : 0;
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_APPLY, applyCount);
        }

        //刷新联盟火星矿战报红点
        private void _refreshGuildBattleReportRedTip()
        {
            long myGuildId = _m_guildInfo?.guildId ?? 0;
            long readReportId = AccountSettingMgr.instance.accountSetting.readMarsExploreGuildBattleReportId;
            long readGuildId = AccountSettingMgr.instance.accountSetting.readMarsExploreGuildBattleReportGuildId;

            bool needShow = _m_latestMarsMineBattleReportId > 0 && myGuildId > 0 &&
                            (readGuildId != myGuildId || _m_latestMarsMineBattleReportId > readReportId);

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_MARS_EXPLORE_GUILD_BATTLE_REPORT, needShow ? 1 : 0);
        }

        #endregion

        #region S2C

        /// <summary>
        /// 联盟初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retGuildInit(GS2GC_002_063_RetGuildInit _msg)
        {
            if (_msg == null)
                return;

            //设置联盟信息
            _m_guildInfo = new GuildInfo(_msg);

            //设置加入联盟cd
            if (_msg.getJoinCdInfo() != null)
            {
                _m_iGuildFreeJoinCdNum = _msg.getJoinCdInfo().getGuildFreeJoinCdNum();
                _m_lJoinGuildCdEndTimeMs = _msg.getJoinCdInfo().getJoinGuildCdEndTimeMs();
            }

            //设置请求入盟记录
            _m_lReqJoinGuildList = new HashSet<long>();
            if (_msg.getSelfRequestJoinGuildList() != null && _msg.getSelfRequestJoinGuildList().Count > 0)
                _m_lReqJoinGuildList.AddAll(_msg.getSelfRequestJoinGuildList().ToArray());

            //更新自己的贡献度
            if (_msg.getGuildInfo() != null && _msg.getGuildInfo().getSelfContributeInfo() != null)
            {
                _m_lSelfSevenDaysContribution = _msg.getGuildInfo().getSelfContributeInfo().getSevenDaysContribute();
                _m_lSelfTotalContribution = _msg.getGuildInfo().getSelfContributeInfo().getTotalContribute();
            }

            _updateSelfDailyData(_msg.getDailyData());

            // 保存最新联盟火星矿战报ID
            _m_latestMarsMineBattleReportId = _msg.getGuildInfo()?.getLatestMarsMineBattleReportId() ?? 0;

            if (isJoinGuild())//若有加入联盟, 加入联盟聊天室
            {
                NPPlayer.instance.chatComp.joinChatRoom(ENPChatRoomType.GUILD, 0, 60, 1f, true, false);
            }

            _updateGuildExpCommonItemCount();
            _updateGuildWealthCommonItemCount();
            _updateSelfTotalContribution();

            // 添加联盟属性加成
            _onGuildAddInitUnionBonusMgr();

            //完成初始化
            setInitDone();
        }

        /// <summary>
        /// 联盟展示信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildShowInfoChg(GS2GC_032_050_OnGuildShowInfoChg _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.updateShowInfo(_msg.getShowInfo());

            // 联盟经验数量变更
            _updateGuildExpCommonItemCount();

            // 刷新红点
            _refreshJoinRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_SHOW_INFO_CHG);
        }

        /// <summary>
        /// 联盟成员基础信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onMemberBaseInfoChg(GS2GC_032_051_OnMemberBaseInfoChg _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.updateMemberBaseInfo(_msg.getBaseInfo());
            GCommon.reloadCustomLoadPrefab();
            //刷新红点
            _refreshApplyRedTip();
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MEMBER_BASE_INFO_CHG, _msg.getBaseInfo().getCid());
        }

        /// <summary>
        /// 联盟公告信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildAnnouncementChg(GS2GC_032_052_OnGuildAnnouncementChg _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.updateAnnouncement(_msg.getAnnouncement());
        }

        /// <summary>
        /// 联盟财富变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildWealthChg(GS2GC_032_053_OnGuildWealthChg _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.updateWealth(_msg.getGuildWealth());
            
            // 联盟财富数量变更
            _updateGuildWealthCommonItemCount();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_WEALTH_CHG);
        }

        /// <summary>
        /// 联盟成员新增
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildMemberAdd(GS2GC_032_054_OnGuildMemberAdd _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.addMember(_msg.getBaseInfo());

            WinMsg.SendMsg(WinMsgType.ON_GUILD_MEMBER_ADD, _msg.getBaseInfo());
        }

        /// <summary>
        /// 联盟成员移除
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildMemberRemove(GS2GC_032_055_OnGuildMemberRemove _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.removeMember(_msg.getMemberId());

            WinMsg.SendMsg(WinMsgType.ON_GUILD_MEMBER_REMOVE, _msg.getMemberId());
        }

        /// <summary>
        /// 联盟自己的贡献变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onSelfGuildContributeChg(GS2GC_032_056_OnSelfGuildContributeChg _msg)
        {
            if (_msg == null || _msg.getContributeInfo() == null)
                return;

            _m_lSelfSevenDaysContribution = _msg.getContributeInfo().getSevenDaysContribute();
            _m_lSelfTotalContribution = _msg.getContributeInfo().getTotalContribute();
            _updateSelfTotalContribution();
        }

        /// <summary>
        /// 自己请求加入联盟列表变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onSelfRequestJoinGuildListChg(GS2GC_032_057_OnSelfRequestJoinGuildListChg _msg)
        {
            if (_msg == null)
                return;

            _m_lReqJoinGuildList?.Clear();
            if (_msg.getSelfRequestJoinGuildList() != null && _msg.getSelfRequestJoinGuildList().Count != 0)
                _m_lReqJoinGuildList?.AddAll(_msg.getSelfRequestJoinGuildList().ToArray());
        }

        /// <summary>
        /// 加入联盟CD变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onJoinGuildCdChg(GS2GC_032_058_OnJoinGuildCdChg _msg)
        {
            if (_msg == null || _msg.getJoinCdInfo() == null)
                return;

            _m_iGuildFreeJoinCdNum = _msg.getJoinCdInfo().getGuildFreeJoinCdNum();
            _m_lJoinGuildCdEndTimeMs = _msg.getJoinCdInfo().getJoinGuildCdEndTimeMs();
        }

        /// <summary>
        /// 联盟事件新增
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildEventAdd(GS2GC_032_059_OnGuildEventAdd _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.addGuildEvent(_msg.getEventInfo());

            WinMsg.SendMsg(WinMsgType.ON_GUILD_EVENT_ADD, _msg.getEventInfo());
        }

        /// <summary>
        /// 联盟事件变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildEventChg(GS2GC_032_060_OnGuildEventChg _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.chgGuildEvent(_msg.getEventInfo());

            WinMsg.SendMsg(WinMsgType.ON_GUILD_EVENT_CHG, _msg.getEventInfo());
        }

        /// <summary>
        /// 联盟事件移除
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildEventRemove(GS2GC_032_061_OnGuildEventRemove _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.removeGuildEvent(_msg.getEventDbId());

            WinMsg.SendMsg(WinMsgType.ON_GUILD_EVENT_REMOVE, _msg.getEventDbId());
        }

        /// <summary>
        /// 联盟总收益变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildTotalEarningsChg(GS2GC_032_062_OnGuildTotalEarningsChg _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.updateTotalEarnings(_msg.getTotalEarnings());
        }

        /// <summary>
        /// 新增入盟请求
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildJoinRequestAdd(GS2GC_032_063_OnGuildJoinRequestAdd _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.addJoinRequest(_msg.getJoinRequestList());
            //刷新红点
            _refreshApplyRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_JOIN_REQUEST_ADD);
        }

        /// <summary>
        /// 移除入盟请求
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildJoinRequestRemove(GS2GC_032_064_OnGuildJoinRequestRemove _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.removeJoinRequest(_msg.getRequestDbIdList());
            //刷新红点
            _refreshApplyRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_JOIN_REQUEST_REMOVE);
        }

        /// <summary>
        /// 加入联盟推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onJoinGuild(GS2GC_032_065_OnJoinGuild _msg)
        {
            if (_msg == null)
                return;

            //设置联盟信息
            _m_guildInfo = new GuildInfo(_msg);
            //更新自己的贡献度
            if (_msg.getGuildInfo() != null && _msg.getGuildInfo().getSelfContributeInfo() != null)
            {
                _m_lSelfSevenDaysContribution = _msg.getGuildInfo().getSelfContributeInfo().getSevenDaysContribute();
                _m_lSelfTotalContribution = _msg.getGuildInfo().getSelfContributeInfo().getTotalContribute();
            }
            
            // 保存最新联盟火星矿战报ID
            _m_latestMarsMineBattleReportId = _msg.getGuildInfo()?.getLatestMarsMineBattleReportId() ?? 0;

            if (isJoinGuild())//若有加入联盟, 加入联盟聊天室
            {
                NPPlayer.instance.chatComp.joinChatRoom(ENPChatRoomType.GUILD, 0,60, 1f, true, false);
            }
            else
            {
                NPPlayer.instance.chatComp.quitChatRoom(ENPChatRoomType.GUILD, 0);
            }

            _updateGuildExpCommonItemCount();
            _updateGuildWealthCommonItemCount();
            _updateSelfTotalContribution();

            // 添加联盟属性加成
            _onGuildAddInitUnionBonusMgr();

            //刷新红点
            _refreshAllRedTip();

            //检查联盟相关本地存储
            AccountSettingMgr.instance.guildSaver.checkGuildId();

            WinMsg.SendMsg(WinMsgType.ON_JOIN_GUILD, _msg.getIsCreate());
        }

        /// <summary>
        /// 联盟火星矿战报新增推送
        /// </summary>
        public void onGuildMarsBattleReportAdd(GS2GC_032_081_OnGuildMarsBattleReportAdd _msg)
        {
            if (_msg == null)
                return;

            _m_latestMarsMineBattleReportId = _msg.getId();
            _refreshGuildBattleReportRedTip();
        }

        /// <summary>
        /// 请求联盟战报列表
        /// </summary>
        public void reqGuildBattleReport(Action<List<Mars_GuildBattleReportIdx>> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_048_ReqGuildMarsBattleReportList(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_048_RetGuildMarsBattleReportList>((_isSuc, _msg) =>
                    _complete?.Invoke(_msg?.getReportList())));

            // 更新已读联盟战报ID，并刷新红点
            long myGuildId = _m_guildInfo?.guildId ?? 0;
            AccountSettingMgr.instance.accountSetting?.setReadMarsExploreGuildBattleReportId(_m_latestMarsMineBattleReportId, myGuildId);
            _refreshGuildBattleReportRedTip();
        }

        /// <summary>
        /// 退出联盟推送
        /// </summary>
        public void onLeaveGuild(GS2GC_032_066_OnLeaveGuild _msg)
        {
            // 移除联盟属性加成
            _onGuildRemoveUnionBonusMgrRemove();
            
            _m_guildInfo = null;
            _m_lSelfSevenDaysContribution = 0;
            _m_lSelfTotalContribution = 0;
            _m_lRecordLastSelectDispatchHeroId = 0;

            NPPlayer.instance.chatComp.quitChatRoom(ENPChatRoomType.GUILD, 0);//退出联盟聊天室
            
            _updateGuildExpCommonItemCount();
            _updateGuildWealthCommonItemCount();
            _updateSelfTotalContribution();

            //刷新红点
            _refreshAllRedTip();

            //检查联盟相关本地存储
            AccountSettingMgr.instance.guildSaver.checkGuildId();

            WinMsg.SendMsg(WinMsgType.ON_LEAVE_GUILD, _msg?.getIsKick());
        }

        /// <summary>
        /// 联盟建造次数信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildConstructListChg(GS2GC_032_067_OnGuildConstructListChg _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.updateConstructInfo(_msg.getConstructList());
            //刷新红点
            _refreshDonateRewardRedTip();
            _refreshDonateByCoinRedTip();
        }

        /// <summary>
        /// 联盟招募CD变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onOpenRecruitCdChg(GS2GC_032_068_OnOpenRecruitCdChg _msg)
        {
            if (_msg == null)
                return;

            _m_guildInfo?.updateNextCanRecruitTimeMs(_msg.getNextCanRecruitTimeMs());
            WinMsg.SendMsg(WinMsgType.ON_GUILD_CAN_RECRUIT_TIME_CHG);
        }
        
        /// <summary>
        /// 杂物委托变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildEntrustChg(GS2GC_032_071_OnGuildEntrustChg _msg)
        {
            if(!isInited || _msg == null || _m_guildInfo == null)
                return;

            _m_guildInfo.updateGuildEntrustInfo(_msg.getData());
        }

        /// <summary>
        /// 派遣信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildDispatchChg(GS2GC_032_072_OnGuildDispatchChg _msg)
        {
            if(!isInited || _msg == null || _m_guildInfo == null)
                return;

            Guild_AttrDispatchInfo dispatchInfo = _msg.getData();
            if(dispatchInfo == null)
                return;
            
            // 移除旧的加成
            _m_unionBonusMgr.removeValue(EBonusFilterType.BUILDING_ATTR, (long)dispatchInfo.getAttr(), EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _m_guildInfo.getDispatchTotalAddPer(dispatchInfo.getAttr()));

            // 更新派遣信息
            _m_guildInfo.updateDispatchInfo(dispatchInfo);
            
            // 添加新的加成
            _m_unionBonusMgr.addValue(EBonusFilterType.BUILDING_ATTR, (long)dispatchInfo.getAttr(), EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _m_guildInfo.getDispatchTotalAddPer(dispatchInfo.getAttr()));

            //刷新红点
            _refreshDispatchRedTip();
        }

        /// <summary>
        /// 自身每日数据变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onSelfDailyDataChg(GS2GC_032_073_OnSelfDailyDataChg _msg)
        {
            if(!isInited || _msg == null)
                return;
            
            _updateSelfDailyData(_msg.getData());
        }
        
        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化
        /// </summary>
        public void reqGuildInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_063_ReqGuildInit());
        }
        
        /// <summary>
        /// 请求创建联盟
        /// </summary>
        /// <param name="_flagId"></param>
        /// <param name="_name"></param>
        /// <param name="_simpleName"></param>
        /// <param name="_declaration"></param>
        /// <param name="_callback"></param>
        public void reqCreateGuild(long _flagId, string _name, string _simpleName, string _declaration, bool _canFreeJoin, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_001_ReqCreateGuild(_flagId, _name, _simpleName, _declaration, _canFreeJoin),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_001_RetCreateGuild>((_msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求加入联盟
        /// </summary>
        /// <param name="_guildId"></param>
        /// <param name="_callback"></param>
        public void reqJoinGuild(long _guildId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_002_ReqJoinGuild(_guildId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_002_RetJoinGuild>((_succ, _msg) =>
                {
                    _callback?.Invoke(_succ);
                }));
        }

        /// <summary>
        /// 请求随机加入联盟
        /// </summary>
        /// <param name="_callback"></param>
        public void reqRandomJoinGuild(Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_003_ReqRandomJoinGuild(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_003_RetRandomJoinGuild>(_mg =>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求其他联盟信息
        /// </summary>
        /// <param name="_callback"></param>
        public void reqOtherGuildInfo(long _guildId, Action<GS2GC_032_004_RetOtherGuildInfo> _callback, Action _onFail, bool _needShowErrorCode = true)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_004_ReqOtherGuildInfo(_guildId),
                new CommonRequestCallbackProtocolDealer<GS2GC_032_004_RetOtherGuildInfo>((_msg) =>
                {
                    _callback?.Invoke(_msg);
                }, (_errorCode) =>
                {
                    if(_needShowErrorCode)
                        NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                    _onFail?.Invoke();
                }));
        }

        /// <summary>
        /// 请求转让联盟
        /// </summary>
        /// <param name="_callback"></param>
        public void reqTransferGuild(long _memberId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_005_ReqTransferGuild(_memberId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_005_RetTransferGuild>((_isSuc,_msg)=>
                {
                    _callback?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 请求解散联盟
        /// </summary>
        /// <param name="_callback"></param>
        public void reqDissolveGuild(Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_006_ReqDissolveGuild(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_006_RetDissolveGuild>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求联盟职位任命
        /// </summary>
        /// <param name="_memberId"></param>
        /// <param name="_positionId"></param>
        /// <param name="_callback"></param>
        public void reqGuildPositionAppoint(long _memberId, long _positionId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_007_ReqGuildPositionAppoint(_memberId, _positionId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_007_RetGuildPositionAppoint>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 请求变更联盟旗帜
        /// </summary>
        /// <param name="_flagId"></param>
        /// <param name="_callback"></param>
        public void reqChgGuildFlag(long _flagId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_008_ReqChgGuildFlag(_flagId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_008_RetChgGuildFlag>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求变更联盟名称
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_simpleName"></param>
        /// <param name="_callback"></param>
        public void reqChgGuildName(string _name, string _simpleName, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_009_ReqChgGuildName(_name, _simpleName),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_009_RetChgGuildName>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求变更联盟宣言
        /// </summary>
        /// <param name="_declaration"></param>
        /// <param name="_callback"></param>
        public void reqChgGuildDeclaration(string _declaration, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_010_ReqChgGuildDeclaration(_declaration),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_010_RetChgGuildDeclaration>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求变更联盟公告
        /// </summary>
        /// <param name="_announcement"></param>
        /// <param name="_callback"></param>
        public void reqChgGuildAnnouncement(string _announcement, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_011_ReqChgGuildAnnouncement(_announcement),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_011_RetChgGuildAnnouncement>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 切换联盟加入类型
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_callback"></param>
        public void reqSetGuildJoinType(EGuildJoinType _type, List<Common.GuildObj.Guild_JoinLimitInfo> _joinLimitInfoList, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_012_ReqSetGuildJoinType(_type, _joinLimitInfoList),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_012_RetSetGuildJoinType>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 处理联盟加入请求
        /// </summary>
        /// <param name="_requestId"></param>
        /// <param name="_isAccept"></param>
        /// <param name="_callback"></param>
        public void reqProcessGuildJoinRequest(long _requestId, bool _isAccept, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_013_ReqProcessGuildJoinRequest(_requestId, _isAccept),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_013_RetProcessGuildJoinRequest>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求踢出联盟成员
        /// </summary>
        /// <param name="_memberId"></param>
        /// <param name="_callback"></param>
        public void reqGuildKickOutMember(long _memberId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_014_ReqGuildKickOutMember(_memberId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_014_RetGuildKickOutMember>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 请求群发消息
        /// </summary>
        /// <param name="_cidList"></param>
        /// <param name="_message"></param>
        /// <param name="_callback"></param>
        public void reqGuildBroadcastMessage(List<long> _cidList, string _message, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_015_ReqGuildBroadcastMessage(_cidList, _message),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_015_RetGuildBroadcastMessage>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求退出联盟
        /// </summary>
        /// <param name="_callback"></param>
        public void reqLeaveGuild(Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_016_ReqLeaveGuild(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_016_RetLeaveGuild>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求建造联盟
        /// </summary>
        /// <param name="_refId"></param>
        /// <param name="_callback"></param>
        public void reqGuildConstruct(long _refId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_017_ReqGuildConstruct(_refId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_017_RetGuildConstruct>(_msg=>
                {
                    _m_constructRemarkInfo?.setRecord(_refId);
                    _callback?.Invoke();
                    WinMsg.SendMsg(WinMsgType.ON_GUILD_CONSTRUCT_DONE);
                }));
        }

        /// <summary>
        /// 请求弹劾盟主
        /// </summary>
        /// <param name="_callback"></param>
        public void reqGuildImpeachLeader(long _eventDbId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_018_ReqGuildImpeachLeader(_eventDbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_018_RetGuildImpeachLeader>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求联盟成员贡献
        /// </summary>
        /// <param name="_callback"></param>
        public void reqGuildMemberContribute(long _cid, Action<GS2GC_032_019_RetMemberContribute> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_019_ReqMemberContribute(_cid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_019_RetMemberContribute>(_msg=>
                {
                    _callback?.Invoke(_msg);
                }));
        }

        /// <summary>
        /// 请求搜索联盟
        /// </summary>
        /// <param name="_callback"></param>
        public void reqSearchGuild(string _searchData, Action<bool, GS2GC_032_021_RetSearchGuild> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_021_ReqSearchGuild(_searchData),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_021_RetSearchGuild>((_succ, _msg)=>
                {
                    _callback?.Invoke(_succ, _msg);
                }));
        }

        /// <summary>
        /// 一键处理入盟请求
        /// </summary>
        public void reqGuildJoinRequestAKeyDeal(bool _isAgree, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_023_ReqGuildJoinRequestAKeyDeal(_isAgree),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_023_RetGuildJoinRequestAKeyDeal>(_msg=>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求取消自己的入盟请求
        /// </summary>
        /// <param name="_guildId"></param>
        public void reqCancelSelfJoinRequest(long _guildId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_024_ReqCancelSelfJoinRequest(_guildId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_024_RetCancelSelfJoinRequest>((_succ, _msg) =>
                {
                    _callback?.Invoke(_succ);
                }));
        }

        /// <summary>
        /// 取消弹劾事件
        /// </summary>
        /// <param name="_eventDbId"></param>
        /// <param name="_callback"></param>
        public void reqCancelLeaderImpeachEvent(long _eventDbId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_025_ReqCancelLeaderImpeachEvent(_eventDbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_025_RetCancelLeaderImpeachEvent>((_msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 批准弹劾事件
        /// </summary>
        /// <param name="_eventDbId"></param>
        /// <param name="_callback"></param>
        public void reqApproveLeaderImpeachEvent(long _eventDbId, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_026_ReqApproveLeaderImpeachEvent(_eventDbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_026_RetApproveLeaderImpeachEvent>((_msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求公开招募
        /// </summary>
        /// <param name="_callback"></param>
        public void reqOpenRecruit(Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_027_ReqOpenRecruit(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_027_RetOpenRecruit>((_msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求处理杂物委托
        /// </summary>
        /// <param name="_onSucc"></param>
        /// <param name="_onFail"></param>
        public void reqGuildEntrust(Action<GS2GC_032_033_RetGuildEntrust> _onSucc, Action _onFail)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_033_ReqGuildEntrust(),
                new CommonRequestCallbackProtocolDealer<GS2GC_032_033_RetGuildEntrust>((_msg) =>
                {
                    _onSucc?.Invoke(_msg);
                }, (_errCode) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _onFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求联盟派遣大臣
        /// </summary>
        /// <param name="_onSucc"></param>
        /// <param name="_onFail"></param>
        public void reqGuildDispatchHero(long _heroId, Action<GS2GC_032_034_RetGuildDispatchHero> _onSucc, Action _onFail)
        {
            //记录最后一次派遣的大臣ID
            recordLastSelectDispatchHeroId = _heroId;

            NPGSClientListener.sendRequestByLog(new GC2GS_032_034_ReqGuildDispatchHero(_heroId),
                new CommonRequestCallbackProtocolDealer<GS2GC_032_034_RetGuildDispatchHero>((_msg) =>
                {
                    _onSucc?.Invoke(_msg);
                }, (_errCode) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _onFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求联盟所有成员的委托信息
        /// </summary>
        /// <param name="_onSucc"></param>
        /// <param name="_onFail"></param>
        public void reqGuildAllMemberEntrustInfo(Action<GS2GC_032_035_RetGuildAllMemberEntrustInfo> _onSucc, Action _onFail)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_035_ReqGuildAllMemberEntrustInfo(),
                new CommonRequestCallbackProtocolDealer<GS2GC_032_035_RetGuildAllMemberEntrustInfo>((_msg) =>
                {
                    _onSucc?.Invoke(_msg);
                }, (_errCode) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _onFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求领取捐赠进度奖励
        /// </summary>
        /// <param name="_onSucc"></param>
        /// <param name="_onFail"></param>
        public void reqDrawConstructReward(int _num, Action<GS2GC_032_037_RetDrawConstructReward> _onSucc, Action _onFail)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_037_ReqDrawConstructReward(_num),
                new CommonRequestCallbackProtocolDealer<GS2GC_032_037_RetDrawConstructReward>((_msg) =>
                {
                    _onSucc?.Invoke(_msg);
                }, (_errCode) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _onFail?.Invoke();
                }));
        }
                
        /// <summary>
        /// 联盟派遣大臣列表
        /// </summary>
        /// <param name="_callback"></param>
        public void reqGuildDispatchHeroList(Action<bool, List<Guild_DispatchHeroDetailInfo>> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_039_ReqGuildDispatchHeroList(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_039_RetGuildDispatchHeroList>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc, _msg?.getInfoList());
                }, null, false));
        }

        /// <summary>
        /// 请求联盟日志列表
        /// </summary>
        /// <param name="_lastDbId"></param>
        /// <param name="_num"></param>
        /// <param name="_callback"></param>
        public void reqGuildLogList(long _lastDbId, int _num, Action<List<Guild_LogInfo>> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_040_ReqGuildLogList(_lastDbId, _num),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_032_040_RetGuildLogList>((_msg) =>
                {
                    _callback?.Invoke(_msg.getInfoList());
                }));
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 当玩家参数变化
        /// </summary>
        private void _onPlayerParamChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is ENPPlayerParam _playerParam))
                return;

            switch (_playerParam)
            {
                case ENPPlayerParam.LAST_LOGIN_DATE:
                    _onDayTagChg();
                    break;
                
                case ENPPlayerParam.LAST_LOGIN_WEEK_TAG:
                    break;
            }
        }
        
        /// <summary>
        /// 当天标记变更
        /// </summary>
        private void _onDayTagChg()
        {
            // 获取当前日期tag
            int curDayTag = (int)NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LAST_LOGIN_DATE);
            
            // 若跨天了, 联盟建设数据还未更新的话, 重置联盟建设数据
            if (_m_selfDailyData != null && _m_selfDailyData.date != curDayTag)
                _m_selfDailyData.resetData();

            if (_m_guildInfo != null)
            {
                _m_guildInfo.onDayTagChg();
            }

            //刷新红点
            _refreshFreeDonateRedTip();
            _refreshDonateRewardRedTip();
            _refreshDonateByCoinRedTip();
        }

        /// <summary>
        /// 更新条件消息
        /// </summary>
        private void _onCustomReload()
        {
            _refreshFreeDonateRedTip();
            _refreshEntrustRedTip();
        }

        /// <summary>
        /// 节点切换
        /// </summary>
        private void _onNodeChg()
        {
            _refreshEntrustRedTip();
        }

        /// <summary>
        /// FixedCD计数变化
        /// </summary>
        private void _onFixedCDCountChg(params object[] _objects)
        {
            _refreshFreeDonateRedTip();
            _refreshDonateByCoinRedTip();
        }

        /// <summary>
        /// 获取伙伴
        /// </summary>
        /// <param name="_objects"></param>
        private void _onGainHero(params object[] _objects)
        {
            _refreshDispatchRedTip();
        }

        /// <summary>
        /// LazyCD变化
        /// </summary>
        /// <param name="_objects"></param>
        private void _onLazyCDChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length <= 0)
                return;

            long cdId = (long)_objects[0];

            //联盟事务
            if (cdId == GRefdataCoreMgr.instance.npGeneral.deal_entrust_lazy_cd_id)
                _refreshEntrustRedTip();
        }

        /// <summary>
        /// 货币类型资源变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onCurrencyChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length <= 0)
                return;

            ECurrency type = (ECurrency) _objects[0];
            if(type == ECurrency.SILVER)
                _refreshDonateByCoinRedTip();
        }

        #endregion

        #region 属性管理器

        /// <summary>
        /// 当添加了联盟时, 加成属性的初始化
        /// </summary>
        private void _onGuildAddInitUnionBonusMgr()
        {
            // 委托大臣加成
            if (_m_guildInfo != null)
            {
                foreach (var dispatchInfo in _m_guildInfo.dispatchInfoList)
                {
                    if(dispatchInfo != null)
                        _m_unionBonusMgr.addValue(EBonusFilterType.BUILDING_ATTR, (long)dispatchInfo.specAttrType, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, dispatchInfo.totalAddPer);
                }
            }
        }

        /// <summary>
        /// 当添加了联盟时, 加成属性的初始化
        /// </summary>
        private void _onGuildRemoveUnionBonusMgrRemove()
        {
            // 委托大臣加成
            if (_m_guildInfo != null)
            {
                foreach (var dispatchInfo in _m_guildInfo.dispatchInfoList)
                {
                    if(dispatchInfo != null)
                        _m_unionBonusMgr.removeValue(EBonusFilterType.BUILDING_ATTR, (long)dispatchInfo.specAttrType, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, dispatchInfo.totalAddPer);
                }
            }
        }

        #endregion
    }
}
