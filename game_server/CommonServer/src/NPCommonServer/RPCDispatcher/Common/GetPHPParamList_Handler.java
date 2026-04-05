package NPCommonServer.RPCDispatcher.Common;

import AllRpcData.CS_Service.Common.GetPHPParamList;
import NPCommonServer.NPCSGeneralListener.NPCSGeneralBasicServerListener;
import NPCommonServer.PHPParmMgr.CSPHPParamMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*******
 * 获取平台参数
 */
public class GetPHPParamList_Handler extends RpcRequestHandler<GetPHPParamList>  implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, GetPHPParamList _rpc)
	{
    	//尚未加载完成
    	if(!CSPHPParamMgr.getInstance().isLoaded())
    	{
    		_rpc.retObj().setIsSucc(false);
    		_rpc.commit();
    		return;
    	}
    	
    	//注册到监听列表
    	NPCSGeneralBasicServerListener listener = (NPCSGeneralBasicServerListener) _committer.getRequestDealer();
    	CSPHPParamMgr.getInstance().regListenerServer(listener.getServerType(), listener.getServerTypeId());
    	
    	//平台参数数据
    	_rpc.retObj().setSerial(CSPHPParamMgr.getInstance().getDataSerial());
    	CSPHPParamMgr.getInstance().makeData(_rpc.retObj().getPListObj().getPList());
    	
    	_rpc.retObj().setIsSucc(true);
		_rpc.commit();
	}
}
