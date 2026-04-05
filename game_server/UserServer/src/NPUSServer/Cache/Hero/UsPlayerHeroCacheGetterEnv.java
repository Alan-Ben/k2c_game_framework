package NPUSServer.Cache.Hero;

import NPCommon.CommonCache.Hero.Getter._IPlayerHeroCacheGetterEnv;
import NPCommon.CommonCache.Hero._ACachedPlayerHeroList;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.PlayerHeroListInfo.UserOfflineTmpDataInfo_HeroListCache;

public class UsPlayerHeroCacheGetterEnv implements _IPlayerHeroCacheGetterEnv
{
    private NPUserServer _m_server;

    public UsPlayerHeroCacheGetterEnv(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer()
    {
        return _m_server;
    }

    @Override
    public void getData(long _key, HandlerTwo<Boolean, _ACachedPlayerHeroList> _handler)
    {
        PlayerHeroCacheFunc.getData(getUSServer(), _key, new HandlerTwo<Boolean, UserOfflineTmpDataInfo_HeroListCache>()
        {
            @Override
            public void handle(Boolean _isSucc, UserOfflineTmpDataInfo_HeroListCache _cacheData)
            {
                _handler.handle(_isSucc, _cacheData);
            }
        });
    }
}