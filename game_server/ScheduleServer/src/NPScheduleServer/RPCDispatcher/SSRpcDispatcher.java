package NPScheduleServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class SSRpcDispatcher extends RpcDispatcher
{
    private static SSRpcDispatcher _instance = new SSRpcDispatcher();

    public static SSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
