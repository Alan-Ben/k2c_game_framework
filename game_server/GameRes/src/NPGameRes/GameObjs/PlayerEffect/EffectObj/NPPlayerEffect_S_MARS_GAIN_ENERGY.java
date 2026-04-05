package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 火星系统：立即获得N小时产出的能源，S_MARS_GAIN_ENERGY:mins（分钟）
 * @author mj
 *
 */
public class NPPlayerEffect_S_MARS_GAIN_ENERGY extends _ANPPlayerEffectInfo
{
	private int _m_iMins;
	public int getMins() {return _m_iMins;}
	
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_MARS_GAIN_ENERGY;
    }

    public static NPPlayerEffect_S_MARS_GAIN_ENERGY readStr(String _str)
    {
        return new NPPlayerEffect_S_MARS_GAIN_ENERGY();
    }

    public static NPPlayerEffect_S_MARS_GAIN_ENERGY readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_MARS_GAIN_ENERGY[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            NPPlayerEffect_S_MARS_GAIN_ENERGY obj = new NPPlayerEffect_S_MARS_GAIN_ENERGY();
            obj._m_iMins = Integer.valueOf(_reader.getSrcString());
            
            return obj;
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
