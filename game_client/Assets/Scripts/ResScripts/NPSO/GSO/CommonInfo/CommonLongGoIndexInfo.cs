using System;
using System.Collections.Generic;

/// <summary>
/// 通用的值对应NPGGoIndex格式的可序列化信息
/// </summary>
[System.Serializable]
public class CommonLongGoIndexInfo
{
    public long value;
    public NPGGoIndex goIndex;

    /// <summary>
    /// 读取字符串
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static CommonLongGoIndexInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        CommonLongGoIndexInfo ret = new CommonLongGoIndexInfo();

        ret.value = long.Parse(strs[0]);
        ret.goIndex = new NPGGoIndex();
        ret.goIndex.readIndex(strs[1]);

        return ret;
    }


    /// <summary>
    /// 读取队列
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static List<CommonLongGoIndexInfo> readList(string _str)
    {
        List<CommonLongGoIndexInfo> list = new List<CommonLongGoIndexInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            CommonLongGoIndexInfo newItem = CommonLongGoIndexInfo.readFromStr(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", value, goIndex);
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
        this.value = long.Parse(strs[0]);
        this.goIndex = new NPGGoIndex();
        this.goIndex.readIndex(strs[1]);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<CommonLongGoIndexInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static CommonLongGoIndexInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}