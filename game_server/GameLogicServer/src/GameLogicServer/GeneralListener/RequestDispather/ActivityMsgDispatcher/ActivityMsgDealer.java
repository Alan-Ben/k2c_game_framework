package GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Dispather._IAutoRegistMsgHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.CustomCommiter._ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter;

public abstract class ActivityMsgDealer<T extends _IALProtocolStructure>
        extends _ATWCGBasicRequestAutoSubOrderDealer_CustomCommiter<ActivityMsgCommiter, T>
        implements _IAutoRegistMsgHandler
{
}
