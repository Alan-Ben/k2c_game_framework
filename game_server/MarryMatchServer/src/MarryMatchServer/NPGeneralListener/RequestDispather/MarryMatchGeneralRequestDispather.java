package MarryMatchServer.NPGeneralListener.RequestDispather;


import CommonProto.Common_R_255_001_ReqRpc;
import MarryMatchServer.RPCDispatcher.RSRpcDispatcher;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.Log.CommLog;
import RPC.RPCDataFactoryMgr;
import RPC._ARPCData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


/********************
 * 平台服务器发送来请求的处理对象
 *
 * @author Administrator
 *
 */
public class MarryMatchGeneralRequestDispather extends NPRequestDispatcher
{
    private static final MarryMatchGeneralRequestDispather _g_instance = new MarryMatchGeneralRequestDispather();

    protected MarryMatchGeneralRequestDispather()
    {
        regHandler(new NPRequestDealer<Common_R_255_001_ReqRpc>()
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
                RSRpcDispatcher.getInstance().dispatchRpc(_committer, rpcData);
            }
        });
    }

    public static MarryMatchGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }
}
