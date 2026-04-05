using ALBasicProtocolPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOE
{
    public class GSMainDealer_002_InitOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_002_InitOp()
            : base(2, 255)
        {
            regDealer(new NPGSSubDealer_002_001_RetPlayerInfo());
            regDealer(new NPGSSubDealer_002_003_RetCurrencyList());
			regDealer(new GSSubDealer_002_004_RetEquipInit());
			regDealer(new GSSubDealer_002_005_RetPlayerSkinInit());
            regDealer(new GSSubDealer_002_006_RetTravelInit());
            regDealer(new NPGSSubDealer_002_007_RetBagItemList());
            regDealer(new NPGSSubDealer_002_008_RetPlayerBuffList());
			regDealer(new GSSubDealer_002_009_RetAnecdoteInit());
			regDealer(new GSSubDealer_002_010_RetBuildingInit());
			regDealer(new GSSubDealer_002_011_RetGoldInit());
            regDealer(new NPGSSubDealer_002_012_RetHeroInit());
            regDealer(new GSSubDealer_002_013_RetChildList());
            regDealer(new NPGSSubDealer_002_014_RetMailStatInfo());
            regDealer(new GSSubDealer_002_015_RetConsortList());
            regDealer(new GSSubDealer_002_018_RetActivityList());
            regDealer(new NPGSSubDealer_002_019_RetChapterInit());
            regDealer(new NPGSSubDealer_002_020_RetPlayerTitleList());
            regDealer(new NPGSSubDealer_002_021_RetPlayerIconList());
            regDealer(new NPGSSubDealer_002_022_RetPlayerIconBgkList());
            regDealer(new NPGSSubDealer_002_023_RetPlayerBubbleList());
			regDealer(new GSSubDealer_002_025_RetArenaInit());
			regDealer(new GSSubDealer_002_027_RetPrivilegeCardInit());
			regDealer(new GSSubDealer_002_028_RetChatEmoteGroupInit());
			regDealer(new GSSubDealer_002_029_RetPlayerFuncUnlockInit());
            regDealer(new NPGSSubDealer_002_030_RetCuteActorInit());
			regDealer(new GSSubDealer_002_031_RetPlayerStationInit());

            regDealer(new NPGSSubDealer_002_032_RetPlayerLazyCDList());
			regDealer(new GSSubDealer_002_033_RetMarqueeInit());
			regDealer(new GSSubDealer_002_034_RetTargetRewardInit());
            regDealer(new NPGSSubDealer_002_036_RetQuestInit());
			regDealer(new GSSubDealer_002_037_RetWeekCardInit());

            regDealer(new NPGSSubDealer_002_038_RetRecordInit());
            regDealer(new GSSubDealer_002_039_RetQuestCountInit());     
            regDealer(new GSSubDealer_002_041_RetRecuritInfo());
			regDealer(new GSSubDealer_002_042_RetQuestionnaireInit());
            regDealer(new NPGSSubDealer_002_043_RetEventRecordInit());
			regDealer(new GSSubDealer_002_044_RetTowerInit());
			regDealer(new GSSubDealer_002_045_RetMiddayDungeonInit());

            regDealer(new GSSubDealer_002_046_RetDailyQuest());
            regDealer(new GSSubDealer_002_047_RetGachaInit());
            regDealer(new NPGSSubDealer_002_048_RetFixedCdInfo());
            regDealer(new NPGSSubDealer_002_049_RetFriendInit());
            regDealer(new GSSubDealer_002_050_RetAchieveInit());
            regDealer(new NPGSSubDealer_002_051_RetShopInit());
            regDealer(new NPGSSubDealer_002_052_RetEveningDungeonInit());
			regDealer(new GSSubDealer_002_053_RetLoginCountInit());
            regDealer(new NPGSSubDealer_002_054_RetOfflineRewardInit());
			regDealer(new GSSubDealer_002_055_RetConsortChatInit());
			regDealer(new GSSubDealer_002_056_RetCountdownEventInit());
			regDealer(new GSSubDealer_002_057_RetInnInit());
			regDealer(new GSSubDealer_002_059_RetPlayerPermissionsInit());
            regDealer(new NPGSSubDealer_002_060_RetDinnerInit());
			regDealer(new GSSubDealer_002_061_RetShieldInit());
			regDealer(new GSSubDealer_002_062_RetStageGoalInit());
			regDealer(new GSSubDealer_002_063_RetGuildInit());
			regDealer(new GSSubDealer_002_064_RetActivityCurrencyList());
			regDealer(new GSSubDealer_002_065_RetSystemQuestInit());
			regDealer(new GSSubDealer_002_066_RetTreasureHuntInit());
			regDealer(new GSSubDealer_002_068_RetGraveInit());
			regDealer(new GSSubDealer_002_070_RetGiftPackInit());
			regDealer(new GSSubDealer_002_067_RetGuildMarsHelpInit());
			regDealer(new GSSubDealer_002_071_RetGuildDungeonInit());
			regDealer(new GSSubDealer_002_072_RetMarsGoRouteInit());
			regDealer(new GSSubDealer_002_075_RetGuildCooperateInit());
			regDealer(new GSSubDealer_002_080_RetRedDotInit());
			regDealer(new GSSubDealer_002_081_RetPushGiftPackList());
			regDealer(new GSSubDealer_002_082_RetGuildBoxInit());
			regDealer(new GSSubDealer_002_083_RetActivityFundInit());
			regDealer(new GSSubDealer_002_085_RetRushExchangeInit());
			regDealer(new GSSubDealer_002_088_RetLoverCollectInit());
			regDealer(new GSSubDealer_002_089_RetForbidChatInit());
			regDealer(new GSSubDealer_002_255_OnQuestCountInit());
        }
    }
}
