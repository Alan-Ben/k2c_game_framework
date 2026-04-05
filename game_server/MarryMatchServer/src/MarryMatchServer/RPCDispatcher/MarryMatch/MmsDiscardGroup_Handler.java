package MarryMatchServer.RPCDispatcher.MarryMatch;

import AllRpcData.MarryMatch_Service.MarryMatch.MmsDiscardGroup;
import MarryMatchServer.MarryMatchMgr.MarryMatchGroup;
import MarryMatchServer.MarryMatchMgr.MarryMatchMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：销毁联姻池
 */
public class MmsDiscardGroup_Handler extends RpcRequestHandler<MmsDiscardGroup>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, MmsDiscardGroup _rpc)
	{
    	MarryMatchGroup group = MarryMatchMgr.getInstance().lookup(_rpc.req().getGroupId());
    	if(null != group)
    	{
    		group.removeItemByUs(_rpc.req().getUsId());
    		
    		MarryMatchMgr.getInstance().checkDiscard(_rpc.req().getGroupId());
    	}
    	
		_rpc.commit();
	}
}
