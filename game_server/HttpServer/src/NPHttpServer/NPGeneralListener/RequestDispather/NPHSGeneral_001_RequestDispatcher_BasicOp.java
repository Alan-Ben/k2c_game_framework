package NPHttpServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPHttpServer.NPGeneralListener.RequestDispather.p001_BasicOp.*;
import NPHttpServer.NPGeneralListener.RequestDispather.p003_AiChatOp.ToHS_R_003_001_ReqAiChatCompletion_Handler;

/**
 * @description:
 * @author: ricci
 * @date: 2022-04-07 16:10:12
 */
public class NPHSGeneral_001_RequestDispatcher_BasicOp extends NPRequestDispatcher
{

    public static void regist(NPRequestDispatcher _disPatcher)
    {
        _disPatcher.regHandler(new NP2HS_R_001_001_ReqServerInfoList_Handler());
        _disPatcher.regHandler(new NP2HS_R_001_002_ReqAllServerMail_Handler());
        _disPatcher.regHandler(new NP2HS_R_001_004_ReqCheckIsInWhiteList_Handler());
        _disPatcher.regHandler(new NP2HS_R_001_005_ReqPlatformInfo_Handler());
        _disPatcher.regHandler(new NP2HS_R_001_006_ReqPlatParamList_Handler());
        _disPatcher.regHandler(new NP2HS_R_001_007_ReqWhiteAccList_Handler());

        _disPatcher.regHandler(new NP2HS_R_001_010_ReqPushActivityScheduleResult_Handler());
        _disPatcher.regHandler(new ToHS_R_003_001_ReqAiChatCompletion_Handler());
    }
}
