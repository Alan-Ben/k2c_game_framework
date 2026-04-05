package GameLogicServer.RPCDispatcher;

import RPC.RpcDispatcher;

public class GLSRpcDispatcher extends RpcDispatcher
{
    private static GLSRpcDispatcher _instance = new GLSRpcDispatcher();

    public static GLSRpcDispatcher getInstance()
    {
        return _instance;
    }
}
