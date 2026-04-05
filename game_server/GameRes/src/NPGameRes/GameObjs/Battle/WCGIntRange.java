package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;

public class WCGIntRange
{
    public static boolean inRange(long _value, long _min, long _max)
    {
        if (-1 == _min && -1 == _max)
            return true;

        if (-1 != _min && _value < _min)
            return false;

        if (-1 != _max && _value > _max)
            return false;

        return true;
    }

    public static boolean inRange(int _value, int _min, int _max)
    {
        if (-1 == _min && -1 == _max)
            return true;

        if (-1 != _min && _value < _min)
            return false;

        if (-1 != _max && _value > _max)
            return false;

        return true;
    }


    private int _m_iMin;
    private int _m_iMax;

    public WCGIntRange(int _min, int _max)
    {
        _m_iMin = _min;
        _m_iMax = _max;
    }

    public WCGIntRange(String _minStr, String _maxStr)
    {
        _m_iMin = Integer.parseInt(_minStr.trim());
        _m_iMax = Integer.parseInt(_maxStr.trim());
    }

    public WCGIntRange(String _str)
    {
        String[] strs = CommonFunc.charSplit(_str, ':');
        if (strs.length > 0)
            _m_iMin = Integer.parseInt(strs[0].trim());
        if (strs.length > 1)
            _m_iMax = Integer.parseInt(strs[1].trim());
    }


    //提前判断结果是否正确
    public boolean preJudgeAddRes(int _num)
    {
        if (-1 == _m_iMin && -1 == _m_iMax)
            return true;

        if (-1 == _m_iMax && _num >= _m_iMin)
            return true;

        return false;
    }

    public boolean preJudgeRedRes(int _num)
    {
        if (-1 == _m_iMin && -1 == _m_iMax)
            return true;

        if (-1 == _m_iMin && _num <= _m_iMax)
            return true;

        return false;
    }


    /***************
     * 判断值是否在范围内
     **/
    public boolean inRange(int _value)
    {
        if (-1 == _m_iMin && -1 == _m_iMax)
            return true;

        if (-1 != _m_iMin && _value < _m_iMin)
            return false;

        if (-1 != _m_iMax && _value > _m_iMax)
            return false;

        return true;
    }

    public boolean inRange(long _value)
    {
        if (-1 == _m_iMin && -1 == _m_iMax)
            return true;

        if (-1 != _m_iMin && _value < _m_iMin)
            return false;

        if (-1 != _m_iMax && _value > _m_iMax)
            return false;

        return true;
    }

    public String toString()
    {
        return "[" + _m_iMin + "-" + _m_iMax + "]";
    }
}
