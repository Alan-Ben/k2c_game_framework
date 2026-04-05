using ALPackage;
using System;
using GOE;

/// <summary>
/// 伙伴资质技能等级表
/// </summary>
[Serializable]
public class HeroTalentSkillLevelRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//唯一id
    public long talent_skill_id;//技能id
    public long level;//等级
    public NPCommonCostItem cost;//升级到下一级消耗
    public NPCommonCostItem cost_per_level;//每级消耗增长
    public _NPPlayerConditionSerializeInfo upgrade_condition;//升级到下一级条件
    public HeroConditionSerializeInfo upgrade_hero_condition;//升级大臣条件
    public string upgrade_condition_desc;//升级条件描述
    public PlayerAttrPropertyModifier self_attr_prop_modifier;//自身加成
    public PlayerAttrPropertyModifier self_attr_prop_modifier_per_level;//每级自身加成
    public _UnionBonusSerializeInfo union_bonus;//全局加成（皮肤等会用到）
    public _UnionBonusSerializeInfo union_bonus_per_level;//每级全局加成 
}

public class GSOHeroTalentSkillLevelRefSet : _TALSOBasicRefSet<HeroTalentSkillLevelRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_talent_skill_level"; } }
}
