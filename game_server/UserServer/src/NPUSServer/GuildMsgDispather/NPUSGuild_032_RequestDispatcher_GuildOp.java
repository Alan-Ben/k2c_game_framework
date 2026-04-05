package NPUSServer.GuildMsgDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.GuildMsgDispather.p032_GuildOp.*;

public class NPUSGuild_032_RequestDispatcher_GuildOp extends NPRequestDispatcher
{
    public static void init(NPUSGuildRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2US_R_032_002_ReqJoinGuild(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_004_ReqOtherGuildInfo(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_005_ReqTransferGuild(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_006_ReqDissolveGuild(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_007_ReqGuildPositionAppoint(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_008_ReqChgGuildFlag(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_009_ReqChgGuildName(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_010_ReqChgGuildDeclaration(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_011_ReqChgGuildAnnouncement(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_012_ReqSetGuildJoinType(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_013_ReqProcessGuildJoinRequest(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_014_ReqGuildKickOutMember(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_015_ReqGuildBroadcastMessage(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_016_ReqLeaveGuild(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_017_ReqGuildConstruct(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_018_ReqGuildImpeachLeader(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_019_ReqMemberContribute(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_023_ReqGuildJoinRequestAKeyDeal(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_025_ReqCancelLeaderImpeachEvent(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_026_ReqApproveLeaderImpeachEvent(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_027_ReqOpenRecruit(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_033_ReqGuildEntrust(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_034_ReqGuildDispatchHero(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_035_ReqGuildAllMemberEntrustInfo(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_037_ReqDrawConstructReward(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_039_ReqGuildDispatchHeroList(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_040_ReqGuildLogList(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_041_ReqGuildCooperateAttackLogList(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_042_ReqSetRecommendRewardPoint(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_043_ReqDrawRewardPointReward(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_044_ReqAttackPropertyPoint(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_046_ReqGuildCooperateDamageRank(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_047_ReqGuildIconShow(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_032_048_ReqGuildMarsBattleReportList(_dispather.getUSServer()));
    }
}

