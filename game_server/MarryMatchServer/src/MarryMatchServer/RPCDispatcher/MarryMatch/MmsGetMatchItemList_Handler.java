package MarryMatchServer.RPCDispatcher.MarryMatch;

import AllRpcData.MarryMatch_Service.MarryMatch.MmsGetMatchItemList;
import Common.ChildObj.Adult_PoolBaseInfo;
import MarryMatchServer.MarryMatchMgr.MarryMatchGroup;
import MarryMatchServer.MarryMatchMgr.MarryMatchItem;
import MarryMatchServer.MarryMatchMgr.MarryMatchMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;
/*******
 * 处理逻辑：获取符合匹配要求的子嗣基础数据列表
 */
public class MmsGetMatchItemList_Handler extends RpcRequestHandler<MmsGetMatchItemList>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, MmsGetMatchItemList _rpc)
	{
    	do 
    	{
    		MarryMatchGroup group = MarryMatchMgr.getInstance().lookup(_rpc.req().getGroupId());
    		if(null == group)
    			break;
    		
    		ArrayList<MarryMatchItem> itemList = group.getMatchItemList(_rpc.req().getAdultId(), _rpc.req().getCid(), _rpc.req().getMatchId(), _rpc.req().getBonus(), _rpc.req().getCount());
    		if(null == itemList)
    			break;
    		
    		for(int i = 0; i < itemList.size(); i++)
			{
				MarryMatchItem item = itemList.get(i);
				if(null == item)
					continue;
				
				Adult_PoolBaseInfo info = new Adult_PoolBaseInfo();
				info.setApplyCid(item.getApplyCid());
				info.setApplyAdultId(item.getApplyAdultId());
				info.setBonus(item.getBonus());
                info.setMinBonus(item.getMinBonus());

				_rpc.retObj().addMatchList(info);
			}
        	
    	} while(false);
    	
		_rpc.commit();
	}
}
