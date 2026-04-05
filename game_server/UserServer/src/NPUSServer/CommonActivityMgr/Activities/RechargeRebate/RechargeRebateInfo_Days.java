package NPUSServer.CommonActivityMgr.Activities.RechargeRebate;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.RechargeRebateEnum.ERechargeRebateType;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_109_OnRechargeRebateCountChg;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RechargeRebate.RefRechargeRebateGroup;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 累天充值返利
 *
 * 功能特点：
 * - 计数方式：活动期间内累计充值天数
 * - 重置机制：每日首次充值时天数+1
 * - 未领奖处理：活动结束后通过邮箱发送
 */
public class RechargeRebateInfo_Days extends _ARechargeRebateInfo
{
    public RechargeRebateInfo_Days(RechargeRebateActivity _activity, RefRechargeRebateGroup _groupRef)
    {
        super(_activity, _groupRef);
    }

    @Override
    public ERechargeRebateType getType()
    {
        return ERechargeRebateType.DAYS;
    }

    @Override
    public void onRecharge(long _cid, long _vipExp)
    {
        RechargeRebateGroupData data = ensurePlayerData(_cid);

        int todayDate = CommonFunc.getNowTagYYYYMMDD();

        // 检查是否为当日首次充值
        int lastRechargeDate = data.getLastRechargeDate();
        if (lastRechargeDate < todayDate)
        {
            // 首次充值，天数+1
            data.addCountAndUpdateLastRechargeDate(_cid, 1, todayDate);

            // 推送计数变化
            ALSynTaskManager.getInstance().regTask(() -> {
                NPUSUserData userData = getUSServer().getUsUserMgr().lookupCacheUserData(_cid);
                if (userData != null)
                {
                    GS2GC_033_109_OnRechargeRebateCountChg proto = new GS2GC_033_109_OnRechargeRebateCountChg();
                    proto.setActivityInstanceId(getActivity().getInstanceId());
                    proto.setGroupId(_m_groupRef.id);
                    proto.setCount(data.getCount());
                    userData.sendMsgToGC(proto);
                }
            });
        }
    }

    @Override
    public void onCrossDay()
    {
        // 累天充值不需要每日重置
    }

}
