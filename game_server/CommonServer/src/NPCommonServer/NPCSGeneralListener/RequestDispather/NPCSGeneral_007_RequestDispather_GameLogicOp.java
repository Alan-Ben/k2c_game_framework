package NPCommonServer.NPCSGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPCommonServer.NPCSGeneralListener.RequestDispather.p007_GameLogicOp.RequestDealer_NP2CS_R_007_001_ReqGameLogicInstance;
import NPCommonServer.NPCSGeneralListener.RequestDispather.p007_GameLogicOp.RequestDealer_NP2CS_R_007_002_ReqGameLogicInstance;

public class NPCSGeneral_007_RequestDispather_GameLogicOp extends NPRequestDispatcher
{
    public static void init(NPCSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2CS_R_007_001_ReqGameLogicInstance());
        _dispather.regHandler(new RequestDealer_NP2CS_R_007_002_ReqGameLogicInstance());
    }
}
