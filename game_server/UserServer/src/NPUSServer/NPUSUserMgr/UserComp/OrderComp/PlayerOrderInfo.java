package NPUSServer.NPUSUserMgr.UserComp.OrderComp;

import Common.CommonFuncObj.Order_Info;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_CommonReward;
import Common.OfflineRewardObj.Offline_OrderDelivery;
import Common.ServerObj.ServerObj_OrderPaySimpleInfo;
import Common.ServerObj.ServerObj_PayCallbackInfo;
import CommonEnum.EGiftPackLogType;
import CommonEnum.EOrderPayType;
import CommonEnum.EOrderStatus;
import CommonEnum.ESpecialItemType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.OrderErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.GiftPack.RefGiftPack;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.RefPay;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_PaidGem;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerOrderBO;
import com.google.gson.reflect.TypeToken;

import java.util.ArrayList;
import java.util.List;

/**
 * 玩家订单数据对象
 * 业务逻辑封装层，使用COLA状态机管理订单状态
 * 封装订单的完整生命周期操作，包括支付、发货、状态变更等
 */
public class PlayerOrderInfo
{
    // 订单组件引用
    private OrderComponent _m_comp;

    // 数据库对象，存储订单的持久化数据
    private PlayerOrderBO _m_bo;

    // 订单包含的物品列表（从JSON反序列化获得）
    private List<NPCommonCostItem> _m_itemList;

    /**
     * 从BO对象构造
     */
    public PlayerOrderInfo(OrderComponent _comp, PlayerOrderBO _bo)
    {
        _m_comp = _comp;
        _m_bo = _bo;
        _m_itemList = CommonFunc.googleJson().fromJson(_bo.getItemList(), new TypeToken<ArrayList<NPCommonCostItem>>()
        {
        }.getType());
    }

    protected void _lock()
    {
        getComp().getUserData().lockUser();
    }

    protected void _unlock()
    {
        getComp().getUserData().unlockUser();
    }

    // Getter方法 - 直接从BO获取
    public long getDbId()
    {
        return _m_bo.getId();
    }

    public long getCid()
    {
        return _m_bo.getCid();
    }

    public EOrderPayType getPayType()
    {
        return EOrderPayType.EOrderPayType_FromInt(_m_bo.getPayType());
    }

    public String getOrderId()
    {
        return _m_bo.getOrderId();
    }

    public long getPayId()
    {
        return _m_bo.getPayId();
    }

    public float getPrice()
    {
        RefPay refPay = RefPay.getMgr().get(getPayId());
        return refPay == null ? 0 : refPay.show_price;
    }

    public long getGoodsId()
    {
        return _m_bo.getGoodsId();
    }

    public EOrderStatus getOrderStatus()
    {
        return EOrderStatus.EOrderStatus_FromInt(_m_bo.getOrderStatus());
    }

    public long getCreateTimeMs()
    {
        return _m_bo.getCreateTimeMs();
    }

    public long getCreateTimeSec()
    {
        return _m_bo.getCreateTimeMs() / 1000;
    }

    public long getPayTimeMs()
    {
        return _m_bo.getPayTimeMs();
    }

    public long getPayTimeSec()
    {
        return _m_bo.getPayTimeMs() / 1000;
    }

    public long getArriveTimeMs()
    {
        return _m_bo.getArriveTimeMs();
    }

    public long getArriveTimeSec()
    {
        return _m_bo.getArriveTimeMs() / 1000;
    }

    public OrderComponent getComp()
    {
        return _m_comp;
    }

