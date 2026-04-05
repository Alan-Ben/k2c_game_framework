using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_034_InnOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_034_InnOp()
        : base(34, 70)
        {
            regDealer(new GSSubDealer_034_001_RetInnReceiveGuest());
			regDealer(new GSSubDealer_034_002_RetInnStationUnlock());
			regDealer(new GSSubDealer_034_003_RetInnStationUpgrade());
			regDealer(new GSSubDealer_034_004_RetInnDishUnlock());
			regDealer(new GSSubDealer_034_005_RetInnDishUpgrade());
			regDealer(new GSSubDealer_034_006_RetInnSettle());
			regDealer(new GSSubDealer_034_008_RetInnUnlockGuest());
			regDealer(new GSSubDealer_034_009_RetInnServeSpecialGuest());
			regDealer(new GSSubDealer_034_010_RetDrawGuestHandbookReward());
			regDealer(new GSSubDealer_034_011_RetDrawSpecialGuestHandbookReward());
			regDealer(new GSSubDealer_034_012_RetInnMedalUpgrade());
			regDealer(new GSSubDealer_034_050_OnInnReceiveChg());
			regDealer(new GSSubDealer_034_052_OnInnDishChg());
			regDealer(new GSSubDealer_034_053_OnInnStationChg());
			regDealer(new GSSubDealer_034_055_OnInnDishAdd());
			regDealer(new GSSubDealer_034_056_OnInnStationAdd());
			regDealer(new GSSubDealer_034_057_OnInnLevelChg());
			regDealer(new GSSubDealer_034_058_OnInnPopularityChg());
			regDealer(new GSSubDealer_034_059_OnInnMedalLevelChg());
			regDealer(new GSSubDealer_034_060_OnInnGuestChg());
			regDealer(new GSSubDealer_034_061_OnInnSpecialGuestChg());
			regDealer(new GSSubDealer_034_063_OnInnFirstTimeUpgradeTimeMsChg());
        }
    }
}