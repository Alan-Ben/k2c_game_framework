package NPUSServer.NPUSUserMgr.UserComp.OrderComp;

import Common.CommonFuncObj.GiftPack_Info;
import Common.ServerObj.ServerObj_WebPayGoodsInfo;
import Common.ServerObj.ServerObj_WebPayGoodsLimitInfo;
import Common.ServerObj.ServerObj_WebPayGoodsList;
import CommonEnum.ECurrency;
import CommonEnum.EGiftPackType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.OrderErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPItemType;
import NPGameRes.Refs.GiftPack.RefGiftPack;
import NPGameRes.Refs.RefPay;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GiftPackLifeCycleMgr.GiftPackLifeCycleData;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerGiftPackRecordBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 礼包组件 - 管理玩家的礼包购买记录和相关业务逻辑
 * <p>
 * 主要功能：
 * 1. 管理玩家的礼包购买记录（包括购买次数、刷新时间等）
 * 2. 处理礼包的购买限制检查（次数限制、时间限制、活动关联等）
 * 3. 支持现金购买和道具购买两种方式
 * 4. 监听礼包生命周期变化，自动清理过期的礼包记录
 * 5. 提供礼包数据的协议封装和客户端同步
 * <p>
 * 线程安全：所有公共方法都使用玩家级别锁进行保护
 */
public class PlayerGiftPackMgr implements _IHandlerHolder
{
    private OrderComponent _m_comp;

    /**
     * 玩家礼包购买记录列表 - 存储所有礼包的购买历史和状态信息
     */
    private List<PlayerGiftPackRecord> _m_recordList;

    /**
     * 构造函数 - 初始化礼包组件
     * @param _comp 所属组件
     */
    public PlayerGiftPackMgr(OrderComponent _comp)
    {
        _m_comp = _comp;
        _m_recordList = new ArrayList<>();
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    public NPUserServer getUSServer()
    {
        return _m_comp.getUSServer();
    }

    /**
     * 组件初始化完成回调 - 注册礼包生命周期监听器并清理过期数据
     * <p>
     * 主要功能：
     * 1. 注册礼包关闭事件监听器，当活动关联的礼包关闭时自动清理数据
     * 2. 执行初始数据清理，移除已关闭的礼包记录
     */
    public void onInited()
    {
        // 注册礼包关闭事件监听器
        _m_comp.getUSServer().getGiftPackLifeCycleMgr().getGiftPackClosedTrigger().addHandler(this, new HandlerOne<List<Long>>()
        {
            @Override
            public void handle(List<Long> _lifeCycleInstanceIdList)
            {
                getUserData().lockUser();
                try
                {
                    // 遍历删除与已关闭生命周期实例关联的礼包记录
                    ArrayList<Long> delList = null;          // 需要从客户端移除的礼包ID列表
                    ArrayList<Long> delDbIdList = null;      // 需要从数据库删除的记录ID列表

                    for (int i = _m_recordList.size() - 1; i >= 0; i--)
                    {
                        PlayerGiftPackRecord record = _m_recordList.get(i);

                        // 如果记录关联的生命周期实例已关闭
                        if (_lifeCycleInstanceIdList.contains(record.getRelativeLifeCycleInstanceId()))
                        {
                            _m_recordList.remove(i);

                            if (delList == null)
                                delList = new ArrayList<>();
                            delList.add(record.getGiftPackId());

                            // 如果有数据库ID，添加到删除列表
                            if (record.getDbId() != 0)
                            {
                                if (delDbIdList == null)
                                    delDbIdList = new ArrayList<>();
                                delDbIdList.add(record.getDbId());
                            }
                        }
                    }

                    // 通知客户端移除礼包记录
                    if (delList != null && !delList.isEmpty())
                        getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_063_OnGiftPackBuyRecordRemove(delList));

                    // 从数据库删除记录
                    if (delDbIdList != null && !delDbIdList.isEmpty())
                        getUSServer().getBM().getBM(PlayerGiftPackRecordBO.class).delAllInList("id", delDbIdList);
                } finally
                {
                    getUserData().unlockUser();
                }
            }
        });

        // 初始清理：移除已关闭的礼包数据
        _checkRemoveCloseData();
    }

