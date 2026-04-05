package NPCommonServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class CSRpcDispatcher extends RpcDispatcher
{
    private static CSRpcDispatcher _instance = new CSRpcDispatcher();

    public static CSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
