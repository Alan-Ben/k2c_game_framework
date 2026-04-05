using UnityEngine;
using System.Collections;


[System.Serializable]
public class NPGModelIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"actors/act_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"act_{mainId}_{subId}"; } }

    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }
    /************
     * 资源加载路径
     **/

    public static string GetAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}actors/act_{_mainId}.unity3d"; }
    public static string GetObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"act_{_mainId}_{_subId}"; }
}
