using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_007_CommOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_007_CommOp() :
            base(7, 100)
        {
            regDealer(new GSSubDealer_007_001_RetDealRemoteEffect());
            regDealer(new GSSubDealer_007_050_OnGainItemList());
            regDealer(new GSSubDealer_007_051_OnCommError());
			regDealer(new GSSubDealer_007_052_OnAnnouncementVersionChg());
            regDealer(new GSSubDealer_007_054_OnOfflineRewardDel());
            regDealer(new GSSubDealer_007_055_OnOfflineRewardAdd());
			regDealer(new GSSubDealer_007_056_OnMarqueeAdd());
			regDealer(new GSSubDealer_007_058_OnMarqueeDel());
			regDealer(new GSSubDealer_007_059_PushRedDotChg());
			regDealer(new GSSubDealer_007_060_PushRedDotRemove());
			regDealer(new GSSubDealer_007_062_OnTargetRewardUpdate());
            
			regDealer(new GSSubDealer_007_063_OnQuestionnaireStart());
			regDealer(new GSSubDealer_007_064_OnQuestionnaireClose());
			regDealer(new GSSubDealer_007_065_OnQuestionnaireRewardAdd());
			regDealer(new GSSubDealer_007_066_OnClientVersionChg());
			regDealer(new GSSubDealer_007_067_OnQuestionnaireRewardChg());
			regDealer(new GSSubDealer_007_069_OnStageGoalBigStepRewardDraw());
			regDealer(new GSSubDealer_007_070_OnStageGoalTaskChg());
			regDealer(new GSSubDealer_007_071_OnStageGoalChg());
			regDealer(new GSSubDealer_007_073_OnRecuritRecordAdd());
			regDealer(new GSSubDealer_007_074_OnGachaPoolChg());
			regDealer(new GSSubDealer_007_076_OnCountdownEventChg());
			regDealer(new GSSubDealer_007_077_OnStageGoalBigStepFirstReachAdd());
			regDealer(new GSSubDealer_007_078_OnStageGoalBigStepFirstReachRewardDraw());
        }
    }
}
