package MarryMatchServer.RPCDispatcher.MarryMatch;

import AllRpcData.MarryMatch_Service.MarryMatch.MmsRemoveMatchItem;
import MarryMatchServer.MarryMatchMgr.MarryMatchGroup;
import MarryMatchServer.MarryMatchMgr.MarryMatchMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑： 移除联姻池请求数据
 */
public class MmsRemoveMatchItem_Handler extends RpcRequestHandler<MmsRemoveMatchItem>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, MmsRemoveMatchItem _rpc)
	{
    	MarryMatchGroup group = MarryMatchMgr.getInstance().lookup(_rpc.req().getGroupId());
    	if(null != group)
    	{
    		group.removeItem(_rpc.req().getApplyAdultId());
    	}
    	
		_rpc.commit();
	}
}
