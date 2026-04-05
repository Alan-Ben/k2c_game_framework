package USServer.RPCDispatcher.Dinner;

import AllRpcData.US_Service.Dinner.UsGetDinnerIdx;
import NPCommon.ErrMain.DinnerErr;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑：获取宴会索引数据
 */
public class UsGetDinnerIdx_Handler extends _ATBasicUSRpc_Handler<UsGetDinnerIdx>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsGetDinnerIdx _rpc)
	{
    	DinnerInfo dinnerInfo = _usServer.getDinnerPool().lookup(_rpc.req().getInstanceId());
    	if(null == dinnerInfo)
    	{
			_rpc.commitFail(DinnerErr.DINNER_NOT_FOUND.getCode());
		}
    	else
    	{
    		_rpc.retObj().setIdx(dinnerInfo.toIdxProto(_rpc.req().getCid()));
			_rpc.commit();
    	}
	}
}
