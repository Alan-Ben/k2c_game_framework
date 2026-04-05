using ALPackage;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMarsExploreTeamSelectContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsExploreTeamSelectContainerItem, GGUIMonoMarsExploreTeamSelectContainer, GGUISubWndMarsExploreTeamSelectContainerItem>
    {
        [ItemNotNull, NotNull] private readonly List<MarsExploreTeamInfo> _m_teamList;
        private _IMarsExploreTeamSelectDealer _m_target;
        private ALCommonEnableTaskController _m_tickTask;


        public GGUISubWndMarsExploreTeamSelectContainer(GGUIMonoMarsExploreTeamSelectContainer _containerMono)
            : base(_containerMono)
        {
            _m_teamList = new List<MarsExploreTeamInfo>();

            initWnd();
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();

            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
            
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_SELECT_ITEM_CONFIRM_BY_INDEX, _simulateClickConfirm);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_MARS_EXPLORE_TEAM_SELECT_ITEM_CONFIRM_BY_INDEX, _simulateClickConfirm);
            
            _m_tickTask.setDisable();
            
            base._onHideWnd();
        }
        

        protected override GGUISubWndMarsExploreTeamSelectContainerItem _createItemWnd(GGUIMonoMarsExploreTeamSelectContainerItem _itemMono)
        {
            GGUISubWndMarsExploreTeamSelectContainerItem item = new GGUISubWndMarsExploreTeamSelectContainerItem(_itemMono);
            return item;
        }
        protected override void _refreshItemWnd(GGUISubWndMarsExploreTeamSelectContainerItem _itemWnd, int _index)
        {
            MarsExploreTeamInfo teamInfo = _m_teamList.SafeGet(_index);
            _itemWnd.refreshWnd(_m_target, teamInfo);
        }


        public void refreshWnd(_IMarsExploreTeamSelectDealer _target)
        {
            _m_target = _target;
            refreshWnd();
        }
        public new void refreshWnd()
        {
            _m_teamList.Clear();
            NPPlayer.instance.marsComp.exploreSubComponent.getTeamListNonAlloc(_m_teamList);
            
            //这里只展示已经解锁的队伍
            for (int i = _m_teamList.Count - 1; i >= 0; i--)
            {
                if (!_m_teamList[i].isUnlock)
                {
                    _m_teamList.RemoveAt(i);
                }
            }
            
            refreshWnd(_m_teamList.Count);
        }
        
        private void _tick()
        {
            refreshAllItem(_item => _item?.refreshRemainTime());
        }
        
        private void _simulateClickConfirm(object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] is not long targetIndex)
                return;
            
            GGUISubWndMarsExploreTeamSelectContainerItem targetItem = getItem((int)targetIndex);
            targetItem?._simulateClickConfirm();
        }
    }
}
