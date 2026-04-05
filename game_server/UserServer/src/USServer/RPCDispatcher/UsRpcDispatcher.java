package USServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class UsRpcDispatcher extends RpcDispatcher
{
    private static UsRpcDispatcher _instance = new UsRpcDispatcher();

    public static UsRpcDispatcher getInstance()
    {
        return _instance;
    }
}
