package CrossTeamServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class CTSRpcDispatcher extends RpcDispatcher
{
    private static CTSRpcDispatcher _instance = new CTSRpcDispatcher();

    public static CTSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
