package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

public class NPPlayerEffect_S_UNLOCK_SEVEN_DAYS_LOGIN extends _ANPPlayerEffectInfo
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_UNLOCK_SEVEN_DAYS_LOGIN;
    }

    public static NPPlayerEffect_S_UNLOCK_SEVEN_DAYS_LOGIN readStr(String _str)
    {
        return new NPPlayerEffect_S_UNLOCK_SEVEN_DAYS_LOGIN();
    }
}
