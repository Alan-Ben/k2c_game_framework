package NPCommon.Util;

public class WCGMatchRange
{
    private float _m_iMin;
    private float _m_iMax;

    public WCGMatchRange(WCGMatchRange _range)
    {
        _m_iMin = _range._m_iMin;
        _m_iMax = _range._m_iMax;
    }

    public WCGMatchRange(float _min, float _max)
    {
        _m_iMin = _min;
        _m_iMax = _max;
    }

    public WCGMatchRange()
    {
    }

    public float getMin()
    {
        return _m_iMin;
    }

    public float getMax()
    {
        return _m_iMax;
    }

    public float getMidV()
    {
        return (_m_iMax + _m_iMin) / 2f;
    }

    public void setRange(float _min, float _max)
    {
        _m_iMin = _min;
        _m_iMax = _max;
    }

    //    /是否在范围内
    public boolean inRange(float _value)
    {
        if (_value > _m_iMax)
            return false;
        if (_value < _m_iMin)
            return false;

        return true;
    }

    //    public WCGMatchRange getMixRange(final WCGMatchRange  _other)
//    {
//        if(!isInterSect(_other))
//            return null;
//        
//        return new WCGMatchRange(Math.max(_m_iMin, _other._m_iMin), Math.min(_m_iMax, _other._m_iMax));
//    }
    public boolean isInterSect(final WCGMatchRange _other)
    {
        if (null == _other)
            return false;

        return !(_m_iMax < _other._m_iMin || _other._m_iMax < _m_iMin);
    }

    public boolean isDualInterSect(final WCGMatchRange _other)
    {
        if (null == _other)
            return false;

        float midV = _other.getMidV();
        if (!inRange(midV))
            return false;

        midV = getMidV();
        if (!_other.inRange(midV))
            return false;

        return !(_m_iMax < _other._m_iMin || _other._m_iMax < _m_iMin);
    }

    @Override
    public String toString()
    {
        return String.format("[min:%f  max:%f] ", _m_iMin, _m_iMax);
    }

    //扩展上下限
    public void extend(float _newValue)
    {
        if (_newValue < _m_iMin)
        {
            _m_iMin = _newValue;
        } else
        {
            if (_newValue > _m_iMax)
            {
                _m_iMax = _newValue;
            }
        }

    }

    //扩展上下限2
    public void extend(WCGMatchRange _otherRange)
    {
        if (_otherRange._m_iMin < _m_iMin)
        {
            _m_iMin = _otherRange._m_iMin;
        }
        if (_otherRange._m_iMax > _m_iMax)
        {
            _m_iMax = _otherRange._m_iMax;
        }

    }

    //最大距离
    public float maxDistance(WCGMatchRange cardPowerRange)
    {
        float dis1 = Math.abs(this._m_iMax - cardPowerRange._m_iMin);
        float dis2 = Math.abs(this._m_iMin - cardPowerRange._m_iMax);
        return Math.max(dis1, dis2);
    }

    public float maxDistance(float _value)
    {
        float dis1 = Math.abs(this._m_iMax - _value);
        float dis2 = Math.abs(this._m_iMin - _value);
        return Math.max(dis1, dis2);
    }
}