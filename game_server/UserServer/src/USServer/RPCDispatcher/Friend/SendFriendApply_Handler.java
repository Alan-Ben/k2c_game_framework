package USServer.RPCDispatcher.Friend;

import AllRpcData.US_Service.Friend.SendFriendApply;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.FriendSystem.FriendSystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class SendFriendApply_Handler extends _ATBasicUSRpc_Handler<SendFriendApply> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, SendFriendApply _rpc)
	{
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SEND_FRIEND_APPLY);
    	
    	FriendSystem.acceptApply(_usServer, _rpc.req().getApplyCid(), _rpc.req().getTargetCid(), context, (isSuc, errCode)->
    	{
    		_rpc.retObj().setIsSuc(isSuc);
    		_rpc.retObj().setErrCode(errCode);

    		_rpc.commit();
    	});
	
	}
}
