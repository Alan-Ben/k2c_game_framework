using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsEnergyYieldDetailGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoMarsEnergyYieldDetailGridItem>
    {
        private MarsBuildingInfo _m_buildingInfo;
        private int _m_index;


        public GGUISubWndMarsEnergyYieldDetailGridItem([NotNull] GGUIMonoMarsEnergyYieldDetailGridItem _mono)
            : base(_mono)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _resetGridItem()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnJump, _onBtnJumpClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnJump, _onBtnJumpClick);
        }


        public void refreshWnd(MarsBuildingInfo _buildingInfo, int _index)
        {
            _m_buildingInfo = _buildingInfo;
            _m_index = _index;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            // Display building index number
            ALUGUICommon.setLabelTxt(wnd.txtNum, _m_index + 1);
            // Display building level
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_buildingInfo.level));
            // Display energy yield speed (per minute)
            long energyYieldSpeed = _m_buildingInfo.energyProperty.value;
            ALUGUICommon.setLabelTxt(wnd.txtYieldSpeed, TextTranslate.instance.getLanguage(TransKeyConst.mars_energyYieldPerMin_num, energyYieldSpeed.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            _refreshProgressData();
        }
        public void tick()
        {
            _refreshProgressData();
        }


        private void _onBtnJumpClick(GameObject _obj)
        {
            if (_m_buildingInfo == null || wnd == null || _m_buildingInfo.refObj == null)
                return;

            float jumpTime = wnd.jumpTime;
            long buildingId = _m_buildingInfo.refObj.id;
            
            // Close current energy yield detail window
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Mars.C_MARS_BUILDING_ENERGY_YIELD_DETAIL);
            WinMsg.SendMsg(WinMsgType.TRIGGER_MARS_BUILDING_FOCUS, buildingId, jumpTime);
        }


        private void _refreshProgressData()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            // Update real-time energy storage progress
            long storedEnergy = _m_buildingInfo.energyProperty.storedEnergy;
            long maxStorage = _m_buildingInfo.energyProperty.maxStorage;

            float progressNormalized = maxStorage > 0 ? (float)storedEnergy / maxStorage : 0f;

            // Update progress slider
            if (wnd.sldYieldProgress != null)
                wnd.sldYieldProgress.value = progressNormalized;

            // Update progress text
            ALUGUICommon.setLabelTxt(wnd.txtYieldProgress, 
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, storedEnergy.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), maxStorage.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            // Set progress color based on fill percentage
            wnd.setProgressColor(progressNormalized);
        }
    }
}