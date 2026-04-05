package NPUSServer.CrossGameSys;

import ALBasicProtocolPack._IALProtocolStructure;
import CrossGame_R.p001_BasicOp.CrossGame_R_001_001_GCForwardMsg;
import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_001_SendCrossGameRequest;
import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_002_ReqCreateCrossGameInstance;
import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_003_ReqDiscardCrossGameInstance;
import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_004_ReqGM;
import NP2CGS_RB.p001_BasicOp.NP2CGS_RB_001_002_RetCreateCrossGameInstance;
import NP2CGS_RB.p001_BasicOp.NP2CGS_RB_001_003_RetDiscardCrossGameInstance;
import NP2CGS_RB.p001_BasicOp.NP2CGS_RB_001_004_RetGM;
import NP2CS_R.np_p002_serverInfoOp.NP2CS_R_002_009_ReqTryCrossGameServer;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_009_RetTryCrossGameServer;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackT;
import NPEnum.ENPCommonGeneralEnum;
import NPEnum.ENPCrossGameCategoryEnum;
import NPUSServer.CrossGameSys.CallbackDealer.CrossGameCallbackDealer_GCForwardCallback;
import NPUSServer.CrossGameSys.CallbackDealer._ICallBack_GCCreateInstanceHandle;
import NPUSServer.CrossGameSys.CallbackDealer._ICallBack_GCMsgForwardHandle;
import NPUSServer.CrossGameSys.CallbackDealer._ICallBack_TryCgsServerHandle;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;

public abstract class _ANPCrossGameInterface
{
    private NPUserServer _m_server;

