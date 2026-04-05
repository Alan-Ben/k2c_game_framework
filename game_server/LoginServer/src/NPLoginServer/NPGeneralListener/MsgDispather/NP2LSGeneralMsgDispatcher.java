package NPLoginServer.NPGeneralListener.MsgDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2LS.p001_BasicOp.NP2LS_001_002_SetForbidenLogin;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPLoginServer.NPLoginServer;

/**************
 * 客户端协议处理对象
 *
 * @author Administrator
 *
 */
public class NP2LSGeneralMsgDispatcher extends NPCustomMsgDispatcher
{
    private static NP2LSGeneralMsgDispatcher _g_instance = new NP2LSGeneralMsgDispatcher();

    public static NP2LSGeneralMsgDispatcher getInstance()
    {

        return _g_instance;
    }

    NP2LSGeneralMsgDispatcher()
    {
        this.regHandler(new NPCustomMsgDealer<NP2LS_001_002_SetForbidenLogin>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2LS_001_002_SetForbidenLogin _msg)
            {
                NPLoginServer.getInstance().setIsForbidenLogin(_msg.getIsForbidenLogin());
            }
        });

    }
}
