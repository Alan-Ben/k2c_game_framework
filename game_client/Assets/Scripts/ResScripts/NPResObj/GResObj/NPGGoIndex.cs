using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class NPGGoIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"go/go_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"go_{mainId}_{subId}"; } }

    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }
    public static List<NPGGoIndex> MakeListFromString(string _str)
    {
        return readList(_str);
    }
    public static List<NPGGoIndex> readList(string _str)
    {
        List<NPGGoIndex> list = new List<NPGGoIndex>();
        if (null == _str || _str.Length <= 0)
            return list;

        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            NPGGoIndex newItem = new NPGGoIndex();
            newItem.readIndex(strs[i]);
            list.Add(newItem);
        }
        return list;
    }
    /************
     * 资源加载路径
     **/

    public static string GetAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}go/go_{_mainId}.unity3d"; }
    public static string GetObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"go_{_mainId}_{_subId}"; }
}
