using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildingInfoPageSettleSlotContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsBuildingInfoPageSettleSlotContainerItem>
    {
        private MarsBuildingInfo _m_buildingInfo;
        private int _m_slotIndex;


        public GGUISubWndMarsBuildingInfoPageSettleSlotContainerItem([NotNull] GGUIMonoMarsBuildingInfoPageSettleSlotContainerItem _mono)
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
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }


        public void refreshWnd(MarsBuildingInfo _buildingInfo, int _slotIndex)
        {
            _m_buildingInfo = _buildingInfo;
            _m_slotIndex = _slotIndex;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo?.refObj == null)
                return;

            int peopleCount = _m_buildingInfo.settleSlotData.peopleCount;
            int slotPeopleLimit = GRefdataCoreMgr.instance.npGeneral.mars_building_slot_people_count;
            int peopleInThisSlot = Mathf.Max(0, Mathf.Min(slotPeopleLimit, peopleCount - slotPeopleLimit * _m_slotIndex));
            ALUGUICommon.setLabelTxt(wnd.txtSettleProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, peopleInThisSlot, slotPeopleLimit));
            if (wnd.sldSettleProgress != null)
            {
                wnd.sldSettleProgress.minValue = 0;
                wnd.sldSettleProgress.maxValue = slotPeopleLimit;
                wnd.sldSettleProgress.value = peopleInThisSlot;
            }
            
            bool isUnlock = _m_slotIndex < _m_buildingInfo.settleSlotData.refObj.slot_num;
            int level = isUnlock ? _m_buildingInfo.settleSlotData.refObj.level : _m_buildingInfo.settleSlotData.nextRefObj?.level ?? 0;
            ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, level));
            wnd.setUnlockState(isUnlock);
        }
        public void playEffect()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (wnd.effectAnim != null)
                wnd.effectAnim.Play(wnd.effectAnimName);
        }
    }
}