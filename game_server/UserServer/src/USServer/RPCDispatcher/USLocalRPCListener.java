package USServer.RPCDispatcher;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NPUSServer.NPUserServer;

/**
 * US服务器本地处理RPC的处理对象
 */
public class USLocalRPCListener implements _IALProtocolReceiver, _IUSRPCDealer
{
    private NPUserServer _m_server;

    public USLocalRPCListener(NPUserServer _server)
    {
        _m_server = _server;
    }

    @Override
    public NPUserServer getUSServer() {
        return _m_server;
    }
}
