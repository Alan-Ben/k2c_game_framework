package NPCommonServer.NPCSGeneralListener.RequestDispather.p007_GameLogicOp;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CS_R.np_p007_GameLogicOp.NP2CS_R_007_002_ReqDiscardGameLogicInstance;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_006_DiscardInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CommonFunc;
import NPCommonServer.HandleServerMgr.HandleServerInfo;
import NPCommonServer.HandleServerMgr.HandleServerMgr;
import NPCommonServer.HandleServerMgr.HandleServerType;
import NPCommonServer.NPCSGeneralListener.Writer.NP2CS_RB_Writer_007_GameLogicOp;
import NPCommonServer.NPCommonServer;
import NPServerProtocolWriter.NP2GLS.Request.NP2GLS_R_Writer_001_BasicOp;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum;

public class RequestDealer_NP2CS_R_007_002_ReqGameLogicInstance extends NPRequestDealer<NP2CS_R_007_002_ReqDiscardGameLogicInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2CS_R_007_002_ReqDiscardGameLogicInstance _msg)
    {
        //获取对应的处理服务器对象
        _AWCGBSReceiverListener listener = (_AWCGBSReceiverListener) _committer.getRequestDealer();
        if (null == listener)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        int severTypeId = CommonFunc.parseGameLogicServerId(_msg.getInstanceId());
        HandleServerMgr mgr = HandleServerType.getInstance().getHandleServerMgr(NPEnum.EServerType.GAME_LOGIC.ordinal());
        HandleServerInfo handle = mgr.lookupServerById(severTypeId);
        if(null == handle)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        handle.reduceHandleUser(1);

        //发送注册消息到平台服务器
        NPCommonServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.GAME_LOGIC.ordinal(), severTypeId,
                NP2GLS_R_Writer_001_BasicOp.make_006_DiscardInstance(_msg.getInstanceId()), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2GLS_RB_001_006_DiscardInstance();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _msg)
                    {
                        _committer.commitSucRes(NP2CS_RB_Writer_007_GameLogicOp.make_002_RetDiscardGameLogicInstance());
                    }

                    @Override
                    public void dealFail(int _error)
                    {
                        _committer.commitFailRes(_error);
                    }
                });
    }
}
