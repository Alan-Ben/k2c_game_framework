package USServer.RPCDispatcher.Grave;

import AllRpcData.US_Service.Grave.GetGravePlayerTitleRecordList;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

import java.util.ArrayList;
/*******
 * 处理逻辑： 获取指定玩家的杰出者的称号列表
 */
public class GetGravePlayerTitleRecordList_Handler extends _ATBasicUSRpc_Handler<GetGravePlayerTitleRecordList>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GetGravePlayerTitleRecordList _rpc)
	{
    	ArrayList<Long> titleIdList = _usServer.getGraveMgr().getGravePlayerTitleRecordMgr().getRecordList(_rpc.req().getCid());
    	_rpc.retObj().getTitleIdList().addAll(titleIdList);
    	
		_rpc.commit();
	}
}
