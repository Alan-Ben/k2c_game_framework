package NPCommonServer.USServerListMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALTask._IALSynTask;
import NP2HS_RB.p001_HSOp.NP2HS_RB_001_001_RetServerInfoList;
import NPCommon.Log.CommLog;
import NPCommonServer.NPCommonServer;
import NPServerProtocolWriter.NP2HS.Np2HS_R_Writer_001_HSOP;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class USServerListLoadFromPHPTask implements _IALSynTask
{
	//序列号
	private long _m_lSerial;
	
	public USServerListLoadFromPHPTask(long _serial)
	{
		_m_lSerial = _serial;
	}

	@Override
	public void run() 
	{
		NPCommonServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.HTTP.ordinal(), Np2HS_R_Writer_001_HSOP.make_001_001_ReqServerInfoList(_m_lSerial), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2HS_RB_001_001_RetServerInfoList();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                    	CommLog.info("USServerListLoadFromPHPTask dealSucSerial:{}", _m_lSerial);
                    	
                        NP2HS_RB_001_001_RetServerInfoList ret = (NP2HS_RB_001_001_RetServerInfoList) _retProto;
                        USServerListMgrCSInstance.getInstance().callbackUSServerListFromPHPSuc(ret.getSerial(), ret.getServerInfoList());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("USServerListLoadFromPHPTask dealFail errCode:{} Serial:{}", _errCode, _m_lSerial);
                        
                        USServerListMgrCSInstance.getInstance().callbackUSServerListFromPHPFail(_m_lSerial);
                    }
                });
	}
}
