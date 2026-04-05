package NPUSServer.GuildMsgDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.GuildMsgDispather.p042_GuildRelatedOp.*;

public class NPUSGuild_042_RequestDispatcher_GuildRelatedOp extends NPRequestDispatcher
{
    public static void init(NPUSGuildRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2US_R_042_001_ReqSendMarsHelp(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_042_002_ReqDealMarsHelp(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_042_003_ReqMarsHelpList(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_042_004_ReqMarsHelpDealedList(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_042_008_ReqSetGuildBoxShareAnonymous(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_042_009_ReqMarsHelpAutoDailyRecord(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_042_060_ReqCreateRally(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_042_061_ReqJoinRally(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_042_062_ReqQueryRallyInfo(_dispather.getUSServer()));
    }
}

