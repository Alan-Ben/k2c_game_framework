using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamHeroContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsExploreTeamHeroContainerItem, GGUIMonoMarsExploreTeamHeroContainer, GGUISubWndMarsExploreTeamHeroContainerItem>
    {
        [NotNull] private readonly List<HeroInfo> _m_heroList;
        private Action<HeroInfo> _m_onItemClick;


        public GGUISubWndMarsExploreTeamHeroContainer(GGUIMonoMarsExploreTeamHeroContainer _containerMono,  Action<HeroInfo> _onItemClick)
            : base(_containerMono)
        {
            _m_heroList = new List<HeroInfo>();
            _m_onItemClick = _onItemClick;
            initWnd();
        }

        protected override void _onDiscardEx()
        {
            _m_onItemClick = null;
        }

        protected override GGUISubWndMarsExploreTeamHeroContainerItem _createItemWnd(GGUIMonoMarsExploreTeamHeroContainerItem _itemMono)
        {
            return new GGUISubWndMarsExploreTeamHeroContainerItem(_itemMono, _onClickItem);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsExploreTeamHeroContainerItem _itemWnd, int _index)
        {
            HeroInfo heroInfo = _m_heroList.SafeGet(_index);
            _itemWnd.refreshWnd(heroInfo);
        }


        public void refreshWnd(MarsExploreTeamInfo _teamInfo)
        {
            if (_teamInfo != null)
                _teamInfo.getHeroListNonAlloc(_m_heroList);
            else
                _m_heroList.Clear();
            
            refreshWnd(GRefdataCoreMgr.instance.npGeneral.mars_explore_team_hero_max_num);
        }
        public void refreshWnd(List<HeroInfo> _heroList)
        {
            _m_heroList.Clear();
            if (_heroList != null)
                _m_heroList.AddRange(_heroList);
            
            refreshWnd(GRefdataCoreMgr.instance.npGeneral.mars_explore_team_hero_max_num);
        }
        
        private void _onClickItem(HeroInfo _heroInfo)
        {
            if (_m_onItemClick != null) 
                _m_onItemClick(_heroInfo);
        }
    }
}
