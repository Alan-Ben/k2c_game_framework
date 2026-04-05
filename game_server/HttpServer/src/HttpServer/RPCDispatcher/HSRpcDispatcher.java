package HttpServer.RPCDispatcher;

import HttpServer.RPCDispatcher.Common.HSDDAlert_Handler;
import RPC.RpcDispatcher;

public class HSRpcDispatcher extends RpcDispatcher
{
    private static HSRpcDispatcher _instance = new HSRpcDispatcher();

    public static HSRpcDispatcher getInstance()
    {
        return _instance;
    }

    public HSRpcDispatcher()
    {
        HSDDAlert_Handler.register(this);

    }
}
