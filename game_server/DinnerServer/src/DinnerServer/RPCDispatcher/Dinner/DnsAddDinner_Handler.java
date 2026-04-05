package DinnerServer.RPCDispatcher.Dinner;

import AllRpcData.Dinner_Service.Dinner.DnsAddDinner;
import DinnerServer.DinnerPool.DinnerCrossGroupMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：向跨服宴会池增加宴会数据
 */
public class DnsAddDinner_Handler extends RpcRequestHandler<DnsAddDinner>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, DnsAddDinner _rpc)
	{
    	DinnerCrossGroupMgr.getInstance().ensure(_rpc.req().getGroupId()).addDinner(_rpc.req().getDinnerIdx()
    			, _rpc.req().getStartTs(), _rpc.req().getSortId(), _rpc.req().getJoinerCidList());
    	
		_rpc.commit();
	}
}
