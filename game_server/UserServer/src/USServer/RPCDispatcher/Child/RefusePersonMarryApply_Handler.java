package USServer.RPCDispatcher.Child;

import AllRpcData.US_Service.Child.RefusePersonMarryApply;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class RefusePersonMarryApply_Handler extends _ATBasicUSRpc_Handler<RefusePersonMarryApply> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, RefusePersonMarryApply _rpc)
	{
    	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_REFUSE_PLAYER_APPLY);
    	
    	AdultMarrySystem.DealRefusePlayerApply(_usServer, _rpc.req().getRefusedApply(), context);
    	
		_rpc.commit();
	}
}
