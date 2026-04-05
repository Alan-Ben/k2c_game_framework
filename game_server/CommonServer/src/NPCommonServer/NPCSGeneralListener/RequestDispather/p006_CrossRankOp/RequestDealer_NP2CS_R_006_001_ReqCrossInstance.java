package NPCommonServer.NPCSGeneralListener.RequestDispather.p006_CrossRankOp;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CRS_RB.p001_CrossRankOp.NP2CRS_RB_001_005_RequestCrossInstance;
import NP2CS_R.np_p006_RankOp.NP2CS_R_006_001_ReqCrossInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.RankErr;
import NPCommonServer.CrossRankServerHandleMgr.CrossRankServerInfo;
import NPCommonServer.CrossRankServerHandleMgr.CrossRankServerMgr;
import NPCommonServer.NPCSGeneralListener.Writer.NP2CS_RB_Writer_006_CrossRankOp;
import NPCommonServer.NPCommonServer;
import NPServerProtocolWriter.NP2CRS.Request.NP2CRS_R_Writer_001_BasicOp;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum;

public class RequestDealer_NP2CS_R_006_001_ReqCrossInstance extends NPRequestDealer<NP2CS_R_006_001_ReqCrossInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2CS_R_006_001_ReqCrossInstance _msg)
    {
        //获取对应的处理服务器对象
        _AWCGBSReceiverListener listener = (_AWCGBSReceiverListener) _committer.getRequestDealer();
        if (null == listener)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        CrossRankServerInfo serverInfo = CrossRankServerMgr.getInstance().tryHandleServer(1);
        if (serverInfo == null)
        {
            _committer.commitFailRes(RankErr.CROSS_RANK_HANDLE_FAIL.getCode());
            return;
        }

        //发送注册消息到平台服务器
        NPCommonServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.CROSS_RANK.ordinal(), serverInfo.getServerTypeId(),
                NP2CRS_R_Writer_001_BasicOp.make_005_RequestCrossInstance(), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CRS_RB_001_005_RequestCrossInstance();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _msg)
                    {
                        NP2CRS_RB_001_005_RequestCrossInstance msg = (NP2CRS_RB_001_005_RequestCrossInstance) _msg;
                        //返回信息
                        _committer.commitSucRes(NP2CS_RB_Writer_006_CrossRankOp.make_001_RetCrossInstance(msg.getCrossInstanceId()));
                    }

                    @Override
                    public void dealFail(int _error)
                    {
                        CrossRankServerMgr.getInstance().reduceWeight(serverInfo.getServerTypeId(), 1);
                        _committer.commitFailRes(_error);
                    }
                });
    }
}
