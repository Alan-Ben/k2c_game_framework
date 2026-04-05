using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


[System.Serializable]
public class NPGTextureIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"textures/tex_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"tex_{mainId}_{subId}"; } }

    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }

    public static NPGTextureIndex readIndexInfo(string _str, string _columnName = "")
    {
        //Debug.LogError(_str);
        NPGTextureIndex textureIndex = null;

        textureIndex = new NPGTextureIndex();
        textureIndex.readIndex(_str, _columnName);

        return textureIndex;
    }

    //数据是否有效
    public bool enable()
    {
        if (0 == mainId && 0 == subId)
            return false;

        return true;
    }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}textures/tex_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"tex_{_mainId}_{_subId}"; }
}
