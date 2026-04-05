package NPHttpServer.HsClientListener;

import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPHttpServer.HsClientListener.p002_HsClientOp.NPHSGeneral_002_RequestDispatcher_HsClientOp;

/**************
 * 客户端协议处理对象
 *
 * @author Administrator
 *
 */
public class HsClientMsgDispather extends NPCustomMsgDispatcher
{
    private static HsClientMsgDispather _g_instance = new HsClientMsgDispather();

    public static HsClientMsgDispather getInstance()
    {
        return _g_instance;
    }

    protected HsClientMsgDispather()
    {
        NPHSGeneral_002_RequestDispatcher_HsClientOp.register(this);

    }
}
