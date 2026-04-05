using ALPackage;

namespace GOE
{
    public class GGUISubWndBusinessBuildingRnDPageDevelopContainerItem : _ATALBasicUISubWnd<GGUIMonoBusinessBuildingRnDPageDevelopContainerItem>
    {
        private BusinessBuildingDevelopRefObj _m_developRef;
        private BusinessBuildingInfo _m_buildingInfo;

        private NPGGuiWndTexture _m_developIcon;
        private GGUISubWndCommonBonusShower _m_bonusShower;
        private int _m_iIndex;


        public GGUISubWndBusinessBuildingRnDPageDevelopContainerItem(GGUIMonoBusinessBuildingRnDPageDevelopContainerItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_developIcon?.showWnd();
            _m_bonusShower?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_developIcon?.hideWnd();
            _m_bonusShower?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_developIcon?.discardTexture();
            _m_bonusShower?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_developIcon?.discard();
            _m_bonusShower?.discard();
            _m_developIcon = null;
            _m_bonusShower = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgDevelopIcon != null)
                _m_developIcon = new NPGGuiWndTexture(wnd.imgDevelopIcon);
            if (wnd.monoDevelopBonus != null)
                _m_bonusShower = new GGUISubWndCommonBonusShower(wnd.monoDevelopBonus);
        }
        

        public void refreshWnd(BusinessBuildingDevelopRefObj _refObj, BusinessBuildingInfo _buildingInfo, int _index)
        {
            _m_developRef = _refObj;
            _m_buildingInfo = _buildingInfo;
            _m_iIndex = _index;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_developRef == null || _m_buildingInfo == null)
                return;
            
            _m_developIcon?.setTexture(_m_developRef.icon);
            ALUGUICommon.setLabelTxt(wnd.txtDevelopName, TextTranslate.instance.getLanguage(_m_developRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtDevelopDesc, TextTranslate.instance.getLanguage(_m_developRef.desc, _m_developRef.desc_args));
            _m_bonusShower?.refreshWnd(_m_developRef.add_bonus?.unionBonus, _m_buildingInfo.bonusJudgeParts);
            ALUGUICommon.setLabelTxt(wnd.txtUnlockRequire, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingDevelopUnlockTip_level, _m_developRef.level_required));
            wnd.setStateShow(_m_buildingInfo.level >= _m_developRef.level_required, _m_iIndex == 0);
        }
    }
}