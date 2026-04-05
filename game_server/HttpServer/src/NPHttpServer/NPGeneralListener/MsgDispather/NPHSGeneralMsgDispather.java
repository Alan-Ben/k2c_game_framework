package NPHttpServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**************
 * 客户端协议处理对象
 * @author Administrator
 *
 */
public class NPHSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static final NPHSGeneralMsgDispather _g_instance = new NPHSGeneralMsgDispather();

    NPHSGeneralMsgDispather()
    {

    }

    public static NPHSGeneralMsgDispather getInstance()
    {

        return _g_instance;
    }
}
