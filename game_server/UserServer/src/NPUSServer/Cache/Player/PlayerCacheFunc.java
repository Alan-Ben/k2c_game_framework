package NPUSServer.Cache.Player;

import Common.CachedObj.*;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate.HandlerTwo;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerPropertyType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.Const_UsOfflineTmpData;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;

public class PlayerCacheFunc
{

    /*******
     * 自定Key，异步返回加载后的Loader
     * @param _cid
     * @param _dealer
     */
    public static void getData(NPUserServer _server, long _cid, HandlerThree<Boolean, NPUSUserData, UserOfflineTmpDataInfo_PlayerCache> _dealer)
    {
        if(null == _server || null == _dealer)
            return ;

        //检索在线玩家
        NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_cid);
        if(null != userData)
        {
            userData.safeCall(()->_dealer.handle(true, userData, null));
            return ;
        }

        //无Userdata数据，使用offline处理
        _server.getOfflineTmpDataCore().localDoOfflineData(Const_UsOfflineTmpData.C_TmpDataType_PlayerCache, _cid, _cid
                , new _ICallBackResultT<UserOfflineTmpDataInfo_PlayerCache>() {
                    @Override
                    public void onRunOver(Result _result, UserOfflineTmpDataInfo_PlayerCache _offlineDataInfo) {
                        if(null == _offlineDataInfo)
                        {
                            _dealer.handle(false, null, null);
                            return ;
                        }

                        _dealer.handle(true, null, _offlineDataInfo);
                    }
                }
        );
    }
    public static void getData(NPUserServer _server, long _cid, HandlerTwo<Boolean, UserOfflineTmpDataInfo_PlayerCache> _dealer)
    {
        if(null == _server || null == _dealer)
            return ;

        //检索在线玩家
        NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_cid);
        if(null != userData)
        {
            userData.safeCall(()->_dealer.handle(true, userData.getCacheComponent().getPlayerCacheData()));
            return ;
        }

        //无Userdata数据，使用offline处理
        _server.getOfflineTmpDataCore().localDoOfflineData(Const_UsOfflineTmpData.C_TmpDataType_PlayerCache, _cid, _cid
                , new _ICallBackResultT<UserOfflineTmpDataInfo_PlayerCache>() {
                    @Override
                    public void onRunOver(Result _result, UserOfflineTmpDataInfo_PlayerCache _offlineDataInfo) {
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
//    private interface _IUpdater<T>
//    {
//        void update(CachedPlayerInfo _cache, T _data);
//    }

//    private static <T> void _doUpdate(NPUserServer _server, long _cid, T _value, _IUpdater<T> _updater)
//    {
//        _server.getCachedPlayerMgr().getData(_cid, new HandlerTwo<Boolean, CachedPlayerInfo>()
//        {
//            @Override
//            public void handle(Boolean aBoolean, CachedPlayerInfo cachedPlayerInfo)
//            {
//                if (null == cachedPlayerInfo)
//                    return;
//
//                _updater.update(cachedPlayerInfo, _value);
//            }
//        });
//    }

    public static void updateName(NPUSUserData _userData, String _cname)
    {
        //直接设置数据并保存
        _userData.getCacheComponent().getPlayerCacheData().updateName(_cname);
//        _doUpdate(_server, _cid, _cname, (_cache, _data) ->
//        {
//            _cache.getBo().savePlayerName(_server.getBM(), _cname);
//            _cache.getPlayerCache().setPlayerName(_cname);
//        });
    }

    public static void updateVipLvl(NPUSUserData _userData, int _vipLvl)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateVipLvl(_vipLvl);
//        _doUpdate(_server, _cid, _vipLvl, (_cache, _data) ->
//        {
//            _cache.getBo().saveVipLvl(_server.getBM(), _vipLvl);
//            _cache.getPlayerCache().setVipLvl(_vipLvl);
//        });
    }

    public static void updatePlayerLvl(NPUSUserData _userData, int _lvl)
    {
        _userData.getCacheComponent().getPlayerCacheData().updatePlayerLvl(_lvl);
//        _doUpdate(_server, _cid, _lvl, (_cache, _data) ->
//        {
//            _cache.getBo().savePlayerLvl(_server.getBM(), _lvl);
//            _cache.getPlayerCache().setPlayerLvl(_lvl);
//        });
    }

    public static void updateEarnings(NPUSUserData _userData, long _earnings)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateEarnings(_earnings);
//        _doUpdate(_server, _cid, _earnings, (_cache, _data) ->
//        {
//            _cache.getPlayerCache().setEarnings(_earnings);
//            _cache.setNeedSaveToDb();
//        });
    }

    public static void updateMaxEarnings(NPUSUserData _userData, long _maxEarnings)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateMaxEarnings(_maxEarnings);
//        _doUpdate(_server, _cid, _maxEarnings, (_cache, _data) ->
//        {
//            _cache.getPlayerCache().setMaxEarnings(_maxEarnings);
//            _cache.setNeedSaveToDb();
//        });
    }

    public static void updateTotalPower(NPUSUserData _userData, long _totalPower)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateTotalPower(_totalPower);
