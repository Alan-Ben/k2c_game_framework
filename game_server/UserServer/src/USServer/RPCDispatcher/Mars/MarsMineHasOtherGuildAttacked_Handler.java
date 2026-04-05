package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsMineHasOtherGuildAttacked;
import NPCommon.ErrMain.MarsErr;
import NPUSServer.NPUserServer;
import NPUSServer.UsMars.MineCore.UsMarsMineObj;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 火星矿结算数据
 */
public class MarsMineHasOtherGuildAttacked_Handler extends _ATBasicUSRpc_Handler<MarsMineHasOtherGuildAttacked> implements _IAutoRegistHandler
{
	@Override
	protected void _deal(NPUserServer _usServer, MarsMineHasOtherGuildAttacked _rpc)
	{
		UsMarsMineObj usMine = _usServer.getMarsMineCore().lookupData(_rpc.req().getMineInstanceId());
		if(null == usMine)
		{
			_rpc.commitFail(MarsErr.MARS_MINE_NOT_FOUND.getCode());
			return ;
		}

		//返回具体数据
		_rpc.retObj().setHasOtherGuildAttacked(usMine.hasOtherGuildAttacked(_rpc.req().getGuildId()));

		_rpc.commit();
	}
}
