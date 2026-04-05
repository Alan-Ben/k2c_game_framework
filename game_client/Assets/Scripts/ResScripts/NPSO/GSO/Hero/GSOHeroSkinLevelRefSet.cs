using ALPackage;
using System;
using System.Collections.Generic;
using GOE;

/// <summary>
/// 骑士皮肤等级表
/// </summary>
[Serializable]
public class HeroSkinLevelRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一识别标记
    public long skin_id;//皮肤id
    public long skin_level;//皮肤等级
    public NPCommonCostItem upgrade_cost;//升级道具（CommonCostItem）
    public string desc;//效果描述
    public List<string> desc_args;//效果描述参数
}

public class GSOHeroSkinLevelRefSet : _TALSOBasicRefSet<HeroSkinLevelRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_skin_level"; } }
}
