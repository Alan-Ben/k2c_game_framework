package RPC;


import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import CommonProto.Common_R_255_001_ReqRpc;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBack;
import NPCommon.Util.Delegate.HandlerNone;
import NPCommon.Util.RefWrap;
import WCGBasicPlatServer.BasicServerListener._AWCGBasicServerListener;
import WCGBasicServer._AWCGBasicServer;
import WCGCommon.Enum.NPEnum;

public class RpcSender
{
    private _AWCGBasicServerListener _m_serverListner = null; //某些清情况下，使用_AWCGBasicServerListener来发送请求
    private _AWCGBasicServer _m_serverObj = null;//使用_AWCGBasicServer来发送请求
    private NPEnum.EServerType _m_TargetServerTye = NPEnum.EServerType.NONE;
    private int _m_serverId = -1;
    private boolean _m_bSendToPlatServer = false;//是否发送给PlatServer

    public NPEnum.EServerType getServerType()
    {
        return NPEnum.EServerType.values()[_m_serverObj.getServerType()];
    }

    public int getServerTypeId()
    {
        return _m_serverObj.getServerTypeId();
    }

    public RpcSender(_AWCGBasicServerListener _serverListner)
    {
        _m_serverListner = _serverListner;
    }

    public RpcSender(_AWCGBasicServer _serverObj, NPEnum.EServerType _targetServerType)
    {
        this._m_serverObj = _serverObj;
        this._m_TargetServerTye = _targetServerType;
    }

    public RpcSender(_AWCGBasicServer _serverObj, NPEnum.EServerType _targetServerType, int _serverId)
    {
        this._m_serverObj = _serverObj;
        this._m_TargetServerTye = _targetServerType;
        this._m_serverId = _serverId;
        this._m_bSendToPlatServer = (_targetServerType == NPEnum.EServerType.SINGLE && _serverId <= 0);//单服切serverid为0,默认为ps
    }

    public RpcSender(_AWCGBasicServer _serverObj) //发送给PlatServer的构造函数
    {
        this._m_serverObj = _serverObj;
        _m_bSendToPlatServer = true;
    }

    /*******
     * 向服务器发送RPC，重试N次
     * @param _rpcData
     * @param _callback
     * @param _retryNum
     * @param <T>
     */
    public <T extends _ARPCData> void requestRepeat(final T _rpcData, final _ARpcCallBack<T> _callback, int _retryNum, final _ICallBack _failCallback)
    {
        if (_m_serverListner == null && _m_serverId == -1 && !_m_bSendToPlatServer)
        {
            CommLog.error("can not call request while not set server id, change to use requestTo alter");
            return;
        }
        requestToRepeat(_m_serverId, _rpcData, _callback, _retryNum, _failCallback);
    }
    /**
     * 兼容旧方法（默认不需要失败回调）
     * @param <T>
     * @param _rpcData
     * @param _callback
     * @param _retryNum
     */
    public <T extends _ARPCData> void requestRepeat(final T _rpcData, final _ARpcCallBack<T> _callback, int _retryNum)
    {
    	requestRepeat(_rpcData, _callback, _retryNum, null);
    }

    /******
     * 发送RPC，不重试
     * @param _rpcData
     * @param _callback
     * @param <T>
     */
    public <T extends _ARPCData> void request(final T _rpcData, final _ARpcCallBack<T> _callback)
    {
        if (_m_serverListner == null && _m_serverId == -1 && !_m_bSendToPlatServer)
        {
            CommLog.error("can not call request while not set server id, change to use requestTo alter");
            return;
        }
        requestTo(_m_serverId, _rpcData, _callback);
    }

