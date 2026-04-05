package DinnerServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class DinnerRpcDispatcher extends RpcDispatcher
{
    private static DinnerRpcDispatcher _instance = new DinnerRpcDispatcher();

    public static DinnerRpcDispatcher getInstance()
    {
        return _instance;
    }
}
