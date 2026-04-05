using ALPackage;
using System.Collections.Generic;
using Common.QuestEnum;

/// <summary>
/// 日常任务积分奖励配表
/// </summary>
[System.Serializable]
public class DailyQuestActiveRewardRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;
    public EDailyQuestType daily_quest_type;//任务类型
    public NPCommonCostItem draw_active_reward_need;//领奖条件，需要达到的积分，达成条件即可领奖
    public long reward_id;//奖励id
}

public class GSODailyQuestActiveRewardRefSet : _TALSOBasicRefSet<DailyQuestActiveRewardRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/daily_quest_refdata.unity3d"; } }
    public static string objName { get { return "daily_quest_active_reward"; } }
}
