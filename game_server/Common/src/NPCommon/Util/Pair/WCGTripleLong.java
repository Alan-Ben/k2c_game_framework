package NPCommon.Util.Pair;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class WCGTripleLong implements _IParseFromStringable
{
    private long first;
    private long second;
    private long three;

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, new char[]{':'}, false);
            if (strs.length >= 1)
            {
                if (!strs[0].isEmpty())
                    setFirst(Long.parseLong(strs[0].trim()));
            }
            if (strs.length >= 2)
            {
                if (!strs[1].isEmpty())
                    setSecond(Long.parseLong(strs[1].trim()));
            }
            if (strs.length >= 3)
            {
                if (!strs[2].isEmpty())
                    setThree(Long.parseLong(strs[2].trim()));
            }
        } catch (Exception e)
        {
            CommLog.error("WCGTupleLong  parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }

    public static WCGTripleLong fromString(String sValue)
    {
        WCGTripleLong pair = new WCGTripleLong();
        if (!pair.parseFromString(sValue))
        {
            return null;
        }
        return pair;
    }

    public long getFirst()
    {
        return first;
    }

    public void setFirst(long first)
    {
        this.first = first;
    }

    public long getSecond()
    {
        return second;
    }

    public void setSecond(long second)
    {
        this.second = second;
    }

    public long getThree()
    {
        return three;
    }

    public void setThree(long three)
    {
        this.three = three;
    }

}