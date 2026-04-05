using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_032_GuildOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_032_GuildOp()
        : base(32, 90)
        {
            regDealer(new GSSubDealer_032_050_OnGuildShowInfoChg());
			regDealer(new GSSubDealer_032_051_OnMemberBaseInfoChg());
			regDealer(new GSSubDealer_032_052_OnGuildAnnouncementChg());
			regDealer(new GSSubDealer_032_053_OnGuildWealthChg());
			regDealer(new GSSubDealer_032_054_OnGuildMemberAdd());
			regDealer(new GSSubDealer_032_055_OnGuildMemberRemove());
			regDealer(new GSSubDealer_032_056_OnSelfGuildContributeChg());
			regDealer(new GSSubDealer_032_057_OnSelfRequestJoinGuildListChg());
			regDealer(new GSSubDealer_032_058_OnJoinGuildCdChg());
			regDealer(new GSSubDealer_032_059_OnGuildEventAdd());
			regDealer(new GSSubDealer_032_060_OnGuildEventChg());
			regDealer(new GSSubDealer_032_061_OnGuildEventRemove());
			regDealer(new GSSubDealer_032_062_OnGuildTotalEarningsChg());
			regDealer(new GSSubDealer_032_063_OnGuildJoinRequestAdd());
			regDealer(new GSSubDealer_032_064_OnGuildJoinRequestRemove());
			regDealer(new GSSubDealer_032_065_OnJoinGuild());
			regDealer(new GSSubDealer_032_066_OnLeaveGuild());
			regDealer(new GSSubDealer_032_067_OnGuildConstructListChg());
			regDealer(new GSSubDealer_032_068_OnOpenRecruitCdChg());
			regDealer(new GSSubDealer_032_071_OnGuildEntrustChg());
			regDealer(new GSSubDealer_032_072_OnGuildDispatchChg());
			regDealer(new GSSubDealer_032_073_OnSelfDailyDataChg());
			regDealer(new GSSubDealer_032_075_OnRecommendRewardPointChange());
			regDealer(new GSSubDealer_032_076_OnPropertyPointInfoChange());
			regDealer(new GSSubDealer_032_077_OnRewardPointDefeat());
			regDealer(new GSSubDealer_032_078_OnHadDrawRewardPointListChg());
			regDealer(new GSSubDealer_032_079_OnHeroUseInfoChange());
			regDealer(new GSSubDealer_032_080_OnRewardPointUnlock());
			regDealer(new GSSubDealer_032_081_OnGuildMarsBattleReportAdd());
        }
    }
}