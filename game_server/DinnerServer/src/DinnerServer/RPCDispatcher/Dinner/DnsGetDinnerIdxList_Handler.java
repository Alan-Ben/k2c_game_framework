package DinnerServer.RPCDispatcher.Dinner;

import AllRpcData.Dinner_Service.Dinner.DnsGetDinnerIdxList;
import Common.DinnerObj.Dinner_Idx;
import DinnerServer.DinnerPool.DinnerCrossGroup;
import DinnerServer.DinnerPool.DinnerCrossGroupMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：获取跨服池宴会索引数据列表
 */
public class DnsGetDinnerIdxList_Handler extends RpcRequestHandler<DnsGetDinnerIdxList>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, DnsGetDinnerIdxList _rpc)
	{
    	DinnerCrossGroup group = DinnerCrossGroupMgr.getInstance().lookup(_rpc.req().getGroupId());
    	if(null != group)
    	{
    		group.makeIdxList(_rpc.req().getCid(), _rpc.req().getPage(), _rpc.req().getNum(), (_errCode, _dinnerResult) -> 
    		{
    			if(_errCode > 0) //失败情况
    			{
    				_rpc.commitFail(_errCode);
    			}
    			else //获取数据
    			{
    				//宴会索引数据
    				for(int i = 0; i < _dinnerResult.getIdxList().size(); i++)
        			{
        				Dinner_Idx idx = _dinnerResult.getIdxList().get(i);
        				if(null == idx)
        					continue;
        				
        				_rpc.retObj().addIdxList(idx);
        			}
        			//是否有下一条数据
        			_rpc.retObj().setHasNext(_dinnerResult.hasNext());
        			
        			_rpc.commit();
    			}
    		});
    	}
    	else
    	{
    		_rpc.commit();
    	}
	}
}
