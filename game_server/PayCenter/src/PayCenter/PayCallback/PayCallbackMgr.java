package PayCenter.PayCallback;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexObject;
import Common.ServerObj.ServerObj_PayCallbackInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.OrderIdParser;
import NPServerProtocolWriter.NP2US.Request.NP2US_R_Writer_004_PayOp;
import PayCenter.Http.HttpContorller.PayCallbackData;
import PayCenter.PayCenter;
import PayCenter.PayServer.PayServer;
import PayDB.Bo.PayCallbackInfoBO;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_RB.p004_PayOp.NP2US_RB_004_001_RetRechargeNotify;
import WCGCommon.Enum.NPEnum;

import java.util.ArrayList;
import java.util.List;

public class PayCallbackMgr
{
    private static PayCallbackMgr _g_instance = new PayCallbackMgr();

    public static PayCallbackMgr getInstance()
    {
        return _g_instance;
    }

    // 待推送列表
    private List<PayCallbackInfo> _m_callbackList;
    private MutexObject _m_mutex;

    private PayCallbackMgr()
    {
        _m_callbackList = new ArrayList<>();
        _m_mutex = new MutexObject();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 初始化
     * @return
     */
    public boolean init()
    {
        // 从数据库加载已有的回调信息
        List<PayCallbackInfoBO> boList = PayCenter.getInstance().getBM().getBM(PayCallbackInfoBO.class).s_findAll();
        if (boList == null)
        {
            CommLog.error("PayCallbackMgr init failed to load PayCallbackInfoBO from DB");
            return false;
        }

        for (PayCallbackInfoBO bo : boList)
        {
            PayCallbackInfo info = new PayCallbackInfo(bo);
            _m_callbackList.add(info);
        }

        return true;
    }

    /**
     * 查询回调信息 通过订单ID
     * @param _orderId
     * @return
     */
    public PayCallbackInfo lookupCallbackInfo(String _orderId)
    {
        _lock();
        try
        {
            for (PayCallbackInfo info : _m_callbackList)
            {
                if (info.getOrderId().equals(_orderId))
                    return info;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查询回调信息 通过实例ID
     * @param _dbId
     * @return
     */
    public PayCallbackInfo lookupCallbackInfoByDbId(long _dbId)
    {
        _lock();
        try
        {
            for (PayCallbackInfo info : _m_callbackList)
            {
                if (info.getDbId() == _dbId)
                    return info;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 添加回调信息
     * @param _data
     */
    public Result addPayCallbackInfo(PayCallbackData _data)
    {
        // 支付回调信息
        PayCallbackInfo callbackInfo = null;

        // 防止重复添加
        _lock();
        try
        {
            if (lookupCallbackInfo(_data.getOrderId()) != null)
                return Result.SUCC;

            // 创建新的BO对象
            PayCallbackInfoBO bo = new PayCallbackInfoBO();
            BM bm = PayCenter.getInstance().getBM();

            // 映射PayCallbackData字段到PayCallbackInfoBO
            bo.setOrderId(bm, _data.getAppOrderId());
            bo.setSdkOrderId(bm, _data.getOrderId());
            bo.setUid(bm, _data.getUid());
            bo.setCid(bm, _data.getRoleId());
            bo.setAppId(bm, _data.getAppId());
            bo.setProductId(bm, _data.getProductId());
            bo.setAmount(bm, _data.getAmount());
            bo.setAmountType(bm, _data.getPayType());
            bo.setPayType(bm, _data.getPayType());
            bo.setCreateTime(bm, _data.getCreateTime());
            bo.setPayTime(bm, _data.getPayTime());
            bo.setExtension(bm, _data.getExtension());
            bo.setSdkType(bm, _data.getSdkType());
            bo.setTradeId(bm, _data.getTradeId());
            bo.setSkuId(bm, _data.getSkuId());
            bo.setSdkPayId(bm, _data.getSdkPayId());
            bo.setPurchaseType(bm, _data.getPurchaseType());
            bo.setPayment(bm, _data.getPayment());
            bo.setPaymentCode(bm, _data.getPaymentCode());
            bo.setChannelCode(bm, _data.getChannelCode());
            bo.setPayId(bm, _data.getPayId());
            bo.setOrderType(bm, _data.getOrderType());

            // 插入数据库
            bo.insert(bm);

            // 创建PayCallbackInfo封装对象并添加到列表
            callbackInfo = new PayCallbackInfo(bo);
            _m_callbackList.add(callbackInfo);
        } finally
        {
            _unlock();
        }

        // 推送充值事件到UserServer
        notifyUserServerForRecharge(callbackInfo);

        return Result.SUCC;
    }

    /**
     * 推送充值事件到UserServer
     * @param _payCallbackInfo 支付回调数据
     */
    private void notifyUserServerForRecharge(PayCallbackInfo _payCallbackInfo)
    {
        // 1. 解析订单号获取服务器信息
        OrderIdParser parser = _payCallbackInfo.getOrderIdParser();
        if (parser == null)
        {
            CommLog.error("PayCallbackMgr invalid app_order_id format: {}", _payCallbackInfo.getOrderId());
            return;
        }

        // 2. 根据订单的platformId和areaId查找对应的PayServer
        int platformId = parser.getPlatformId();
        int areaId = parser.getAreaId();

        PayServer targetPayServer = PayCenter.getInstance().lookupPayServer(platformId, areaId);
        if (targetPayServer == null)
        {
            CommLog.error("PayCallbackMgr no PayServer found for platformId={}, areaId={}, orderId={}",
                    platformId, areaId, _payCallbackInfo.getOrderId());
            return;
        }

        // 3. 获取UserServer ID（区服ID）
        int userServerId = parser.getServerTypeId();

        CommLog.info("PayCallbackMgr notifying UserServer for recharge: orderId={}, platformId={}, areaId={}, userServerId={}, cid={}",
                _payCallbackInfo.getOrderId(), platformId, areaId, userServerId, parser.getCid());

        // 4. 构造充值通知协议
        String orderId = _payCallbackInfo.getOrderId();      // UserServer订单号

        _IALProtocolStructure protocol = NP2US_R_Writer_004_PayOp.make_004_001_ReqRechargeNotify(_payCallbackInfo.makeProto());

        // 5. 通过对应的PayServer发送协议到UserServer
        targetPayServer.sendRequestToBSServer(
                NPEnum.EServerType.USER.ordinal(),
                userServerId,
                protocol,
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_004_001_RetRechargeNotify();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        NP2US_RB_004_001_RetRechargeNotify ret = (NP2US_RB_004_001_RetRechargeNotify) _retProto;

                        // 记录已经推送
                        _payCallbackInfo.markHadPush();
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.warn("PayCallbackMgr notify UserServer failed: orderId={}, platformId={}, areaId={}, cid={}, errCode={}"
                                , orderId, platformId, areaId, _payCallbackInfo.getCid(), _errCode);
                    }
                });
    }

    /**
     * us通知已经获取数据
     * @param _dbIdList
     */
    public void notifyHadGetData(List<Long> _dbIdList)
    {
        _lock();
        try
        {
            for (long dbId : _dbIdList)
            {
                PayCallbackInfo info = lookupCallbackInfoByDbId(dbId);
                if (info != null)
                {
                    info.markHadPush();
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * us通知已处理数据
     */
    public void notifyHadProcessedData(long _dbId, boolean _isDeliverySuccess)
    {
        _lock();
        try
        {
            PayCallbackInfo info = lookupCallbackInfoByDbId(_dbId);
            if (info != null)
            {
                // 根据发货结果处理
                if (_isDeliverySuccess)
                {
                    _m_callbackList.remove(info);
                    info.discard();
                }else
                {
                    info.markDeliveryFail(true);
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造待推送列表 - 根据UserServer信息筛选待推送的充值回调数据
     *
     * 执行流程：
     * 1. 遍历所有待推送的回调信息
     * 2. 根据订单号解析出平台ID、区服ID、服务器ID
     * 3. 匹配指定的platformId、platAreaId、usId
     * 4. 将匹配的回调信息转换为协议对象并返回
     *
     * @param _platformId 平台ID
     * @param _platAreaId 区服ID
     * @param _usId UserServer服务器ID
     * @return 匹配的回调信息列表，转换为协议对象格式
     *
     * 线程安全：通过_lock()/_unlock()保护_m_waitPushList访问
     */
    public ArrayList<ServerObj_PayCallbackInfo> makeCallbackListByUs(int _platformId, int _platAreaId, int _usId)
    {
        ArrayList<ServerObj_PayCallbackInfo> resultList = new ArrayList<>();

        _lock();
        try
        {
            // 遍历所有待推送的回调信息
            for (PayCallbackInfo callbackInfo : _m_callbackList)
            {
                if (callbackInfo.hadPush())
                    continue;

                OrderIdParser parser = callbackInfo.getOrderIdParser();
                if (parser == null)
                {
                    // 订单号解析失败，跳过该记录
                    CommLog.error("PayCallbackMgr makeCallbackListByUs skip invalid orderId: {}",
                                 callbackInfo.getOrderId());
                    continue;
                }

                // 检查平台ID、区服ID、服务器ID是否匹配
                if (parser.getPlatformId() == _platformId &&
                    parser.getAreaId() == _platAreaId &&
                    parser.getServerTypeId() == _usId)
                {
                    // 匹配成功，转换为协议对象并添加到结果列表
                    resultList.add(callbackInfo.makeProto());
                }
            }
        } finally
        {
            _unlock();
        }

        return resultList;
    }
}
