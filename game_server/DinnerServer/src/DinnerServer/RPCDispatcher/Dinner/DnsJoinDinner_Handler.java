package DinnerServer.RPCDispatcher.Dinner;

import AllRpcData.Dinner_Service.Dinner.DnsJoinDinner;
import DinnerServer.DinnerPool.DinnerCrossGroup;
import DinnerServer.DinnerPool.DinnerCrossGroupDinner;
import DinnerServer.DinnerPool.DinnerCrossGroupMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：玩家参加宴会
 */
public class DnsJoinDinner_Handler extends RpcRequestHandler<DnsJoinDinner>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, DnsJoinDinner _rpc)
	{
		DinnerCrossGroup group = DinnerCrossGroupMgr.getInstance().lookup(_rpc.req().getGroupId());
    	if(null == group)
    	{
    		_rpc.commit();
    		return;
    	}
    	
    	DinnerCrossGroupDinner dinner = group.lookup(_rpc.req().getInstanceId());
    	if(null == dinner)
    	{
    		_rpc.commit();
    		return;
    	}
    	
    	//如果是玩家类型，则需要记录
    	if(_rpc.req().getIsPlayer())
    	{
    		dinner.addJoiner(_rpc.req().getJoinerId());
    	}
    	
    	//增加宴会人气数值
    	dinner.addScore(_rpc.req().getJoinerScore());
    	
    	_rpc.commit();
	}
}
