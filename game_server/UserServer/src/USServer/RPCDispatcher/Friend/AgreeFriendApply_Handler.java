package USServer.RPCDispatcher.Friend;

import AllRpcData.US_Service.Friend.AgreeFriendApply;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.FriendSystem.FriendSystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class AgreeFriendApply_Handler extends _ATBasicUSRpc_Handler<AgreeFriendApply> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, AgreeFriendApply _rpc)
	{
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.AGREE_FRIEND_APPLY);
    	
    	FriendSystem.acceptAgreeApply(_usServer, _rpc.req().getApplyCid(), _rpc.req().getAgreeCid(), context, (isSuc, errCode)->
    	{
    		_rpc.retObj().setIsSuc(isSuc);
    		_rpc.retObj().setErrCode(errCode);

    		_rpc.commit();
    	});
	}
}
