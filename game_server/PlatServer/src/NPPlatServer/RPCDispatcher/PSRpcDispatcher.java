package NPPlatServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class PSRpcDispatcher extends RpcDispatcher
{
    private static PSRpcDispatcher _instance = new PSRpcDispatcher();

    public static PSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