    /**
     * 设置支付
     * @param _sdkOrderId SDK订单号
     * @param _payTimeSec 支付时间
     * @param _nowTimeSec
     * @param _payType    支付方式类型
     */
    public void setPay(String _sdkOrderId, long _payTimeSec, long _nowTimeSec, EOrderPayType _payType)
    {
        _lock();
        try
        {
            _m_bo.setSdkOrderId(getComp().getUSServer().getBM(), _sdkOrderId);
            _m_bo.setPayTimeMs(getComp().getUSServer().getBM(), _payTimeSec * 1000L);
            _m_bo.setArriveTimeMs(getComp().getUSServer().getBM(), _nowTimeSec * 1000L);
            _m_bo.setPayType(getComp().getUSServer().getBM(), _payType.ordinal());
            _m_bo.saveOrderStatus(getComp().getUSServer().getBM(), EOrderStatus.PAY_SUCCESS.ordinal());
            _m_bo.saveAllMarked(getComp().getUSServer().getBM());

            _m_comp.getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_067_OnOrderChg(makeProto()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 保存订单状态
     * @param status
     */
    public void saveStatus(EOrderStatus status)
    {
        _lock();
        try
        {
            _m_bo.saveOrderStatus(getComp().getUSServer().getBM(), status.ordinal());

            _m_comp.getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_067_OnOrderChg(makeProto()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 开始发货，单纯发货的行为，验证由外围进行处理
     * @return
     */
    public void sendOrderItem(NPPlayerContext _context, RefPay _refPay, short _sdkType, short _orderType, float _payMoney, String _payCurrency)
    {
        _lock();
        try
        {
            ArrayList<NPCommon_ItemInfo> itemList = CommonFunc.costItemListToProto(_m_itemList);

            ServerObj_OrderPaySimpleInfo paySimpleInfo = null;
            //奖励列表
            Offline_CommonReward rewardItemListObj = new Offline_CommonReward();
            for (NPCommon_ItemInfo item : itemList)
            {
                rewardItemListObj.addItemList(item);
                if (item.getItemType() == ENPItemType.BAG_ITEM.ordinal() && item.getSubId() == RefGeneral.Ref().voucher_item_bag_item_id)
                {
                    paySimpleInfo = new ServerObj_OrderPaySimpleInfo();
                    paySimpleInfo.setOrderId(getOrderId());
                    paySimpleInfo.setMoney(_refPay.show_price);
                    paySimpleInfo.setSdkPayId(_refPay.sdk_pay_id);
                }
            }

            //客户端展示数据
            Offline_OrderDelivery orderDelivery = new Offline_OrderDelivery();
            orderDelivery.setOrderId(getOrderId());
            orderDelivery.setGiftPackId(getGoodsId());
            orderDelivery.getItemList().addAll(itemList);
            orderDelivery.setSdkOrderId(getSdkOrderId());
            orderDelivery.setPayMoney(_payMoney);
            orderDelivery.setPayCurrency(_payCurrency);
            orderDelivery.setOrderType(_orderType);

            getComp().getUserData().getOfflineRewardComponent().addReward(
                    EOfflineRewardEnum.C_ORDER_DELIVERY, paySimpleInfo == null ? null : paySimpleInfo.makePackage(),
                    rewardItemListObj.makePackage(), orderDelivery.makePackage(), _context);

            // 记录充值钻石日志
            logPaidGem(_context, _refPay, _sdkType);

        } finally
        {
            _unlock();
        }
    }

    /**
     * 记录充值钻石日志
     */
    public void logPaidGem(NPPlayerContext _context, RefPay _refPay, short _sdkType)
    {
        SpecialItemDealer_PaidGem dealer =
                getComp().getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.PAID_GEM, SpecialItemDealer_PaidGem.class);
        if (dealer == null)
        {
            USLog.error(getComp().getUSServer(), "PlayerOrderInfo.logPaidGem dealer is null, cid:{} orderId:{}",
                    getCid(), getOrderId());
            return;
        }

        RefGiftPack refGiftPack = RefGiftPack.getMgr().get(getGoodsId());
        if (refGiftPack == null)
        {
            USLog.error(getComp().getUSServer(), "PlayerOrderInfo.logPaidGem refGiftPack is null, cid:{} orderId:{} goodsId:{}",
                    getCid(), getOrderId(), getGoodsId());
            return;
        }

        if (refGiftPack.log_type == EGiftPackLogType.NONE || refGiftPack.log_type == EGiftPackLogType.GEM)
        {
            dealer.gainItem(_refPay.recharge_diamond_num,getGoodsId(), _sdkType, _context);
        }

        if (refGiftPack.log_type == EGiftPackLogType.NONE)
        {
            dealer.spendItem(_refPay.recharge_diamond_num,getGoodsId(), _sdkType, _context);
        }
    }

    /**
     * 客户端支付通知
     * 返回是否有状态变更
     * @param _context
     */
    public boolean clientNotifyPay(NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 如果已经处理过支付通知，则直接返回成功
            if (_m_bo.getHadClientNotifyPay())
                return false;

            _m_bo.saveHadClientNotifyPay(getComp().getUSServer().getBM(), true);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 取消订单
     * @param _context
     * @return
     */
    public Result cancel(NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 只能取消待支付状态的订单，且必须是当前待处理的订单
            if (getOrderStatus() != EOrderStatus.WAIT_PAY
                    || getDbId() != _m_comp.getUserData().getParam(ENPPlayerParam.PENDING_ORDER_DB_ID))
                return OrderErr.ORDER_CANT_OPERATE;

            // 取消订单
            saveStatus(EOrderStatus.PAY_CANCELED);

            // 发送取消订单消息
            getComp().getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_067_OnOrderChg(makeProto()));

            // 清除当前待处理订单
            _m_comp.getUserData().setParam(ENPPlayerParam.PENDING_ORDER_DB_ID, 0);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    public String getSdkOrderId()
    {
        _lock();
        try
        {
            return _m_bo.getSdkOrderId();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 校验sdkPayId、商品id、金额是否匹配
     * @param _refPay
     * @param _callbackInfo
     * @return
     */
    public boolean validatePlatPayInfo(RefPay _refPay, ServerObj_PayCallbackInfo _callbackInfo)
    {
        _lock();
        try
        {
            // 校验SdkPayId
            if (!_refPay.sdk_pay_id.equals(_callbackInfo.getSdkPayId()))
                return false;

            // 校验商品id
            if (getGoodsId() != _callbackInfo.getProductId())
                return false;

            // 校验金额
            if (Math.abs(_refPay.show_price - Float.parseFloat(_callbackInfo.getAmount())) > 0.01f)
            {
                USLog.error(getComp().getUSServer(), "PlayerOrderInfo.validatePlatPayInfo payId:{} orderId:{} price not match, orderPrice:{} callbackPrice:{}",
                        getPayId(), getOrderId(), getPrice(), _callbackInfo.getAmount());
                return false;
            }

            return true;
        } finally
        {
            _unlock();
        }

    }

    @Override
    public String toString()
    {
        return String.format("PlayerOrder{orderId='%s', payId=%d, status=%s}",
                getOrderId(), getPayId(), getOrderStatus());
    }

    public Order_Info makeProto()
    {
        _lock();
        try
        {
            Order_Info proto = new Order_Info();
            proto.setOrderId(getOrderId());
            proto.setPayId(getPayId());
            proto.setGiftPackId(getGoodsId());
            proto.setStatus(getOrderStatus());
            proto.setCreateTimeMs(getCreateTimeMs());
            proto.setPayTimeMs(getPayTimeMs());
            return proto;
        } finally
        {
            _unlock();
        }
    }
}