using ALPackage;
using UnityEngine;

namespace GOE
{
    public enum GGUIMonoSevenDayGoalsDayContentTabType
    {
        [InspectorName("Task === 任务列表")]
        Task,
        [InspectorName("Shop === 礼包")]
        Shop,
    }
    public class GGUIMonoSevenDayGoalsDayContent : _AALBasicUIWndMono
    {
        [ALHeader("页签按钮列表")]
        public GGUIMonoSevenDayGoalsDayContentPageTabList tabList;
    }
}