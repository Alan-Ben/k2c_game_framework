using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildQueueDetailContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsBuildQueueDetailContainerItem>
    {
        private MarsBuildingInfo _m_buildingInfo;
        
        private NPGGuiWndTexture _m_buildingIconTexture;
        private TextUpgradePropertyShow<int> _m_txtLevelChgShow;


        public GGUISubWndMarsBuildQueueDetailContainerItem(GGUIMonoMarsBuildQueueDetailContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_buildingIconTexture?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_buildingIconTexture?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_buildingIconTexture?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_buildingIconTexture?.discard();
            _m_buildingIconTexture = null;
            _m_txtLevelChgShow = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgBuildingIcon != null)
                _m_buildingIconTexture = new NPGGuiWndTexture(wnd.imgBuildingIcon);
            if (wnd.txtLevelChg != null)
                _m_txtLevelChgShow = new TextUpgradePropertyShow<int>(wnd.txtLevelChg, string.Empty);

            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
        }


        public void refreshWnd(MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            MarsBuildingRefObj buildingRefObj = _m_buildingInfo.refObj;
            _m_buildingIconTexture?.setTexture(buildingRefObj.building_icon);
            ALUGUICommon.setLabelTxt(wnd.txtBuildingName, _m_buildingInfo.nameTranslated);
            long remainingTime = _m_buildingInfo.remainingBuildOrUpgradeTime;
            long totalTime = _m_buildingInfo.buildOrUpgradeEndTime - _m_buildingInfo.buildOrUpgradeStartTime;
            if (wnd.sldBuildProgress != null)
            {
                wnd.sldBuildProgress.minValue = 0;
                wnd.sldBuildProgress.maxValue = totalTime;
                wnd.sldBuildProgress.value = totalTime - remainingTime;
            }
            ALUGUICommon.setLabelTxt(wnd.txtBuildRemainTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainingTime));

            int currentLevel = _m_buildingInfo.level;
            int targetLevel = _m_buildingInfo.state == MarsBuildingInfo.StateType.Constructing ? 1 : currentLevel + 1;
            _m_txtLevelChgShow?.setValue(currentLevel, targetLevel);
            bool isConstructing = _m_buildingInfo.state == MarsBuildingInfo.StateType.Constructing;
            bool isUpgrading = _m_buildingInfo.state == MarsBuildingInfo.StateType.Upgrading;
            ALUGUICommon.setGameObjEnable(wnd.listConstructionShow, isConstructing);
            ALUGUICommon.setGameObjEnable(wnd.listUpgradeShow, isUpgrading);
        }
        public void tickRefresh()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            long remainingTime = _m_buildingInfo.remainingBuildOrUpgradeTime;
            long totalTime = _m_buildingInfo.buildOrUpgradeEndTime - _m_buildingInfo.buildOrUpgradeStartTime;

            if (wnd.sldBuildProgress != null)
            {
                wnd.sldBuildProgress.value = totalTime - remainingTime;
            }

            ALUGUICommon.setLabelTxt(wnd.txtBuildRemainTime, TimeUtil.millisecondsToTime_DayHourOrHMS(remainingTime));
        }


        private void _onBtnSpeedUpClick(GameObject _obj)
        {
            if (_m_buildingInfo == null)
                return;

            GGUIWndMarsTimeSpeedUp.addNode(_m_buildingInfo, _m_buildingInfo);
        }
    }
}
