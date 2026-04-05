package USServer.RPCDispatcher.Dinner;

import AllRpcData.US_Service.Dinner.UsAddBeJoinedCount;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.DinnerSystem.DinnerSystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑：玩家宴会交互记录
 */
public class UsAddBeJoinedCount_Handler extends _ATBasicUSRpc_Handler<UsAddBeJoinedCount>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsAddBeJoinedCount _rpc)
	{
    	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DINNER_JOIN);
    	
    	DinnerSystem.DealAddBeJoinedCount(_usServer, _rpc.req().getJoinerCid(), _rpc.req().getOwnerCid(), _errCode -> 
    	{
    		if(_errCode > 0)
    		{
    			_rpc.commitFail(_errCode);
    		}
    		else
    		{
    			_rpc.commit();
    		}
    	}, context);
	}
}
