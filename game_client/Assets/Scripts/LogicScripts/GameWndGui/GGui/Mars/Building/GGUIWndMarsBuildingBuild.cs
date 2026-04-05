using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingBuild : _ATALBasicUIWnd<GGUIMonoMarsBuildingBuilld>
    {
        [NotNull] public static GGUIWndMarsBuildingBuild instance { get { return _g_instance ??= new GGUIWndMarsBuildingBuild(); } }
        private static GGUIWndMarsBuildingBuild _g_instance;
        

        private GGUIWndConditionDescContainer _m_conditionDescContainer;
        private NPGGUIWndCommonItem _m_completeNowCostItemWnd;
        
        private _IMarsBuildingView _m_buildingView;
        private CommonItemData _m_completeNowCostItemData;
        
        private ALCommonEnableTaskController _m_tickTask;
        private int _m_showSerialize;

        private new bool _m_bIsShow;
        

        public GGUIWndMarsBuildingBuild() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingBuilld.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingBuilld.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_bIsShow = true;

            _m_conditionDescContainer?.showWnd();
            _m_completeNowCostItemWnd?.showWnd();

            NPPlayer.instance.rescourceComp.onResourceCountChg += _onPlayerResChg;
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_BUILD, _onSimulateClickBuild);

            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);

            refreshWnd();

            _trySelectBuildingView();
        }
        protected override void _onHideWnd()
        {
            _tryUnselectBuildingView();

            _m_conditionDescContainer?.hideWnd();
            _m_completeNowCostItemWnd?.hideWnd();

            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_BUILD, _onSimulateClickBuild);
            NPPlayer.instance.rescourceComp.onResourceCountChg -= _onPlayerResChg;

            _m_tickTask.setDisable();
            _m_showSerialize = ALSerializeOpMgr.next();

            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_conditionDescContainer?.resetWnd();
            _m_completeNowCostItemWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_conditionDescContainer?.discard();
            _m_conditionDescContainer = null;
            _m_completeNowCostItemWnd?.discard();
            _m_completeNowCostItemWnd = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnBuild, _onBtnBuildClick);
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

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnBuild, _onBtnBuildClick);
            ALUGUICommon.combineBtnClick(wnd.btnBuffExplain, _onBtnBuffExplainClick);
            ALUGUICommon.combineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _tryUnselectBuildingView();
            _m_buildingView = _buildingView;
            _trySelectBuildingView();
            refreshWnd();
        }
        public void refreshWnd() 
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
                
            ALUGUICommon.setLabelTxt(wnd.txtName, buildingInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, buildingInfo.descTranslated);
            long buildTime = buildingInfo.realBuildTimeMS();
            ALUGUICommon.setLabelTxt(wnd.txtBuildTime, TimeUtil.millisecondsToTime_DayHourOrHMS(buildTime));
            long originTime = buildingInfo.refObj.build_time_cost_sec * 1000;
            ALUGUICommon.setLabelTxt(wnd.txtOriginTime, TimeUtil.millisecondsToTime_DayHourOrHMS(originTime));
            wnd.setBuildState(buildingInfo.checkCanBuild());
            _refreshCompleteNowCost();
            _refreshConditionDescContainer();
        }
        
        
        private void _refreshConditionDescContainer()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if (buildingInfo == null || buildingInfo.levelData.refObj == null )
                return;

            _m_conditionDescContainer?.setShowData(MarsUtil.generateConditionShowList(buildingInfo.refObj.build_condition_id_list, buildingInfo.refObj.build_cost_list));
        }
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_BUILD);
        }
        private void _onBtnBuildClick(GameObject _obj)
        {
            if (_m_buildingView == null)
                return;
            
            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if (!buildingInfo.checkCanBuild())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_buildingCantBuildTip_none);
                return;
            }

            if (!NPPlayer.instance.marsComp.buildingSubComponent.hasLeisureQueue)
            {
                MarsUtil.showNoLeisureQueue();
                return;
            }
            
            int serialize = _m_showSerialize;
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.marsComp.buildingSubComponent.startBuild(buildingInfo.refObj.id, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                if (serialize != _m_showSerialize)
                    return;
                
                long upgradeAudioId = GRefdataCoreMgr.instance.npGeneral.mars_building_build_audio_id;
                PlayAudioMgr.instance.playClip(upgradeAudioId);
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_BUILD);
            });
        }
        private void _onBtnBuffExplainClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingTimeBuffExplain.instance, GGUIWndMarsBuildingTimeBuffExplain.instance.showWnd, UINodeTagConst_Mars.C_MARS_BUILDING_TIME_BUFF_EXPLAIN);
        }
        private void _onBtnCompleteNowClick(GameObject _obj)
        {
            if (_m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if (!buildingInfo.checkCanBuild())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_buildingCantBuildTip_none);
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
            if (_m_buildingView == null || _m_completeNowCostItemWnd == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if(buildingInfo == null)
                return;

            _m_completeNowCostItemWnd.setItem(buildingInfo.completeNowCostItem);
        }
        private void _onSimulateClickBuild()
        {
            if (wnd == null) return;
            _onBtnBuildClick(wnd.btnBuild);
        }
        private void _tick()
        {
            _refreshCompleteNowCost();
        }
        private void _trySelectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;
            
            _m_buildingView.setSelected(true);
            MainAdditionMarsTDScene.instance.focusToPos(_m_buildingView.position, wnd.focusViewportPos, wnd.focusScale, wnd.focusTime);
        }
        private void _tryUnselectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;
            
            _m_buildingView?.setSelected(false);
            MainAdditionMarsTDScene.instance.cancelThePosFocus(wnd.focusTime);
        }
        private void _onPlayerResChg(ECurrency _currencyType, long _oldCount, long _newCount)
        {
            if (_currencyType == ECurrency.MARS_ENERGY)
                _refreshConditionDescContainer();
        }
    }
}