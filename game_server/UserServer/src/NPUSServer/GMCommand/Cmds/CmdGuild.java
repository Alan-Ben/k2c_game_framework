package NPUSServer.GMCommand.Cmds;

import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.GuildMgr;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.Guild.Member.GuildMemberMgr;

/**
 * @description: 任务相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "联盟", name = "guild")
public class CmdGuild extends UsCmdBase
{
    @ACommand(comment = "清除cd")
    public String cleanJoinCd()
    {
        return getOwner().getGuildComponent().cleanJoinCd() ? "ok" : "fail";
    }

    @ACommand(comment = "踢出成员[联盟Id][成员角色cid]")
    public String kickMember(long _guildId, long _memberCid)
    {
        // 获取公会管理器
        GuildMgr guildMgr = getUserServer().getGuildMgr();

        // 查找公会
        GuildInfo guildInfo = guildMgr.lookupGuild(_guildId);
        if (guildInfo == null)
            return "guild not found";

        // 检查是否是盟主（盟主不能被踢出）
        if (guildInfo.getLeaderId() == _memberCid)
            return "cannot kick leader";

        // 踢出成员（第三个参数 true 表示是踢出操作）
        Result result = guildMgr.removeMember(guildInfo, _memberCid, true);

        return result.isSucc() ? "ok" : "fail: " + result.getCode();
    }

    @ACommand(comment = "增加免cd次数")
    public String addFreeJoinCd(int _times)
    {
        getOwner().getGuildComponent().addFreeJoinCd(_times);
        return "ok";
    }

    @ACommand(comment = "增加联盟经验")
    public String addGuildExp(int _exp)
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        guildInfo.gainExp(getOwner().getCid(), _exp, getContext());

        return "ok";
    }

    @ACommand(comment = "增加联盟财富")
    public String addGuildWealth(int _wealth)
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        guildInfo.gainWealth(_wealth, getContext());
        return "ok";
    }

    @ACommand(comment = "增加联盟贡献")
    public String addGuildDevote(int _devote)
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        GuildMemberMgr memberMgr = guildInfo.getMemberMgr();
        GuildMemberInfo memberInfo = memberMgr.lookup(getOwner().getCid());
        if (memberInfo == null)
            return "not in guild";

        memberInfo.gainDevote(_devote, getContext());
        return "ok";
    }

    @ACommand(comment = "触发弹劾")
    public String triggerImpeachLeader()
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        GuildMemberMgr memberMgr = guildInfo.getMemberMgr();
        GuildMemberInfo memberInfo = memberMgr.getLeader();
        if (memberInfo == null)
            return "leader not found";

        memberInfo.setOfflineTime(RefGeneral.Ref().guild_leader_impeach_offline_beyond_hours * 3600 * 1000);
        return "ok";
    }

    @ACommand(comment = "触发被动转让")
    public String triggerPositiveTransLeader()
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        GuildMemberMgr memberMgr = guildInfo.getMemberMgr();
        GuildMemberInfo memberInfo = memberMgr.getLeader();
        if (memberInfo == null)
            return "leader not found";

        memberInfo.setOfflineTime(RefGeneral.Ref().guild_leader_trigger_passive_transfer_offline_beyond_hours * 3600 * 1000);
        return "ok";
    }

    @ACommand(comment = "清除公开招募cd")
    public String clearOpenRecruitCd()
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        guildInfo.clearOpenRecruitCd();
        return "ok";
    }

    @ACommand(comment = "增加委托进度[数量]")
    public String addEntrustPoint(int _num)
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        guildInfo.incEntrustPoint(_num);
        return "ok";
    }

    @ACommand(comment = "增加建造进度[数量]")
    public String addConstructPoint(int _num)
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        guildInfo.addConstructPoint(_num);
        return "ok";
    }

    @ACommand(comment = "刷新联盟委托")
    public String refreshEntrust()
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        boolean result = guildInfo.refreshEntrustData();
        return result ? "ok" : "fail";
    }

    @ACommand(comment = "转让盟主[联盟Id][当前盟主cid][新盟主cid]")
    public String transferLeader(long _guildId, long _currentLeaderCid, long _newLeaderCid)
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(_guildId);
        if (guildInfo == null)
            return "guild not found";

        Result result = guildInfo.transferLeaderByGM(_currentLeaderCid, _newLeaderCid, getContext());

        return result.isSucc() ? "ok" : "fail: " + result.getCode();
    }
    
    @ACommand(comment = "获取可以自动帮助的玩家列表")
    public String getAutoPlayerList()
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";
    	
        return guildInfo.getMarsHelpMgr().getAutoHelpPlayerMgr().toString();
    }
}
