using System.Collections.Generic;
using Common.ActivityFundObj;

namespace GOE
{
    /// <summary>
    /// 基金任务详情Grid
    /// </summary>
    public class GGUIWndFundTaskDetailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoFundTaskDetailGridItem, GGUIMonoFundTaskDetailGrid, GGUIWndFundTaskDetailGridItem>
    {
        private List<ActivityFund_TaskInfo> _m_lTaskInfoList;


        public GGUIWndFundTaskDetailGrid(GGUIMonoFundTaskDetailGrid _gridMono) : base(_gridMono)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }
        protected override void _onRefreshItemWnd(GGUIWndFundTaskDetailGridItem _itemWnd, int _itemIdx)
        {
            if (_m_lTaskInfoList == null)
                return;

            if (_itemIdx < 0 || _itemIdx >= _m_lTaskInfoList.Count)
                return;
            
            ActivityFund_TaskInfo taskInfo = _m_lTaskInfoList[_itemIdx];
            _itemWnd?.refreshWnd(taskInfo);
        }
        protected override GGUIWndFundTaskDetailGridItem _createItemWnd(GGUIMonoFundTaskDetailGridItem _itemMono)
        {
            return new GGUIWndFundTaskDetailGridItem(_itemMono);
        }


        /// <summary>
        /// 显示任务列表
        /// </summary>
        public void refreshWnd(List<ActivityFund_TaskInfo> _taskInfoList)
        {
            _m_lTaskInfoList = _taskInfoList;
            setItemCount(_m_lTaskInfoList?.Count ?? 0);
        }
    }
}
