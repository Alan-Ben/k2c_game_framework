using ALPackage;
using GOE;
using System.Collections.Generic;


/**************
 * 任务步骤配表
 **/

[System.Serializable]
public class QuestStepRefObj : _IALBasicRefObj
{
    public long _refId { get { return step_id; } }

    public long step_id;

    public long next_step_id;//下一步骤id

    public long sort_id;//排序序号id

    public NPGTextureIndex icon;//图标

    public string quest_step_name;//步骤标题
    public List<string> quest_step_name_args;//步骤标题参数

    public string quest_step_desc;//步骤描述文本

    public List<string> quest_step_desc_args;//步骤描述文本参数

    public bool auto_done;//是否自动完成任务

    public bool is_not_show_tip;//完成任务是否不弹tip

    public EValueFormatType process_num_format;//进度值格式化显示方式

    public List<NPCommonCostItem> done_gain_item_list;//完成后奖励的物品列表

    public List<NPCommonCostItem> show_item_list;//纯客户端展示的奖励物品列表

    public long continued_sec_s;//持续时长

    public _NPPlayerEffectSerializeInfo ext_done_effect;//任务完成触发的额外效果
    public _NPPlayerEffectSerializeInfo ext_done_client_effect;//任务完成触发的客户端额外效果

    public string quest_step_desc_str { get { return TextTranslate.instance.getLanguage(quest_step_desc, quest_step_desc_args); } }
    public string quest_step_name_str { get { return TextTranslate.instance.getLanguage(quest_step_name, quest_step_name_args); } }

    [System.NonSerialized]
    private List<QuestTargetRefObj> _m_lStepTargetList;
    public List<QuestTargetRefObj> stepTagetList { get { return _m_lStepTargetList; } }
    public void addTarget(QuestTargetRefObj _target)
    {
        if (null == _target)
            return;

        if (null == _m_lStepTargetList)
            _m_lStepTargetList = new List<QuestTargetRefObj>();

        _m_lStepTargetList.Add(_target);
    }
}

public class GSOQuestStepRefSet : _TALSOBasicRefSet<QuestStepRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/quest_refdata.unity3d"; } }
    public static string objName { get { return "quest_step"; } }
}
