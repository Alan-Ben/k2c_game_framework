package NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo;

import Common.CachedObj.*;
import NPCommon.CommonCache.Player._ACachedPlayerInfo;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr._IUserOfflineTmpDataInfo;
import USDB.Bo.PlayerCacheBO;

import java.nio.ByteBuffer;

/**
 * 子嗣数据处理对象
 */
public class UserOfflineTmpDataInfo_PlayerCache extends _ACachedPlayerInfo implements _IUserOfflineTmpDataInfo
{

    //数据库数据对象
    private PlayerCacheBO _m_bo;

    public UserOfflineTmpDataInfo_PlayerCache()
    {
        _m_bo = null;
    }

    protected PlayerCacheBO _getBO() {return _m_bo;}

    /**
     * 初始化缓存数据
     */
    public void initFromBo(PlayerCacheBO _bo)
    {
        _m_bo = _bo;

        _m_playerCache.setCid(_bo.getCid());
        _m_playerCache.setPlayerName(_bo.getPlayerName());
        _m_playerCache.setVipLvl(_bo.getVipLvl());
        _m_playerCache.setPlayerLvl(_bo.getPlayerLvl());
        _m_playerCache.setLastOfflineTimeMs(_bo.getLastOfflineTimeMs());
        _m_playerCache.setLastOnlineTimeMs(_bo.getLastOnlineTimeMs());
        _m_playerCache.setFreezeTimeMs(_bo.getFreezeTimeMs());
        _m_playerCache.setLanguage(_bo.getLanguage());
        _m_playerCache.setEarnings(_bo.getEarnings());
        _m_playerCache.setMaxEarnings(_bo.getMaxEarnings());
        _m_playerCache.setTotalPower(_bo.getTotalPower());
        _m_playerCache.setMaxPower(_bo.getMaxPower());
        _m_playerCache.setExp(_bo.getExp());
        _m_playerCache.setPlayerSkin(_bo.getPlayerSkin());
        _m_playerCache.setVipExp(_bo.getVipExp());
        //联盟信息
        try
        {
            if (null != _bo.getGuildInfo())
            {
                CachedObj_CachedGuildInfo guildInfo = new CachedObj_CachedGuildInfo();
                guildInfo.readPackage(ByteBuffer.wrap(_bo.getGuildInfo()));
                _m_playerCache.setGuildInfo(guildInfo);
            }
        } catch (Exception e)
        {
            CommLog.error("cid: " + _bo.getCid(), e);
        }
        //当前佩戴头像
        try
        {
            if (null != _bo.getIconInfo())
            {
                CachedObj_CachedIconInfo iconInfo = new CachedObj_CachedIconInfo();
                iconInfo.readPackage(ByteBuffer.wrap(_bo.getIconInfo()));
                _m_playerCache.setIconInfo(iconInfo);
            }
        } catch (Exception e)
        {
            CommLog.error("cid: " + _bo.getCid(), e);
        }
        //当前佩戴头像框
        try
        {
            if (null != _bo.getIconBgkInfo())
            {
                CachedObj_CachedIconBgkInfo iconBgkInfo = new CachedObj_CachedIconBgkInfo();
                iconBgkInfo.readPackage(ByteBuffer.wrap(_bo.getIconBgkInfo()));
                _m_playerCache.setIconBgkInfo(iconBgkInfo);
            }
        } catch (Exception e)
        {
            CommLog.error("cid: " + _bo.getCid(), e);
        }
        //头衔相关
        try
        {
            if (null != _bo.getTitleObjV2())
            {
                CachedObj_CachedTitleObj titleObj = new CachedObj_CachedTitleObj();
                titleObj.readPackage(ByteBuffer.wrap(_bo.getTitleObjV2()));
                _m_playerCache.setTitleObjV2(titleObj);
            }
        } catch (Exception e)
        {
            CommLog.error("cid: " + _bo.getCid(), e);
        }
        //当前佩戴气泡框
        try
        {
            if (null != _bo.getBubbleInfo())
            {
                CachedObj_CachedBubbleInfo bubbleInfo = new CachedObj_CachedBubbleInfo();
                bubbleInfo.readPackage(ByteBuffer.wrap(_bo.getBubbleInfo()));
                _m_playerCache.setBubbleInfo(bubbleInfo);
            }
        } catch (Exception e)
        {
            CommLog.error("cid: " + _bo.getCid(), e);
        }
        //Q版形象信息
        try
        {
            if (null != _bo.getCuteActorInfo())
            {
                CachedObj_CachedCuteActorInfo cuteActorInfo = new CachedObj_CachedCuteActorInfo();
                cuteActorInfo.readPackage(ByteBuffer.wrap(_bo.getCuteActorInfo()));
                _m_playerCache.setCuteAcotrInfo(cuteActorInfo);
            }
        } catch (Exception e)
        {
            CommLog.error("cid: " + _bo.getCid(), e);
        }
    }


    public void updateGuildInfo(NPUserServer _server, CachedObj_CachedGuildInfo _guildInfo)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setGuildInfo(_guildInfo);
        _getBO().saveGuildInfo(_server.getBM(), CommonFunc.ByteBfferToBytes(_guildInfo.makePackage()));
    }

    public void updateFreezeTime(NPUserServer _server, long _freezeTime)
    {
        if(null == _getBO())
            return ;

        getPlayerCache().setFreezeTimeMs(_freezeTime);
        _getBO().saveFreezeTimeMs(_server.getBM(), _freezeTime);
    }

    /**
     * 返回数据Id，用于在管理器中校验数据匹配
     * @return
     */
    public long getDataId()
    {
        return getPlayerCache().getCid();
    }
}
