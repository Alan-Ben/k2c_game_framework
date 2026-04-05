package NPUSServer.NPUserMsgDispather.Write;

import GS2GC.p033_SimpleActivityOp.*;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EarningsGoalActivity;
import NPUSServer.CommonActivityMgr.Activities.RechargeRebate.RechargeRebateActivity;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 033 协议writer
 */
public class US2GCWriter_033_SimpleActivityOp
{
    public static GS2GC_033_001_RetEarningsGoalInfo make_001_RetEarningsGoalInfo(EarningsGoalActivity _activity, long _cid)
    {
        GS2GC_033_001_RetEarningsGoalInfo proto = new GS2GC_033_001_RetEarningsGoalInfo();
        _activity.fillProto(proto, _cid);
        return proto;
    }

    public static GS2GC_033_002_RetEarningsGoalDrawReward make_002_RetEarningsGoalDrawReward()
    {
        return new GS2GC_033_002_RetEarningsGoalDrawReward();
    }

    public static GS2GC_033_003_RetEarningsGoalDrawHonorReward make_003_RetEarningsGoalDrawHonorReward()
    {
        return new GS2GC_033_003_RetEarningsGoalDrawHonorReward();
    }

    public static GS2GC_033_004_RetSevenDayGoalsInfo make_004_RetSevenDayGoalsInfo(NPUSUserData _userData)
    {
        GS2GC_033_004_RetSevenDayGoalsInfo proto = new GS2GC_033_004_RetSevenDayGoalsInfo();
        _userData.getSevenDayGoalsComponent().fillProto(proto);
        return proto;
    }

    public static GS2GC_033_005_RetSevenDayGoalsDrawReward make_005_RetSevenDayGoalsDrawReward()
    {
        return new GS2GC_033_005_RetSevenDayGoalsDrawReward();
    }

    public static GS2GC_033_006_RetSevenDayGoalsDrawStepReward make_006_RetSevenDayGoalsDrawStepReward()
    {
        return new GS2GC_033_006_RetSevenDayGoalsDrawStepReward();
    }

    /**
     * 返回充值返利信息
     */
    public static GS2GC_033_007_RetRechargeRebateInfo make_007_RetRechargeRebateInfo(RechargeRebateActivity _activity, long _cid)
    {
        GS2GC_033_007_RetRechargeRebateInfo proto = new GS2GC_033_007_RetRechargeRebateInfo();
        _activity.fillProto(proto, _cid);
        return proto;
    }

    /**
     * 返回领取充值返利奖励结果（空响应）
     */
    public static GS2GC_033_008_RetRechargeRebateDrawReward make_008_RetRechargeRebateDrawReward()
    {
        return new GS2GC_033_008_RetRechargeRebateDrawReward();
    }
}
