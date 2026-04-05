package GameLogicServer.GeneralListener.RequestDispather;

import GameLogicServer.GeneralListener.RequestDispather.p001_BasicOp.*;
import NPCommon.Dispather.NPRequestDispatcher;

public class GOMCDGeneral_001_RequestDispatcher extends NPRequestDispatcher
{
    public static void init(GOMCDGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2GLS_R_001_005_RegGroupInstance());
        _dispather.regHandler(new RequestDealer_NP2GLS_R_001_006_DiscardInstance());
        _dispather.regHandler(new RequestDealer_NP2GLS_R_001_007_QuitInstance());
        _dispather.regHandler(new RequestDealer_NP2GLS_R_001_008_RegUs());
        _dispather.regHandler(new RequestDealer_NP2GLS_R_001_009_SyncGroupTeamData());
        _dispather.regHandler(new RequestDealer_NP2GLS_R_001_010_RemoveGroupByTeam());
        _dispather.regHandler(new RequestDealer_NP2GLS_R_001_020_DealMsg());
    }
}
