package NPGateServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class GSRpcDispatcher extends RpcDispatcher
{
    private static GSRpcDispatcher _instance = new GSRpcDispatcher();

    public static GSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