//        _doUpdate(_server, _cid, _totalPower, (_cache, _data) ->
//        {
//            _cache.getPlayerCache().setTotalPower(_totalPower);
//            _cache.setNeedSaveToDb();
//        });
    }

    /**
     * 更新历史最高实力
     * 只在当前实力大于历史最高实力时更新
     * @param _power
     */
    public static void updateMaxPower(NPUSUserData _userData, long _power)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateMaxPower(_power);
//        _doUpdate(_server, _cid, _power, (_cache, _data) ->
//        {
//            _cache.getPlayerCache().setMaxPower(_power);
//            _cache.setNeedSaveToDb();
//        });
    }

    public static void updateIconInfo(NPUSUserData _userData, CachedObj_CachedIconInfo _iconInfo)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateIconInfo(_iconInfo);
//        _doUpdate(_server, _cid, _iconInfo, (_cache, _data) ->
//        {
//            _cache.getBo().saveIconInfo(_server.getBM(), CommonFunc.ByteBfferToBytes(_iconInfo.makePackage()));
//            _cache.getPlayerCache().setIconInfo(_iconInfo);
//        });
    }

    public static void updateIconBgkInfo(NPUSUserData _userData, CachedObj_CachedIconBgkInfo _iconBgkInfo)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateIconBgkInfo(_iconBgkInfo);
//        _doUpdate(_server, _cid, _iconBgkInfo, (_cache, _data) ->
//        {
//            _cache.getBo().saveIconBgkInfo(_server.getBM(), CommonFunc.ByteBfferToBytes(_iconBgkInfo.makePackage()));
//            _cache.getPlayerCache().setIconBgkInfo(_iconBgkInfo);
//        });
    }

    public static void updateBubbleInfo(NPUSUserData _userData, CachedObj_CachedBubbleInfo _bubbleInfo)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateBubbleInfo(_bubbleInfo);
//        _doUpdate(_server, _cid, _bubbleInfo, (_cache, _data) ->
//        {
//            _cache.getBo().saveBubbleInfo(_server.getBM(), CommonFunc.ByteBfferToBytes(_bubbleInfo.makePackage()));
//            _cache.getPlayerCache().setBubbleInfo(_bubbleInfo);
//        });
    }

    public static void updateCuteActorInfo(NPUSUserData _userData, CachedObj_CachedCuteActorInfo _cuteActorInfo)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateCuteActorInfo(_cuteActorInfo);
//        _doUpdate(_server, _cid, _cuteActorInfo, (_cache, _data) ->
//        {
//            _cache.getBo().saveCuteActorInfo(_server.getBM(), CommonFunc.ByteBfferToBytes(_cuteActorInfo.makePackage()));
//            _cache.getPlayerCache().setCuteAcotrInfo(_cuteActorInfo);
//        });
    }

    public static void updateGuildInfo(NPUserServer _server, long _cid, CachedObj_CachedGuildInfo _guildInfo)
    {
        getData(_server, _cid
                , new HandlerThree<Boolean, NPUSUserData, UserOfflineTmpDataInfo_PlayerCache>() {
                    @Override
                    public void handle(Boolean _isSucc, NPUSUserData _userData, UserOfflineTmpDataInfo_PlayerCache _cacheInfo) {
                        if(!_isSucc)
                            return ;

                        if(null != _userData)
                        {
                            _userData.getCacheComponent().getPlayerCacheData().updateGuildInfo(_guildInfo);
                        }
                        else
                        {
                            //直接修改缓存数据，并存入数据库
                            _cacheInfo.updateGuildInfo(_server, _guildInfo);
                        }
                    }
                });
//        _doUpdate(_server, _cid, _guildInfo, (_cache, _data) ->
//        {
//            _cache.getPlayerCache().setGuildInfo(_guildInfo);
//        });
    }

    public static void updateLastOfflineTime(NPUSUserData _userData, long _lastOfflineTimeMs)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateLastOfflineTime(_lastOfflineTimeMs);
//        _doUpdate(_server, _cid, _lastOfflineTimeMs, (_cache, _data) ->
//        {
//            _cache.getBo().saveLastOfflineTimeMs(_server.getBM(), _lastOfflineTimeMs);
//            _cache.getPlayerCache().setLastOfflineTimeMs(_lastOfflineTimeMs);
//        });
    }

    public static void updateLastOnlineTime(NPUSUserData _userData, long _lastOnlineTimeMs)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateLastOnlineTime(_lastOnlineTimeMs);
