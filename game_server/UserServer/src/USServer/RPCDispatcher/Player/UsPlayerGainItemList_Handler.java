package USServer.RPCDispatcher.Player;

import AllRpcData.US_Service.Player.UsPlayerGainItemList;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class UsPlayerGainItemList_Handler extends _ATBasicUSRpc_Handler<UsPlayerGainItemList> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, UsPlayerGainItemList _rpc)
	{
		_rpc.commit();
	}
}
