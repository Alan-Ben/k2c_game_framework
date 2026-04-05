package NPUSServer.NPUSUserMgr.UserComp.OrderComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.CommonFuncObj.GiftPack_Info;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.OrderErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.GiftPack.RefGiftPack;
import NPGameRes.Refs.RefPay;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerGiftPackRecordBO;

/**
 * 玩家礼包购买记录数据对象 - 封装礼包购买状态和业务逻辑
 *
 * 职责：
 * 1. 管理礼包的购买状态（购买次数、刷新时间等）
 * 2. 处理购买限制检查（次数限制、时间限制）
 * 3. 支持现金购买和道具购买两种模式
 * 4. 提供数据库持久化和客户端同步功能
 *
 * 数据存储策略：
 * - 使用懒加载模式，只有在需要持久化时才插入数据库
 * - 客户端和服务器购买次数分别记录，取最大值作为实际购买次数
 * - 支持购买次数定时刷新机制
 *
 * 线程安全：所有公共方法都通过组件的玩家锁进行保护
 */
public class PlayerGiftPackRecord
{
    // 所属的礼包组件，用于访问服务器资源和用户数据
    private PlayerGiftPackMgr _m_comp;
    // 礼包配置引用，缓存的配表数据
    private RefGiftPack _m_ref;

    // 数据库记录ID，0表示尚未插入数据库
    private long _m_dbId;
    // 客户端购买次数，记录通过客户端支付的购买次数
    private int _m_clientBuyCount;
    // 服务器购买次数，记录通过服务器道具购买的次数
    private int _m_buyCount;
    // 下次刷新时间戳(毫秒)，购买次数重置的时间点
    private long _m_nextRefreshTimeMs;
    // 关联的生命周期实例ID，用于活动关联礼包的生命周期管理
    private long _m_relativeLifeCycleInstanceId;

    /**
     * 纯配置构造（用于新建记录）
     */
    public PlayerGiftPackRecord(PlayerGiftPackMgr _comp, RefGiftPack _ref, long _relativeLifeCycleInstanceId)
    {
        _m_comp = _comp;
        _m_ref = _ref;
        _m_dbId = 0;  // 标记为未插入数据库
        _m_clientBuyCount = 0;
        _m_buyCount = 0;
        _m_nextRefreshTimeMs = _m_ref.buy_limit_refresh_time.getNextFreshTimeTagMS(CommonFunc.getNowTimeMS());
        _m_relativeLifeCycleInstanceId = _relativeLifeCycleInstanceId;
    }

    /**
     * 从BO构造（用于数据加载）
     */
    public PlayerGiftPackRecord(PlayerGiftPackMgr _comp, RefGiftPack _ref, PlayerGiftPackRecordBO _bo)
    {
        this(_comp, _ref, _bo.getRelativeLifeCycleInstanceId());
        _m_dbId = _bo.getId();
        _m_clientBuyCount = _bo.getClientBuyCount();
        _m_buyCount = _bo.getBuyCount();
        _m_nextRefreshTimeMs = _bo.getNextRefreshTimeMs();
    }

    public long getRelativeLifeCycleInstanceId()
    {
        return _m_relativeLifeCycleInstanceId;
    }

    protected void _lock()
    {
        _m_comp.getUserData().lockUser();
    }

    protected void _unlock()
    {
        _m_comp.getUserData().unlockUser();
    }

    public long getGiftPackId()
    {
        return _m_ref.Id();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public int getBuyCount()
    {
        return _m_buyCount;
    }

    public int getClientBuyCount()
    {
        return _m_clientBuyCount;
    }

    public long getNextRefreshTimeMs()
    {
        return _m_nextRefreshTimeMs;
    }

    public RefGiftPack getRef()
    {
        return _m_ref;
    }

    public BM getBM()
    {
        return _m_comp.getUSServer().getBM();
    }

    public int getHadBuyCount()
    {
        // 返回客户端购买次数和服务器购买次数的最大值
        return Math.max(_m_buyCount, _m_clientBuyCount);
    }

    /**
     * 检查是否达到购买限制 - 综合检查购买次数和时间限制
     *
     * 检查逻辑：
     * 1. 如果礼包没有购买次数限制，返回false（可以购买）
     * 2. 如果在刷新时间内且购买次数超限，返回true（不能购买）
     * 3. 如果已超过刷新时间，返回false（可以购买）
     *
     * @return true=达到限制不能购买，false=可以购买
     */
    public boolean reachBuyLimit()
    {
        _lock();
        try
        {
            // 如果有限制购买次数
            if (_m_ref.buy_limit_count > 0)
            {
                // 如果还没到下次刷新时间，且购买次数未超过限制，则可以购买
                if (_m_nextRefreshTimeMs > 0 && _m_nextRefreshTimeMs > CommonFunc.getNowTimeMS())
                    return getHadBuyCount() >= _m_ref.buy_limit_count;

                return false;
            }

            return false;
        } finally
        {
            _unlock();
        }
    }

    /***
     * 尝试增加服务端发货次数，只需要验证服务器发货次数
     * 如果发货次数低于合法次数则直接发货
     * 发货成功后才会调整次数，而客户端购买次数如果带入为true，则不论是否合法都需要增加
     * @param _needAddClientPay
     * @return
     */
    public boolean tryPurchase(boolean _needAddClientPay, NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 先尝试刷新次数
            refreshBuyCount(_context);

            // 如果有限制购买次数
            //判断次数是否可以发送
            if (_m_ref.buy_limit_count > 0)
            {
                //判断购买次数是否超出,不可购买则直接返回
                if(_m_buyCount >= _m_ref.buy_limit_count)
                    return false;
            }

            //累加购买次数，并根据是否需要累加客户端购买次数增加相关统计
            if(_needAddClientPay)
                recordPurchase(1, 1, _context);
            else
                recordPurchase(1, 0, _context);

            //累加次数，返回成功
            return true;
        } finally
        {
            _unlock();
        }
    }


