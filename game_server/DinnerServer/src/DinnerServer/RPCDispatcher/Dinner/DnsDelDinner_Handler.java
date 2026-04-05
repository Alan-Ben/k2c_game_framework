package DinnerServer.RPCDispatcher.Dinner;

import AllRpcData.Dinner_Service.Dinner.DnsDelDinner;
import DinnerServer.DinnerPool.DinnerCrossGroup;
import DinnerServer.DinnerPool.DinnerCrossGroupMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：向跨服宴会池移除宴会数据
 */
public class DnsDelDinner_Handler extends RpcRequestHandler<DnsDelDinner>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, DnsDelDinner _rpc)
	{
    	DinnerCrossGroup group = DinnerCrossGroupMgr.getInstance().lookup(_rpc.req().getGroupId());
    	if(null != group)
    	{
    		group.removeDinner(_rpc.req().getInstanceId());
    	}
    	
		_rpc.commit();
	}
}
