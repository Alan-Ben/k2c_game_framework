package NPGameRes.GameObjs.Battle;

import NPCommon.Util.Mathf;
import ResCommon.Allocator._IResAllocator;

public class WCGFloatValue
{
    private static WCGFloatValue _g_fZero = new WCGFloatValue(0);

    public static WCGFloatValue zero()
    {
        return _g_fZero;
    }

    public WCGFloatValue()
    {
        _m_iValue = 0;
    }

    //从放大值直接生成对象
    public static WCGFloatValue makeFromValue(int _value, _IResAllocator _alloc)
    {
        WCGFloatValue v = _alloc.newFloatValue(_value);
        v._m_iValue = _value;

        return v;
    }

    public static WCGFloatValue makeFromValue(long _value, _IResAllocator _alloc)
    {
        WCGFloatValue v = _alloc.newFloatValue(_value);
        v._m_iValue = (int) _value;

        return v;
    }

    private int _m_iValue;

    public int v()
    {
        return _m_iValue;
    }

    public void setV(int value)
    {
        _m_iValue = value;
    }

    public float fV()
    {
        return _m_iValue / 100f;
    }

    public int iV()
    {
        return _m_iValue / 100;
    }

    public int roundIV()
    {
        return (_m_iValue + 50) / 100;
    }

    public WCGFloatValue(float _value)
    {
        _m_iValue = Mathf.CeilToInt(_value * 100);
    }

    public WCGFloatValue(int _value)
    {
        _m_iValue = _value * 100;
    }

    public WCGFloatValue(WCGFloatValue _value)
    {
        _m_iValue = _value._m_iValue;
    }

    public boolean equal(WCGFloatValue _value)
    {
        if (_m_iValue == _value._m_iValue)
            return true;

        else return false;
    }

    public boolean equal(int _value)
    {
        if (_m_iValue == _value * 100)
            return true;

        else return false;
    }

    public WCGFloatValue add(WCGFloatValue _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_m_iValue + _value._m_iValue, _alloc);
    }

    public WCGFloatValue add(int _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_m_iValue + (_value * 100), _alloc);
    }

    public WCGFloatValue sub(WCGFloatValue _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_m_iValue - _value._m_iValue, _alloc);
    }

    public WCGFloatValue sub(int _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_m_iValue - (_value * 100), _alloc);
    }

    public WCGFloatValue subed(WCGFloatValue _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_value._m_iValue - _m_iValue, _alloc);
    }

    public WCGFloatValue subed(int _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue((_value * 100) - _m_iValue, _alloc);
    }

    public WCGFloatValue mul(WCGFloatValue _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_m_iValue * _value._m_iValue / 100, _alloc);
    }

    public WCGFloatValue mul(int _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_m_iValue * _value, _alloc);
    }

    public WCGFloatValue div(WCGFloatValue _value, _IResAllocator _alloc)
    {
        if (_value._m_iValue == 0)
            return WCGFloatValue.makeFromValue(_m_iValue * 100, _alloc);

        return WCGFloatValue.makeFromValue(_m_iValue * 100 / _value._m_iValue, _alloc);
    }

    public WCGFloatValue div(int _value, _IResAllocator _alloc)
    {
        if (_value == 0)
            return WCGFloatValue.makeFromValue(_m_iValue * 100, _alloc);

        return WCGFloatValue.makeFromValue(_m_iValue / _value, _alloc);
    }

    public WCGFloatValue dived(WCGFloatValue _value, _IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_value._m_iValue * 100 / _m_iValue, _alloc);
    }

    public WCGFloatValue dived(int _value, _IResAllocator _alloc)
    {
        if (_m_iValue == 0)
            return WCGFloatValue.makeFromValue(_value * 10000, _alloc);

        return WCGFloatValue.makeFromValue(_value * 10000 / _m_iValue, _alloc);
    }

    public void setValue(float _value)
    {
        _m_iValue = Mathf.CeilToInt(_value * 100);
    }

    public void setVritualValue(int _value)
    {
        _m_iValue = _value;
    }

    public void setValue(int _value)
    {
        _m_iValue = _value * 100;
    }

    public void setValue(WCGFloatValue _value)
    {
        _m_iValue = _value._m_iValue;
    }

    /**
     * 平方处理
     */
    public WCGFloatValue pow2(_IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(_m_iValue * _m_iValue / 100, _alloc);
    }

    //裁剪到0-1区域内
    public WCGFloatValue Clamp01(_IResAllocator _alloc)
    {
        if (_m_iValue < 0)
            return _alloc.newFloatValue();
        else
            return WCGFloatValue.makeFromValue(Math.min(_m_iValue, 100), _alloc);
    }

    /**
     * 获取平方根
     */
    public WCGFloatValue sqrt(_IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(DLCommon.toInt(Mathf.Sqrt(_m_iValue / 100f) * 100f), _alloc);
    }

    /**
     * 获取平方根
     */
    public WCGFloatValue abs(_IResAllocator _alloc)
    {
        return WCGFloatValue.makeFromValue(Math.abs(_m_iValue), _alloc);
    }
}