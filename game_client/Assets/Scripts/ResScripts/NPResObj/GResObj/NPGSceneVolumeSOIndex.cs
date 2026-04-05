
[System.Serializable]
public class NPGSceneVolumeSOIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"render_so/volume_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"volume_{mainId}_{subId}"; } }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}render_so/volume_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"volume_{_mainId}_{_subId}"; }
}
