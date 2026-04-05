using System;
using System.Collections.Generic;
using System.IO;
using ALPackage;
using Common.GuildObj;
using Common.MarsEnum;
using Common.MarsObj;
using GC2GS.p002_InitOp;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p002_InitOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// Mars探索子组件 - 负责管理火星探索系统
    /// </summary>
    public class MarsExploreSubComponent : _AMarsSubComponent
    {
        // 总共探索次数
        private long _m_exploreNumTotal;
        // 当前等级和下一等级
        private MarsExploreLvlRefObj _m_levelRef;
        private MarsExploreLvlRefObj _m_nextLevelRef;
        
        [ItemNotNull, NotNull] private readonly List<MarsExploreTeamInfo> _m_teamList;
        [NotNull] private readonly _EventMgr _m_eventMgr;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_tickDealer;

        private long _m_newestLogTime;
        private NewestSharedMineData _m_newestSharedMineData;
        private MarsExploreTeamReturnLocalPushDealer _m_teamReturnLocalPushDealer;//火星队伍派遣返回本地推送


        public MarsExploreSubComponent([NotNull] MarsComponent _parentComp)
            : base(_parentComp)
        {
            _m_teamList = new List<MarsExploreTeamInfo>();
            _m_eventMgr = new _EventMgr(this);
            _m_tickDealer = new LazyNextFrameTaskDealer(new _TickTask(this));
        }


        public event Action<MarsExploreTeamInfo> onTeamStateChg;
        public event Action<MarsExploreTeamInfo> onTeamHeroChg;
        public event Action<_AMarsExploreEventInfo> onEventAdd;
        public event Action<_AMarsExploreEventInfo> onEventRemove;
        public event Action<_AMarsExploreEventInfo, bool> onEventChg; // bool 是是否是同位置增加了新事件
        public event Action onExploreDataChg;
        public event Action<_IMarsExploreMineItem> onMineAdd;
        public event Action<_IMarsExploreMineItem> onMineRemove;
        public event Action<long, bool> onMineChg; // bool 是是否是同位置增加了新采集点
        public long exploreNumTotal { get { return _m_exploreNumTotal; } }
        public MarsExploreLvlRefObj levelRef { get { return _m_levelRef; } }
        public MarsExploreLvlRefObj nextLevelRef { get { return _m_nextLevelRef; } }
        public int eventCount { get { return _m_eventMgr.eventCount; } } // todo: 加上采集的部分
        public long newestLogTime { get { return _m_newestLogTime; } }
        public NewestSharedMineData newestSharedMineData { get { return _m_newestSharedMineData; } }


        public override void init(Action<bool> _complete)
        {
            List<MarsExploreTeamRefObj> teamRef = GRefdataCoreMgr.instance.marsExploreTeamRefCore.refList;
            foreach (MarsExploreTeamRefObj team in teamRef)
            {
                MarsExploreTeamInfo teamInfo = new MarsExploreTeamInfo(team);
                _m_teamList.Add(teamInfo);
            }

            _onExploreDataChg(0, 1);
            
            NPGSClientListener.sendRequestByLog(new GC2GS_002_077_ReqMarsExploreInit(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_077_RetMarsExploreInit>((_isSuc, _msg) =>
                {
                    _m_parentComponent.dealPreInitFunc(() =>
                    {
                        if (!_isSuc || _msg == null)
                        {
                            _complete?.Invoke(false);
                            return;
                        }

                        Mars_Explore serverExploreInfo = _msg.getExplore();
                        _onExploreDataChg(serverExploreInfo);

                        //先添加矿数据，确保队伍如果有采矿行为，对矿的扩展数据判断是正确的
                        List<Mars_MineIdx> serverMineIdxList = _msg.getMineIdx();
                        if (serverMineIdxList != null)
                        {
                            foreach (Mars_MineIdx mineIdx in serverMineIdxList)
                                _onMineInfoAdd(mineIdx);
                        }

                        List<Mars_ExploreEvent> serverEventList = _msg.getEventList();
                        if (serverEventList != null)
                        {
                            foreach (Mars_ExploreEvent serverEvent in serverEventList)
                                _onEventAdd(serverEvent);
                        }

                        List<Mars_Team> serverTeamList = _msg.getTeamList();
                        if (serverTeamList != null)
                        {
                            foreach (Mars_Team serverTeam in serverTeamList)
                                _onTeamInfoInit(serverTeam);
                        }

                        NPPlayer.instance.playerPropertyMgr.propertyChgDelegate += _onPlayerPropertyChg;
                        WinMsg.RegisterMsg(WinMsgType.ON_HERO_STAR_CHG, _onHeroStarChg);
                        WinMsg.RegisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);

                        _m_teamReturnLocalPushDealer = new MarsExploreTeamReturnLocalPushDealer();
                        LocalPushMgr.instance.regDealer(_m_teamReturnLocalPushDealer);

                        _complete?.Invoke(true);
                    });
                }));
            
            // 让服务器发送最新的日志时间，服务端会推送 041_061 下来
            NPGSClientListener.sendMsgByLog(new GC2GS_041_022_ReqLastPVPLogFlag());
            // 让服务器发送最新的共享矿数据，服务端会推送 041_063 下来
            if (NPPlayer.instance.guildComp.isJoinGuild())
                NPGSClientListener.sendMsgByLog(new GC2GS_041_023_ReqGuildMarsMineShareFlag());
        }
        public override void discard()
        {
            LocalPushMgr.instance.unRegDealer(_m_teamReturnLocalPushDealer);
            _m_teamReturnLocalPushDealer = null;

            NPPlayer.instance.playerPropertyMgr.propertyChgDelegate -= _onPlayerPropertyChg;
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_POWER_CHG, _onHeroPowerChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_STAR_CHG, _onHeroStarChg);

            _onExploreDataChg(0, 0);
        }
        
        public MarsExploreTeamInfo getTeamInfoById(long _teamId)
        {
            return _m_teamList.Find(_t => _t.teamId == _teamId);
        }
        /// <summary>
        /// 逐个处理
        /// </summary>
        /// <param name="_eachAction"></param>
        public void doEachTeam(Action<MarsExploreTeamInfo> _eachAction)
        {
            if (null == _eachAction)
                return;

            foreach(MarsExploreTeamInfo teamInfo in _m_teamList)
            {
                _eachAction(teamInfo);
            }
        }
        public void getTeamListNonAlloc(List<MarsExploreTeamInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_teamList);
        }
        public void actionWithAllTeam(Func<MarsExploreTeamInfo, bool> _func)
        {
            if (_func == null)
                return;

            foreach (MarsExploreTeamInfo teamInfo in _m_teamList)
            {
                if (!_func(teamInfo))
                    return;
            }
        }
        public bool isTeamMarchTarget(long _instanceId)
        {
            foreach (MarsExploreTeamInfo teamInfo in _m_teamList)
            {
                if (teamInfo.exDataObj is MarsTeamEx_March { data: not null } marchData)
                {
                    if (marchData.data.getInstanceId() == _instanceId)
                        return true;
                }
            }

            return false;
        }
        public MarsExploreTeamInfo getStrongestTime()
        {
            MarsExploreTeamInfo strongestTeam = null;
            long maxPower = long.MinValue;
            foreach (MarsExploreTeamInfo teamInfo in _m_teamList)
            {
                long teamPower = teamInfo.getTeamPower();
                if (teamPower > maxPower)
                {
                    maxPower = teamPower;
                    strongestTeam = teamInfo;
                }
            }

            return strongestTeam;
        }
        public long calculateCollectSpeed(long _mineId)
        {
            MarsExploreMineRefObj mineRef = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_mineId);
            return calculateCollectSpeed(mineRef);
        }
        public long calculateCollectSpeed(MarsExploreMineRefObj _mineRef)
        {
            if (_mineRef == null)
                return 0;

            MarsExploreTeamInfo teamInfo = NPPlayer.instance.marsComp.exploreSubComponent.getPowerestTeamInfo();
            long collectPer = GRefdataCoreMgr.instance.getMarsExploreMineCollectBonus(teamInfo?.getTeamPower() ?? 0);
            long collectSpeed = (long) Math.Ceiling(1.0f * _mineRef.mars_mine_collects_speed * (1 + collectPer / 10000f));
            if (collectSpeed <= 0)
                return 0;
            
            return collectSpeed;
        }
        public long calculateCollectSpeed(MarsExploreMineRefObj _mineRef, MarsExploreTeamInfo _teamInfo)
        {
            if (_mineRef == null || _teamInfo == null)
                return 0;

            long collectPer = GRefdataCoreMgr.instance.getMarsExploreMineCollectBonus(_teamInfo?.getTeamPower() ?? 0);
            long collectSpeed = (long) Math.Ceiling(1.0f * _mineRef.mars_mine_collects_speed * (1 + collectPer / 10000f));
            if (collectSpeed <= 0)
                return 0;
            
            return collectSpeed;
        }
        public MarsExploreTeamInfo getPowerestTeamInfo()
        {
            MarsExploreTeamInfo powerestTeam = null;
            long maxPower = long.MinValue;
            foreach (MarsExploreTeamInfo teamInfo in _m_teamList)
            {
                long teamPower = teamInfo.getTeamPower();
                if (teamPower > maxPower)
                {
                    maxPower = teamPower;
                    powerestTeam = teamInfo;
                }
            }

            return powerestTeam;
        }

        /// <summary>
        /// 对外提供的位置对象操作处理函数
        /// </summary>
        /// <param name="_posItem"></param>
        public void addExPosItem(_IMarsExplorePosItem _posItem)
        {
            _m_eventMgr._addExPosItem(_posItem);
        }
        public void removeExPosItem(_IMarsExplorePosItem _posItem)
        {
            _m_eventMgr._removeExPosItem(_posItem);
        }

        public _AMarsExploreEventInfo getEventInfoByInstanceId(long _eventInstanceId)
        {
            return _m_eventMgr.getEventInfoByInstanceId(_eventInstanceId);
        }

        public MarsExploreMineInfo getMineInfoByInstanceId(long _mineInstanceId)
        {
            return _m_eventMgr.getMineInfoByInstanceId(_mineInstanceId);
        }

        /// <summary>
        /// 针对每个事件进行处理的方法
        /// </summary>
        /// <param name="_eachAction"></param>
        public void doEachEvent(Action<_AMarsExploreEventInfo> _eachAction)
        {
            if (null == _eachAction)
                return;

            _m_eventMgr.doEachEvent(_eachAction);
        }
        /// <summary>
        /// 针对每个采集点进行处理的方法
        /// </summary>
        public void doEachMine(Action<MarsExploreMineInfo> _eachAction)
        {
            if (null == _eachAction)
                return;

            _m_eventMgr.doEachMine(_eachAction);
        }
        /// <summary>
        /// 针对每个对象进行处理的方法
        /// </summary>
        /// <param name="_eachAction"></param>
        public void doEachPosItem(Action<_IMarsExplorePosItem> _eachAction)
        {
            if (null == _eachAction)
                return;

            _m_eventMgr.doEachPosItem(_eachAction);
        }

        public int getEventNumAtPos(long _posId)
        {
            return _m_eventMgr.getEventNumAtPos(_posId);
        }
        
        public List<_IMarsExplorePosItem> getPosItemListAtPos(long _posId)
        {
            return _m_eventMgr.getPosItemListAtPos(_posId);
        }

        public void updateTime()
        {
            _m_tickDealer.setNeedDeal();
        }
        
        public void reqTeamBack(long _teamId, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_041_012_ReqMarsTeamBack(_teamId),
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(_complete));
        }
        public void reqTeamRepair(long _teamId, long _num, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_041_007_ReqStartExploreTeamRepair(_teamId, _num),
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(_complete));
        }
        public void reqTeamRename(long _teamId, string _confirmName, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_041_004_ReqSetExploreTeamName(_teamId, _confirmName),
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(_complete));
        }
        public void reqTeamHeroList(long _teamId, List<long> _newHeroIdList, Action _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_041_003_ReqSetExploreTeamHero(_teamId, _newHeroIdList), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(_complete));
        }
        /// <summary>
        /// 请求PVP日志列表
        /// </summary>
        public void reqPvPLog(Action<List<Mars_ExplorePVPLogIdx>> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_041_014_ReqGetCollectPVPLogIdxList(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_014_RetGetCollectPVPLogIdxList>((_isSuc, _msg) =>
                    _complete?.Invoke(_msg?.getIdxList())));
            
            // 更新最新日志时间为当前时间，并刷新日志红点
            AccountSettingMgr.instance.accountSetting?.setReadMarsExplorePvPLogTime(_m_newestLogTime);
            _m_parentComponent.redTipDealer.refreshExplorePvPLogRedTip();
        }
        /// <summary>
        /// 请求共享矿列表
        /// </summary>
        public void reqSharedMine(Action<List<Guild_MineShareInfo>> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_041_017_ReqGuildMineShareList(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_017_RetGuildMineShareList>((_isSuc, _msg) =>
                    _complete?.Invoke(_msg?.getMineShareList())));
            
            // 更新最新共享矿数据为当前数据，并刷新共享矿红点
            AccountSettingMgr.instance.accountSetting?.setReadSharedMineData(_m_newestSharedMineData);
            _m_parentComponent.redTipDealer.refreshExploreSharedMineRedTip();
        }


        internal void _onTeamNameChg(GS2GC_041_056_OnExploreTeamNameChg _msg)
        {
            if (_msg == null)
                return;
            
            MarsExploreTeamInfo teamInfo = getTeamInfoById(_msg.getTeamId());
            if (teamInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 找不到对应的探索队伍, teamId={_msg.getTeamId()}");
                return;
            }
            
            teamInfo._updateName(_msg.getName());
        }
        internal void _onTeamHeroChg(GS2GC_041_057_OnExploreTeamHeroChg _msg)
        {
            if (_msg == null)
                return;
            
            MarsExploreTeamInfo teamInfo = getTeamInfoById(_msg.getTeamId());
            if (teamInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 找不到对应的探索队伍, teamId={_msg.getTeamId()}");
                return;
            }

            teamInfo._updateHeroList(_msg.getHeroIdList());
            onTeamHeroChg?.Invoke(teamInfo);
        }
        internal void _onTeamStateChg(GS2GC_041_058_OnExploreTeamState _msg)
        {
            if (_msg == null)
                return;
            
            MarsExploreTeamInfo teamInfo = getTeamInfoById(_msg.getTeamId());
            if (teamInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 找不到对应的探索队伍, teamId={_msg.getTeamId()}");
                return;
            }

            teamInfo._updateState(_msg.getState());
            onTeamStateChg?.Invoke(teamInfo);
        }
        internal void _onTeamLossValueChg(GS2GC_041_062_OnExploreTeamLossValueChg _msg)
        {
            if (_msg == null)
                return;
            
            MarsExploreTeamInfo teamInfo = getTeamInfoById(_msg.getTeamId());
            if (teamInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 找不到对应的探索队伍, teamId={_msg.getTeamId()}");
                return;
            }
            
            long oriSoldierLoss = teamInfo.soldierLoss;
            teamInfo._updateSoldierLoss(_msg.getLossValue());
            // 状态变更提示，目前只有修复完成时损耗兵量会减小，所以弹出修复完成提示
            if (oriSoldierLoss > teamInfo.soldierLoss)
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreTeamRepairComplete_name, teamInfo.name));
        }

        /// <summary>
        /// 调用数据初始化处理，不调用事件
        /// </summary>
        /// <param name="_msg"></param>
        internal void _onTeamInfoInit(Mars_Team _msg)
        {
            if (_msg == null)
                return;

            MarsExploreTeamInfo teamInfo = getTeamInfoById(_msg.getTeamId());
            if (teamInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 找不到对应的探索队伍, teamId={_msg.getTeamId()}");
                return;
            }

            teamInfo._initData(_msg);
        }
        /// <summary>
        /// 更新数据，会调用相关数据变更事件
        /// </summary>
        /// <param name="_msg"></param>
        internal void _onTeamInfoChg(Mars_Team _msg)
        {
            if (_msg == null)
                return;
            
            MarsExploreTeamInfo teamInfo = getTeamInfoById(_msg.getTeamId());
            if (teamInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 找不到对应的探索队伍, teamId={_msg.getTeamId()}");
                return;
            }
            
            teamInfo._updateData(_msg);
        }
        internal void _onExploreDataChg(Mars_Explore _msg)
        {
            if (_msg == null)
                return;
            
            _onExploreDataChg(_msg.getExploreSum(), _msg.getLvl());
            onExploreDataChg?.Invoke();
            WinMsg.SendMsg(WinMsgType.ON_MARS_EXPLORE_DATA_CHG, _msg.getExploreSum(), _msg.getLvl());
        }
        internal void _onExploreDataChg(long _totalExploreNum, int _level)
        {
            _m_exploreNumTotal = _totalExploreNum;
            if (_m_levelRef != null && _m_levelRef.explore_level == _level)
                return;
            
            if (_m_levelRef != null)
                _m_parentComponent.playerPropertyContainer.removeModifier(_m_levelRef.player_property);
            
            _m_levelRef = GRefdataCoreMgr.instance.marsExploreLvlRefCore.getRef(_level);
            _m_nextLevelRef = GRefdataCoreMgr.instance.marsExploreLvlRefCore.getRef(_level + 1);
            
            if (_m_levelRef != null)
                _m_parentComponent.playerPropertyContainer.addModifier(_m_levelRef.player_property);
        }
        internal void _onEventAdd(GS2GC_041_051_OnExploreEventAdd _msg)
        {
            Mars_ExploreEvent eventData = _msg?.getInfo();
            _onEventAdd(eventData);
        }
        internal void _onEventRemove(GS2GC_041_052_OnExploreEventDel _msg)
        {
            if (_msg == null)
                return;
            
            long eventInstanceId = _msg.getId();
            _m_eventMgr.removeEvent(eventInstanceId);
        }
        internal void _onEventDoneChg(GS2GC_041_055_OnExploreEventDone _msg)
        {
            if (_msg == null)
                return;
            
            long eventInstanceId = _msg.getId();
            _AMarsExploreEventInfo eventInfo = getEventInfoByInstanceId(eventInstanceId);
            if (eventInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 找不到对应的探索事件信息, eventInstanceId={eventInstanceId}");
                return;
            }

            eventInfo._updateIsDone();
            onEventChg?.Invoke(eventInfo, false);
        }
        internal void _onEventAdd(Mars_ExploreEvent _serverData)
        {
            if (_serverData == null)
                return;

            _AMarsExploreEventInfo eventInfo = _createEventInfo(_serverData);
            if (eventInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 无法创建对应的探索事件信息, eventId={_serverData.getEventId()}");
                return;
            }

            _m_eventMgr.addEvent(eventInfo);
        }
        internal void _onMineInfoAdd(GS2GC_041_059_OnMineAdd _msg)
        {
            Mars_MineIdx serverMineInfo = _msg?.getIdx();
            _onMineInfoAdd(serverMineInfo);
        }
        internal void _onMineInfoAdd(Mars_MineIdx _serverData)
        {
            if (_serverData == null)
                return;
        
            MarsExploreMineInfo mineInfo = new MarsExploreMineInfo(_serverData);
            if (!mineInfo._isValid())
            {
                ALLog.Error($"[MarsExploreSubComponent] 无法创建对应的采集点信息, mineId={_serverData.getRefId()}");
                return;
            }
        
            _m_eventMgr.addMine(mineInfo);
        }
        internal void _onMineInfoRemove(GS2GC_041_060_OnMineDel _msg)
        {
            if (_msg == null)
                return;
            
            long instanceId = _msg.getId();
            MarsExploreMineInfo mineInfo = getMineInfoByInstanceId(instanceId);
            if (mineInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] 找不到对应的采集点信息, mineInstanceId={instanceId}");
                return;
            }

            _m_eventMgr.removeMine(instanceId);
        }
        internal void _onMineInfoChg(GS2GC_041_013_RetNoticeMarsMine _msg)
        {
            Mars_MineDynamic serverMineInfo = _msg?.getInfo();
            if (serverMineInfo == null)
                return;

            MarsExploreMineInfo mineInfo = getMineInfoByInstanceId(serverMineInfo.getId());
            // if (mineInfo == null)
            // {
            //     //alzq： 这里对于找不到的索引不需要报错，直接不处理即可
            //     //ALLog.Error($"[MarsExploreSubComponent] 找不到对应的采集点信息, mineInstanceId={_serverData.getId()}");
            //     return;
            // }

            mineInfo?._updateData(serverMineInfo);
            onMineChg?.Invoke(serverMineInfo.getId(), false);
        }
        /// <summary>
        /// 更新最新日志时间
        /// </summary>
        internal void _onNewestLogTimeChg(long _time)
        {
            _m_newestLogTime = _time;
            // 刷新日志红点
            _m_parentComponent.redTipDealer.refreshExplorePvPLogRedTip();
        }
        /// <summary>
        /// 更新共享矿数据
        /// </summary>
        internal void _onSharedMineDataChg(NewestSharedMineData _data)
        {
            _m_newestSharedMineData = _data;
            // 刷新共享矿红点
            _m_parentComponent.redTipDealer.refreshExploreSharedMineRedTip();
        }


        private _AMarsExploreEventInfo _createEventInfo(Mars_ExploreEvent _serverData)
        {
            if (_serverData == null)
                return null;

            _AMarsExploreEventInfo eventInfo = null;
            EMarsExploreEventType eventType = _serverData.getEventType();
            switch (eventType)
            {
                case EMarsExploreEventType.BATTLE:
                    eventInfo = new MarsExploreBattleEventInfo(_serverData);
                    break;
                case EMarsExploreEventType.BOSS:
                    eventInfo = new MarsExploreBossEventInfo(_serverData);
                    break;
            }

            if (eventInfo != null && !eventInfo._isValid())
                eventInfo = null;

            return eventInfo;
        }
        private void _onPlayerPropertyChg(ENPPlayerPropertyType _type, long _, long __)
        {
            switch (_type)
            {
                case ENPPlayerPropertyType.MARS_TEAM_TROOP_NUM:
                case ENPPlayerPropertyType.MARS_TEAM_TROOP_NUM_PER:
                case ENPPlayerPropertyType.MARS_TEAM_TROOP_EXT_NUM:
                    foreach (MarsExploreTeamInfo teamInfo in _m_teamList)
                        teamInfo.needUpdateSoldierMax();
                    break;
                case ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER:
                    foreach (MarsExploreTeamInfo teamInfo in _m_teamList)
                        teamInfo.needUpdateTeamPower();
                    break;
            }
        }
        private void _onHeroPowerChg(object[] _params)
        {
            if (_params == null || _params.Length < 1 || _params[0] is not long heroId)
                return;
            
            foreach (MarsExploreTeamInfo teamInfo in _m_teamList)
            {
                if (teamInfo.containsHero(heroId))
                {
                    teamInfo.needUpdateSoldierMax();
                    break;
                }
            }
        }
        private void _onHeroStarChg(object[] _params)
        {
            if (_params == null || _params.Length < 1 || _params[0] is not long heroId)
                return;
            
            foreach (MarsExploreTeamInfo teamInfo in _m_teamList)
            {
                if (teamInfo.containsHero(heroId))
                {
                    teamInfo.needUpdatePowerAddPer();
                    break;
                }
            }
        }

        internal void onItemHelpSecsChg(long _objId, int _secs)
        {
            MarsExploreTeamInfo teamInfo = getTeamInfoById(_objId);
            if (teamInfo == null)
            {
                ALLog.Error($"[MarsExploreSubComponent] onItemHelpSecsChg: 未找到对应的队伍信息, _objId={_objId}");
                return;
            }
            
            teamInfo.onItemHelpSecsChg(_secs);
            onTeamStateChg?.Invoke(teamInfo);
        }

        private class _TickTask : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsExploreSubComponent _m_parentComp;
            
            
            public _TickTask([NotNull] MarsExploreSubComponent _parentComp)
            {
                _m_parentComp = _parentComp;
            }
            
            
            public void deal()
            {
                foreach (MarsExploreTeamInfo teamInfo in _m_parentComp._m_teamList)
                {
                    teamInfo._updateTime();
                }
                _m_parentComp._m_eventMgr.Update();
            }
        }


        private class _EventMgr
        {
            [NotNull] private readonly MarsExploreSubComponent _m_parentComp;
            
            [ItemNotNull, NotNull] private readonly List<_AMarsExploreEventInfo> _m_eventList;
            [ItemNotNull, NotNull] private readonly List<MarsExploreMineInfo> _m_mineList;
            [NotNull] private readonly Dictionary<long, List<_IMarsExplorePosItem>> _m_posItems;
            
            
            public _EventMgr([NotNull] MarsExploreSubComponent _parentComp)
            {
                _m_parentComp = _parentComp;
                
                _m_eventList = new List<_AMarsExploreEventInfo>();
                _m_mineList = new List<MarsExploreMineInfo>();
                _m_posItems = new Dictionary<long, List<_IMarsExplorePosItem>>();
            }
            
            
            public int eventCount { get { return _m_eventList.Count; } }

            
            public _AMarsExploreEventInfo getEventInfoByInstanceId(long _eventInstanceId)
            {
                return _m_eventList.Find(_e => _e.instanceId == _eventInstanceId);
            }

            public MarsExploreMineInfo getMineInfoByInstanceId(long _mineInstanceId)
            {
                return _m_mineList.Find(_m => _m.instanceId == _mineInstanceId);
            }

            /// <summary>
            /// 针对每个事件进行处理的方法
            /// </summary>
            /// <param name="_eachAction"></param>
            public void doEachEvent(Action<_AMarsExploreEventInfo> _eachAction)
            {
                if (null == _eachAction)
                    return;

                foreach (_AMarsExploreEventInfo eventInfo in _m_eventList)
                {
                    //逐个处理
                    _eachAction(eventInfo);
                }
            }
            
            public void doEachMine(Action<MarsExploreMineInfo> _eachAction)
            {
                if (null == _eachAction)
                    return;

                foreach (MarsExploreMineInfo mineInfo in _m_mineList)
                {
                    //逐个处理
                    _eachAction(mineInfo);
                }
            }

            /// <summary>
            /// 针对每个对象进行处理的方法
            /// </summary>
            /// <param name="_eachAction"></param>
            public void doEachPosItem(Action<_IMarsExplorePosItem> _eachAction)
            {
                if (null == _eachAction)
                    return;

                foreach (List<_IMarsExplorePosItem> posItems in _m_posItems.Values)
                {
                    //取出第一个有效对象做处理
                    _IMarsExplorePosItem item = __getFirstItem(posItems);
                    //逐个处理
                    _eachAction(item);
                }
            }

            public void Update()
            {
                foreach (MarsExploreMineInfo mineInfo in _m_mineList)
                {
                    mineInfo.updateTime();
                }
            }
            
            public void addEvent([NotNull] _AMarsExploreEventInfo _eventInfo)
            {
                _m_eventList.Add(_eventInfo);
                _addPosItem(_eventInfo);
            }
            public void removeEvent(long _eventInstanceId)
            {
                _AMarsExploreEventInfo eventInfo = getEventInfoByInstanceId(_eventInstanceId);
                if (eventInfo == null)
                    return;

                _m_eventList.Remove(eventInfo);
                _removePosItem(eventInfo);
            }
            public void addMine(MarsExploreMineInfo _mineInfo)
            {
                _m_mineList.Add(_mineInfo);
                _addPosItem(_mineInfo);
            }
            public void removeMine(long _instanceId)
            {
                MarsExploreMineInfo mineInfo = getMineInfoByInstanceId(_instanceId);
                if (mineInfo == null)
                    return;

                _m_mineList.Remove(mineInfo);
                _removePosItem(mineInfo);
            }

            /// <summary>
            /// 对外提供的位置对象操作处理函数
            /// </summary>
            /// <param name="_posItem"></param>
            protected internal void _addExPosItem(_IMarsExplorePosItem _posItem)
            {
                _addPosItem(_posItem);
            }
            protected internal void _removeExPosItem(_IMarsExplorePosItem _posItem)
            {
                _removePosItem(_posItem);
            }

            public int getEventNumAtPos(long _posId)
            {
                if (!_m_posItems.TryGetValue(_posId, out List<_IMarsExplorePosItem> posItems))
                    return 0;
                
                return posItems.Count;
            }
            
            [NotNull, ItemNotNull]
            public List<_IMarsExplorePosItem> getPosItemListAtPos(long _posId)
            {
                if (!_m_posItems.TryGetValue(_posId, out List<_IMarsExplorePosItem> posItems) || posItems == null)
                    return new List<_IMarsExplorePosItem>(0);
                
                return posItems;
            }


            private void _addPosItem(_IMarsExplorePosItem _posItem)
            {
                List<_IMarsExplorePosItem> posItems = _m_posItems.getValueDefinitely(_posItem.posId);
                _IMarsExplorePosItem oldFirst = __getFirstItem(posItems);
                posItems.Add(_posItem);
                posItems.Sort((_a, _b) => _a.startTime.CompareTo(_b.startTime));

                _IMarsExplorePosItem newFirst = __getFirstItem(posItems);

                // 当新添加的事件挤占首位时触发事件变更回调
                if (newFirst == _posItem)
                {
                    // 添加新的事件到首位
                    if (_posItem is _AMarsExploreEventInfo eventInfo)
                        _m_parentComp.onEventAdd?.Invoke(eventInfo);
                    else if (_posItem is _IMarsExploreMineItem mineInfo)
                        _m_parentComp.onMineAdd?.Invoke(mineInfo);
                    
                    // 移除旧的首位事件
                    if (oldFirst is _AMarsExploreEventInfo oldFirstEvent)
                        _m_parentComp.onEventRemove?.Invoke(oldFirstEvent);
                    else if (oldFirst is _IMarsExploreMineItem oldFirstMine)
                        _m_parentComp.onMineRemove?.Invoke(oldFirstMine);
                }
                else
                {
                    if (oldFirst is _AMarsExploreEventInfo eventInfo)
                        _m_parentComp.onEventChg?.Invoke(eventInfo, true);
                    else if (oldFirst is _IMarsExploreMineItem mineInfo)
                        _m_parentComp.onMineChg?.Invoke(mineInfo.instanceId, true);
                }
            }
            private void _removePosItem(_IMarsExplorePosItem _posItem)
            {
                List<_IMarsExplorePosItem> posItems = _m_posItems.getValueDefinitely(_posItem.posId);
                //获取第一个数据对象，判断是否刷新显示信息
                _IMarsExplorePosItem checkItem = __getFirstItem(posItems);
                bool wasTop = checkItem == _posItem;
                posItems.Remove(_posItem);
                
                // 当移除事件是首位时，触发事件变更回调
                if (wasTop)
                {
                    // 移除旧的首位事件
                    if (_posItem is _AMarsExploreEventInfo eventInfo)
                        _m_parentComp.onEventRemove?.Invoke(eventInfo);
                    else if (_posItem is _IMarsExploreMineItem mineInfo)
                        _m_parentComp.onMineRemove?.Invoke(mineInfo);
                    
                    _IMarsExplorePosItem newTopItem = __getFirstItem(posItems);
                    if (newTopItem is _AMarsExploreEventInfo newTopEvent)
                        _m_parentComp.onEventAdd?.Invoke(newTopEvent);
                    else if (newTopItem is _IMarsExploreMineItem newTopMine)
                        _m_parentComp.onMineAdd?.Invoke(newTopMine);
                }
                else
                {
                    _IMarsExplorePosItem firstItem = __getFirstItem(posItems);
                    if (firstItem is _AMarsExploreEventInfo eventInfo)
                        _m_parentComp.onEventChg?.Invoke(eventInfo, false);
                    else if (firstItem is _IMarsExploreMineItem mineInfo)
                        _m_parentComp.onMineChg?.Invoke(mineInfo.instanceId, false);
                }
            }

            /// <summary>
            /// 取出第一个有效的事件或采集点
            /// 原则上是，可领取奖励事件优先，正在采集的采集点其次，其它事件先展示，最后是未采集矿
            /// </summary>
            /// <param name="_posItemList"></param>
            /// <returns></returns>
            private _IMarsExplorePosItem __getFirstItem(List<_IMarsExplorePosItem> _posItemList)
            {
                if (null == _posItemList || _posItemList.Count <= 0)
                    return null;

                _AMarsExploreEventInfo doneEvent = null;
                _IMarsExplorePosItem occupyMine = null;
                _AMarsExploreEventInfo firstEvent = null;
                _IMarsExplorePosItem firstDefaultItem = null;
                foreach(_IMarsExplorePosItem posItem in _posItemList)
                {
                    if (null == firstDefaultItem)
                        firstDefaultItem = posItem;

                    //针对posItem类型进行区分处理
                    if (posItem is _AMarsExploreEventInfo eventInfo)
                    {
                        //设置第一个事件
                        if (null == firstEvent)
                            firstEvent = eventInfo;

                        //判断是否完成，是则尝试设置第一个完成
                        if(eventInfo.isDone)
                        {
                            if (null == doneEvent)
                                doneEvent = eventInfo;
                        }
                    }
                    else if (posItem is _IMarsExploreMineItem _mineItem)
                    {
                        //如果自己占有，则尝试设置挖矿对象
                        if(_mineItem.isMe)
                        {
                            if (null == occupyMine)
                                occupyMine = posItem;
                        }
                    }
                }

                //按优先级返回
                if (null != doneEvent)
                    return doneEvent;
                if(null != occupyMine)
                    return occupyMine;
                if(null != firstEvent)
                    return firstEvent;

                return firstDefaultItem;
            }
        }
    }
}
