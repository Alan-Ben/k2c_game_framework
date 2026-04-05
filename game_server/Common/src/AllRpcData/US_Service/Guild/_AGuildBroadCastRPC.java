package AllRpcData.US_Service.Guild;

import ALBasicProtocolPack._IALProtocolStructure;
import RPC._ARPCBase;

/****
 * 公会广播RPC必须实现的接口，设置Cid
 */
public abstract class _AGuildBroadCastRPC<TRequest extends _IALProtocolStructure, TResponse extends _IALProtocolStructure>
        extends _ARPCBase<TRequest, TResponse> {
    public abstract _AGuildBroadCastRPC<TRequest, TResponse> cloneRPCForCid(long _cid);
}
