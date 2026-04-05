package MarryMatchServer.RPCDispatcher.MarryMatch;

import AllRpcData.MarryMatch_Service.MarryMatch.MmsUploadMatchItemList;
import Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo;
import MarryMatchServer.MarryMatchMgr.MarryMatchGroup;
import MarryMatchServer.MarryMatchMgr.MarryMatchMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
/*******
 * 处理逻辑：增加联姻池请求数据
 */
public class MmsUploadMatchItemList_Handler extends RpcRequestHandler<MmsUploadMatchItemList>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, MmsUploadMatchItemList _rpc)
	{
    	MarryMatchGroup group = MarryMatchMgr.getInstance().ensure(_rpc.req().getGroupId());
    	
    	for(int i = 0; i < _rpc.req().getItemList().size(); i++)
    	{
    		ServerObj_AdultMarryGroupApplyInfo item = _rpc.req().getItemList().get(i);
    		if(null == item)
    			continue;
    		
    		group.addItem(item);
    	}
    	
		_rpc.commit();
	}
}
