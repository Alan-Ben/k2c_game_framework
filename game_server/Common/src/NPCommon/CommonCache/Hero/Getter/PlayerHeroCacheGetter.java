package NPCommon.CommonCache.Hero.Getter;

import Common.HeroObj.Hero_ArenaShowList;
import NPCommon.CommonCache.GetterBase._ACacheGetter;
import NPCommon.CommonCache.Hero.Getter.Dealer.HeroArenaShowListGetterDealer;
import NPCommon.CommonCache.Hero._ACachedPlayerHeroList;
import WCGBasicServer._AWCGBasicServer;

public class PlayerHeroCacheGetter extends _ACacheGetter<_ACachedPlayerHeroList, _IPlayerHeroCacheGetterEnv>
{
    public PlayerHeroCacheGetter(_AWCGBasicServer _server)
    {
        super(_server);
    }

    public PlayerHeroCacheGetter(_AWCGBasicServer _server, _IPlayerHeroCacheGetterEnv _env)
    {
        super(_server, _env);
    }


    @Override
    protected void _startRegDealer()
    {
        _regDealer(Hero_ArenaShowList.class, new HeroArenaShowListGetterDealer(this));
    }
}
