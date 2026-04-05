using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 骑士阶段表
/// </summary>
[Serializable]
public class HeroStepRefObj : _IALBasicRefObj
{
    public long _refId { get { return step; } }
    public long step;//阶段
    public long level_limit;//骑士等级上限
    public List<NPCommonCostItem> cost_item_list;//升到下一阶消耗物品列表
}

public class GSOHeroStepRefSet : _TALSOBasicRefSet<HeroStepRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_step"; } }
}
