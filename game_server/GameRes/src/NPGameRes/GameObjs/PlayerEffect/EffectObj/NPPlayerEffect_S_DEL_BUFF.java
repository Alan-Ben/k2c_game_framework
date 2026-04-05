package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

public class NPPlayerEffect_S_DEL_BUFF extends _ANPPlayerEffectInfo
{
    private long _m_lBuffId;

    public long buffId()
    {
        return _m_lBuffId;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_DEL_BUFF;
    }

    public static NPPlayerEffect_S_DEL_BUFF readStr(String _str)
    {
        if (null == _str || _str.isEmpty())
        {
            ALServerLog.Error("can not get effect S_DEL_BUFF[" + _str + "]");
            return null;
        }

        try
        {
            NPPlayerEffect_S_DEL_BUFF obj = new NPPlayerEffect_S_DEL_BUFF();
            obj._m_lBuffId = Long.parseLong(_str);
            
            return obj;
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
