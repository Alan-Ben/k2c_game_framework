using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class BusinessBuildingViewModule : _AFunctionBuildingViewMgrSubModule
    {
        [NotNull] private readonly Dictionary<GTDMonoBusinessFunction, BusinessBuildingView> _m_buildingViewDict;
        private bool _m_isInit;
        private int _m_initSerialize;
        private ALCommonEnableTaskController _m_earningDurationTask;
        
        public BusinessBuildingViewModule()
        {
            _m_buildingViewDict = new Dictionary<GTDMonoBusinessFunction, BusinessBuildingView>();
        }


        public override void init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            _m_isInit = true;
            
            NPPlayer.instance.buildingComp.onBusinessBuildingChg += _onBusinessBuildingChg;
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_GAIN, _onHeroGain);
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _refreshCanSettleHero);

            float time = MainAdditionBuildingTDScene.instance.sceneConfig.businessBuildingEarningTipSpace;
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
            
            NPPlayer.instance.buildingComp.onBusinessBuildingChg -= _onBusinessBuildingChg;
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_GAIN, _onHeroGain);
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _refreshCanSettleHero);

            foreach (BusinessBuildingView buildingView in _m_buildingViewDict.Values)
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
            
            if (_function is not GTDMonoBusinessFunction businessFunction || businessFunction.parentTrans == null)
            {
                _complete?.Invoke();
                return;
            }
            
            BusinessBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(businessFunction.buildingId);
            if (buildingInfo == null)
            {
                _complete?.Invoke();
                return;
            }
            
            BusinessBuildingView buildingView = new BusinessBuildingView(buildingInfo, businessFunction.parentTrans.position);
            _m_buildingViewDict.Add(businessFunction, buildingView);
            buildingView.load(_complete);
        }
        public override void removeBuilding(_AGTDMonoBuildingFunction _function)
        {
            if (!_m_isInit)
                return;
            
            if (_function is not GTDMonoBusinessFunction businessFunction)
                return;

            if (!_m_buildingViewDict.TryGetValue(businessFunction, out BusinessBuildingView buildingView))
                return;
            
            buildingView.discard();
            _m_buildingViewDict.Remove(businessFunction);
        }


        private void _showEarningTip()
        {
            float deltaTime = MainAdditionBuildingTDScene.instance.sceneConfig.businessBuildingEarningTipSpace;
            if (deltaTime <= 0)
                deltaTime = 1f;
            
            foreach (BusinessBuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.showEarningTip(deltaTime);
            }
        }
        private void _onBusinessBuildingChg(BusinessBuildingInfo _buildingInfo)
        {
            if (_buildingInfo == null)
                return;

            List<NPUINoticeMgr._ANPUINoticeDealer> upgradeDealerList = new List<NPUINoticeMgr._ANPUINoticeDealer>();
            foreach ((GTDMonoBusinessFunction function, BusinessBuildingView buildingView) in _m_buildingViewDict)
            {
                if (function.buildingId != _buildingInfo.id)
                    continue;

                if (buildingView.level != _buildingInfo.level)
                {
                    upgradeDealerList.Add(new BuildingUpgradeDealer(this, function, _buildingInfo));
                    // upgradeDealerList.Add(new NoticeDealer_BusinessBuildingUpgradeSuccess(_buildingInfo.baseRef, _buildingInfo.level));
                }
                
                buildingView.refresh();
            }

            foreach (NPUINoticeMgr._ANPUINoticeDealer dealer in upgradeDealerList)
            {
                NPUINoticeMgr.instance.addDealer(dealer);
            }
        }
        private void _onHeroGain(object[] _params)
        {
            _refreshCanSettleHero();
        }
        private void _refreshCanSettleHero()
        {
            foreach ((GTDMonoBusinessFunction function, BusinessBuildingView buildingView) in _m_buildingViewDict)
            {
                buildingView?._refreshCanSettleHero();
            }
        }
        
        
        private class BuildingUpgradeDealer : NPUINoticeMgr._ANPUINoticeDealer
        {
            [NotNull] private readonly BusinessBuildingViewModule _m_viewMgr;
            private readonly int _m_serialize;
            [NotNull] private readonly GTDMonoBusinessFunction _m_functionMono;
            [NotNull] private readonly BusinessBuildingInfo _m_newBuildingInfo;

            private CommonTDSfxObj _m_sfxObj;
            
            
            public BuildingUpgradeDealer([NotNull] BusinessBuildingViewModule _viewMgr, [NotNull] GTDMonoBusinessFunction _functionMono, [NotNull] BusinessBuildingInfo _newBuildingInfo)
            {
                _m_viewMgr = _viewMgr;
                _m_serialize = _m_viewMgr._m_initSerialize;
                _m_functionMono = _functionMono;
                _m_newBuildingInfo = _newBuildingInfo;
            }
            
            
            public override bool canCurShow { get { return true; } }
            public override bool canPlayPriority { get { return false; } }
            public override bool isPriorityDealer { get { return false; } }
            public override bool needTransBk { get { return false; } }
            public override bool isNoticeFullScreen { get { return false; } }
            public override bool isOnlyUINode { get { return false; } }
            public override bool noticeCanDoESC { get { return false; } }

            protected override void _onDealerDone()
            {
                if (_m_serialize != _m_viewMgr._m_initSerialize)
                    return;
                if (!_m_viewMgr._m_buildingViewDict.TryGetValue(_m_functionMono, out BusinessBuildingView buildingView))
                    return;
                if (BasicResIndexInfo.IsEqual(buildingView.resIndex, _m_newBuildingInfo.getCurrentResIndex()))
                    return;
                
                buildingView.discard();
                BusinessBuildingView newBuildingView = new BusinessBuildingView(_m_newBuildingInfo, buildingView.position);
                _m_viewMgr._m_buildingViewDict[_m_functionMono] = newBuildingView;
                newBuildingView.load();
            }
            public override void dealShowNotice()
            {
                if (_m_serialize != _m_viewMgr._m_initSerialize)
                {
                    setDealerDone();
                    return;
                }
                if (!_m_viewMgr._m_buildingViewDict.TryGetValue(_m_functionMono, out BusinessBuildingView buildingView))
                {
                    setDealerDone();
                    return;
                }
                
                // 判断是否要换资源
                if (!BasicResIndexInfo.IsEqual(buildingView.resIndex, _m_newBuildingInfo.getCurrentResIndex()))
                {
                    _m_sfxObj = PlaySfxMgr.instance.playSfxByPos(MainAdditionBuildingTDScene.instance.sceneConfig.buildingResChgSfxId, buildingView.position);
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        if (_m_serialize != _m_viewMgr._m_initSerialize)
                        {
                            setDealerDone();
                            return;
                        }
                        
                        if (!_m_viewMgr._m_buildingViewDict.TryGetValue(_m_functionMono, out BusinessBuildingView currentView) || currentView != buildingView)
                        {
                            setDealerDone();
                            return;
                        }

                        Vector3 position = buildingView.position;
                        buildingView.discard();
                        BusinessBuildingView newBuildingView = new BusinessBuildingView(_m_newBuildingInfo, position);
                        _m_viewMgr._m_buildingViewDict[_m_functionMono] = newBuildingView;
                        newBuildingView.load(() =>
                        {
                            ALCommonActionMonoTask.addMonoTask(() =>
                            {
                                if (_m_serialize != _m_viewMgr._m_initSerialize)
                                {
                                    setDealerDone();
                                    return;
                                }
                                
                                _m_sfxObj?.forceDiscard();
                                _m_sfxObj = null;

                                setDealerDone();

                            }, MainAdditionBuildingTDScene.instance.sceneConfig.buildingResChgSfxDeleteDelay);
                        });
                    }, MainAdditionBuildingTDScene.instance.sceneConfig.buildingResChgSfxDeleteDelay);
                }
                else
                    setDealerDone();
            }
            public override void dealHideNotice()
            {
                _m_sfxObj?.forceDiscard();
                _m_sfxObj = null;
            }
        }
    }
}