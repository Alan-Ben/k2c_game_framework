using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamHeroContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsExploreTeamHeroContainerItem>
    {
        private readonly Action<HeroInfo> _m_onItemClick;

        private NPGGuiWndTexture _m_wndHeroTexture;
        private GGuiWndSprite _m_wndQualityHeadBg;
        private GGUIWndHeroCommonStar _m_wndHeroStar;
        private HeroInfo _m_heroInfo;


        public GGUISubWndMarsExploreTeamHeroContainerItem(GGUIMonoMarsExploreTeamHeroContainerItem _wnd, Action<HeroInfo> _onItemClick)
            : base(_wnd)
        {
            _m_onItemClick = _onItemClick;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_wndHeroTexture?.showWnd();
            _m_wndQualityHeadBg?.showWnd();
            _m_wndHeroStar?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_wndHeroTexture?.hideWnd();
            _m_wndQualityHeadBg?.hideWnd();
            _m_wndHeroStar?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_wndHeroTexture?.discardTexture();
            _m_wndQualityHeadBg?.discardTexture();
            _m_wndHeroStar?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_wndHeroTexture?.discard();
            _m_wndHeroTexture = null;
            _m_wndQualityHeadBg?.discard();
            _m_wndQualityHeadBg = null;
            _m_wndHeroStar?.discard();
            _m_wndHeroStar = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgHero != null)
                _m_wndHeroTexture = new NPGGuiWndTexture(wnd.imgHero);
            if (wnd.imgQualityHeadBg != null)
                _m_wndQualityHeadBg = new GGuiWndSprite(wnd.imgQualityHeadBg);
            if (wnd.monoHeroStar != null)
                _m_wndHeroStar = new GGUIWndHeroCommonStar(wnd.monoHeroStar);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickBtn);
        }


        public void refreshWnd(HeroInfo _heroInfo)
        {
            _m_heroInfo = _heroInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            bool isEmpty = _m_heroInfo == null;
            wnd.setState(isEmpty);

            if (_m_heroInfo == null) 
                return;
            
            _m_wndHeroTexture?.setTexture(_m_heroInfo.getIcon());
            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroInfo.id);
            _m_wndQualityHeadBg?.setTexture(qualityExtRef?.hero_head_bg);
            _m_wndHeroStar?.setInfo(_m_heroInfo.star);
            ALUGUICommon.setLabelTxt(wnd.txtHeroPower, _m_heroInfo.power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, _m_heroInfo.star);
            long addPer = 0;
            if (starRef?.mars_team_player_property != null)
                addPer = starRef.mars_team_player_property.getPropertyValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
            ALUGUICommon.setLabelTxt(wnd.txtTeamPowerBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, addPer / 100f));
        }

        private void _onClickBtn(GameObject _go)
        {
            if (_m_heroInfo == null)
                return;

            _m_onItemClick?.Invoke(_m_heroInfo);
        }
    }
}
