using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 成就阶段数据
    /// </summary>
    [System.Serializable]
    public class StageGoalTaskRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id;//唯一识别ID
        public long simple_unlock_id;//解锁id
        public _NPPlayerVariableSerializeInfo process_cur_count;//计数器的高级公式(ENPPlayerVariableType)
        public long process_count;//进度条计数目标值
        public List<string> add_msg_type_list;
        public EValueFormatType process_num_format;//进度值格式化显示方式
        public NPGTextureIndex icon;//图标
        public string title;//标题
        public List<string> title_args;//标题参数
        public _NPPlayerEffectSerializeInfo go_to;//跳转效果
        public List<NPCommonCostItem> reward_item_list;//奖励物品列表
        public string desc;//描述
        public List<string> desc_args;//描述参数

        public string getTitle { get => TextTranslate.instance.getLanguage(title, title_args); }
        public string getDesc { get => TextTranslate.instance.getLanguage(desc, desc_args); }

    }

    public class GSOStageGoalTaskRefSet : _TALSOBasicRefSet<StageGoalTaskRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/stage_refdata.unity3d"; } }
        public static string objName { get { return "stage_goal_task"; } }
    }
}