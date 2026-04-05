using ALPackage;
using System;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 伙伴经营技能表
/// </summary>
[Serializable]
public class HeroBusinessSkillRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//阶段
    public bool can_upgrade;//是否可以升级
    public _BuildingConditionSerializeInfo limit_range;//限制范围
    public string name;//技能名称
    public List<string> name_args;//技能名称参数
    public string desc;//技能描述
    public NPGTextureIndex icon;//图标
    public long business_skill_lvl_max;//技能等级上限
    public PlayerBonusPropertyModifier bonus_prop_modifier;//初始加成
    public PlayerBonusPropertyModifier bonus_prop_modifier_per_level;//每级加成增长值
    public long upgrade_cost_group;//消耗组
}

public class GSOHeroBusinessSkillRefSet : _TALSOBasicRefSet<HeroBusinessSkillRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_business_skill"; } }
}
