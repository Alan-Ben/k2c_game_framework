using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndMarsBuildQueueDetailContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsBuildQueueDetailContainerItem, GGUIMonoMarsBuildQueueDetailContainer, GGUISubWndMarsBuildQueueDetailContainerItem>
    {
        private List<MarsBuildingInfo> _m_buildingList;


        public GGUISubWndMarsBuildQueueDetailContainer(GGUIMonoMarsBuildQueueDetailContainer _containerMono)
            : base(_containerMono)
        {
            initWnd();
        }


        protected override GGUISubWndMarsBuildQueueDetailContainerItem _createItemWnd(GGUIMonoMarsBuildQueueDetailContainerItem _itemMono)
        {
            return new GGUISubWndMarsBuildQueueDetailContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsBuildQueueDetailContainerItem _itemWnd, int _index)
        {
            MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoByQueueIndex(_index);
            _itemWnd.refreshWnd(buildingInfo);
        }


        public new void refreshWnd()
        {
            int queueCount = NPPlayer.instance.marsComp.buildingSubComponent.getCurrentQueueCount();
            refreshWnd(queueCount);
        }
        public void tickRefresh()
        {
            refreshAllItem(_item => _item?.tickRefresh());
        }
    }
}
