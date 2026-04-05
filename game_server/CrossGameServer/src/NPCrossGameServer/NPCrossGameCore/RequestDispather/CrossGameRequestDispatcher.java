package NPCrossGameServer.NPCrossGameCore.RequestDispather;

import NPCrossGameServer.NPCrossGameCore.MsgMgr.Commiter.CrossGameInstanceCommiter;
import NPCrossGameServer.NPCrossGameCore.RequestDispather.p001_BasicOp.NPCrossGameDealer_001_001_GCForwardMsg;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._TWCGBasicRequestDispather_CustomCommiter;

/***************
 * 空间的请求统一处理对象
 * @author mj
 *
 */
public class CrossGameRequestDispatcher extends _TWCGBasicRequestDispather_CustomCommiter<CrossGameInstanceCommiter>
{
    private static CrossGameRequestDispatcher _g_instance = new CrossGameRequestDispatcher();

    public static CrossGameRequestDispatcher getInstance()
    {
        if (null == _g_instance)
            _g_instance = new CrossGameRequestDispatcher();
        return _g_instance;
    }

    protected CrossGameRequestDispatcher()
    {
        regSubDealer(new NPCrossGameDealer_001_001_GCForwardMsg());
    }
}
