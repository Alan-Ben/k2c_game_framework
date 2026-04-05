package USServer.RPCDispatcher.Child;

import AllRpcData.US_Service.Child.GetServerPoolAdult;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 获取联姻池指定子嗣数据
 */
public class GetServerPoolAdult_Handler extends _ATBasicUSRpc_Handler<GetServerPoolAdult>  implements _IAutoRegistHandler
{
    @Override
    public void _deal(NPUserServer _usServer, GetServerPoolAdult _rpc)
	{
    	AdultMarrySystem.DealGetPoolAdult(_usServer, _rpc.req().getCid(), _rpc.req().getAdultId(), (_errCode, _poolAdult) -> 
    	{
    		if(_errCode > 0)
    		{
    			_rpc.commitFail(_errCode);
    		}
    		else
    		{
    			_rpc.retObj().setPoolAdult(_poolAdult);
    			_rpc.commit();
    		}
    	});
	}
}