    /********
     * 向服务器发送RPC，重试n次
     * @param _serverId
     * @param _rpcData
     * @param _callback
     * @param _retryNum
     * @param <T>
     */
    public <T extends _ARPCData> void requestToRepeat(final int _serverId, final T _rpcData, final _ARpcCallBack<T> _callback, int _retryNum, final _ICallBack _failCallback)
    {
        //封装rpc请求
        Common_R_255_001_ReqRpc requestProto = new Common_R_255_001_ReqRpc();
        requestProto.setClassId(_rpcData.getClassId());
        requestProto.setReqBytes(_rpcData.getRequestBytes());

        //如果是本服则直接处理
        if(null != _m_serverObj && _m_serverObj.getServerType() == _m_TargetServerTye.ordinal() && _m_serverObj.getServerTypeId() == _serverId
                && (_m_serverObj instanceof  _IRpcDealer))
        {
            _IRpcDealer rpcDealer = (_IRpcDealer)_m_serverObj;
            //直接本地处理
            RpcLocalDealerCommiter<T> commiter = new RpcLocalDealerCommiter<T>(rpcDealer.getLocalRpcDealer(), _rpcData, _callback);
            //开启任务单独处理
            ALSynTaskManager.getInstance().regTask(new _IALSynTask() {
                @Override
                public void run() {
                    rpcDealer.dispatchRpc(commiter, _rpcData);
                }
            });
        }
        else {
            //设置回调函数
            RPCCallBack<T> callbackDealer = new RPCCallBack<T>(_m_TargetServerTye, _serverId, _rpcData, _callback);
            RefWrap<Integer> retryNum = new RefWrap<>(_retryNum);
            callbackDealer.OnCallFailed.addHandler(null, new HandlerNone() {
                @Override
                public void handle() {
                    if (retryNum.get() > 0 || retryNum.get() == -1) {
                        CommLog.error("send RPC:{} to Server:{} typeid:{} faield, remain retry num:{} retry will start in next 5000 ms..."
                                , _rpcData.getClass().getSimpleName()
                                , _m_TargetServerTye
                                , _serverId
                                , retryNum.get()
                        );
                        if (retryNum.get() > 0) {
                            retryNum.v--;
                        }
                        ALSynTaskManager.getInstance().regTask(() -> _requestCore(_serverId, requestProto, callbackDealer), 5000);

                    } else //全部重试失败，调用失败回调方法
                    {
                        if (null != _failCallback) {
                            _failCallback.onRunOver();
                        }
                    }
                }
            });

            _requestCore(_serverId, requestProto, callbackDealer);
        }
    }

    /******
     * 发送RPC，不重试
     * @param _rpcData
     * @param _callback
     * @param <T>
     */
    public <T extends _ARPCData> void requestTo(final int _serverId, final T _rpcData, final _ARpcCallBack<T> _callback)
    {
        //如果是本服则直接处理
        if(null != _m_serverObj && _m_serverObj.getServerType() == _m_TargetServerTye.ordinal() && _m_serverObj.getServerTypeId() == _serverId
                && (_m_serverObj instanceof  _IRpcDealer))
        {
            _IRpcDealer rpcDealer = (_IRpcDealer)_m_serverObj;
            //直接本地处理
            RpcLocalDealerCommiter<T> commiter = new RpcLocalDealerCommiter<T>(rpcDealer.getLocalRpcDealer(), _rpcData, _callback);
            //开启任务单独处理
            ALSynTaskManager.getInstance().regTask(new _IALSynTask() {
                @Override
                public void run() {
                    //直接处理
                    rpcDealer.dispatchRpc(commiter, _rpcData);
                }
            });
        }
        else {
            //封装rpc请求
            Common_R_255_001_ReqRpc requestProto = new Common_R_255_001_ReqRpc();
            requestProto.setClassId(_rpcData.getClassId());
            requestProto.setReqBytes(_rpcData.getRequestBytes());
            //设置回调函数

            RPCCallBack<T> callbackDealer = null;
            if (null != _callback)
                callbackDealer = new RPCCallBack<T>(_m_TargetServerTye, _serverId, _rpcData, _callback);

            _requestCore(_serverId, requestProto, callbackDealer);
        }
    }

    /************
     * 发送请求
     * @param _serverId
     * @param requestProto
     * @param callbackDealer
     * @param <T>
     */
    private <T extends _ARPCData> void _requestCore(final int _serverId, Common_R_255_001_ReqRpc requestProto, final RPCCallBack<T> callbackDealer)
    {
        //发送请求
        if (_m_serverObj != null)
        {
            if (_m_bSendToPlatServer)
            {
                _m_serverObj.sendRequestToPlat(requestProto, callbackDealer);
            } else
            {
                _m_serverObj.sendRequestToBSServer(_m_TargetServerTye.ordinal(), _serverId, requestProto, callbackDealer);
            }
        } else if (_m_serverListner != null)
        {
            _m_serverListner.sendRequest(requestProto, callbackDealer);
        } else
        {
            CommLog.error("RPC No Sender");
        }
    }
}
