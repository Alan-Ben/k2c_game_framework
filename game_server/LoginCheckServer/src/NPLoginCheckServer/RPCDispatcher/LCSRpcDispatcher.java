package NPLoginCheckServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class LCSRpcDispatcher extends RpcDispatcher
{
    private static LCSRpcDispatcher _instance = new LCSRpcDispatcher();

    public static LCSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
