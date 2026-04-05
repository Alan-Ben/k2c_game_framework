using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndMarsBuildingInfoPageEquipment : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsBuildingInfoPageEquipment>
    {
        private MarsBuildingInfo _m_buildingInfo;

        private NPGGuiWndTexture _m_iconWnd;
        private NPGGUIWndCommonItem _m_upgradeCostWnd;
        private GGUISubWndMarsBuildingInfoEquipmentContainer _m_equipmentContainer;
        private GGUISubWndMarsBuildingInfoEquipmentPropertyContainer _m_propertyContainer;


        public GGUIPrefabSubWndMarsBuildingInfoPageEquipment([NotNull] Transform _parent)
            : base(_parent)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsBuildingInfoPageEquipment.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsBuildingInfoPageEquipment.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public GGUISubWndMarsBuildingInfoEquipmentContainer equipmentContainer { get { return _m_equipmentContainer; } }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            _m_upgradeCostWnd?.showWnd();
            _m_equipmentContainer?.showWnd();
            _m_propertyContainer?.showWnd();

            refreshWnd(true);

            NPPlayer.instance.marsComp.buildingSubComponent.onEquipmentUpgraded += _onEquipmentUpgraded;
            NPPlayer.instance.rescourceComp.onResourceCountChg += _onPlayerResChg;

            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_EQUIPMENT_UPGRADE, _onSimulateClickUpgrade);
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.marsComp.buildingSubComponent.onEquipmentUpgraded -= _onEquipmentUpgraded;
            NPPlayer.instance.rescourceComp.onResourceCountChg -= _onPlayerResChg;

            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_EQUIPMENT_UPGRADE, _onSimulateClickUpgrade);

            _m_iconWnd?.hideWnd();
            _m_upgradeCostWnd?.hideWnd();
            _m_equipmentContainer?.hideWnd();
            _m_propertyContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
            _m_upgradeCostWnd?.resetWnd();
            _m_equipmentContainer?.resetWnd();
            _m_propertyContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            _m_upgradeCostWnd?.discard();
            _m_upgradeCostWnd = null;
            _m_equipmentContainer?.discard();
            _m_equipmentContainer = null;
            _m_propertyContainer?.discard();
            _m_propertyContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.monoUpgradeCost != null)
                _m_upgradeCostWnd = new NPGGUIWndCommonItem(wnd.monoUpgradeCost);
            if (wnd.monoEquipmentContainer != null)
                _m_equipmentContainer = new GGUISubWndMarsBuildingInfoEquipmentContainer(wnd.monoEquipmentContainer, _onEquipmentSelectedChg);
            if (wnd.monoPropertyContainer != null)
                _m_propertyContainer = new GGUISubWndMarsBuildingInfoEquipmentPropertyContainer(wnd.monoPropertyContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
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

            if (_reset)
                _m_equipmentContainer?.refreshWnd(_m_buildingInfo);
            else
                _m_equipmentContainer?.refreshAllItem();
            
            MarsBuildingEquipmentInfo equipmentInfo = _m_equipmentContainer?.currentSelected;
            if (equipmentInfo == null)
                return;
                
            ALUGUICommon.setLabelTxt(wnd.txtName, equipmentInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, equipmentInfo.descTranslated);
            
            _m_iconWnd?.setTexture(equipmentInfo.refObj.icon);

            bool isLock = !equipmentInfo.isUnlock;
            bool isLevelLimit = equipmentInfo.isLevelLimit;
            bool isLevelMax = equipmentInfo.isLevelMax;
            if (isLock)
                wnd.setLock();
            else if (isLevelMax)
                wnd.setLevelMax();
            else if (isLevelLimit)
                wnd.setReachLevelLimit();
            else
                wnd.setCanUpgrade();
            
            _m_propertyContainer?.refreshWnd(equipmentInfo);

            _refreshUpgradeCost();
        }

        private void _refreshUpgradeCost()
        {
            MarsBuildingEquipmentInfo equipmentInfo = _m_equipmentContainer?.currentSelected;
            if (equipmentInfo == null)
                return;
            _m_upgradeCostWnd?.setItem(equipmentInfo.currentLevelRef?.upgrade_cost);
        }


        private void _onEquipmentUpgraded(long _buildingId, long _equipmentId)
        {
            if (wnd == null || _m_buildingInfo == null || _m_buildingInfo.refObj.id != _buildingId)
                return;
            
            if (_m_equipmentContainer == null || !_m_equipmentContainer.trySwitchToNextUpgradableEquipment())
                refreshWnd(false);

            GGUISubWndMarsBuildingInfoEquipmentContainerItem equipmentItem = _m_equipmentContainer?.getItem(_item => _item.equipmentId == _equipmentId);
            equipmentItem?.playUpgradeEffect();

            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (wnd == null)
                    return;
                if (wnd.upgradeEffectSfxParent != null)
                    PlaySfxMgr.instance.playUISfx(wnd.upgradeEffectSfxId, wnd.upgradeEffectSfxParent);
            }, wnd.upgradeEffectSfxDelay);
        }
        private void _onPlayerResChg(ECurrency _currencyType, long _oldCount, long _newCount)
        {
            if (_currencyType == ECurrency.MARS_ENERGY)
                _refreshUpgradeCost();
        }
        private void _onBtnUpgradeClick(GameObject _obj)
        {
            if (_m_buildingInfo == null || _m_equipmentContainer?.currentSelected == null)
                return;
            
            MarsBuildingEquipmentInfo equipmentInfo = _m_equipmentContainer.currentSelected;
            if (!equipmentInfo.isUnlock || equipmentInfo.isLevelMax)
                return;

            if (equipmentInfo.isLevelLimit)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_buildingEquipmentLevelLimitTip_none);
                return;
            }

            if (equipmentInfo.currentLevelRef != null && !GCommon.isItemEnough(equipmentInfo.currentLevelRef.upgrade_cost, true))
                return;
                
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.marsComp.buildingSubComponent.requestUpgradeEquipment(_m_buildingInfo.refObj.id, equipmentInfo.refObj.id, () =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
            });
        }
        private void _onEquipmentSelectedChg(MarsBuildingEquipmentInfo _)
        {
            refreshWnd(false);
        }

        private void _onSimulateClickUpgrade()
        {
            if (wnd == null) return;
            _onBtnUpgradeClick(wnd.btnUpgrade);
        }
    }
}