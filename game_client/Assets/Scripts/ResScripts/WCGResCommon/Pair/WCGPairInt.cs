using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WCGPairInt
{
    [SerializeField]
    private WCGPair<int, int> _m_pair = new WCGPair<int, int>(0, 0);

    public WCGPairInt()
    {

    }

    public WCGPairInt(int first, int second)
    {
        _m_pair.first = first;
        _m_pair.second = second;
    }

    public int first()
    {
        return _m_pair.first;
    }

    public int second()
    {
        return _m_pair.second;
    }

    public void setSecond(int _second)
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
                    _m_pair.second = int.Parse(strs[1]);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"WCGPairLong  parse failed,{_str}\n{e}");
        }
    }
    public static WCGPairInt readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        //需要支持物品类型没有配置的情况
        WCGPairInt ret = new WCGPairInt();
        try
        {
            if (strs.Length > 0)
                ret._m_pair.first = int.Parse(strs[0]);

            if (strs.Length > 1)
                ret._m_pair.second = int.Parse(strs[1]);
        }
        catch (Exception e)
        {
            Debug.LogError($"解析WCGPairInt时发生错误：{_str}\n{e}");
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
    public static List<WCGPairInt> readList(string _str)
    {
        List<WCGPairInt> list = new List<WCGPairInt>();
        if (null == _str || _str.Length <= 0)
            return list;

        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            WCGPairInt newItem = WCGPairInt.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }


    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<WCGPairInt> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static WCGPairInt[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }

    public override string ToString()
    {
        return _m_pair.ToString();
    }

    public int rand()
    {
        int delta = _m_pair.second - _m_pair.first;
        if (delta > 0)
        {
            return _m_pair.first + Random.Range(0, delta + 1);
        }
        else
        {
            return _m_pair.first;
        }
    }
    
    /**
 * 把传入值限制在范围内
 */
    public int limit(double _value)
    {
        if (_value < _m_pair.first)
            return _m_pair.first;
        if (_value > _m_pair.second)
            return _m_pair.second;
        return (int) _value;
    }
}
