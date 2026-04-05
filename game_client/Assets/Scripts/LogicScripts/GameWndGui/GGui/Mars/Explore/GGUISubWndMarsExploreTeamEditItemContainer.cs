using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamEditItemContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsExploreTeamEditItemContainerItem, GGUIMonoMarsExploreTeamEditItemContainer, GGUISubWndMarsExploreTeamEditItemContainerItem>
    {
        [NotNull] private readonly List<MarsExploreTeamInfo> _m_teamList;
        private ALCommonEnableTaskController _m_tickTask;
        
        
        public GGUISubWndMarsExploreTeamEditItemContainer(GGUIMonoMarsExploreTeamEditItemContainer _containerMono)
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
            _m_tickTask.setDisable();
            
            base._onHideWnd();
        }
        protected override GGUISubWndMarsExploreTeamEditItemContainerItem _createItemWnd(GGUIMonoMarsExploreTeamEditItemContainerItem _itemMono)
        {
            return new GGUISubWndMarsExploreTeamEditItemContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsExploreTeamEditItemContainerItem _itemWnd, int _index)
        {
            MarsExploreTeamInfo teamInfo = _m_teamList.SafeGet(_index);
            _itemWnd.setItemIndex(_index);
            _itemWnd.refreshWnd(teamInfo);
        }


        public new void refreshWnd()
        {
            _m_teamList.Clear();
            NPPlayer.instance.marsComp.exploreSubComponent.getTeamListNonAlloc(_m_teamList);
            
            for (int i = 0; i < _m_teamList.Count; i++)
            {
                MarsExploreTeamInfo item = _m_teamList[i];
                if (item.isUnlock)
                    continue;

                if (!item.isShow)
                {
                    _m_teamList.RemoveAt(i);
                    i--;
                }
            }
            
            refreshWnd(_m_teamList.Count);
        }


        private void _tick()
        {
            refreshAllItem(_item => _item?.refreshRemainTime());
        } 
    }
}
