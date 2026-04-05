package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;

/**************
 * 浮点型范围信息类
 **/
public class WCGFloatRange
{
    public static boolean inRange(float _value, float _min, float _max)
    {
        if (-1 >= _min && -1 >= _max)
            return true;

        if (-1 < _min && _value < _min)
            return false;

        if (-1 < _max && _value > _max)
            return false;

        return true;
    }

    private float _m_fMin;
    private float _m_fMax;

    public WCGFloatRange(float _min, float _max)
    {
        _m_fMin = _min;
        _m_fMax = _max;
    }

    public WCGFloatRange(String _minStr, String _maxStr)
    {
        _m_fMin = Float.parseFloat(_minStr);
        _m_fMax = Float.parseFloat(_maxStr);
    }

    public WCGFloatRange(String _str)
    {
        String[] strs = CommonFunc.charSplit(_str, ':');
        if (strs.length > 0)
            _m_fMin = Float.parseFloat(strs[0]);
        if (strs.length > 1)
            _m_fMax = Float.parseFloat(strs[1]);
    }

    /***************
     * 判断值是否在范围内
     **/
    public boolean inRange(float _value)
    {
        if (-1 >= _m_fMin && -1 >= _m_fMax)
            return true;

        if (-1 < _m_fMin && _value < _m_fMin)
            return false;

        if (-1 < _m_fMax && _value > _m_fMax)
            return false;

        return true;
    }
}
