using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public abstract class GGUIWndMarsBuildingUpgrade<T> : _ATALBasicUIWnd<T>
        where T : GGUIMonoMarsBuildingUpgrade
    {
        private GGUIWndConditionDescContainer _m_conditionDescContainer;
        private NPGGUIWndCommonItem _m_completeNowCostItemWnd;
        private GGUIWndMarsPropertyShowItemContainer _m_wPropertyShowContainer;
        
        private _IMarsBuildingView _m_commonNormalBuildingView;
        private List<MasrPropertyShowInfo> _m_lPropertyShowList;//属性显示列表

        private CommonItemData _m_completeNowCostItemData;
        private ALCommonEnableTaskController _m_tickTask;
        private int _m_showSerialize;
        
        private new bool _m_bIsShow;
        

        public GGUIWndMarsBuildingUpgrade() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected virtual bool needFocusBuildingPos { get { return true; } }

        protected override void _onShowWnd()
        {
            NPPlayer.instance.rescourceComp.onResourceCountChg += _onPlayerResChg;
            _m_bIsShow = true;

            _m_conditionDescContainer?.showWnd();
            _m_completeNowCostItemWnd?.showWnd();
            _m_wPropertyShowContainer?.showWnd();

            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);

            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_UPGRADE, _onSimulateClickUpgrade);

            refreshWnd();

            _trySelectBuildingView();
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.rescourceComp.onResourceCountChg -= _onPlayerResChg;
            _tryUnselectBuildingView();

            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_UPGRADE, _onSimulateClickUpgrade);

            _m_conditionDescContainer?.hideWnd();
            _m_completeNowCostItemWnd?.hideWnd();
            _m_wPropertyShowContainer?.hideWnd();

            _m_lPropertyShowList?.Clear();

            _m_tickTask.setDisable();
            _m_showSerialize = ALSerializeOpMgr.next();

            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_conditionDescContainer?.resetWnd();
            _m_completeNowCostItemWnd?.resetWnd();
            _m_wPropertyShowContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_conditionDescContainer?.discard();
            _m_conditionDescContainer = null;
            _m_completeNowCostItemWnd?.discard();
            _m_completeNowCostItemWnd = null;
            _m_wPropertyShowContainer?.discard();
            _m_wPropertyShowContainer = null;
            
            _m_lPropertyShowList?.Clear();
            _m_lPropertyShowList = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnBack, _onBtnBackClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnBuffExplain, _onBtnBuffExplainClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoConditionContainer != null)
                _m_conditionDescContainer = new GGUIWndConditionDescContainer(wnd.monoConditionContainer);
            if (wnd.monoCompleteNowCostItem != null)
                _m_completeNowCostItemWnd = new NPGGUIWndCommonItem(wnd.monoCompleteNowCostItem);
            if (wnd.monoMarsPropertyShowContainer != null)
                _m_wPropertyShowContainer = new GGUIWndMarsPropertyShowItemContainer(wnd.monoMarsPropertyShowContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnBack, _onBtnBackClick);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
            ALUGUICommon.combineBtnClick(wnd.btnBuffExplain, _onBtnBuffExplainClick);
            ALUGUICommon.combineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _tryUnselectBuildingView();
            _m_commonNormalBuildingView = _buildingView;
            _trySelectBuildingView();
            
            _updatePropertyShowList();
            
            refreshWnd();
        }
        
        private void _updatePropertyShowList()
        {
            if (_m_lPropertyShowList == null)
                _m_lPropertyShowList = new List<MasrPropertyShowInfo>();
            _m_lPropertyShowList.Clear();
            if (_m_commonNormalBuildingView == null || _m_commonNormalBuildingView.buildingInfo == null)
                return;
                
            MarsBuildingInfoLevelData levelData = _m_commonNormalBuildingView.buildingInfo.levelData;
            if(levelData.refObj == null)
                return;
            
            List<_IPropertyShow> allPropertyList = new List<_IPropertyShow>();
            
            Dictionary<_IPropertyShow, long> nowLevelPropertyValueDict = new Dictionary<_IPropertyShow, long>();
            levelData.refObj.addPropertyToCollections(nowLevelPropertyValueDict, allPropertyList);
            if (levelData.nextRefObj == null)//最高等级时
            {
                foreach (var propertyShow in allPropertyList)
                {
                    if(propertyShow == null)
                        continue;
                    
                    nowLevelPropertyValueDict.TryGetValue(propertyShow, out long nowValue);
                    _m_lPropertyShowList.Add(new MasrPropertyShowInfo()
                    {
                        propertyShow = propertyShow,
                        nowValue = nowValue,
                        nextValue = nowValue,
                    });
                }
                
                return;
            }
            
            Dictionary<_IPropertyShow, long> nextLevelPropertyValueDict = new Dictionary<_IPropertyShow, long>();
            levelData.nextRefObj.addPropertyToCollections(nextLevelPropertyValueDict, allPropertyList);
            foreach (var propertyShow in allPropertyList)
            {
                if(propertyShow == null)
                    continue;
                    
                nowLevelPropertyValueDict.TryGetValue(propertyShow, out long nowValue);
                nextLevelPropertyValueDict.TryGetValue(propertyShow, out long nextValue);
                _m_lPropertyShowList.Add(new MasrPropertyShowInfo()
                {
                    propertyShow = propertyShow,
                    nowValue = nowValue,
                    nextValue = nextValue,
                });
            }
        }
        
        public void refreshWnd() 
        {
            if (wnd == null || !_m_bIsShow || _m_commonNormalBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonNormalBuildingView.buildingInfo;
                
            ALUGUICommon.setLabelTxt(wnd.txtName, buildingInfo.nameTranslated);
            
            if(string.IsNullOrEmpty(wnd.txtLevelKey))
                ALUGUICommon.setLabelTxt(wnd.txtLevel, buildingInfo.level);
            else
                ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(wnd.txtLevelKey, buildingInfo.level));
            
            long upgradeTime = buildingInfo.levelData.realUpgradeTimeMS();
            ALUGUICommon.setLabelTxt(wnd.txtUpgradeTime, TimeUtil.millisecondsToTime_DayHourOrHMS(upgradeTime));
            long originTime = buildingInfo.levelData.refObj.upgrade_time_cost_sec * 1000;
            ALUGUICommon.setLabelTxt(wnd.txtOriginTime, TimeUtil.millisecondsToTime_DayHourOrHMS(originTime));
            wnd.setUpgradeState(buildingInfo.levelData.checkCanUpgrade());
            wnd.setLevelMaxState(buildingInfo.levelData.isLevelMax);
            _refreshCompleteNowCost();

            if (_m_wPropertyShowContainer != null)
            {
                _m_wPropertyShowContainer.showWnd();
                _m_wPropertyShowContainer.setData(_m_lPropertyShowList);
            }

            _refreshConditionDescContainer();
        }

        private void _refreshConditionDescContainer()
        {
            if (wnd == null || !_m_bIsShow || _m_commonNormalBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonNormalBuildingView.buildingInfo;
            if (buildingInfo == null || buildingInfo.levelData.refObj == null )
                return;

            _m_conditionDescContainer?.setShowData(MarsUtil.generateConditionShowList(buildingInfo.levelData.refObj.upgrade_condition_id_list, buildingInfo.levelData.getUpgradeCostList()));
        }
        

        protected virtual void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_UPGRADE);
        }
        protected virtual void _onBtnBackClick(GameObject _obj)
        {
            if (_m_commonNormalBuildingView == null)
                return;
                
            _onBtnCloseClick(null);
            GGUIWndMarsBuildingInfo.instance.refreshWnd(_m_commonNormalBuildingView);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingInfo.instance, GGUIWndMarsBuildingInfo.instance.showWnd,
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_INFO, false, false);
        }
        private void _onBtnUpgradeClick(GameObject _obj)
        {
            if (_m_commonNormalBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonNormalBuildingView.buildingInfo;
            if (!buildingInfo.checkCanUpgrade())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_buildingCantUpgradeTip_none);
                return;
            }
            
            if (!NPPlayer.instance.marsComp.buildingSubComponent.hasLeisureQueue)
            {
                MarsUtil.showNoLeisureQueue();
                return;
            }

            int serialize = _m_showSerialize;
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.marsComp.buildingSubComponent.startUpgrade(buildingInfo.refObj.id, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                if (serialize != _m_showSerialize)
                    return;

                long upgradeAudioId = GRefdataCoreMgr.instance.npGeneral.mars_building_upgrade_audio_id;
                PlayAudioMgr.instance.playClip(upgradeAudioId);
                _onBtnCloseClick(null);
            });
        }
        private void _onSimulateClickUpgrade()
        {
            _onBtnUpgradeClick(null);
        }
        private void _onBtnBuffExplainClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingTimeBuffExplain.instance, GGUIWndMarsBuildingTimeBuffExplain.instance.showWnd, UINodeTagConst_Mars.C_MARS_BUILDING_TIME_BUFF_EXPLAIN);
        }
        private void _onBtnCompleteNowClick(GameObject _obj)
        {
            if (_m_commonNormalBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonNormalBuildingView.buildingInfo;
            if (!buildingInfo.checkCanUpgrade())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_buildingCantUpgradeTip_none);
                return;
            }

            bool needShowWarningTip = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_COMPLETE_NOW_CONFIRM);
            if (needShowWarningTip)
            {
                GGUIWndMarsTimeCompleteNowConfim.addNode(buildingInfo, () =>
                {
                	_onBtnCloseClick(null);
                });
            }
            else
            {
            	int serialize = _m_showSerialize;
                buildingInfo.reqCompleteNow((_isSucc)=>
                {
                	if (serialize != _m_showSerialize || !_isSucc)
                        return;
                        
                	_onBtnCloseClick(null);
                });
            }
        }


        private void _refreshCompleteNowCost()
        {
            if (_m_commonNormalBuildingView == null || _m_completeNowCostItemWnd == null)
                return;
                
            MarsBuildingInfo buildingInfo = _m_commonNormalBuildingView.buildingInfo;
            if(buildingInfo != null)
                _m_completeNowCostItemWnd.setItem(buildingInfo.completeNowCostItem);
        }
        private void _tick()
        {
            _refreshCompleteNowCost();
        }
        bool _m_isFocusedBuildingPos = false;
        private void _trySelectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_commonNormalBuildingView == null)
                return;
            
            _m_commonNormalBuildingView.setSelected(true);
            if (needFocusBuildingPos)
            {
                _m_isFocusedBuildingPos = true;
                MainAdditionMarsTDScene.instance.focusToPos(_m_commonNormalBuildingView.position, wnd.focusViewportPos, wnd.focusScale, wnd.focusTime);
            }
        }
        private void _tryUnselectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_commonNormalBuildingView == null)
                return;
            
            _m_commonNormalBuildingView?.setSelected(false);
            if (_m_isFocusedBuildingPos)
            {
                _m_isFocusedBuildingPos = false;
                MainAdditionMarsTDScene.instance.cancelThePosFocus(wnd.focusTime);
            }
        }

        private void _onPlayerResChg(ECurrency _currencyType, long _oldCount, long _newCount)
        {
            if (_currencyType == ECurrency.MARS_ENERGY)
                _refreshConditionDescContainer();
        }

        /// <summary>
        /// 在处理升级请求时调用的处理函数，可以在子类强制处理一些其他操作
        /// </summary>
        protected virtual void _onDealUpgrade()
        {

        }
    }
    public class GGUIWndMarsBuildingUpgrade : GGUIWndMarsBuildingUpgrade<GGUIMonoMarsBuildingUpgrade>
    {
        [NotNull] public static GGUIWndMarsBuildingUpgrade instance { get { return _g_instance ??= new GGUIWndMarsBuildingUpgrade(); } }
        private static GGUIWndMarsBuildingUpgrade _g_instance;
        
        
        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingUpgrade.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingUpgrade.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}