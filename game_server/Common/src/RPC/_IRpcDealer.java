package RPC;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 直接处理RPC操作的对象
 */
public interface _IRpcDealer {
    public void dispatchRpc(final _IWCGBasicRequestCommiter _committer, final _ARPCData _rpcData);

    public _IALProtocolReceiver getLocalRpcDealer();
}
