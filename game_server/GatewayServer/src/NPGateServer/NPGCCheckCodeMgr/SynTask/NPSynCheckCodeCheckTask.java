package NPGateServer.NPGCCheckCodeMgr.SynTask;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPGateServer.NPGCCheckCodeMgr.NPGCCheckCodeMgr;

public class NPSynCheckCodeCheckTask implements _IALSynTask
{

    @Override
    public void run()
    {
        NPGCCheckCodeMgr.getInstance().checkCheckCodeTime();

        //继续注册本对象
        ALSynTaskManager.getInstance().regTask(this, 1000);
    }

}
