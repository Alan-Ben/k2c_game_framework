package ActivitiesV01.Activities.RegularActivity.Player;

import ActivitiesV01.Bo.RegularActivityPlayerInfoBO;
import ActivitiesV01.Bo.RegularActivityPlayerShopBuyRecordBO;
import ActivitiesV01.Refs.RegularEvent.RefRegularEventShop;
import ActivitiesV01.Refs.RegularEvent.RefRegularEventShopItem;
import Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopInfo;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_101_OnRegularActivityShopItemBuy;
import Hotfix.V01.GS2GC.p200_HotSimpleActivityOp.GS2GC_200_102_OnRegularActivityShopRefresh;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.ShopErr;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动玩家商店数据
 */
public class RegularActivityPlayerData
{
    // 所属商店
    private RegularActivityPlayerMgr _m_playerMgr;
    // 商店数据库对象
    private RegularActivityPlayerInfoBO _m_bo;
    // 购买记录 <商品ID, 购买记录>
    private List<RegularActivityPlayerShopBuyRecord> _m_buyRecordList;

    /**
     * 构造函数
     * @param _shopInfo 所属商店
     */
    public RegularActivityPlayerData(RegularActivityPlayerMgr _shopInfo)
    {
        _m_playerMgr = _shopInfo;
        _m_buyRecordList = new ArrayList<>();
    }

    /**
     * 初始化商店数据库对象
     * @param _bo 数据库对象
     */
    public void initShopInfoFromDB(RegularActivityPlayerInfoBO _bo)
    {
        _m_bo = _bo;
    }

    /**
     * 初始化购买记录
     * @param _bo 购买记录数据库对象
     */
    public void initBuyRecordFromDB(RegularActivityPlayerShopBuyRecordBO _bo)
    {
        _m_buyRecordList.add(new RegularActivityPlayerShopBuyRecord(_bo));
    }

    /**
     * 清空购买记录
     */
    public void clearBuyRecords()
    {
        //如果还没初始化商店, 则不需要处理
        if (_m_bo == null)
            return;

        _m_buyRecordList.clear();

        // 清空购买记录
        _m_playerMgr.getActivity().getUSServer().getBM().getBM(RegularActivityPlayerShopBuyRecordBO.class).delAll("shop_db_id", _m_bo.getId());
    }

    /**
     * 尝试刷新商店
     * @param _userData 玩家数据
     * @return 是否成功刷新
     */
    public Result tryInitAndRefresh(NPUSUserData _userData)
    {
        RefRegularEventShop refRegularEventShop = RefRegularEventShop.getMgr().get(_m_playerMgr.getActivity().getActivityId());
        if (refRegularEventShop == null)
            return CommErr.REF_NOT_FOUND;

        long nowMs = CommonFunc.getNowTimeMS();
        if (_m_bo == null)
        {
            // 计算下次刷新时间
            long nextRefreshMs = refRegularEventShop.refresh_clock.getNextFreshTimeTagMS(nowMs);

            // 创建数据库记录
            BM bmObj = _m_playerMgr.getActivity().getUSServer().getBM();

            RegularActivityPlayerInfoBO bo = new RegularActivityPlayerInfoBO();
            bo.setInstanceId(bmObj, _m_playerMgr.getActivityInstanceId());
            bo.setCid(bmObj, _userData.getCid());
            bo.setNextRefreshTimeMs(bmObj, nextRefreshMs);
            bo.insert(bmObj);
            _m_bo = bo;

        } else
        {
            // 检查是否可以刷新
            if (_m_bo.getNextRefreshTimeMs() > nowMs)
                return Result.SUCC;

            // 计算下次刷新时间
            long nextRefreshMs = refRegularEventShop.refresh_clock.getNextFreshTimeTagMS(nowMs);

            // 清空购买记录
            clearBuyRecords();

            // 更新数据库记录
            _m_bo.saveNextRefreshTimeMs(_m_playerMgr.getActivity().getUSServer().getBM(), nextRefreshMs);
        }

        //推送变更
        _userData.sendMsgToGC(new GS2GC_200_102_OnRegularActivityShopRefresh(_m_playerMgr.getActivityInstanceId(), makeProto()));

        return Result.SUCC;
    }

