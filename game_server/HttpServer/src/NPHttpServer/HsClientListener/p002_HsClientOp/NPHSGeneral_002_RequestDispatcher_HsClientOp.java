package NPHttpServer.HsClientListener.p002_HsClientOp;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**
 * @description:
 * @author: ricci
 * @date: 2022-04-07 16:10:12
 */
public class NPHSGeneral_002_RequestDispatcher_HsClientOp extends NPCustomMsgDispatcher
{

    public static void register(NPCustomMsgDispatcher _disPatcher)
    {
        _disPatcher.regHandler(new NP2HS_R_002_001_ReqExecGmSuper_Handler());
    }
}
