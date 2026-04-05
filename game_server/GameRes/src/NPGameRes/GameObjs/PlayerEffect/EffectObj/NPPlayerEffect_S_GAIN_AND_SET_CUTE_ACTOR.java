package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 解锁征收
 */
public class NPPlayerEffect_S_GAIN_AND_SET_CUTE_ACTOR extends _ANPPlayerEffectInfo
{
    //Q版形象ID
    private long _m_cuteActorId;
    //有效时间
    private long _m_effectTimeS;

    public long getCuteActorId()
    {
        return _m_cuteActorId;
    }

    public long getEffectTimeS()
    {
        return _m_effectTimeS;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_AND_SET_CUTE_ACTOR;
    }

    public static NPPlayerEffect_S_GAIN_AND_SET_CUTE_ACTOR readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_GAIN_AND_SET_CUTE_ACTOR[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            //Q版形象ID
            String rawCuteActorId = _reader.readItem(':');
            //有效时间
            String rawEffectTime = _reader.readItem(':');

            if (null == rawCuteActorId)
            {
                ALServerLog.Error("can not get effect S_LEVY_ADD_COUNT[" + _reader.getSrcString() + "]");
                return null;
            }
            
            NPPlayerEffect_S_GAIN_AND_SET_CUTE_ACTOR obj = new NPPlayerEffect_S_GAIN_AND_SET_CUTE_ACTOR();
            obj._m_cuteActorId = Long.parseLong(rawCuteActorId);
            if (null != rawEffectTime)
            {
                obj._m_effectTimeS = Long.parseLong(rawEffectTime);
            }
            
            return obj;
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
