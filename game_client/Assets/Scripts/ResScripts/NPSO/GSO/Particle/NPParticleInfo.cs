using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;


/********************
 * 信息结构体
 **/
[System.Serializable]
public class NPParticleInfo
{
    public long particleId;//粒子id
    public int num;//数量

    public NPParticleInfo()
    {

    }
    public NPParticleInfo(NPParticleInfo _item)
    {
        particleId = _item.particleId;
        num = _item.num;
    }

    /************
    * 读取字符串
    **/
    public static NPParticleInfo readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        NPParticleInfo ret = new NPParticleInfo();

        ret.particleId = long.Parse(strs[0]);
        ret.num = int.Parse(strs[1]);

        return ret;
    }

    /************
     * 读取队列
     **/
    public static List<NPParticleInfo> readList(string _str)
    {
        List<NPParticleInfo> list = new List<NPParticleInfo>();
        if (null == _str || _str.Length <= 0)
            return list;

        //string[] strs = _str.Split('|');
        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            NPParticleInfo newItem = NPParticleInfo.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", particleId, num);
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

        this.particleId = long.Parse(strs[0]);
        this.num = int.Parse(strs[1]);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<NPParticleInfo> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static NPParticleInfo[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }
}