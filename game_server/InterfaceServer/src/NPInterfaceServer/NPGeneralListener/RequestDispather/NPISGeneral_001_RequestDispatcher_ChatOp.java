package NPInterfaceServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPInterfaceServer.NPGeneralListener.RequestDispather.p001_ChatOp.*;


public class NPISGeneral_001_RequestDispatcher_ChatOp extends NPRequestDispatcher
{
    public static void init(NPRequestDispatcher _disPatcher)
    {
        _disPatcher.regHandler(new NP2IS_R_001_001_ReqRegRoom_Handler());
        _disPatcher.regHandler(new NP2IS_R_001_002_ReqUnRegRoom_Handler());
        _disPatcher.regHandler(new NP2IS_R_001_003_ReqRegChatUser_Handler());
        _disPatcher.regHandler(new NP2IS_R_001_004_ReqJoinRoom_Handler());
        _disPatcher.regHandler(new NP2IS_R_001_006_ReqSendRoomMsg_Handler());
        _disPatcher.regHandler(new NP2IS_R_001_007_ReqSendPrivateMsg_Handler());
    }
}
