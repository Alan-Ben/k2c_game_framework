package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsMineInfoReq;
import NPCommon.ErrMain.MarsErr;
import NPUSServer.NPUserServer;
import NPUSServer.UsMars.MineCore.UsMarsMineObj;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 火星矿结算数据
 */
public class MarsMineInfoReq_Handler extends _ATBasicUSRpc_Handler<MarsMineInfoReq> implements _IAutoRegistHandler
{
	@Override
	protected void _deal(NPUserServer _usServer, MarsMineInfoReq _rpc)
	{
		UsMarsMineObj mineObj = _usServer.getMarsMineCore().lookupData(_rpc.req().getMineInstanceId());
		if(null == mineObj)
		{
			_rpc.commitFail(MarsErr.MARS_MINE_NOT_FOUND.getCode());
			return ;
		}

		//返回具体数据
		_rpc.retObj().setInfo(mineObj.toServerProto());

		//提交结果
		_rpc.commit();
	}
}
