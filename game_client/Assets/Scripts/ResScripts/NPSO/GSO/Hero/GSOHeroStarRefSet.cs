using ALPackage;
using System;
using System.Collections.Generic;
using CommonEnum;
using GOE;

/// <summary>
/// 伙伴觉醒表
/// </summary>
[Serializable]
public class HeroStarRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    public long id;//唯一id
    public long star;//星级
    public long hero_star_group_id;//组id
    public NPCommonCostItem upgrade_cost;//升到当前星级消耗
    public _NPPlayerConditionSerializeInfo upgrade_player_condition;//升到当前星级玩家条件
    public HeroConditionSerializeInfo upgrade_condition;//升到当前星级条件
    public string upgrade_condition_desc;//升到当前星级条件描述
    public List<string> desc_args;//升到当前星级条件描述参数
    public PlayerAttrPropertyModifier self_attr_prop_modifier;//自身获得实力加成
    public NPPlayerPropertyModifier mars_team_player_property;//火星探索队伍玩家属性加成
}

public class GSOHeroStarRefSet : _TALSOBasicRefSet<HeroStarRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_star"; } }
}
