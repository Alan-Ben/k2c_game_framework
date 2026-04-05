package NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer;

import CommonEnum.ESpecialItemType;
import MJLog.MJLog;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.SpecialItemComponent;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent._ASpecialItemDealer;
import USDB.Bo.PlayerSpecialItemBO;

public class SpecialItemDealer_PaidGem extends _ASpecialItemDealer implements _IHandlerHolder
{
    public SpecialItemDealer_PaidGem(SpecialItemComponent _comp)
    {
        super(_comp);
    }

    @Override
    public ESpecialItemType getSpecialItemType()
    {
        return ESpecialItemType.PAID_GEM;
    }

    @Override
    public void initBo(PlayerSpecialItemBO _bo)
    {
    }

    /**
     * 增加
     * @param _count
     * @param _context
     */
    public void gainItem(long _count, long _goodsId, short _sdkType, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (_count <= 0)
                return;

            // 记录旧值用于日志
            long oldCount = getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.PAID_GEM_COUNT);

            //增加金币
            long newCount = oldCount + _count;

            //保存数据
            getUserData().getRecordComponent().setRecord(ENPPlayerRecordParam.PAID_GEM_COUNT, newCount, _context);

            // 记录充值钻石日志（获得）
            MJLog.logRechargeDiamond(
                    getComp().getUserData(),
                    _goodsId,  // 商品ID，根据实际业务场景填充
                    _context.getContextId(),  // 事件ID
                    oldCount,  // 旧值
                    newCount,  // 新值
                    _count,  // 变化值
                    _sdkType,  // 订单类型，根据实际业务场景填充
                    1  // 操作类型：1=获得
            );
        } finally
        {
            _unlock();
        }
    }

    /**
     * 消耗
     * @param _count
     * @param _context
     * @return
     */
    public void spendItem(long _count, long _goodsId, short _sdkType, NPPlayerContext _context)
    {
        _lock();
        try
        {
            //消耗金币
            if (_count <= 0)
                return;

            // 记录旧值用于日志
            long oldCount = getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.PAID_GEM_COUNT);

            //没有金币可消耗
            if (oldCount == 0)
                return;

            long newCount;

            //扣除金币
            if (_count >= oldCount)
            {
                newCount = 0;
            } else
            {
                newCount = oldCount - _count;
            }

            //保存数据
            getUserData().getRecordComponent().setRecord(ENPPlayerRecordParam.PAID_GEM_COUNT, newCount, _context);

            // 记录充值钻石日志（消耗）
            MJLog.logRechargeDiamond(
                    getComp().getUserData(),
                    _goodsId,  // 商品ID，根据实际业务场景填充
                    _context.getContextId(),  // 事件ID
                    oldCount,  // 旧值
                    newCount,  // 新值
                    oldCount - newCount,  // 变化值（实际消耗的数量）
                    _sdkType,  // 订单类型，根据实际业务场景填充
                    2  // 操作类型：2=消耗
            );
        } finally
        {
            _unlock();
        }
    }

    @Override
    public void onInited()
    {
    }

    @Override
    public void dispose()
    {
    }
}
