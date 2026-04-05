package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsMineLeave;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 火星矿结算数据
 */
public class MarsMineLeave_Handler extends _ATBasicUSRpc_Handler<MarsMineLeave> implements _IAutoRegistHandler
{
	@Override
	protected void _deal(NPUserServer _usServer, MarsMineLeave _rpc)
	{
		Result result = _usServer.getMarsMineCore().leaveMine(
				_rpc.req().getMineInstanceId()
				, _rpc.req().getCid()
				, _rpc.req().getTeamId());

		if(result != Result.SUCC)
		{
			_rpc.commitFail(result.getCode());
			return;
		}

		_rpc.commit();
	}
}
