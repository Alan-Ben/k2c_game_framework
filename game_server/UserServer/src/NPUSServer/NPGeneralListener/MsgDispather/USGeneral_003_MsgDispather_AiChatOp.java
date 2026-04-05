package NPUSServer.NPGeneralListener.MsgDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2US.p003_AiChatOp.ToUS_003_001_AiMsgAdd;
import NPCommon.Dispather.NPCustomMsgDispatcher.NPCustomMsgDealer;
import NPUSServer.NPUserServer;

public class USGeneral_003_MsgDispather_AiChatOp
{
    public static void init(NPUSGeneralMsgDispather _dispather)
    {
        final NPUserServer usServer = _dispather.getUSServer();

        _dispather.regHandler(new NPCustomMsgDealer<ToUS_003_001_AiMsgAdd>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, ToUS_003_001_AiMsgAdd _msg)
            {
                usServer.getAiServiceFunc().handleAIResponse(_msg.getUsRequestId(), _msg.getErrCode(), _msg.getResponse());
            }
        });
    }
}
