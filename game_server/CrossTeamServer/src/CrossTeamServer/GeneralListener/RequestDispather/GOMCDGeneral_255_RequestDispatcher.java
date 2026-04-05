package CrossTeamServer.GeneralListener.RequestDispather;

import CommonProto.Common_R_255_001_ReqRpc;
import CrossTeamServer.RPCDispatcher.CTSRpcDispatcher;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.Log.CommLog;
import RPC.RPCDataFactoryMgr;
import RPC._ARPCData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class GOMCDGeneral_255_RequestDispatcher extends NPRequestDispatcher
{
    public static void init(GOMCDGeneralRequestDispather _dispatcher)
    {
        _dispatcher.regHandler(new NPRequestDealer<Common_R_255_001_ReqRpc>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, Common_R_255_001_ReqRpc _msg)
            {
                _ARPCData rpcData = RPCDataFactoryMgr.getInstance().create(_msg.getClassId());
                if (null == rpcData)
                {
                    CommLog.error("can not read rpc Data for Id:" + _msg.getClassId());
                    return;
                }
                rpcData.readRequest(_msg.get_buffer_ReqBytes());
                CTSRpcDispatcher.getInstance().dispatchRpc(_committer, rpcData);
            }
        });
    }
}
