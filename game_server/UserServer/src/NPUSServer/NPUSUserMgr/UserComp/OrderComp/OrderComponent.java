package NPUSServer.NPUSUserMgr.UserComp.OrderComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.ServerObj.ServerObj_PayCallbackInfo;
import Common.ServerObj.ServerObj_WebPayGoodsList;
import Common.ServerObj.ServerObj_WebPayOrderInfo;
import Common.ServerObj.ServerObj_WebPayOrderList;
import CommonEnum.EGiftPackType;
import CommonEnum.EOrderPayType;
import CommonEnum.EOrderStatus;
import CommonEnum.ESpecialItemType;
import MJLog.MJLog;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.EUsParam;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.OrderErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.GiftPack.RefGiftPack;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RefPay;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_PaidVoucher;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import NPUSServer.UserServerConf;
import USDB.Bo.PlayerOrderBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 订单组件
 * 负责管理玩家的订单系统，包括订单创建、状态管理、支付流程等
 * 使用COLA状态机模式管理订单状态转换
 * 支持礼包购买、充值等多种商品类型的订单处理
 */
public class OrderComponent extends _ANPUserComponent
{
    private PlayerGiftPackMgr _m_mgr;
    // 玩家订单列表（缓存值）
    private List<PlayerOrderInfo> _m_orderList;

