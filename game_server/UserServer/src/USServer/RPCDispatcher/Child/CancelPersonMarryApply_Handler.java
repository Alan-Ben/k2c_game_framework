package USServer.RPCDispatcher.Child;

import AllRpcData.US_Service.Child.CancelPersonMarryApply;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class CancelPersonMarryApply_Handler extends _ATBasicUSRpc_Handler<CancelPersonMarryApply> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, CancelPersonMarryApply _rpc)
	{
    	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_CANCEL_PLAYER_APPLY);
    	
    	AdultMarrySystem.DealCancelPlayerApply(_usServer, _rpc.req().getCancel(), context);
    	
		_rpc.commit();
	}
}
