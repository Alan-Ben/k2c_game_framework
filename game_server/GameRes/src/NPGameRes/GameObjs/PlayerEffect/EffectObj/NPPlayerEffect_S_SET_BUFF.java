package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

public class NPPlayerEffect_S_SET_BUFF extends _ANPPlayerEffectInfo
{
    private long _m_lBuffId;
    private int _m_iLayer;
    private int _m_iSecs;
    
    public long buffId()
    {
        return _m_lBuffId;
    }

    public int layer()
    {
        return _m_iLayer;
    }

    public int secs()
    {
        return _m_iSecs;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_SET_BUFF;
    }

    public static NPPlayerEffect_S_SET_BUFF readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("can not get effect S_SET_BUFF[" + _reader.getSrcString() + "]");
            return null;
        }

        try
        {
            String buffIdS = _reader.readItem(':');
            String layerS = _reader.readItem(':');
            String secS = _reader.readItem(':');

            if (null == buffIdS
                    || null == layerS
                    || null == secS)
            {
                ALServerLog.Error("can not get effect S_SET_BUFF[" + _reader.getSrcString() + "]");
                return null;
            }

            NPPlayerEffect_S_SET_BUFF obj = new NPPlayerEffect_S_SET_BUFF();
            obj._m_lBuffId = Long.parseLong(buffIdS);
            obj._m_iLayer = Integer.parseInt(layerS);
            obj._m_iSecs = Integer.parseInt(secS);
            
            return obj;
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
