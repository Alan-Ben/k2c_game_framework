using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ALPackage;
using Common.MarsEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsExploreViewMgr : _AALBasicLoadObj
    {
        [CanBeNull] public static MarsExploreViewMgr instance { get { return _g_instance; } }
        private static MarsExploreViewMgr _g_instance;


        [NotNull] private readonly Dictionary<MarsExploreTeamInfo, MarsExploreTeamView> _m_teamViewDict;
        [NotNull] private readonly Dictionary<long, _AMarsExploreEventView> _m_eventViewDict;
        [NotNull] private readonly Dictionary<long, MarsExploreMineView> _m_mineViewDict;
        private MarsExploreHomeBaseView _m_homeBaseView;
        private ALCommonEnableTaskController _m_tickTask;


        public MarsExploreViewMgr()
        {
            _m_teamViewDict = new Dictionary<MarsExploreTeamInfo, MarsExploreTeamView>();
            _m_eventViewDict = new Dictionary<long, _AMarsExploreEventView>();
            _m_mineViewDict = new Dictionary<long, MarsExploreMineView>();
        }


        public MarsExploreHomeBaseView homeBaseView { get { return _m_homeBaseView; } }
        

        public void focusOnTeam(long _teamId)
        {
            
        }


        protected override void _loadOp()
        {
            MarsExploreSubComponent exploreComp = NPPlayer.instance.marsComp.exploreSubComponent;

            ALStepCounter stepCounter = new ALStepCounter();
            //2个队列加载加上2个初始化处理，每个队列加载的时候单独增加步骤
            stepCounter.chgTotalStepCount(2 + 2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _g_instance = this;
                _setLoadDone();
            });

            _m_homeBaseView = new MarsExploreHomeBaseView(this);
            _m_homeBaseView.load(stepCounter.addDoneStepCount);

            //处理队伍信息
            NPPlayer.instance.marsComp.exploreSubComponent.doEachTeam(
                (_teamInfo) =>
                {
                    if (!_needTeamView(_teamInfo.state))
                        return;

                    if (_m_teamViewDict.ContainsKey(_teamInfo))
                        return;

                    //增加加载步骤
                    stepCounter.chgTotalStepCount(1);

                    //增加数据对象并加载
                    MarsExploreTeamView teamView = new MarsExploreTeamView(_teamInfo);
                    _m_teamViewDict.Add(_teamInfo, teamView);
                    teamView.load(stepCounter.addDoneStepCount);
                });
            //完成第一次加载
            stepCounter.addDoneStepCount();

            //逐个位置对象进行处理
            NPPlayer.instance.marsComp.exploreSubComponent.doEachPosItem(
                (_posItem) =>
                { 
                    //针对posItem类型进行区分处理
                    if(_posItem is _AMarsExploreEventInfo eventInfo)
                    {
                        long instanceId = eventInfo.instanceId;
                        if (_m_eventViewDict.ContainsKey(instanceId))
                            return;

                        _AMarsExploreEventView eventView = _createEventView(eventInfo);
                        if (eventView == null)
                            return;

                        //增加加载步骤
                        stepCounter.chgTotalStepCount(1);

                        //开启加载
                        _m_eventViewDict.Add(instanceId, eventView);
                        eventView.load(stepCounter.addDoneStepCount);
                    }
                    else if(_posItem is _IMarsExploreMineItem _mineItem)
                    {
                        long instanceId = _mineItem.instanceId;
                        if (_m_mineViewDict.ContainsKey(instanceId))
                            return;

                        //增加加载步骤
                        stepCounter.chgTotalStepCount(1);

                        MarsExploreMineView mineView = new MarsExploreMineView(_mineItem);
                        _m_mineViewDict.Add(instanceId, mineView);
                        mineView.load(stepCounter.addDoneStepCount);

                        //调用刷新检查
                        _mineItem.checkInfo();
                    }
                });
            //完成第二次加载
            stepCounter.addDoneStepCount();

            exploreComp.onTeamStateChg += _onTeamStateChg;
            exploreComp.onEventAdd += _onEventAdd;
            exploreComp.onEventRemove += _onEventRemove;
            exploreComp.onEventChg += _onEventChg;
            exploreComp.onMineAdd += _onMineAdd;
            exploreComp.onMineRemove += _onMineRemove;
            exploreComp.onMineChg += _onMineChg;
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);

            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_EVENT, _simulateClickEvent);

            stepCounter.addDoneStepCount();
        }
        protected override void _discard()
        {
            if (_g_instance == this)
                _g_instance = null;

            _m_homeBaseView?.discard();
            _m_homeBaseView = null;

            foreach (MarsExploreTeamView teamView in _m_teamViewDict.Values)
            {
                teamView.discard();
            }
            _m_teamViewDict.Clear();

            foreach (_AMarsExploreEventView eventView in _m_eventViewDict.Values)
            {
                eventView.discard();
            }
            _m_eventViewDict.Clear();

            foreach (MarsExploreMineView mineView in _m_mineViewDict.Values)
            {
                mineView.discard();
            }
            _m_mineViewDict.Clear();

            _m_tickTask.setDisable();

            MarsExploreSubComponent exploreComp = NPPlayer.instance.marsComp.exploreSubComponent;
            exploreComp.onTeamStateChg -= _onTeamStateChg;
            exploreComp.onEventAdd -= _onEventAdd;
            exploreComp.onEventRemove -= _onEventRemove;
            exploreComp.onEventChg -= _onEventChg;
            exploreComp.onMineAdd -= _onMineAdd;
            exploreComp.onMineRemove -= _onMineRemove;
            exploreComp.onMineChg -= _onMineChg;

            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_EVENT, _simulateClickEvent);
        }


        private void _tick()
        {
            NPPlayer.instance.marsComp.exploreSubComponent.updateTime();
            foreach (MarsExploreTeamView teamView in _m_teamViewDict.Values)
            {
                teamView.tick();
            }
            foreach (_AMarsExploreEventView eventView in _m_eventViewDict.Values)
            {
                eventView.tick();
            }
            foreach (MarsExploreMineView mineView in _m_mineViewDict.Values)
            {
                mineView.tick();
            }
        }
        private void _onTeamStateChg(MarsExploreTeamInfo _teamInfo)
        {
            if (_teamInfo == null)
                return;

            bool needView = _needTeamView(_teamInfo.state);
            bool hasView = _m_teamViewDict.ContainsKey(_teamInfo);

            if (needView && !hasView)
            {
                // Team entered MARCH or BACK state, create view
                MarsExploreTeamView teamView = new MarsExploreTeamView(_teamInfo);
                _m_teamViewDict.Add(_teamInfo, teamView);
                teamView.load();
            }
            else if (!needView && hasView)
            {
                // Team left MARCH or BACK state, remove view
                if (_m_teamViewDict.TryGetValue(_teamInfo, out MarsExploreTeamView teamView))
                {
                    teamView.discard();
                    _m_teamViewDict.Remove(_teamInfo);
                }
            }
        }
        private void _onEventAdd(_AMarsExploreEventInfo _eventInfo)
        {
            if (_eventInfo == null)
                return;

            long instanceId = _eventInfo.instanceId;
            if (_m_eventViewDict.ContainsKey(instanceId))
                return;

            _AMarsExploreEventView eventView = _createEventView(_eventInfo);
            if (eventView == null)
            {
                ALLog.Error($"[MarsExploreViewMgr] 创建事件视图失败, instanceId={instanceId}");
                return;
            }

            _m_eventViewDict.Add(instanceId, eventView);
            eventView.setNeedShowLoadedEffect();
            eventView.load();
        }
        private void _onEventRemove(_AMarsExploreEventInfo _eventInfo)
        {
            if (_eventInfo == null)
                return;

            long instanceId = _eventInfo.instanceId;
            if (!_m_eventViewDict.TryGetValue(instanceId, out _AMarsExploreEventView eventView))
                return;

            eventView.discard();
            _m_eventViewDict.Remove(instanceId);
        }
        private void _onEventChg(_AMarsExploreEventInfo _eventInfo, bool _newEventStacked)
        {
            if (_eventInfo == null)
                return;

            long instanceId = _eventInfo.instanceId;
            if (!_m_eventViewDict.TryGetValue(instanceId, out _AMarsExploreEventView eventView))
                return;

            eventView.updateView(_newEventStacked);
        }
        private _AMarsExploreEventView _createEventView(_AMarsExploreEventInfo _eventInfo)
        {
            if (_eventInfo == null)
                return null;

            _AMarsExploreEventView eventView = null;
            switch (_eventInfo.eventType)
            {
                case EMarsExploreEventType.BATTLE:
                    eventView = new MarsExploreBattleEventView(this, (MarsExploreBattleEventInfo)_eventInfo);
                    break;
                case EMarsExploreEventType.BOSS:
                    eventView = new MarsExploreBossEventView(this, (MarsExploreBossEventInfo)_eventInfo);
                    break;
            }

            return eventView;
        }
        private bool _needTeamView(EMarsExploreTeamState _state)
        {
            return _state is EMarsExploreTeamState.MARCH or EMarsExploreTeamState.BACK;
        }
        private void _onMineAdd(_IMarsExploreMineItem _mineInfo)
        {
            if (_mineInfo == null)
                return;

            long instanceId = _mineInfo.instanceId;
            if (_m_mineViewDict.ContainsKey(instanceId))
                return;

            MarsExploreMineView mineView = new MarsExploreMineView(_mineInfo);
            _m_mineViewDict.Add(instanceId, mineView);
            mineView.setNeedShowLoadedEffect();
            mineView.load();
        }
        private void _onMineRemove(_IMarsExploreMineItem _mineInfo)
        {
            if (_mineInfo == null)
                return;

            long instanceId = _mineInfo.instanceId;
            if (!_m_mineViewDict.TryGetValue(instanceId, out MarsExploreMineView mineView))
                return;

            mineView.discard();
            _m_mineViewDict.Remove(instanceId);
        }
        private void _onMineChg(long _mineInstanceId, bool _newEventStacked)
        {
            if (!_m_mineViewDict.TryGetValue(_mineInstanceId, out MarsExploreMineView mineView))
                return;

            mineView.updateView(_newEventStacked);
        }
        private void _simulateClickEvent(object[] _params)
        {
            if (_params == null || _params.Length < 1 || _params[0] is not long posId)
                return;

            foreach (_AMarsExploreEventView eventView in _m_eventViewDict.Values)
            {
                if (eventView.eventInfo.posId == posId)
                {
                    eventView.triggerClick();
                    return;
                }
            }
        }
    }
}
