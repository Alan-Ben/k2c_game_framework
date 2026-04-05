package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

public class NPPlayerEffect_NONE extends _ANPPlayerEffectInfo
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.NONE;
    }
}
