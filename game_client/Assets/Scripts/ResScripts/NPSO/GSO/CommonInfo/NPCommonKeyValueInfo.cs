using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;


/// <summary>
/// 通用的K,V格式的可序列化信息，自动导出上用
/// </summary>
/// <typeparam name="E"></typeparam>
[System.Serializable]
public class NPCommonKeyValueInfo
{
    public long key;
    public long value;
    
    /************
    * 读取字符串
    **/
    public static NPCommonKeyValueInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        NPCommonKeyValueInfo ret = new NPCommonKeyValueInfo();

        ret.key = long.Parse(strs[0]);
        ret.value = long.Parse(strs[1]);

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<NPCommonKeyValueInfo> readList(string _str)
    {
        List<NPCommonKeyValueInfo> list = new List<NPCommonKeyValueInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            NPCommonKeyValueInfo newItem = NPCommonKeyValueInfo.readFromStr(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", key, value);
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
        this.key = long.Parse(strs[0]);
        this.value = long.Parse(strs[1]);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<NPCommonKeyValueInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static NPCommonKeyValueInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}