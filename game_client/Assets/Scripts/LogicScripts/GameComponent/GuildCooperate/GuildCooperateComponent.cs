using ALPackage;
using Common.GuildCooperateObj;
using GC2GS.p032_GuildOp;
using GS2GC.p002_InitOp;
using GS2GC.p032_GuildOp;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟协作任务管理器
    /// </summary>
    public class GuildCooperateComponent : _ANPBasicPlayerComponent
    {
        //下次刷新时间
        private long _m_lNextRefreshTimeMs;
        //重置次数
        private long _m_lResetCount;
        //当前区域ID
        private long _m_lCurAreaId;
        //推荐奖励据点信息
        private GuildCooperateRecommendPointInfo _m_recommendPointInfo;
        //奖励据点信息字典 <区域id，奖励据点信息列表>
        private Dictionary<long, List<GuildCooperateRewardPointInfo>> _m_dRewardPointInfoDic;
        //已领取奖励点字典 <区域id，已领奖据点下标列表>
        private Dictionary<long, List<int>> _m_dHadDrawRewardPointsDic;
        //伙伴使用记录信息字典 <伙伴id，伙伴使用信息>
        private Dictionary<long, GuildCooperate_HeroUseInfo> _m_dHeroUseRecordDic;
        //定时任务
        private ALCommonEnableTaskController _m_checkTask;
        //是否正在请求新的初始化
        private bool _m_bIsReqNewInit;
        //记录当前界面进入的联盟协作刷新时间
        private long _m_lRecordCurShowRefreshTimeMs;

        public GuildCooperateComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.GUILD };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.GUILD_COOPERATE; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 下次刷新时间
        /// </summary>
        public long nextRefreshTimeMs { get { return _m_lNextRefreshTimeMs; } }
        /// <summary>
        /// 公会协作重置次数
        /// </summary>
        public long resetCount { get { return _m_lResetCount; } }
        /// <summary>
        /// 当前区域ID
        /// </summary>
        public long curAreaId { get { return _m_lCurAreaId; } }
        /// <summary>
        /// 记录当前界面进入的联盟协作刷新时间
        /// </summary>
        public long recordCurShowRefreshTimeMs { get { return _m_lRecordCurShowRefreshTimeMs; } set { _m_lRecordCurShowRefreshTimeMs = value; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqGuildCooperateInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
            WinMsg.RegisterMsg(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_GAIN, _onHeroGain);
            _startCheck();
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            _refreshAllRedTip();
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("GuildCooperateComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
            WinMsg.UnregisterMsg(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_GAIN, _onHeroGain);
            _clear();
            _stopCheck();
        }

        //析构函数
        private void _clear()
        {
            _m_lNextRefreshTimeMs = 0;
            _m_lCurAreaId = 0;
            _m_recommendPointInfo = null;
            _m_dRewardPointInfoDic?.Clear();
            _m_dHadDrawRewardPointsDic?.Clear();
            _m_dHeroUseRecordDic?.Clear();
        }

        /// <summary>
        /// 是否是推荐奖励据点
        /// </summary>
        public bool isRecommendPoint(long _areaId, int _index)
        {
            if (_m_recommendPointInfo == null)
                return false;

            return _m_recommendPointInfo.areaId == _areaId && _m_recommendPointInfo.index == _index;
        }

        /// <summary>
        /// 伙伴是否可使用
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public bool isHeroCanUse(long _heroId)
        {
            if (_m_dHeroUseRecordDic == null)
                return true;

            if (_m_dHeroUseRecordDic.TryGetValue(_heroId, out GuildCooperate_HeroUseInfo useInfo))
            {
                if (useInfo == null)
                    return true;

                //比较时间，如果已经跨天，则使用次数重置可以使用
                if (TimeUtil.getTimeByYYYYMM(useInfo.getLastRefreshTimeMs()) !=
                    TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag))
                    return true;

                //使用次数小于等于恢复次数则可使用
                return useInfo.getUseCount() <= useInfo.getRecoveredCount();
            }
            else
                return true;
        }

        /// <summary>
        /// 获取奖励据点列表
        /// </summary>
        /// <param name="_areaId"></param>
        /// <returns></returns>
        public List<GuildCooperateRewardPointInfo> getRewardPointList(long _areaId)
        {
            if (_m_dRewardPointInfoDic == null)
                return null;

            if (_m_dRewardPointInfoDic.TryGetValue(_areaId, out List<GuildCooperateRewardPointInfo> _infoList))
                return _infoList;

            return null;
        }

        /// <summary>
        /// 奖励据点是否已领取奖励
        /// </summary>
        /// <param name="_areaId"></param>
        /// <param name="_index"></param>
        /// <returns></returns>
        public bool isRewardPointGetReward(long _areaId, int _index)
        {
            if (_m_dHadDrawRewardPointsDic == null)
                return false;

            if (_m_dHadDrawRewardPointsDic.TryGetValue(_areaId, out List<int> indexList))
            {
                if (indexList != null && indexList.Contains(_index))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取区域状态
        /// </summary>
        /// <param name="_areaId"></param>
        /// <returns></returns>
        public EGuildCooperateMapAreaState getAreaState(long _areaId)
        {
            if (_m_dRewardPointInfoDic == null)
                return EGuildCooperateMapAreaState.LOCK;

            bool canGetReward = false;
            if (_m_dRewardPointInfoDic.TryGetValue(_areaId, out List<GuildCooperateRewardPointInfo> rewardPointInfoList))
            {
                if (rewardPointInfoList == null)
                    return EGuildCooperateMapAreaState.LOCK;

                for (int i = 0; i < rewardPointInfoList.Count; i++)
                {
                    if (rewardPointInfoList[i] == null)
                        continue;

                    if (!rewardPointInfoList[i].isUnlock)
                        return EGuildCooperateMapAreaState.LOCK;

                    if(rewardPointInfoList[i].getRewardType() == ECommonRewardType.CAN_GET_REWARD)
                        canGetReward = true;
                }
            }
            else
                return EGuildCooperateMapAreaState.LOCK;

            if (canGetReward)
                return EGuildCooperateMapAreaState.UNLOCK_HAVE_REWARD;
            else
                return EGuildCooperateMapAreaState.UNLOCK_NO_REWARD;
        }

        /// <summary>
        /// 根据区域ID获取区域建设进度
        /// </summary>
        /// <param name="_areaId"></param>
        /// <returns></returns>
        public long getAreaAttackProgressPercent(long _areaId)
        {
            if (_m_dRewardPointInfoDic == null)
                return 0;

            if (_m_dRewardPointInfoDic.TryGetValue(_areaId, out List<GuildCooperateRewardPointInfo> rewardPointInfoList))
            {
                if (rewardPointInfoList == null)
                    return 0;
                long hadAttackHp = 0;
                long totalHp = 0;
                for (int i = 0; i < rewardPointInfoList.Count; i++)
                {
                    if (rewardPointInfoList[i] == null || rewardPointInfoList[i].propertyPointInfoList == null)
                        continue;

                    for (int j = 0; j < rewardPointInfoList[i].propertyPointInfoList.Count; j++)
                    {
                        hadAttackHp += rewardPointInfoList[i].propertyPointInfoList[j].hadAttackHp;
                        totalHp += rewardPointInfoList[i].propertyPointInfoList[j].totalHp;
                    }
                }
                if (totalHp <= 0)
                    return 0;

                return hadAttackHp * 100 / totalHp;
            }
            else
                return 0;
        }

        /// <summary>
        /// 开启定时检查
        /// </summary>
        private void _startCheck()
        {
            _m_checkTask.setDisable();
            _m_checkTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCheck, 1.0f);
        }

        /// <summary>
        /// 关闭定时检查
        /// </summary>
        private void _stopCheck()
        {
            _m_checkTask.setDisable();
        }

        /// <summary>
        /// 定时检查函数
        /// </summary>
        private void _tickCheck()
        {
            if (_m_lNextRefreshTimeMs <= 0 || _m_bIsReqNewInit)
                return;

            if (FpsAndPingMgr.instance.serverTimeTag >= _m_lNextRefreshTimeMs)
            {
                _m_bIsReqNewInit = true;
                //请求初始化
                reqGuildCooperateInit();
            }
        }

        /// <summary>
        /// 更新奖励据点信息字典
        /// </summary>
        /// <param name="_rewardPointList"></param>
        private void _updateRewardPointDic(List<GuildCooperate_RewardPointInfo> _rewardPointList)
        {
            if (_rewardPointList == null)
                return;

            if (_m_dRewardPointInfoDic == null)
                _m_dRewardPointInfoDic = new Dictionary<long, List<GuildCooperateRewardPointInfo>>();

            for (int i = 0; i < _rewardPointList.Count; i++)
            {
                if (_rewardPointList[i] == null)
                    continue;

                GuildCooperateRewardPointInfo pointInfo = new GuildCooperateRewardPointInfo(_rewardPointList[i]);
                if (_m_dRewardPointInfoDic.TryGetValue(pointInfo.areaId, out List<GuildCooperateRewardPointInfo> targetPointList))
                {
                    targetPointList?.Add(pointInfo);
                }
                else
                {
                    List<GuildCooperateRewardPointInfo> pointList = new List<GuildCooperateRewardPointInfo>();
                    pointList.Add(pointInfo);
                    _m_dRewardPointInfoDic[pointInfo.areaId] = pointList;
                }
            }

            //更新当前区域ID
            _updateCurAreaId();
        }

        /// <summary>
        /// 更新推荐据点信息
        /// </summary>
        /// <param name="_pointInfo"></param>
        private void _updateRecommendPoint(GuildCooperate_RewardPointPos _pointInfo)
        {
            if (_pointInfo == null || _pointInfo.getAreaId() <= 0)
            {
                _m_recommendPointInfo = null;
                return;
            }

            if (_m_recommendPointInfo == null)
                _m_recommendPointInfo = new GuildCooperateRecommendPointInfo(_pointInfo);
            else
                _m_recommendPointInfo.updateInfo(_pointInfo);
        }

        /// <summary>
        /// 更新已领取奖励点
        /// </summary>
        /// <param name="_lastRefreshTimeMs"></param>
        /// <param name="_hadDrawRewardPointList"></param>
        private void _updateHadDrawRewardPointList(long _lastRefreshTimeMs, List<GuildCooperate_RewardPointPos> _hadDrawRewardPointList)
        {
            if (_hadDrawRewardPointList == null)
                return;

            if (_m_dHadDrawRewardPointsDic == null)
                _m_dHadDrawRewardPointsDic = new Dictionary<long, List<int>>();

            //如果上次刷新时间小于公会协助重置时间的上次刷新时间，则清空已领取奖励点列表
            if (_lastRefreshTimeMs < GRefdataCoreMgr.instance.npGeneral.guild_cooperate_refresh_time.getPreviousFreshTimeTagMs())
            {
                _m_dHadDrawRewardPointsDic.Clear();
                return;
            }

            for (int i = 0; i < _hadDrawRewardPointList.Count; i++)
            {
                GuildCooperate_RewardPointPos pos = _hadDrawRewardPointList[i];
                if (pos == null)
                    continue;

                if (_m_dHadDrawRewardPointsDic.TryGetValue(pos.getAreaId(), out List<int> targetList))
                {
                    if(!targetList.Contains(pos.getIndex()))
                        targetList.Add(pos.getIndex());
                }
                else
                {
                    List<int> newList = new List<int>();
                    newList.Add(pos.getIndex());
                    _m_dHadDrawRewardPointsDic[pos.getAreaId()] = newList;
                }
            }

            //更新当前区域ID
            _updateCurAreaId();
        }

        /// <summary>
        /// 更新伙伴使用信息
        /// </summary>
        /// <param name="_heroUseInfoList"></param>
        private void _updateHeroUseInfo(List<GuildCooperate_HeroUseInfo> _heroUseInfoList)
        {
            if (_heroUseInfoList == null)
                return;

            if (_m_dHeroUseRecordDic == null)
                _m_dHeroUseRecordDic = new Dictionary<long, GuildCooperate_HeroUseInfo>();

            for (int i = 0; i < _heroUseInfoList.Count; i++)
            {
                if (_heroUseInfoList[i] == null)
                    continue;

                _m_dHeroUseRecordDic[_heroUseInfoList[i].getHeroId()] = _heroUseInfoList[i];
            }
        }

        /// <summary>
        /// 刷新当前区域ID
        /// </summary>
        private void _updateCurAreaId()
        {
            if (_m_dRewardPointInfoDic != null)
            {
                long canGetRewardAreaId = 0;
                foreach (KeyValuePair<long, List<GuildCooperateRewardPointInfo>> keyValuePair in _m_dRewardPointInfoDic)
                {
                    bool isAreaUnlock = true;
                    bool canGetReward = false;
                    if (keyValuePair.Value != null)
                    {
                        for (int i = 0; i < keyValuePair.Value.Count; i++)
                        {
                            if (keyValuePair.Value[i] == null)
                                continue;

                            // 如果有一个据点未解锁，则该区域未解锁，直接跳过
                            if (!keyValuePair.Value[i].isUnlock)
                            {
                                isAreaUnlock = false;
                                break;
                            }

                            // 如果有可领取奖励的据点，记录该区域ID
                            if (keyValuePair.Value[i].getRewardType() == ECommonRewardType.CAN_GET_REWARD)
                            {
                                if (canGetRewardAreaId == 0 || canGetRewardAreaId > keyValuePair.Key)
                                    canGetRewardAreaId = keyValuePair.Key;
                            }
                        }
                    }

                    // 如果区域已解锁并且没有可领取奖励的据点，取一个最大的已解锁区域ID
                    if (isAreaUnlock && _m_lCurAreaId < keyValuePair.Key)
                        _m_lCurAreaId = keyValuePair.Key;

                    // 如果区域已解锁并且有可领取奖励的据点，取一个最小的可领取奖励区域ID
                    if (_m_lCurAreaId > canGetRewardAreaId && canGetRewardAreaId > 0)
                        _m_lCurAreaId = canGetRewardAreaId;

                    //刷新红点
                    _refreshAreaUnlockRedTip();
                }
            }
        }

        #region 红点

        //刷新全部红点
        private void _refreshAllRedTip()
        {
            _refreshUnlockSysRedTip();
            _refreshFreeConstructRedTip();
            _refreshRewardPointRewardRedTip();
            _refreshAreaUnlockRedTip();
        }
        
        /// <summary>
        /// 刷新解锁系统红点
        /// </summary>
        private void _refreshUnlockSysRedTip()
        {
            long count = 0;
            if (NPPlayer.instance.guildComp.isJoinGuild() && !AccountSettingMgr.instance.accountSetting.isReadGuildRedTip(RedTipConst.RED_GUILD_COOPERATE_UNLOCK))
                count = 1;

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_COOPERATE_UNLOCK, count);
        }

        /// <summary>
        /// 刷新免费建设红点
        /// </summary>
        private void _refreshFreeConstructRedTip()
        {
            long count = 0;
            if (NPPlayer.instance.guildComp.isJoinGuild())
            {
                List<HeroInfo> heroInfoList = new List<HeroInfo>();
                NPPlayer.instance.heroComponent.getAllList(heroInfoList);
                for (int i = 0; i < heroInfoList.Count; i++)
                {
                    if (isHeroCanUse(heroInfoList[i].id))
                        count++;
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_COOPERATE_FREE_CONSTRUCT, count);
        }

        /// <summary>
        /// 刷新奖励据点红点
        /// </summary>
        private void _refreshRewardPointRewardRedTip()
        {
            long count = 0;
            if (_m_dRewardPointInfoDic != null && NPPlayer.instance.guildComp.isJoinGuild())
            {
                foreach (List<GuildCooperateRewardPointInfo> rewardPointInfoList in _m_dRewardPointInfoDic.Values)
                {
                    foreach (GuildCooperateRewardPointInfo info in rewardPointInfoList)
                    {
                        if (info != null && info.getRewardType() == ECommonRewardType.CAN_GET_REWARD)
                            count++;
                    }
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_COOPERATE_REWARD, count);
        }

        /// <summary>
        /// 刷新区域解锁红点
        /// </summary>
        private void _refreshAreaUnlockRedTip()
        {
            long count = 0;
            if (NPPlayer.instance.guildComp.isJoinGuild())
            {
                if (!AccountSettingMgr.instance.accountSetting.isReadGuildCooperateAreaUnlockRedTip($"{_m_lResetCount}_{_m_lCurAreaId}"))
                    count = 1;
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_COOPERATE_UNLOCK_NEW_AREA, count);
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 加入联盟事件
        /// </summary>
        private void _onJoinGuild(params object[] _objects)
        {
            _m_bIsReqNewInit = true;
            reqGuildCooperateInit();
            _refreshUnlockSysRedTip();
        }

        /// <summary>
        /// 离开联盟事件
        /// </summary>
        private void _onLeaveGuild(params object[] _objects)
        {
            AccountSettingMgr.instance.accountSetting.removeAlreadyReadGuildRedTip(RedTipConst.RED_GUILD_COOPERATE_UNLOCK);
            _refreshAllRedTip();
            _clear();
        }

        /// <summary>
        /// 新增顾问事件
        /// </summary>
        /// <param name="_objects"></param>
        private void _onHeroGain(params object[] _objects)
        {
            _refreshFreeConstructRedTip();
        }

        #endregion

        #region S2C

        /// <summary>
        /// 联盟初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retGuildCooperateInit(GS2GC_002_075_RetGuildCooperateInit _msg)
        {
            if (_msg == null)
                return;

            //清空数据
            _clear();

            //下次刷新时间
            _m_lNextRefreshTimeMs = _msg.getInfo().getNextRefreshTimeMs();
            //重置次数
            _m_lResetCount = _msg.getInfo().getResetCount();
            //已领取奖励点
            _updateHadDrawRewardPointList(_msg.getHadDrawRewardPointList().getLastRefreshTimeMs(), _msg.getHadDrawRewardPointList().getHadDrawList());
            //奖励据点信息
            _updateRewardPointDic(_msg.getInfo().getPointList());
            //推荐奖励据点
            _updateRecommendPoint(_msg.getInfo().getRecommendPos());
            //伙伴使用信息
            _updateHeroUseInfo(_msg.getHeroUseInfoList());
            //完成初始化
            if (!isInitDone)
                setInitDone();

            _m_bIsReqNewInit = false;

            //刷新红点
            _refreshAllRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_COOPERATE_RESET);
        }

        /// <summary>
        /// 推荐奖励据点变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onRecommendRewardPointChange(GS2GC_032_075_OnRecommendRewardPointChange _msg)
        {
            if (_msg == null)
                return;

            _updateRecommendPoint(_msg.getRecommendPos());
            WinMsg.SendMsg(WinMsgType.ON_GUILD_COOPERATE_RECOMMEND_CHG);
        }

        /// <summary>
        /// 属性据点信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onPropertyPointInfoChange(GS2GC_032_076_OnPropertyPointInfoChange _msg)
        {
            if (_msg == null || _m_dRewardPointInfoDic == null)
                return;

            if (_m_dRewardPointInfoDic.TryGetValue(_msg.getPos().getAreaId(), out List<GuildCooperateRewardPointInfo> pointList))
            {
                for (int i = 0; i < pointList.Count; i++)
                {
                    if (pointList[i] == null)
                        continue;

                    if (pointList[i].index == _msg.getPos().getIndex())
                    {
                        pointList[i].updatePropertyPointInfo(_msg.getPropertyPointInfo());
                        break;
                    }
                }
            }
            //刷新红点
            _refreshRewardPointRewardRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_COOPERATE_PROPERTY_POINT_CHG, _msg.getPos().getAreaId(), _msg.getPos().getIndex(), _msg.getPropertyPointInfo().getIndex());
        }

        /// <summary>
        /// 联盟协作奖励据点击败推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onRewardPointDefeat(GS2GC_032_077_OnRewardPointDefeat _msg)
        {
            if (_msg == null)
                return;

            if (_m_dRewardPointInfoDic != null && _m_dRewardPointInfoDic.TryGetValue(_msg.getPos().getAreaId(), out List<GuildCooperateRewardPointInfo> _pointList))
            {
                if (_pointList != null)
                {
                    for (int i = 0; i < _pointList.Count; i++)
                    {
                        if (_pointList[i] != null && _pointList[i].index == _msg.getPos().getIndex())
                            _pointList[i].updateLeaderCid(_msg.getLeaderCid());
                    }
                }
            }

            WinMsg.SendMsg(WinMsgType.ON_GUILD_COOPERATE_REWARD_POINT_DEFEAT, _msg.getPos().getAreaId(), _msg.getPos().getIndex());
        }

        /// <summary>
        /// 已领取奖励点列表变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onHadDrawRewardPointListChg(GS2GC_032_078_OnHadDrawRewardPointListChg _msg)
        {
            if (_msg == null)
                return;

            //已领取奖励点
            _updateHadDrawRewardPointList(_msg.getData().getLastRefreshTimeMs(), _msg.getData().getHadDrawList());
            //刷新红点
            _refreshRewardPointRewardRedTip();
            WinMsg.SendMsg(WinMsgType.ON_GUILD_COOPERATE_DRAW_REWARD_CHG);
        }

        /// <summary>
        /// 大臣使用信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onHeroUseInfoChange(GS2GC_032_079_OnHeroUseInfoChange _msg)
        {
            if (_msg == null)
                return;

            if (_m_dHeroUseRecordDic == null)
                _m_dHeroUseRecordDic = new Dictionary<long, GuildCooperate_HeroUseInfo>();

            _m_dHeroUseRecordDic[_msg.getHeroUseInfo().getHeroId()] = _msg.getHeroUseInfo();
            //刷新红点
            _refreshFreeConstructRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_COOPERATE_HERO_USE_INFO_CHG, _msg.getHeroUseInfo().getHeroId());
        }

        /// <summary>
        /// 联盟协作奖励据点解锁推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onRewardPointUnlock(GS2GC_032_080_OnRewardPointUnlock _msg)
        {
            if (_msg == null)
                return;

            for (int i = 0; i < _msg.getPosList().Count; i++)
            {
                GuildCooperate_RewardPointPos pos = _msg.getPosList()[i];
                if(pos == null)
                    continue;

                if (_m_dRewardPointInfoDic != null && _m_dRewardPointInfoDic.TryGetValue(pos.getAreaId(), out List<GuildCooperateRewardPointInfo> _pointList))
                {
                    if (_pointList != null)
                    {
                        for (int j = 0; j < _pointList.Count; j++)
                        {
                            if (_pointList[j] != null && _pointList[j].index == pos.getIndex())
                                _pointList[j].updateIsUnlock(true);
                        }
                    }
                }
            }
            _updateCurAreaId();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_COOPERATE_REWARD_POINT_UNLOCK);
        }


        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化
        /// </summary>
        public void reqGuildCooperateInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_075_ReqGuildCooperateInit());
        }

        /// <summary>
        /// 请求联盟协作攻击日志列表
        /// </summary>
        /// <param name="_lastDbId">上一次读取的数据id</param>
        /// <param name="_num">所需日志数量</param>
        /// <param name="_callback"></param>
        public void reqGuildCooperateAttackLogList(long _lastDbId, int _num, Action<bool, GS2GC_032_041_RetGuildCooperateAttackLogList> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_041_ReqGuildCooperateAttackLogList(_lastDbId, _num),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_041_RetGuildCooperateAttackLogList>(_callback));
        }

        /// <summary>
        /// 请求设置推荐奖励据点
        /// </summary>
        /// <param name="_pos"></param>
        public void reqSetRecommendRewardPoint(GuildCooperate_RewardPointPos _pos)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_042_ReqSetRecommendRewardPoint(_pos),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_042_RetSetRecommendRewardPoint>(null));
        }

        /// <summary>
        /// 请求领取奖励据点个人奖励
        /// </summary>
        /// <param name="_pos"></param>
        public void reqDrawRewardPointReward(GuildCooperate_RewardPointPos _pos, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_043_ReqDrawRewardPointReward(_pos),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_043_RetDrawRewardPointReward>((_isSuc, _msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 请求攻击属性据点
        /// </summary>
        /// <param name="_pos"></param>
        /// <param name="_propertyPointIndex"></param>
        /// <param name="_heroId"></param>
        public void reqAttackPropertyPoint(GuildCooperate_RewardPointPos _pos, int _propertyPointIndex, long _heroId, Action<bool, GS2GC_032_044_RetAttackPropertyPoint> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_044_ReqAttackPropertyPoint(_pos, _propertyPointIndex, _heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_044_RetAttackPropertyPoint>(_callback));
        }

        /// <summary>
        /// 请求增加大臣恢复次数
        /// </summary>
        /// <param name="_heroId"></param>
        public void reqAddHeroRecoveryCount(long _heroId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_045_ReqAddHeroRecoveryCount(_heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_045_RetAddHeroRecoveryCount>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }));
        }

        /// <summary>
        /// 请求联盟协作伤害排行榜
        /// </summary>
        /// <param name="_callback"></param>
        public void reqGuildCooperateDamageRank(Action<GS2GC_032_046_RetGuildCooperateDamageRank> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_032_046_ReqGuildCooperateDamageRank(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_032_046_RetGuildCooperateDamageRank>((_isSuc, _msg) =>
                {
                    if (!_isSuc)
                        return;

                    _callback?.Invoke(_msg);
                }));
        }

        #endregion
    }
}
