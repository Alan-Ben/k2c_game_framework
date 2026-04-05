package NPUSServer.SynTask;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2HS_RB.p001_HSOp.NP2HS_RB_001_005_RetPlatformInfo;
import NPServerProtocolWriter.NP2HS.Np2HS_R_Writer_001_HSOP;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UserServerConf;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

/**
 * @description:
 * @author: ricci
 * @date: 2022-06-29 14:32:51
 */
public class USServerHSPlatformInfoSynTask implements _IALSynTask
{
    private NPUserServer _m_server;

    public USServerHSPlatformInfoSynTask(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void run()
    {
        final USServerHSPlatformInfoSynTask self = this;
        getUSServer().sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(),
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
                        UserServerConf.getInstance().setHSPlatformId(ret.getPlatformId());
                        UserServerConf.getInstance().setHSPlatAreaId(ret.getPlatAreaId());

                        USLog.info(_m_server, "NPUserServer USServerHSPlatformInfoSynTask deal success, hsPlatformId:{} hsPlatAreaId:{}", ret.getPlatformId(), ret.getPlatAreaId());
                        
                        //如果HS的平台数据与US不一致，则需要日志+钉钉预警
                        if(UserServerConf.getInstance().getHSPlatformId() != UserServerConf.getInstance().getPlatformId()
                        		|| UserServerConf.getInstance().getHSPlatAreaId() != UserServerConf.getInstance().getPlatAreaId())
                        {
                        	StringBuilder errSb = new StringBuilder();
                        	errSb.append("HS PlatForm:").append(UserServerConf.getInstance().getHSPlatformId()).append("-").append(UserServerConf.getInstance().getHSPlatAreaId())
                        	.append(" not equal US PlatForm:").append(UserServerConf.getInstance().getPlatformId()).append("-").append(UserServerConf.getInstance().getPlatAreaId());
                        	
                        	String errStr = errSb.toString();
                        	
                        	//日志输出
                        	USLog.error(_m_server, errStr);
                        	//钉钉预警
                        	getUSServer().getDDAlert().err("plat-err", errStr);
                        }
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(_m_server, "NPUserServer USServerHSPlatformInfoSynTask deal fail err:{}", _errCode);

                        //如果是非正式服务器则不重试
                        if(!UserServerConf.getInstance().getIsIllegalServer())
                            ALSynTaskManager.getInstance().regTask(self, 3000);
                    }
                });
    }
}
