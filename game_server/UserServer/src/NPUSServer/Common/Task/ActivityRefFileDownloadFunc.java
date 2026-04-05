package NPUSServer.Common.Task;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPGameRes.UsHotRefDataMgr._IActivityRefFileDownloadTask;

public class ActivityRefFileDownloadFunc implements _IActivityRefFileDownloadTask
{
    @Override
    public void downloadFile(String _url, _ICallBackResultT<String> _callback)
    {
        ALSynTaskManager.getInstance().regTask(new ActivityRefFileDownloadTask(_url, 10, _callback));
    }
}
