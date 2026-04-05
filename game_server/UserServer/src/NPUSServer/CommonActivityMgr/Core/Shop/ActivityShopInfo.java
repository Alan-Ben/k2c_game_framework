package NPUSServer.CommonActivityMgr.Core.Shop;

import Common.ActivityObj.Activity_ShopInfo;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Activity.RefActivityShop;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.ActivityPlayerShopBuyRecordBO;
import USDB.Bo.ActivityPlayerShopInfoBO;

import java.util.HashMap;
import java.util.Map;

/**
 * 活动商店信息类
 */
public class ActivityShopInfo
{
    // 活动基类
    private _AActivityBase _m_activity;
    // 商店配置
    private RefActivityShop _m_refShop;
    // 玩家商店数据映射表 <玩家ID, 玩家商店数据>
    private Map<Long, ActivityPlayerShopData> _m_playerDataMap;

    public ActivityShopInfo(_AActivityBase _activity, RefActivityShop _refShop)
    {
        _m_activity = _activity;
        _m_refShop = _refShop;
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
     * 获取商店ID
     * @return 商店ID
     */
    public long getShopId()
    {
        return _m_refShop.id;
    }

    /**
     * 获取商店配置
     * @return 商店配置
     */
    public RefActivityShop getRefShop()
    {
        return _m_refShop;
    }

    /**
     * 初始化商店信息
     * @param _bo
     */
    public void initShopInfoFromDB(ActivityPlayerShopInfoBO _bo)
    {
        ensurePlayerData(_bo.getCid()).initShopInfoFromDB(_bo);
    }

    /**
     * 初始化商店购买信息
     * @param _bo
     */
    public void initShopBuyRecordFromDB(ActivityPlayerShopBuyRecordBO _bo)
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
        NPRefreshTimeObj refreshClock = _m_refShop.refresh_clock;
        if (refreshClock == null)
            return 0;

        return refreshClock.getNextFreshTimeTagMS(_nowTimeMs);
    }

    /**
     * 查询玩家数据
     * @param _cid 玩家ID
     * @return 玩家商店数据
     */
    public ActivityPlayerShopData lookupPlayerData(long _cid)
    {
        return _m_playerDataMap.get(_cid);
    }

    /**
     * 获取玩家商店数据
     * @param _cid 玩家ID
     * @return 玩家商店数据
     */
    public ActivityPlayerShopData ensurePlayerData(long _cid)
    {
        return _m_playerDataMap.computeIfAbsent(_cid, c -> new ActivityPlayerShopData(this));
    }

    /**
     * 刷新玩家商店
     * @param _userData 玩家数据
     * @return 是否成功刷新
     */
    public Result refreshShop(NPUSUserData _userData)
    {
        return ensurePlayerData(_userData.getCid()).tryRefresh(_userData);
    }

    /**
     * 记录商品购买
     * @param _userData 玩家数据
     * @param _itemId   商品ID
     * @param _buyCount 购买数量
     * @return 是否购买成功
     */
    public Result buyItem(NPUSUserData _userData, long _itemId, int _buyCount, NPPlayerContext _context)
    {
        return ensurePlayerData(_userData.getCid()).buyItem(_userData, _itemId, _buyCount, _context);
    }

    /**
     * 构建商店信息协议对象
     * @param _cid 玩家数据
     * @return 商店信息协议对象
     */
    public Activity_ShopInfo makeProtoShopInfo(long _cid)
    {
        ActivityPlayerShopData playerData = lookupPlayerData(_cid);
        if (null == playerData)
        {
            Activity_ShopInfo activityShopInfo = new Activity_ShopInfo();
            activityShopInfo.setShopId(getShopId());
            return activityShopInfo;
        }
        return playerData.makeProto();
    }
}
