package NPUSServer.CommonActivityMgr.Core.Shop;

import Common.ActivityObj.Activity_ShopInfo;
import GS2GC.p017_ActivityOp.GS2GC_017_058_OnActivityShopRefresh;
import GS2GC.p017_ActivityOp.GS2GC_017_060_OnActivityShopItemBuy;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.ShopErr;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Activity.RefActivityShopItem;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.ActivityPlayerShopBuyRecordBO;
import USDB.Bo.ActivityPlayerShopInfoBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动玩家商店数据
 */
public class ActivityPlayerShopData
{
    // 所属商店
    private ActivityShopInfo _m_shopInfo;
    // 商店数据库对象
    private ActivityPlayerShopInfoBO _m_bo;
    // 购买记录 <商品ID, 购买记录>
    private List<ActivityPlayerShopBuyRecord> _m_buyRecordList;

    /**
     * 构造函数
     * @param _shopInfo 所属商店
     */
    public ActivityPlayerShopData(ActivityShopInfo _shopInfo)
    {
        _m_shopInfo = _shopInfo;
        _m_buyRecordList = new ArrayList<>();
    }

    /**
     * 获取折扣
     * @param _refItem
     * @return
     */
    public int getDiscount(RefActivityShopItem _refItem)
    {
        if (_refItem.discount > 0)
            return _refItem.discount;

        return 10000;
    }

    /**
     * 初始化商店数据库对象
     * @param _bo 数据库对象
     */
    public void initShopInfoFromDB(ActivityPlayerShopInfoBO _bo)
    {
        _m_bo = _bo;
    }

    /**
     * 初始化购买记录
     * @param _bo 购买记录数据库对象
     */
    public void initBuyRecordFromDB(ActivityPlayerShopBuyRecordBO _bo)
    {
        _m_buyRecordList.add(new ActivityPlayerShopBuyRecord(_bo));
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
        _m_shopInfo.getActivity().getUSServer().getBM().getBM(ActivityPlayerShopBuyRecordBO.class).delAll("shop_db_id", _m_bo.getId());
    }

    /**
     * 尝试刷新商店
     * @param _userData 玩家数据
     * @return 是否成功刷新
     */
    public Result tryRefresh(NPUSUserData _userData)
    {
        long nowMs = CommonFunc.getNowTimeMS();

        if (_m_bo == null)
        {
            // 计算下次刷新时间
            long nextRefreshMs = _m_shopInfo.calcNextRefreshTimeMs(CommonFunc.getNowTimeMS());

            // 更新或创建数据库记录
            BM bm = _m_shopInfo.getActivity().getUSServer().getBM();

            ActivityPlayerShopInfoBO bo = new ActivityPlayerShopInfoBO();
            bo.setInstanceId(bm, _m_shopInfo.getActivityInstanceId());
            bo.setShopId(bm, _m_shopInfo.getShopId());
            bo.setCid(bm, _userData.getCid());
            bo.setNextRefreshTimeMs(bm, nextRefreshMs);
            bo.insert(bm);
            _m_bo = bo;
        } else
        {
            // 检查是否可以刷新
            if (_m_bo.getNextRefreshTimeMs() > nowMs)
                return Result.SUCC;

            // 计算下次刷新时间
            long nextRefreshMs = _m_shopInfo.calcNextRefreshTimeMs(nowMs);

            // 清空购买记录
            clearBuyRecords();

            // 更新数据库记录
            _m_bo.saveNextRefreshTimeMs(_m_shopInfo.getActivity().getUSServer().getBM(), nextRefreshMs);
        }

        //推送变更
        _userData.sendMsgToGC(new GS2GC_017_058_OnActivityShopRefresh(_m_shopInfo.getActivityInstanceId(), makeProto()));

        return Result.SUCC;
    }

