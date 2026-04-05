
using System;
using System.Collections.Generic;
using ALPackage;
using Common.PlayerEnum;
using NPEnum;

[System.Serializable]
public class NPMuseumItemRecordInfo
{
    public EPlayerEventRecordType recordType;
    public long id;
    
    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":"}, StringSplitOptions.RemoveEmptyEntries);
        if(strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return;
        }

        this.recordType = (EPlayerEventRecordType)ALCommon.EnumParse(typeof(EPlayerEventRecordType), strs[0], true);
        this.id = strs.Length > 1 ? long.Parse(strs[1]) : 0;

    }
    
    /************
    * 读取字符串
    **/
    public static NPMuseumItemRecordInfo readFromString(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 计数类型!");
            return null;
        }

        NPMuseumItemRecordInfo ret = new NPMuseumItemRecordInfo();

        ret.recordType = (EPlayerEventRecordType)ALCommon.EnumParse(typeof(EPlayerEventRecordType), strs[0], true);
        ret.id = strs.Length > 1 ? long.Parse(strs[1]) : 0;

        return ret;
    }
    /************
     * 读取队列
     **/
    public static List<NPMuseumItemRecordInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }
    public static List<NPMuseumItemRecordInfo> readList(string _str)
    {
        List<NPMuseumItemRecordInfo> list = new List<NPMuseumItemRecordInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            NPMuseumItemRecordInfo newItem = NPMuseumItemRecordInfo.readFromString(strs[i]);
            if(null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }
    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static NPMuseumItemRecordInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}