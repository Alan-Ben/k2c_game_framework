package USServer.RPCDispatcher;

import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import RPC.RpcRequestHandler;
import RPC._ARPCData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public abstract class _ATBasicUSRpc_Handler<T extends _ARPCData> extends RpcRequestHandler<T>
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, T _rpc)
    {
        _IUSRPCDealer usRpcDealer = (_IUSRPCDealer)_committer.getRequestDealer();

        NPUserServer usServer = usRpcDealer.getUSServer();
        if(null == usServer)
        {
            USLog.fatal("Can not cast Commiter into USServer !!!");
            return ;
        }

        //调用子类处理函数
        _deal(usServer, _rpc);
    }

    /***************
     * 将接收对象统一转化为US之后处理
     * @param _usServer
     * @param _rpc
     */
    protected abstract void _deal(NPUserServer _usServer, T _rpc);
}
