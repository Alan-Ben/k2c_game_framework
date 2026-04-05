package NPUSServer.CommonActivityMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

public class CommonActivityTickTask implements _IALSynTask
{
    private CommonActivityMgr _m_commonActivityMgr;

    public CommonActivityTickTask(CommonActivityMgr _commonActivityMgr)
    {
        _m_commonActivityMgr = _commonActivityMgr;
    }

    @Override
    public void run()
    {
        //tick处理
        _m_commonActivityMgr.tick();

        //0.5秒一次tick
        ALSynTaskManager.getInstance().regTask(this, 500);
    }
}
