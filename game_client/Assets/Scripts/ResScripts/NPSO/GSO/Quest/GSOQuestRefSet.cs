using ALPackage;
using NPEnum;
using System.Collections.Generic;
using Common.QuestEnum;
using GOE;



/**************
 * 任务配表
 **/
 
[System.Serializable]
public class QuestRefObj : _IALBasicRefObj
{
    public long _refId { get { return quest_id; } }

    public long quest_id;

    public long next_quest_id;//下一个任务id

    public long first_step_id;//第一步id

    public string quest_name;//任务标题文本

    public List<NPCommonCostItem> start_cost_item_list;//开启任务需要消耗的物品列表

    public bool is_drop;//是否可以主动放弃

    public int done_count;//可以完成的次数

    public int start_count; //可以接受的次数

    public List<NPCommonCostItem> done_gain_item_list; //完成最后一个step获得的额外奖励

    public _NPPlayerConditionSerializeInfo start_condition;//开启条件

    public string start_condition_desc;//开启条件文本

    public List<string> start_condition_desc_args;//开启条件文本参数
    
    public EQuestType quest_type;//任务枚举 主线/支线

    public bool is_auto_follow; //接受任务时是否自动追踪

    public string start_condition_str { get { return TextTranslate.instance.getLanguage(start_condition_desc, start_condition_desc_args); } }

    public string quest_name_str { get { return TextTranslate.instance.getLanguage(TransKeyConst.quest_name_str, quest_name); } }

}

public class GSOQuestRefSet : _TALSOBasicRefSet<QuestRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/quest_refdata.unity3d"; } }
    public static string objName { get { return "quest"; } }
}
