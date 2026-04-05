using ALPackage;

[System.Serializable]
public class NPLoginWayRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long)login_type; } }

    public NPEnum.ENPLoginWayType login_type;//唯一识别标记
    public NPGTextureIndex icon_index;
    public string name;
}

public class NPGSOLoginWayRefSet : _TALSOBasicRefSet<NPLoginWayRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/login_ref.unity3d"; } }
    public static string objName { get { return "login_way"; } }
}
