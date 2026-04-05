package NPCommon.Util.Pair;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class WCGPairLongList implements _IParseFromStringable
{
    private ArrayList<WCGPairLong> _m_list = new ArrayList<>();

    @Override
    public boolean parseFromString(String sValue)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, new char[]{';'}, true);
            for (String str : strs)
            {
                WCGPairLong pair = WCGPairLong.fromString(str, ':');
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

    public ArrayList<WCGPairLong> getList()
    {
        return _m_list;
    }

    public static WCGPairLongList fromString(String sValue)
    {
        WCGPairLongList list = new WCGPairLongList();
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

    public WCGPairLong lookup(long _first)
    {
        for (WCGPairLong pair : _m_list)
        {
            if (pair.first() == _first)
            {
                return pair;
            }
        }
        return null;
    }

    public WCGPairLong ensure(long _first)
    {
        WCGPairLong pair = lookup(_first);
        if (pair == null)
        {
            pair = new WCGPairLong(_first, 0);
            _m_list.add(pair);
        }

        return pair;
    }
    
    /**
     * 获取随机数据
     * @return
     */
    public WCGPairLong rand()
    {
    	if(_m_list.isEmpty())
    		return null;
    	
    	int idx = CommonFunc.randomInt(_m_list.size() - 1);
    	return _m_list.get(idx);
    }
}