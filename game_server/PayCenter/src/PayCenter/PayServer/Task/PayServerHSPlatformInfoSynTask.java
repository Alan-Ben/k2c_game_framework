package PayCenter.PayServer.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2HS_RB.p001_HSOp.NP2HS_RB_001_005_RetPlatformInfo;
import NPCommon.Log.CommLog;
import NPServerProtocolWriter.NP2HS.Np2HS_R_Writer_001_HSOP;
import PayCenter.PayServer.PayServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

/**
 * @description:
 * @author: ricci
 * @date: 2022-06-29 14:32:51
 */
public class PayServerHSPlatformInfoSynTask implements _IALSynTask
{
    private PayServer _m_server;

    public PayServerHSPlatformInfoSynTask(PayServer _server)
    {
        _m_server = _server;
    }

    public PayServer getPayServer()
    {
        return _m_server;
    }

    @Override
    public void run()
    {
        getPayServer().sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(),
                NPEnum.ENPSingleServerType.HTTP.ordinal(), Np2HS_R_Writer_001_HSOP.make_001_005_ReqPlatformInfo(), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2HS_RB_001_005_RetPlatformInfo();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        NP2HS_RB_001_005_RetPlatformInfo ret = (NP2HS_RB_001_005_RetPlatformInfo) _retProto;
                        _m_server.getConf().setPlatformId(ret.getPlatformId());
                        _m_server.getConf().setPlatAreaId(ret.getPlatAreaId());

                        CommLog.info("PayServer[{}] PayServerHSPlatformInfoSynTask deal success, platformId:{} platAreaId:{}", _m_server.getConf().getId(), ret.getPlatformId(), ret.getPlatAreaId());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("PayServer[{}] PayServerHSPlatformInfoSynTask deal fail err:{}", _m_server.getConf().getId(), _errCode);

                        //如果是非正式服务器则不重试
                        ALSynTaskManager.getInstance().regTask(PayServerHSPlatformInfoSynTask.this, 3000);
                    }
                });
    }
}
