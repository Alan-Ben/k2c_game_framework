
[System.Serializable]
public class NPGSceneWindSOIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"render_so/wind_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"wind_{mainId}_{subId}"; } }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}render_so/wind_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"wind_{_mainId}_{_subId}"; }
}
