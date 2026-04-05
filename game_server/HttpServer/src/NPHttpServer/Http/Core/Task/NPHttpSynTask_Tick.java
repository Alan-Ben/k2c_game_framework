package NPHttpServer.Http.Core.Task;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;

/**
 * @description: tick 50 ms
 * @author: ricci
 * @date: 2023-03-23 14:40:26
 */
public class NPHttpSynTask_Tick implements _IALSynTask
{
    private final NPHSHttpServiceCore _m_npHsHttpServiceCore;

    public NPHttpSynTask_Tick(NPHSHttpServiceCore _npHsHttpServiceCore)
    {
        this._m_npHsHttpServiceCore = _npHsHttpServiceCore;
    }

    /**
     * 开始任务
     */
    public void start()
    {
        ALSynTaskManager.getInstance().regTask(this);
    }

    @Override
    public void run()
    {

        if (!_m_npHsHttpServiceCore.isInit())
        {
            CommLog.error("NPHttpSynTask_Tick stop reason:{}", "_m_npHsHttpServiceCore not init");
            return;
        }

        _m_npHsHttpServiceCore.tick();

        //注册下一个任务
        ALSynTaskManager.getInstance().regTask(this, 50);
    }
}
