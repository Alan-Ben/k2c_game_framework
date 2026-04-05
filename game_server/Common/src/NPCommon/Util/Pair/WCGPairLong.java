package NPCommon.Util.Pair;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class WCGPairLong implements _IParseFromStringable
{
    private WCGPair<Long, Long> _m_pair = new WCGPair<Long, Long>(0L, 0L);

    public WCGPairLong()
    {

    }

    public WCGPairLong(long first, long second)
    {
        _m_pair.first = first;
        _m_pair.second = second;
    }

    public long first()
    {
        return _m_pair.first;
    }

    public long second()
    {
        return _m_pair.second;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        return parseFromString(sValue, ':');
    }

    public boolean parseFromString(String sValue, char token)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, token, 2);
            if (strs.length >= 1)
            {
                if (!strs[0].isEmpty())
                    _m_pair.first = Long.parseLong(strs[0].trim());
            }
            if (strs.length >= 2)
            {
                if (!strs[1].isEmpty())
                    _m_pair.second = Long.parseLong(strs[1].trim());
            }
        } catch (Exception e)
        {
            CommLog.error("WCGPairLong  parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }

    public boolean isDefault()
    {
        return _m_pair.first == 0 && _m_pair.second == 0;
    }

    public static WCGPairLong fromString(String sValue)
    {
        return fromString(sValue, ':');
    }

    public static WCGPairLong fromString(String sValue, char token)
    {
        WCGPairLong pair = new WCGPairLong();
        if (!pair.parseFromString(sValue, token))
        {
            return null;
        }
        return pair;
    }

    public void setFirst(long _first)
    {
        _m_pair.first = _first;
    }

    public void setSecond(long _second)
    {
        _m_pair.second = _second;
    }

    public void incSecond(long _inc)
    {
        _m_pair.second += _inc;
    }

    @Override
    public String toString()
    {
        return _m_pair.toString();
    }

}