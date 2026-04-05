package NPUSServer.NPUSUserMgr.UserComp.CacheComp.CacheData;

import Common.CachedObj.*;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBack;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.CacheComp._IBasicPlayerCacheData;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;
import USDB.Bo.PlayerCacheBO;

/**
 * 玩家基础数据cache
 */
public class PlayerCacheData extends UserOfflineTmpDataInfo_PlayerCache implements  _IBasicPlayerCacheData
{
    //存储服务器对象
    private NPUSUserData _m_userData;

    public PlayerCacheData(NPUSUserData _userData)
    {
        _m_userData = _userData;
    }

    public NPUserServer getServer() {return _m_userData.getUSServer();}

    @Override
    public void init(_ICallBack _callback) {
        _m_userData.getUSServer().getBM().getBM(PlayerCacheBO.class).findOne("cid", _m_userData.getCid(), new _ASelectCallback<PlayerCacheBO>()
        {
            @Override
            public void dealFail()
            {
                //加载失败此时也需要直接设置完成，需要等待玩家数据都初始化完成之后才可以进行插入处理
                //调用回调
                _callback.onRunOver();
            }

            @Override
            public void dealSuc(PlayerCacheBO _bo)
            {
                //初始化数据
                initFromBo(_bo);

                //调用回调
                _callback.onRunOver();
            }
        });
    }

    /**
     * 确保缓存数据的处理，在玩家初始化完成后调用，确保数据已经加载完成
     */
    @Override
    public void ensureCache()
    {
        //直接调用bo的savemarked处理
        if(null == _getBO())
        {
            //创建最简缓存数据
            PlayerCacheBO newCacheBo = new PlayerCacheBO();
            newCacheBo.setCid(getServer().getBM(), _m_userData.getCid());
            newCacheBo.setPlayerName(getServer().getBM(), _m_userData.getPlayerComponent().getName());
            newCacheBo.setPlayerLvl(getServer().getBM(), _m_userData.getPlayerComponent().getCurLvl());
            newCacheBo.setLastOnlineTimeMs(getServer().getBM(), CommonFunc.getNowTimeMS());
            newCacheBo.setLanguage(getServer().getBM(), _m_userData.getSdkInfo().language);

            newCacheBo.insert(getServer().getBM());

            //初始化数据
            initFromBo(newCacheBo);
        }
        else
        {
            //记录历史最高实力
            _getBO().setMaxPower(getServer().getBM(), _m_userData.getParam(ENPPlayerParam.POWER_MAX_RECORD));

            saveAll();
        }
    }

    @Override
    public void saveAll()
    {
        //直接调用bo的savemarked处理
        if(null == _getBO())
            return ;

        _getBO().saveAll(_m_userData.getUSServer().getBM());
    }

    //////////////////
    // 其他业务函数
    //////////////////

    //////////////////
    // 其他业务函数 结束
    //////////////////

