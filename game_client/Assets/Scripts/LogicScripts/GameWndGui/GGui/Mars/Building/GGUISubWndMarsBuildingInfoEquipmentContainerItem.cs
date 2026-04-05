using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildingInfoEquipmentContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsBuildingInfoEquipmentContainerItem>
    {
        private NPGGuiWndTexture _m_iconWnd;
        private Action<GGUISubWndMarsBuildingInfoEquipmentContainerItem> _m_onSelectCallback;
        private MarsBuildingEquipmentInfo _m_equipmentInfo;
        private bool _m_isSelected;


        public MarsBuildingEquipmentInfo equipmentInfo { get { return _m_equipmentInfo; } }
        public long equipmentId { get { return _m_equipmentInfo?.refObj.id ?? 0; } }


        public GGUISubWndMarsBuildingInfoEquipmentContainerItem(GGUIMonoMarsBuildingInfoEquipmentContainerItem _mono, Action<GGUISubWndMarsBuildingInfoEquipmentContainerItem> _onSelectCallback = null)
            : base(_mono)
        {
            _m_onSelectCallback = _onSelectCallback;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onBtnSelectClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onBtnSelectClick);
        }


        public void refreshWnd(MarsBuildingEquipmentInfo _equipmentInfo, bool _isSelected)
        {
            _m_equipmentInfo = _equipmentInfo;
            _m_isSelected = _isSelected;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_equipmentInfo == null)
                return;
                
            _m_iconWnd?.setTexture(_m_equipmentInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, _m_equipmentInfo.level);
            
            wnd.setSelectState(_m_isSelected);
            wnd.setEquipmentTypeState(_m_equipmentInfo.isMain);
            wnd.setLockState(_m_equipmentInfo.isUnlock);
        }
        public void playUpgradeEffect()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (wnd.upgradeEffectAnim != null)
                wnd.upgradeEffectAnim.Play(wnd.upgradeEffectAnimName);
        }


        private void _onBtnSelectClick(GameObject _obj)
        {
            _m_onSelectCallback?.Invoke(this);
        }
    }
}