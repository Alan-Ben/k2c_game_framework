package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 火星系统：修改心情值，S_MARS_CHG_MOOD:count数量
 * @author mj
 *
 */
public class NPPlayerEffect_S_MARS_CHG_MOOD extends _ANPPlayerEffectInfo
{
	private int _m_iCount;
	public int getCount() {return _m_iCount;}
	
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_MARS_CHG_MOOD;
    }

    public static NPPlayerEffect_S_MARS_CHG_MOOD readStr(String _str)
    {
        return new NPPlayerEffect_S_MARS_CHG_MOOD();
    }

    public static NPPlayerEffect_S_MARS_CHG_MOOD readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_MARS_CHG_MOOD[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            NPPlayerEffect_S_MARS_CHG_MOOD obj = new NPPlayerEffect_S_MARS_CHG_MOOD();
            obj._m_iCount = Integer.valueOf(_reader.getSrcString());
            
            return obj;
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