    /////////////////
    // 各属性变化的处理函数
    //////////////////
    public void updateName(String _cname)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setPlayerName(_cname);
        //直接设置数据并保存
        _getBO().savePlayerName(getServer().getBM(), _cname);
    }

    public void updateVipLvl(int _vipLvl)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setVipLvl(_vipLvl);
        _getBO().saveVipLvl(getServer().getBM(), _vipLvl);
    }

    public void updatePlayerLvl(int _lvl)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setPlayerLvl(_lvl);
        _getBO().savePlayerLvl(getServer().getBM(), _lvl);
    }

    public void updateEarnings(long _earnings)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setEarnings(_earnings);
        //变动太频繁。不即时存储，等待定时存储
        _getBO().setEarnings(getServer().getBM(), _earnings);
    }

    public void updateMaxEarnings(long _maxEarnings)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setMaxEarnings(_maxEarnings);
        //变动太频繁。不即时存储，等待定时存储
        _getBO().setMaxEarnings(getServer().getBM(), _maxEarnings);
    }

    public void updateTotalPower(long _totalPower)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setTotalPower(_totalPower);
        //变动太频繁。不即时存储，等待定时存储
        _getBO().setTotalPower(getServer().getBM(), _totalPower);
    }

    /**
     * 更新历史最高实力
     * 只在当前实力大于历史最高实力时更新
     * @param _power
     */
    public void updateMaxPower(long _power)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setMaxPower(_power);
        //变动太频繁。不即时存储，等待定时存储
        _getBO().setMaxPower(getServer().getBM(), _power);
    }

    public void updateIconInfo(CachedObj_CachedIconInfo _iconInfo)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setIconInfo(_iconInfo);
        //变动太频繁。不即时存储，等待定时存储
        _getBO().setIconInfo(getServer().getBM(), CommonFunc.ByteBfferToBytes(_iconInfo.makePackage()));
    }

    public void updateIconBgkInfo(CachedObj_CachedIconBgkInfo _iconBgkInfo)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setIconBgkInfo(_iconBgkInfo);
        //变动太频繁。不即时存储，等待定时存储
        _getBO().setIconBgkInfo(getServer().getBM(), CommonFunc.ByteBfferToBytes(_iconBgkInfo.makePackage()));
    }

    public void updateBubbleInfo(CachedObj_CachedBubbleInfo _bubbleInfo)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setBubbleInfo(_bubbleInfo);
        //变动太频繁。不即时存储，等待定时存储
        _getBO().setBubbleInfo(getServer().getBM(), CommonFunc.ByteBfferToBytes(_bubbleInfo.makePackage()));
    }

    public void updateCuteActorInfo(CachedObj_CachedCuteActorInfo _cuteActorInfo)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setCuteAcotrInfo(_cuteActorInfo);
        //变动太频繁。不即时存储，等待定时存储
        _getBO().setCuteActorInfo(getServer().getBM(), CommonFunc.ByteBfferToBytes(_cuteActorInfo.makePackage()));
    }

    public void updateGuildInfo(CachedObj_CachedGuildInfo _guildInfo)
    {
        updateGuildInfo(getServer(), _guildInfo);
    }

    public void updateLastOfflineTime(long _lastOfflineTimeMs)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setLastOfflineTimeMs(_lastOfflineTimeMs);
        //这里不保存，后续有统一保存处理操作
        _getBO().setLastOfflineTimeMs(getServer().getBM(), _lastOfflineTimeMs);
    }

    public void updateLastOnlineTime(long _lastOnlineTimeMs)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setLastOnlineTimeMs(_lastOnlineTimeMs);
        //这里不保存，后续有统一保存处理操作
        _getBO().setLastOnlineTimeMs(getServer().getBM(), _lastOnlineTimeMs);
    }

    public void updateFreezeTime(long _freezeTime)
    {
        updateFreezeTime(getServer(), _freezeTime);
    }

    public void updateLanguage(String _language)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setLanguage(_language);
        //可能变动太频繁。不即时存储，等待定时存储
        _getBO().setLanguage(getServer().getBM(), _language);
    }

    public void updateExp(long _exp)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setExp(_exp);
        //可能变动太频繁。不即时存储，等待定时存储
        _getBO().setExp(getServer().getBM(), _exp);
    }

    public void updateTitleObjV2(CachedObj_CachedTitleObj _titleObj)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setTitleObjV2(_titleObj);
        //可能变动太频繁。不即时存储，等待定时存储
        _getBO().setTitleObjV2(getServer().getBM(), CommonFunc.ByteBfferToBytes(_titleObj.makePackage()));
    }

    public void updatePlayerSkin(long _playerSkin)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setPlayerSkin(_playerSkin);
        //可能变动太频繁。不即时存储，等待定时存储
        _getBO().setPlayerSkin(getServer().getBM(), _playerSkin);
    }

    public void updateVipExp(long _vipExp)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setVipExp(_vipExp);
        //可能变动太频繁。不即时存储，等待定时存储
        _getBO().setVipExp(getServer().getBM(), _vipExp);
    }
    ///////////////////////////
    // 各属性变化的处理函数结束
    ///////////////////////////
}
