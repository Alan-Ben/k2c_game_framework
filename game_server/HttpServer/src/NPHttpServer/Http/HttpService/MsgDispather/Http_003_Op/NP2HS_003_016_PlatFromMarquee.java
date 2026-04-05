package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ServerObj.ServerObj_PHPMarquee;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.ErrMain.CommErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Core.NPHSHttpUtil;
import NPHttpServer.Http.Entity.NPEntityMarqueeAdd;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormMarqueeAddDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op.Task.SendMarqueeToUsTask;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import com.google.gson.JsonObject;

import java.util.Collections;
import java.util.HashSet;
import java.util.Set;

public class NP2HS_003_016_PlatFromMarquee extends _ANPPlatFormHttpSubDealer<NPEntityMarqueeAdd>
{
	//发送失败的US集合
	final private Set<Integer> _m_hsFailUidSet = Collections.synchronizedSet(new HashSet<>());
	
    @Override
    public int subOrder()
    {
        return 16;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityMarqueeAdd> getDecoder()
    {
        return NPPlatFormMarqueeAddDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityMarqueeAdd _decodeObj)
    {
    	//参数检查
        if (null == _decodeObj)
        {
            _commiter.commitFail(CommErr.PARAM_ERROR);
            return;
        }
        
        //提交数据到平台，后续的结果走单独推送
        _commiter.commitSuc();

        //发送US的协议数据
        ServerObj_PHPMarquee phpMarquee = _decodeObj.toProto();
        
        //构造US推送数据任务
        ALProcess process = ALProcess.CreateProcess("send_marquee");
        
        ALProcess[] list = new ALProcess[_decodeObj.getUsIdList().size()];
        for (int i = 0; i < _decodeObj.getUsIdList().size(); i++)
		{
			int usId = _decodeObj.getUsIdList().get(i);
			
			list[i] = ALProcess.CreateProcess("send_marquee_sub");
			list[i].addResDelegateProcess(action -> //发起向US推送数据的任务
			{
				CommLog.info("Send PHP-Marquee:{} to US-{} Start.", phpMarquee.getPhpId(), usId);
				
				//注册推送任务
				ALSynTaskManager.getInstance().regTask(new SendMarqueeToUsTask(usId, phpMarquee, action));
			}, 
			"send_marquee" + usId,
			()-> //失败处理
			{
				_m_hsFailUidSet.add(usId);
				
				CommLog.error("Send PHP-Marquee:{} to US-{} Fail.", phpMarquee.getPhpId(), usId);
			}, 
			true);
		}
		process.addMultiProcess("send_marquee_list", list);
        
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
					
					NPHSHttpUtil.callbackToPHP("3-16", _commiter.getPHPReqSerial(), 0, "Error US " + _m_hsFailUidSet.size(), jsonObj.toString());
				}
				else
				{
					NPHSHttpUtil.callbackToPHP("3-16", _commiter.getPHPReqSerial(), 0, "success", null);
				}
			}

			@Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
            }
        });
    }
}
