using System;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndBusinessBuildingOperatingHeroContainerItem : _ATALBasicUISubWnd<GGUIMonoBusinessBuildingOperatingHeroContainerItem>
    {
        private readonly Action<GGUISubWndBusinessBuildingOperatingHeroContainerItem> _m_onItemClick;
        
        private NPGGuiWndTexture _m_heroIcon;
        
        private bool _m_isUnlock;
        private bool _m_isLastUnlock;
        private long _m_slotEmployeeNum;
        private BusinessBuildingRefObj _m_buildingRef;
        private HeroInfo _m_heroInfo;
        private bool _m_showRedTip;


        public GGUISubWndBusinessBuildingOperatingHeroContainerItem(GGUIMonoBusinessBuildingOperatingHeroContainerItem _wnd, Action<GGUISubWndBusinessBuildingOperatingHeroContainerItem> _onItemClick) 
            : base(_wnd)
        {
            _m_onItemClick = _onItemClick;
            
            initWnd();
        }
        
        
        public bool isUnlock { get { return _m_isUnlock; } }
        public long slotEmployeeNum { get { return _m_slotEmployeeNum; } }
        public BusinessBuildingRefObj buildingRef { get { return _m_buildingRef; } }
        public HeroInfo heroInfo { get { return _m_heroInfo; } }
        

        protected override void _onShowWnd()
        {
            _m_heroIcon?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_heroIcon?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_heroIcon?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_heroIcon?.discard();
            _m_heroIcon = null;

            if (wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onBtnSelect);
        }
        protected override void _onWndInitDone()
        {
            if (wnd != null)
            {
                if (wnd.imgHero != null)
                    _m_heroIcon = new NPGGuiWndTexture(wnd.imgHero);
                
                ALUGUICommon.combineBtnClick(wnd.btnSelect, _onBtnSelect);
            }
        }

        
        public void refreshWnd(bool _isUnlock, bool _lastUnlock, long _slotEmployeeNum, BusinessBuildingRefObj _buildingRef, HeroInfo _heroInfo, bool _showRedTip)
        {
            _m_isUnlock = _isUnlock;
            _m_isLastUnlock = _lastUnlock;
            _m_slotEmployeeNum = _slotEmployeeNum;
            _m_buildingRef = _buildingRef;
            _m_heroInfo = _heroInfo;
            _m_showRedTip = _showRedTip;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            GGUIMonoBusinessBuildingOperatingHeroContainerItemState state;
            if (!_m_isUnlock)
            {
                if (_m_isLastUnlock)
                    state = GGUIMonoBusinessBuildingOperatingHeroContainerItemState.Locked_Next_Slot;
                else
                    state = GGUIMonoBusinessBuildingOperatingHeroContainerItemState.Locked;
            }
            else if (_m_heroInfo == null)
                state = GGUIMonoBusinessBuildingOperatingHeroContainerItemState.Unlocked;
            else
                state = GGUIMonoBusinessBuildingOperatingHeroContainerItemState.Selected;
            wnd.stateShow.setShowData(state);
            _m_heroIcon?.setTexture(_m_heroInfo?.getCardImage());
            ALUGUICommon.setLabelTxt(wnd.txtHeroBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_heroInfo?.getBusinessSkillAddPropValue(_m_buildingRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f));
            ALUGUICommon.setLabelTxt(wnd.txtUnlockNum, _m_slotEmployeeNum);
            wnd.setRedTipShow(_m_showRedTip);
        }


        private void _onBtnSelect(GameObject _)
        {
            _m_onItemClick?.Invoke(this);
        }
    }
}