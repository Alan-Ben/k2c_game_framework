package NPCommon.CommonCache.Player;


import Common.CachedObj.*;
import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import NPCommon.CommonCache.ComCachedDataBase;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.CommonFunc;

public class _ACachedPlayerInfo extends ComCachedDataBase
{
    //玩家缓存数据
    protected CachedObj_CachedPlayerInfo _m_playerCache = new CachedObj_CachedPlayerInfo();

    public CachedObj_CachedPlayerInfo getPlayerCache()
    {
        return _m_playerCache;
    }

    public long getOfflineLeaveTimeMs()
    {
        //最后离线时间大于上线时间，玩家已离线
        if (getPlayerCache().getLastOfflineTimeMs() > getPlayerCache().getLastOnlineTimeMs())
        {
            return CommonFunc.getNowTimeMS() - getPlayerCache().getLastOfflineTimeMs();
        } else
        {
            //上线时间大于离线时间，说明玩家在线
            return 0;
        }
    }

    /**
     * 构造通用展示全部的数据
     * 最完整的展示数据，所有展示数据新增都应该更新这个结构体
     * @return CommPlayerInfo_Show
     */
    public PlayerInfo_CommonShow makeShowInfo()
    {
        PlayerInfo_CommonShow proto = new PlayerInfo_CommonShow();
        proto.setCid(_m_playerCache.getCid());
        proto.setIconShow(makeIconInfo());
        proto.setCuteActorId(getCuteActorId());
        proto.setExp(_m_playerCache.getExp());
        proto.setVipExp(_m_playerCache.getVipExp());
        //如果最大战力为0，说明还没有计算过最大战力，直接用当前战力作为最大战力
        if (_m_playerCache.getMaxPower() == 0)
        {
            _m_playerCache.setMaxPower(_m_playerCache.getTotalPower());
        }else
        {
            proto.setMaxPower(_m_playerCache.getMaxPower());
        }
        return proto;
    }

    /**
     * 通用的头像信息展示数据，适用于只需要展示头像的地方
     * @return CommPlayerInfo_IconShow
     */
    public PlayerInfo_IconShow makeIconInfo()
    {
        PlayerInfo_IconShow proto = new PlayerInfo_IconShow();
        proto.setCid(_m_playerCache.getCid());
        proto.setPlayerName(_m_playerCache.getPlayerName());
        proto.setIconId(getIconId());
        proto.setIconBgkId(getIconBgkId());
        proto.setBubbleId(getBubbleId());
        proto.setVipLvl(_m_playerCache.getVipLvl());
        proto.setPlayerLvl(_m_playerCache.getPlayerLvl());
        proto.setGuildId(_m_playerCache.getGuildInfo().getGuildId());
        proto.setGuildName(_m_playerCache.getGuildInfo().getGuildName());
        proto.setGuildSimpleName(_m_playerCache.getGuildInfo().getGuildSimpleName());
        proto.setIsOnline(getOfflineLeaveTimeMs() == 0);
        proto.setLastOfflineMs(_m_playerCache.getLastOfflineTimeMs());
        proto.setLastOnlineMs(_m_playerCache.getLastOnlineTimeMs());
        proto.setTotalPower(_m_playerCache.getTotalPower());
        proto.setEarnings(_m_playerCache.getEarnings());
        
        if(null != _m_playerCache.getTitleObjV2())
        {
        	proto.setCurTitle(_m_playerCache.getTitleObjV2().getCurInfo());
        	proto.setIsShow(_m_playerCache.getTitleObjV2().getIsShow());
        }
        
        proto.setPlayerSkinId(_m_playerCache.getPlayerSkin());
        proto.setExp(_m_playerCache.getExp());

        return proto;
    }

    /**
     * 通用的头像信息展示数据，适用于只需要展示头像的地方
     * @return CommPlayerInfo_IconShow
     */
    public NP_SYS_PlayerJoinedUSInfo makeJoinUSInfo()
    {
        NP_SYS_PlayerJoinedUSInfo proto = new NP_SYS_PlayerJoinedUSInfo();
        proto.setCid(_m_playerCache.getCid());
        proto.setPlayerName(_m_playerCache.getPlayerName());
        proto.setIconShowInfo(makeIconInfo());
        long freezeTimeMs = _m_playerCache.getFreezeTimeMs();
        proto.setFreezeTimeMs(freezeTimeMs);
        proto.setIsFreeze(CommonFunc.getNowTimeMS() < freezeTimeMs);
        return proto;
    }

    /**
     * 获取玩家头像id
     * @return 头像id
     */
    public long getIconId()
    {
        CachedObj_CachedIconInfo iconInfo = _m_playerCache.getIconInfo();
        //如果头像过期，返回默认头像
        if (CommonFunc.getNowTimeSec() <= iconInfo.getExpiredTimeS() || iconInfo.getExpiredTimeS() <= 0)
        {
            return iconInfo.getIconId();
        }
        return 0;
    }

    /**
     * 获取玩家头像框id
     * @return 头像框id
     */
    public long getIconBgkId()
    {
        CachedObj_CachedIconBgkInfo iconBgkInfo = _m_playerCache.getIconBgkInfo();
        //如果头像框过期，返回默认头像框
        if (CommonFunc.getNowTimeSec() <= iconBgkInfo.getExpiredTimeS() || iconBgkInfo.getExpiredTimeS() <= 0)
        {
            return iconBgkInfo.getIconBgkId();
        }
        return 0;
    }

    /**
     * 获取玩家气泡框id
     * @return 气泡框id
     */
    public long getBubbleId()
    {
        CachedObj_CachedBubbleInfo bubbleInfo = _m_playerCache.getBubbleInfo();
        //如果头像框过期，返回默认头像框
        if (CommonFunc.getNowTimeSec() <= bubbleInfo.getExpiredTimeS() || bubbleInfo.getExpiredTimeS() <= 0)
        {
            return bubbleInfo.getBubbleId();
        }
        return 0;
    }

    /**
     * 获取玩家Q版形象id
     * @return Q版形象id
     */
    public long getCuteActorId()
    {
        CachedObj_CachedCuteActorInfo cuteActorInfo = _m_playerCache.getCuteAcotrInfo();
        if (CommonFunc.getNowTimeSec() <= cuteActorInfo.getExpiredTimeS() || cuteActorInfo.getExpiredTimeS() <= 0)
        {
            return cuteActorInfo.getCuteActorId();
        }
        //如果Q版形象过期，返回0
        return 0;
    }
}
