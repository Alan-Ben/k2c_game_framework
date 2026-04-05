package NPCommon.CommonCache.Player.Getter;

import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import NPCommon.CommonCache.GetterBase._ACacheGetter;
import NPCommon.CommonCache.Player.Getter.Dealer.PlayerCommonShowGetterDealer;
import NPCommon.CommonCache.Player.Getter.Dealer.PlayerIconShowGetterDealer;
import NPCommon.CommonCache.Player.Getter.Dealer.PlayerJoinedUsGetterDealer;
import NPCommon.CommonCache.Player._ACachedPlayerInfo;
import NPCommon.PlayerInfo_IconShow;
import WCGBasicServer._AWCGBasicServer;

/**
 * 玩家缓存数据获取器
 * 1、服务器需要在初始化时候调用setEnv进行初始化，一般情况下只有US需要传入环境类
 * 2、针对需要的不同数据结构体，实现不同的处理器
 */
public class PlayerCacheGetter extends _ACacheGetter<_ACachedPlayerInfo, _IPlayerCacheGetterEnv>
{
    public PlayerCacheGetter(_AWCGBasicServer _server)
    {
        super(_server);
    }

    public PlayerCacheGetter(_AWCGBasicServer _server, _IPlayerCacheGetterEnv _env)
    {
        super(_server, _env);
    }

    @Override
    public void _startRegDealer()
    {
        _regDealer(PlayerInfo_CommonShow.class, new PlayerCommonShowGetterDealer(this));
        _regDealer(PlayerInfo_IconShow.class, new PlayerIconShowGetterDealer(this));
        _regDealer(NP_SYS_PlayerJoinedUSInfo.class, new PlayerJoinedUsGetterDealer(this));
    }
}
