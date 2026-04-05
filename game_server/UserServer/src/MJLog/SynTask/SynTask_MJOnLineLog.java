package MJLog.SynTask;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import MJLog.MJLog;
import NPUSServer.NPUserServer;

public class SynTask_MJOnLineLog implements _IALSynTask
{
    private NPUserServer _m_usServer;
    public SynTask_MJOnLineLog(NPUserServer _usServer)
    {
        _m_usServer = _usServer;
    }

    @Override
    public void run()
    {
        if (_m_usServer.isServerReady())
        {
            MJLog.logOnlineNum(_m_usServer, _m_usServer.getUsUserMgr().getAllOnlineUserDataCount(),
                    _m_usServer.getUsUserMgr().getOnlineAndroidUserDataCount(),
                    _m_usServer.getUsUserMgr().getOnlineIOSUserDataCount());
        }

        //每5分钟记录一次
        ALSynTaskManager.getInstance().regTask(this, 5 * 60 * 1000);
    }
}