    /**
     * 查询购买记录
     * @param _itemId
     * @return
     */
    public RegularActivityPlayerShopBuyRecord lookupBuyRecord(long _itemId)
    {
        for (RegularActivityPlayerShopBuyRecord buyRecord : _m_buyRecordList)
        {
            if (buyRecord.getItemId() == _itemId)
            {
                return buyRecord;
            }
        }
        return null;
    }

    /**
     * 获取购买数量
     * @param _itemId
     * @return
     */
    public long getHadBuyCount(long _itemId)
    {
        RegularActivityPlayerShopBuyRecord buyRecord = lookupBuyRecord(_itemId);
        return buyRecord == null ? 0 : buyRecord.getHadBuyCount();
    }

    /**
     * 增加商品购买数量
     * @param _userData 玩家数据
     * @param _itemId   商品ID
     * @param _addCount 增加数量
     * @return 是否成功增加
     */
    public Result buyItem(NPUSUserData _userData, long _itemId, int _addCount, NPPlayerContext _context)
    {
        //查询商品配置
        RefRegularEventShopItem refItem = RefRegularEventShopItem.getMgr().get(_itemId);
        if (refItem == null)
            return CommErr.REF_NOT_FOUND;

        //判断是否可以直接购买
        if (!refItem.can_direct_buy)
            return ShopErr.SHOP_BUY_FAIL;

        //尝试刷新
        Result result = tryInitAndRefresh(_userData);
        if (!result.isSucc())
            return result;

        //判断是否超过上限
        long hadBuyCount = getHadBuyCount(_itemId);
        if (hadBuyCount + _addCount > refItem.buy_num)
            return ShopErr.SHOP_BUY_FAIL;

        //消耗道具
        List<NPCommonCostItem> costItemList = new ArrayList<>();

        for (int i = 0; i < _addCount; i++)
        {
            if (!refItem.buy_cost.isEmpty())
            {
                costItemList.addAll(refItem.buy_cost);
            }
        }

        if (!_userData.hasCostItemList(costItemList))
            return CommErr.ITEM_NOT_ENOUGH;

        if (!_userData.spendItem(costItemList, _context))
            return CommErr.CONSUME_FAIL;

        BM bmObj = _m_playerMgr.getActivity().getUSServer().getBM();

        //查询购买记录
        RegularActivityPlayerShopBuyRecord record = lookupBuyRecord(_itemId);
        if (record == null)
        {
            RegularActivityPlayerShopBuyRecordBO bo = new RegularActivityPlayerShopBuyRecordBO();
            bo.setInstanceId(bmObj, _m_playerMgr.getActivityInstanceId());
            bo.setShopDbId(bmObj, _m_bo.getId());
            bo.setCid(bmObj, _userData.getCid());
            bo.setItemId(bmObj, _itemId);
            bo.setHadBuyCount(bmObj, _addCount);
            bo.insert(bmObj);

            record = new RegularActivityPlayerShopBuyRecord(bo);
            _m_buyRecordList.add(record);
        } else
        {
            record.addBuyCount(bmObj, _addCount);
        }

        //获取道具
        _userData.gainItem(CommonFunc.itemMultiple(refItem.item,_addCount), _context);

        //推送变更
        _userData.sendMsgToGC(new GS2GC_200_101_OnRegularActivityShopItemBuy(_m_playerMgr.getActivityInstanceId(), record.makeProto()));

        return Result.SUCC;
    }

    /**
     * 构造商店协议
     * @return
     */
    public RegularActivity_ShopInfo makeProto()
    {
        RegularActivity_ShopInfo proto = new RegularActivity_ShopInfo();
        proto.setNextRefreshTimeMs(_m_bo == null ? 0 : _m_bo.getNextRefreshTimeMs());
        for (RegularActivityPlayerShopBuyRecord buyRecord : _m_buyRecordList)
        {
            proto.addBuyRecordList(buyRecord.makeProto());
        }
        return proto;
    }
}
