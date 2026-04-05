package NPCommon.Util.Pair;


import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

/**
 * 整数对
 * 用于表示范围（无限）时 只能用于general表中 单字段判断
 */
public class WCGPairInt implements _IParseFromStringable
{
    private WCGPair<Integer, Integer> _m_pair = new WCGPair<Integer, Integer>(0, 0);

    public WCGPairInt()
    {

    }

    public WCGPairInt(int first, int second)
    {
        _m_pair.first = first;
        _m_pair.second = second;
    }

    public int first()
    {
        return _m_pair.first;
    }

    public int second()
    {
        return _m_pair.second;
    }

    public void setSecond(int _second)
    {
        _m_pair.second = _second;
    }

    public void addSecond(int _second)
    {
        _m_pair.second += _second;
    }

    public void setFirst(int _first)
    {
        _m_pair.first = _first;
    }

    public void addFirst(int _first)
    {
        _m_pair.first += _first;
    }
    
    public void add(WCGPairInt _pair)
    {
    	_m_pair.first += _pair.first();
    	_m_pair.second += _pair.second();
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
            CommLog.error("WCGPairLong  parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }

    public boolean isDefault()
    {
        return _m_pair.first == 0 && _m_pair.second == 0;
    }

    public static WCGPairInt fromString(String sValue)
    {
        return fromString(sValue, ':');
    }

    public static WCGPairInt fromString(String sValue, char token)
    {
        WCGPairInt pair = new WCGPairInt();
        if (!pair.parseFromString(sValue, token))
        {
            return null;
        }
        return pair;
    }

    @Override
    public String toString()
    {
        return _m_pair.toString();
    }

    public int rand()
    {
        int delta = _m_pair.second - _m_pair.first;
        if (delta > 0)
        {
            return _m_pair.first + CommonFunc.randomInt(delta);
        } else
        {
            return _m_pair.first;
        }
    }

    /**
     * 是否在范围内 支持不限制范围，-1表示不限制
     * @param _value
     * @return
     */
    public boolean inRange(int _value)
    {
        if (_m_pair.first == -1 && _m_pair.second == -1)
            return true;

        if (_m_pair.first != -1 && _value < _m_pair.first)
            return false;

        if (_m_pair.second != -1 && _value > _m_pair.second)
            return false;

        return true;
    }

    /**
     * 把传入值限制在范围内 支持不限制范围，-1表示不限制
     */
    public long limit(long _value)
    {
        if (_m_pair.first == -1 && _m_pair.second == -1)
            return _value;

        if (_m_pair.first != -1 && _value < _m_pair.first)
            return _m_pair.first;

        if (_m_pair.second != -1 && _value > _m_pair.second)
            return _m_pair.second;

        return _value;
    }
    
    public void clear()
    {
    	_m_pair.first = 0;
    	_m_pair.second = 0;
    }
}