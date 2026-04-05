package NPUSServer.GuildMsgDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.GuildMsgDispather.p041_MarsExploreOp.*;

public class NPUSGuild_041_RequestDispatcher_GuildExploreOp extends NPRequestDispatcher
{
    public static void init(NPUSGuildRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2US_R_041_017_ReqGuildMineShareList(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_041_018_ReqGuildMateForwardCollectMine(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_041_019_ReqGuildShareMineInfo(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_041_023_ReqGuildMarsMineShareFlag(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_041_024_ReqGuildShareMineHadAttackByOthersTag(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_041_025_ReqShareMarsMineToGuildChat(_dispather.getUSServer()));
    }
}

