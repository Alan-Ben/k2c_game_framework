package DinnerServer.RPCDispatcher.Dinner;

import AllRpcData.Dinner_Service.Dinner.DnsGetDinnerInfoBySort;
import DinnerServer.DinnerPool.DinnerCrossGroup;
import DinnerServer.DinnerPool.DinnerCrossGroupMgr;
import NPCommon.ErrMain.DinnerErr;
import NPGameRes.GameObjs.Dinner.DinnerGetIdxResult;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：获取排序规则的当前宴会数据
 */
public class DnsGetDinnerInfoBySort_Handler extends RpcRequestHandler<DnsGetDinnerInfoBySort>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, DnsGetDinnerInfoBySort _rpc)
	{
		DinnerCrossGroup group = DinnerCrossGroupMgr.getInstance().lookup(_rpc.req().getGroupId());
    	if(null != group)
    	{
    		DinnerGetIdxResult result = new DinnerGetIdxResult();
    		
    		group.makeResult(result, _rpc.req().getInstanceId(), _rpc.req().getCid());
    		if(null == result.getDinnerIdx())
    		{
    			_rpc.commitFail(DinnerErr.DINNER_NOT_FOUND.getCode());
    		}
    		else
    		{
    			_rpc.retObj().setInstanceId(result.getDinnerIdx().getInstanceId());
    			_rpc.retObj().setIdx(result.getIdx());
    			_rpc.retObj().setHasPre(result.hasPre());
    			_rpc.retObj().setHasNext(result.hasNext());
    			
    			_rpc.commit();
    		}
    	}
    	else
    	{
    		_rpc.commit();
    	}
	}
}
