using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子羁绊等级表(目前对于不同品质不同妃子羁绊等级升级消耗和带来的子嗣加成效果一致, 所以这里只有羁绊等级表, 没有具体羁绊表)
/// </summary>
[System.Serializable]
public class ConsortFettersLvlRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long) lvl; } }

    public int lvl;//羁绊等级
    public string name;//羁绊名称
    public NPGTextureIndex fetters_sign_icon;//羁绊称号图标
    public int consort_fetters_skill_lvl;//对应的羁绊技能等级
    
    public long need_consort_intimacy;//升到下一级需要的家人亲密度
    public long need_consort_charm;//升到下一级需要的家人加护力
    public int need_consort_num;//需要的妃子数量
    // public long need_player_lvl;//升到下一级需要的玩家等级
    
    public long adopt_child_quality_id;//可收养学生资质id
    public long study_bonus;//教学经验加成（万分比）
    public long income_bonus;//收益天资加成（万分比）
    public long graduate_get_item_num;//学生毕业奖励数量

    [ALHeader("效果描述参数列表")]
    public List<string> effect_desc_args_list;

#if NP_GAME
    
    /// <summary>
    /// 是否是最高等级
    /// </summary>
    public bool isMaxLvl
    {
        get
        {
            ConsortFettersLvlRefObj maxFettersLvlRefObj = GRefdataCoreMgr.instance.consortFettersLvlRefCore.refList.GetLast();//因为已经排序过了, 所以最高级就是最后一级
            return maxFettersLvlRefObj == null || maxFettersLvlRefObj.lvl <= lvl;
        }
    }
    
    private ChildQualityRefObj _m_rAdoptChildQualityRefObj;
    public long caretaker_bonus_calculate_coefficient; // 子嗣毕业的收益加成系数（万分比）

    public ChildQualityRefObj adoptChildQualityRefObj
    {
        get
        {
            if (_m_rAdoptChildQualityRefObj == null)
            {
                _m_rAdoptChildQualityRefObj = GRefdataCoreMgr.instance.childQualityCore.getRef(adopt_child_quality_id);
            }

            if (_m_rAdoptChildQualityRefObj == null)
            {
                Debug.LogError_EditorOnly($"找不到id:{adopt_child_quality_id} 对应的学生资质配置表数据");
            }

            return _m_rAdoptChildQualityRefObj;
        }
    }

#endif

}

/// <summary>
/// 
/// </summary>
public class GSOConsortFettersLvlRefSet : _TALSOBasicRefSet<ConsortFettersLvlRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_fetters_lvl"; } }
}