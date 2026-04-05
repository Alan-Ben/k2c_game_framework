package NPUSServer.CommonActivityMgr.Activities.RechargeRebate;

import Common.RechargeRebateEnum.ERechargeRebateType;
import NPGameRes.Refs.RechargeRebate.RefRechargeRebateGroup;

/**
 * 累计充值返利
 *
 * 功能特点：
 * - 计数方式：活动期间内累计充值获得的VIP点数
 * - 重置机制：活动期间持续累加，不重置
 * - 未领奖处理：活动结束后通过邮箱发送
 */
public class RechargeRebateInfo_Total extends _ARechargeRebateInfo
{
    public RechargeRebateInfo_Total(RechargeRebateActivity _activity, RefRechargeRebateGroup _groupRef)
    {
        super(_activity, _groupRef);
    }

    @Override
    public ERechargeRebateType getType()
    {
        return ERechargeRebateType.TOTAL;
    }

    @Override
    public void onRecharge(long _cid, long _vipExp)
    {
        RechargeRebateGroupData data = ensurePlayerData(_cid);

        // 累加VIP点数
        data.addCount(_cid, _vipExp);
    }

    @Override
    public void onCrossDay()
    {
        // 累计充值不需要每日重置
    }

}
