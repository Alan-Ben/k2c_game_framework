package NPRecordServer.NPGeneralListener.RequestDispather;


import CommonProto.Common_R_255_001_ReqRpc;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.Log.CommLog;
import NPRecordServer.NPGeneralListener.RequestDispather.NP2RCS_001_RSOp.NP2RCSRequestDealer_R_001_001_ReqUpdateLoginRecord;
import NPRecordServer.NPGeneralListener.RequestDispather.NP2RCS_001_RSOp.NP2RCSRequestDealer_R_001_002_ReqLookupLoginRecord;
import NPRecordServer.RPCDispatcher.RSRpcDispatcher;
import RPC.RPCDataFactoryMgr;
import RPC._ARPCData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


/********************
 * 平台服务器发送来请求的处理对象
 *
 * @author Administrator
 *
 */
public class NPRCSGeneralRequestDispather extends NPRequestDispatcher
{
    private static final NPRCSGeneralRequestDispather _g_instance = new NPRCSGeneralRequestDispather();

    protected NPRCSGeneralRequestDispather()
    {
        this.regHandler(new NP2RCSRequestDealer_R_001_001_ReqUpdateLoginRecord());
        this.regHandler(new NP2RCSRequestDealer_R_001_002_ReqLookupLoginRecord());

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

    public static NPRCSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }
}
