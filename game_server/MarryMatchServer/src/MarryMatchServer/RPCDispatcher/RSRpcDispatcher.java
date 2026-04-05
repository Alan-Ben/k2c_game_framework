package MarryMatchServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class RSRpcDispatcher extends RpcDispatcher
{
    private static RSRpcDispatcher _instance = new RSRpcDispatcher();

    public static RSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
