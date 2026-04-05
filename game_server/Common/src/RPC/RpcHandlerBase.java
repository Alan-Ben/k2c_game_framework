package RPC;

import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public abstract class RpcHandlerBase
{
    public abstract void handleRpc(_IWCGBasicRequestCommiter _committer, _ARPCData _rpcData);

    public abstract Class<?> getDataClass();
}
