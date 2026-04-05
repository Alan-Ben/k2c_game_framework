package RPC;

import ALBasicProtocolPack._IALProtocolStructure;
import CommonProto.Common_RB_255_001_RetRpc;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.ADelegateNone;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

public class RPCCallBack<T extends _ARPCData> implements _IWCGCallbackDealer
{


    private final T _m_rpcData;
    private final _ARpcCallBack<T> _m_callback;
    private final NPEnum.EServerType _m_serverType;
    private final int _m_serverTypeId;

    public ADelegateNone OnCallFailed = new ADelegateNone(this);

    public RPCCallBack(NPEnum.EServerType _serverType, int _serverTypeId, T _rpcData, _ARpcCallBack<T> _callback)
    {
        _m_serverType = _serverType;
        _m_serverTypeId = _serverTypeId;
        _m_rpcData = _rpcData;
        _m_callback = _callback;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new Common_RB_255_001_RetRpc();
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        Common_RB_255_001_RetRpc retProto = (Common_RB_255_001_RetRpc) _msg;
        _m_rpcData.readResponse(retProto.get_buffer_RetBytes());
        if (null != _m_callback)
        {
            _m_callback.call_back(0, _m_rpcData);
        }
    }

    @Override
    public void dealFail(int _errCode)
    {
        if (null != _m_callback)
        {
            _m_callback.call_back(_errCode, _m_rpcData);
        }
        if (_m_serverType != NPEnum.EServerType.SINGLE)
        {
            CommLog.error("RPC to server:[{}] typeId:[{}] failed {} err:{}", _m_serverType, _m_serverTypeId, _m_rpcData.getClass().getSimpleName(), _errCode);
        } else
        {
            NPEnum.ENPSingleServerType eSingleServerType = NPEnum.ENPSingleServerType.values()[_m_serverTypeId];
            if (null == eSingleServerType)
            {
                CommLog.error("RPC to unknow single server Id:[{}] failed {},err:{}", _m_serverTypeId, _m_rpcData.getClass().getSimpleName(), _errCode);
            } else
            {
                CommLog.error("RPC to single server:[{}] failed {},err:{}", eSingleServerType, _m_rpcData.getClass().getSimpleName(), _errCode);
            }

        }
        OnCallFailed.onEvent();
    }
}
