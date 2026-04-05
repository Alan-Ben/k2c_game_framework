package NPUSServer.Cache.Hero;

import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.Const_UsOfflineTmpData;
import NPUSServer.UserOfflineTmpDataMgr.PlayerHeroListInfo.UserOfflineTmpDataInfo_HeroListCache;

public class PlayerHeroCacheFunc
{
    public static void getData(NPUserServer _server, long _cid, HandlerTwo<Boolean, UserOfflineTmpDataInfo_HeroListCache> _dealer)
    {
        if(null == _server || null == _dealer)
            return ;

        //检索在线玩家
        NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_cid);
        if(null != userData)
        {
            userData.safeCall(()->_dealer.handle(true, userData.getCacheComponent().getPlayerHeroListCachedata()));
            return ;
        }

        //无Userdata数据，使用offline处理
        _server.getOfflineTmpDataCore().localDoOfflineData(Const_UsOfflineTmpData.C_TmpDataType_PlayerHeroListCache, _cid, _cid
                , new _ICallBackResultT<UserOfflineTmpDataInfo_HeroListCache>() {
                    @Override
                    public void onRunOver(Result _result, UserOfflineTmpDataInfo_HeroListCache _offlineDataInfo) {
                        if(null == _offlineDataInfo)
                        {
                            _dealer.handle(false, null);
                            return ;
                        }

                        _dealer.handle(true, _offlineDataInfo);
                    }
                }
        );
    }

    public static void onGainHero(NPUSUserData _userData, HeroInfo _heroInfo)
    {
        _userData.getCacheComponent().getPlayerHeroListCachedata().onGainHero(_heroInfo);
    }

    public static void updateSkinId(NPUSUserData _userData, HeroInfo _heroInfo)
    {
        _userData.getCacheComponent().getPlayerHeroListCachedata().updateSkinId(_heroInfo);
    }

    public static void updatePower(NPUSUserData _userData, HeroInfo _heroInfo)
    {
        _userData.getCacheComponent().getPlayerHeroListCachedata().updatePower(_heroInfo);
    }

    public static void updateLevel(NPUSUserData _userData, HeroInfo _heroInfo)
    {
        _userData.getCacheComponent().getPlayerHeroListCachedata().updateLevel(_heroInfo);
    }


}
