package NPUSServer.GuildMsgDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.NPUserServer;

public class NPUSGuildRequestDispather extends NPRequestDispatcher
{
    private NPUserServer _m_usUSServer;

    public NPUSGuildRequestDispather(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        NPUSGuild_002_RequestDispatcher_InitOp.init(this);
        NPUSGuild_032_RequestDispatcher_GuildOp.init(this);
        NPUSGuild_037_RequestDispatcher_GuildDungeonOp.init(this);
        NPUSGuild_041_RequestDispatcher_GuildExploreOp.init(this);
        NPUSGuild_042_RequestDispatcher_GuildRelatedOp.init(this);
    }

    public NPUserServer getUSServer() { return this._m_usUSServer; }
}
