using ALPackage;
using NPEnum;
using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

/// <summary>
/// 刷新配置信息
/// </summary>
[Serializable]
public class NPTimeRefreshInfo
{
    public ENPTimeRefreshType refreshType;//刷新类型
    public List<int> paramList;//参数列表


    /************
    * 读取字符串
    **/
    public static NPTimeRefreshInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { "-", ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        NPTimeRefreshInfo ret = new NPTimeRefreshInfo();

        ret.refreshType = (ENPTimeRefreshType)ALCommon.EnumParse(typeof(ENPTimeRefreshType), strs[0], true);
        ret.paramList = new List<int>();
        for (int i = 1; i < strs.Length; i++)
        {
            ret.paramList.Add(int.Parse(strs[i]));
        }

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<NPTimeRefreshInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    public static List<NPTimeRefreshInfo> readList(string _str)
    {
        List<NPTimeRefreshInfo> list = new List<NPTimeRefreshInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            NPTimeRefreshInfo newItem = NPTimeRefreshInfo.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", refreshType, paramList);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return;
        }

        this.refreshType = (ENPTimeRefreshType)ALCommon.EnumParse(typeof(ENPTimeRefreshType), strs[0], true);
        this.paramList = new List<int>();
        for (int i = 1; i < strs.Length; i++)
        {
            this.paramList.Add(int.Parse(strs[i]));
        }
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static NPTimeRefreshInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}