//        _doUpdate(_server, _cid, _lastOnlineTimeMs, (_cache, _data) ->
//        {
//            _cache.getBo().saveLastOnlineTimeMs(_server.getBM(), _lastOnlineTimeMs);
//            _cache.getPlayerCache().setLastOnlineTimeMs(_lastOnlineTimeMs);
//        });
    }

    public static void updateFreezeTime(NPUserServer _server, long _cid, long _freezeTime)
    {
        getData(_server, _cid
                , new HandlerThree<Boolean, NPUSUserData, UserOfflineTmpDataInfo_PlayerCache>() {
                    @Override
                    public void handle(Boolean _isSucc, NPUSUserData _userData, UserOfflineTmpDataInfo_PlayerCache _cacheInfo) {
                        if(!_isSucc)
                            return ;

                        if(null != _userData)
                        {
                            _userData.getCacheComponent().getPlayerCacheData().updateFreezeTime(_freezeTime);
                        }
                        else
                        {
                            //直接修改缓存数据，并存入数据库
                            _cacheInfo.updateFreezeTime(_server, _freezeTime);
                        }
                    }
                });
//        _doUpdate(_server, _cid, _freezeTime, (_cache, _data) ->
//        {
//            _cache.getBo().saveFreezeTimeMs(_server.getBM(), _freezeTime);
//            _cache.getPlayerCache().setFreezeTimeMs(_freezeTime);
//        });
    }

    public static void updateLanguage(NPUSUserData _userData, String _language)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateLanguage(_language);
//        _doUpdate(_server, _cid, _language, (_cache, _data) ->
//        {
//            _cache.getBo().saveLanguage(_server.getBM(), _language);
//            _cache.getPlayerCache().setLanguage(_language);
//        });
    }

    public static void updateExp(NPUSUserData _userData, long _exp)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateExp(_exp);
//        _doUpdate(_server, _cid, _exp, (_cache, _data) ->
//        {
//            _cache.getBo().saveExp(_server.getBM(), _exp);
//            _cache.getPlayerCache().setExp(_exp);
//        });
    }

    public static void updateTitleObjV2(NPUSUserData _userData, CachedObj_CachedTitleObj _titleObj)
    {
        _userData.getCacheComponent().getPlayerCacheData().updateTitleObjV2(_titleObj);
//        _doUpdate(_server, _cid, _titleObj, (_cache, _data) ->
//        {
//            _cache.getBo().saveTitleObjV2(_server.getBM(), CommonFunc.ByteBfferToBytes(_titleObj.makePackage()));
//            _cache.getPlayerCache().setTitleObjV2(_titleObj);
//        });
    }

    public static void updatePlayerSkin(NPUSUserData _userData, long _playerSkin)
    {
        _userData.getCacheComponent().getPlayerCacheData().updatePlayerSkin(_playerSkin);
//        _doUpdate(_server, _cid, _playerSkin, (_cache, _data) ->
//        {
//            _cache.getBo().savePlayerSkin(_server.getBM(), _playerSkin);
//            _cache.getPlayerCache().setPlayerSkin(_playerSkin);
//        });
    }

    public static void updateVipExp(NPUSUserData _userData, long _vipExp)
    {
            _userData.getCacheComponent().getPlayerCacheData().updateVipExp(_vipExp);
//        _doUpdate(_server, _cid, _vipExp, (_cache, _data) ->
//        {
//            _cache.getBo().saveVipExp(_server.getBM(), _vipExp);
//            _cache.getPlayerCache().setVipExp(_vipExp);
//        });
    }

    /**
     * 更新玩家属性
     * @param _userData      玩家id
     * @param _type     属性类型
     * @param _newValue 最新值
     */
    public static void updateProperty(NPUSUserData _userData, ENPPlayerPropertyType _type, Long _newValue)
    {
        switch (_type)
        {
            default:
                break;
        }
    }

    /**
     * 更新玩家参数
     * @param _userData 玩家
     * @param _type     参数类型
     * @param _newValue 最新值
     */
    public static void updateParam(NPUSUserData _userData, ENPPlayerParam _type, Long _newValue)
    {
        switch (_type)
        {
            case LEVEL:
                updatePlayerLvl(_userData, Math.toIntExact(_newValue));
                break;
            case VIP_LVL:
                updateVipLvl(_userData, Math.toIntExact(_newValue));
                break;
            case ICON:
                updateIconInfo(_userData, _userData.getIconComponent().makeCachedInfo(_newValue));
                break;
            case ICON_BGK:
                updateIconBgkInfo(_userData, _userData.getIconBgkComponent().makeCachedInfo(_newValue));
                break;
            case BUBBLE:
                updateBubbleInfo(_userData, _userData.getBubbleComponent().makeCachedInfo(_newValue));
                break;
            case CUTE_ACTOR:
                updateCuteActorInfo(_userData, _userData.getCuteActorComponent().makeCachedInfo(_newValue));
                break;
            case PLAYER_SKIN:
                updatePlayerSkin(_userData, _newValue);
                break;
            default:
                break;
        }
    }


}
