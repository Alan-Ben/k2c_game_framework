package USServer.RPCDispatcher.Child;

import AllRpcData.US_Service.Child.AgreePersonMarryApply;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class AgreePersonMarryApply_Handler extends _ATBasicUSRpc_Handler<AgreePersonMarryApply> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, AgreePersonMarryApply _rpc)
	{
    	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_AGREE_PLAYER_APPLY);
    	
    	AdultMarrySystem.DealAgreePlayerApply(_usServer, _rpc.req().getMarryInfo(), context, _errCode->
    	{
    		if(_errCode > 0)
    		{
    			_rpc.commitFail(_errCode);
    		}
    		else
    		{
    			_rpc.commit();
    		}
    	});
	}
}
