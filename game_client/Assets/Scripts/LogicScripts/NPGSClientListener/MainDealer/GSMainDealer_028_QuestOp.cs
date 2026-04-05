
using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_028_QuestOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_028_QuestOp() :
            base(28, 90)
        {
            regDealer(new GSSubDealer_028_050_OnPlayerQuestChg());
            regDealer(new GSSubDealer_028_051_OnPlayerQuestStepCountChg());
            regDealer(new GSSubDealer_028_052_OnPlayerQuestRemove());
            regDealer(new GSSubDealer_028_053_OnPlayerQuestCountChg());

            regDealer(new GSSubDealer_028_060_OnPlayerDailyQuestChg());
            regDealer(new GSSubDealer_028_061_OnPlayerDailyQuestActiveRewardChg());
			regDealer(new GSSubDealer_028_070_OnPlayerSystemQuestChg());
        }
    }
}
