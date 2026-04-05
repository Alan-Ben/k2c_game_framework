package NPCrossGameServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class CGSRpcDispatcher extends RpcDispatcher
{
    private static CGSRpcDispatcher _instance = new CGSRpcDispatcher();

    public static CGSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
