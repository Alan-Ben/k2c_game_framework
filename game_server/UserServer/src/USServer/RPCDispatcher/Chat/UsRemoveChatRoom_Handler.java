package USServer.RPCDispatcher.Chat;

import AllRpcData.US_Service.Chat.UsRemoveChatRoom;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 移除聊天房间
 */
public class UsRemoveChatRoom_Handler extends _ATBasicUSRpc_Handler<UsRemoveChatRoom> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsRemoveChatRoom _rpc)
	{
		_rpc.commit();
	}
}
