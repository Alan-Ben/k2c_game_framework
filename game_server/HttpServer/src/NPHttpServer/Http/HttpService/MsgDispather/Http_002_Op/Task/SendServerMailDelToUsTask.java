package NPHttpServer.Http.HttpService.MsgDispather.Http_002_Op.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALProcess._ITALProcessAction;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_018_ReqSendServerMailDel;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_018_RetSendServerMailDel;
import WCGCommon.Enum.NPEnum.EServerType;

public class SendServerMailDelToUsTask implements _IALSynTask
{
	//目标US
	private int _m_iUsId;
	//全服邮件平台ID
	private long _m_lPHPMailId;
    //process回调
    private _ITALProcessAction<Boolean> _m_action;
    //失败重试次数
    private int _m_iFailCount;
	
	public SendServerMailDelToUsTask(int _usId, long _phpMailId, _ITALProcessAction<Boolean> _action)
	{
		_m_iUsId = _usId;
	
		_m_lPHPMailId = _phpMailId;
		
		_m_action = _action;
	}

	@Override
	public void run() 
	{
		final SendServerMailDelToUsTask task = this;
		
        NP2US_R_003_018_ReqSendServerMailDel proto = new NP2US_R_003_018_ReqSendServerMailDel();
        proto.setPhpMailId(_m_lPHPMailId);
        
        NPHttpServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUsId, proto, new _IWCGCallbackDealer() {
			
			@Override
			public void dealSuc(_IALProtocolStructure _proto) 
			{
				CommLog.info("SendServerMailDelToUsTask DealSuc, US:{} PHP-PHPMailId:{}", _m_iUsId, _m_lPHPMailId);
				
				_m_action.dealAction(true);
			}
			
			@Override
			public void dealFail(int _errCode) 
			{
				_m_iFailCount++;
				
				if(_m_iFailCount > 10)
				{
					CommLog.error("SendServerMailDelToUsTask DealFail, US:{} PHPMailId:{}", _m_iUsId, _m_lPHPMailId);
					
					_m_action.dealAction(true);
				}
				else
				{
					//1秒后进行重试
					ALSynTaskManager.getInstance().regTask(task, 1000);
				}
			}
			
			@Override
			public _IALProtocolStructure createProtocolObj() 
			{
				return new NP2US_RB_003_018_RetSendServerMailDel();
			}
		});
	}
}
