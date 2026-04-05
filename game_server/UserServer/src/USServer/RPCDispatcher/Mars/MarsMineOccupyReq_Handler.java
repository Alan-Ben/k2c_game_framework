package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsMineOccupyReq;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 火星矿结算数据
 */
public class MarsMineOccupyReq_Handler extends _ATBasicUSRpc_Handler<MarsMineOccupyReq> implements _IAutoRegistHandler
{
	@Override
	protected void _deal(NPUserServer _usServer, MarsMineOccupyReq _rpc)
	{
		Result result = _usServer.getMarsActionCore().getMineActionMgr().addAttackAction(
				_rpc.req().getMineInstanceId()
				, _rpc.req().getIsOtherTeamForward()
				, _rpc.req().getPlayerInfo()
				, _rpc.req().getStartCollectMs()
				, _rpc.req().getCollectSpeed());
		if(result != Result.SUCC)
		{
			_rpc.commitFail(result.getCode());
			return;
		}

		_rpc.commit();
	}
}
