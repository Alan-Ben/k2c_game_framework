package PayCenter.PayServer.GeneralListener.RequestDispather;

import CommonProto.Common_R_255_001_ReqRpc;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.Log.CommLog;
import PayCenter.PayServer.RPCDispatcher.PCSRpcDispatcher;
import RPC.RPCDataFactoryMgr;
import RPC._ARPCData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * PCSGeneral_255_RequestDispatcher - 通用系统请求分发器
 * 
 * 主要功能：
 * 1. 处理协议号255的通用系统请求
 * 2. 支持RPC请求的分发和处理
 * 3. 集成RPC分发器处理复杂请求
 * 
 * 设计特点：
 * - 静态初始化模式
 * - RPC请求透明处理
 * - 错误处理和日志记录
 */
public class PayGeneral_255_RequestDispatcher extends NPRequestDispatcher
{
    public static void init(PCSGeneralRequestDispather _dispatcher)
    {
        _dispatcher.regHandler(new NPRequestDealer<Common_R_255_001_ReqRpc>()
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
                PCSRpcDispatcher.getInstance().dispatchRpc(_committer, rpcData);
            }
        });
    }
}