using ALPackage;
using System;
using System.Collections.Generic;

/// <summary>
/// 骑士星辉表
/// </summary>
[Serializable]
public class HeroHaloRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//光环id
    public string halo_name;//光环名称
    public string desc;//光环描述
    public List<string> desc_args;//光环描述参数
    public List<WCGPairLong> halo_suit_skill_extra_level;//光环默认套系技能加成（未激活时也生效）
}

public class GSOHeroHaloRefSet : _TALSOBasicRefSet<HeroHaloRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_halo"; } }
}
