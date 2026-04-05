package NPInterfaceServer.NPGeneralListener.RequestDispather;

import CommonProto.Common_R_255_001_ReqRpc;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.Log.CommLog;
import NPInterfaceServer.RPCDispatcher.ISRpcDispatcher;
import RPC.RPCDataFactoryMgr;
import RPC._ARPCData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NPISGeneral_255_RequestDispatcher extends NPRequestDispatcher
{
    public static void init(NPISGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPRequestDealer<Common_R_255_001_ReqRpc>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, Common_R_255_001_ReqRpc _msg)
            {
                _ARPCData rpcData = RPCDataFactoryMgr.getInstance().create(_msg.getClassId());
                if (null == rpcData)
                {
                    CommLog.error("can not read rpc Data for id:", _msg.getClassId());
                    return;
                }
                rpcData.readRequest(_msg.get_buffer_ReqBytes());
                ISRpcDispatcher.getInstance().dispatchRpc(_committer, rpcData);
            }
        });
    }
}
