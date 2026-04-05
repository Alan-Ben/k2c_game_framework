package NPRecordServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**************
 * 客户端协议处理对象
 * @author Administrator
 *
 */
public class NPRCSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static final NPRCSGeneralMsgDispather _g_instance = new NPRCSGeneralMsgDispather();

    NPRCSGeneralMsgDispather()
    {

    }

    public static NPRCSGeneralMsgDispather getInstance()
    {

        return _g_instance;
    }
}
