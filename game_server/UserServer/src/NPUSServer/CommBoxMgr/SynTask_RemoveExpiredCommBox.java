package NPUSServer.CommBoxMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

public class SynTask_RemoveExpiredCommBox implements _IALSynTask
{
    private CommBoxMgr _m_cbmCommBoxMgr;

    public SynTask_RemoveExpiredCommBox(CommBoxMgr _cbmCommBoxMgr)
    {
        _m_cbmCommBoxMgr = _cbmCommBoxMgr;
    }

    @Override
    public void run()
    {
        //移除过期宝箱
        _m_cbmCommBoxMgr.removeExpired();

        //5分钟执行一次
        ALSynTaskManager.getInstance().regTask(this, 300000);
    }

}
