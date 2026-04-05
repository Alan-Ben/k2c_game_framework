package NPCommonServer.NPCSGeneralListener.RequestDispather.p007_GameLogicOp;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CS_R.np_p007_GameLogicOp.NP2CS_R_007_001_ReqGameLogicInstance;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_005_RegGroupInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
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

public class RequestDealer_NP2CS_R_007_001_ReqGameLogicInstance extends NPRequestDealer<NP2CS_R_007_001_ReqGameLogicInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2CS_R_007_001_ReqGameLogicInstance _msg)
    {
        //获取对应的处理服务器对象
        _AWCGBSReceiverListener listener = (_AWCGBSReceiverListener) _committer.getRequestDealer();
        if (null == listener)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        HandleServerMgr mgr = HandleServerType.getInstance().getHandleServerMgr(NPEnum.EServerType.GAME_LOGIC.ordinal());
        HandleServerInfo handle = mgr.tryHandleServer(1);
        if (handle == null)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        //发送注册消息到平台服务器
        NPCommonServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.GAME_LOGIC.ordinal(), handle.getServerTypeId(),
                NP2GLS_R_Writer_001_BasicOp.make_005_RegGroupInstance(_msg.getGroupId(), _msg.getActivityId(), _msg.get_buffer_AddInfo()), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2GLS_RB_001_005_RegGroupInstance();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _msg)
                    {
                        NP2GLS_RB_001_005_RegGroupInstance msg = (NP2GLS_RB_001_005_RegGroupInstance) _msg;
                        //返回信息
                        _committer.commitSucRes(NP2CS_RB_Writer_007_GameLogicOp.make_001_RetGameLogicInstance(msg.getInstanceId()));
                    }

                    @Override
                    public void dealFail(int _error)
                    {
                        _committer.commitFailRes(_error);
                    }
                });
    }
}
