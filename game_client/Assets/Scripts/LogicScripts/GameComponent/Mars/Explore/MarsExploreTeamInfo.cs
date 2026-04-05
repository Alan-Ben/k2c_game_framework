using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using Common.MarsEnum;
using Common.MarsObj;
using CommonEnum;
using GC2GS.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class MarsExploreTeamInfo : _IMarsTimeSpeedUpObject, _IMarsCompleteNowObject
    {
        [ItemNotNull, NotNull] private readonly List<HeroInfo> _m_heroList;

        private readonly long _m_teamId;
        [NotNull] private readonly MarsExploreTeamRefObj _m_refObj;

        private string _m_name;
        private long _m_statePosId;
        private long _m_stateStartTime;
        private long _m_oriStateEndTime;// 原始状态结束时间(不含被加速减少的时间)
        private EMarsExploreTeamState _m_state;
        //状态序列号，用于标记状态变化
        private long _m_lStateSerialize;
        private long _m_soldierLoss; // 队伍损耗
        private int _m_stateSerialize;
        private int _m_sentStateUpdateSerialize = -1;

        private long _m_soldierMax;
        private long _m_teamPower;
        private long _m_powerAddPer;
        
        private NPCommonCostItem _m_completeNowCostItem;

        //不同状态下，队伍信息可能存在的不同数据对象
        //不同数据对象根据其实际逻辑可能进行不同的数据处理
        private _IMarsTeamExDataInterface _m_edExDataObj;

        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalSoldierMaxDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalTeamPowerDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalPowerAddPerDealer;


        public MarsExploreTeamInfo([NotNull] MarsExploreTeamRefObj _refObj)
        {
            _m_heroList = new List<HeroInfo>();

            _m_teamId = _refObj.team_id;
            _m_refObj = _refObj;

            _m_recalSoldierMaxDealer = new LazyNextFrameTaskDealer(new _RecalSoldierMax(this));
            _m_recalTeamPowerDealer = new LazyNextFrameTaskDealer(new _RecalTeamPower(this));
            _m_recalPowerAddPerDealer = new LazyNextFrameTaskDealer(new _RecalPowerAddPer(this));

            needUpdateSoldierMax();

            //默认为空
            _m_edExDataObj = null;
        }
        
        
        public event Action onNameChg;
        public event Action onSoldierNumChg;
        public event Action onHeroListChg;
        public event Action onUIStateChg;
        public event Action onTeamPowerChg;
        
        public long teamId { get { return _m_teamId; } }
        public MarsExploreTeamRefObj refObj { get { return _m_refObj; } }
        public string name
        {
            get
            {
                if (string.IsNullOrEmpty(_m_name))
                    return TextTranslate.instance.getLanguage(_m_refObj.team_name);
                
                return _m_name;
            }
        }
        public EMarsExploreTeamState state { get { return _m_state; } }
        public long stateSerialize { get { return _m_lStateSerialize; } }
        public long statePosId { get { return _m_statePosId; } }
        public long stateStartTime { get { return _m_stateStartTime; } }
        public long stateEndTime { get 
            {
                return _m_oriStateEndTime - (null == _m_edExDataObj ? 0: _m_edExDataObj.exReduceTimeMS);
            }
        }
        public bool isUnlock { get { return _m_refObj.unlock_cond == null || _m_refObj.unlock_cond.IsEnable(null); } }
        public bool isShow { get { return _m_refObj.show_cond == null || _m_refObj.show_cond.IsEnable(null); } }
        public long soldierNum { get { return Math.Max(0, _m_soldierMax - _m_soldierLoss); } }
        public long soldierMax { get { return _m_soldierMax; } }
        public bool isSoldierFull { get { return soldierNum >= _m_soldierMax; } }
        public long soldierLoss { get { return _m_soldierLoss; } }
        public long stateRemainTimeMs { get { return Math.Max(0, stateEndTime - FpsAndPingMgr.instance.serverTimeTag); } }
        public int heroCount { get { return _m_heroList.Count; } }
        public NPCommonCostItem completeNowCostItem
        {
            get
            {
                _m_completeNowCostItem ??= new NPCommonCostItem(ENPItemType.CURRENCY, (int)ECurrency.GEM, 0);
                long gemCost = 0;
                if (((_IMarsCompleteNowObject)this).hasRemainTime)
                    gemCost = MarsUtil.calculateGemCostForSpeedUpMs(stateRemainTimeMs);
                _m_completeNowCostItem.setCount(gemCost);
                return _m_completeNowCostItem;
            }
        }
        public _IMarsTeamExDataInterface exDataObj { get { return _m_edExDataObj; } }

        
        public List<HeroInfo> getHeroList()
        {
            return new List<HeroInfo>(_m_heroList);
        }
        public void getHeroListNonAlloc(List<HeroInfo> _heroList)
        {
            if (_heroList == null)
                return;
            
            _heroList.Clear();
            _heroList.AddRange(_m_heroList);
        }
        public void actionWithHero(Action<HeroInfo> _action)
        {
            if (_action == null)
                return;

            foreach (HeroInfo heroInfo in _m_heroList)
                _action(heroInfo);
        }
        public HeroInfo getHeroByIndex(int _index)
        {
            if (_index < 0 || _index >= _m_heroList.Count)
                return null;

            return _m_heroList[_index];
        }
        public bool containsHero(long _heroId)
        {
            foreach (HeroInfo heroInfo in _m_heroList)
            {
                if (heroInfo.id == _heroId)
                    return true;
            }
            return false;
        }
        public string getUnlockConditionDesc()
        {
            return TextTranslate.instance.getLanguage(_m_refObj.unlock_cond_desc, _m_refObj.unlock_cond_param_list);
        }
        public long getTeamPower()
        {
            return _m_teamPower;
        }
        public EMarsExploreTeamUIState getUIState()
        {
            if (!isUnlock)
                return EMarsExploreTeamUIState.Lock;

            switch (state)
            {
                case EMarsExploreTeamState.ERROR:
                    return EMarsExploreTeamUIState.Empty;
                case EMarsExploreTeamState.NONE:
                case EMarsExploreTeamState.IDLE:
                {
                    if (_m_heroList.Count <= 0)
                        return EMarsExploreTeamUIState.Empty;
                    if (_m_soldierLoss > 0)
                        return EMarsExploreTeamUIState.IdleSoldierLoss;
                    return EMarsExploreTeamUIState.Idle;
                }
                case EMarsExploreTeamState.MARCH:
                    return EMarsExploreTeamUIState.Exploring;
                case EMarsExploreTeamState.BACK:
                    return EMarsExploreTeamUIState.Back;
                case EMarsExploreTeamState.REPAIR:
                    if(NPPlayer.instance.guildMarsHelpComp.canAskGuildHelp(EGuildMarsHelpObjType.TEAM_REPAIR, _m_teamId) )
                        return EMarsExploreTeamUIState.CanAskHelp;
                    return EMarsExploreTeamUIState.Repairing;
                case EMarsExploreTeamState.COLLECT:
                case EMarsExploreTeamState.BATTLE:
                    return EMarsExploreTeamUIState.Collecting;
                default:
                    return EMarsExploreTeamUIState.Empty;
            }
        }
        public long getPowerAddPer()
        {
            return _m_powerAddPer;
        }
        public void needUpdateSoldierMax()
        {
            _m_recalSoldierMaxDealer.setNeedDeal();
        }
        public void needUpdateTeamPower()
        {
            _m_recalTeamPowerDealer.setNeedDeal();
        }
        public void needUpdatePowerAddPer()
        {
            _m_recalPowerAddPerDealer.setNeedDeal();
        }


        /// <summary>
        /// 初始化数据函数，初始化不会调用相关事件
        /// </summary>
        /// <param name="_msg"></param>
        internal void _initData(Mars_Team _msg)
        {
            if (_msg == null)
                return;

            if (_m_teamId != _msg.getTeamId())
            {
                ALLog.Warning("[MarsExploreTeamInfo::_updateData] teamId 不匹配, 忽略更新");
                return;
            }

            _initName(_msg.getName());
            _initState(_msg.getState());
            _initHeroList(_msg.getHeroIdList());
            _initSoldierLoss(_msg.getLossValue());

            //设置需要更新数据
            needUpdatePowerAddPer();
        }
        internal void _initName(string _newName)
        {
            _m_name = _newName;
        }
        internal void _initState(Mars_TeamState _state)
        {
            if (_state == null)
                return;

            _IMarsTeamExDataInterface preExData = _m_edExDataObj;
            _m_edExDataObj = null;
            if (null != preExData)
                preExData.onExitExData();

            _m_state = _state.getState();
            _m_lStateSerialize = _state.getStateSerialize();
            _m_stateStartTime = _state.getStateStartMs();
            _m_oriStateEndTime = _state.getStateEndMs();
            _m_statePosId = _state.getPos();
            _m_stateSerialize = ALSerializeOpMgr.next();

            //构造扩展数据结构体
            _m_edExDataObj = _createTeamExData(_state.getExData());
            //调用进入处理
            if (null != _m_edExDataObj)
                _m_edExDataObj.onEnterExData();
        }
        internal void _initHeroList(List<long> _heroIdList)
        {
            _m_heroList.Clear();
            if (_heroIdList != null)
            {
                foreach (long heroId in _heroIdList)
                {
                    HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(heroId);
                    if (heroInfo == null)
                    {
                        ALLog.Error($"[MarsExploreTeamInfo::_updateData] heroId {heroId} 对应的 HeroInfo 不存在, 忽略添加");
                        return;
                    }

                    _m_heroList.Add(heroInfo);
                }
            }

            needUpdatePowerAddPer();
            needUpdateSoldierMax();
        }
        internal void _initSoldierLoss(long _lossValue) 
        {
            _m_soldierLoss = _lossValue;
            needUpdateTeamPower();
        }

        internal void _updateData(Mars_Team _msg)
        {
            if (_msg == null)
                return;

            if (_m_teamId != _msg.getTeamId())
            {
                ALLog.Warning("[MarsExploreTeamInfo::_updateData] teamId 不匹配, 忽略更新");
                return;
            }

            _updateName(_msg.getName());
            _updateState(_msg.getState());
            _updateHeroList(_msg.getHeroIdList());
            _updateSoldierLoss(_msg.getLossValue());
            needUpdatePowerAddPer();
        }

        internal void _updateName(string _newName)
        {
            _m_name = _newName;
            onNameChg?.Invoke();
        }
        internal void _updateState(Mars_TeamState _state)
        {
            if (_state == null)
                return;

            //判断原状态是否有数据对象，调用离开处理
            //注意，此处需要使用状态序列号进行判断和处理，避免状态未变化，只是数据变化，导致状态进出
            bool isStateChg = (stateSerialize != _state.getStateSerialize());
            if (isStateChg)
            {
                _IMarsTeamExDataInterface preExData = _m_edExDataObj;
                _m_edExDataObj = null;
                if (null != preExData)
                    preExData.onExitExData();
            }

            //构造新数据对象
            _m_state = _state.getState();
            _m_lStateSerialize = _state.getStateSerialize();
            _m_stateStartTime = _state.getStateStartMs();
            _m_oriStateEndTime = _state.getStateEndMs();
            _m_statePosId = _state.getPos();
            _m_stateSerialize = ALSerializeOpMgr.next();

            //构造扩展数据结构体
            if (isStateChg)
            {
                _m_edExDataObj = _createTeamExData(_state.getExData());
                //调用进入处理
                if (null != _m_edExDataObj)
                    _m_edExDataObj.onEnterExData();
            }
            else
            {
                _IMarsTeamExDataInterface tmpData = _createTeamExData(_state.getExData());
                //状态未变化，数据变化，调用数据变化处理
                if (null != _m_edExDataObj)
                    _m_edExDataObj.onUpdateExData(tmpData);
            }

            onUIStateChg?.Invoke();
        }
        internal void _updateHeroList(List<long> _heroIdList)
        {
            _m_heroList.Clear();
            if (_heroIdList != null)
            {
                foreach (long heroId in _heroIdList)
                {
                    HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(heroId);
                    if (heroInfo == null)
                    {
                        ALLog.Error($"[MarsExploreTeamInfo::_updateData] heroId {heroId} 对应的 HeroInfo 不存在, 忽略添加");
                        return;
                    }

                    _m_heroList.Add(heroInfo);
                }
            }

            onHeroListChg?.Invoke();
            onUIStateChg?.Invoke();
            needUpdatePowerAddPer();
            needUpdateSoldierMax();
        }
        internal void _updateSoldierLoss(long _lossValue)
        {
            _m_soldierLoss = _lossValue;
            onSoldierNumChg?.Invoke();
            WinMsg.SendMsg(WinMsgType.ON_MARS_TEAM_SOLDIER_NUM_CHG, this);
            needUpdateTeamPower();
        }

        internal void _updateTime()
        {
            if (stateEndTime <= 0)
                return;
            
            long timeNow = FpsAndPingMgr.instance.serverTimeTag;
            switch (_m_state)
            {
                case EMarsExploreTeamState.BACK:
                case EMarsExploreTeamState.REPAIR:
                case EMarsExploreTeamState.BATTLE:
                {
                    if (timeNow >= stateEndTime && _m_sentStateUpdateSerialize != _m_stateSerialize)
                    {
                        _m_sentStateUpdateSerialize = _m_stateSerialize;
                        NPGSClientListener.sendMsgByLog(new GC2GS_041_008_ReqNoticeExploreTeamState(teamId));
                    }
                    break;
                    }
                case EMarsExploreTeamState.MARCH:
                    {
                        //获取前往的目标状态，如果是挖矿不发送请求协议
                        if (_m_edExDataObj is MarsTeamEx_March marchExData)
                        {
                            //挖矿行为不主动发检查
                            if (marchExData.data.getTargetState() != (int)EMarsExploreTeamState.COLLECT)
                            {
                                if (timeNow >= stateEndTime && _m_sentStateUpdateSerialize != _m_stateSerialize)
                                {
                                    _m_sentStateUpdateSerialize = _m_stateSerialize;
                                    NPGSClientListener.sendMsgByLog(new GC2GS_041_008_ReqNoticeExploreTeamState(teamId));
                                }
                            }
                        }
                        break;
                    }
                case EMarsExploreTeamState.COLLECT:
                    break;
                case EMarsExploreTeamState.NONE:
                    break;
                case EMarsExploreTeamState.ERROR:
                    break;
                case EMarsExploreTeamState.IDLE:
                    break;
            }
        }

        public void onItemHelpSecsChg(long _itemHelpSecs)
        {
            if(_m_edExDataObj != null && _m_edExDataObj is MarsTeamEx_Repaire repaireExData)
            {
                repaireExData.onItemHelpSecsChg(_itemHelpSecs);
            }
        }
        
        /// <summary>
        /// 根据带入的数据以及当前队伍状态，构造相关的队伍扩展数据对象
        /// </summary>
        /// <param name="_exData"></param>
        /// <returns></returns>
        protected _IMarsTeamExDataInterface _createTeamExData(byte[] _exData)
        {
            switch(_m_state)
            {
                case EMarsExploreTeamState.REPAIR:
                    return new MarsTeamEx_Repaire(this, _exData);
                case EMarsExploreTeamState.COLLECT:
                    return new MarsTeamEx_Collect(this, _exData);
                case EMarsExploreTeamState.MARCH:
                    return new MarsTeamEx_March(this, _exData);
                default:
                    return null;
            }
        }

        private class _RecalSoldierMax : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsExploreTeamInfo _m_teamInfo;

            public _RecalSoldierMax([NotNull] MarsExploreTeamInfo _teamInfo)
            {
                _m_teamInfo = _teamInfo;
            }

            public void deal()
            {
                long totalValue = MarsUtil.calculateMarsExploreTeamSoldierMax(_m_teamInfo._m_heroList);

                if (_m_teamInfo._m_soldierMax != totalValue)
                {
                    _m_teamInfo._m_soldierMax = totalValue;
                    _m_teamInfo.onSoldierNumChg?.Invoke();

                    _m_teamInfo.needUpdateTeamPower();
                }
            }
        }
        private class _RecalTeamPower : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsExploreTeamInfo _m_teamInfo;

            public _RecalTeamPower([NotNull] MarsExploreTeamInfo _teamInfo)
            {
                _m_teamInfo = _teamInfo;
            }

            public void deal()
            {
                long soldierNum = _m_teamInfo.soldierNum;
                long teamPowerAddPer = _m_teamInfo._m_powerAddPer;

                long totalValue = MarsUtil.calculateMarsExploreTeamPower(soldierNum, teamPowerAddPer);

                if (_m_teamInfo._m_teamPower != totalValue)
                {
                    _m_teamInfo._m_teamPower = totalValue;
                    _m_teamInfo.onTeamPowerChg?.Invoke();
                }
            }
        }
        private class _RecalPowerAddPer : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsExploreTeamInfo _m_teamInfo;

            public _RecalPowerAddPer([NotNull] MarsExploreTeamInfo _teamInfo)
            {
                _m_teamInfo = _teamInfo;
            }

            public void deal()
            {
                long powerAddPer = MarsUtil.calculateMarsExploreTeamPowerAddPer(_m_teamInfo._m_heroList);

                if (_m_teamInfo._m_powerAddPer != powerAddPer)
                {
                    _m_teamInfo._m_powerAddPer = powerAddPer;
                    _m_teamInfo.needUpdateTeamPower();
                }
            }
        }


        EMarsBagItemUseTimeType _IMarsTimeSpeedUpObject.timeType { get { return _m_edExDataObj?.timeType ?? default; } }
        long _IMarsTimeSpeedUpObject.timeObjId { get { return _m_teamId; } }
        bool _IMarsCompleteNowObject.checkCanCompleteNow(bool _checkCompleteNowCostEnough, bool _showUnableTip)
        {
            // 若需要检查立即完成消耗是否足够, 且 立即完成消耗不足
            return !_checkCompleteNowCostEnough || GCommon.isItemEnough(completeNowCostItem, _showUnableTip);
        }
        bool _IMarsCompleteNowObject.hasRemainTime { get { return _m_edExDataObj != null && _m_edExDataObj.timeType != EMarsBagItemUseTimeType.NONE; } }
        long _IMarsCompleteNowObject.remainTimeMs { get { return stateRemainTimeMs; } }
        NPCommonCostItem _IMarsCompleteNowObject.completeNowCostItem { get { return completeNowCostItem; } }
        Action<Action<bool>> _IMarsCompleteNowObject.dealCompleteNowAction
        {
            get { return _m_edExDataObj == null ? null : _m_edExDataObj.reqCompleteNow; }
        }
        long _IMarsTimeSpeedUpObject.remainTimeMs { get { return stateRemainTimeMs; } }
        public long beforeReductionTotalTimeMs { get { return _m_oriStateEndTime - stateStartTime; } }
        public long afterReductionTotalTimeMs { get { return stateEndTime - stateStartTime; } }
        long _IMarsTimeSpeedUpObject.guildHelpId { get { return _m_edExDataObj?.guildHelpId ?? 0; } }
    }
}