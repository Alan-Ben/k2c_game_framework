package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.ErrMain.CommErr;
import NPCommon.Log.CommLog;
import NPHttpServer.Http.Entity.NPEntityMarqueeDel;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormMarqueeDelDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op.Task.SendMarqueeDelToUsTask;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;

public class NP2HS_003_017_PlatFromMarqueeDel extends _ANPPlatFormHttpSubDealer<NPEntityMarqueeDel>
{
    @Override
    public int subOrder()
    {
        return 17;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityMarqueeDel> getDecoder()
    {
        return NPPlatFormMarqueeDelDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityMarqueeDel _decodeObj)
    {
    	//参数检查
        if (null == _decodeObj)
        {
            _commiter.commitFail(CommErr.PARAM_ERROR);
            return;
        }
        
        //提交数据到平台，后续的结果走单独推送
        _commiter.commitSuc();
        
        //构造US推送数据任务
        ALProcess process = ALProcess.CreateProcess("send_marquee_del");
        
        ALProcess[] list = new ALProcess[_decodeObj.getUsIdList().size()];
        for (int i = 0; i < _decodeObj.getUsIdList().size(); i++)
		{
			int usId = _decodeObj.getUsIdList().get(i);
			
			list[i] = ALProcess.CreateProcess("send_marquee_del_sub");
			list[i].addResDelegateProcess(action -> //发起向US推送数据的任务
			{
				CommLog.info("Send Del PHP-Marquee:{} to US-{} Start.", _decodeObj.getPHPId(), usId);
				
				//注册推送任务
				ALSynTaskManager.getInstance().regTask(new SendMarqueeDelToUsTask(usId, _decodeObj.getPHPId(), action));
			}, 
			"send_marquee_del_" + usId);
		}
		process.addMultiProcess("send_marquee_del_list", list);
        
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
			}

			@Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
            }
        });
    }
}
