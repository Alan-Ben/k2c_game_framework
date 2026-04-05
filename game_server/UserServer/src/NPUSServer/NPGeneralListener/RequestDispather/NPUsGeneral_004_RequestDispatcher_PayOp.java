package NPUSServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.NPGeneralListener.RequestDispather.p004_PayOp.RequestDealer_NP2US_R_004_001_ReqRechargeNotify;

/**
 * 充值相关协议分发器
 */
public class NPUsGeneral_004_RequestDispatcher_PayOp extends NPRequestDispatcher
{
    public static void init(NPUSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2US_R_004_001_ReqRechargeNotify(_dispather.getUSServer()));
    }
}