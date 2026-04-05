using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// showCase舞台资源索引信息
/// </summary>
[System.Serializable]
public class NPGShowcaseIndex : BasicResIndexInfo
{
    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }

    public static NPGShowcaseIndex readIndexInfo(string _str, string _columnName = "")
    {
        //Debug.LogError(_str);
        NPGShowcaseIndex textureIndex = null;

        textureIndex = new NPGShowcaseIndex();
        textureIndex.readIndex(_str, _columnName);

        return textureIndex;
    }

    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"showcase/showcase_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"showcase_{mainId}_{subId}"; } }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}showcase/showcase_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"showcase_{_mainId}_{_subId}"; }
}
