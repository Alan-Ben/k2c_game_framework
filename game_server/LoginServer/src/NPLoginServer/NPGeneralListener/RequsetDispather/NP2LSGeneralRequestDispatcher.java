package NPLoginServer.NPGeneralListener.RequsetDispather;

import CommonProto.Common_R_255_001_ReqRpc;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.Log.CommLog;
import NPLoginServer.RPCDispatcher.LSRpcDispatcher;
import RPC.RPCDataFactoryMgr;
import RPC._ARPCData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*************
 * 在LS服务器接收所有请求类消息的处理对象
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2019年3月26日 下午11:08:39
 */
public class NP2LSGeneralRequestDispatcher extends NPRequestDispatcher
{
    private static NP2LSGeneralRequestDispatcher _g_instance = new NP2LSGeneralRequestDispatcher();

    public static NP2LSGeneralRequestDispatcher getInstance()
    {
        return _g_instance;
    }

    protected NP2LSGeneralRequestDispatcher()
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
                LSRpcDispatcher.getInstance().dispatchRpc(_committer, rpcData);
            }
        });
    }
}
