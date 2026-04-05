package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALProcess._ITALProcessAction;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_017_ReqSendMarqueeDel;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_017_RetSendMarqueeDel;
import WCGCommon.Enum.NPEnum.EServerType;

public class SendMarqueeDelToUsTask implements _IALSynTask
{
	//目标US
	private int _m_iUsId;
	//后台跑马灯ID
	private long _m_lPHPId;
    //process回调
    private _ITALProcessAction<Boolean> _m_action;
    //失败重试次数
    private int _m_iFailCount;
	
	public SendMarqueeDelToUsTask(int _usId, long _phpId, _ITALProcessAction<Boolean> _action)
	{
		_m_iUsId = _usId;
	
		_m_lPHPId = _phpId;
		
		_m_action = _action;
	}

	@Override
	public void run() 
	{
		final SendMarqueeDelToUsTask task = this;
		
        NP2US_R_003_017_ReqSendMarqueeDel proto = new NP2US_R_003_017_ReqSendMarqueeDel();
        proto.setPhpId(_m_lPHPId);
        
        NPHttpServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_iUsId, proto, new _IWCGCallbackDealer() {
			
			@Override
			public void dealSuc(_IALProtocolStructure _proto) 
			{
				CommLog.info("SendMarqueeDelToUsTask DealSuc, US:{} PHP-Marquee:{}", _m_iUsId, _m_lPHPId);
				
				_m_action.dealAction(true);
			}
			
			@Override
			public void dealFail(int _errCode) 
			{
				_m_iFailCount++;
				
				if(_m_iFailCount > 10)
				{
					CommLog.error("SendMarqueeDelToUsTask DealFail, US:{} PHP-Marquee:{}", _m_iUsId, _m_lPHPId);
					
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
				return new NP2US_RB_003_017_RetSendMarqueeDel();
			}
		});
	}
}
