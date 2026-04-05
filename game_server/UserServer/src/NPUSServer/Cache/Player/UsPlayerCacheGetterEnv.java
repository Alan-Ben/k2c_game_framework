package NPUSServer.Cache.Player;

import NPCommon.CommonCache.Player.Getter._IPlayerCacheGetterEnv;
import NPCommon.CommonCache.Player._ACachedPlayerInfo;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;

public class UsPlayerCacheGetterEnv implements _IPlayerCacheGetterEnv
{
    private NPUserServer _m_server;

    public UsPlayerCacheGetterEnv(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void getData(long _key, HandlerTwo<Boolean, _ACachedPlayerInfo> _handler)
    {
        PlayerCacheFunc.getData(getUSServer(), _key, new HandlerTwo<Boolean, UserOfflineTmpDataInfo_PlayerCache>()
        {
            @Override
            public void handle(Boolean _isSuc, UserOfflineTmpDataInfo_PlayerCache _cacheInfo)
            {
                _handler.handle(_isSuc, _cacheInfo);
            }
        });
    }

    @Override
    public long getLikeCount(long _key)
    {
        return _m_server.getCollectLikeMgr().getLikeCount(_key);
    }
}
