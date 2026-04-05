
using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_004_PlayerOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_004_PlayerOp() :
            base(4, 106)
        {
            regDealer(new GSSubDealer_004_005_SetIconRes());
            regDealer(new GSSubDealer_004_007_SetIconBgkRes());
            regDealer(new GSSubDealer_004_009_SetBubbleRes());
			regDealer(new GSSubDealer_004_019_RetDrawDailyReward());

            regDealer(new GSSubDealer_004_051_PushCurrencyInfo());
            regDealer(new GSSubDealer_004_052_OnPlayerBuffRemove());
            regDealer(new GSSubDealer_004_053_OnPlayerBuffChg());
            regDealer(new GSSubDealer_004_054_OnPlayerParamUpdated());
            regDealer(new NPGSSubDealer_004_055_OnPlayerNameUpdated());
			regDealer(new GSSubDealer_004_056_OnGoldInfoChg());
			regDealer(new GSSubDealer_004_057_OnStationInfoChg());
			regDealer(new GSSubDealer_004_058_OnLoginCountChg());
			regDealer(new GSSubDealer_004_059_PushActivityCurrencyInfo());
            regDealer(new GSSubDealer_004_060_OnPlayerRecordChg());
			regDealer(new GSSubDealer_004_061_OnPlayerBuffTrigger());
			regDealer(new GSSubDealer_004_062_OnGraveNewReward());
            regDealer(new GSSubDealer_004_063_OnWeekCardChg());
            regDealer(new GSSubDealer_004_064_OnRankGiftPackChg());
            regDealer(new GSSubDealer_004_065_OnPlayerEventRecordChg());
			regDealer(new GSSubDealer_004_066_OnOrderAdd());
			regDealer(new GSSubDealer_004_067_OnOrderChg());
			regDealer(new GSSubDealer_004_068_OnForbidChatChg());
			regDealer(new GSSubDealer_004_069_OnRemoveForbidChat());

            regDealer(new GSSubDealer_004_071_OnShieldCidAdd());
            regDealer(new GSSubDealer_004_072_OnShieldCidRemove());
            regDealer(new GSSubDealer_004_074_OnPushGiftPackGroupChg());
            regDealer(new GSSubDealer_004_075_OnRushExchangeChg());
            regDealer(new GSSubDealer_004_076_OnLoverCollectChg());
            regDealer(new GSSubDealer_004_104_RetSetLoverTarget());
            regDealer(new GSSubDealer_004_105_RetClaimLover());
        }
    }
}
