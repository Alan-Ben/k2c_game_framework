package NPCommon.Util.Pair;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPEnum.EQuality;

public class WCGPairQualityInt implements _IParseFromStringable
{
    //品质
    private EQuality _m_eQuality;
    //匹配 int 数据
    private int _m_value;

    public WCGPairQualityInt()
    {
        _m_eQuality = EQuality.NONE;
        _m_value = 0;
    }
    public WCGPairQualityInt(EQuality _quality, int _value)
    {
        _m_eQuality = _quality;
        _m_value = _value;
    }

    public EQuality getQuality()
    {
        return _m_eQuality;
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
            this._m_eQuality = EQuality.valueOf(strs[0].toUpperCase());
            this._m_value = Integer.parseInt(strs[1]);
        } 
        catch (Exception e)
        {
            CommLog.error("WCGPairQualityInt parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }
    
    public static WCGPairQualityInt fromString(String sValue, char token)
    {
    	WCGPairQualityInt pair = new WCGPairQualityInt();
        if (!pair.parseFromString(sValue, token))
        {
            return null;
        }
        
        return pair;
    }
}