    /**
     * 检查并移除已关闭的礼包数据
     * 遍历所有礼包记录，检查关联的活动是否仍然开放，
     * 如果活动已关闭则从内存和数据库中移除对应的礼包记录
     */
    private void _checkRemoveCloseData()
    {
        getUserData().lockUser();
        try
        {
            ArrayList<Long> delDbIdList = null;
            for (int i = _m_recordList.size() - 1; i >= 0; i--)
            {
                PlayerGiftPackRecord record = _m_recordList.get(i);
                if (record == null || record.getRelativeLifeCycleInstanceId() == 0)
                    continue;

                // 检查生命周期实例是否已关闭
                if (getUSServer().getGiftPackLifeCycleMgr().canPurchase(record.getGiftPackId()) != record.getRelativeLifeCycleInstanceId())
                {
                    // 如果已关闭，移除该记录
                    _m_recordList.remove(i);

                    // 如果有数据库ID，添加到删除列表
                    if (record.getDbId() != 0)
                    {
                        if (delDbIdList == null)
                            delDbIdList = new ArrayList<>();
                        delDbIdList.add(record.getDbId());
                    }
                }
            }
            // 如果有需要删除的数据库ID列表，执行删除操作
            if (delDbIdList != null && !delDbIdList.isEmpty())
                getUSServer().getBM().getBM(PlayerGiftPackRecordBO.class).delAllInList("id", delDbIdList);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public void dispose()
    {
        getUSServer().getGiftPackLifeCycleMgr().getGiftPackClosedTrigger().clear(this);
    }

    /**
     * 数据加载 - 从数据库加载玩家的礼包购买记录
     * @param _handler 异步回调处理器
     */
    protected void _initGiftPackRecords(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerGiftPackRecordBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerGiftPackRecordBO>>()
        {
            @Override
            public void dealSuc(List<PlayerGiftPackRecordBO> _boList)
            {
                for (PlayerGiftPackRecordBO bo : _boList)
                {
                    RefGiftPack ref = RefGiftPack.getMgr().get(bo.getGiftPackId());
                    if (ref == null)
                    {
                        USLog.error(getUSServer(), "GiftPackComponent initGiftPackRecords fail, ref not found, cid:{} giftPackId:{}", getUserData().getCid(), bo.getGiftPackId());
                        continue; // 如果配表不存在，跳过该记录
                    }

                    PlayerGiftPackRecord record = new PlayerGiftPackRecord(PlayerGiftPackMgr.this, ref, bo);
                    _m_recordList.add(record);
                }

                _handler.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "GiftPackComponent initGiftPackRecords fail, cid:{}", getUserData().getCid());
                _handler.onRunOver(false);
            }
        });
    }

