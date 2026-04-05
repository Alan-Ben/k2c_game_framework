using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 妃子星辉等级表
/// </summary>
[Serializable]
public class ConsortHaloLvlRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }
    
    public long id;//唯一id
    public long halo_id;//星辉id
    public int level;//等级
    public NPCommonCostItem cost_item;//升到下一级消耗
    public long add_intimacy;//增加的亲密度
    public long add_charm;//增加的加护力
    public List<NPCommonCostItem> reward_item_list;//奖励列表
    public List<ConsortHaloSkillInfo> halo_skill_list;//星辉技能

    public NPGTextureIndex unreach_icon;//未达到等级icon
    public NPGTextureIndex reach_icon;//达到等级icon
}

public class GSOConsortHaloLvlRefSet : _TALSOBasicRefSet<ConsortHaloLvlRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_halo_lvl"; } }
}