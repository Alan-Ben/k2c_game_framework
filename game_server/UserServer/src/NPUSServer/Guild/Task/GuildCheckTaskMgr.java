package NPUSServer.Guild.Task;

import NPUSServer.Guild.GuildInfo;

import java.util.ArrayList;
import java.util.List;

public class GuildCheckTaskMgr
{
    private GuildInfo _m_guildInfo;
    private List<_AGuildCheckTask> _m_checkTaskList;

    public GuildCheckTaskMgr(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;

        _m_checkTaskList = new ArrayList<>();
        _m_checkTaskList.add(new GuildCheckTask_LeaderPassiveTransfer(_m_guildInfo));
        _m_checkTaskList.add(new GuildCheckTask_LeaderImpeach(_m_guildInfo));
    }

    /**
     * tick
     * @param _nowTimeMs
     */
    public void tick(long _nowTimeMs)
    {
        for (_AGuildCheckTask checkTask : _m_checkTaskList)
        {
            checkTask.doCheck(_nowTimeMs);
        }
    }
}
