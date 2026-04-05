package RPC.DefaultHandler;

import AllRpcData.All_Service.PushPHPParamList;
import NPCommon.PHPParam.BSPHPParamMgr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class PushPHPParamList_Handler extends RpcRequestHandler<PushPHPParamList> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, PushPHPParamList _rpc)
    {
    	BSPHPParamMgr.getInstance().loadAllData(_rpc.req().getSerial(), _rpc.req().getPListObj());
    	
    	_rpc.retObj().setIsSucc(true);
    	
    	_rpc.commit();
    }
}
