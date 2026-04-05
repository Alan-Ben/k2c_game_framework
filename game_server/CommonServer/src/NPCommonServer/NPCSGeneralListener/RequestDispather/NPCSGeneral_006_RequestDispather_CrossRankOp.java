package NPCommonServer.NPCSGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPCommonServer.NPCSGeneralListener.RequestDispather.p006_CrossRankOp.RequestDealer_NP2CS_R_006_001_ReqCrossInstance;
import NPCommonServer.NPCSGeneralListener.RequestDispather.p006_CrossRankOp.RequestDealer_NP2CS_R_006_002_ReqDiscardCrossInstance;

public class NPCSGeneral_006_RequestDispather_CrossRankOp extends NPRequestDispatcher
{
    public static void init(NPCSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2CS_R_006_001_ReqCrossInstance());
        _dispather.regHandler(new RequestDealer_NP2CS_R_006_002_ReqDiscardCrossInstance());
    }
}
