package DinnerServer.RPCDispatcher.Dinner;

import AllRpcData.Dinner_Service.Dinner.DnsDiscardGroup;
import DinnerServer.DinnerPool.DinnerCrossGroupMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*******
 * 处理逻辑：解散跨服宴会池指定分组
 */
public class DnsDiscardGroup_Handler extends RpcRequestHandler<DnsDiscardGroup>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, DnsDiscardGroup _rpc)
	{
    	DinnerCrossGroupMgr.getInstance().discardByUs(_rpc.req().getGroupId(), _rpc.req().getUsId());
    	
		_rpc.commit();
	}
}
