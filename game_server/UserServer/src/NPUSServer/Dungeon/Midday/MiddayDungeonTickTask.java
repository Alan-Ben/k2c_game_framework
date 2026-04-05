package NPUSServer.Dungeon.Midday;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUserServer;

public class MiddayDungeonTickTask implements _IALSynTask
{
    private NPUserServer _m_server;

    public MiddayDungeonTickTask(NPUserServer _server)
    {
        _m_server = _server;
    }

    @Override
    public void run()
    {
        _m_server.getMiddayDungeonMgr().tick1Sec();

        ALSynTaskManager.getInstance().regTask(this, 1000);
    }
}
