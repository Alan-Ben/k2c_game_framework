using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_041_MarsExploreOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_041_MarsExploreOp()
        : base(41, 70)
        {
			regDealer(new GSSubDealer_041_001_RetBuildExploreEventByPos());
			regDealer(new GSSubDealer_041_002_RetBuildExploreEvent());
            regDealer(new GSSubDealer_041_003_RetSetExploreTeamHero());
			regDealer(new GSSubDealer_041_004_RetSetExploreTeamName());
			regDealer(new GSSubDealer_041_005_RetStartDealExploreEvent());
			regDealer(new GSSubDealer_041_006_RetGetBattleDoneReward());
			regDealer(new GSSubDealer_041_007_RetStartExploreTeamRepair());
			regDealer(new GSSubDealer_041_008_RetNoticeExploreTeamState());
			regDealer(new GSSubDealer_041_009_RetGetBossDoneReward());
			regDealer(new GSSubDealer_041_010_RetForwardCollectMine());
			regDealer(new GSSubDealer_041_011_RetMarsMineInfo());
            regDealer(new GSSubDealer_041_013_RetNoticeMarsMine());
            regDealer(new GSSubDealer_041_015_RetGetTempBuildingQueue());
			regDealer(new GSSubDealer_041_016_RetSetTempRepairDone());
			regDealer(new GSSubDealer_041_017_RetGuildMineShareList());
			regDealer(new GSSubDealer_041_018_RetGuildMateForwardCollectMine());
			regDealer(new GSSubDealer_041_020_RetCancelExploreTeamRepair());
			regDealer(new GSSubDealer_041_021_RetStartAndFinishRepair());
			regDealer(new GSSubDealer_041_024_RetGuildShareMineHadAttackByOthersTag());
			regDealer(new GSSubDealer_041_025_RetShareMarsMineToGuildChat());
			regDealer(new GSSubDealer_041_050_OnExploreChg());
			regDealer(new GSSubDealer_041_051_OnExploreEventAdd());
			regDealer(new GSSubDealer_041_052_OnExploreEventDel());
			regDealer(new GSSubDealer_041_055_OnExploreEventDone());
			regDealer(new GSSubDealer_041_056_OnExploreTeamNameChg());
			regDealer(new GSSubDealer_041_057_OnExploreTeamHeroChg());
			regDealer(new GSSubDealer_041_058_OnExploreTeamState());
			regDealer(new GSSubDealer_041_059_OnMineAdd());
			regDealer(new GSSubDealer_041_060_OnMineDel());
			regDealer(new GSSubDealer_041_061_OnMarsExplorePVPLogAdd());
			regDealer(new GSSubDealer_041_062_OnExploreTeamLossValueChg());
			regDealer(new GSSubDealer_041_063_OnGuildMarsMineShareAdd());
        }
    }
}