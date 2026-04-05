package NPCommon.Game.RangeWeight;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

/**
 * 范围随机类
 */
public class RangeRandom implements _IParseFromStringable
{
    private int _m_rangeStart;
    private int _m_rangeEnd;

    public RangeRandom()
    {
    }

    public RangeRandom(int _rangeStart, int _rangeEnd)
    {
        this._m_rangeStart = _rangeStart;
        this._m_rangeEnd = _rangeEnd;
    }

    public int getRangeStart()
    {
        return _m_rangeStart;
    }

    public int getRangeEnd()
    {
        return _m_rangeEnd;
    }

    /**
     * 是否在范围内
     * @param _value
     * @return
     */
    public boolean inRange(int _value)
    {
        return _m_rangeStart <= _value && _value <= _m_rangeEnd;
    }

    /**
     * 从范围内随机一个值
     * @return
     */
    public int random()
    {
        return (int) (Math.random() * (_m_rangeEnd - _m_rangeStart + 1)) + _m_rangeStart;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        String[] valueStr = sValue.split("-");
        if (valueStr.length < 2)
        {
            CommLog.error("RangeWeight read err! : " + sValue);
            return false;
        }

        try
        {
            this._m_rangeStart = Integer.parseInt(valueStr[0]);
            this._m_rangeEnd = Integer.parseInt(valueStr[1]);
        } catch (Exception e)
        {
            CommLog.error("RangeWeight read err! : " + sValue);
            return false;
        }

        return true;
    }
}
