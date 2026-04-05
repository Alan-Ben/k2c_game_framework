
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildQueueContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsBuildQueueContainerItem, GGUIMonoMarsBuildQueueContainer, GGUISubWndMarsBuildQueueContainerItem>
    {
        private ALCommonEnableTaskController _m_tickTask;


        public GGUISubWndMarsBuildQueueContainer(GGUIMonoMarsBuildQueueContainer _containerMono)
            : base(_containerMono)
        {
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


        protected override GGUISubWndMarsBuildQueueContainerItem _createItemWnd(GGUIMonoMarsBuildQueueContainerItem _itemMono)
        {
            return new GGUISubWndMarsBuildQueueContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsBuildQueueContainerItem _itemWnd, int _index)
        {
            _itemWnd.refreshWnd(_index);
        }


        public new void refreshWnd()
        {
            int maxQueueCount = GRefdataCoreMgr.instance.npGeneral.mars_building_queue_max_count;
            long realQueueCount = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_BUILDING_QUEUE_NUM);
            int showCount = Mathf.Max(maxQueueCount, (int)realQueueCount);
            refreshWnd(showCount);
        }


        private void _tick()
        {
            refreshAllItem(_item => _item?.refreshWnd());
        }
    }
}
