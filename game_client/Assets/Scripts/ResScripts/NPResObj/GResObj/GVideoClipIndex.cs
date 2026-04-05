using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class GVideoClipIndex : BasicResIndexInfo
{
    public GVideoClipIndex()
    {

    }
    public GVideoClipIndex(int _mainId, int _subId)
        : base(_mainId, _subId)
    {
    }
    public void ParseFromString(string _str)
    {
        readIndex(_str, string.Empty);
    }

    public static GVideoClipIndex readFromStr(string _str)
    {
        GVideoClipIndex info = new GVideoClipIndex();
        info.ParseFromString(_str);
        return info;
    }

    public static List<GVideoClipIndex> MakeListFromString(string _str)
    {
        return readList(_str);
    }
    public static List<GVideoClipIndex> readList(string _str)
    {
        List<GVideoClipIndex> list = new List<GVideoClipIndex>();
        if (null == _str || _str.Length <= 0)
            return list;

        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            GVideoClipIndex newItem = new GVideoClipIndex();
            newItem.readIndex(strs[i]);
            list.Add(newItem);
        }
        return list;
    }
    /************
     * 资源加载路径
     **/
    protected override string customAssetPath
    {
        get
        {
#if UNITY_IOS
            return $"{mainId}/vc_{mainId}_{subId}.mp4";
#elif UNITY_ANDROID
            return $"{mainId}/vc_{mainId}_{subId}.gobvc";
#else
            return $"{mainId}/vc_{mainId}_{subId}.mp4";
#endif
        }
    }

    protected override string customObjName { get { return $"vc_{mainId}_{subId}"; } }



    /***************
     * 根据主id和副id获取对应的资源路径
     **/
    public static string getAssetPath(int _mainId, EIndexType _indexType = EIndexType.DEFAULT) { return $"{_indexType.assetPathRoot()}video_clip/vc_{_mainId}.unity3d"; }
    public static string getObjName(int _mainId, int _subId, EIndexType _indexType = EIndexType.DEFAULT) { return $"vc_{_mainId}_{_subId}"; }
}
