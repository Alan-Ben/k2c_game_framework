package NPCommon.Util.Pair;

import Common.MarsEnum.EMarsExploreEventType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class WCGPairMarsEventTypeInt implements _IParseFromStringable
{
    //类型
    private EMarsExploreEventType _m_eType;
    //int数值
    private int _m_value;

    public WCGPairMarsEventTypeInt()
    {
    	_m_eType = EMarsExploreEventType.NONE;
        _m_value = 0;
    }
    public WCGPairMarsEventTypeInt(EMarsExploreEventType _type, int _value)
    {
    	_m_eType = _type;
        _m_value = _value;
    }

    public EMarsExploreEventType getType()
    {
        return _m_eType;
    }

    public int getValue()
    {
        return _m_value;
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

            //资质等级
            this._m_eType = EMarsExploreEventType.valueOf(strs[0].toUpperCase());
            this._m_value = Integer.parseInt(strs[1]);
        } 
        catch (Exception e)
        {
            CommLog.error("WCGPairMarsEventTypeInt parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }
    
    public static WCGPairMarsEventTypeInt fromString(String sValue, char token)
    {
    	WCGPairMarsEventTypeInt pair = new WCGPairMarsEventTypeInt();
        if (!pair.parseFromString(sValue, token))
        {
            return null;
        }
        
        return pair;
    }
}
