package NPUSServer.Guild.Task;

import Common.GuildEnum.EGuildEventType;
import Common.GuildObj.GuildEvent_ImpeachLeader;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;

public class GuildCheckTask_LeaderImpeach extends _AGuildCheckTask
{
    public GuildCheckTask_LeaderImpeach(GuildInfo _guildInfo)
    {
        super(_guildInfo);
    }

    @Override
    protected void doAction(long _curTimeMs)
    {
        //检查是否已经存在弹劾事件
        if (getGuildInfo().getEventMgr().hasExistEvent(EGuildEventType.IMPEACH_LEADER))
            return;

        //创建弹劾事件
        GuildMemberInfo leader = getGuildInfo().getMemberMgr().getLeader();
        if (leader == null)
            return;

        //是否超过要求时间
        long offlineTimeMs = leader.getOfflineTimeMs();
        if (offlineTimeMs < (long) RefGeneral.Ref().guild_leader_impeach_offline_beyond_hours * 3600 * 1000)
            return;

        //创建弹劾事件
        GuildEvent_ImpeachLeader event = new GuildEvent_ImpeachLeader();
        event.setLeaderCid(leader.getCid());

        getGuildInfo().getEventMgr().createEvent(EGuildEventType.IMPEACH_LEADER, event);
    }
}