    /**
     * 查询购买记录
     * @param _itemId
     * @return
     */
    public ActivityPlayerShopBuyRecord lookupBuyRecord(long _itemId)
    {
        for (ActivityPlayerShopBuyRecord buyRecord : _m_buyRecordList)
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
        ActivityPlayerShopBuyRecord buyRecord = lookupBuyRecord(_itemId);
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
        RefActivityShopItem refActivityShopItem = RefActivityShopItem.getMgr().get(_itemId);
        if (refActivityShopItem == null)
            return CommErr.REF_NOT_FOUND;

        //检查活动商店id是否匹配
        if (refActivityShopItem.activity_shop_id != _m_shopInfo.getShopId())
            return CommErr.PARAM_ERROR;

        //尝试刷新商店
        tryRefresh(_userData);

        //判断是否超过上限
        long hadBuyCount = getHadBuyCount(_itemId);
        if (hadBuyCount + _addCount > refActivityShopItem.buy_num)
            return ShopErr.SHOP_BUY_FAIL;

        //消耗道具
        List<NPCommonCostItem> costItemList = new ArrayList<>();

        if (refActivityShopItem.times_price_type_id != 0)
        {
            for (int i = 0; i < _addCount; i++)
            {
                //检查购买价格
                NPCommonCostItem cost = UsFunc.calCostPrice(_userData, refActivityShopItem.times_price_type_id, (int) (hadBuyCount + i));
                if (cost == null)
                    return CommErr.REF_NOT_FOUND;

                CommonFunc.mergeCostItem(costItemList, cost);
            }
        } else if (!refActivityShopItem.cost_item.isEmpty())
        {
            costItemList.addAll(CommonFunc.itemMultiple(refActivityShopItem.cost_item, _addCount));

            int discount = getDiscount(refActivityShopItem);
            //计算折扣 只有固定消耗需要计算折扣 【GOB-6161】
            if (discount != 10000)
                costItemList = CommonFunc.costItemDiscount(costItemList, discount);
        }

        if (!_userData.hasCostItemList(costItemList))
            return CommErr.ITEM_NOT_ENOUGH;

        if (!_userData.spendItem(costItemList, _context))
            return CommErr.CONSUME_FAIL;

        BM bmObj = _m_shopInfo.getActivity().getUSServer().getBM();

        //查询购买记录
        ActivityPlayerShopBuyRecord record = lookupBuyRecord(_itemId);
        if (record == null)
        {
            ActivityPlayerShopBuyRecordBO bo = new ActivityPlayerShopBuyRecordBO();
            bo.setInstanceId(bmObj, _m_shopInfo.getActivityInstanceId());
            bo.setShopDbId(bmObj, _m_bo.getId());
            bo.setShopId(bmObj, _m_shopInfo.getShopId());
            bo.setCid(bmObj, _userData.getCid());
            bo.setItemId(bmObj, _itemId);
            bo.setHadBuyCount(bmObj, _addCount);
            bo.insert(bmObj);

            record = new ActivityPlayerShopBuyRecord(bo);
            _m_buyRecordList.add(record);
        } else
        {
            record.addBuyCount(bmObj, _addCount);
        }

        //获取道具
        _userData.gainItem(CommonFunc.itemMultiple(refActivityShopItem.item, _addCount), _context);

        //推送变更
        _userData.sendMsgToGC(new GS2GC_017_060_OnActivityShopItemBuy(_m_shopInfo.getActivityInstanceId(), _m_shopInfo.getShopId(), record.makeProto()));

        return Result.SUCC;
    }

    /**
     * 构造商店协议
     * @return
     */
    public Activity_ShopInfo makeProto()
    {
        Activity_ShopInfo proto = new Activity_ShopInfo();
        proto.setShopId(_m_shopInfo.getShopId());
        proto.setNextRefreshTimeMs(_m_bo == null ? 0 : _m_bo.getNextRefreshTimeMs());
        for (ActivityPlayerShopBuyRecord buyRecord : _m_buyRecordList)
        {
            proto.addBuyRecordList(buyRecord.makeProto());
        }
        return proto;
    }
}
