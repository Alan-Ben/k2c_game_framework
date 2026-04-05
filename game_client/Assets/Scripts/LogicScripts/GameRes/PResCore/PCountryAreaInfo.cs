using ALPackage;
using JetBrains.Annotations;

/// <summary>
/// 国家大区表对象
/// </summary>
public class PCountryAreaInfo : _ATALBasicSingleAssetObj<PSOCountryAreaRefSet>
{
    private static PCountryAreaInfo _g_instance = new PCountryAreaInfo();
    [NotNull]
    public static PCountryAreaInfo instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new PCountryAreaInfo();
            return _g_instance;
        }
    }

    /** 获取资源管理对象 */
    protected override _AALResourceCore _resCore { get { return PlatResCore.instance; } }
    /** 获取加载路径 */
    protected override string _resPath { get { return PSOCountryAreaRefSet.assetPath; } }
    /** 获取加载对象名 */
    protected override string _objName { get { return PSOCountryAreaRefSet.objName; } }
}
