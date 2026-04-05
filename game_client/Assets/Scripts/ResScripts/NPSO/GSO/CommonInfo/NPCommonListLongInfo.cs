using System;
using System.Collections.Generic;

using ALPackage;
using GOE;
using NPEnum;


/// <summary>
/// 通用的List<List<long>>格式的可序列化信息，自动导出上用
/// </summary>
/// <typeparam name="E"></typeparam>
[System.Serializable]
public class NPCommonListLongInfo
{
    public List<long> value;
    
    /************
    * 读取字符串
    **/
    public static NPCommonListLongInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        NPCommonListLongInfo ret = new NPCommonListLongInfo();
        ret.value = new List<long>();
        
        for (int i = 0; i < strs.Length; i++)
        {
            ret.value.Add(long.Parse(strs[i]));
        }

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<NPCommonListLongInfo> readList(string _str)
    {
        List<NPCommonListLongInfo> list = new List<NPCommonListLongInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            NPCommonListLongInfo newItem = NPCommonListLongInfo.readFromStr(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        if(null == value)
            return String.Empty;
        
        return value.ToStringList();
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
        
        this.value = new List<long>();
        for (int i = 0; i < strs.Length; i++)
        {
            this.value.Add(long.Parse(strs[i]));
        }
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<NPCommonListLongInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static NPCommonListLongInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}