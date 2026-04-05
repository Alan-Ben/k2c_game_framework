using ALPackage;
using System;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 伙伴觉醒技能表
/// </summary>
[Serializable]
public class HeroStarSkillLevelRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//唯一id
    public long skill_id;//技能id
    public long skill_level;//技能等级（也就是星级+1）
    public _UnionBonusSerializeInfo union_bonus;//全局加成
    public List<string> skill_desc_args;//技能描述参数
    public List<string> next_level_add_desc_args;//下一级加成描述参数
    public NPPlayerPropertyModifier add_player;//玩家属性加成
}

public class GSOHeroStarSkillLevelRefSet : _TALSOBasicRefSet<HeroStarSkillLevelRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_star_skill_level"; } }
}
