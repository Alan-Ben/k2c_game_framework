package NPCommon.Util.Pair;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class WCGPairInteger implements _IParseFromStringable
{
    private WCGPair<Integer, Integer> _m_pair = new WCGPair<Integer, Integer>(0, 0);

    public WCGPairInteger()
    {

    }

    public WCGPairInteger(int _first, int _second)
    {
        _m_pair.first = _first;
        _m_pair.second = _second;
    }

    public int first()
    {
        return _m_pair.first;
    }

    public int second()
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
                    _m_pair.first = Integer.parseInt(strs[0].trim());
            }
            if (strs.length >= 2)
            {
                if (!strs[1].isEmpty())
                    _m_pair.second = Integer.parseInt(strs[1].trim());
            }
        } catch (Exception e)
        {
            CommLog.error("WCGPairInteger  parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }

    public boolean isDefault()
    {
        return _m_pair.first == 0 && _m_pair.second == 0;
    }

    public static WCGPairInteger fromString(String sValue)
    {
        return fromString(sValue, ':');
    }

    public static WCGPairInteger fromString(String sValue, char token)
    {
        WCGPairInteger pair = new WCGPairInteger();
        if (!pair.parseFromString(sValue, token))
        {
            return null;
        }
        return pair;
    }

    public int getRange()
    {
        return Math.abs(_m_pair.first - _m_pair.second);
    }

    @Override
    public String toString()
    {
        return _m_pair.toString();
    }
}