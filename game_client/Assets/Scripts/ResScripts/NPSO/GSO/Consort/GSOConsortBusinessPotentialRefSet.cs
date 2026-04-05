using ALPackage;

/// <summary>
/// 妃子经营天赋
/// </summary>
[System.Serializable]
public class ConsortBusinessPotentialRefObj : _IALBasicRefObj
{
    public long _refId { get { return potential_lvl; } }

    public int potential_lvl;
}

public class GSOConsortBusinessPotentialRefSet : _TALSOBasicRefSet<ConsortBusinessPotentialRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_business_potential"; } }
}