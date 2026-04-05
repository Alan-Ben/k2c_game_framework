using ALPackage;
using System;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 伙伴经营技能升级消耗组表
/// </summary>
[Serializable]
public class HeroBusinessSkillUpgradeRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//唯一id
    public long group_id;//组id
    public long business_skill_lvl;//技能等级
    public NPCommonCostItem upgrade_cost_item;//升到下级消耗
}

public class GSOHeroBusinessSkillUpgradeRefSet : _TALSOBasicRefSet<HeroBusinessSkillUpgradeRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_business_skill_upgrade"; } }
}
