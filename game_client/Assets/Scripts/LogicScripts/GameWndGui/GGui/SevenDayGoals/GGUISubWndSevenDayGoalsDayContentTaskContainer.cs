using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsDayContentTaskContainer : _AGGUISubWndCommonContainer<GGUIMonoSevenDayGoalsDayContentTaskContainerItem, GGUIMonoSevenDayGoalsDayContentTaskContainer, GGUISubWndSevenDayGoalsDayContentTaskContainerItem>
    {
        private List<SevenDayGoalsTaskRewardRefObj> _m_taskList;
        
        
        public GGUISubWndSevenDayGoalsDayContentTaskContainer(GGUIMonoSevenDayGoalsDayContentTaskContainer _containerMono) 
            : base(_containerMono)
        {
            initWnd();
        }
        

        protected override GGUISubWndSevenDayGoalsDayContentTaskContainerItem _createItemWnd(GGUIMonoSevenDayGoalsDayContentTaskContainerItem _itemMono)
        {
            return new GGUISubWndSevenDayGoalsDayContentTaskContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndSevenDayGoalsDayContentTaskContainerItem _itemWnd, int _index)
        {
            SevenDayGoalsTaskRewardRefObj taskInfo = _m_taskList.SafeGet(_index);
            if (taskInfo == null)
                return;

            _itemWnd.refreshWnd(taskInfo);
        }


        public void refreshWnd(List<SevenDayGoalsTaskRewardRefObj> _taskList)
        {
            _m_taskList = _taskList;
            refreshWnd(_m_taskList?.Count ?? 0);
        }
    }
}