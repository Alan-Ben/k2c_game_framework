package NPUSServer.Guild.Task;

import NPUSServer.Guild.GuildInfo;

public abstract class _AGuildCheckTask
{
    private GuildInfo _m_guildInfo;

    public _AGuildCheckTask(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
    }

    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    /**
     * 执行检查
     * @param _curTimeMs
     */
    public void doCheck(long _curTimeMs)
    {
        doAction(_curTimeMs);
    }

    /**
     * 执行操作
     */
    protected abstract void doAction(long _curTimeMs);
}
