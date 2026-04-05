using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_017_ActivityOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_017_ActivityOp()
        : base(17, 70)
        {
			regDealer(new GSSubDealer_017_018_RetActivityFundDrawStepReward());
			regDealer(new GSSubDealer_017_019_RetActivityFundTaskInfo());
            regDealer(new GSSubDealer_017_050_OnActivityAdd());
            regDealer(new GSSubDealer_017_051_OnActivityChg());
            regDealer(new GSSubDealer_017_052_OnActivityRemove());
			regDealer(new GSSubDealer_017_053_OnActivityRankScoreChg());
			regDealer(new GSSubDealer_017_056_OnActivityStateChg());
			regDealer(new GSSubDealer_017_057_OnActivityStepRewardScoreChg());
			regDealer(new GSSubDealer_017_058_OnActivityShopRefresh());
			regDealer(new GSSubDealer_017_059_OnActivityCrystalGiftPackRefresh());
			regDealer(new GSSubDealer_017_060_OnActivityShopItemBuy());
			regDealer(new GSSubDealer_017_061_OnActivityCrystalGiftPackItemBuy());
			regDealer(new GSSubDealer_017_062_OnActivityHotRefInfoChg());
			regDealer(new GSSubDealer_017_063_OnActivityStepRewardEventTaskScoreChg());
			regDealer(new GSSubDealer_017_064_OnActivityFundScoreChg());
			regDealer(new GSSubDealer_017_066_OnActivityFundDrawRewardChg());
			regDealer(new GSSubDealer_017_067_OnActivityFundAdd());
			regDealer(new GSSubDealer_017_068_OnActivityFundRemove());
        }
    }
}