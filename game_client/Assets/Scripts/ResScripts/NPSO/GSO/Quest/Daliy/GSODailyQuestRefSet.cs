using ALPackage;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 日常任务配表
/// </summary>
[System.Serializable]
public class DailyQuestRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;
    public NPGTextureIndex icon;//图标
    public string daily_quest_desc;//目标文本
    public List<string> daily_quest_desc_args;//目标文本参数列表
    public long simple_unlock_id;//解锁id
    public _NPPlayerConditionSerializeInfo show_cond;//客户端显示条件
    public EValueFormatType process_num_format;//进度值格式化显示方式
    public int process_count;//进度目标值
    public _NPPlayerVariableSerializeInfo process_cur_count;//进度当前值 (高级公式)
    public _NPPlayerEffectSerializeInfo go_to;//跳转效果
    public long go_to_simple_tutorial_id;//跳转简单引导id
    public List<NPCommonCostItem> reward_item_list;//奖励（未领取需要补发）
    public NPCommonCostItem activation_item;//完成获得活跃度


    public string daily_quest_title { get { return TextTranslate.instance.getLanguage(daily_quest_desc, daily_quest_desc_args); } }
}

public class GSODailyQuestRefSet : _TALSOBasicRefSet<DailyQuestRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/daliy_quest_refdata.unity3d"; } }
    public static string objName { get { return "daily_quest"; } }
}
