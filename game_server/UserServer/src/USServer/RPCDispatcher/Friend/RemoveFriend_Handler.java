package USServer.RPCDispatcher.Friend;

import AllRpcData.US_Service.Friend.RemoveFriend;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.FriendSystem.FriendSystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class RemoveFriend_Handler extends _ATBasicUSRpc_Handler<RemoveFriend> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, RemoveFriend _rpc)
	{
    	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.REMOVE_FRIEND);
    	
    	FriendSystem.acceptRemoveFriend(_usServer, _rpc.req().getSendCid(), _rpc.req().getTargetCid(), context);
    	
		_rpc.commit();
	}
}
