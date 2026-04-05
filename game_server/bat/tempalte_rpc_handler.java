package <%PackageName%>;

import <%RpcFullClassName%>;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：<%RpcCommnet%>
 */
public class <%RpcName%>_Handler extends RpcRequestHandler<<%RpcName%>>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, <%RpcName%> _rpc)
	{
		_rpc.commit();
	}
}
