using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


[System.Serializable]
public class NPPSpriteIndex : BasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath { get { return $"p_sprites/p_spt_{mainId}"; } }
    protected override string customObjName { get { return $"p_spt_{mainId}_{subId}"; } }


    public static List<NPPSpriteIndex> readIndexList(string _str, string _columnName = "")
    {
        List<NPPSpriteIndex> spriteIndexList = new List<NPPSpriteIndex>();
        //Debug.LogError(_str);
        NPPSpriteIndex spriteIndex = null;
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; ++i)
        {
            spriteIndex = new NPPSpriteIndex();
            spriteIndex.readIndex(strs[i], _columnName);
            spriteIndexList.Add(spriteIndex);
        }
        return spriteIndexList;
    }

    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}p_sprites/p_spt_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"p_spt_{_mainId}_{_subId}"; }
}
