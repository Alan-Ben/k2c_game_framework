
/// <summary>
/// 灯光环境Go索引
/// </summary>
[System.Serializable]
public class GLightGoIndex : BasicResIndexInfo
{
    
    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }
    
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"light_go/light_go_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"light_go_{mainId}_{subId}"; } }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}light_go/light_go_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"light_go_{_mainId}_{_subId}"; }
}
