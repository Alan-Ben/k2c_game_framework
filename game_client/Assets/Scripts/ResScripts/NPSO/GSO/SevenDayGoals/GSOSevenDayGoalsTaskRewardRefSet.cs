
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 开服七天活动的任务奖励配置
    /// </summary>
    [Serializable]
    public class SevenDayGoalsTaskRewardRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; //唯一id
        public int day; //第几天
        public long goal_count; //目标数量
        public List<string> desc_args; //任务描述参数
        public long task_id; //任务id
        public List<NPCommonCostItem> reward_item_list; //奖励物品列表
        public long gain_score; //获得的积分

        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public SevenDayGoalsTaskRefObj task_ref;
        
        
        public string getDescTranslated
        {
            get { return task_ref == null ? string.Empty : TextTranslate.instance.getLanguage(task_ref.desc, desc_args); }
        }
    }
    public class GSOSevenDayGoalsTaskRewardRefSet : _TALSOBasicRefSet<SevenDayGoalsTaskRewardRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/seven_day_goals_refdata.unity3d"; } }
        public static string objName { get { return "seven_day_goals_task_reward"; } }
    }
}