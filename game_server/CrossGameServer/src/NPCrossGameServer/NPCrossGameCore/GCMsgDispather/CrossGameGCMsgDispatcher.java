package NPCrossGameServer.NPCrossGameCore.GCMsgDispather;

import NPCrossGameServer.NPCrossGameCore.RequestDispather.NPCrossGameGCCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._TWCGBasicRequestDispather_CustomCommiter;

/***************
 * 空间的请求统一处理对象
 * @author mj
 *
 */
public class CrossGameGCMsgDispatcher extends _TWCGBasicRequestDispather_CustomCommiter<NPCrossGameGCCommiter>
{
    private static CrossGameGCMsgDispatcher _g_instance = new CrossGameGCMsgDispatcher();

    public static CrossGameGCMsgDispatcher getInstance()
    {
        if (null == _g_instance)
            _g_instance = new CrossGameGCMsgDispatcher();
        return _g_instance;
    }

    protected CrossGameGCMsgDispatcher()
    {
    }
}
