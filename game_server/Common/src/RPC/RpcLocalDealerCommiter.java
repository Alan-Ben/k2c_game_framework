package RPC;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 本地处理RPC消息的提交对象
 */
public class RpcLocalDealerCommiter<T extends _ARPCData> implements _IWCGBasicRequestCommiter
{
    private _IALProtocolReceiver _m_requestDealer;
    private T _m_rpcData;
    private _ARpcCallBack<T> _m_cbCallback;

    public RpcLocalDealerCommiter(_IALProtocolReceiver _requestDealer, T _rpcData, _ARpcCallBack<T> _callback)
    {
        _m_requestDealer = _requestDealer;
        this._m_rpcData = _rpcData;
        this._m_cbCallback = _callback;
    }

    @Override
    public _IALProtocolReceiver getRequestDealer() {
        return _m_requestDealer;
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _msg) {
        if(null != _m_cbCallback)
            _m_cbCallback.call_back(0, _m_rpcData);
    }

    @Override
    public void commitFailRes(int _errCode) {
        if(null != _m_cbCallback)
            _m_cbCallback.call_back(_errCode, _m_rpcData);
    }
}
