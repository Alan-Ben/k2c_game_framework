package NPUSServer.GuildMsgDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.GuildMsgDispather.p002_InitOp.*;

public class NPUSGuild_002_RequestDispatcher_InitOp extends NPRequestDispatcher
{
    public static void init(NPUSGuildRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2US_R_002_063_ReqGuildInit(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_002_067_ReqGuildMarsHelpInit(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_002_071_ReqGuildDungeonInit(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_002_075_ReqGuildCooperateInit(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_002_082_ReqGuildBoxInit(_dispather.getUSServer()));
    }
}

