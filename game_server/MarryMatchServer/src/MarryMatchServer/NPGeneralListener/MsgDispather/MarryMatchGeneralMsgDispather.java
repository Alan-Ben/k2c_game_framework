package MarryMatchServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**************
 * 客户端协议处理对象
 * @author Administrator
 *
 */
public class MarryMatchGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static final MarryMatchGeneralMsgDispather _g_instance = new MarryMatchGeneralMsgDispather();

    MarryMatchGeneralMsgDispather()
    {

    }

    public static MarryMatchGeneralMsgDispather getInstance()
    {

        return _g_instance;
    }
}
