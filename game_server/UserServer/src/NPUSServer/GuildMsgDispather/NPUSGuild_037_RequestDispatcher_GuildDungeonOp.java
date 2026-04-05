package NPUSServer.GuildMsgDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.GuildMsgDispather.p037_GuildDungeonOp.*;

public class NPUSGuild_037_RequestDispatcher_GuildDungeonOp extends NPRequestDispatcher
{
    public static void init(NPUSGuildRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2US_R_037_001_ReqDungeontGlobalSet(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_002_ReqSetAutoStartDungeon(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_003_ReqStartDungeon(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_004_ReqUpgradeDungeonLvl(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_005_ReqAttackDungeon(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_007_ReqDamageRank(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_008_ReqGainAllDungeonReward(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_010_ReqGetDungeonLogList(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_011_ReqGainDungeonReward(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_037_012_ReqSetTagMonsterList(_dispather.getUSServer()));
    }
}

