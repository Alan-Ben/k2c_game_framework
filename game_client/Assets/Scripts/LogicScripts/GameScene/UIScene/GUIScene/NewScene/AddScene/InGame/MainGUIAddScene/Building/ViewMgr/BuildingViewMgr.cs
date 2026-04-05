
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class BuildingViewMgr
    {
        // 建筑表现对象的总字典
        [NotNull] private readonly Dictionary<long, BuildingView> _m_buildingViewDict;
        // 建筑的功能表现的管理器（如经营建筑，生产建筑等）
        [NotNull] private FunctionBuildingViewMgr _m_functionBuildingViewMgr;
        // 是否初始化和当前的序列号
        private bool _m_isInit;
        private int _m_initSerialize;
        
        
        public BuildingViewMgr()
        {
            _m_buildingViewDict = new Dictionary<long, BuildingView>();
            _m_functionBuildingViewMgr = new FunctionBuildingViewMgr();
        }
        
        
        /// <summary>
        /// 初始化建筑的展示内容
        /// </summary>
        /// <remarks>
        /// 根据玩家当前的数据，展示出完整的状态
        /// </remarks>
        public void init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            _m_isInit = true;
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_complete);
            stepCounter.chgTotalStepCount(1);
            {
                stepCounter.chgTotalStepCount(1);
                _m_functionBuildingViewMgr.init(stepCounter.addDoneStepCount);

                IReadOnlyList<BuildingInfo> buildingList = NPPlayer.instance.buildingComp.buildingList;
                foreach (BuildingInfo buildingInfo in buildingList)
                {
                    BuildingView buildingView = new BuildingView(_m_functionBuildingViewMgr, buildingInfo);
                    _m_buildingViewDict.Add(buildingInfo.id, buildingView);
                    stepCounter.chgTotalStepCount(1);
                    buildingView.load(stepCounter.addDoneStepCount);
                    if (buildingInfo == NPPlayer.instance.buildingComp.nextOrderedBuilding)
                        buildingView.setNextOrderedBuildTip(true);
                    if (buildingInfo.baseRef.build_order < 0 && buildingInfo.baseRef.build_condition.IsEnable(null))
                        buildingView.setNextOrderedBuildTip(true);
                }

                NPPlayer.instance.buildingComp.onBuildingBuilt += _onBuildingBuilt;
                NPPlayer.instance.buildingComp.onNextOrderedBuildingChg += _refreshNextOrderedBuildTip;
            }
            stepCounter.addDoneStepCount();
        }
        public void discard()
        {
            if (!_m_isInit)
                return;
            _m_isInit = false;
            
            NPPlayer.instance.buildingComp.onNextOrderedBuildingChg -= _refreshNextOrderedBuildTip;
            NPPlayer.instance.buildingComp.onBuildingBuilt -= _onBuildingBuilt;
            
            foreach (BuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.discard();
            }
            _m_buildingViewDict.Clear();
            
            _m_functionBuildingViewMgr.discard();
            _m_initSerialize = ALSerializeOpMgr.next();
        }

        /// <summary>
        /// 获取入口点对象
        /// </summary>
        /// <param name="_entryPointId"></param>
        /// <returns></returns>
        public _IGTDHoneEntryPointView getEntryPointView(long _entryPointId)
        {
            foreach (KeyValuePair<long, BuildingView> buildingView in _m_buildingViewDict)
            {
                if (buildingView.Value.entryPointViewList != null)
                {
                    for (int i = 0; i < buildingView.Value.entryPointViewList.Length; i++)
                    {
                        if (buildingView.Value.entryPointViewList[i].entryPointRefObj != null &&
                            buildingView.Value.entryPointViewList[i].entryPointRefObj.id == _entryPointId)
                            return buildingView.Value.entryPointViewList[i];
                    }
                }
            }

            return null;
        }


        private void _onBuildingBuilt(BuildingInfo _buildingInfo)
        {
            if (_buildingInfo == null)
                return;
            
            int serialize = _m_initSerialize;
            GCommon.setBuildingIsUnderConstruction(true, _buildingInfo.baseRef.build_sfx_id);
            showBuildingBuild(_buildingInfo, () =>
            {
                GCommon.setBuildingIsUnderConstruction(false, _buildingInfo.baseRef.build_sfx_id);
                if (serialize != _m_initSerialize)
                    return;
                
                //建造表现完成引导trigger
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.BUILD_BUILD_EFFECT_END);
                
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    GGUIWndBuildingBuildSuccess.instance.refreshWnd(_buildingInfo.baseRef, () =>
                    {
                        //关闭弹窗，触发对话
                        if (_buildingInfo.baseRef != null && _buildingInfo.baseRef.build_success_dialogue_id > 0)
                            GCommon.enterDialogueNode(_buildingInfo.baseRef.build_success_dialogue_id, null);
                    });
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBuildingBuildSuccess.instance, GGUIWndBuildingBuildSuccess.instance.showWnd, UINodeTagConst.C_BUILDING_BUILD_SUC);
                });
            });
        }
        private void _refreshNextOrderedBuildTip(long _lastBuildingId, long _nextBuildingId)
        {
            if (_m_buildingViewDict.TryGetValue(_lastBuildingId, out BuildingView lastBuildingView))
                lastBuildingView.setNextOrderedBuildTip(false);
            if (_m_buildingViewDict.TryGetValue(_nextBuildingId, out BuildingView nextBuildingView))
                nextBuildingView.setNextOrderedBuildTip(true);
        }


        private void showBuildingBuild(BuildingInfo _buildingInfo, Action _complete)
        {
            if (_buildingInfo == null)
            {
                _complete?.Invoke();
                return;
            }

            if (!_m_buildingViewDict.TryGetValue(_buildingInfo.id, out BuildingView buildingView))
            {
                _complete?.Invoke();
                return;
            }

            int serialize = _m_initSerialize;
            CommonTDSfxObj sfxObj = PlaySfxMgr.instance.playSfxByPos(_buildingInfo.baseRef.build_sfx_id, buildingView.position);
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_initSerialize)
                {
                    sfxObj?.forceDiscard();
                    _complete?.Invoke();
                    return;
                }
                    
                if (!_m_buildingViewDict.TryGetValue(_buildingInfo.id, out BuildingView currentView) || currentView != buildingView)
                {
                    sfxObj?.forceDiscard();
                    _complete?.Invoke();
                    return;
                }
                    
                buildingView.discard();
                BuildingView newBuildingView = new BuildingView(_m_functionBuildingViewMgr, _buildingInfo);
                _m_buildingViewDict[_buildingInfo.id] = newBuildingView;
                newBuildingView.load(() =>
                {
                    sfxObj?.forceDiscard();
                    _complete?.Invoke();
                });
            }, _buildingInfo.baseRef.build_sfx_delay);
        }
    }
}