    public _ANPCrossGameInterface(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer() {return _m_server;}

    /**
     * 指定类型
     * @return
     */
    public abstract ENPCrossGameCategoryEnum getCategory();

    /**
     * 转发GCMsg协议
     * @param _msgItem
     * @param _instanceId
     * @param _proto
     */
    public void sendGCMsgToCrossGameInstance(_ANPUSUserBasicMsgItem _msgItem, long _instanceId
            , _IALProtocolStructure _proto)
    {
        //封装玩家协议数据
        CrossGame_R_001_001_GCForwardMsg forwardProto = new CrossGame_R_001_001_GCForwardMsg();
        forwardProto.setCid(_msgItem.getUserData().getCid());
        forwardProto.setMsg(_proto.makeFullPackage());

        //发送GCMsg转发消息
        sendRequestToCrossGameInstance(_msgItem.getUserData().getCid(), _instanceId, forwardProto.makeFullPackage(), new CrossGameCallbackDealer_GCForwardCallback(_msgItem));
    }

    /**
     * 转发GMMsg协议，并自定义回调处理
     * @param _msgItem
     * @param _instanceId
     * @param _proto
     * @param _callbackDealer
     */
    public void sendGCMsgToCrossGameInstance(_ANPUSUserBasicMsgItem _msgItem, long _instanceId
            , _IALProtocolStructure _proto
            , _ICallBack_GCMsgForwardHandle _callbackDealer)
    {
        //封装玩家协议数据
        CrossGame_R_001_001_GCForwardMsg forwardProto = new CrossGame_R_001_001_GCForwardMsg();
        forwardProto.setCid(_msgItem.getUserData().getCid());
        forwardProto.setMsg(_proto.makeFullPackage());

        //发送GCMsg转发消息
        sendRequestToCrossGameInstance(_msgItem.getUserData().getCid(), _instanceId, forwardProto.makeFullPackage()
                , new CrossGameCallbackDealer_GCForwardCallback(_callbackDealer, _msgItem));
    }

    /**
     * 发送Request请求
     * @param _cid
     * @param _instanceId
     * @param _reqMsg
     * @param _callback
     */
    public void sendRequestToCrossGameInstance(long _cid, long _instanceId, ByteBuffer _reqMsg, _IWCGCallbackDealer _callback)
    {
        NP2CGS_R_001_001_SendCrossGameRequest proto = new NP2CGS_R_001_001_SendCrossGameRequest();
        proto.setCategory(getCategory());
        proto.setCid(_cid);
        proto.setInstanceId(_instanceId);
        proto.setReqMsg(_reqMsg);

        //发送请求
        int cgsTypeId = parseCrossGameServerTypeId(_instanceId);
        if (cgsTypeId <= 0)
        {
            if (null != _callback)
                _callback.dealFail(CommErr.PARAM_ERROR.getCode());

            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_GAME.ordinal(), cgsTypeId, proto, _callback);
    }

    /**
     * 创建新实例请求
     * @param _instanceId 实例id
     * @param _extData    额外数据
     * @param _callback   回调
     */
    public void createNewInstance(long _instanceId, ByteBuffer _extData, _ICallBack_GCCreateInstanceHandle _callback)
    {
        NP2CGS_R_001_002_ReqCreateCrossGameInstance proto = new NP2CGS_R_001_002_ReqCreateCrossGameInstance();
        proto.setCategory(getCategory());
        proto.setInstanceId(_instanceId);
        proto.setExtData(_extData);

        //发送请求
        int cgsTypeId = parseCrossGameServerTypeId(_instanceId);
        if (cgsTypeId <= 0)
        {
            _callback.onRunOver(CommErr.PARAM_ERROR.getCode(), null);
            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_GAME.ordinal(), cgsTypeId, proto, new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2CGS_RB_001_002_RetCreateCrossGameInstance();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _ret)
            {
                NP2CGS_RB_001_002_RetCreateCrossGameInstance ret = (NP2CGS_RB_001_002_RetCreateCrossGameInstance) _ret;

                _callback.onRunOver(0, ret.get_buffer_RetMsg());
            }

            @Override
            public void dealFail(int _errCode)
            {
                USLog.error(_m_server, " _ANPCrossGameInterface createNewInstance fail errCode:{}", _errCode);

                _callback.onRunOver(_errCode, null);
            }
        });
    }

    /**
     * 发送请求销毁实例
     * @param _instanceId
     */
    public void sendRequestCrossGameInstanceDiscard(long _instanceId, _ICallBackBool _callback)
    {
        NP2CGS_R_001_003_ReqDiscardCrossGameInstance proto = new NP2CGS_R_001_003_ReqDiscardCrossGameInstance();
        proto.setCategory(getCategory());
        proto.setInstanceId(_instanceId);

        //发送请求
        int cgsTypeId = parseCrossGameServerTypeId(_instanceId);
        if (cgsTypeId <= 0)
        {
            if (null != _callback)
                _callback.onRunOver(false);

            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_GAME.ordinal(), cgsTypeId, proto, new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2CGS_RB_001_003_RetDiscardCrossGameInstance();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _ret)
            {
                if (null != _callback)
                {
                    _callback.onRunOver(true);
                }
            }

            @Override
            public void dealFail(int _errCode)
            {
                USLog.error(_m_server, " _ANPCrossGameInterface discardInstance fail errCode:{}", _errCode);

                if (null != _callback)
                {
                    _callback.onRunOver(false);
                }
            }
        });
    }

    /**
     * 销毁实例无回调
     * @param _instanceId
     */
    public void sendRequestCrossGameInstanceDiscard(long _instanceId)
    {
        sendRequestCrossGameInstanceDiscard(_instanceId, null);
    }

    ///////////////////////////////// 静态方法 /////////////////////////////////

    /**
     * 尝试获取对应的CrossGame服务器
     * @param _commGeneralV
     * @param _tmpWeight
     * @param _callback
     */
    public void tryCGSServerTypeId(ENPCommonGeneralEnum _commGeneralV, int _tmpWeight, _ICallBack_TryCgsServerHandle _callback)
    {
        NP2CS_R_002_009_ReqTryCrossGameServer proto = new NP2CS_R_002_009_ReqTryCrossGameServer();
        proto.setGeneralV(_commGeneralV);
        proto.setTmpHandleWeight(_tmpWeight);

        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal()
                , ENPSingleServerType.COMMON.ordinal()
                , proto
                , new _IWCGCallbackDealer()
                {

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CS_RB_002_009_RetTryCrossGameServer ret = (NP2CS_RB_002_009_RetTryCrossGameServer) _proto;

                        _callback.dealSuc(ret.getInstanceId());
                    }

                    @Override
                    public void dealFail(int paramInt)
                    {
                        _callback.dealFail(paramInt);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_009_RetTryCrossGameServer();
                    }
                });
    }

    /**
     * 尝试获取对应的CrossGame服务器，默认权重1
     * @param _commGeneralV ENPCommonGeneralEnum
     * @param _callback     _ICallBack_TryCgsServerHandle
     */
    public void tryCGSServerTypeId(ENPCommonGeneralEnum _commGeneralV, _ICallBack_TryCgsServerHandle _callback)
    {
        tryCGSServerTypeId(_commGeneralV, 10, _callback);
    }

    /**
     * 解析获取CGS的服务器ID
     * @param _instacneId
     * @return
     */
    public static int parseCrossGameServerTypeId(long _instacneId)
    {
        return (int) (_instacneId % 100000);
    }


    /**
     * 去cgs上执行gm命令
     * @param _gm         String GM命令
     * @param _instanceId 实例id
     * @param _callBack   回调
     */
    public void runCrossGameServerGM(String _gm, long _instanceId, _ICallBackT<String> _callBack)
    {

        String finalGM = _gm.replace('%', ' ');

        NP2CGS_R_001_004_ReqGM proto = new NP2CGS_R_001_004_ReqGM();
        proto.setGm(finalGM);

        //发送请求
        int cgsTypeId = parseCrossGameServerTypeId(_instanceId);
        if (cgsTypeId <= 0)
        {
            if (null != _callBack)
                _callBack.onRunOver("_errCode" + CommErr.PARAM_ERROR.getCode());

            return;
        }

        getUSServer().sendRequestToBSServer(EServerType.CROSS_GAME.ordinal(), cgsTypeId, proto, new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2CGS_RB_001_004_RetGM();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _ret)
            {
                NP2CGS_RB_001_004_RetGM ret = (NP2CGS_RB_001_004_RetGM) _ret;

                _callBack.onRunOver(ret.getReturnMsg());
            }

            @Override
            public void dealFail(int _errCode)
            {
                USLog.error(_m_server, " _ANPCrossGameInterface runCrossGameServerGM fail errCode:{}", _errCode);

                _callBack.onRunOver("_errCode" + _errCode);
            }
        });
    }
}
