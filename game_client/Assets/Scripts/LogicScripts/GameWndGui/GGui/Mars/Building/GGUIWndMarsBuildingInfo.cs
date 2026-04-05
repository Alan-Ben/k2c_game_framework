using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingInfo : _ATALBasicUIWnd<GGUIMonoMarsBuildingInfo>
    {
        [NotNull] public static GGUIWndMarsBuildingInfo instance { get { return _g_instance ??= new GGUIWndMarsBuildingInfo(); } }
        private static GGUIWndMarsBuildingInfo _g_instance;

        
        private GGUISubWndMarsBuildingInfoPageTab _m_pageTabSubWnd;
        private _IMarsBuildingView _m_commonNormalBuildingView;
        private GGUIMonoMarsBuildingInfoPageTabType _m_defaultType;
        private new bool _m_bIsShow;
        [NotNull] private readonly _ProgressRefreshHandle _m_progressRefreshHandle;
        

        public GGUIWndMarsBuildingInfo() 
            : base(EALUIWndLayer.ADDITION)
        {
            _m_progressRefreshHandle = new _ProgressRefreshHandle(this);
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_bIsShow = true;

            _m_pageTabSubWnd?.showWnd();

            refreshWnd();

            _trySelectBuildingView();

            NPPlayer.instance.marsComp.buildingSubComponent.onEquipmentUpgraded += _onEquipmentUpgraded;

            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_UPGRADE_WND_BTN, _onSimulateClickUpgrade);
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.onEquipmentUpgraded -= _onEquipmentUpgraded;

            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_UPGRADE_WND_BTN, _onSimulateClickUpgrade);

            _tryUnselectBuildingView();

            _m_pageTabSubWnd?.hideWnd();

            _m_progressRefreshHandle.clear();

            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_pageTabSubWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_pageTabSubWnd?.discard();
            _m_pageTabSubWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnEnergyOutputDetail, _onClickBtnEnergyOutputDetail);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoPageTab != null)
                _m_pageTabSubWnd = new GGUISubWndMarsBuildingInfoPageTab(wnd.monoPageTab);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
            ALUGUICommon.combineBtnClick(wnd.btnEnergyOutputDetail, _onClickBtnEnergyOutputDetail);
        }

        
        public void refreshWnd(_IMarsBuildingView _buildingView, GGUIMonoMarsBuildingInfoPageTabType _defaultType = GGUIMonoMarsBuildingInfoPageTabType.Equipment)
        {
            _tryUnselectBuildingView();
            _m_commonNormalBuildingView = _buildingView;
            _m_defaultType = _defaultType;
            _trySelectBuildingView();
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_commonNormalBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonNormalBuildingView.buildingInfo;
            ALUGUICommon.setLabelTxt(wnd.txtName, buildingInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, buildingInfo.level));
            _m_pageTabSubWnd?.refreshWnd(buildingInfo);
            _m_pageTabSubWnd?.setSelectTab(_m_defaultType);
            wnd.setSettleState(buildingInfo.settleSlotData.isValid());
            wnd.setBuildingLevelMaxState(buildingInfo.levelData.isLevelMax);
            int equipmentLevelStart = buildingInfo.equipmentData.totalMainLevelStart;
            int equipmentLevelEnd = buildingInfo.equipmentData.totalMainLevelEnd;
            int equipmentLevel = buildingInfo.equipmentData.totalMainLevel;
            _m_progressRefreshHandle.reset(equipmentLevelStart, equipmentLevelEnd, equipmentLevel);

            _refreshBtnShow();
        }

        private void _refreshBtnShow()
        {
            if(wnd == null || _m_commonNormalBuildingView == null || _m_commonNormalBuildingView.buildingInfo == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.btnEnergyOutputDetail, _m_commonNormalBuildingView.buildingInfo.energyProperty.value > 0);
        }

        private void _onEquipmentUpgraded(long _buildingId, long _equipmentId)
        {
            if (_m_commonNormalBuildingView?.buildingInfo == null || _m_commonNormalBuildingView.buildingInfo.refObj.id != _buildingId)
                return;
            
            _m_progressRefreshHandle.createFlyShow(_equipmentId);
        }
        private void _refreshEquipmentProgress(int _equipmentLevelStart, int _equipmentLevelEnd, int _equipmentLevel)
        {
            if (wnd == null)
                return;
            
            int equipmentLevelStart = _equipmentLevelStart;
            int equipmentLevelEnd = _equipmentLevelEnd;
            int equipmentLevel = _equipmentLevel;
            int equipmentProgressPercentage = 0;
            if (equipmentLevelEnd > equipmentLevelStart)
                equipmentProgressPercentage = (equipmentLevel - equipmentLevelStart) * 100 / (equipmentLevelEnd - equipmentLevelStart);
            ALUGUICommon.setLabelTxt(wnd.txtEquipmentProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, equipmentProgressPercentage));
            if (wnd.sldEquipmentProgress != null)
            {
                wnd.sldEquipmentProgress.minValue = equipmentLevelStart;
                wnd.sldEquipmentProgress.maxValue = equipmentLevelEnd;
                wnd.sldEquipmentProgress.value = equipmentLevel;
            }
            wnd.setEquipmentCompleteState(equipmentLevel >= equipmentLevelEnd);
        }
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_INFO);
        }
        private void _onBtnUpgradeClick(GameObject _obj)
        {
            if (_m_commonNormalBuildingView == null)
                return;
              
            MarsBuildingInfo buildingInfo = _m_commonNormalBuildingView.buildingInfo;
            int equipmentLevelEnd = buildingInfo.equipmentData.totalMainLevelEnd;
            int equipmentLevel = buildingInfo.equipmentData.totalMainLevel;
            if (equipmentLevel < equipmentLevelEnd)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_building_equipment_not_full_level);
                return;
            }
            
            _onBtnCloseClick(null);
            GGUIWndMarsBuildingUpgrade.instance.refreshWnd(_m_commonNormalBuildingView);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingUpgrade.instance, GGUIWndMarsBuildingUpgrade.instance.showWnd,
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_UPGRADE, false, false);
        }
        private void _onClickBtnEnergyOutputDetail(GameObject _obj)
        {
            if (_m_commonNormalBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonNormalBuildingView.buildingInfo;
            if(buildingInfo == null || buildingInfo.energyProperty.value <= 0)
                return;
            
            GGUIWndMarsBuildingEnergyDetail.instance.setData(_m_commonNormalBuildingView);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingEnergyDetail.instance, GGUIWndMarsBuildingEnergyDetail.instance.showWnd,
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_ENERGY_DETAIL);
        }
        private void _trySelectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_commonNormalBuildingView == null)
                return;

            _m_commonNormalBuildingView.setSelected(true);
            MainAdditionMarsTDScene.instance.focusToPos(_m_commonNormalBuildingView.position, wnd.focusViewportPos, wnd.focusScale, wnd.focusTime);
        }
        private void _tryUnselectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_commonNormalBuildingView == null)
                return;

            _m_commonNormalBuildingView?.setSelected(false);
            MainAdditionMarsTDScene.instance.cancelThePosFocus(wnd.focusTime);
        }
        private void _onSimulateClickUpgrade()
        {
            if (wnd == null) return;
            _onBtnUpgradeClick(wnd.btnUpgrade);
        }


        public class _ProgressRefreshHandle 
        {
            [NotNull] private readonly GGUIWndMarsBuildingInfo _m_wnd;
            private int _m_resetSerialize;
            [ItemNotNull, NotNull] private readonly List<MoveObject> _m_effectObjList;
            private const int _k_maxEffectObjCount = 10;

            private int _m_progressStart;
            private int _m_progressEnd;
            private int _m_currentProgress;
            
            
            public _ProgressRefreshHandle([NotNull] GGUIWndMarsBuildingInfo _wnd)
            {
                _m_wnd = _wnd;
                _m_effectObjList = new List<MoveObject>();
            }


            public void reset(int _progressStart, int _progressEnd, int _currentProgress)
            {
                clear();
                _m_progressStart = _progressStart;
                _m_progressEnd = _progressEnd;
                _m_currentProgress = _currentProgress;
                _m_wnd._refreshEquipmentProgress(_progressStart, _progressEnd, _currentProgress);
            }
            public void clear()
            {
                _m_resetSerialize = ALSerializeOpMgr.next();
                foreach (MoveObject effectObj in _m_effectObjList)
                {
                    effectObj.cancelMove();
                    GGoIndexCacheMgr.instance.pushbackItem(GRefdataCoreMgr.instance.npGeneral.mars_equipment_upgrade_effect_go, effectObj.gameObject);
                }
                _m_effectObjList.Clear();
            }
            public void createFlyShow(long _equipmentId)
            {
                if (_m_wnd.wnd == null)
                    return;
                
                MarsBuildingEquipmentInfo equipmentInfo = _m_wnd._m_commonNormalBuildingView?.buildingInfo?.equipmentData.getEquipmentById(_equipmentId);
                // 如果不是主要部件不用刷新外部的进度
                if (equipmentInfo is not { isMain: true })
                    return;
                
                GGUIPrefabSubWndMarsBuildingInfoPageEquipment equipmentPage = _m_wnd._m_pageTabSubWnd?.getPageWnd(GGUIMonoMarsBuildingInfoPageTabType.Equipment) as GGUIPrefabSubWndMarsBuildingInfoPageEquipment;
                GGUISubWndMarsBuildingInfoEquipmentContainerItem equipmentItem = equipmentPage?.equipmentContainer?.getItem(_item => _item.equipmentId == _equipmentId);
                if (equipmentItem?.wnd == null || _m_wnd.wnd.upgradeEffectParent == null || _m_wnd.wnd.upgradeEffectEndPos == null)
                {
                    _addProgress();
                    return;
                }
                
                int serialize = _m_resetSerialize;
                Vector3 startPos = equipmentItem.wnd.transform.position;
                Vector3 endPos = _m_wnd.wnd.upgradeEffectEndPos.position;                
                GGoIndexCacheMgr.instance.popItem(GRefdataCoreMgr.instance.npGeneral.mars_equipment_upgrade_effect_go, _go =>
                {
                    if (serialize != _m_resetSerialize)
                    {
                        GGoIndexCacheMgr.instance.pushbackItem(GRefdataCoreMgr.instance.npGeneral.mars_equipment_upgrade_effect_go, _go);
                        return;
                    }

                    MoveObject effectObj = _go.GetComponent<MoveObject>();
                    if (effectObj == null)
                    {
                        ALLog.Warning("_showUpgradeEffect: MoveObject component missing on effect GO.");
                        GGoIndexCacheMgr.instance.pushbackItem(GRefdataCoreMgr.instance.npGeneral.mars_equipment_upgrade_effect_go, _go);
                        _addProgress();
                        return;
                    }

                    _m_effectObjList.Add(effectObj);
                    effectObj.transform.SetParent(_m_wnd.wnd.upgradeEffectParent);
                    effectObj.transform.position = startPos;
                    effectObj.startMove(endPos, () =>
                    {
                        GGoIndexCacheMgr.instance.pushbackItem(GRefdataCoreMgr.instance.npGeneral.mars_equipment_upgrade_effect_go, effectObj.gameObject);
                        _m_effectObjList.Remove(effectObj);
                        if (_m_wnd.wnd.upgradeEffectEndPos != null)
                            PlaySfxMgr.instance.playUISfx(_m_wnd.wnd.upgradeEffectEndSfxId, _m_wnd.wnd.upgradeEffectEndPos);
                        _addProgress();
                    });

                    // Clean up if too many effect objects
                    if (_m_effectObjList.Count > _k_maxEffectObjCount)
                    {
                        MoveObject oldestObj = _m_effectObjList[0];
                        oldestObj.cancelMove();
                        GGoIndexCacheMgr.instance.pushbackItem(GRefdataCoreMgr.instance.npGeneral.mars_equipment_upgrade_effect_go, oldestObj.gameObject);
                        _m_effectObjList.RemoveAt(0);
                        _addProgress();
                    }
                });
            }
            

            private void _addProgress()
            {
                if (_m_wnd.wnd == null)
                    return;
                
                int serialize = _m_resetSerialize;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (serialize != _m_resetSerialize)
                        return;
                    
                    if (_m_wnd.wnd.equipmentProgressSfxParent != null)
                        PlaySfxMgr.instance.playUISfx(_m_wnd.wnd.equipmentProgressSfxId, _m_wnd.wnd.equipmentProgressSfxParent);
                    _m_currentProgress++;
                    _m_wnd._refreshEquipmentProgress(_m_progressStart, _m_progressEnd, _m_currentProgress);
                }, _m_wnd.wnd.equipmentProgressRefreshDelay);
            }
        }
    }
}