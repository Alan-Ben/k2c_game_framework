using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndMarsBuildingInfoPageSettle : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsBuildingInfoPageSettle>
    {
        private MarsBuildingInfo _m_buildingInfo;
        private GGUISubWndMarsBuildingInfoPageSettleSlotContainer _m_slotContainer;
        private NPGGUIWndCommonToggleEx _m_tenToggleWnd;


        public GGUIPrefabSubWndMarsBuildingInfoPageSettle([NotNull] Transform _parent)
            : base(_parent)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingInfoPageSettle.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingInfoPageSettle.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_slotContainer?.showWnd();
            _m_tenToggleWnd?.showWnd();
            
            refreshWnd(true);

            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingSettleChg += _onBuildingStateChg;
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_SETTLE, _onSimulateClickSettle);
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingSettleChg -= _onBuildingStateChg;
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_SETTLE, _onSimulateClickSettle);
            
            _m_slotContainer?.hideWnd();
            _m_tenToggleWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_slotContainer?.resetWnd();
            _m_tenToggleWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_slotContainer?.discard();
            _m_slotContainer = null;
            _m_tenToggleWnd?.discard();
            _m_tenToggleWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSettle, _onBtnSettleClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoSlotContainer != null)
                _m_slotContainer = new GGUISubWndMarsBuildingInfoPageSettleSlotContainer(wnd.monoSlotContainer);
            if (wnd.monoTenToggle != null)
            {
                _m_tenToggleWnd = new NPGGUIWndCommonToggleEx(wnd.monoTenToggle);
                _m_tenToggleWnd.clickDelegate += _onTenToggleClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnSettle, _onBtnSettleClick);
        }


        public void refreshWnd(MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd(true);
        }
        public void refreshWnd(bool _reset)
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            _m_slotContainer?.refreshWnd(_m_buildingInfo, _reset);
            bool isMax = _m_buildingInfo.settleSlotData.isMax;
            wnd.setSettleMax(isMax);
        }


        private void _onBtnSettleClick(GameObject _obj)
        {
            if (_m_buildingInfo == null)
                return;

            bool isTenMode = _m_tenToggleWnd is { isOn: true };
            int settleCount = isTenMode ? 10 : 1;
            if (_m_buildingInfo.settleSlotData.canSettleCount < settleCount)
                settleCount = _m_buildingInfo.settleSlotData.canSettleCount;
            if (settleCount <= 0)
                return;

            long totalIdlePeople = NPPlayer.instance.marsComp.peopleSubComponent.idlePeopleNum;
            if (totalIdlePeople < settleCount)
                settleCount = (int) totalIdlePeople;

            if (totalIdlePeople <= 0)
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsPopularGetTip.instance, GGUIWndMarsPopularGetTip.instance.showWnd, UINodeTagConst.C_MARS_POPULAR_GET_TIP);
                return;
            }

            int oldPeopleCount = _m_buildingInfo.settleSlotData.peopleCount;
            NPPlayer.instance.marsComp.buildingSubComponent.ReqDispatchPeople(_m_buildingInfo.refObj.id, settleCount, () =>
            {
                if (_m_buildingInfo == null || _m_slotContainer == null)
                    return;

                int slotPeopleLimit = GRefdataCoreMgr.instance.npGeneral.mars_building_slot_people_count;
                int newPeopleCount = _m_buildingInfo.settleSlotData.peopleCount;

                int firstChangedSlot = oldPeopleCount / slotPeopleLimit;
                int lastChangedSlot = (newPeopleCount - 1) / slotPeopleLimit;

                for (int i = firstChangedSlot; i <= lastChangedSlot; i++)
                {
                    GGUISubWndMarsBuildingInfoPageSettleSlotContainerItem item = _m_slotContainer.getItem(i);
                    item?.playEffect();
                    _m_slotContainer.moveIfCantSee(item);
                }
            });
        }
        private void _onTenToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_toggle == null)
                return;
            
            _toggle.setSelected(!_toggle.isOn);
        }
        private void _onBuildingStateChg(long _buildingId)
        {
            if (_m_buildingInfo == null)
                return;

            if (_m_buildingInfo.refObj.id != _buildingId)
                return;
            
            refreshWnd(false);
        }
        private void _onSimulateClickSettle()
        {
            if (wnd == null) return;
            _onBtnSettleClick(wnd.btnSettle);
        }
    }
}