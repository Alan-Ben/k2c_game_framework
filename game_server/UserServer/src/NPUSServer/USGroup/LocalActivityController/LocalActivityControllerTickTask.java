package NPUSServer.USGroup.LocalActivityController;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

public class LocalActivityControllerTickTask implements _IALSynTask
{
    private NPUserServer _m_server;

    public LocalActivityControllerTickTask(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void run()
    {
        try
        {
            //tick处理
            getUSServer().getLocalActivityController().tick10Sec();
        } catch (Exception e)
        {
            USLog.error(_m_server, "", e);
        }

        //开启10秒tick
        ALSynTaskManager.getInstance().regTask(this, 10000);
    }
}
