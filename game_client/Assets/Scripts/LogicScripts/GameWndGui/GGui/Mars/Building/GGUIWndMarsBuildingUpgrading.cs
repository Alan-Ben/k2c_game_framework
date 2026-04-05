using ALPackage;
using Common.GuildEnum;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public abstract class GGUIWndMarsBuildingUpgrading<T> : _ATALBasicUIWnd<T>
        where T : GGUIMonoMarsBuildingUpgrading
    {
        private NPGGUIWndCommonItem _m_completeNowCostItemWnd;
        
        private _IMarsBuildingView _m_commonUpgradingBuildingView;

        protected _IMarsBuildingView upgradingBuildingView { get { return _m_commonUpgradingBuildingView; } }
        private CommonItemData _m_completeNowCostItemData;
        private TextUpgradePropertyShow<string> _m_powerUpgradeShow;
        private ALCommonEnableTaskController _m_tickTask;
        private int _m_showSerialize;

        private new bool _m_bIsShow;
        

        public GGUIWndMarsBuildingUpgrading() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _m_completeNowCostItemWnd?.showWnd();

            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);

            refreshWnd();

            _trySelectBuildingView();
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, refreshWnd);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;
        }
        protected override void _onHideWnd()
        {
            _tryUnselectBuildingView();

            _m_completeNowCostItemWnd?.hideWnd();

            _m_tickTask.setDisable();
            _m_showSerialize = ALSerializeOpMgr.next();

            _m_bIsShow = false;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, refreshWnd);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;
        }
        protected override void _onReset()
        {
            _m_completeNowCostItemWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_completeNowCostItemWnd?.discard();
            _m_completeNowCostItemWnd = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnBack, _onBtnBackClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onBtnCancelClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnAssist, _onClickAssist);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCompleteNowCostItem != null)
                _m_completeNowCostItemWnd = new NPGGUIWndCommonItem(wnd.monoCompleteNowCostItem);
            if (wnd.txtPower != null)
                _m_powerUpgradeShow = new TextUpgradePropertyShow<string>(wnd.txtPower, string.Empty);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnBack, _onBtnBackClick);
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onBtnCancelClick);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
            ALUGUICommon.combineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
            ALUGUICommon.combineBtnClick(wnd.btnAssist, _onClickAssist);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _tryUnselectBuildingView();
            _m_commonUpgradingBuildingView = _buildingView;
            _trySelectBuildingView();
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_commonUpgradingBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonUpgradingBuildingView.buildingInfo;
                
            ALUGUICommon.setLabelTxt(wnd.txtName, buildingInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, buildingInfo.level));
            _m_powerUpgradeShow?.setValue(buildingInfo.levelData.currentPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), buildingInfo.levelData.nextPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            long remainingTime = buildingInfo.remainingBuildOrUpgradeTime;
            ALUGUICommon.setLabelTxt(wnd.txtUpgradeTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainingTime));
            long totalTime = buildingInfo.buildOrUpgradeEndTime - buildingInfo.buildOrUpgradeStartTime;
            if (wnd.sldUpgradeProgress != null)
            {
                wnd.sldUpgradeProgress.minValue = 0;
                wnd.sldUpgradeProgress.maxValue = totalTime;
                wnd.sldUpgradeProgress.value = totalTime - remainingTime;
            }

            bool canAssist = buildingInfo != null && buildingInfo.guildHelpId <= 0 && NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding();
            ALUGUICommon.setGameObjEnable(wnd.canAssistShowGos, canAssist);
            ALUGUICommon.setGameObjEnable(wnd.canAssistHideGos, !canAssist);
            _refreshCompleteNowCost();
        }
        

        protected virtual void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_UPGRADING);
        }
        protected virtual void _onBtnBackClick(GameObject _obj)
        {
            _onBtnCloseClick(null);
            if (_m_commonUpgradingBuildingView?.buildingInfo == null || !_m_commonUpgradingBuildingView.buildingInfo.equipmentData.isValid())
                return;
            
            GGUIWndMarsBuildingInfo.instance.refreshWnd(_m_commonUpgradingBuildingView);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsBuildingInfo.instance, GGUIWndMarsBuildingInfo.instance.showWnd,
                EUIQueueStageType.MAIN, UINodeTagConst_Mars.C_MARS_BUILDING_INFO, false, false);
        }
        private void _onBtnCancelClick(GameObject _obj)
        {
            if (_m_commonUpgradingBuildingView == null)
                return;
                
            MarsBuildingInfo buildingInfo = _m_commonUpgradingBuildingView.buildingInfo;
            int serialize = _m_showSerialize;
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.marsComp.buildingSubComponent.cancelUpgrade(buildingInfo.refObj.id, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                if (serialize != _m_showSerialize)
                    return;

                _onBtnCloseClick(null);
            });
        }
        private void _onBtnSpeedUpClick(GameObject _obj)
        {
            if (_m_commonUpgradingBuildingView == null || _m_commonUpgradingBuildingView.buildingInfo == null)
                return;
                
            GGUIWndMarsTimeSpeedUp.addNode(_m_commonUpgradingBuildingView.buildingInfo, _m_commonUpgradingBuildingView.buildingInfo);
        }
        protected virtual void _onBtnCompleteNowClick(GameObject _obj)
        {
            if (_m_commonUpgradingBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonUpgradingBuildingView.buildingInfo;
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
                buildingInfo.reqCompleteNow((_isSucc) =>
                {
                    if (serialize != _m_showSerialize || !_isSucc)
                        return;
                    
                    _onBtnCloseClick(null);
                });
            }
        }
        private void _onClickAssist(GameObject _obj)
        {
            MarsBuildingInfo buildingInfo = _m_commonUpgradingBuildingView?.buildingInfo;

            if (buildingInfo == null) return;
            bool canAssist = buildingInfo.guildHelpId <= 0 && NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding();
            if (!canAssist) return;
            NPPlayer.instance.guildMarsHelpComp.reqSendMarsHelp(EGuildMarsHelpObjType.BUILDING_QUEUE, buildingInfo.queueId,
                (_isSuc) =>
                {
                    refreshWnd();
                });
        }
        
        /// <summary>
        /// 建筑状态变化事件处理
        /// </summary>
        private void _onBuildingStateChg(long _buildingId, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            var buildingRef = GRefdataCoreMgr.instance.marsBuildingRefCore.getRef(_buildingId);
            if (buildingRef != null && buildingRef.building_type == Common.MarsEnum.EMarsBuildingType.HELP)
            {
                if (_newState == MarsBuildingInfo.StateType.Normal)
                    refreshWnd();
            }
        }
        private void _refreshCompleteNowCost()
        {
            if (_m_commonUpgradingBuildingView == null || _m_completeNowCostItemWnd == null)
                return;
                
            MarsBuildingInfo buildingInfo = _m_commonUpgradingBuildingView.buildingInfo;
            if(buildingInfo != null)
                _m_completeNowCostItemWnd.setItem(buildingInfo.completeNowCostItem);
        }
        private void _tick()
        {
            _refreshCompleteNowCost();
            _refreshTimeData();
        }

        private void _refreshTimeData()
        {
            if (wnd == null || !_m_bIsShow || _m_commonUpgradingBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonUpgradingBuildingView.buildingInfo;

            long remainingTime = buildingInfo.remainingBuildOrUpgradeTime;

            // Auto-close if time is up
            if (remainingTime <= 0)
            {
                _onBtnCloseClick(null);
                return;
            }

            ALUGUICommon.setLabelTxt(wnd.txtUpgradeTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainingTime));

            long totalTime = buildingInfo.buildOrUpgradeEndTime - buildingInfo.buildOrUpgradeStartTime;
            if (wnd.sldUpgradeProgress != null)
            {
                wnd.sldUpgradeProgress.minValue = 0;
                wnd.sldUpgradeProgress.maxValue = totalTime;
                wnd.sldUpgradeProgress.value = totalTime - remainingTime;
            }
        }
        private void _trySelectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_commonUpgradingBuildingView == null)
                return;

            _m_commonUpgradingBuildingView.setSelected(true);
            MainAdditionMarsTDScene.instance.focusToPos(_m_commonUpgradingBuildingView.position, wnd.focusViewportPos, wnd.focusScale, wnd.focusTime);
        }
        private void _tryUnselectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_commonUpgradingBuildingView == null)
                return;

            _m_commonUpgradingBuildingView?.setSelected(false);
            MainAdditionMarsTDScene.instance.cancelThePosFocus(wnd.focusTime);
        }
    }
    public class GGUIWndMarsBuildingUpgrading : GGUIWndMarsBuildingUpgrading<GGUIMonoMarsBuildingUpgrading>
    {
        [NotNull] public static GGUIWndMarsBuildingUpgrading instance { get { return _g_instance ??= new GGUIWndMarsBuildingUpgrading(); } }
        private static GGUIWndMarsBuildingUpgrading _g_instance;
        
        
        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingUpgrading.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingUpgrading.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}