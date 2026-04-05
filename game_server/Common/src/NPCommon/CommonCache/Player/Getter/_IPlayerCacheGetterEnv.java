package NPCommon.CommonCache.Player.Getter;

import NPCommon.CommonCache.GetterBase._ICacheGetterEnv;
import NPCommon.CommonCache.Player._ACachedPlayerInfo;

/**
 * 玩家缓存数据获取器的环境类
 * 用于处理Common包下无法获取到通用缓存管理器子类的问题
 */
public interface _IPlayerCacheGetterEnv extends _ICacheGetterEnv<_ACachedPlayerInfo>
{
    long getLikeCount(long _key);
}
