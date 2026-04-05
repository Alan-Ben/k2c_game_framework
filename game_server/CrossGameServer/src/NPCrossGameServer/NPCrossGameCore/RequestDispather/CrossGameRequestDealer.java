package NPCrossGameServer.NPCrossGameCore.RequestDispather;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCrossGameServer.NPCrossGameCore.MsgMgr.Commiter.CrossGameInstanceCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter;

/***************
 * 通用的Sub Order dealer
 * @author mj
 *
 * @param <T>
 */
public abstract class CrossGameRequestDealer<T extends _IALProtocolStructure>
        extends _ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter<CrossGameInstanceCommiter, T>
{
}
