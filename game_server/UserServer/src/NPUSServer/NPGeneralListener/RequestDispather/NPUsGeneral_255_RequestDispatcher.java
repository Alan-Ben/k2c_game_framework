package NPUSServer.NPGeneralListener.RequestDispather;

import CommonProto.Common_R_255_001_ReqRpc;
import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.USLog;
import RPC.RPCDataFactoryMgr;
import RPC._ARPCData;
import USServer.RPCDispatcher.UsRpcDispatcher;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NPUsGeneral_255_RequestDispatcher extends NPRequestDispatcher
{
    public static void init(NPUSGeneralRequestDispather _dispatcher)
    {
        _dispatcher.regHandler(new NPRequestDealer<Common_R_255_001_ReqRpc>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, Common_R_255_001_ReqRpc _msg)
            {
                _ARPCData rpcData = RPCDataFactoryMgr.getInstance().create(_msg.getClassId());
                if (null == rpcData)
                {
                    USLog.error(_dispatcher.getUSServer(), "can not read rpc Data for Id:" + _msg.getClassId());
                    return;
                }
                rpcData.readRequest(_msg.get_buffer_ReqBytes());
                UsRpcDispatcher.getInstance().dispatchRpc(_committer, rpcData);
            }
        });
    }
}
