package ActivitiesV01.Activities.RegularActivity.Player;

import ActivitiesV01.Activities.RegularActivity.RegularActivity;
import ActivitiesV01.Bo.RegularActivityPlayerInfoBO;
import ActivitiesV01.Bo.RegularActivityPlayerShopBuyRecordBO;
import ActivitiesV01.Refs.RegularEvent.RefRegularEventShopItem;
import Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.util.HashMap;
import java.util.Map;

/**
 * 活动商店信息类
 */
public class RegularActivityPlayerMgr
{
    // 活动基类
    private RegularActivity _m_activity;
    // 玩家数据映射表 <玩家ID, 玩家商店数据>
    private Map<Long, RegularActivityPlayerData> _m_playerDataMap;

    public RegularActivityPlayerMgr(RegularActivity _activity)
    {
        _m_activity = _activity;
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
     * 初始化商店信息
     * @param _bo
     */
    public void initShopInfoFromDB(RegularActivityPlayerInfoBO _bo)
    {
        ensurePlayerData(_bo.getCid()).initShopInfoFromDB(_bo);
    }

    /**
     * 初始化商店购买信息
     * @param _bo
     */
    public void initShopBuyRecordFromDB(RegularActivityPlayerShopBuyRecordBO _bo)
    {
        ensurePlayerData(_bo.getCid()).initBuyRecordFromDB(_bo);
    }

    /**
     * 查询玩家数据
     * @param _cid 玩家ID
     * @return 玩家商店数据
     */
    public RegularActivityPlayerData lookupPlayerData(long _cid)
    {
        return _m_playerDataMap.get(_cid);
    }

    /**
     * 获取玩家商店数据
     * @param _cid 玩家ID
     * @return 玩家商店数据
     */
    public RegularActivityPlayerData ensurePlayerData(long _cid)
    {
        return _m_playerDataMap.computeIfAbsent(_cid, c -> new RegularActivityPlayerData(this));
    }

    /**
     * 刷新玩家商店
     * @param _userData 玩家数据
     * @return 是否成功刷新
     */
    public Result refreshShop(NPUSUserData _userData)
    {
        return ensurePlayerData(_userData.getCid()).tryInitAndRefresh(_userData);
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
    public RegularActivity_ShopInfo makeProtoShopInfo(long _cid)
    {
        RegularActivityPlayerData playerData = lookupPlayerData(_cid);
        if (null == playerData)
        {
            return new RegularActivity_ShopInfo();
        }
        return playerData.makeProto();
    }

    /**
     * 使用道具
     * @param _userData
     * @param _itemId
     * @param _isTen
     * @param _context
     * @return
     */
    public Result useItem(NPUSUserData _userData, long _itemId, boolean _isTen, NPPlayerContext _context)
    {
        RefRegularEventShopItem refItem = RefRegularEventShopItem.getMgr().get(_itemId);
        if (refItem == null)
            return CommErr.REF_NOT_FOUND;

        int count = _isTen ? 10 : 1;

        long itemCount = _userData.getItemCount(refItem.item.getItemType(), refItem.item.getItemId());
        count = Math.min(count, (int)itemCount);

        if (!_userData.hasItem(refItem.item, count))
            return CommErr.ITEM_NOT_ENOUGH;

        if (!_userData.spendItem(refItem.item, count, _context))
            return CommErr.ITEM_NOT_ENOUGH;

        _userData.gainItemList(CommonFunc.itemMultiple(refItem.use_gain_item_list, count), _context);

        return Result.SUCC;
    }
}
