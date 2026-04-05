using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamMiniInfoContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsExploreTeamMiniInfoContainerItem, GGUIMonoMarsExploreTeamMiniInfoContainer, GGUISubWndMarsExploreTeamMiniInfoContainerItem>
    {
        [ItemNotNull, NotNull] private readonly List<MarsExploreTeamInfo> _m_teamList;
        private ALCommonEnableTaskController _m_tickTask;


        public GGUISubWndMarsExploreTeamMiniInfoContainer(GGUIMonoMarsExploreTeamMiniInfoContainer _containerMono)
            : base(_containerMono)
        {
            _m_teamList = new List<MarsExploreTeamInfo>();
            initWnd();
        }


        protected override void _onShowWnd()
        {
            base._onShowWnd();
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
        }
        protected override void _onHideWnd()
        {
            base._onHideWnd();
            _m_tickTask.setDisable();
        }


        protected override GGUISubWndMarsExploreTeamMiniInfoContainerItem _createItemWnd(GGUIMonoMarsExploreTeamMiniInfoContainerItem _itemMono)
        {
            return new GGUISubWndMarsExploreTeamMiniInfoContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsExploreTeamMiniInfoContainerItem _itemWnd, int _index)
        {
            MarsExploreTeamInfo teamInfo = _m_teamList.SafeGet(_index);
            _itemWnd.refreshWnd(teamInfo);
        }


        public void refreshWnd()
        {
            _m_teamList.Clear();
            NPPlayer.instance.marsComp.exploreSubComponent.getTeamListNonAlloc(_m_teamList);
            
            //这里只展示1队未解锁队伍
            int lockCount = 1;
            for (int i = 0; i < _m_teamList.Count; i++)
            {
                MarsExploreTeamInfo item = _m_teamList[i];
                if (item.isUnlock)
                    continue;

                if (lockCount <= 0)
                {
                    _m_teamList.RemoveAt(i);
                    i--;
                }
                lockCount--;
            }
            
            refreshWnd(_m_teamList.Count);
        }


        private void _tick()
        {
            refreshAllItem(_item => _item?.tick());
        }
    }
}
