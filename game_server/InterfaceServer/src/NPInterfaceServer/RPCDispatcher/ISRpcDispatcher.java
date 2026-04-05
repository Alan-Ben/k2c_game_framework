package NPInterfaceServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class ISRpcDispatcher extends RpcDispatcher
{
    private static ISRpcDispatcher _instance = new ISRpcDispatcher();

    public static ISRpcDispatcher getInstance()
    {
        return _instance;
    }
}