    /**
     * 查找礼包购买记录
     * @param _giftPackId 礼包ID
     * @return 记录对象，如果不存在则返回null
     * <p>
     * 线程安全：使用玩家级别锁保护
     */
    public PlayerGiftPackRecord lookup(long _giftPackId)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerGiftPackRecord record : _m_recordList)
            {
                if (record.getGiftPackId() == _giftPackId)
                {
                    return record;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 确保礼包购买记录存在 - 如果不存在则创建新记录
     * 功能逻辑：
     * 1. 先尝试查找已有记录
     * 2. 如果不存在，验证礼包配表是否有效
     * 3. 检查活动关联礼包的开放状态
     * 4. 创建新的礼包记录并通知客户端
     * @param _giftPackId 礼包ID
     * @return 记录对象，如果礼包配表不存在或活动未开放则返回null
     * <p>
     * 线程安全：使用玩家级别锁保护
     */
    public PlayerGiftPackRecord createRecord(long _giftPackId)
    {
        getUserData().lockUser();
        try
        {
            PlayerGiftPackRecord record = lookup(_giftPackId);
            if (record == null)
            {
                RefGiftPack ref = RefGiftPack.getMgr().get(_giftPackId);
                if (ref == null)
                {
                    USLog.error(getUSServer(), "GiftPackComponent ensureRecord: gift pack ref not found, giftPackId={}", _giftPackId);
                    return null;
                }

                // 如果是活动关联礼包，检查活动是否开放
                long lifeCycleInstanceId = 0;
                if (!ref.getRelativeActivityIdList().isEmpty())
                {
                    lifeCycleInstanceId = getUSServer().getGiftPackLifeCycleMgr().canPurchase(ref.Id());
                    if (lifeCycleInstanceId == -1)
                        return null;
                }

                record = new PlayerGiftPackRecord(this, ref, lifeCycleInstanceId);
                _m_recordList.add(record);

                getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_062_OnGiftPackBuyRecordAdd(record.makeProto()));
            }
            return record;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 记录购买
     * @param _giftPackId          记录对象
     * @param _needRecordClientPay
     */
    public Result recordPurchase(long _giftPackId, boolean _needRecordClientPay, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            PlayerGiftPackRecord record = createRecord(_giftPackId);
            if (record == null)
            {
                USLog.error(getUSServer(), "GiftPackMgr recordPurchase, gift pack record not found, cid:{} giftPackId:{}",
                        getUserData().getCid(), _giftPackId);
                return CommErr.REF_NOT_FOUND;
            }

            // 如果是推送礼包，检查是否激活
            RefGiftPack ref = record.getRef();
            if (ref.type == EGiftPackType.PUSH_GIFT && !getUserData().getPushGiftPackComponent().canBuyGiftPack(_giftPackId))
            {
                USLog.error(getUSServer(), "GiftPackMgr recordPurchase, push gift not active, cid:{} giftPackId:{}",
                        getUserData().getCid(), _giftPackId);
                return OrderErr.PUSH_GIFT_NOT_ACTIVE;
            }

            // 尝试发货，如果发货失败则返回错误
            if (!record.tryPurchase(_needRecordClientPay, _context)) {
                USLog.error(getUSServer(), "GiftPackMgr recordPurchase, purchase limit reached, cid:{} giftPackId:{}",
                        getUserData().getCid(), _giftPackId);
                return OrderErr.PURCHASE_LIMIT_REACHED;
            }

            // 订单发货需要通知推送礼包
            if (ref.type == EGiftPackType.PUSH_GIFT)
                getUserData().getPushGiftPackComponent().onOrderDelivery(_giftPackId, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /***
     * 记录客户端购买的状态
     * 客户端购买状态表示的是客户端已经支付的礼包数量，此数量可能小于服务端发货数量
     * 客户端购买数量主要用于防止超出数量的异常订单生成
     * @param _giftPackId
     * @param _context
     * @return
     */
    public Result recordClientPay(long _giftPackId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            PlayerGiftPackRecord record = createRecord(_giftPackId);
            if (record == null)
            {
                USLog.error(getUSServer(), "GiftPackComponent recordClientPay, gift pack record not found, cid:{} giftPackId:{}",
                        getUserData().getCid(), _giftPackId);
                return CommErr.REF_NOT_FOUND;
            }

            record.recordPurchase(0, 1, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造订单商品数据
     * @return
     */
    public ResultOne<OrderGoodsData> makeOrderGoodsData(long _giftPackId)
    {
        getUserData().lockUser();
        try
        {
            // 先查找已有记录
            PlayerGiftPackRecord record = lookup(_giftPackId);
            if (record != null)
                return record.makeOrderGoodsData();

            // 如果记录不存在
            RefGiftPack ref = RefGiftPack.getMgr().get(_giftPackId);
            if (ref == null)
            {
                USLog.error(getUSServer(), "GiftPackComponent makeOrderGoodsData: gift pack ref not found, giftPackId={}", _giftPackId);
                return ResultOne.failed(CommErr.REF_NOT_FOUND);
            }

            // 检查礼包是否支持现金购买
            ResultOne<Long> getPayIdResult = ref.getPayId();
            if (!getPayIdResult.isSucc())
                return ResultOne.failed(getPayIdResult.getResult());

            long payId = getPayIdResult.getData();
            RefPay refPay = RefPay.getMgr().get(payId);
            if (refPay == null)
            {
                USLog.error(getUSServer(), "GiftPackComponent makeOrderGoodsData: pay ref not found, giftPackId={} payId={}", _giftPackId, payId);
                return ResultOne.failed(OrderErr.CREATE_ORDER_FAILED);
            }

            // 如果是活动关联礼包，检查活动是否开放
            if (!ref.getRelativeActivityIdList().isEmpty() && getUSServer().getGiftPackLifeCycleMgr().canPurchase(ref.Id()) == -1)
                return ResultOne.failed(OrderErr.RELATIVE_ACTIVITY_NOT_OPEN);

            OrderGoodsData orderGoods = new OrderGoodsData();
            orderGoods.setGoodsId(ref.Id());
            orderGoods.setRefPay(refPay);

            // 注意 由于为了避免创建Record对象，所以在@PlayerGiftPackRecord.makeOrderGoodsData()中有重复逻辑，更改时需要同步
            // 填充奖励物品
            orderGoods.getItemList().addAll(ref.item_list);
            // 填充额外奖励物品
            ref.fillExtraGainItemList(1, orderGoods.getItemList());
            // 添加VIP经验
            if (!ref.not_send_vip_exp)
                orderGoods.getItemList().add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.VIP_EXP.ordinal(), refPay.vip_exp));

            return ResultOne.succ(orderGoods);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 刷新礼包记录列表
     * @param _giftPackIdList
     * @param _context
     */
    public void refreshRecordList(List<Long> _giftPackIdList, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            List<GiftPack_Info> infoList = new ArrayList<>();

            for (PlayerGiftPackRecord record : _m_recordList)
            {
                if (_giftPackIdList.contains(record.getGiftPackId()))
                {
                    Result result = record.refreshBuyCount(_context);
                    if (!result.isSucc())
                    {
                        USLog.error(getUSServer(), "GiftPackComponent refreshRecordList, refresh buy count fail, cid:{} giftPackId:{} result:{}",
                                getUserData().getCid(), record.getGiftPackId(), result);
                        continue;
                    }

                    infoList.add(record.makeProto());
                }
            }

            getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_060_OnGiftPackRefresh(infoList));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 通过道具购买礼包
     * @param _giftPackId 礼包ID
     * @param _context    上下文
     */
    public Result itemBuy(long _giftPackId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            PlayerGiftPackRecord record = createRecord(_giftPackId);
            if (record == null)
                return CommErr.REF_NOT_FOUND;

            return record.itemBuy(_context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清除礼包购买记录
     * @param _giftPackId 礼包ID，0表示清除所有礼包记录
     * @param _context    操作上下文
     * @return 操作结果
     */
    public Result clearPurchaseRecord(long _giftPackId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_giftPackId == 0)
            {
                // 清除所有礼包购买记录
                ArrayList<Long> delDbIdList = null;
                ArrayList<Long> delList = null;

                for (PlayerGiftPackRecord record : _m_recordList)
                {
                    if (record.getDbId() != 0)
                    {
                        if (delDbIdList == null)
                            delDbIdList = new ArrayList<>();
                        delDbIdList.add(record.getDbId());
                    }

                    if (delList == null)
                        delList = new ArrayList<>();
                    delList.add(record.getGiftPackId());
                }

                // 清空记录列表
                _m_recordList.clear();

                // 从数据库删除记录
                if (delDbIdList != null && !delDbIdList.isEmpty())
                    getUSServer().getBM().getBM(PlayerGiftPackRecordBO.class).delAllInList("id", delDbIdList);

                // 通知客户端移除礼包记录
                if (delList != null)
                    getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_063_OnGiftPackBuyRecordRemove(delList));

                return Result.SUCC;
            } else
            {
                // 清除指定礼包购买记录
                PlayerGiftPackRecord record = lookup(_giftPackId);
                if (record == null)
                {
                    USLog.error(getUSServer(), "GiftPackComponent clearPurchaseRecord, gift pack record not found, cid:{} giftPackId:{}",
                            getUserData().getCid(), _giftPackId);
                    return CommErr.REF_NOT_FOUND;
                }

                // 从列表中移除记录
                _m_recordList.remove(record);

                // 从数据库删除记录
                if (record.getDbId() != 0)
                    getUSServer().getBM().getBM(PlayerGiftPackRecordBO.class).delAll("id", record.getDbId());

                // 通知客户端移除礼包记录
                ArrayList<Long> delList = new ArrayList<>();
                delList.add(_giftPackId);
                getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_063_OnGiftPackBuyRecordRemove(delList));

                return Result.SUCC;
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造礼包信息列表
     * @param _infoList
     */
    public void makeProtoList(List<GiftPack_Info> _infoList)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerGiftPackRecord record : _m_recordList)
            {
                _infoList.add(record.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    public String toString()
    {
        getUserData().lockUser();
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.append("GiftPackComponent{");
            sb.append(", recordCount=").append(_m_recordList.size());
            sb.append(", records=[");

            for (int i = 0; i < _m_recordList.size(); i++)
            {
                if (i > 0)
                    sb.append("\n");

                PlayerGiftPackRecord record = _m_recordList.get(i);
                sb.append("{");
                sb.append("giftPackId=").append(record.getGiftPackId());
                sb.append(", dbId=").append(record.getDbId());
                sb.append(", buyCount=").append(record.getBuyCount());
                sb.append(", clientBuyCount=").append(record.getClientBuyCount());
                sb.append(", limit=").append(record.getRef().buy_limit_count);
                sb.append(", nextRefreshTimeMs=").append(record.getNextRefreshTimeMs());
                sb.append(", relativeLifeCycleInstanceId=").append(record.getRelativeLifeCycleInstanceId());
                sb.append("}");
            }

            sb.append("]}");
            return sb.toString();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取网页充值商品列表
     *
     * 功能说明：
     * 1. 遍历所有标记为网页充值的礼包（is_web_gift_pack = true）
     * 2. 如果是活动关联礼包，检查活动是否开放
     * 3. 构造每个礼包的详细信息（价格、限购、奖励等）
     * 4. 返回封装好的 WebPayGoodsList 对象
     *
     * @return 网页充值商品列表
     *
     * 线程安全：已通过玩家锁保护
     */
    public ServerObj_WebPayGoodsList getWebPayGoodsList()
    {
        getUserData().lockUser();
        try
        {
            ServerObj_WebPayGoodsList result = new ServerObj_WebPayGoodsList();

            // 1. 遍历所有礼包配表
            List<RefGiftPack> allGiftPacks = RefGiftPack.getMgr().getList();
            for (RefGiftPack ref : allGiftPacks)
            {
                // 2. 过滤：只处理网页充值礼包
                if (!ref.is_web_gift_pack)
                    continue;

                // 3. 如果是活动关联礼包，检查活动是否开放
                if (!ref.getRelativeActivityIdList().isEmpty())
                {
                    long canPurchaseResult = getUSServer().getGiftPackLifeCycleMgr().canPurchase(ref.id);
                    // 活动未开放，跳过
                    if (canPurchaseResult == -1)
                        continue;
                }

                // 4. 获取支付配置
                RefPay refPay = _getPayFromGiftPack(ref);
                if (refPay == null)
                {
                    USLog.error(getUSServer(), "getWebPayGoodsList: refPay not found, giftPackId={}", ref.id);
                    continue;
                }

                // 5. 查找购买记录（不创建）
                PlayerGiftPackRecord record = lookup(ref.id);
                int hadBuyCount = (record != null) ? record.getHadBuyCount() : 0;

                // 6. 构造限购信息
                ServerObj_WebPayGoodsLimitInfo limitInfo = _buildLimitInfo(ref, record, hadBuyCount);

                // 7. 构造基础奖励列表（item）
                ArrayList<NPCommon_ItemInfo> itemList = new ArrayList<>(CommonFunc.costItemListToProto(ref.item_list));

                // 8. 构造赠品列表（gift）- 额外奖励
                List<NPCommonCostItem> extraGainList = new ArrayList<>();
                ref.fillExtraGainItemList(hadBuyCount + 1, extraGainList);
                ArrayList<NPCommon_ItemInfo> giftList = new ArrayList<>(CommonFunc.costItemListToProto(extraGainList));

                // 9. 构造 WebPayGoodsInfo
                ServerObj_WebPayGoodsInfo goodsInfo = new ServerObj_WebPayGoodsInfo(
                        ref.id,                    // goodsId
                        refPay.sdk_pay_id,        // sdkPayId
                        refPay.show_price,        // amount
                        limitInfo,                 // limit
                        itemList,                  // item
                        giftList                   // gift
                );

                result.addGoodsList(goodsInfo);
            }

            return result;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 从礼包获取支付配置
     *
     * 功能说明：
     * 遍历礼包的消耗列表，找到支付类型（ENPItemType.PAY）的配置项，
     * 根据其 itemId 查找对应的 RefPay 支付档位配置
     *
     * @param _ref 礼包配置
     * @return 支付配置，如果没有找到返回null
     */
    private RefPay _getPayFromGiftPack(RefGiftPack _ref)
    {
        // 遍历消耗列表，找到支付类型的配置
        for (NPCommonCostItem costItem : _ref.cost_list)
        {
            if (costItem.getItemType() == ENPItemType.PAY)
            {
                return RefPay.getMgr().get(costItem.getItemId());
            }
        }
        return null;
    }

    /**
     * 构造限购信息
     *
     * 功能说明：
     * 根据礼包配置和玩家购买记录构造限购信息对象
     *
     * 限购信息包含：
     * 1. maxPurchase - 最大购买数量（来自配表的 buy_limit_count）
     * 2. purchased - 已购买数量（来自玩家购买记录）
     * 3. endTime - 限购结束时间（10位时间戳）
     *
     * 结束时间计算规则：
     * - 如果不限购（buy_limit_count=0）：所有字段传默认值 0
     * - 如果礼包关联活动且活动开放：使用活动结束时间
     * - 否则使用限购刷新时间（如果有）
     *
     * @param _ref 礼包配置
     * @param _record 购买记录（可能为null）
     * @param _hadBuyCount 已购买次数
     * @return 限购信息对象
     */
    private ServerObj_WebPayGoodsLimitInfo _buildLimitInfo(RefGiftPack _ref, PlayerGiftPackRecord _record, int _hadBuyCount)
    {
        getUserData().lockUser();
        try
        {
            // 如果不限购，传默认值
            if (_ref.buy_limit_count == 0)
                return new ServerObj_WebPayGoodsLimitInfo(0, 0, 0);

            // 有限购
            int maxPurchase = _ref.buy_limit_count;
            long endTimeSec = 0;

            // 计算结束时间
            // 1. 如果礼包关联活动，使用活动结束时间
            GiftPackLifeCycleData lifeCycleData = getUSServer().getGiftPackLifeCycleMgr().lookup(_ref.id);
            if (lifeCycleData != null && !lifeCycleData.isEmpty())
            {
                endTimeSec = lifeCycleData.getExpectedEndTimeMs() / 1000;
            }
            // 2. 否则使用限购刷新时间
            else if (_record != null)
            {
                long nextRefreshMs = _record.getNextRefreshTimeMs();
                if (nextRefreshMs > 0)
                {
                    endTimeSec = nextRefreshMs / 1000;
                }
            }

            return new ServerObj_WebPayGoodsLimitInfo(maxPurchase, _hadBuyCount, endTimeSec);
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
