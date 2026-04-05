using ALPackage;
using System;
using GOE;

/// <summary>
/// 伙伴套系技能等级表
/// </summary>
[Serializable]
public class HeroSuitSkillLevelRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//阶段
    public long halo_suit_skill_id;//技能id
    public long level;//技能等级
    public PlayerAttrPropertyModifier attr_prop_modifier;//基础套系内大臣属性加成
    public PlayerAttrPropertyModifier attr_prop_modifier_per_lvl;//基于起始等级后的每级加成
}

public class GSOHeroSuitSkillLevelRefSet : _TALSOBasicRefSet<HeroSuitSkillLevelRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_halo_suit_skill_level"; } }
}
