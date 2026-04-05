package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.Refs.RefRemoteEffect;

public class NPPlayerEffect_S_R_EFFECT extends _ANPPlayerEffectInfo
{
    private RefRemoteEffect _m_refRemoteEffectRef;

    public RefRemoteEffect effectRef()
    {
        return _m_refRemoteEffectRef;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_R_EFFECT;
    }

    public static NPPlayerEffect_S_R_EFFECT readStr(String _str)
    {
        if (null == _str || _str.isEmpty())
        {
            ALServerLog.Error("can not get effect S_R_EFFECT[" + _str + "]");
            return null;
        }

        try
        {
            long effectId = Long.parseLong(_str);
            RefRemoteEffect ref = RefRemoteEffect.getMgr().get(effectId);
            if (null == ref)
            {
                ALServerLog.Error("can not get effect ref S_R_EFFECT[" + _str + "]");
                return null;
            }

            NPPlayerEffect_S_R_EFFECT effect = new NPPlayerEffect_S_R_EFFECT();
            effect._m_refRemoteEffectRef = ref;
            return effect;
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
    }
}
