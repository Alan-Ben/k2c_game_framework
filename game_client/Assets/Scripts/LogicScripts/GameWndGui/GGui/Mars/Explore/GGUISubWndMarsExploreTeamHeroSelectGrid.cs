using ALPackage;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamHeroSelectGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMarsExploreTeamHeroSelectGridItem, GGUIMonoMarsExploreTeamHeroSelectGrid, GGUISubWndMarsExploreTeamHeroSelectGridItem>
    {
        [ItemNotNull, NotNull] private readonly List<HeroInfo> _m_heroList;
        [ItemNotNull, NotNull] private readonly List<HeroInfo> _m_selectedHeroList;
        
        private MarsExploreTeamInfo _m_teamInfo;
        private Action<List<HeroInfo>> _m_onSelectChg;


        public GGUISubWndMarsExploreTeamHeroSelectGrid(GGUIMonoMarsExploreTeamHeroSelectGrid _containerMono)
            : base(_containerMono)
        {
            _m_heroList = new List<HeroInfo>();
            _m_selectedHeroList = new List<HeroInfo>();
            
            initWnd();
        }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_SELECT_GRID_ITEM_BY_INDEX, _simulateClickMarsExploreTeamHeroCard);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_HERO_SELECT_GRID_ITEM_BY_INDEX, _simulateClickMarsExploreTeamHeroCard);
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }


        [ItemNotNull, NotNull] public List<HeroInfo> curSelectedHeroList { get { return _m_selectedHeroList; } }


        protected override GGUISubWndMarsExploreTeamHeroSelectGridItem _createItemWnd(GGUIMonoMarsExploreTeamHeroSelectGridItem _itemMono)
        {
            GGUISubWndMarsExploreTeamHeroSelectGridItem gridItem = new GGUISubWndMarsExploreTeamHeroSelectGridItem(_itemMono, _onClickItem);
            return gridItem;
        }
        protected override void _onRefreshItemWnd(GGUISubWndMarsExploreTeamHeroSelectGridItem _itemWnd, int _itemIdx)
        {
            if (_itemWnd == null)
                return;

            HeroInfo heroInfo = _m_heroList.SafeGet(_itemIdx);
            int selectNum = _m_selectedHeroList.IndexOf(heroInfo);
            _itemWnd.refreshWnd(heroInfo, selectNum + 1);
        }


        public void refreshWnd(MarsExploreTeamInfo _teamInfo, Action<List<HeroInfo>> _onSelectChg)
        {
            if (_teamInfo == null)
                return;

            _m_teamInfo = _teamInfo;
            _m_selectedHeroList.Clear();
            _teamInfo.getHeroListNonAlloc(_m_selectedHeroList);
            _m_onSelectChg = _onSelectChg;

            _m_heroList.Clear();
            NPPlayer.instance.heroComponent.getAllList(_m_heroList);
            NPPlayer.instance.marsComp.exploreSubComponent.actionWithAllTeam(_team =>
            {
                if (_team == null || _team == _teamInfo)
                    return true;

                _team.actionWithHero(_heroInfo => _m_heroList.Remove(_heroInfo));
                return true;
            });

            _m_heroList.Sort((_a, _b) => _b.power.CompareTo(_a.power));
            setItemCount(_m_heroList.Count);
        }

        public void clickItem(HeroInfo _heroInfo)
        {
            _onClickItem(_heroInfo);
        }

        private void _onClickItem(HeroInfo _heroInfo)
        {
            if (_heroInfo == null || _m_teamInfo == null)
                return;

            if (_m_selectedHeroList.Contains(_heroInfo))
            {
                _m_selectedHeroList.Remove(_heroInfo);
            }
            else
            {
                int maxHeroNum = GRefdataCoreMgr.instance.npGeneral.mars_explore_team_hero_max_num;
                if (_m_selectedHeroList.Count >= maxHeroNum)
                {
                    // todo: maybe shows the tip.
                    return;
                }

                _m_selectedHeroList.Add(_heroInfo);
            }

            forceRefreshAllItem();
            _m_onSelectChg?.Invoke(_m_selectedHeroList);
        }

        private void _simulateClickMarsExploreTeamHeroCard(params object[] _objects)
        {
            if (_objects is not { Length: > 0 } || _objects[0] is not long itemIndex)
                return;

            HeroInfo heroInfo = _m_heroList.SafeGet((int)itemIndex);
            if (heroInfo != null)
                _onClickItem(heroInfo);
        }
    }
}
