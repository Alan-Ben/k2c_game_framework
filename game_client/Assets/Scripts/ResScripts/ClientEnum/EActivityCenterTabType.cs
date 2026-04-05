using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动中心页签类型
    /// </summary>
    public enum EActivityCenterTabType
    {
        [InspectorName("限时冲榜")]
        RANK_RUSH,
        [InspectorName("阶段奖励")]
        STEP_REWARD,
        [InspectorName("千万目标")]
        EARNING_GOAL,
        [InspectorName("七日目标")]
        SEVEN_DAY_GOALS,
        [InspectorName("热更活动")]
        HOTFIX_ACTIVITY,
        [InspectorName("阶段任务冲榜活动")]
        STEP_TASK_RUSH_RANK_ACTIVITY,
        [InspectorName("首次组队活动")]
        FIRST_TEAM,
    }
}