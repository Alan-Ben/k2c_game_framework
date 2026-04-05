package USServer.RPCDispatcher.Dinner;

import AllRpcData.US_Service.Dinner.UsGetDinnerInfo;
import NPCommon.ErrMain.DinnerErr;
import NPGameRes.GameObjs.Dinner.DinnerGetInfoResult;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑：获取宴会详细数据
 */
public class UsGetDinnerInfo_Handler extends _ATBasicUSRpc_Handler<UsGetDinnerInfo>  implements _IAutoRegistHandler
{
	@Override
	protected void _deal(NPUserServer _usServer, UsGetDinnerInfo _rpc) 
	{
    	DinnerGetInfoResult result = new DinnerGetInfoResult();
		//构造玩家查看宴会详情数据
    	_usServer.getDinnerPool().makeResult(result, _rpc.req().getInstanceId(), _rpc.req().getCid());
		if(null == result.getDinner())
		{
			_rpc.commitFail(DinnerErr.DINNER_NOT_FOUND.getCode());
		}
		else
		{
			_rpc.retObj().setInfo(result.getDinner());
			
			_rpc.commit();
		}
	}
}
