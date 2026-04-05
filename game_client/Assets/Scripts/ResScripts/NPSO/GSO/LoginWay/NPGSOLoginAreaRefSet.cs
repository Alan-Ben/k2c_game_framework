using ALPackage;
using System.Collections.Generic;

[System.Serializable]
public class NPLoginAreaRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long)id; } }

    public long id;
    public string tag;//cdn唯一识别标记
    public NPGSpriteIndex icon_index;//大区图标
    public string name;//大区名字
}

public class NPGSOLoginAreaRefSet : _TALSOBasicRefSet<NPLoginAreaRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/login_ref.unity3d"; } }
    public static string objName { get { return "login_area"; } }
}
