package NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer;

import CommonEnum.ESpecialItemType;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.SpecialItemComponent;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent._ASpecialItemDealer;
import USDB.Bo.PlayerSpecialItemBO;

public class SpecialItemDealer_PaidVoucher extends _ASpecialItemDealer implements _IHandlerHolder
{
    public SpecialItemDealer_PaidVoucher(SpecialItemComponent _comp)
    {
        super(_comp);
    }

    @Override
    public ESpecialItemType getSpecialItemType()
    {
        return ESpecialItemType.PAID_VOUCHER;
    }

    @Override
    public void initBo(PlayerSpecialItemBO _bo)
    {
    }

    public long getItemCount()
    {
        _lock();
        try
        {
            return getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.PAID_VOUCHER_COUNT);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加
     * @param _count
     * @param _context
     */
    public void gainItem(long _count, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (_count <= 0)
                return;

            // 记录旧值用于日志
            long oldCount = getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.PAID_VOUCHER_COUNT);

            //增加金币
            long newCount = oldCount + _count;

            //保存数据
            getUserData().getRecordComponent().setRecord(ENPPlayerRecordParam.PAID_VOUCHER_COUNT, newCount, _context);
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
    public void spendItem(long _count, NPPlayerContext _context)
    {
        _lock();
        try
        {
            //消耗金币
            if (_count <= 0)
                return;

            // 记录旧值用于日志
            long oldCount = getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.PAID_VOUCHER_COUNT);

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
            getUserData().getRecordComponent().setRecord(ENPPlayerRecordParam.PAID_VOUCHER_COUNT, newCount, _context);
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
