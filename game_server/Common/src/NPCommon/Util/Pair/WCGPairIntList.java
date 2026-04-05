package NPCommon.Util.Pair;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.function.Predicate;

public class WCGPairIntList implements _IParseFromStringable
{
    private ArrayList<WCGPairInt> _m_list = new ArrayList<>();

    @Override
    public boolean parseFromString(String sValue)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;
        _m_list.clear();
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, new char[]{';'}, true);
            for (String str : strs)
            {
                if (str == null || str.trim().isEmpty())
                    continue;
                WCGPairInt pair = WCGPairInt.fromString(str, ':');
                if (null != pair)
                {
                    _m_list.add(pair);
                }
            }
        } catch (Exception e)
        {
            CommLog.error("WCGPairLongList  parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }

    public WCGPairInt addPair(int first, int second)
    {
        WCGPairInt pair = new WCGPairInt(first, second);
        _m_list.add(pair);
        return pair;
    }

    public WCGPairInt addPair(WCGPairInt record)
    {
        _m_list.add(record);
        return record;
    }

    public ArrayList<WCGPairInt> getList()
    {
        return _m_list;
    }

    public static WCGPairIntList fromString(String sValue)
    {
        WCGPairIntList list = new WCGPairIntList();
        if (!list.parseFromString(sValue))
        {
            return null;
        }
        return list;
    }

    @Override
    public String toString()
    {
        return CommonFunc.list2String(_m_list);
    }

    public void clear()
    {
        _m_list.clear();
    }

    public WCGPairInt lookup(long _first)
    {
        for (WCGPairInt pair : _m_list)
        {
            if (pair.first() == _first)
            {
                return pair;
            }
        }
        return null;
    }

    public WCGPairInt ensure(long _first)
    {
        WCGPairInt pair = lookup(_first);
        if (pair == null)
        {
            pair = addPair((int) _first, 0);
        }

        return pair;
    }

    public void removeIf(Predicate<WCGPairInt> _predicate)
    {
        _m_list.removeIf(_predicate);
    }


}