    public OrderComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ORDER);

        _m_mgr = new PlayerGiftPackMgr(this);
        _m_orderList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("order_component_init");

        // 添加加载订单数据任务
        process.addResDelegateProcess(
                action -> _loadOrderData(action::dealAction), "load_order_data",
                () -> USLog.error(getUSServer(), "player:{} load order data fail.", getUserData().getCid()), false);

        // 添加加载订单礼包数据任务
        process.addResDelegateProcess(
                action -> _m_mgr._initGiftPackRecords(action::dealAction), "load_order_giftpack_data",
                () -> USLog.error(getUSServer(), "player:{} load order giftpack data fail.", getUserData().getCid()), false);

        // 监听处理结果
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "player:{} order component init fail.", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        // 所有组件初始化完成后的回调
        _m_mgr.onInited();
    }

    @Override
    public void dispose()
    {
        _m_mgr.dispose();
    }

    /**
     * 加载订单数据
     */
    private void _loadOrderData(_ICallBackBool _action)
    {
        getUSServer().getBM().getBM(PlayerOrderBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerOrderBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "OrderComponent load orders failed: cid={}", getUserData().getCid());
                _action.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerOrderBO> _orderBoList)
            {
                if (_orderBoList != null)
                {
                    for (PlayerOrderBO orderBO : _orderBoList)
                    {
                        PlayerOrderInfo order = new PlayerOrderInfo(OrderComponent.this, orderBO);
                        _m_orderList.add(order);
                    }
                }
                _action.onRunOver(true);
            }
        });
    }

    public PlayerGiftPackMgr getMgr()
    {
        return _m_mgr;
    }

    /**
     * 根据订单号获取订单
     * @param _orderId 订单号
     * @return 订单对象，未找到返回null
     */
    public PlayerOrderInfo lookupOrderByOrderId(String _orderId)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerOrderInfo orderInfo : _m_orderList)
            {
                if (orderInfo.getOrderId().equals(_orderId))
                {
                    return orderInfo;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 根据订单数据ID查找订单
     * @param _dbId 数据库ID
     * @return 订单对象，未找到返回null
     */
    public PlayerOrderInfo lookupOrderByDbId(long _dbId)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerOrderInfo order : _m_orderList)
            {
                if (order.getDbId() == _dbId)
                {
                    return order;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 生成订单号
     * @param _cid
     * @param _goodsId
     * @return
     */
    public String generateOrderId(long _cid, long _goodsId)
    {
        // 获取订单序列号
        long newOrderSerial = getUSServer().getUSParams().incParam(EUsParam.ORDER_SERIAL, 1);

        // 拼接订单号 规则是：序列号-用户ID-商品ID-时间戳(秒)-平台ID-区服ID-服务器ID
        StringBuilder sb = new StringBuilder();
        sb.append(newOrderSerial).append("-")
                .append(_cid).append("-")
                .append(_goodsId).append("-")
                .append(CommonFunc.getNowTimeSec()).append("-")
                .append(UserServerConf.getInstance().getPlatformId()).append("-")
                .append(UserServerConf.getInstance().getPlatAreaId()).append("-")
                .append(getUSServer().getServerTypeId());

        return sb.toString();
    }

    /**
     * 创建新订单
     * @param _goodsId 商品ID
     * @param _context 操作上下文
     * @return 创建的订单对象
     */
    public ResultOne<OrderGoodsData> createOrder(long _goodsId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查商品类型和ID是否有效
            ResultOne<OrderGoodsData> checkResult = makeOrderGoodsData(_goodsId);
            if (!checkResult.isSucc())
                return ResultOne.failed(checkResult.getResult());

            // 检查是否有待处理的订单
            long pendingOrderDbId = getUserData().getParam(ENPPlayerParam.PENDING_ORDER_DB_ID);
            // 取消待处理订单
            if (pendingOrderDbId != 0)
            {
                Result result = cancelOrderByDbId(pendingOrderDbId, _context);
                if (!result.isSucc())
                    return ResultOne.failed(result);
            }

            OrderGoodsData goodsData = checkResult.getData();

            // 创建订单对象
            PlayerOrderInfo order = _createOrderInfo(goodsData);

            // 设置为待处理订单
            getUserData().setParam(ENPPlayerParam.PENDING_ORDER_DB_ID, order.getDbId());

            getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_066_OnOrderAdd(order.makeProto()));

            return ResultOne.succ(goodsData);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取订单商品数据
     * @param _giftPackId
     * @return
     */
    public ResultOne<OrderGoodsData> makeOrderGoodsData(long _giftPackId)
    {
        // 如果记录不存在
        RefGiftPack ref = RefGiftPack.getMgr().get(_giftPackId);
        if (ref == null)
            return ResultOne.failed(CommErr.REF_NOT_FOUND);

        // 判断推送礼包是否可以购买
        if (ref.type == EGiftPackType.PUSH_GIFT && !getUserData().getPushGiftPackComponent().canBuyGiftPack(_giftPackId))
            return ResultOne.failed(OrderErr.PUSH_GIFT_NOT_ACTIVE);

        return _m_mgr.makeOrderGoodsData(_giftPackId);
    }

    /**
     * 批量创建网页充值订单
     * <p>
     * 执行流程：
     * 1. 第一阶段：批量检查所有商品ID的有效性
     * - 遍历商品ID列表，调用 makeOrderGoodsData 进行检查
     * - 如果任何一个商品检查失败，立即返回第一个失败的错误码
     * - 将所有检查通过的 OrderGoodsData 收集到列表
     * 2. 第二阶段：全部检查通过后，批量创建订单
     * - 使用第一阶段返回的 OrderGoodsData 列表创建订单
     * - 构造 ServerObj_WebPayOrderInfo 对象
     * - 发送客户端通知
     * @param _goodsIdList   商品ID列表
     * @param _failedGoodsIds
     * @param _context       操作上下文
     * @return 成功创建的订单信息列表（ServerObj_WebPayOrderList）
     * <p>
     * 线程安全：通过getUserData().lockUser()保证线程安全
     */
    public ResultOne<ServerObj_WebPayOrderList> createWebOrderBatch(List<Long> _goodsIdList, List<Long> _failedGoodsIds, NPPlayerContext _context)
    {
        if (_goodsIdList == null || _goodsIdList.isEmpty())
        {
            return ResultOne.failed(CommErr.PARAM_ERROR);
        }

        getUserData().lockUser();
        try
        {
            // 第一阶段：批量检查所有商品
            List<OrderGoodsData> goodsDataList = new ArrayList<>();
            for (Long goodsId : _goodsIdList)
            {
                ResultOne<OrderGoodsData> checkResult = makeOrderGoodsData(goodsId);
                if (!checkResult.isSucc())
                {
                    _failedGoodsIds.add(goodsId);
                    USLog.warn(getUSServer(), "createWebOrderBatch makeOrderGoodsData failed: cid={}, goodsId={}, err={}",
                            getUserData().getCid(), goodsId, checkResult.getResult().toString());
                    continue;
                }
                goodsDataList.add(checkResult.getData());
            }

            // 任何一个商品检查失败，立即返回错误，不创建任何订单
            if (!_failedGoodsIds.isEmpty())
                return ResultOne.failed(OrderErr.CREATE_ORDER_FAILED);

            // 第二阶段：全部检查通过，批量创建订单
            ServerObj_WebPayOrderList orderList = new ServerObj_WebPayOrderList();
            for (OrderGoodsData goodsData : goodsDataList)
            {
                // 创建订单对象
                PlayerOrderInfo order = _createOrderInfo(goodsData);

                // 构造ServerObj_WebPayOrderInfo对象
                ServerObj_WebPayOrderInfo orderInfo = new ServerObj_WebPayOrderInfo();
                orderInfo.setOrderId(goodsData.getOrderId());
                orderInfo.setGoodsId(goodsData.getGoodsId());
                orderInfo.setSdkPayId(goodsData.getRefPay().sdk_pay_id);
                orderInfo.setAmount(goodsData.getRefPay().show_price);

                // 添加到订单列表
                orderList.addOrderList(orderInfo);

                // 发送订单创建通知给客户端
                getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_066_OnOrderAdd(order.makeProto()));
            }

            return ResultOne.succ(orderList);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 创建订单信息
     * @param _goodsData
     * @return
     */
    private PlayerOrderInfo _createOrderInfo(OrderGoodsData _goodsData)
    {
        getUserData().lockUser();
        try
        {
            // 生成唯一订单号
            String orderId = generateOrderId(getUserData().getCid(), _goodsData.getGoodsId());
            _goodsData.setOrderId(orderId);

            // 创建BO对象
            PlayerOrderBO orderBO = new PlayerOrderBO();
            orderBO.setCid(getUSServer().getBM(), getUserData().getCid());
            orderBO.setOrderId(getUSServer().getBM(), orderId);
            orderBO.setPayId(getUSServer().getBM(), _goodsData.getPayId());
            orderBO.setGoodsId(getUSServer().getBM(), _goodsData.getGoodsId());
            orderBO.setOrderStatus(getUSServer().getBM(), EOrderStatus.WAIT_PAY.ordinal());
            orderBO.setPayType(getUSServer().getBM(), EOrderPayType.NONE.ordinal());
            orderBO.setCreateTimeMs(getUSServer().getBM(), CommonFunc.getNowTimeMS());
            orderBO.setPayTimeMs(getUSServer().getBM(), 0L);
            orderBO.setItemList(getUSServer().getBM(), CommonFunc.googleJson().toJson(_goodsData.getItemList()));
            orderBO.insert(getUSServer().getBM());

            // 创建PlayerOrder对象并缓存
            PlayerOrderInfo order = new PlayerOrderInfo(OrderComponent.this, orderBO);
            _m_orderList.add(order);

            return order;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 客户端通知支付取消
     * @param _orderId 订单号
     * @param _context 操作上下文
     * @return 是否设置成功
     */
    public Result cancelOrderByOrderId(String _orderId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            PlayerOrderInfo orderInfo = lookupOrderByOrderId(_orderId);
            if (orderInfo == null)
                return OrderErr.ORDER_NOT_EXIST;

            return orderInfo.cancel(_context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 支付取消
     * @param _dbId    订单实例id
     * @param _context 操作上下文
     * @return 是否设置成功
     */
    public Result cancelOrderByDbId(long _dbId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            PlayerOrderInfo orderInfo = lookupOrderByDbId(_dbId);
            if (orderInfo == null)
                return OrderErr.ORDER_NOT_EXIST;

            return orderInfo.cancel(_context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 平台订单支付完成调用
     * @param _callbackInfo
     * @param _isOfflinePay 是否是离线时支付
     * @param _context      操作上下文
     * @return 是否设置成功
     */
    public Result platNotifyPay(ServerObj_PayCallbackInfo _callbackInfo, boolean _isOfflinePay, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查用户ID是否匹配
            if (!getUserData().getUid().equals(_callbackInfo.getUid()))
                return OrderErr.PLAT_NOTIFY_PAY_UID_NOT_MATCH;

            String orderId = _callbackInfo.getOrderId();

            // 查询订单信息
            PlayerOrderInfo orderInfo = lookupOrderByOrderId(orderId);
            if (orderInfo == null)
                return OrderErr.ORDER_NOT_EXIST;

            // 如果订单状态不是待支付状态，则认为订单无效
            if (orderInfo.getOrderStatus() != EOrderStatus.WAIT_PAY)
            {
                USLog.warn(getUSServer(), "platNotifyPay order status invalid: orderId={}, status={}",
                        orderId, orderInfo.getOrderStatus());
                return OrderErr.ORDER_CANT_OPERATE;
            }

            // 获取支付配置
            RefPay refPay = RefPay.getMgr().get(orderInfo.getPayId());
            if (refPay == null)
                return CommErr.REF_NOT_FOUND;

            //校验sdkPayId、商品id、金额是否匹配
            if (!orderInfo.validatePlatPayInfo(refPay, _callbackInfo))
            {
                USLog.warn(getUSServer(), "platNotifyPay order pay info not match: orderId={}, sdkPayId={}, goodsId={}, amount={}",
                        orderId, _callbackInfo.getSdkPayId(), _callbackInfo.getProductId(), _callbackInfo.getAmount());
                return OrderErr.PLAT_NOTIFY_PAY_INFO_NOT_MATCH;
            }

            // 标记订单完成支付
            orderInfo.setPay(_callbackInfo.getSdkOrderId(), _callbackInfo.getPayTime(),
                    CommonFunc.getNowTimeSec(), EOrderPayType.PLATFORM);

            // 清除待处理订单标记
            if (getUserData().getParam(ENPPlayerParam.PENDING_ORDER_DB_ID) == orderInfo.getDbId())
                getUserData().setParam(ENPPlayerParam.PENDING_ORDER_DB_ID, 0);

            // 执行发货逻辑
            tryDelivery(orderInfo, _callbackInfo, refPay, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }


    /**
     * 平台订单支付完成调用
     * @param _orderId
     * @param _context      操作上下文
     * @return 是否设置成功
     */
    public Result gmPay(String _orderId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 查询订单信息
            PlayerOrderInfo orderInfo = lookupOrderByOrderId(_orderId);
            if (orderInfo == null)
                return OrderErr.ORDER_NOT_EXIST;

            // 如果订单状态不是待支付状态，则认为订单无效
            if (orderInfo.getOrderStatus() != EOrderStatus.WAIT_PAY)
            {
                USLog.warn(getUSServer(), "gmPay order status invalid: orderId={}, status={}",
                        _orderId, orderInfo.getOrderStatus());
                return OrderErr.ORDER_CANT_OPERATE;
            }

            // 获取支付配置
            RefPay refPay = RefPay.getMgr().get(orderInfo.getPayId());
            if (refPay == null)
                return CommErr.REF_NOT_FOUND;

            // 标记订单完成支付
            int nowTimeSec = CommonFunc.getNowTimeSec();
            orderInfo.setPay("", nowTimeSec, nowTimeSec, EOrderPayType.GM);

            // 清除待处理订单标记
            if (getUserData().getParam(ENPPlayerParam.PENDING_ORDER_DB_ID) == orderInfo.getDbId())
                getUserData().setParam(ENPPlayerParam.PENDING_ORDER_DB_ID, 0);

            // 执行发货逻辑
            tryDelivery(orderInfo, null, refPay, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取订单状态
     * @param _orderId   订单号
     * @param _sdkOrderId
     * @return 订单状态
     */
    public boolean isOrderDeliverySuccess(String _orderId, String _sdkOrderId)
    {
        getUserData().lockUser();
        try
        {
            PlayerOrderInfo orderInfo = lookupOrderByOrderId(_orderId);
            if (orderInfo == null)
                return false;

            return orderInfo.getOrderStatus() == EOrderStatus.DELIVERY_COMPLETED && orderInfo.getSdkOrderId().equals(_sdkOrderId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 客户端通知支付
     * @param _orderId
     * @param _context
     * @return
     */
    public Result clientNotifyPay(String _orderId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 查询订单信息
            PlayerOrderInfo orderInfo = lookupOrderByOrderId(_orderId);
            if (orderInfo == null)
                return OrderErr.ORDER_NOT_EXIST;

            boolean isChgState = orderInfo.clientNotifyPay(_context);
            //如状态未变更，直接返回成功
            if (!isChgState)
                return Result.SUCC;

            //调用次数管理器记录客户端通知支付次数
            Result result = getMgr().recordClientPay(orderInfo.getGoodsId(), _context);
            if (!result.isSucc())
            {
                USLog.error(getUSServer(),
                        "PlayerOrderInfo clientNotifyPay failed: orderId={}, cid={}, err={}",
                        orderInfo.getOrderId(), getCid(), result.getMsg());
                return result;
            }

            // 清除待处理订单标记
            if (getUserData().getParam(ENPPlayerParam.PENDING_ORDER_DB_ID) == orderInfo.getDbId())
                getUserData().setParam(ENPPlayerParam.PENDING_ORDER_DB_ID, 0);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 代金券支付
     * @param _orderId 订单号
     * @param _context 操作上下文
     * @return 支付结果
     */
    public Result voucherPay(String _orderId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 查询订单信息
            PlayerOrderInfo orderInfo = lookupOrderByOrderId(_orderId);
            if (orderInfo == null)
                return OrderErr.ORDER_NOT_EXIST;

            // 如果订单状态不是待支付状态，则认为订单无效
            if (orderInfo.getOrderStatus() != EOrderStatus.WAIT_PAY)
            {
                USLog.warn(getUSServer(), "voucherPay order status invalid: orderId={}, status={}",
                        _orderId, orderInfo.getOrderStatus());
                return OrderErr.ORDER_CANT_OPERATE;
            }

            // 获取支付配置
            RefPay refPay = RefPay.getMgr().get(orderInfo.getPayId());
            if (refPay == null)
                return CommErr.REF_NOT_FOUND;

            // 检查是否配置了代金券物品
            if (refPay.voucher_item == null || refPay.voucher_item.getItemId() == 0)
                return OrderErr.NOT_SUPPORT_PAY_TYPE;

            long oldVoucherCount = getUserData().getItemCount(ENPItemType.BAG_ITEM, RefGeneral.Ref().voucher_item_bag_item_id);
            long oldPaidVoucherCount = getPaidVoucherCount();

            // 扣除代金券
            boolean consumeSucc = getUserData().spendItem(refPay.voucher_item, _context);
            if (!consumeSucc)
                return OrderErr.VOUCHER_NOT_ENOUGH;

            // 标记订单完成支付（代金券支付）
            long nowTimeSec = CommonFunc.getNowTimeSec();
            orderInfo.setPay("", nowTimeSec, nowTimeSec, EOrderPayType.VOUCHER);

            // 清除待处理订单标记
            if (getUserData().getParam(ENPPlayerParam.PENDING_ORDER_DB_ID) == orderInfo.getDbId())
                getUserData().setParam(ENPPlayerParam.PENDING_ORDER_DB_ID, 0);

            long newVoucherCount = getUserData().getItemCount(ENPItemType.BAG_ITEM, RefGeneral.Ref().voucher_item_bag_item_id);
            long newPaidVoucherCount = getPaidVoucherCount();

            // 记录代金券支付日志（消耗）
            MJLog.logVoucher(
                    getUserData(),
                    _context.getContextId(), // 事件ID
                    oldVoucherCount,// 旧值
                    newVoucherCount,// 新值
                    newVoucherCount - oldVoucherCount,// 改变值（负数）
                    Long.toString(orderInfo.getGoodsId()),// 商品id
                    1,// 购买数量
                    2,// action: 2=消耗
                    0,// 人民币金额
                    refPay.show_price,// 美元/人民币
                    _orderId,// 订单号
                    refPay.sdk_pay_id,// sdk_pay_id
                    oldPaidVoucherCount,// 付费代金券旧值
                    newPaidVoucherCount,// 付费代金券新值
                    newPaidVoucherCount - oldPaidVoucherCount// 付费代金券改变值
            );

            // 执行发货逻辑
            tryDelivery(orderInfo, null, refPay, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取付费代金券数量
     */
    public long getPaidVoucherCount()
    {
        SpecialItemDealer_PaidVoucher dealer =
                getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.PAID_VOUCHER, SpecialItemDealer_PaidVoucher.class);
        if (dealer == null)
            return 0;

        return dealer.getItemCount();
    }

    /**
     * 执行发货逻辑
     * @param _orderInfo
     * @param _callbackInfo
     * @param _refPay
     * @param _context
     */
    public void tryDelivery(PlayerOrderInfo _orderInfo, ServerObj_PayCallbackInfo _callbackInfo,
                            RefPay _refPay, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try {
            //修正订单的客户端通知统计次数，如修改了状态，后续需要一并修改统计次数
            boolean isChgState = _orderInfo.clientNotifyPay(_context);

            //这里验证订单状态是否可以发货
            Result result = getMgr().recordPurchase(_orderInfo.getGoodsId(), isChgState, _context);
            if (!result.isSucc())
            {
                // 更新订单状态
                _orderInfo.saveStatus(EOrderStatus.DELIVERY_FAILED);
                return;
            }

            //处理实际发货的行为
            USLog.info(getUSServer(),
                    "doDelivery: orderId={}, cid={}, goodsId={}", _orderInfo.getOrderId(), getCid(), _orderInfo.getGoodsId());

            float payMoney = 0.0f;
            String payCurrency = "";
            short sdkType = 0;
            short orderType = 0;

            // 记录充值日志
            try
            {
                //区分普通订单支付和代金券支付
                if (_orderInfo.getPayType() == EOrderPayType.PLATFORM)
                {
                    payMoney = Float.parseFloat(_callbackInfo.getPayment());
                    payCurrency = _callbackInfo.getPaymentCode();
                    sdkType = (short) _callbackInfo.getSdkType();
                    orderType = (short) _callbackInfo.getOrderType();

                    MJLog.logRecharge(getUserData(), _orderInfo.getOrderId(), _callbackInfo.getTradeId(), _callbackInfo.getSdkOrderId(),
                            _refPay.show_price, Long.toString(_orderInfo.getGoodsId()), 1, 1, (int) _orderInfo.getPayTimeSec(),
                            (int) _orderInfo.getArriveTimeSec(), (int) _orderInfo.getCreateTimeSec(),
                            sdkType, _callbackInfo.getPayId(), payMoney,
                            payCurrency, orderType
                    );
                } else if (_orderInfo.getPayType() == EOrderPayType.VOUCHER)
                {
                    payMoney = _refPay.voucher_item.getCount();
                    payCurrency = "voucher";
                    sdkType = 99;
                    orderType = 95;

                    MJLog.logRecharge(getUserData(), _orderInfo.getOrderId(), "", "",
                            _refPay.show_price, Long.toString(_orderInfo.getGoodsId()), 1, 1, (int) _orderInfo.getPayTimeSec(),
                            (int) _orderInfo.getArriveTimeSec(), (int) _orderInfo.getCreateTimeSec(),
                            sdkType, "gametoken", payMoney, payCurrency, orderType
                    );
                }
            } catch (Exception e)
            {
                USLog.error(getUSServer(), "logRecharge exception: orderId={}, cid={}", _orderInfo.getOrderId(), getCid(), e);
            }

            // 执行发货逻辑
            _orderInfo.sendOrderItem(_context, _refPay, sdkType, orderType, payMoney, payCurrency);

            // 更新订单状态
            _orderInfo.saveStatus(EOrderStatus.DELIVERY_COMPLETED);
        }
        finally {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取网页充值商品列表
     *
     * @return 网页充值商品列表
     */
    public ServerObj_WebPayGoodsList getWebPayGoodsList()
    {
        return _m_mgr.getWebPayGoodsList();
    }

}