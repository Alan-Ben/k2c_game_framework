using ALPackage;
using JetBrains.Annotations;
using System;
using NPEnum;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamHeroSelectGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoMarsExploreTeamHeroSelectGridItem>
    {
        private readonly Action<HeroInfo> _m_onItemClick;
        private GGUIWndHeroCommonCardItem _m_subWndHeroCard;
        [CanBeNull] private HeroInfo _m_heroInfo;
        private int _m_selectNum;


        public GGUISubWndMarsExploreTeamHeroSelectGridItem(GGUIMonoMarsExploreTeamHeroSelectGridItem _wnd, Action<HeroInfo> _onItemClick)
            : base(_wnd)
        {
            _m_onItemClick = _onItemClick;

            initWnd();
        }


        [CanBeNull] public HeroInfo heroInfo { get { return _m_heroInfo; } }


        protected override void _onShowWnd()
        {
            _m_subWndHeroCard?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_subWndHeroCard?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_subWndHeroCard?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (_m_subWndHeroCard != null)
            {
                _m_subWndHeroCard.ClickAction -= _onClickHeroCard;
                _m_subWndHeroCard.discard();
                _m_subWndHeroCard = null;
            }
        }
        protected override void _resetGridItem()
        {
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroCard != null)
            {
                _m_subWndHeroCard = new GGUIWndHeroCommonCardItem(wnd.monoHeroCard);
                _m_subWndHeroCard.ClickAction += _onClickHeroCard;
            }
        }


        public void refreshWnd(HeroInfo _heroInfo, int _selectNum)
        {
            _m_heroInfo = _heroInfo;
            _m_selectNum = _selectNum;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_heroInfo == null)
                return;

            _m_subWndHeroCard?.setInfo(_m_heroInfo);
            wnd.setSelected(_m_selectNum > 0);
            ALUGUICommon.setLabelTxt(wnd.txtSelectNum, _m_selectNum);
            HeroStarRefObj starRef = GRefdataCoreMgr.instance.getHeroStarRef(_m_heroInfo.id, _m_heroInfo.star);
            long addPer = 0;
            if (starRef?.mars_team_player_property != null)
                addPer = starRef.mars_team_player_property.getPropertyValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
            ALUGUICommon.setLabelTxt(wnd.txtTeamPowerBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, addPer / 100f));
        }


        private void _onClickHeroCard(GGUIWndHeroCommonCardItem _card)
        {
            if (_m_heroInfo == null)
                return;

            _m_onItemClick?.Invoke(_m_heroInfo);
        }
    }
}
