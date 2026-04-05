using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

/// <summary>
/// 通用的值对应int的可序列化信息
/// </summary>
[System.Serializable]
public class CommonIntLongInfo
{
    public int intValue;
    public long longValue;

    /// <summary>
    /// 读取字符串
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static CommonIntLongInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        CommonIntLongInfo ret = new CommonIntLongInfo();

        ret.intValue = int.Parse(strs[0]);
        ret.longValue = long.Parse(strs[1]);

        return ret;
    }


    /// <summary>
    /// 读取队列
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static List<CommonIntLongInfo> readList(string _str)
    {
        List<CommonIntLongInfo> list = new List<CommonIntLongInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            CommonIntLongInfo newItem = CommonIntLongInfo.readFromStr(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", intValue, longValue);
    }
    
    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { "-", ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return;
        }

        //需要支持物品类型没有配置的情况
        this.intValue = int.Parse(strs[0]);    
        this.longValue = long.Parse(strs[1]);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<CommonIntLongInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static CommonIntLongInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}