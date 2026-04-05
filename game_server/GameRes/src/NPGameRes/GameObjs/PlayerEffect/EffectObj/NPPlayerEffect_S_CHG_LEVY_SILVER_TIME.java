package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerEffectType;
import NPEnum.ENPTimeAddType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 解锁征收
 */
public class NPPlayerEffect_S_CHG_LEVY_SILVER_TIME extends _ANPPlayerEffectInfo
{
    //时间操作类型
    private ENPTimeAddType _m_timeAddType;
    //有效时间
    private long _m_effectTimeS;

    public ENPTimeAddType getTimeAddType()
    {
        return _m_timeAddType;
    }

    public long getEffectTimeS()
    {
        return _m_effectTimeS;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_CHG_LEVY_SILVER_TIME;
    }

    public static NPPlayerEffect_S_CHG_LEVY_SILVER_TIME readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_CHG_LEVY_SILIVER_TIME[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            //时间操作类型
            String rawTimeAddType = _reader.readItem(':');
            //有效时间
            String rawEffectTime = _reader.readItem(':');

            if (null == rawTimeAddType || null == rawEffectTime)
            {
                ALServerLog.Error("can not get effect S_LEVY_ADD_COUNT[" + _reader.getSrcString() + "]");
                return null;
            }
            
            NPPlayerEffect_S_CHG_LEVY_SILVER_TIME obj = new NPPlayerEffect_S_CHG_LEVY_SILVER_TIME();
            obj._m_timeAddType = ENPTimeAddType.valueOf(rawTimeAddType);
            obj._m_effectTimeS = Long.parseLong(rawEffectTime);
            
            return obj;
        } 
        catch (Exception e)
        {
            CommLog.error("", e);
            return null;
        }
    }
}
