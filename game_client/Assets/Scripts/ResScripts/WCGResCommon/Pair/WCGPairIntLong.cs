using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WCGPairIntLong
{
    [SerializeField]
    [NotNull] private WCGPair<int, long> _m_pair = new WCGPair<int, long>(0, 0);

    public WCGPairIntLong()
    {

    }

    public WCGPairIntLong(int first, long second)
    {
        _m_pair.first = first;
        _m_pair.second = second;
    }

    public int first()
    {
        return _m_pair.first;
    }

    public long second()
    {
        return _m_pair.second;
    }

    public void setSecond(long _second)
    {
        _m_pair.second = _second;
    }

    public void setFirst(int _first)
    {
        _m_pair.first = _first;
    }

    public void ParseFromString(string _str)
    {
        if (string.IsNullOrEmpty(_str))
            return;

        try
        {
            string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length >= 1)
            {
                if (!string.IsNullOrEmpty(strs[0]))
                    _m_pair.first = int.Parse(strs[0]);
            }
            if (strs.Length >= 2)
            {
                if (!string.IsNullOrEmpty(strs[1]))
                    _m_pair.second = long.Parse(strs[1]);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"WCGPairIntLong  parse failed,{_str}\n{e}");
        }
    }
    public static WCGPairIntLong readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        //需要支持物品类型没有配置的情况
        WCGPairIntLong ret = new WCGPairIntLong();
        try
        {
            if (strs.Length > 0)
                ret._m_pair.first = int.Parse(strs[0]);

            if (strs.Length > 1)
                ret._m_pair.second = long.Parse(strs[1]);
        }
        catch (Exception e)
        {
            Debug.LogError($"解析 WCGPairIntLong 时发生错误：{_str}\n{e}");
            return null;
        }

        return ret;
    }

    public bool isDefault()
    {
        return _m_pair.first == 0 && _m_pair.second == 0;
    }

    /// <summary>
    /// 读取队列
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static List<WCGPairIntLong> readList(string _str)
    {
        List<WCGPairIntLong> list = new List<WCGPairIntLong>();
        if (null == _str || _str.Length <= 0)
            return list;

        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            WCGPairIntLong newItem = WCGPairIntLong.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }


    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<WCGPairIntLong> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static WCGPairIntLong[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }

    public override string ToString()
    {
        return _m_pair.ToString();
    }
}
