package NPCrossGameServer.NPCrossGameCore.GCMsgDispather;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCrossGameServer.NPCrossGameCore.RequestDispather.NPCrossGameGCCommiter;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter;

/***************
 * 通用的Sub Order dealer
 * @author mj
 *
 * @param <T>
 */
public abstract class CrossGameGCMsgDealer<T extends _IALProtocolStructure>
        extends _ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter<NPCrossGameGCCommiter, T>
{
}
