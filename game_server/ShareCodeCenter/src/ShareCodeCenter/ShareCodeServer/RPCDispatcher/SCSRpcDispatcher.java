package ShareCodeCenter.ShareCodeServer.RPCDispatcher;

public class SCSRpcDispatcher extends RPC.RpcDispatcher
{
    private static SCSRpcDispatcher _g_instance = new SCSRpcDispatcher();

    public static SCSRpcDispatcher getInstance()
    {
        return _g_instance;
    }
}
