package NPUSServer.Guild.Task;

import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;

public class GuildCheckTask_LeaderPassiveTransfer extends _AGuildCheckTask
{
    public GuildCheckTask_LeaderPassiveTransfer(GuildInfo _guildInfo)
    {
        super(_guildInfo);
    }

    @Override
    protected void doAction(long _curTimeMs)
    {
        GuildMemberInfo leader = getGuildInfo().getMemberMgr().getLeader();
        if (leader == null)
            return;

        //是否超过要求时间
        long offlineTimeMs = leader.getOfflineTimeMs();
        if (offlineTimeMs < (long) RefGeneral.Ref().guild_leader_trigger_passive_transfer_offline_beyond_hours * 3600 * 1000)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_LEADER_PASSIVE_TRANSFER_OFFLINE);
        getGuildInfo().getMemberMgr().passiveTransLeader(leader.getCid(), context);
    }
}
