using UnityEngine;
using System.Collections;
using System;


[System.Serializable]
public class NPGSfxIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"sfx/sfx_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"sfx_{mainId}_{subId}"; } }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}sfx/sfx_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"sfx_{_mainId}_{_subId}"; }
}
