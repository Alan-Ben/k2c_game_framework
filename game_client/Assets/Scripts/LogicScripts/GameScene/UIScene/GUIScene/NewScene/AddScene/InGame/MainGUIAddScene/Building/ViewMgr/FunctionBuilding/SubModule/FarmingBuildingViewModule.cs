using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class FarmingBuildingViewModule : _AFunctionBuildingViewMgrSubModule
    {
        [NotNull] private readonly Dictionary<GTDMonoFarmingFunction, FarmingBuildingView> _m_buildingViewDict;
        private bool _m_isInit;
        private int _m_initSerialize;
        private ALCommonEnableTaskController _m_earningDurationTask;
        
        public FarmingBuildingViewModule()
        {
            _m_buildingViewDict = new Dictionary<GTDMonoFarmingFunction, FarmingBuildingView>();
        }


        public override void init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            _m_isInit = true;
            
            NPPlayer.instance.buildingComp.onFarmingBuildingChg += _onFarmingBuildingChg;
            WinMsg.RegisterMsg(WinMsgType.MOVE_FOCUS_TO_FARMING_BUILDING_UPGRADE_BTN_AND_SHOW_GUIDE_HAND, _moveFocusToFarmingBuildingUpgradeBtnAndShowGuideHand);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_FARMING_BUILDING_COLLECTION, _simulateClickCollection);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BUILDING_LEVEL_CHG, _onBuildingLevelChg);

            float time = MainAdditionBuildingTDScene.instance.sceneConfig.farmingBuildingEarningTipSpace;
            if (time <= 0)
                time = 1f;
            _m_earningDurationTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_showEarningTip, time);
            
            _complete?.Invoke();
        }
        public override void discard()
        {
            if (!_m_isInit)
                return;
            _m_isInit = false;
            
            _m_earningDurationTask.setDisable();
            
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BUILDING_LEVEL_CHG, _onBuildingLevelChg);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_FARMING_BUILDING_COLLECTION, _simulateClickCollection);
            WinMsg.UnregisterMsg(WinMsgType.MOVE_FOCUS_TO_FARMING_BUILDING_UPGRADE_BTN_AND_SHOW_GUIDE_HAND, _moveFocusToFarmingBuildingUpgradeBtnAndShowGuideHand);
            NPPlayer.instance.buildingComp.onFarmingBuildingChg -= _onFarmingBuildingChg;

            foreach (FarmingBuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.discard();
            }
            _m_buildingViewDict.Clear();
            
            _m_initSerialize = ALSerializeOpMgr.next();
        }
        public override void addBuilding(_AGTDMonoBuildingFunction _function, Action _complete)
        {
            if (!_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            
            if (_function is not GTDMonoFarmingFunction farmingFunction || farmingFunction.parentTrans == null)
            {
                _complete?.Invoke();
                return;
            }
            
            FarmingBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getFarmingBuildingInfo(farmingFunction.buildingId);
            if (buildingInfo == null)
            {
                _complete?.Invoke();
                return;
            }
            
            FarmingBuildingView buildingView = new FarmingBuildingView(buildingInfo, farmingFunction.parentTrans.position);
            _m_buildingViewDict.Add(farmingFunction, buildingView);
            buildingView.load(_complete);
        }
        public override void removeBuilding(_AGTDMonoBuildingFunction _function)
        {
            if (!_m_isInit)
                return;
            
            if (_function is not GTDMonoFarmingFunction farmingFunction)
                return;

            if (!_m_buildingViewDict.TryGetValue(farmingFunction, out FarmingBuildingView buildingView))
                return;
            
            buildingView.discard();
            _m_buildingViewDict.Remove(farmingFunction);
        }


        private void _showEarningTip()
        {
            float deltaTime = MainAdditionBuildingTDScene.instance.sceneConfig.farmingBuildingEarningTipSpace;
            if (deltaTime <= 0)
                deltaTime = 1f;
            
            foreach (FarmingBuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.showEarningTip(deltaTime);
            }
        }
        private void _onFarmingBuildingChg(FarmingBuildingInfo _buildingInfo)
        {
            if (_buildingInfo == null)
                return;

            int serialize = _m_initSerialize;
            foreach ((GTDMonoFarmingFunction function, FarmingBuildingView buildingView) in _m_buildingViewDict)
            {
                if (function.buildingId != _buildingInfo.id)
                    continue;

                if (buildingView.level != _buildingInfo.level)
                {
                    int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                    _showBuildingUpgrade(function, buildingView, _buildingInfo, () =>
                    {
                        MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                        if (serialize != _m_initSerialize)
                            return;

                        ALCommonActionMonoTask.addMonoTask(() =>
                        {
                            GGUIWndFarmingBuildingUpgradeSuccess.instance.refreshWnd(_buildingInfo.baseRef, _buildingInfo.level);
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFarmingBuildingUpgradeSuccess.instance, GGUIWndFarmingBuildingUpgradeSuccess.instance.showWnd);
                        });
                    });
                }
                
                buildingView.refresh();
            }
        }

        private void _showBuildingUpgrade(GTDMonoFarmingFunction _function, FarmingBuildingView _buildingView, FarmingBuildingInfo _buildingInfo, Action _complete)
        {
            if (_function == null || _buildingView == null || _buildingInfo == null)
            {
                _complete?.Invoke();
                return;
            }
            
            // 判断是否要换资源
            if (BasicResIndexInfo.IsEqual(_buildingView.resIndex, _buildingInfo.getCurrentResIndex()))
            {
                _complete?.Invoke();
                return;                
            }
            
            int serialize = _m_initSerialize;
            CommonTDSfxObj sfxObj = PlaySfxMgr.instance.playSfxByPos(MainAdditionBuildingTDScene.instance.sceneConfig.buildingResChgSfxId, _buildingView.position);
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_initSerialize)
                {
                    sfxObj?.forceDiscard();
                    _complete?.Invoke();
                    return;
                }

                if (!_m_buildingViewDict.TryGetValue(_function, out FarmingBuildingView currentView) || currentView != _buildingView)
                {
                    sfxObj?.forceDiscard();
                    _complete?.Invoke();
                    return;
                }

                Vector3 position = _buildingView.position;
                _buildingView.discard();
                FarmingBuildingView newBuildingView = new FarmingBuildingView(_buildingInfo, position);
                _m_buildingViewDict[_function] = newBuildingView;
                newBuildingView.load(() =>
                {
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        sfxObj?.forceDiscard();
                        _complete?.Invoke();
                    }, MainAdditionBuildingTDScene.instance.sceneConfig.buildingResChgSfxDeleteDelay);
                });
            }, MainAdditionBuildingTDScene.instance.sceneConfig.buildingResChgSfxDeleteDelay);
        }
        private void _refreshUpgradeState()
        {
            foreach (FarmingBuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.refreshUpgradeState();
            }
        }
        private void _moveFocusToFarmingBuildingUpgradeBtnAndShowGuideHand(object[] _params)
        {
            if (_params is not { Length: > 0 } || _params[0] is not long buildingId)
                return;

            foreach (FarmingBuildingView buildingView in _m_buildingViewDict.Values)
            {
                if (buildingView.id == buildingId)
                {
                    buildingView.moveFocusToFarmingBuildingUpgradeBtnAndShowGuideHand();
                    return;
                }       
            }
        }
        private void _simulateClickCollection(object[] _params)
        {
            if (_params is not { Length: > 0 } || _params[0] is not long buildingId)
                return;

            foreach (FarmingBuildingView buildingView in _m_buildingViewDict.Values)
            {
                if (buildingView.id == buildingId)
                {
                    buildingView._onEarningsClick();
                    return;
                }
            }
        }
        private void _onBagItemChg(object[] _params)
        {
            _refreshUpgradeState();
        }
        private void _onBuildingLevelChg(object[] _params)
        {
            _refreshUpgradeState();
        }
    }
}