package NPHttpServer.Http.HttpService.MsgDispather.Http_002_Op;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Core.NPHSHttpUtil;
import NPHttpServer.Http.Entity.NPEntityServerMailDel;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormServerMailDelDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather.Http_002_Op.Task.SendServerMailDelToUsTask;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHSAllServerMail.NPHSAllServerMailMgr;
import com.google.gson.JsonObject;

import java.util.HashSet;

/**
 * @description: 移除平台全服邮件
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_002_004_PlatFromServerMailDel extends _ANPPlatFormHttpSubDealer<NPEntityServerMailDel>
{
	//发送失败的US集合
	final private HashSet<Integer> _m_hsFailUidSet = new HashSet<>();
	
    @Override
    public int subOrder()
    {
        return 4;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityServerMailDel> getDecoder()
    {
        return NPPlatFormServerMailDelDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityServerMailDel _decodeObj)
    {
        //参数检查
        if (null == _decodeObj || _decodeObj.getUsTypeIdList().isEmpty())
        {
            _commiter.commitFail(CommErr.PARAM_ERROR);
            return;
        }
        
        //移除HS的全服邮件
        NPHSAllServerMailMgr.getInstance().delPHPMail(_decodeObj.getPHPMailId());
        
        //返回成功
        _commiter.commitSuc();

        //构造US推送数据任务
        ALProcess process = ALProcess.CreateProcess("send_revoke_server_mail");
        
        ALProcess[] list = new ALProcess[_decodeObj.getUsTypeIdList().size()];
        for (int i = 0; i < _decodeObj.getUsTypeIdList().size(); i++)
		{
			int usId = _decodeObj.getUsTypeIdList().get(i);
			
			list[i] = ALProcess.CreateProcess("send_revoke_server_mail_sub");
			list[i].addResDelegateProcess(action -> //发起向US推送数据的任务
			{
				//注册推送任务
				ALSynTaskManager.getInstance().regTask(new SendServerMailDelToUsTask(usId, _decodeObj.getPHPMailId(), action));
			}, 
			"send_revoke_server_mail_" + usId,
			()-> //失败处理
			{
				_m_hsFailUidSet.add(usId);
			}, 
			true);
		}

        //构造US推送数据任务
		process.addMultiProcess("send_revoke_server_mail", list);
        
		//开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            //异常终止的事件函数
            @Override
            public void onRootProecssStop()
            {
            }

			@Override
			public void onRootProecssSuc()
			{
				if(_m_hsFailUidSet.size() > 0)
				{
					JsonObject jsonObj = new JsonObject();
					jsonObj.addProperty("failedUsList", CommonFunc.list2String(_m_hsFailUidSet, ','));
					
					NPHSHttpUtil.callbackToPHP("2-4", _commiter.getPHPReqSerial(), 0, "Error US " + _m_hsFailUidSet.size(), jsonObj.toString());
				}
				else
				{
					NPHSHttpUtil.callbackToPHP("2-4", _commiter.getPHPReqSerial(), 0, "success", null);
				}
			}

			@Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
            }
        });
    }
}
