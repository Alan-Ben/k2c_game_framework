package PayCenter.PayCallback;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.ServerObj.ServerObj_PayCallbackInfo;
import NPCommon.Log.CommLog;
import NPCommon.Util.OrderIdParser;
import PayCenter.PayCenter;
import PayDB.Bo.PayCallbackInfoBO;

public class PayCallbackInfo
{
    private PayCallbackInfoBO _m_bo;
    private OrderIdParser _m_orderIdParser;
    private MutexAtom _m_mutex;

    public PayCallbackInfo(PayCallbackInfoBO _bo)
    {
        _m_bo = _bo;
        _m_orderIdParser = OrderIdParser.parse(_bo.getOrderId());
        if (_m_orderIdParser == null)
            CommLog.error("PayCallbackInfo init error, orderId:{}", _bo.getOrderId());

        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public OrderIdParser getOrderIdParser()
    {
        return _m_orderIdParser;
    }

    public String getOrderId()
    {
        return _m_bo.getOrderId();
    }

    public long getDbId()
    {
        return _m_bo.getId();
    }

    public String getSdkOrderId()
    {
        return _m_bo.getSdkOrderId();
    }

    public long getCid()
    {
        return _m_bo.getCid();
    }

    public String getUid()
    {
        return _m_bo.getUid();
    }

    public long getPayTime()
    {
        return _m_bo.getPayTime();
    }

    public boolean hadPush()
    {
        _lock();
        try{
            return _m_bo.getHadPush();
        }finally
        {
            _unlock();
        }
    }

    /**
     * 标记该回调信息已经推送给游戏服
     */
    public void markHadPush()
    {
        _lock();
        try
        {
            _m_bo.saveHadPush(PayCenter.getInstance().getBM(), true);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 标记该回调信息的投递结果
     * @param _isFail
     */
    public void markDeliveryFail(boolean _isFail)
    {
        _lock();
        try
        {
            _m_bo.saveDeliveryFail(PayCenter.getInstance().getBM(), _isFail);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 丢弃该回调信息
     */
    public void discard()
    {
        _m_bo.del(PayCenter.getInstance().getBM());
    }

    /**
     * 构造协议对象
     *
     * 将BO对象的所有字段映射到协议对象中
     *
     * @return 支付回调信息协议对象
     */
    public ServerObj_PayCallbackInfo makeProto()
    {
        ServerObj_PayCallbackInfo proto = new ServerObj_PayCallbackInfo();
        proto.setDbId(_m_bo.getId());
        proto.setOrderId(_m_bo.getOrderId());
        proto.setSdkOrderId(_m_bo.getSdkOrderId());
        proto.setCid(_m_bo.getCid());
        proto.setUid(_m_bo.getUid());
        proto.setAppId(_m_bo.getAppId());
        proto.setProductId(_m_bo.getProductId());
        proto.setAmount(_m_bo.getAmount());
        proto.setAmountType(_m_bo.getAmountType());
        proto.setPayType(_m_bo.getPayType());
        proto.setCreateTime(_m_bo.getCreateTime());
        proto.setPayTime(_m_bo.getPayTime());
        proto.setSdkType(_m_bo.getSdkType());
        proto.setTradeId(_m_bo.getTradeId());
        proto.setSkuId(_m_bo.getSkuId());
        proto.setSdkPayId(_m_bo.getSdkPayId());
        proto.setPurchaseType(_m_bo.getPurchaseType());
        proto.setPayment(_m_bo.getPayment());
        proto.setPaymentCode(_m_bo.getPaymentCode());
        proto.setChannelCode(_m_bo.getChannelCode());
        proto.setPayId(_m_bo.getPayId());
        proto.setOrderType(_m_bo.getOrderType());
        return proto;
    }
}
