package USServer.RPCDispatcher.Grave;

import AllRpcData.US_Service.Grave.AddGraveNewInfo;
import Common.GraveObj.GraveObj_NewInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 增加新晋杰出者
 */
public class AddGraveNewInfo_Handler extends _ATBasicUSRpc_Handler<AddGraveNewInfo>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, AddGraveNewInfo _rpc)
	{
		for(int i = 0; i < _rpc.req().getNewInfoList().size(); i++)
		{
			GraveObj_NewInfo info = _rpc.req().getNewInfoList().get(i);
			if(null == info)
				continue;
			
			//本服数据处理
			_usServer.getGraveMgr().addGraveNewInfo(info);
		}
    	
		_rpc.commit();
	}
}
