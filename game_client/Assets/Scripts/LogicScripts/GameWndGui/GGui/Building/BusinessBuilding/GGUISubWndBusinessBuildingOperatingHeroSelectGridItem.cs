using System;
using ALPackage;
using CommonEnum;
using UnityEngine;
using HeroGridData = GOE.GGUISubWndBusinessBuildingOperatingHeroSelectGrid.HeroGridData;

namespace GOE
{
    public class GGUISubWndBusinessBuildingOperatingHeroSelectGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoBusinessBuildingOperatingHeroSelectGridItem>
    {
        private readonly Action<HeroInfo> _m_onItemClick;

        private GGUIWndHeroCommonCardItem _m_heroCard;
        
        private HeroGridData _m_gridData;
        private int _m_selectIndex;
        private BusinessBuildingRefObj _m_buildingRef;
        
        
        public GGUISubWndBusinessBuildingOperatingHeroSelectGridItem(GGUIMonoBusinessBuildingOperatingHeroSelectGridItem _wnd, Action<HeroInfo> _onItemClick) 
            : base(_wnd)
        {
            _m_onItemClick = _onItemClick;
            
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_heroCard?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_heroCard?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_heroCard?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (_m_heroCard != null)
            {
                _m_heroCard.discard();
                _m_heroCard.ClickAction -= _onHeroClick;
                _m_heroCard = null;
            }
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroCard != null)
            {
                _m_heroCard = new GGUIWndHeroCommonCardItem(wnd.monoHeroCard);
                _m_heroCard.ClickAction = _onHeroClick;
            }
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(HeroGridData _gridData, int _selectIndex, BusinessBuildingRefObj _buildingRef)
        {
            _m_gridData = _gridData;
            _m_selectIndex = _selectIndex;
            _m_buildingRef = _buildingRef;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingRef == null)
                return;
            
            wnd.stateShow.setShowData(
                _m_selectIndex > 0 ? GGUIMonoBusinessBuildingOperatingHeroSelectGridItemState.SELECTED :
                    _m_gridData.isGain ? GGUIMonoBusinessBuildingOperatingHeroSelectGridItemState.CAPABLE : GGUIMonoBusinessBuildingOperatingHeroSelectGridItemState.NOT_JOINED);
            if (_m_gridData.isGain) GGameCommonInfo.disgrayImage(wnd.listGrayNotJoined);
            else GGameCommonInfo.grayImage(wnd.listGrayNotJoined);

            bool alreadyJoined = false;
            if (_m_gridData.heroInfo is { placeData: { buildingId: > 0 } } && _m_gridData.heroInfo.placeData.buildingId != _m_buildingRef.building_id)
            {
                BusinessBuildingRefObj refObj = GRefdataCoreMgr.instance.businessBuildingRefCore.getRef(_m_gridData.heroInfo.placeData.buildingId);
                if (refObj != null)
                {
                    alreadyJoined = true;
                    ALUGUICommon.setLabelTxt(wnd.txtAlreadyJoined, TextTranslate.instance.getLanguage(TransKeyConst.building_businessBuildingHeroAlreadyJoinsAnother_name, TextTranslate.instance.getLanguage(refObj.name)));
                }
            }
            ALUGUICommon.setGameObjEnable(wnd.listAlreadyJoinedShow, alreadyJoined);
            ALUGUICommon.setLabelTxt(wnd.txtSelectNum, _m_selectIndex > 0 ? _m_selectIndex.ToString() : "");
            ALUGUICommon.setLabelTxt(wnd.txtEarningBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_gridData.heroInfo?.getBusinessSkillAddPropValue(_m_buildingRef, EBonusPropertyType.BUILDING_PROFIT_ADD_PER) / 100f));
            _m_heroCard?.setInfo(new HeroCardShowInfo(_m_gridData.heroInfo, _m_gridData.heroRef));
        }
        
        
        private void _onHeroClick(GGUIWndHeroCommonCardItem _item)
        {
            if (wnd == null || _item == null || _m_gridData.heroRef == null)
                return;
            
            HeroInfo heroInfo = null;
            if (_item.heroCardShow is HeroInfo)
                heroInfo = _item.heroCardShow as HeroInfo;
            else if (_item.heroCardShow is HeroCardShowInfo showInfo)
                heroInfo = showInfo.heroInfo;

            if (heroInfo == null)
            {
                QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                    UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                    UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                    TextTranslate.instance.getLanguage(TransKeyConst.hero_accessWayDesc_str, TextTranslate.instance.getLanguage(_m_gridData.heroRef.transSource)),
                    rectTransform, wnd.accessTipOffset.x, wnd.accessTipOffset.y));
                return;
            }
                
            _m_onItemClick?.Invoke(heroInfo);
        }
    }
}