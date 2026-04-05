package MarryMatchServer.RPCDispatcher.MarryMatch;

import AllRpcData.MarryMatch_Service.MarryMatch.MmsAddMatchItem;
import MarryMatchServer.MarryMatchMgr.MarryMatchMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：增加联姻池请求数据
 */
public class MmsAddMatchItem_Handler extends RpcRequestHandler<MmsAddMatchItem>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, MmsAddMatchItem _rpc)
	{
    	MarryMatchMgr.getInstance().ensure(_rpc.req().getGroupId()).addItem(_rpc.req().getItem());
    	
		_rpc.commit();
	}
}
