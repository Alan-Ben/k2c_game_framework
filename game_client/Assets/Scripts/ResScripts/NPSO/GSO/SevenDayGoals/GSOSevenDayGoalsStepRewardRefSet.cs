
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 开服七天活动的总积分奖励
    /// </summary>
    [Serializable]
    public class SevenDayGoalsStepRewardRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; //唯一id
        public long need_score; //目标积分
        public List<NPCommonCostItem> gain_item_list; //奖励物品列表
    }
    public class GSOSevenDayGoalsStepRewardRefSet : _TALSOBasicRefSet<SevenDayGoalsStepRewardRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/seven_day_goals_refdata.unity3d"; } }
        public static string objName { get { return "seven_day_goals_step_reward"; } }
    }
}