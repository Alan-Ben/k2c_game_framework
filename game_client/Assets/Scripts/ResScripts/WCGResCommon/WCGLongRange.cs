using System;
using System.Collections.Generic;
using UnityEngine;

/**************
 * 长整型范围信息类
 **/
[Serializable]
public class WCGLongRange
{
    public static bool inRange(long _value, long _min, long _max)
    {
        if (-1 == _min && -1 == _max)
            return true;

        if (-1 != _min && _value < _min)
            return false;

        if (-1 != _max && _value > _max)
            return false;

        return true;
    }

    [SerializeField]
    private long _m_lMin;
    [SerializeField]
    private long _m_lMax;

    public WCGLongRange()
    {
    }
    public WCGLongRange(long _min, long _max)
    {
        _m_lMin = _min;
        _m_lMax = _max;
    }
    public WCGLongRange(string _minStr, string _maxStr)
    {
        _m_lMin = long.Parse(_minStr);
        _m_lMax = long.Parse(_maxStr);
    }
    public WCGLongRange(string _str)
    {
        ParseFromString(_str);
    }
    public long max { get { return _m_lMax; } }
    public long min { get { return _m_lMin; } }

    //获取最大最小差值
    public long getMargin()
    {
        return _m_lMax - _m_lMin;
    }
    /***************
     * 判断值是否在范围内
     **/
    public bool inRange(long _value)
    {
        if (-1 == _m_lMin && -1 == _m_lMax)
            return true;

        if (-1 != _m_lMin && _value < _m_lMin)
            return false;

        if (-1 != _m_lMax && _value > _m_lMax)
            return false;

        return true;
    }

    public long Clamp(long _value)
    {
        if (_value < min)
            _value = min;
        else if (_value > max)
            _value = max;
        return _value;
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public void ParseFromString(string _str)
    {
        if (string.IsNullOrEmpty(_str))
            return;

        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
        if (strs.Length > 0)
            _m_lMin = int.Parse(strs[0]);
        if (strs.Length > 1)
            _m_lMax = int.Parse(strs[1]);
    }

    public static WCGLongRange readFromStr(string _str)
    {
        //拆分字符串后进行读取
        string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

        if (strs.Length < 1)
        {
            UnityEngine.Debug.LogWarning("没有配置 对象名!");
            return null;
        }

        //需要支持物品类型没有配置的情况
        WCGLongRange ret = new WCGLongRange();
        try
        {
            if (strs.Length > 0)
                ret._m_lMin = int.Parse(strs[0]);

            if (strs.Length > 1)
                ret._m_lMax = int.Parse(strs[1]);
        }
        catch (Exception e)
        {
            Debug.LogError($"解析WCGLongRange时发生错误：{_str}\n{e}");
            return null;
        }

        return ret;
    }

    /// <summary>
    /// 读取队列
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static List<WCGLongRange> readList(string _str)
    {
        List<WCGLongRange> list = new List<WCGLongRange>();
        if (null == _str || _str.Length <= 0)
            return list;

        string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < strs.Length; i++)
        {
            WCGLongRange newItem = WCGLongRange.readFromStr(strs[i]);
            if (null == newItem)
                continue;

            list.Add(newItem);
        }
        return list;
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static List<WCGLongRange> MakeListFromString(string _str)
    {
        return readList(_str);
    }

    /// <summary>
    /// 为自动导出写的
    /// </summary>
    public static WCGLongRange[] MakeArrayFromString(string _str)
    {
        return readList(_str).ToArray();
    }

    public override string ToString()
    {
        return "[" + min + "-" + max + "]";
    }
}