    /**
     * 刷新购买次数 - 重置购买次数并更新下次刷新时间
     *
     * 刷新逻辑：
     * 1. 检查当前时间是否达到刷新时间点
     * 2. 如果未达到，返回限制错误
     * 3. 如果达到，重置购买次数并计算下次刷新时间
     * 4. 更新数据库（仅在有购买记录时）
     *
     * @param _context 操作上下文
     * @return 操作结果
     */
    public Result refreshBuyCount(NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 判断是否到了刷新时间
            long nowTimeMS = CommonFunc.getNowTimeMS();
            if (nowTimeMS < _m_nextRefreshTimeMs)
                return OrderErr.PURCHASE_LIMIT_REACHED;

            // 计算下次刷新时间
            _m_nextRefreshTimeMs = _m_ref.buy_limit_refresh_time.getNextFreshTimeTagMS(nowTimeMS);

            // 如果没有购买过，则只在内存做刷新时间更新
            if (_m_buyCount != 0 || _m_clientBuyCount != 0)
            {
                // 重置购买次数
                _m_buyCount = 0;
                _m_clientBuyCount = 0;

                // 更新数据库
                if (makeSureBoInsert())
                {
                    ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                    updateValue.addValueObj("buy_count", _m_buyCount);
                    updateValue.addValueObj("client_buy_count", _m_clientBuyCount);
                    updateValue.addValueObj("next_refresh_time_ms", _m_nextRefreshTimeMs);
                    getBM().getBM(PlayerGiftPackRecordBO.class).update("id", _m_dbId, updateValue);
                }
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 重置购买次数
     * @param _context
     */
    public void resetBuyCount(NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (_m_buyCount == 0 && _m_clientBuyCount == 0)
                return;

            // 重置购买次数
            _m_buyCount = 0;
            _m_clientBuyCount = 0;

            // 更新数据库
            if (makeSureBoInsert())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("buy_count", _m_buyCount);
                updateValue.addValueObj("client_buy_count", _m_clientBuyCount);
                getBM().getBM(PlayerGiftPackRecordBO.class).update("id", _m_dbId, updateValue);
            }

            _m_comp.getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_061_OnGiftPackBuyRecordChg(makeProto()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 记录购买 - 更新购买次数并同步数据库
     *
     * 更新逻辑：
     * 1. 检查当前时间是否超过刷新时间
     * 2. 如果超过，重置购买次数并更新刷新时间
     * 3. 如果未超过，累加购买次数
     * 4. 同步数据库并通知客户端更新
     *
     * @param _count 服务器购买数量
     * @param _clientBuyCount 客户端购买数量
     * @param _context 操作上下文
     */
    public void recordPurchase(int _count, int _clientBuyCount, NPPlayerContext _context)
    {
        _lock();
        try
        {
            boolean needPushChg = false;

            long nowTimeMS = CommonFunc.getNowTimeMS();
            if (nowTimeMS > _m_nextRefreshTimeMs)
            {
                // 如果当前时间已经超过下次刷新时间，则重置购买次数
                _m_nextRefreshTimeMs = _m_ref.buy_limit_refresh_time.getNextFreshTimeTagMS(nowTimeMS);
                _m_clientBuyCount = _clientBuyCount; // 客户端购买次数重置为当前购买数量
                _m_buyCount = _count;  // 购买次数重置为当前购买数量

                needPushChg = true;
            } else
            {
                int oriData = getHadBuyCount();

                _m_buyCount += _count;
                _m_clientBuyCount += _clientBuyCount;

                if (oriData != getHadBuyCount())
                    needPushChg = true;
            }

            // 更新数据库
            if (makeSureBoInsert())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("buy_count", _m_buyCount);
                updateValue.addValueObj("client_buy_count", _m_clientBuyCount);
                updateValue.addValueObj("next_refresh_time_ms", _m_nextRefreshTimeMs);
                getBM().getBM(PlayerGiftPackRecordBO.class).update("id", _m_dbId, updateValue);
            }

            if (needPushChg)
                _m_comp.getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_061_OnGiftPackBuyRecordChg(makeProto()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保BO插入数据库
     * @return true表示已存在，false表示刚插入
     */
    private boolean makeSureBoInsert()
    {
        _lock();
        try
        {
            if (_m_dbId != 0)
                return true;

            PlayerGiftPackRecordBO bo = new PlayerGiftPackRecordBO();
            bo.setCid(getBM(), _m_comp.getUserData().getCid());
            bo.setGiftPackId(getBM(), _m_ref.Id());
            bo.setClientBuyCount(getBM(), _m_clientBuyCount);
            bo.setBuyCount(getBM(), _m_buyCount);
            bo.setNextRefreshTimeMs(getBM(), _m_nextRefreshTimeMs);
            bo.setRelativeLifeCycleInstanceId(getBM(), _m_relativeLifeCycleInstanceId);
            bo.insert(getBM());

            _m_dbId = bo.getId();
            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造订单商品数据
     * @return
     */
    public ResultOne<OrderGoodsData> makeOrderGoodsData()
    {
        _lock();
        try
        {
            // 检查购买次数限制
            if (reachBuyLimit())
                return ResultOne.failed(OrderErr.PURCHASE_LIMIT_REACHED);

            // 检查礼包是否支持现金购买
            ResultOne<Long> getPayIdResult = _m_ref.getPayId();
            if (!getPayIdResult.isSucc())
                return ResultOne.failed(getPayIdResult.getResult());

            long payId = getPayIdResult.getData();
            RefPay refPay = RefPay.getMgr().get(payId);
            if (refPay == null)
            {
                USLog.error(_m_comp.getUSServer(),
                        "PlayerGiftPackRecord makeOrderGoodsData failed: no pay ref, giftPackId={}, payId={}", _m_ref.Id(), payId);
                return ResultOne.failed(OrderErr.CREATE_ORDER_FAILED);
            }

            OrderGoodsData orderGoods = new OrderGoodsData();
            orderGoods.setGoodsId(_m_ref.Id());
            orderGoods.setRefPay(refPay);

            // 注意 由于为了避免创建Record对象，所以在@PlayerGiftPackMgr.makeOrderGoodsData()中有重复逻辑，更改时需要同步
            // 添加基础奖励
            orderGoods.getItemList().addAll(_m_ref.item_list);
            // 添加额外奖励
            _m_ref.fillExtraGainItemList(getHadBuyCount() + 1, orderGoods.getItemList());
            // 添加VIP经验
            if (!_m_ref.not_send_vip_exp)
                orderGoods.getItemList().add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.VIP_EXP.ordinal(), refPay.vip_exp));

            return ResultOne.succ(orderGoods);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 购买礼包，这里是直接通过物品购买的行为
     * @param _context
     * @return
     */
    public Result itemBuy(NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 检查购买次数限制
            if (reachBuyLimit())
                return OrderErr.PURCHASE_LIMIT_REACHED;

            // 检查礼包是否支持现金购买
            ResultOne<Long> getPayIdResult = _m_ref.getPayId();
            if (getPayIdResult.isSucc())
                return OrderErr.NOT_SUPPORT_PAY_TYPE;

            if (!_m_comp.getUserData().hasCostItemList(_m_ref.cost_list))
                return CommErr.ITEM_NOT_ENOUGH;

            if (!_m_comp.getUserData().spendCostItemList(_m_ref.cost_list, _context))
                return CommErr.ITEM_NOT_ENOUGH;

            recordPurchase(1, 1, _context);

            _m_comp.getUserData().gainItemList(_m_ref.item_list, _context);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清除数据
     */
    public void discard()
    {
        if (_m_dbId == 0)
            return;

        _m_comp.getUSServer().getBM().getBM(PlayerGiftPackRecordBO.class).delAll("id", _m_dbId);
    }

    /**
     * 构造协议
     * @return
     */
    public GiftPack_Info makeProto()
    {
        _lock();
        try
        {
            GiftPack_Info info = new GiftPack_Info();
            info.setGiftPackId(_m_ref.Id());
            info.setNextRefreshTimeMs(_m_nextRefreshTimeMs);
            info.setHadBuyCount(getHadBuyCount());
            return info;
        } finally
        {
            _unlock();
        }
    }
}