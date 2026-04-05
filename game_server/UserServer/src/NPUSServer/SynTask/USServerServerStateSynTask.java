package NPUSServer.SynTask;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_007_RetUSInfoListByTypeId;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * @description:
 * @author: ricci
 * @date: 2022-06-29 14:32:51
 */
public class USServerServerStateSynTask implements _IALSynTask
{
    private NPUserServer _m_server;

    public USServerServerStateSynTask(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void run()
    {
        final USServerServerStateSynTask self = this;

        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(),
                NP2CS_R_Writer_002_ServerInfoOp.make_007_ReqUSInfoListByTypeId(getUSServer().getServerTypeId())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_007_RetUSInfoListByTypeId();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CS_RB_002_007_RetUSInfoListByTypeId proto = (NP2CS_RB_002_007_RetUSInfoListByTypeId) _proto;

                        getUSServer().setUsState(proto.getServerItem().getOnlineStateTypeId(),
                                proto.getServerItem().getShowStateTypeId(), proto.getServerItem().getStartDate());
                    }

                    @Override
                    public void dealFail(int i)
                    {
                        USLog.error(_m_server, "NPUserServer synUSOnlineState deal fail err:{}", i);
                        ALSynTaskManager.getInstance().regTask(self, 3000);
                    }
                });
    }
}
