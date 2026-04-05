package NPUSServer.CommonActivityMgr.Core.Shop;

import Common.ActivityObj.Activity_ShopInfo;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Activity.RefActivityShop;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.ActivityPlayerShopBuyRecordBO;
import USDB.Bo.ActivityPlayerShopInfoBO;

import java.util.ArrayList;

/**
 * 活动商店管理器
 */
public class ActivityShopMgr
{
    // 所属活动
    private _AActivityBase _m_activity;
    // 商店列表
    private ArrayList<ActivityShopInfo> _m_shopList;

    /**
     * 构造函数
     * @param _activity 所属活动
     */
    public ActivityShopMgr(_AActivityBase _activity)
    {
        _m_activity = _activity;
        _m_shopList = new ArrayList<>();
    }

    /**
     * 初始化商店列表
     */
    public void initShops()
    {
        //如果没有配置商店ID，则不需要初始化商店
        long shopId = _m_activity.getRef().exchange_shop_id;
        if (shopId <= 0)
            return;

        // 获取商店配置
        RefActivityShop refShop = RefActivityShop.getMgr().get(shopId);
        if (refShop == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityShopMgr initShops get null refShop:{} activity:{}",
                    shopId, _m_activity.getActivityId());
            return;
        }

        // 创建商店对象
        ActivityShopInfo shopInfo = new ActivityShopInfo(_m_activity, refShop);
        _m_shopList.add(shopInfo);
    }

    /**
     * 获取商店信息
     * @param _shopId 商店ID
     */
    public ActivityShopInfo lookupShopInfo(long _shopId)
    {
        for (ActivityShopInfo shopInfo : _m_shopList)
        {
            if (shopInfo.getShopId() == _shopId)
            {
                return shopInfo;
            }
        }
        return null;
    }

    /**
     * 初始化商店信息
     * @param _bo 数据库对象
     */
    public void initShopInfoFromDB(ActivityPlayerShopInfoBO _bo)
    {
        ActivityShopInfo shopInfo = lookupShopInfo(_bo.getShopId());
        if (shopInfo == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityShopMgr initShopInfoFromDB shop not found, shopId:{} activityId:{}",
                    _bo.getShopId(), _m_activity.getActivityId());
            return;
        }

        shopInfo.initShopInfoFromDB(_bo);
    }

    /**
     * 初始化购买记录
     * @param _bo 数据库对象
     */
    public void initShopBuyRecordFromDB(ActivityPlayerShopBuyRecordBO _bo)
    {
        ActivityShopInfo shopInfo = lookupShopInfo(_bo.getShopId());
        if (shopInfo == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityShopMgr initShopBuyRecordFromDB shop not found, shopId:{} activityId:{}",
                    _bo.getShopId(), _m_activity.getActivityId());
            return;
        }

        shopInfo.initShopBuyRecordFromDB(_bo);
    }

    /**
     * 刷新玩家商店
     * @param _userData 玩家数据
     * @param _shopId   商店ID
     * @return 是否成功刷新
     */
    public Result refreshShop(NPUSUserData _userData, long _shopId)
    {
        ActivityShopInfo shopInfo = lookupShopInfo(_shopId);
        if (shopInfo == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityShopMgr refreshShop shop not found, shopId:{} activityId:{}",
                    _shopId, _m_activity.getActivityId());
            return ActivityErr.ACTIVITY_SHOP_NOT_FOUND;
        }

        return shopInfo.refreshShop(_userData);
    }

    /**
     * 购买商品
     * @param _userData 玩家数据
     * @param _shopId   商店ID
     * @param _itemId   商品ID
     * @param _buyCount 购买数量
     * @return 是否成功购买
     */
    public Result buyItem(NPUSUserData _userData, long _shopId, long _itemId, int _buyCount, NPPlayerContext _context)
    {
        ActivityShopInfo shopInfo = lookupShopInfo(_shopId);
        if (shopInfo == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityShopMgr buyItem shop not found, shopId:{} activityId:{}",
                    _shopId, _m_activity.getActivityId());
            return ActivityErr.ACTIVITY_SHOP_NOT_FOUND;
        }

        return shopInfo.buyItem(_userData, _itemId, _buyCount, _context);
    }

    /**
     * 构建玩家商店数据
     * @param _cid 玩家数据
     * @return 商店信息
     */
    public Activity_ShopInfo makeProtoShopInfo(long _cid)
    {
        // 只获取第一个商店信息
        if (_m_shopList.isEmpty())
            return null;

        return _m_shopList.get(0).makeProtoShopInfo(_cid);
    }
}
