using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


[System.Serializable]
public class NPGSpriteIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"sprites/spt_{mainId}.unity3d"; } }
    protected override string customObjName { get { return $"spt_{mainId}_{subId}"; } }


    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }

    public static NPGSpriteIndex readIndexInfo(string _str, string _columnName = "")
    {
        //Debug.LogError(_str);
        NPGSpriteIndex spriteIndex = null;

        spriteIndex = new NPGSpriteIndex();
        spriteIndex.readIndex(_str, _columnName);

        return spriteIndex;
    }
    public static List<NPGSpriteIndex> readIndexList(string _str, string _columnName = "")
    {
        List<NPGSpriteIndex> spriteIndexList = new List<NPGSpriteIndex>();
        //Debug.LogError(_str);
        NPGSpriteIndex spriteIndex = null;
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; ++i)
        {
            spriteIndex = new NPGSpriteIndex();
            spriteIndex.readIndex(strs[i], _columnName);
            spriteIndexList.Add(spriteIndex);
        }
        return spriteIndexList;
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
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}sprites/spt_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"spt_{_mainId}_{_subId}"; }
}
