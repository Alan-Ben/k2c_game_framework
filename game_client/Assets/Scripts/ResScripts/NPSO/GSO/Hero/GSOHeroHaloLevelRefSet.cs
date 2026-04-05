using ALPackage;
using System;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 伙伴光环等级表
/// </summary>
[Serializable]
public class HeroHaloLevelRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一识别id
    public long halo_id;//光环id
    public long level;//等级
    public NPGTextureIndex lock_icon;//等级图标（未解锁状态）
    public NPGTextureIndex unlock_icon;//等级图标（已解锁状态）
    public List<WCGPairInt> halo_suit_skill_level_list;//套系技能加成等级
    public PlayerAttrPropertyModifier self_attr_prop_modifier;//自身加成
    public NPCommonCostItem upgrade_cost;//升级到下一级消耗
}

public class GSOHeroHaloLevelRefSet : _TALSOBasicRefSet<HeroHaloLevelRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_halo_level"; } }
}
