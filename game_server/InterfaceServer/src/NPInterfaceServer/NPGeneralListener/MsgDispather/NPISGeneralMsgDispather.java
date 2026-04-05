package NPInterfaceServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**************
 * 客户端协议处理对象
 * @author Administrator
 *
 */
public class NPISGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static final NPISGeneralMsgDispather _g_instance = new NPISGeneralMsgDispather();

    NPISGeneralMsgDispather()
    {

    }

    public static NPISGeneralMsgDispather getInstance()
    {

        return _g_instance;
    }
}
