using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using Random = UnityEngine.Random;

/**************
 * 浮点型范围信息类
 **/
[System.Serializable]
public class WCGFloatRange
{
    public static bool inRange(float _value, float _min, float _max)
    {
        if (-1 >= _min && -1 >= _max)
            return true;

        if (-1 < _min && _value < _min)
            return false;

        if (-1 < _max && _value > _max)
            return false;

        return true;
    }

    [SerializeField]
    private float _m_fMin;
    [SerializeField]
    private float _m_fMax;

    public WCGFloatRange(float _min, float _max)
    {
        _m_fMin = _min;
        _m_fMax = _max;
    }
    public WCGFloatRange(string _minStr, string _maxStr)
    {
        _m_fMin = ALCommon.ParseFloat(_minStr);
        _m_fMax = ALCommon.ParseFloat(_maxStr);
    }
    public WCGFloatRange(string _str)
    {
        string[] strs = _str.Split(':');
        if (strs.Length > 0)
            _m_fMin = ALCommon.ParseFloat(strs[0]);
        if (strs.Length > 1)
            _m_fMax = ALCommon.ParseFloat(strs[1]);
    }
    
    /// <summary>
    /// 最小值
    /// </summary>
    public float min { get { return _m_fMin; } }
    /// <summary>
    /// 最大值
    /// </summary>
    public float max { get { return _m_fMax; } }

    /***************
     * 判断值是否在范围内
     **/
    public bool inRange(float _value)
    {
        if (-1 >= _m_fMin && -1 >= _m_fMax)
            return true;

        if (-1 < _m_fMin && _value < _m_fMin)
            return false;

        if (-1 < _m_fMax && _value > _m_fMax)
            return false;

        return true;
    }
    /// <summary>
    /// 纯数学上判断是否在范围内
    /// </summary>
    public bool inRangeMathmatically(float _value, bool _includeLeft = false, bool _includeRight = false)
    {
        if (_m_fMax < _m_fMin)
            return false;

        bool leftIn = _includeLeft ? _value >= _m_fMin : _value > _m_fMin;
        bool rightIn = _includeRight ? _value <= _m_fMax : _value < _m_fMax;
        return leftIn && rightIn;
    }
    /// <summary>
    /// 获取随机值
    /// </summary>
    public float getRandomValue()
    {
        return Random.Range(_m_fMin, _m_fMax);
    }

    public float clampValue(float _value)
    {
        return Mathf.Clamp(_value, _m_fMin, _m_fMax);
    }
}
