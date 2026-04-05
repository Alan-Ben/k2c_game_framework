using ALPackage;
using CommonEnum;

namespace GOE
{
    public class GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem : _ATALBasicUISubWnd<GGUIMonoBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem>
    {
        private NPGGuiWndTexture _m_imgHeroIcon;
        
        private HeroInfo _m_heroInfo;
        private BusinessBuildingRefObj _m_buildingRef;
        
        
        public GGUISubWndBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem(GGUIMonoBusinessBuildingSelectOperatingHeroConfirmHeroContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_imgHeroIcon?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_imgHeroIcon?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_imgHeroIcon?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_imgHeroIcon?.discard();
            _m_imgHeroIcon = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgHeroIcon != null)
                _m_imgHeroIcon = new NPGGuiWndTexture(wnd.imgHeroIcon);
        }
        

        public void refreshWnd(HeroInfo _heroInfo, BusinessBuildingRefObj _newBuildingRef)
        {
            _m_heroInfo = _heroInfo;
            _m_buildingRef = _newBuildingRef;

            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_heroInfo == null)
                return;

            if (_m_heroInfo.heroRefObj != null)
                ALUGUICommon.setLabelTxt(wnd.txtHeroName, _m_heroInfo.heroRefObj.transName);

            _m_imgHeroIcon?.setTexture(_m_heroInfo.getIcon());

            BusinessBuildingRefObj originBuildingRef = GRefdataCoreMgr.instance.businessBuildingRefCore.getRef(_m_heroInfo.placeData.buildingId);
            if (originBuildingRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtOriginBuilding, TextTranslate.instance.getLanguage(originBuildingRef.name));
                ALUGUICommon.setLabelTxt(wnd.txtOriginBuildingBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_heroInfo?.getBusinessSkillAddPropValue(originBuildingRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f));
            }

            if (_m_buildingRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtNewBuilding, TextTranslate.instance.getLanguage(_m_buildingRef.name));
                ALUGUICommon.setLabelTxt(wnd.txtNewBuildingBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_heroInfo?.getBusinessSkillAddPropValue(_m_buildingRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f));
            }
        }
    }
}