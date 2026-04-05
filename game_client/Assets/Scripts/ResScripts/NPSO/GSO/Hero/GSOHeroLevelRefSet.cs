using ALPackage;
using System;

/// <summary>
/// 伙伴等级表
/// </summary>
[Serializable]
public class HeroLevelRefObj : _IALBasicRefObj
{
    public long _refId { get { return level; } }
    public long level;//唯一识别标记
    public long need_exp; //升到下一等级需要经验
    public long level_ratio;//等级系数
}

public class GSOHeroLevelRefSet : _TALSOBasicRefSet<HeroLevelRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_level"; } }
}
