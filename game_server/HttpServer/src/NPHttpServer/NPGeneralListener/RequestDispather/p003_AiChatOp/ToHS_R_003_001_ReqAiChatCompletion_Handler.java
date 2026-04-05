package NPHttpServer.NPGeneralListener.RequestDispather.p003_AiChatOp;

import NP2HS_R.p003_AiChatOp.ToHS_R_003_001_ReqAiChatCompletion;
import NP2HS_RB.p003_AiChatOp.ToHS_RB_003_001_RetAiChatCompletion;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPHttpServer.HttpAiService.HttpAiService;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * @description: 请求注册新房间房间
 * @author: ricci
 * @date: 2022-04-07 16:13:13
 */
public class ToHS_R_003_001_ReqAiChatCompletion_Handler extends NPRequestDealer<ToHS_R_003_001_ReqAiChatCompletion>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, ToHS_R_003_001_ReqAiChatCompletion _msg)
    {
        HttpAiService.getInstance().dealRequest(_msg.getUsId(), _msg.getUsRequestSerial(), _msg);

        _receiver.commitSucRes(new ToHS_RB_003_001_RetAiChatCompletion());
    }
}
