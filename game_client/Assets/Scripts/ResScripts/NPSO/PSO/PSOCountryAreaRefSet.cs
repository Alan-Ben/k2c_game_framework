using ALPackage;
using System;

/// <summary>
/// 国家大区表
/// </summary>
[Serializable]
public class CountryAreaRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一识别标记
    public string area_code;//国家地区代码
    public string continent_code;//大洲代码
    public string region_code;//大区代码（用于CDN）
}

public class PSOCountryAreaRefSet : _TALSOBasicRefSet<CountryAreaRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/plat_refdata.unity3d"; } }
    public static string objName { get { return "country_area"; } }
}
