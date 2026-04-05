using System;
using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using Common.MarsObj;
using GC2GS.p039_MarsBuildingOp;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingViewMgr : _AALBasicLoadObj
    {
        [NotNull] private readonly Dictionary<MarsBuildingInfo, _AMarsBuildingView> _m_buildingViewDict;
        private ALCommonEnableTaskController _m_tickTask;
        private int _m_loadSerialize;
        
        
        public MarsBuildingViewMgr()
        {
            _m_buildingViewDict = new Dictionary<MarsBuildingInfo, _AMarsBuildingView>();
        }
        
        
        public _AMarsBuildingView getBuildingView(MarsBuildingInfo _buildingInfo)
        {
            if (_buildingInfo == null)
                return null;

            _m_buildingViewDict.TryGetValue(_buildingInfo, out _AMarsBuildingView view);
            return view;
        }
        
        
        protected override void _loadOp()
        {
            List<MarsBuildingInfo> buildingInfoList = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoList();
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(buildingInfoList.Count + 1);
            stepCounter.regAllDoneDelegate(_setLoadDone);
            
            foreach (MarsBuildingInfo buildingInfo in buildingInfoList)
            {
                if (_m_buildingViewDict.ContainsKey(buildingInfo))
                {
                    continue;
                }

                _AMarsBuildingView buildingView = MarsBuildingViewFactory.instance.createBuildingView(buildingInfo);
                if (buildingView == null)
                    continue;
                
                _m_buildingViewDict.Add(buildingInfo, buildingView);
                buildingView.load(stepCounter.addDoneStepCount);
            }

            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _buildingStateChg;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingLevelChg += _buildingLvlChg;
            NPPlayer.instance.marsComp.buildingSubComponent.updateClientData();
            WinMsg.RegisterMsgAct(WinMsgType.TRIGGER_MARS_ENERGY_COLLECT, _tryCollectMarsEnergy);
            WinMsg.RegisterMsg(WinMsgType.TRIGGER_MARS_BUILDING_FOCUS, _tryFocusToBuilding);
            WinMsg.RegisterMsg(WinMsgType.TRIGGER_MARS_BUILDING_INTELLIGENT_CONTROL_EFFECT, _playIntelligentControlEffect);
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
            
            stepCounter.addDoneStepCount();
        }
        protected override void _discard()
        {
            foreach (_AMarsBuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.discard();
            }
            _m_buildingViewDict.Clear();
            _m_tickTask.setDisable();
            _m_loadSerialize = ALSerializeOpMgr.next();
            
            WinMsg.UnregisterMsg(WinMsgType.TRIGGER_MARS_BUILDING_INTELLIGENT_CONTROL_EFFECT, _playIntelligentControlEffect);
            WinMsg.UnregisterMsg(WinMsgType.TRIGGER_MARS_BUILDING_FOCUS, _tryFocusToBuilding);
            WinMsg.UnregisterMsgAct(WinMsgType.TRIGGER_MARS_ENERGY_COLLECT, _tryCollectMarsEnergy);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingLevelChg -= _buildingLvlChg;
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _buildingStateChg;
        }


        private void _tick()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.updateClientData();
            NPPlayer.instance.marsComp.exploreSubComponent.updateTime();
            foreach (_AMarsBuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.tick();
            }
        }
        private void _recreateBuildingView(MarsBuildingInfo _buildingInfo, Action _complete)
        {
            if (_buildingInfo == null)
            {
                _complete?.Invoke();
                return;
            }

            _m_buildingViewDict.TryGetValue(_buildingInfo, out _AMarsBuildingView view);
            view?.discard();
                    
            _AMarsBuildingView buildingView = MarsBuildingViewFactory.instance.createBuildingView(_buildingInfo);
            if (buildingView == null)
            {
                _m_buildingViewDict.Remove(_buildingInfo);
                _complete?.Invoke();
                return;
            }
                
            _m_buildingViewDict[_buildingInfo] = buildingView;
            buildingView.load(_complete);
        }
        private void _buildingStateChg(long _buildingId, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(_buildingId);
            if (buildingInfo == null)
                return;
            
            _recreateBuildingView(buildingInfo, null);
            
            // 播放建筑建造特效
            if (_oldState is MarsBuildingInfo.StateType.Constructing &&
                _newState is MarsBuildingInfo.StateType.Normal &&
                _m_buildingViewDict.TryGetValue(buildingInfo, out _AMarsBuildingView view) &&
                view != null)
            {
                string sideTipStr = TextTranslate.instance.getLanguage(TransKeyConst.mars_buildingConstructedComplete_name, buildingInfo.nameTranslated);
                MainAdditionMarsTDScene.instance.playBuildingUpgradeEffect(view.position, sideTipStr, buildingInfo.refObj);
                
                //尝试触发新手引导
                GCommon.triggerTutorial();
            }
        }
        
        private void _buildingLvlChg(long _buildingId, long _oldLevel) 
        {
            MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(_buildingId);
            if (buildingInfo == null)
                return;
            
            _recreateBuildingView(buildingInfo, null);

            //播放升级特效
            if (buildingInfo.level > _oldLevel && _m_buildingViewDict.TryGetValue(buildingInfo, out _AMarsBuildingView view) && view != null)
            {
                string sideTipStr = TextTranslate.instance.getLanguage(TransKeyConst.mars_buildingUpgradeComplete_name_level, buildingInfo.nameTranslated, buildingInfo.level);
                MainAdditionMarsTDScene.instance.playBuildingUpgradeEffect(view.position, sideTipStr, buildingInfo.refObj);
            }
        }
        private void _tryCollectMarsEnergy()
        {
            int serialize = _m_loadSerialize;
            NPPlayer.instance.marsComp.buildingSubComponent.collectEnergy(_msg =>
            {
                if (serialize != _m_loadSerialize)
                    return;

                List<Mars_BuildingGainEnergyResult> collectList = _msg.getList();
                foreach (Mars_BuildingGainEnergyResult result in collectList)
                {
                    MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(result.getBuildingId());
                    if (buildingInfo == null)
                        continue;
                    
                    _m_buildingViewDict.TryGetValue(buildingInfo, out _AMarsBuildingView view);
                    if (view is _IEnergyMarsBuildingView energyView)
                        energyView.playCollectEnergyEffect(result.getCount());
                }
            });   
        }
        private void _tryFocusToBuilding(object[] _params)
        {
            if (_params == null || _params.Length < 1)
                return;

            float jumpTime = 0.5f;
            if (_params.Length >= 2 && _params[1] is float f)
                jumpTime = f;

            MarsBuildingInfo buildingInfo = null;
            if (_params[0] is long buildingId)
            {
                buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(buildingId);
            }
            else if (_params[0] is MarsBuildingInfo info)
            {
                buildingInfo = info;
            }
            if (buildingInfo == null)
                return;
            
            _m_buildingViewDict.TryGetValue(buildingInfo, out _AMarsBuildingView view);
            MainAdditionMarsTDScene.instance.focusToTarget(view.position, jumpTime);
        }
        private void _playIntelligentControlEffect(object[] _params)
        {
            // 参数：_params[0] 为 long buildingId 或 MarsBuildingInfo
            //      _params[1] 为 long intelligentControlId
            //      _params[2] (可选) 为 Action 完成回调
            if (_params == null || _params.Length < 2)
            {
                Debug.LogError($"[MarsBuildingViewMgr]_playIntelligentControlEffect 参数错误, 参数数量不足");
                return;
            }

            MarsBuildingInfo buildingInfo = null;
            if (_params[0] is long buildingId)
            {
                buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(buildingId);
            }
            else if (_params[0] is MarsBuildingInfo info)
            {
                buildingInfo = info;
            }

            if (buildingInfo == null || !(_params[1] is long intelligentControlId))
            {
                Debug.LogError($"[MarsBuildingViewMgr]_playIntelligentControlEffect 参数错误, 建筑信息(buildingInfo:{buildingInfo})或智能控制ID:{_params[1]}无效");
                return;
            }

            Action complete = null;
            if (_params.Length >= 3 && _params[2] is Action action)
                complete = action;

            if (_m_buildingViewDict.TryGetValue(buildingInfo, out _AMarsBuildingView view) && view != null)
            {
                view.playIntelligentControlEffect(intelligentControlId, complete);
            }
            else
            {
                complete?.Invoke();
            }
        }
    }
}