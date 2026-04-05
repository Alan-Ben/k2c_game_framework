package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 火星系统：治疗数量，S_MARS_CURE:数量（-1 全部病人）
 * @author mj
 *
 */
public class NPPlayerEffect_S_MARS_CURE extends _ANPPlayerEffectInfo
{
	private int _m_iCount;//-1 表示全部
	public int getCount() {return _m_iCount;}
	
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_MARS_CURE;
    }

    public static NPPlayerEffect_S_MARS_CURE readStr(String _str)
    {
        return new NPPlayerEffect_S_MARS_CURE();
    }

    public static NPPlayerEffect_S_MARS_CURE readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_MARS_CURE[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            NPPlayerEffect_S_MARS_CURE obj = new NPPlayerEffect_S_MARS_CURE();
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
