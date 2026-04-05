package RPC;

public abstract class _ARpcCallBack<T extends _ARPCData>
{
    public abstract void call_back(int _errCode, T _rpc);
}
