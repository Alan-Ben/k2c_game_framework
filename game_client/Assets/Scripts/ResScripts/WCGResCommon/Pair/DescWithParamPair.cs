using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 描述加参数解析
/// </summary>
[Serializable]
public class DescWithParamPair
{
    [SerializeField]
    private WCGPair<string, List<string>> _m_pair = new WCGPair<string, List<string>>(null,null);

    public DescWithParamPair()
    {

    }

    public DescWithParamPair(string first, List<string> second)
    {
        _m_pair.first = first;
        _m_pair.second = second;
    }

    public string first()
    {
        return _m_pair.first;
    }

    public List<string> second()
    {
        return _m_pair.second;
    }

    public void setSecond(List<string> _second)
    {
        _m_pair.second = _second;
    }

    public void setFirst(string _first)
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
                _m_pair.first = strs[0];
            }
            if (strs.Length >= 2)
            {
                _m_pair.second = new List<string>();
                for (int i = 1; i < strs.Length; i++)
                {
                    _m_pair.second.Add(strs[i]);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"WCGPairDescParam parse failed,{_str}\n{e}");
        }
    }
    public static DescWithParamPair readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        DescWithParamPair ret = new DescWithParamPair();
        try
        {
            if (strs.Length >= 1)
            {
                ret._m_pair.first = strs[0];
            }
            if (strs.Length >= 2)
            {
                ret._m_pair.second = new List<string>();
                for (int i = 1; i < strs.Length; i++)
                {
                    ret._m_pair.second.Add(strs[i]);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"解析WCGPairDescParam时发生错误：{_str}\n{e}");
            return null;
        }

        return ret;
    }

    /// <summary>
    /// 读取队列
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static List<DescWithParamPair> readList(string _str)
    {
        List<DescWithParamPair> list = new List<DescWithParamPair>();
        if (null == _str || _str.Length <= 0)
            return list;

        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            DescWithParamPair newItem = DescWithParamPair.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }


    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<DescWithParamPair> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static DescWithParamPair[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }

    public override string ToString()
    {
        return _m_pair.ToString();
    }
}
