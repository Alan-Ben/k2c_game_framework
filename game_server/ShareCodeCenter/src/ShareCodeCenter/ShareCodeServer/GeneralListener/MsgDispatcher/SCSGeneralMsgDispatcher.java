package ShareCodeCenter.ShareCodeServer.GeneralListener.MsgDispatcher;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**************
 * 客户端协议处理对象
 * @author Administrator
 *
 */
public class SCSGeneralMsgDispatcher extends NPCustomMsgDispatcher
{
    private static final SCSGeneralMsgDispatcher _g_instance = new SCSGeneralMsgDispatcher();

    public static SCSGeneralMsgDispatcher getInstance()
    {
        return _g_instance;
    }
}
