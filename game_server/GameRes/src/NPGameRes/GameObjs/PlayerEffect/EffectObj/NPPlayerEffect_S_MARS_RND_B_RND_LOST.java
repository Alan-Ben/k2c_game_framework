package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 火星系统：随机建筑随机（需要有工作居民）减少1~n个居民，S_MARS_RND_B_RND_LOST
 * @author mj
 *
 */
public class NPPlayerEffect_S_MARS_RND_B_RND_LOST extends _ANPPlayerEffectInfo
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_MARS_RND_B_RND_LOST;
    }

    public static NPPlayerEffect_S_MARS_RND_B_RND_LOST readStr(String _str)
    {
        return new NPPlayerEffect_S_MARS_RND_B_RND_LOST();
    }
}
