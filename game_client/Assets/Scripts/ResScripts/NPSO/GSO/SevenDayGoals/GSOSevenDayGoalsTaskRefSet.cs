
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 开服七天活动的任务配置
    /// </summary>
    [Serializable]
    public class SevenDayGoalsTaskRefObj : _IALBasicRefObj
    {
        public long _refId { get { return task_id; } }
        
        public long task_id; //任务ID
        public string desc; //任务描述
        public EValueFormatType process_num_format;//进度值格式化显示方式
        public _NPPlayerVariableSerializeInfo process_cur_count;//计数器的高级公式(ENPPlayerVariableType)
        public List<string> add_msg_type_list;
        public _NPPlayerEffectSerializeInfo go_to;//跳转效果
        
    }
    public class GSOSevenDayGoalsTaskRefSet : _TALSOBasicRefSet<SevenDayGoalsTaskRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/seven_day_goals_refdata.unity3d"; } }
        public static string objName { get { return "seven_day_goals_task"; } }
    }
}