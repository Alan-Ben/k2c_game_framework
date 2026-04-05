using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_033_SimpleActivityOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_033_SimpleActivityOp()
        : base(33, 120)
        {
			regDealer(new GSSubDealer_033_004_RetSevenDayGoalsInfo());
			regDealer(new GSSubDealer_033_005_RetSevenDayGoalsDrawReward());
			regDealer(new GSSubDealer_033_006_RetSevenDayGoalsDrawStepReward());
			regDealer(new GSSubDealer_033_101_OnEarningsGoalRewardReach());
			regDealer(new GSSubDealer_033_102_OnEarningsGoalHonorRewardReach());
			regDealer(new GSSubDealer_033_103_OnSevenDayGoalTaskChg());
			regDealer(new GSSubDealer_033_104_OnSevenDayGoalScoreChg());
            regDealer(new GSSubDealer_033_105_OnEarningsGoalRewardDraw());
			regDealer(new GSSubDealer_033_106_OnEarningsGoalHonorRewardDraw());
			regDealer(new GSSubDealer_033_107_OnSevenDayGoalRewardDraw());
			regDealer(new GSSubDealer_033_108_OnSevenDayGoalStepRewardDraw());
			regDealer(new GSSubDealer_033_109_OnRechargeRebateCountChg());
			regDealer(new GSSubDealer_033_110_OnRechargeRebateRewardDraw());
			regDealer(new GSSubDealer_033_111_OnRechargeRebateGroupInfoChg());
        }
    }
}