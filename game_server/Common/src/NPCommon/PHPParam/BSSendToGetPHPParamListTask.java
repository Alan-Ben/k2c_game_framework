package NPCommon.PHPParam;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import AllRpcData.CS_Service.Common.GetPHPParamList;
import NPCommon.Log.CommLog;
import NPCommon.Util._ABasicServerObj;
import RPC.RpcSender;
import RPC._ARpcCallBack;
import WCGCommon.Enum.NPEnum;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;

public class BSSendToGetPHPParamListTask implements _IALSynTask
{
	//本次任务序列号
	private long _m_lSendSerial;
	//服务器对象
	private _ABasicServerObj _m_soServerObj;
	//失败次数
	private int _m_iFailCount;
	//是否需要重试
	private boolean _m_bNeedRetry;
	
	public BSSendToGetPHPParamListTask(long _sendSerial, _ABasicServerObj _server, boolean _needRetry)
	{
		_m_lSendSerial = _sendSerial;
		
		_m_soServerObj = _server;
		_m_iFailCount = 0;
		_m_bNeedRetry = _needRetry;
	}

	@Override
	public void run() 
	{	
		final BSSendToGetPHPParamListTask task = this;
		
		GetPHPParamList rpc = new GetPHPParamList();
		
		RpcSender sender = new RpcSender(_m_soServerObj, NPEnum.EServerType.SINGLE);
        sender.requestTo(ENPSingleServerType.COMMON.ordinal(), rpc, new _ARpcCallBack<GetPHPParamList>() 
        {
            @Override
            public void call_back(int _errCode, GetPHPParamList _rpc)
            {
            	//已经发起了新的任务，则此任务不再处理
            	if(BSPHPParamMgr.getSendSerial() != _m_lSendSerial)
            		return;
            	
            	if(!_rpc.retObj().getIsSucc()) //失败处理
            	{
					//如果不重试则直接返回
					if(!_m_bNeedRetry)
						return ;

            		if(_m_iFailCount > 10)
            		{
            			CommLog.error("SendToCS Get PHP-ParamList Fail, SendSerial:{} FailCount:{}", _m_lSendSerial, _m_iFailCount);
            			
            			_m_soServerObj.getDDAlert().err("GetPHPParamListFailFromCS", "请求CommonServer的平台参数失败！失败次数：" + _m_iFailCount);
            		}
            		
            		_m_iFailCount++;
            		//1秒后重试
            		ALSynTaskManager.getInstance().regTask(task, 1000);
            	}
            	else
            	{
            		//更新
            		BSPHPParamMgr.getInstance().loadAllData(_rpc.retObj().getSerial(), _rpc.retObj().getPListObj());
            	}
            }
        });
	}
}
