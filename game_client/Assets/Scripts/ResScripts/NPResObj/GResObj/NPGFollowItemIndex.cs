using UnityEngine;
using System.Collections;


/// <summary>
/// 跟随UI的索引数据对象，使用ID索引，可以通过ID进行简单判重
/// </summary>
[System.Serializable]
public class NPGFollowItemIndex : BasicResIndexInfo
{
    /// <summary>
    /// 资源加载路径
    /// </summary>
    protected override string customAssetPath { get { return $"gui/follow_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"follow_{mainId}_{subId}"; } }

    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }

    /// <summary>
    /// 资源加载路径
    /// </summary>
    public static string GetAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}gui/follow_{_mainId}.unity3d"; }
    public static string GetObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"follow_{_mainId}_{_subId}"; }
}
