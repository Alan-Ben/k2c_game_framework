package NPUSServer.CommonActivityMgr.Core.CrystalGiftPack;

import Common.CommonFuncObj.CrystalGiftPack_Info;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.CrystalGiftPack.RefCrystalGiftPackGroup;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.ActivityPlayerCrystalGiftPackBuyRecordBO;
import USDB.Bo.ActivityPlayerCrystalGiftPackInfoBO;

import java.util.HashMap;
import java.util.Map;

/**
 * 活动钻石礼包组信息类
 */
public class ActivityCrystalGiftPackGroupInfo
{
    // 活动基类
    private _AActivityBase _m_activity;
    // 礼包组配置
    private RefCrystalGiftPackGroup _m_refGroup;
    // 玩家礼包组数据映射表 <玩家ID, 玩家礼包组数据>
    private Map<Long, ActivityPlayerCrystalGiftPackData> _m_playerDataMap;

    public ActivityCrystalGiftPackGroupInfo(_AActivityBase _activity, RefCrystalGiftPackGroup _refGroup)
    {
        _m_activity = _activity;
        _m_refGroup = _refGroup;
        _m_playerDataMap = new HashMap<>();
    }

    /**
     * 获取活动实例ID
     * @return 活动实例ID
     */
    public long getActivityInstanceId()
    {
        return _m_activity.getInstanceId();
    }

    /**
     * 获取活动实例
     * @return 活动实例
     */
    public _AActivityBase getActivity()
    {
        return _m_activity;
    }

    /**
     * 获取礼包组ID
     * @return 礼包组ID
     */
    public long getGroupId()
    {
        return _m_refGroup.id;
    }

    /**
     * 获取礼包组配置
     * @return 礼包组配置
     */
    public RefCrystalGiftPackGroup getRefGroup()
    {
        return _m_refGroup;
    }

    /**
     * 初始化礼包组信息
     * @param _bo
     */
    public void initInfoFromDB(ActivityPlayerCrystalGiftPackInfoBO _bo)
    {
        ensurePlayerData(_bo.getCid()).initInfoFromDB(_bo);
    }

    /**
     * 初始化礼包购买信息
     * @param _bo
     */
    public void initBuyRecordFromDB(ActivityPlayerCrystalGiftPackBuyRecordBO _bo)
    {
        ensurePlayerData(_bo.getCid()).initBuyRecordFromDB(_bo);
    }

    /**
     * 计算下次刷新时间
     * @param _nowTimeMs 当前时间
     * @return 下次刷新时间（毫秒）
     */
    public long calcNextRefreshTimeMs(long _nowTimeMs)
    {
        NPRefreshTimeObj refreshClock = _m_refGroup.refresh_clock;
        if (refreshClock == null)
            return 0;

        return refreshClock.getNextFreshTimeTagMS(_nowTimeMs);
    }

    /**
     * 查询玩家数据
     * @param _cid 玩家ID
     * @return 玩家礼包组数据
     */
    public ActivityPlayerCrystalGiftPackData lookupPlayerData(long _cid)
    {
        return _m_playerDataMap.get(_cid);
    }

    /**
     * 获取玩家礼包组数据
     * @param _cid 玩家ID
     * @return 玩家礼包组数据
     */
    public ActivityPlayerCrystalGiftPackData ensurePlayerData(long _cid)
    {
        return _m_playerDataMap.computeIfAbsent(_cid, c -> new ActivityPlayerCrystalGiftPackData(this));
    }

    /**
     * 刷新玩家礼包组
     * @param _userData 玩家数据
     * @return 是否成功刷新
     */
    public Result refreshGroup(NPUSUserData _userData)
    {
        return ensurePlayerData(_userData.getCid()).tryRefresh(_userData);
    }

    /**
     * 购买礼包
     * @param _userData 玩家数据
     * @param _packId   礼包ID
     * @param _buyCount 购买数量
     * @return 是否成功购买
     */
    public Result buyGiftPack(NPUSUserData _userData, long _packId, int _buyCount, NPPlayerContext _context)
    {
        return ensurePlayerData(_userData.getCid()).buyGiftPack(_userData, _packId, _buyCount, _context);
    }

    /**
     * 构建玩家礼包组协议
     * @param _cid 玩家数据
     * @return 礼包组信息
     */
    public CrystalGiftPack_Info makeProtoGiftPackInfo(long _cid)
    {
        ActivityPlayerCrystalGiftPackData playerData = lookupPlayerData(_cid);
        if (playerData == null)
        {
            CrystalGiftPack_Info crystalGiftPackInfo = new CrystalGiftPack_Info();
            crystalGiftPackInfo.setGroupId(getGroupId());
            return crystalGiftPackInfo;
        }
        return playerData.makeProto();
    }
}
