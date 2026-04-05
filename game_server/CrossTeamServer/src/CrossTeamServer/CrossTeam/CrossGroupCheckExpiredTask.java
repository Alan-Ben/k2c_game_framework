package CrossTeamServer.CrossTeam;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

public class CrossGroupCheckExpiredTask implements _IALSynTask
{
    @Override
    public void run()
    {
        CrossDiscardGroupMgr.getInstance().checkExpired();

        //1天执行一次
        ALSynTaskManager.getInstance().regTask(this, 86400000);
    }
}
