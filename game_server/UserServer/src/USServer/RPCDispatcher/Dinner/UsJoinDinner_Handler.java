package USServer.RPCDispatcher.Dinner;

import AllRpcData.US_Service.Dinner.UsJoinDinner;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.DinnerSystem.DinnerSystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑：加入宴会
 */
public class UsJoinDinner_Handler extends _ATBasicUSRpc_Handler<UsJoinDinner>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsJoinDinner _rpc) 
	{
    	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DINNER_JOIN);
    	
    	DinnerSystem.DealJoinDinner(_usServer, _rpc.req().getInstanceId(), _rpc.req().getPlayer(), context, _errCode -> 
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
