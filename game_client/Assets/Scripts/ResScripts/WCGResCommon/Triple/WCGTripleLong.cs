using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WCGTripleLong
{
    [SerializeField]
    private WCGTriple<long, long, long> _m_triple = new WCGTriple<long, long, long>(0, 0, 0);

    public WCGTripleLong()
    {
    }

    public WCGTripleLong(long first, long second, long third)
    {
        _m_triple.first = first;
        _m_triple.second = second;
        _m_triple.third = third;
    }

    public long first()
    {
        return _m_triple.first;
    }

    public long second()
    {
        return _m_triple.second;
    }

    public long third()
    {
        return _m_triple.third;
    }

    public void setFirst(long _first)
    {
        _m_triple.first = _first;
    }

    public void setSecond(long _second)
    {
        _m_triple.second = _second;
    }

    public void setThird(long _third)
    {
        _m_triple.third = _third;
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
                    _m_triple.first = long.Parse(strs[0]);
            }
            if (strs.Length >= 2)
            {
                if (!string.IsNullOrEmpty(strs[1]))
                    _m_triple.second = long.Parse(strs[1]);
            }
            if (strs.Length >= 3)
            {
                if (!string.IsNullOrEmpty(strs[2]))
                    _m_triple.third = long.Parse(strs[2]);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"WCGTripleLong parse failed,{_str}\n{e}");
        }
    }
    public static WCGTripleLong readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        //需要支持物品类型没有配置的情况
        WCGTripleLong ret = new WCGTripleLong();
        try
        {
            if (strs.Length > 0)
                ret._m_triple.first = long.Parse(strs[0]);

            if (strs.Length > 1)
                ret._m_triple.second = long.Parse(strs[1]);

            if (strs.Length > 2)
                ret._m_triple.third = long.Parse(strs[2]);
        }
        catch (Exception e)
        {
            Debug.LogError($"解析WCGTripleLong时发生错误：{_str}\n{e}");
            return null;
        }

        return ret;
    }

    /// <summary>
    /// 读取队列
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static List<WCGTripleLong> readList(string _str)
    {
        List<WCGTripleLong> list = new List<WCGTripleLong>();
        if (null == _str || _str.Length <= 0)
            return list;

        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            WCGTripleLong newItem = WCGTripleLong.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }


    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<WCGTripleLong> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static WCGTripleLong[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }

    public override string ToString()
    {
        return _m_triple.ToString();
    }
}
