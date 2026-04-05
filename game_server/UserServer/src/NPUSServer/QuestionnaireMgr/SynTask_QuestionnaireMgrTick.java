package NPUSServer.QuestionnaireMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Util.CommonFunc;
import NPUSServer.USLog;

public class SynTask_QuestionnaireMgrTick implements _IALSynTask
{
    private QuestionnaireMgr _m_questionnaireMgr;

    public SynTask_QuestionnaireMgrTick(QuestionnaireMgr _questionnaireMgr)
    {
        _m_questionnaireMgr = _questionnaireMgr;
    }

    /**
     * 刷新tick
     */
    public void run()
    {
        //调用刷新处理
        long nowTimeMS = CommonFunc.getNowTimeMS();
        try
        {
            _m_questionnaireMgr.tick(nowTimeMS);
        } catch (Exception e)
        {
            USLog.error(_m_questionnaireMgr.getUserServer(), "SynTask_QuestionnaireMgrTick tick error", e);
        }

        //延迟1秒继续处理
        ALSynTaskManager.getInstance().regTask(this, 1000);
    }
}
