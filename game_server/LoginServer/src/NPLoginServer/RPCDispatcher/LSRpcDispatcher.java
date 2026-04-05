package NPLoginServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class LSRpcDispatcher extends RpcDispatcher
{
    private static LSRpcDispatcher _instance = new LSRpcDispatcher();

    public static LSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
