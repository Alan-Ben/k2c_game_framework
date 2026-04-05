package NPCommonServer.PHPParmMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2HS_RB.p001_HSOp.NP2HS_RB_001_006_RetPlatParamList;
import NPCommon.Log.CommLog;
import NPCommonServer.NPCommonServer;
import NPServerProtocolWriter.NP2HS.Np2HS_R_Writer_001_HSOP;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class CSSendToGetPHPParamListTask implements _IALSynTask
{
	//本次任务序列号
	private long _m_lSendSerial;
	//失败次数
	private int _m_iFailCount;
	
	public CSSendToGetPHPParamListTask(long _sendSerial)
	{
		_m_lSendSerial = _sendSerial;
	}
	
	@Override
	public void run() 
	{
		final CSSendToGetPHPParamListTask task = this;
		
		NPCommonServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.HTTP.ordinal(), Np2HS_R_Writer_001_HSOP.make_001_006_ReqPlatParamList(), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2HS_RB_001_006_RetPlatParamList();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                    	if(_m_lSendSerial == CSPHPParamMgr.getSendSerial())
                    	{
                    		NP2HS_RB_001_006_RetPlatParamList ret = (NP2HS_RB_001_006_RetPlatParamList) _retProto;

                    		CSPHPParamMgr.getInstance().acceptPHPParamList(ret.getDataSerial(), ret.getPhpSerial(), ret.getPListObj());
                    	}
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                    	if(_m_lSendSerial == CSPHPParamMgr.getSendSerial())
                    	{
                    		if(_m_iFailCount > 10)
                    		{
                    			CommLog.error("Send Get PHP-ParamList Fail, SendSerial:{} FailCount:{}", _m_lSendSerial, _m_iFailCount);
                    			
                    			NPCommonServer.getInstance().getDDAlert().err("GetPHPParamListFail", "请求平台参数失败！失败次数：" + _m_iFailCount);
                    		}

                    		_m_iFailCount++;
                    		//1秒后重试
                    		ALSynTaskManager.getInstance().regTask(task, 1000);
                    	}
                    }
                });
	}
}
