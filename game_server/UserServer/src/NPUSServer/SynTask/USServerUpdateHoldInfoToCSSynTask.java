package NPUSServer.SynTask;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_004_RetUpdateUSHoldInfo;
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
public class USServerUpdateHoldInfoToCSSynTask implements _IALSynTask
{
    private NPUserServer _m_server;

    public USServerUpdateHoldInfoToCSSynTask(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void run()
    {
        final USServerUpdateHoldInfoToCSSynTask self = this;

        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(),
                NP2CS_R_Writer_002_ServerInfoOp.make_004_ReqUpdateUSHoldInfo(getUSServer().makeHoldInfo())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_004_RetUpdateUSHoldInfo();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {

                    }

                    @Override
                    public void dealFail(int i)
                    {
                        //重试
                        USLog.error(_m_server, "USServerUpdateHoldInfoToCSSynTask dealFail err:{}", i);
                        ALSynTaskManager.getInstance().regTask(self, 3000);
                    }
                });
    }
}
