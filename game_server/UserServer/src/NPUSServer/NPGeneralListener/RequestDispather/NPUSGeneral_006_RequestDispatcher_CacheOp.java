package NPUSServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.NPGeneralListener.RequestDispather.p006_CacheOp.RequestDealer_NP2US_R_006_001_GetPlayerShowInfo;
import NPUSServer.NPGeneralListener.RequestDispather.p006_CacheOp.RequestDealer_NP2US_R_006_002_GetPlayerIconShowInfo;
import NPUSServer.NPGeneralListener.RequestDispather.p006_CacheOp.RequestDealer_NP2US_R_006_004_GetPlayerJoinUSInfo;

public class NPUSGeneral_006_RequestDispatcher_CacheOp extends NPRequestDispatcher
{
    public static void init(NPUSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2US_R_006_001_GetPlayerShowInfo(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_006_002_GetPlayerIconShowInfo(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_006_004_GetPlayerJoinUSInfo(_dispather.getUSServer()));
    }
}
