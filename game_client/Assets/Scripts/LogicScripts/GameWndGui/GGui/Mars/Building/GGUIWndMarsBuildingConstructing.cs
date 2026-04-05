using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingConstructing : _ATALBasicUIWnd<GGUIMonoMarsBuildingConstructing>
    {
        [NotNull] public static GGUIWndMarsBuildingConstructing instance { get { return _g_instance ??= new GGUIWndMarsBuildingConstructing(); } }
        private static GGUIWndMarsBuildingConstructing _g_instance;

        
        private NPGGUIWndCommonItem _m_completeNowCostItemWnd;
        private _IMarsBuildingView _m_commonConstructingBuildingView;
        private CommonItemData _m_completeNowCostItemData;
        private ALCommonEnableTaskController _m_tickTask;
        private int _m_showSerialize;
        private new bool _m_bIsShow;
        

        public GGUIWndMarsBuildingConstructing() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingConstructing.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingConstructing.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _m_completeNowCostItemWnd?.showWnd();

            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);

            refreshWnd();

            _trySelectBuildingView();
        }
        protected override void _onHideWnd()
        {
            _tryUnselectBuildingView();

            _m_completeNowCostItemWnd?.hideWnd();

            _m_tickTask.setDisable();
            _m_showSerialize = ALSerializeOpMgr.next();
            
            _m_bIsShow = false;
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
            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onBtnCancelClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCompleteNowCostItem != null)
                _m_completeNowCostItemWnd = new NPGGUIWndCommonItem(wnd.monoCompleteNowCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onBtnCancelClick);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
            ALUGUICommon.combineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _tryUnselectBuildingView();
            _m_commonConstructingBuildingView = _buildingView;
            _trySelectBuildingView();
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_commonConstructingBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonConstructingBuildingView.buildingInfo;
                
            ALUGUICommon.setLabelTxt(wnd.txtName, buildingInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, buildingInfo.descTranslated);
            long remainingTime = buildingInfo.remainingBuildOrUpgradeTime;
            ALUGUICommon.setLabelTxt(wnd.txtBuildTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainingTime));
            long totalTime = buildingInfo.buildOrUpgradeEndTime - buildingInfo.buildOrUpgradeStartTime;
            if (wnd.sldBuildProgress != null)
            {
                wnd.sldBuildProgress.minValue = 0;
                wnd.sldBuildProgress.maxValue = totalTime;
                wnd.sldBuildProgress.value = totalTime - remainingTime;
            }

            _refreshCompleteNowCost();
        }
        

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_CONSTRUCTING);
        }
        private void _onBtnCancelClick(GameObject _obj)
        {
            if (_m_commonConstructingBuildingView == null)
                return;
                
            MarsBuildingInfo buildingInfo = _m_commonConstructingBuildingView.buildingInfo;
            int serialize = _m_showSerialize;
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.marsComp.buildingSubComponent.cancelBuild(buildingInfo.refObj.id, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                if (serialize != _m_showSerialize)
                    return;

                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_CONSTRUCTING);
            });
        }
        private void _onBtnSpeedUpClick(GameObject _obj)
        {
            if (_m_commonConstructingBuildingView == null)
                return;
                
            GGUIWndMarsTimeSpeedUp.addNode(_m_commonConstructingBuildingView.buildingInfo, _m_commonConstructingBuildingView.buildingInfo);
        }
        private void _onBtnCompleteNowClick(GameObject _obj)
        {
            if (_m_commonConstructingBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonConstructingBuildingView.buildingInfo;

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
        
        
        private void _refreshCompleteNowCost()
        {
            if (_m_commonConstructingBuildingView == null || _m_completeNowCostItemWnd == null)
                return;
                
            MarsBuildingInfo buildingInfo = _m_commonConstructingBuildingView.buildingInfo;
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
            if (wnd == null || !_m_bIsShow || _m_commonConstructingBuildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_commonConstructingBuildingView.buildingInfo;

            long remainingTime = buildingInfo.remainingBuildOrUpgradeTime;

            // Auto-close if time is up
            if (remainingTime <= 0)
            {
                _onBtnCloseClick(null);
                return;
            }

            ALUGUICommon.setLabelTxt(wnd.txtBuildTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainingTime));

            long totalTime = buildingInfo.buildOrUpgradeEndTime - buildingInfo.buildOrUpgradeStartTime;
            if (wnd.sldBuildProgress != null)
            {
                wnd.sldBuildProgress.minValue = 0;
                wnd.sldBuildProgress.maxValue = totalTime;
                wnd.sldBuildProgress.value = totalTime - remainingTime;
            }
        }
        private void _trySelectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_commonConstructingBuildingView == null)
                return;

            _m_commonConstructingBuildingView.setSelected(true);
            MainAdditionMarsTDScene.instance.focusToPos(_m_commonConstructingBuildingView.position, wnd.focusViewportPos, wnd.focusScale, wnd.focusTime);
        }
        private void _tryUnselectBuildingView()
        {
            if (wnd == null || !_m_bIsShow || _m_commonConstructingBuildingView == null)
                return;

            _m_commonConstructingBuildingView?.setSelected(false);
            MainAdditionMarsTDScene.instance.cancelThePosFocus(wnd.focusTime);
        }
    }
}