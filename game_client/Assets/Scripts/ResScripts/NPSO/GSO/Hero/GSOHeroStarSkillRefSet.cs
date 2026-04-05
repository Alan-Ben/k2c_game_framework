using ALPackage;
using System;

/// <summary>
/// 伙伴觉醒技能表
/// </summary>
[Serializable]
public class HeroStarSkillRefObj : _IALBasicRefObj
{
    public long _refId { get { return skill_id; } }
    public long skill_id;//技能id
    public NPGTextureIndex icon;//技能图标
    public string name;//技能名称
    public string desc;//技能描述

    [NonSerialized]
    private long _m_lMaxLevel;//最大等级
    public long maxLevel { get { return _m_lMaxLevel; } set { _m_lMaxLevel = value; } }
}

public class GSOHeroStarSkillRefSet : _TALSOBasicRefSet<HeroStarSkillRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_star_skill"; } }
}
