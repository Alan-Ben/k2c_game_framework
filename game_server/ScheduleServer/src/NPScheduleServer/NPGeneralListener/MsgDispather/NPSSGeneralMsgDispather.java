package NPScheduleServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**************
 * 客户端协议处理对象
 * @author Administrator
 *
 */
public class NPSSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static final NPSSGeneralMsgDispather _g_instance = new NPSSGeneralMsgDispather();

    NPSSGeneralMsgDispather()
    {

    }

    public static NPSSGeneralMsgDispather getInstance()
    {
        return _g_instance;
    }
}
