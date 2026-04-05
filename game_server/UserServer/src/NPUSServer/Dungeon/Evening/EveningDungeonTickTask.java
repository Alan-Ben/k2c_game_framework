package NPUSServer.Dungeon.Evening;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUserServer;

public class EveningDungeonTickTask implements _IALSynTask
{
    private NPUserServer _m_server;

    public EveningDungeonTickTask(NPUserServer _server)
    {
        _m_server = _server;
    }

    @Override
    public void run()
    {
        _m_server.getEveningDungeonMgr().tick1Sec();

        ALSynTaskManager.getInstance().regTask(this, 1000);
    }
}
