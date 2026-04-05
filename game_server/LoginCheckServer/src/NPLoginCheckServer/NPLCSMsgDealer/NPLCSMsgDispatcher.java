package NPLoginCheckServer.NPLCSMsgDealer;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**************
 * 客户端协议处理对象
 * @author Administrator
 *
 */
public class NPLCSMsgDispatcher extends NPCustomMsgDispatcher
{
    private static NPLCSMsgDispatcher _g_instance = new NPLCSMsgDispatcher();

    public static NPLCSMsgDispatcher getInstance()
    {
        return _g_instance;
    }

    NPLCSMsgDispatcher()
    {

    }
}
