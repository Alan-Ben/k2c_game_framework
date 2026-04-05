package NPCommon.Util;


/**
 * 浮动类型值
 */
public class WCGRangeValue
{
    private int _m_iMin;   //最小值
    private int _m_iMax;   //最大值

    public int min()
    {
        return _m_iMin;
    }

    public int max()
    {
        return _m_iMax;
    }

    //value min:max
    public WCGRangeValue(String value)
    {
        String[] strs = CommonFunc.charSplit(value, ':');
        if (strs.length == 2)
        {
            _m_iMin = Integer.parseInt(strs[0]);
            _m_iMax = Integer.parseInt(strs[1]);
        }
    }

    public WCGRangeValue(int min, int max)
    {
        _m_iMin = min;
        _m_iMax = max;
    }

    public int rand()
    {
        int delta = _m_iMax - _m_iMin;
        if (delta > 0)
        {
            return _m_iMin + CommonFunc.randomInt(delta);
        } else
        {
            return _m_iMin;
        }
    }
}