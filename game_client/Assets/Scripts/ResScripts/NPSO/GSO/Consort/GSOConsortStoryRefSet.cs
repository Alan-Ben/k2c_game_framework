using System.Collections.Generic;
using ALPackage;
using Common.ConsortEnum;
using GOE;

/// <summary>
/// 妃子故事表
/// </summary>
[System.Serializable]
public class ConsortStoryRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public string story_name;//故事名称
    public string story_desc;//故事描述
    public bool is_hide;//是否隐藏
    public long consort_id;//家人id
    public EConsortStoryType dialog_type;//对话类型
    public long dialogue_id;//对话id
    public long unlock_cg;//解锁cg
    
    public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
    public string unlock_condition_desc;//解锁条件描述
    public List<string> unlock_condition_desc_args_list;//解锁条件描述参数列表
    public EHintShowWay unlock_hint_show_type;//解锁提示显示方式
    public string unlock_progress_desc;//解锁进度描述
    public List<string> unlock_progress_desc_args_list;//解锁进度描述参数列表
    public string unlock_way_goto_btn_desc;//解锁方式前往按钮描述
    public _NPPlayerEffectSerializeInfo unlock_way_goto_effect;//解锁方式前往效果

    public bool default_trigger;//是否默认触发(解锁的同时触发)
    public string trigger_way_desc;//触发方式描述
    public List<string> trigger_way_desc_args_list;//触发方式描述参数列表
    public EHintShowWay trigger_way_hint_show_type;//触发方式提示显示类型
    public string trigger_way_goto_btn_desc;//触发方式前往按钮描述
    public _NPPlayerEffectSerializeInfo trigger_way_goto_effect;//触发方式前往效果

    /// <summary>
    /// 获取故事状态
    /// </summary>
    /// <returns></returns>
    public EConsortStoryState getStoryState()
    {
        // 有配置解锁条件 且 解锁条件不满足
        if (unlock_condition != null && unlock_condition.hasCondition && !unlock_condition.IsEnable(null))
        {
            return EConsortStoryState.LOCK;
        }

        // 下面都是已解锁的状态
        
        // 默认触发
        if (default_trigger)
        {
            return EConsortStoryState.UNLOCK_TRIGGERED;
        }

#if NP_GAME
        GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(consort_id);
        if (consortInfo != null && consortInfo.isTriggeredStory(id))
            return EConsortStoryState.UNLOCK_TRIGGERED;
        else
            return EConsortStoryState.UNLOCK_NOT_TRIGGERED_YET;
#else
        return EConsortStoryState.UNLOCK_NOT_TRIGGERED_YET;
#endif
    }

#if NP_GAME

    public EConsortStoryState getStoryState(GGottenConsortInfo _consortInfo)
    {
        // 有配置解锁条件 且 解锁条件不满足
        if (unlock_condition != null && unlock_condition.hasCondition && !unlock_condition.IsEnable(null))
        {
            return EConsortStoryState.LOCK;
        }

        // 下面都是已解锁的状态
        
        // 默认触发
        if (default_trigger)
        {
            return EConsortStoryState.UNLOCK_TRIGGERED;
        }
        
        // 若传入的已获取妃子数据为空 或 妃子id不匹配 或 通过已获取妃子数据判断还未触发过
        if(_consortInfo == null || _consortInfo.consortId != consort_id || !_consortInfo.isTriggeredStory(id))
            return EConsortStoryState.UNLOCK_NOT_TRIGGERED_YET;
        
        return EConsortStoryState.UNLOCK_TRIGGERED;
    }
    
#endif
}

public class GSOConsortStoryRefSet : _TALSOBasicRefSet<ConsortStoryRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_story"; } }
}