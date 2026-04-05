package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 火星系统：随机建筑（需要有工作居民）随机1~n个居民进入医疗室，S_MARS_RND_B_RND_SICK
 * @author mj
 *
 */
public class NPPlayerEffect_S_MARS_RND_B_RND_SICK extends _ANPPlayerEffectInfo
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_MARS_RND_B_RND_SICK;
    }

    public static NPPlayerEffect_S_MARS_RND_B_RND_SICK readStr(String _str)
    {
        return new NPPlayerEffect_S_MARS_RND_B_RND_SICK();
    }
}
