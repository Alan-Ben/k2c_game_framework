package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsMineAddPVPLog;
import Common.MarsEnum.EMarsExplorePVPLogType;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/**
 * 处理逻辑：火星矿PVP战报跨服添加
 * 接收来自其他UserServer的战报数据，调用本地DealLog完成处理
 */
public class MarsMineAddPVPLog_Handler extends _ATBasicUSRpc_Handler<MarsMineAddPVPLog> implements _IAutoRegistHandler
{
	@Override
	protected void _deal(NPUserServer _usServer, MarsMineAddPVPLog _rpc)
	{
		long cid = _rpc.req().getCid();
		EMarsExplorePVPLogType logType = _rpc.req().getLogObj().getLogType();
		byte[] logData = _rpc.req().getLogObj().getLogData();

		MarsMineSystem.DealLog(_usServer, cid, logType, logData);

		_rpc.commit();
	}
}
